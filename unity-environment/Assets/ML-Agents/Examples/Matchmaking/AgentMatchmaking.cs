using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AgentMatchmaking : Agent
{
    private const float PUNITION = -0.01f;
    private Matchmaking matchmakingQueue;

    private float lastCumulativeReward;

    private List<Player> PlayersInQueue
    {
        get
        {
            return matchmakingQueue.playersInQueue;
        }
    }

    public override void InitializeAgent()
    {
        matchmakingQueue = gameObject.AddComponent<Matchmaking>();
    }

    public override void CollectObservations()
    {
        for (int i = 0; i < brain.brainParameters.vectorObservationSize / 2; i++)
        {
            if (i < PlayersInQueue.Count)
            {
                AddVectorObs(PlayersInQueue[i].Rank);
                AddVectorObs(PlayersInQueue[i].TimeInQueue / 3000);
            }
            else
            {
                AddVectorObs(-1);
                AddVectorObs(-1);
            }
        }
        //AddVectorObs(PlayersInQueue.Count);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        matchmakingQueue.IncrementTime();

        int firstPlayerIdx = (int)Mathf.Floor(vectorAction[0]);
        int secondPlayerIdx = (int)Mathf.Floor(vectorAction[1]);

        bool correctMatch = true;
        if (firstPlayerIdx >= PlayersInQueue.Count)
        {
            AddReward(PUNITION);
            correctMatch = false;
        }
        if (secondPlayerIdx >= PlayersInQueue.Count)
        {
            AddReward(PUNITION);
            correctMatch = false;
        }

        if(firstPlayerIdx < 0 ^ secondPlayerIdx < 0)
        {
            AddReward(PUNITION);
            correctMatch = false;
        } else if (firstPlayerIdx < 0 && secondPlayerIdx < 0)
        {
            correctMatch = false;
        } else if (secondPlayerIdx == firstPlayerIdx)
        {
            AddReward(PUNITION);
            correctMatch = false;
        }

        if (correctMatch)
        {
            float matchReward = matchmakingQueue.MakeMatch(firstPlayerIdx, secondPlayerIdx);
            AddReward(matchReward);
            noOfMatches++;
            averageMatchReward += (matchReward - averageMatchReward) / noOfMatches;

        }
        lastCumulativeReward = GetCumulativeReward();
        if (matchmakingQueue.myAgentId == 1)
            Debug.Log(vectorAction[0] + "  " + vectorAction[1] + " R: " + GetReward() + matchmakingQueue.StateToString());
    }

    private float averageMatchReward;
    private float noOfMatches;

    public override void AgentReset()
    {
        if (matchmakingQueue.myAgentId < 5)
        {
            Debug.LogWarning("CumulativeReward " + lastCumulativeReward);
            Debug.LogWarning("AverageReward " + averageMatchReward);
        }
        averageMatchReward = 0;
        noOfMatches = 0;
        matchmakingQueue.Reset();
    }

}
