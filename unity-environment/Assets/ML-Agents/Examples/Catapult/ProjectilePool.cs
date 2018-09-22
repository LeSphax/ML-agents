using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CatapultTraining
{
    public delegate void ExplosionHandler(Vector3 position, bool correctArea);

    public class ProjectilePool : MonoBehaviour
    {
        public GameObject projectilePrefab;

        private List<Projectile> idleProjectiles = new List<Projectile>();
        private List<Projectile> movingProjectiles = new List<Projectile>();
        private Projectile[] projectiles;
        public Projectile[] Projectiles
        {
            get
            {
                return projectiles;
            }
        }
        public event ExplosionHandler ExplosionEvent;

        public float NoMovingProjectiles
        {
            get
            {
                return movingProjectiles.Count;
            }
        }

        private void Awake()
        {
            projectilePrefab = Resources.Load<GameObject>("CatapultProjectile");
        }

        public void Init(int poolSize)
        {
            if (projectiles == null || projectiles.Length != poolSize)
            {
                projectiles = new Projectile[poolSize];
                for (int i = 0; i < projectiles.Length; i++)
                {
                    projectiles[i] = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
                    projectiles[i].ExplosionEvent += ProjectileExploded;
                }
            }
            Reset();
        }

        private void Reset()
        {
            idleProjectiles.Clear();
            projectiles.ToList().ForEach(projectile => ResetProjectile(projectile));
        }

        public void Throw(Vector3 direction, float timeToLive, bool correctArea)
        {
            idleProjectiles[0].Throw(direction, timeToLive, correctArea);
            movingProjectiles.Add(idleProjectiles[0]);
            idleProjectiles.RemoveAt(0);
        }

        void ProjectileExploded(Projectile projectile, Vector3 position, bool correctArea)
        {
            ResetProjectile(projectile);
            ExplosionEvent.Invoke(position, correctArea);
        }

        void ResetProjectile(Projectile projectile)
        {
            projectile.Reset(transform.position);
            movingProjectiles.Remove(projectile);
            idleProjectiles.Add(projectile);

        }

    }

}