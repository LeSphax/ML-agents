using System;
using UnityEngine;

public class Person : MonoBehaviour
{

    public Population population;
    public SpriteRenderer sprite;
    private float contamination = 0;
    private float health = 1;
    public float Health
    {
        set
        {
            health = value;
        }
        get
        {
            return health;
        }
    }
    public NeighboursDetector neighbours;
    public Move moveScript;

    private float healthLossRate;

    private void Start()
    {
        neighbours.Population = population;
        healthLossRate = population.HealthLossRate / 2 + (UnityEngine.Random.value * population.HealthLossRate * 2);

    }

    public bool IsContaminated
    {
        get
        {
            return contamination == 1;
        }
    }

    public void SetContaminated()
    {
        contamination = 1;
    }

    private void Update()
    {
        //sprite.color = Color.black * (1 - health) + health * new Color(1 - 0.8f * contamination, 1f, 1 - 0.8f * contamination);
    }

    //private void FixedUpdate()
    //{
    //    //moveScript.Speed = 1.5f - contamination;
    //    //if (contamination >= 1)
    //    //{
    //    //    health -= healthLossRate /60;
    //    //    foreach (var n in neighbours.list)
    //    //    {
    //    //        try
    //    //        {
    //    //            n.GetContaminatedBySpread(population.ContaminationRate);
    //    //        }
    //    //        catch (Exception e)
    //    //        {
    //    //            Debug.Log(name);
    //    //            Debug.Log(e.Message);
    //    //        }
    //    //    }
    //    //}
    //    //if (health < 0)
    //    //{
    //    //    population.Kill(this);
    //    //}
    //}

    public bool GetContaminatedBySpread(float chance)
    {
        if (!IsContaminated)
        {
            contamination = UnityEngine.Random.value > (1 - chance) ? 1 : 0;
            return IsContaminated;
        }
        return false;

    }

    public void IncreaseContamination(float amount)
    {
        contamination = Mathf.Min(1, contamination + amount);
    }
}
