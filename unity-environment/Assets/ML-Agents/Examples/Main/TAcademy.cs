using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAcademy : Academy {

    public static float minAngle;
    public static float maxAngle;

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

        minAngle = (float)resetParameters["min_angle"];
        maxAngle = (float)resetParameters["max_angle"];

        Monitor.verticalOffset = 1f;
    }


    public override void AcademyStep()
	{


	}

}
