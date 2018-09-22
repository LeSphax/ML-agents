using UnityEngine;

public class ExplosionScript : MonoBehaviour {


    [SerializeField]
    private float explosionRadius;
    [SerializeField]
    private float diseaseSpreadPower;
    public float lifeTime = 0.1f;

	// Use this for initialization
	void Start () {
        int noOfContaminated = 0;

        var peopleInZone = Physics.OverlapSphere(transform.position, explosionRadius, 1 << Layers.Person);
        foreach (Collider collider in peopleInZone)
        {
            float distanceMultiplicator = (explosionRadius - Vector3.Distance(collider.transform.position, transform.position)) / explosionRadius;
            bool contaminatedTarget = collider.GetComponent<Person>().GetContaminatedBySpread(diseaseSpreadPower * distanceMultiplicator);
            noOfContaminated += contaminatedTarget ? 1 : 0;
            //collider.GetComponent<Person>().Health -= 0.2f * distanceMultiplicator;
        }

        int i = 0;
        while (noOfContaminated < Mathf.Max(1, peopleInZone.Length / 5) && i < peopleInZone.Length)
        {
            var person = peopleInZone[i].GetComponent<Person>();
            if (!person.IsContaminated)
            {
                person.SetContaminated();
                noOfContaminated++;
            }
            i++;
        }
        Destroy(gameObject,lifeTime);
	}


}
