using UnityEngine;
using UnityEngine.UI;

public class PopulationCounter : MonoBehaviour {

    public void SetPopulation(int healthy, int contaminated, int dead)
    {
        GetComponent<Text>().text = "Healthy: " + healthy 
            + System.Environment.NewLine + "Contaminated: " + contaminated
            + System.Environment.NewLine + "Dead bodies: " + dead;
    }
}
