using UnityEngine;

public class GGJAcademy : Academy
{
    public static float maxAngle;
    public static float minPower;
    public static float maxPower;
    public static int startNoOfDead;
    private Curriculum curriculum;

    public override void InitializeAcademy()
    {
        foreach (Brain brain in GetComponentsInChildren<Brain>())
        {
#if !UNITY_EDITOR
        brain.brainType = BrainType.External;
#else
            if (brain.brainType == BrainType.External)
                brain.brainType = BrainType.Heuristic;
#endif

        }
        Monitor.verticalOffset = 1f;

        //curriculum = Curriculum.GetFromJson();
        //Debug.Log(curriculum.measure);
        //Debug.Log(curriculum.parameters);
        //Debug.Log(curriculum.parameters["min_power"][2]);
    }
    public override void AcademyReset()
    {
        maxAngle = (float)resetParameters["max_angle"];
        minPower = (float)resetParameters["min_power"];
        maxPower = (float)resetParameters["max_power"];
        startNoOfDead = (int)resetParameters["start_no_of_dead"];
    }

    public override void AcademyStep()
    {

    }

    private void Update()
    {
        if(Time.timeScale < 5 && Input.GetKeyDown("r"))
        {
            maxAngle = 160;
            minPower = 0;
            maxPower = 16;
            startNoOfDead = 100;
        }
    }
}
