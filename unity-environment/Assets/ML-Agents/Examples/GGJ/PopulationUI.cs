using UnityEngine;
using UnityEngine.UI;

public class PopulationUI : MonoBehaviour {

    public int NoOfHealthy
    {
        set
        {
            noOfHealthy.text = value + "";
        }
    }

    public int NoOfSick
    {
        set
        {
            noOfSick.text = value + "";
        }
    }

    public int NoOfDead
    {
        set
        {
            noOfDead.text = value + "";
        }
    }

    public Text noOfHealthy;
    public Text noOfSick;
    public Text noOfDead;

}
