using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public Text id;
    public ColoredNumber rank;
    public ColoredNumber time;

    public Player Player
    {
        set
        {
            id.text = value.Id.ToString();
            rank.Number = value.Rank;
            time.Number = value.TimeInQueue;
        }
    }
}
