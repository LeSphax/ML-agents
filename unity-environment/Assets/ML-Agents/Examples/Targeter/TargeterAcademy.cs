using UnityEngine;

namespace Targeter
{
    public class TargeterAcademy : Academy
    {

        public override void InitializeAcademy()
        {
#if !UNITY_EDITOR
        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
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
}