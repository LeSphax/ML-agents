using UnityEngine;

namespace CatapultTraining
{
    public class CatapultAcademy : Academy
    {

        public static float AngleMin;
        public static float AngleMax;
        public static float ExplosionDelay;
        public static int Munitions = 10;

        public static float PowerMin;
        public static float PowerMax;

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
            //Munitions = (int)resetParameters["Munitions"];
            ExplosionDelay = (float)resetParameters["ExplosionDelay"];
            AngleMin = (float)resetParameters["AngleMin"];
            AngleMax = (float)resetParameters["AngleMax"];
            PowerMin = (float)resetParameters["PowerMin"];
            PowerMax = (float)resetParameters["PowerMax"];
        }

        public override void AcademyStep()
        {


        }
    }
}
