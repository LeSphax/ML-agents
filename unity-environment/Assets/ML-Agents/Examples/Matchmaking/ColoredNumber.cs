using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColoredNumber : MonoBehaviour
{

    private Text text;

    public bool RankBased;


    public int Number
    {
        set
        {
            text.text = value.ToString();

            float maxValue = Matchmaking.MAX_VALUE;
            Color maxColor = Color.green;
            Color minColor = Color.red;
            if (!RankBased)
            {
                maxColor = Color.red;
                minColor = Color.green;
                maxValue = 3000;
            }
            text.color = minColor * (1 - value / maxValue) + maxColor * value / maxValue;

        }
    }

    // Use this for initialization
    void Awake()
    {
        text = GetComponent<Text>();
    }
}
