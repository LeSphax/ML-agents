using UnityEngine;

public class DeadBody : MonoBehaviour
{
    //hej
    public GameObject explosion;
    private Vector3 Mgoal;
    private Vector2 MDir;
    Rigidbody2D rig;
    public float radie = 0.1f;
    float halfdistance;
    private float mStartSpeed;
    private float mSpeed = 10000;
    private Vector2 mHalfDistancePos;
    public Transform sprite;
    private Vector3 mOriginalScale;
    private bool flying;
    private GGJAgent agent;

    private Vector3 startPosition; //Used to check if we passed the target
    bool rotateLeft;
    float rotationSpeed;

    public float explosionRadius;
    public float diseaseSpreadPower;

    public void SetGoal(GGJAgent agent, Vector3 _goal, float speed)
    {
        flying = true;
        Mgoal = _goal;
        mStartSpeed = speed;
        Vector3 dir = (_goal - gameObject.transform.position);
        MDir = new Vector2(dir.x, dir.y);
        mHalfDistancePos = (Vector2)gameObject.transform.position + (MDir * 0.5f);
        halfdistance = Vector2.Distance(gameObject.transform.position, mHalfDistancePos);
        MDir = MDir.normalized;
        this.agent = agent;
        agent.RegisterFlying(this);
    }

    // Use this for initialization
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        mOriginalScale = sprite.localScale;
        startPosition = transform.position;
        rotationSpeed = Random.Range(-100, 100);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rotationSpeed < 10 && rotationSpeed> -10)
            rotationSpeed = Random.Range(-100, 100);

        //Check if we passed the goal
        if (Vector2.Distance(transform.position, startPosition) > Vector2.Distance(Mgoal, startPosition))
        {
            agent.RegisterExplosion(this, Mgoal);
            Destroy(gameObject);
        }

        rig.velocity = mSpeed * MDir;

      

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        agent.RegisterOutOfBox(this);
        Destroy(gameObject);
    }
}
