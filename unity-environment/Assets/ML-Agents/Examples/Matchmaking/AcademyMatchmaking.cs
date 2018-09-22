using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcademyMatchmaking : Academy {

    public override void InitializeAcademy()
    {
#if !UNITY_EDITOR
        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
        Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
#endif
        foreach (Brain brain in GetComponentsInChildren<Brain>())
        {
#if !UNITY_EDITOR
        brain.brainType = BrainType.External;
#else
            if (brain.brainType == BrainType.External)
                brain.brainType = BrainType.Heuristic;
#endif

        }
    }

    public override void AcademyReset()
    {
    }

    public override void AcademyStep()
    {


    }
}
