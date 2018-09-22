using System.Collections.Generic;
using UnityEngine;

public class NeighboursDetector : MonoBehaviour
{

    private Population population;
    public Population Population
    {
        set
        {
            population = value;
            population.PersonDied += RemoveFromNeighbours;
        }
    }
    public List<Person> list = new List<Person>();

    private void Start()
    {
        //foreach (Collider collider in Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius, 1 << Layers.Person))
        //{
        //    list.Add(collider.GetComponent<Person>());
        //}
    }

    private void RemoveFromNeighbours(Person person)
    {
        list.Remove(person);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == Layers.Person)
    //        list.Add(other.gameObject.GetComponent<Person>());
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.layer == Layers.Person)
    //        list.Remove(other.gameObject.GetComponent<Person>());
    //}
}
