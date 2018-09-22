using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Matchmaking : MonoBehaviour
{

    public const int MAX_VALUE = 50;
    public const int BATCH_MAX_SIZE = 10;

    private GameObject batchPanel;
    private GameObject resultPanel;
    private GameObject playerPrefab;
    private GameObject matchPrefab;

    private float lastShownTime;
    private bool showExample;

    private static int _playerId = 0;
    private static int PlayerId
    {
        get
        {
            _playerId++;
            return _playerId;
        }
    }

    public int myAgentId;
    private static int _agentId = 0;
    private static int AgentId
    {
        get
        {
            _agentId++;
            return _agentId;
        }
    }

    public List<Player> playersInQueue = new List<Player>();
    public LinkedList<Match> lastMatches = new LinkedList<Match>();

    private void Awake()
    {
        batchPanel = GameObject.Find("BatchPanel");
        resultPanel = GameObject.Find("ResultPanel");
        lastShownTime = Time.realtimeSinceStartup;
        playerPrefab = Resources.Load<GameObject>("MMPlayer");
        matchPrefab = Resources.Load<GameObject>("Match");
        myAgentId = AgentId;
    }

    private void FixedUpdate()
    {
        if (UnityEngine.Random.value > 0.99f && playersInQueue.Count < BATCH_MAX_SIZE)
        {
            //Debug.Log("Add player");
            Player player = new Player(PlayerId, (int)Mathf.Floor(UnityEngine.Random.Range(0, MAX_VALUE + 1)));
            playersInQueue.Add(player);
            playersInQueue.Sort();
            ResetVisuals();
        }
    }

    public void IncrementTime()
    {
        playersInQueue.ForEach(p => p.IncrementTimeInQueue());
        if(Time.timeScale < 5)
        {
            ResetVisuals();
        }
    }

    public float MakeMatch(int firstPlayerIdx, int secondPlayerIdx)
    {
        Player player1 = playersInQueue[firstPlayerIdx];
        Player player2 = playersInQueue[secondPlayerIdx];

        Match match = new Match(player1, player2);
        float reward = match.GetReward();

        if (myAgentId == 1)
        {
            Debug.Log("Match " + player1.Rank + " vs " + player2.Rank + " R: " + reward + StateToString());
        }

        lastMatches.AddFirst(match);
        playersInQueue.Remove(player1);
        playersInQueue.Remove(player2);
        ResetVisuals();

        return reward;
    }

    public string StateToString()
    {
        return " State: " + string.Join(", ", playersInQueue.Select(player => player.Rank.ToString()));
            //+ "    " + string.Join(", ", playersInQueue.Select(player => player.TimeInQueue.ToString())) 
            //+ "      " + string.Join(", ", playersInQueue.Select(player => player.QueuePenalty().ToString()));
    }

    public void Reset()
    {
        playersInQueue.Clear();
        lastMatches.Clear();
    }

    public void ResetVisuals()
    {
        if (myAgentId == 1)
        {
            ClearAndFillPanel(playersInQueue, batchPanel);
            foreach (Transform child in resultPanel.transform)
            {
                Destroy(child.gameObject);
            }
            lastMatches.Take(5).ToList().ForEach(match =>
            {
                GameObject matchContainer = Instantiate(matchPrefab, resultPanel.transform);
                Instantiate(playerPrefab, matchContainer.transform).GetComponent<PlayerUI>().Player = match.firstPlayer;
                Instantiate(playerPrefab, matchContainer.transform).GetComponent<PlayerUI>().Player = match.secondPlayer;
            });
        }
    }

    private void ClearAndFillPanel(List<Player> players, GameObject panel)
    {
        foreach (Transform child in panel.transform)
        {
            Destroy(child.gameObject);
        }
        players.ForEach(player => Instantiate(playerPrefab, panel.transform)
                .GetComponent<PlayerUI>().Player = player);
    }
}

public class Player : IComparable
{
    public int Id;
    public int Rank;
    public int TimeInQueue;

    public Player(int id, int rank)
    {
        Id = id;
        Rank = rank;
        TimeInQueue = 0;
    }

    public void IncrementTimeInQueue()
    {
        TimeInQueue += 1;
    }

    public float QueuePenalty()
    {
        return Mathf.Pow(TimeInQueue / 3000.0f, 1);
    }

    public int CompareTo(object obj)
    {
        return Rank.CompareTo(((Player)obj).Rank);
    }
}

public class Match
{
    public Player firstPlayer;
    public Player secondPlayer;

    public Match(Player firstPlayer, Player secondPlayer)
    {
        this.firstPlayer = firstPlayer;
        this.secondPlayer = secondPlayer;
    }

    public float GetReward()
    {
        return Mathf.Pow(1 - (Mathf.Abs(firstPlayer.Rank - secondPlayer.Rank) / (float)Matchmaking.MAX_VALUE), 3) - firstPlayer.QueuePenalty() - secondPlayer.QueuePenalty();
    }
}