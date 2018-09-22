using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Targeter
{
    public delegate void ProjectileLandingHandler(Vector2 location);

    public class AimingAgent : Agent
    {
        private const float INCREMENT = 0.1f;
        private const float RADIUS = 10f;

        public GameObject targetPrefab;
        private GameObject targetGameObject;

        private static int nextId = 0;
        private int agentId;

        private float power;

        private bool hasTarget = false;
        private Vector2 _target;
        public Vector2 Target
        {
            set
            {
                _target = value;
                hasTarget = true;
                targetGameObject.transform.position = (Vector2)transform.position + (_target/RADIUS);
            }
        }

        public event ProjectileLandingHandler ProjectileLanded;

        private float lastReward;

        private bool usePower = false;

        public override void InitializeAgent()
        {
            agentId = nextId;
            nextId++;
            targetGameObject = Instantiate(targetPrefab);
        }

        public override void CollectObservations()
        {
            //AddVectorObs(new Vector2(gameObject.transform.rotation.w, gameObject.transform.rotation.z));
            AddVectorObs((Vector2)transform.right);
            AddVectorObs(_target);
            AddVectorObs(power);
        }

        public override void AgentAction(float[] vectorAction, string textAction)
        {
            bool action_throw = false;
            if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
            {
                float action_z = 2f * Mathf.Clamp(vectorAction[0], -1f, 1f);
                action_throw = vectorAction[1] > 0f;
                gameObject.transform.Rotate(new Vector3(0, 0, 1), action_z);
                //SetReward(-0.001f);
            } else if (brain.brainParameters.vectorActionSpaceType == SpaceType.discrete) {
                int action = Mathf.FloorToInt(vectorAction[0]);

                float rotate = 0f;

                // Goalies and Strikers have slightly different action spaces.
                switch (action)
                {
                    case 0:
                        rotate = -2f;
                        break;
                    case 1:
                        rotate = 2f;
                        break;
                    case 2:
                        action_throw = true;
                        break;
                }
                gameObject.transform.Rotate(new Vector3(0, 0, 1), rotate);
            }
            if (action_throw)
            {
                power += INCREMENT;
            }
            if ((!action_throw && power >= 0f) || power > 2f)
            {

                Vector2 result = transform.right * RADIUS;
                if (usePower) result *= power;

                float reward = (RADIUS * 2  - Vector2.Distance(result, _target)) / (2 * RADIUS);
                if (reward < 0) reward = 0;
                SetReward(reward * reward);
                Done();

                if (agentId == 0)
                {
                    Debug.Log("Power : " + power + " vs " + _target.magnitude);
                    Debug.Log("Result: " + result + " vs " + _target + " vs " + transform.right);
                    Debug.Log(GetReward());
                }

                lastReward = GetReward();
            }

            if (Time.timeScale < 5f)
            {
                Monitor.SetActive(true);
                Monitor.Log("LastReward", lastReward);
                Monitor.Log("Reward", GetReward());
                Monitor.Log("RotationZ", gameObject.transform.rotation.z);
                Monitor.Log("RotationW", gameObject.transform.rotation.w);
            }


        }

        public override void AgentReset()
        {

            hasTarget = false;
            float angle = Random.value * 2 * Mathf.PI;

            Target = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * RADIUS;

            power = -INCREMENT;
            gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            gameObject.transform.Rotate(new Vector3(0, 0, 1), UnityEngine.Random.Range(0f, 360f));
        }
    }
}