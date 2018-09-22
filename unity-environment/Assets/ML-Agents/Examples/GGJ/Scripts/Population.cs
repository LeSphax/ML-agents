using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PersonHandler(Person person);
public delegate void VoidHandler();

public class Population : MonoBehaviour
{

    public event PersonHandler PersonDied;
    public event VoidHandler Resetted;
    public float MovementChangeRate;

    public float RespawnRate;
    public float ContaminationRate;
    public float HealthLossRate;

    public int numberOfPeople;
    public GameObject personPrefab;

    private List<Person> persons = new List<Person>();
    public int noOfDead;
    public PopulationUI UI;
    public Bounds bounds;



    // Use this for initialization
    void Start()
    {
        bounds.center = transform.position;
        Reset();
    }

    public void Reset()
    {
        noOfDead = GGJAcademy.startNoOfDead;
        persons.ForEach(person => { Destroy(person.gameObject); });
        persons.Clear();

        for (int i = 0; i < numberOfPeople; i++)
        {
            CreatePerson("Person " + i);
        }

        //Invoke("Spawn", RespawnRate * persons.Count / 100);
        if (Resetted != null)
            Resetted.Invoke();
    }

    void Spawn()
    {
        CreatePerson("Spawned " + persons.Count);
        Invoke("Spawn", RespawnRate * persons.Count / 100 * UnityEngine.Random.value);
    }

    void CreateContaminated()
    {
        Person contaminatedPerson = CreatePerson("Contaminated person " + persons.Count);
        contaminatedPerson.IncreaseContamination(1);
        contaminatedPerson.Health = UnityEngine.Random.value;
    }

    internal void Kill(Person person)
    {
        Destroy(person.gameObject);
        if (person.IsContaminated)
        {
            noOfDead++;
        }
        persons.Remove(person);
        if (PersonDied != null)
            PersonDied.Invoke(person);
    }

    Person CreatePerson(string name)
    {
        GameObject personGO = Instantiate(
            personPrefab,
            GetRandomPositionInBounds(),
            Quaternion.identity
        );
        personGO.name = name;
        personGO.transform.SetParent(transform, true);
        personGO.GetComponent<Move>().parent = this;
        Person person = personGO.GetComponent<Person>();
        person.population = this;
        persons.Add(person);
        return person;
    }

    public bool UseDeadBody()
    {
        if (noOfDead > 0)
        {
            noOfDead--;
            return true;
        }
        return false;
    }

    public int contaminated = 0;

    private void Update()
    {

        if (Input.GetKey("left ctrl") && Input.GetKeyDown("r"))
        {
            Reset();
        }
        int healthy = 0;
        contaminated = 0;
        foreach (Person person in persons)
        {
            if (person.IsContaminated)
            {
                contaminated++;
            }
            else
            {
                healthy++;
            }
        }
        //UI.NoOfHealthy = healthy;
        //UI.NoOfSick = contaminated;
        //UI.NoOfDead = noOfDead;
    }

    public Vector3 GetRandomPositionInBounds()
    {
        return Utils.GetRandomPositionInSquare(bounds.min.x, bounds.max.x, bounds.min.y, bounds.max.y);
    }

}
