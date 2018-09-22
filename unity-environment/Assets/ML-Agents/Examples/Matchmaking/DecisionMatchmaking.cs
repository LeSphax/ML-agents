using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DecisionMatchmaking : MonoBehaviour, Decision
{
    public float[] Decide(List<float> vectorObs, List<Texture2D> visualObs, float reward, bool done, List<float> memory)
    {
        List<float> nonEmpties = vectorObs.Where((_, i) => i % 2 == 0).Where(rank => rank >= 0).ToList();

        if (nonEmpties.Count == Matchmaking.BATCH_MAX_SIZE)
        {
            //Debug.Log(string.Join(", ", nonEmpties.Select(p => p.ToString())));
            List<float> differences = new List<float>();
            for (int i = 0; i < Matchmaking.BATCH_MAX_SIZE - 1; i++)
            {
                differences.Add(nonEmpties[i + 1] - nonEmpties[i]);
            }
            int index = differences.IndexOf(differences.Min());
            return new float[] { index, index + 1 };
        }
        else
        {
            return new float[] { -1, -1 };
        }
    }

    public List<float> MakeMemory(List<float> vectorObs, List<Texture2D> visualObs, float reward, bool done, List<float> memory)
    {
        return new List<float>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
