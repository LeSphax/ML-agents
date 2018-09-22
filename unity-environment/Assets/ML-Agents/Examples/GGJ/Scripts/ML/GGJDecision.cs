using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGJDecision : MonoBehaviour, Decision
{

    public float[] Decide(
        List<float> vectorObs,
        List<Texture2D> visualObs,
        float reward,
        bool done,
        List<float> memory)
    {
        if (gameObject.GetComponent<Brain>().brainParameters.vectorActionSpaceType
            == SpaceType.continuous)
        {
            List<float> ret = new List<float>();
            ret.Add(Random.value * 2);
            ret.Add(Random.value * 2);
            ret.Add(Random.value * 2);
            ret.Add(Random.value * 2);
            return ret.ToArray();
        }

        // If the vector action space type is discrete, then we don't do anything.     
        return new float[1] { 1f };
    }

    public List<float> MakeMemory(
        List<float> vectorObs,
        List<Texture2D> visualObs,
        float reward,
        bool done,
        List<float> memory)
    {
        return new List<float>();
    }
}
