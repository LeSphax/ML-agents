using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Targeter
{
    public class MasterAgent : Agent
    {
        private const int MAP_RESOLUTION = 10;
        private const int MEMORY_SIZE = 10;
        public AimingAgent aimingAgent;

        private LinkedList<Vector2> latestLandings = new LinkedList<Vector2>();

        public override void InitializeAgent()
        {
            for(int i=0; i< MEMORY_SIZE; i++)
            {
                latestLandings.AddFirst(new Vector2(-1, -1));
            }
            aimingAgent.ProjectileLanded += ProjectileLanded;
        }

        public override void CollectObservations()
        {
            var tempList = latestLandings;
            for (int i = 0; i < MEMORY_SIZE; i++)
            {
                AddVectorObs(tempList.First.Value);
                tempList.RemoveFirst();
            }
        }

        public override void AgentAction(float[] vectorAction, string textAction)
        {
            aimingAgent.Target = new Vector2(vectorAction[0], vectorAction[1]);


            //SetReward()
        }

        private bool[,] GetHitZones()
        {
            bool[,] map = new bool[MAP_RESOLUTION, MAP_RESOLUTION];
            for (int i = 0; i < MEMORY_SIZE; i++)
            {
                //AddVectorObs(tempList.First.Value);
                //tempList.RemoveFirst();
            }
            return map;
        }

        public override void AgentReset()
        {
           
        }

        private void ProjectileLanded(Vector2 location)
        {
            latestLandings.AddFirst(location);
            if(latestLandings.Count > MEMORY_SIZE)
            {
                latestLandings.RemoveLast();
            }
        }

    }
}