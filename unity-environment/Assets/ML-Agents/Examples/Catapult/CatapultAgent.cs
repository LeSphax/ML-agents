using System.Linq;
using UnityEngine;

namespace CatapultTraining
{
    public class CatapultAgent : Agent
    {
        private static int nextId = 0;
        private int agentId;
        private float power;
        private ProjectilePool projectilePool;
        private int munitions;

        private Vector3 target;

        private string[] shootResults;

        public override void InitializeAgent()
        {
            agentId = nextId;
            nextId++;
            projectilePool = gameObject.AddComponent<ProjectilePool>();
            projectilePool.ExplosionEvent += ProjectileExploded;
        }

        public override void CollectObservations()
        {
            AddVectorObs(new Vector2(gameObject.transform.rotation.w, gameObject.transform.rotation.z));
            projectilePool.Projectiles.ToList().ForEach(projectile =>
            {
                AddVectorObs(new Vector2(projectile.transform.position.x - transform.position.x, projectile.transform.position.y - transform.position.y));
                AddVectorObs(projectile.transform.GetComponent<Rigidbody2D>().velocity);
            });

            AddVectorObs(power);
            AddVectorObs(munitions / CatapultAcademy.Munitions);
        }

        public override void AgentAction(float[] vectorAction, string textAction)
        {
            bool action_throw = false;
            if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
            {
                float action_z = 2f * Mathf.Clamp(vectorAction[0], -1f, 1f);
                action_throw = vectorAction[1] > 0f;
                gameObject.transform.Rotate(new Vector3(0, 0, 1), action_z);
                SetReward(-0.001f);
            }
            if (action_throw)
            {
                if (munitions > 0)
                    power += 0.01f;
            }
            else if (power > 0f)
            {

                float angle = gameObject.transform.rotation.eulerAngles.z;
                bool correctAngle = angle > CatapultAcademy.AngleMin && angle < CatapultAcademy.AngleMax;
                bool correctPower = power > CatapultAcademy.PowerMin && power < CatapultAcademy.PowerMax;

                int logIndex = CatapultAcademy.Munitions - munitions;

                shootResults[logIndex] = "";
                if (!correctPower)
                {
                    shootResults[logIndex] += "P";
                }
                if (!correctAngle)
                {
                    shootResults[logIndex] += "A";
                }
                shootResults[logIndex] += "(" + Mathf.Floor(angle) + "," + power + ")";

                projectilePool.Throw(transform.right, CatapultAcademy.ExplosionDelay, correctAngle && correctPower);

                munitions--;
                ResetPower();
            }

            if (Time.timeScale < 5f)
            {
                Monitor.SetActive(true);
                Monitor.Log("Reward", GetReward());
                Monitor.Log("RotationZ", gameObject.transform.rotation.z);
                Monitor.Log("RotationX", gameObject.transform.rotation.eulerAngles.z);
                Monitor.Log("RotationW", gameObject.transform.rotation.w);
                Monitor.Log("CumulativeReward", GetCumulativeReward());
            }


        }

        void ProjectileExploded(Vector3 position, bool correctArea)
        {
            if (correctArea)
            {
                SetReward(1f);
            }
            if (munitions == 0 && projectilePool.NoMovingProjectiles == 0)
            {
                Done();
            }
        }

        private void ResetPower()
        {
            power = 0f;
        }

        public override void AgentReset()
        {
            if (shootResults != null && agentId == 0)
            {
                Debug.Log(string.Join(",", shootResults));
                Debug.Log(cumulativeReward);
            }

            projectilePool.Init(CatapultAcademy.Munitions);
            shootResults = new string[CatapultAcademy.Munitions];
            munitions = CatapultAcademy.Munitions;
            ResetPower();
            gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            gameObject.transform.Rotate(new Vector3(0, 0, 1), UnityEngine.Random.Range(0f, 360f));
        }
    }
}
