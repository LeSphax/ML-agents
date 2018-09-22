using UnityEngine;


namespace Targeter
{
    public delegate void ProjectileExplosionHandler(Projectile projectile, Vector3 position, bool correctArea);

    public class Projectile : MonoBehaviour
    {

        private float timeToLive;
        private bool correctArea;
        public event ProjectileExplosionHandler ExplosionEvent;

        public void Throw(Vector3 direction, float timeToLive, bool correctArea)
        {
            GetComponent<Rigidbody2D>().velocity = direction;
            this.correctArea = correctArea;
            this.timeToLive = timeToLive;
            Invoke("Explode", timeToLive);
        }

        void Explode()
        {
            ExplosionEvent.Invoke(this, transform.position, correctArea);
        }

        public void Reset(Vector3 position)
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            transform.position = position;
            CancelInvoke("Explode");
        }
    }
}