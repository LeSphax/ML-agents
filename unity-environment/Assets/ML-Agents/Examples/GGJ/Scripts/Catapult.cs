using UnityEngine.UI;
using UnityEngine;

public class Catapult : MonoBehaviour
{

    private GGJAgent agent;
    [SerializeField]
    private Population population;

    [SerializeField]
    private float turnSpeed = 1.5f;

    [SerializeField]
    private float maxAngle;

    [SerializeField]
    private float maxProjectileSpeed;
    [SerializeField]
    private float valueSpeed;   //The speed the UI element moves
    [SerializeField]
    public bool leftSide = true;   //Is this catapult located on the right side?

    public float Rotation
    {
        get
        {
            return catapultTransform.rotation.eulerAngles.z;
        }
    }
    [SerializeField]
    private Text noOfBodiesLabel;
    private int _noOfLoadedBodies = 1;
    public int NoOfLoadedBodies
    {
        get
        {
            return _noOfLoadedBodies;
        }
        set
        {
            _noOfLoadedBodies = value;
            noOfBodiesLabel.text = value + "";
        }
    }
    private bool loaded
    {
        get
        {
            return NoOfLoadedBodies > 0;
        }
    }


    [SerializeField]
    private GameObject bodyPrefab;  //A reference to the body we want to instantiate!

    [SerializeField]
    private Transform catapultTransform;    //The transform for the catapult sprite

    [SerializeField]
    private GameObject displayBody; //The body to be displayed on the catapult (get enabled and disabled, visual representation only!)

    [SerializeField]
    private Slider valueSlider; //Reference for the value slider connected to this catapult
    public float CurrentValue
    {
        get
        {
            return valueSlider.value;
        }
    }
    public float MaxValue
    {
        get
        {
            return valueSlider.maxValue;
        }
    }
    [SerializeField]
    private bool humanControlled;

    private void Start()
    {
        Reset();
    }

    private float ClampRotation(float rotation)
    {

        rotation -= (!leftSide ? 180 : 0);
        if (rotation > 180) rotation = rotation - 360;
        rotation = Mathf.Clamp(rotation, -GGJAcademy.maxAngle, GGJAcademy.maxAngle);
        if (rotation < 0) rotation = 360 + rotation;

        rotation += (!leftSide ? 180 : 0);
        if (rotation > 360)
            rotation -= 360;
        return rotation;
    }

    public void Reset()
    {
        NoOfLoadedBodies = 0;
        if (leftSide)
            catapultTransform.rotation = Quaternion.Euler(0, 0, Random.Range(-GGJAcademy.maxAngle, GGJAcademy.maxAngle));
        else
            catapultTransform.rotation = Quaternion.Euler(0, 0, Random.Range(180 - GGJAcademy.maxAngle, 180 + GGJAcademy.maxAngle));

        valueSlider.gameObject.SetActive(true);
        agent = GetComponent<GGJAgent>();
    }

    void Update()
    {
        if (humanControlled)
        {
            if (leftSide)
            {
                bool[] inputs = SetInputs("w", "s", "Left Horizontal");
                Controls(inputs, 1);
            }
            else
            {
                bool[] inputs = SetInputs("up", "down", "Right Horizontal");
                Controls(inputs, 2);
            }
        }
    }

    public bool[] SetInputs(string shootKey, string loadKey, string turnAxis)
    {
        bool[] inputs = new bool[4];
        if (Input.GetKey(shootKey))
        {
            inputs[0] = true;
        }
        if (Input.GetKey(loadKey))
        {
            inputs[1] = true;
        }
        if (Input.GetAxis(turnAxis) > 0)
        {
            inputs[2] = true;
        }
        if (Input.GetAxis(turnAxis) < 0)
        {
            inputs[3] = true;
        }
        return inputs;
    }


    bool shooting = false;
    bool previousShooting = false;


    public void Controls(bool[] inputs, int catapultId)
    {
        if (inputs[0])
        {
            shooting = !shooting;
        }

        if (inputs[0] && loaded)
        {
            shooting = true;
            valueSlider.value += valueSpeed / 60;
            if (valueSlider.value >= valueSlider.maxValue)
            {
                valueSlider.value = valueSlider.maxValue;
                FireCatapult(catapultId, valueSlider.value / valueSlider.maxValue);
            }
        }

        if (!inputs[0] && shooting && loaded)
        {
            FireCatapult(catapultId, valueSlider.value / valueSlider.maxValue);
        }
        previousShooting = shooting;

        if (inputs[1])
        {
            LoadCatapult(catapultId);
        }

        //Rotation
        int left = inputs[2] ? 0 : 1;
        int right = inputs[3] ? 0 : -1;
        float h = left + right;

        if (h != 0)
        {
            catapultTransform.Rotate(0, 0, -turnSpeed * h / 60);
            catapultTransform.rotation = Quaternion.Euler(0, 0, ClampRotation(Rotation));
        }
    }

    private void FireCatapult(int catapultId, float powerProportion)
    {
        shooting = false;
        CalculateAndFire(catapultId, powerProportion);
        NoOfLoadedBodies = 0;
    }

    void LoadCatapult(int id)
    {
        if (population.UseDeadBody())
        {
            //AudioManager.instance.PlayReloadSound(id);
            NoOfLoadedBodies++;
            if (!displayBody.activeSelf)
                displayBody.SetActive(true);

            //Enable slider ui     
        }
    }

    void CalculateAndFire(int id, float powerProportion)
    {
        // AudioManager.instance.StopChargeSound(id);

        valueSlider.value = valueSlider.minValue;

        Vector2 impactPosition = Vector2.zero;

        float power = GGJAcademy.minPower + powerProportion * (GGJAcademy.maxPower - GGJAcademy.minPower);

        impactPosition = catapultTransform.position + catapultTransform.right * power;
        //Disable slider ui     
        //valueSlider.gameObject.SetActive(false);

        //Play Animation

        //Disable the visual representation on the catapult
        if (displayBody.activeSelf)
            displayBody.SetActive(false);

        for (int i = 0; i < NoOfLoadedBodies; i++)
        {
            float spread = 0;// Mathf.Min(0.8f * (NoOfLoadedBodies-1)/0.5f, 20);
            SendDeadBody(id, impactPosition + Random.insideUnitCircle * spread, power);
        }

        //Reset variables
        NoOfLoadedBodies = 0;
    }

    private void SendDeadBody(int id, Vector2 impactPosition, float power)
    {
        GameObject body = Instantiate(bodyPrefab, catapultTransform.position, catapultTransform.rotation);     //change spawn position? To the "basket" position in animation?

        //AudioManager.instance.PlayShootSound(id);
        //Send impactPosition to body.
        power = Mathf.Clamp(power, 0, maxProjectileSpeed);
        body.GetComponent<DeadBody>().SetGoal(agent, impactPosition, power);
    }
}
