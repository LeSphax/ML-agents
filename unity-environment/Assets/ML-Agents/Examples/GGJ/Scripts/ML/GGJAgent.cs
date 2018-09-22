using System.Collections.Generic;
using UnityEngine;

public class GGJAgent : Agent
{
    [Header("Specific to Ball3D")]
    public Population MyPop;
    public Population OtherPop;
    public Catapult Catapult;

    private void Start()
    {
        AgentReset();
    }

    public override void CollectObservations()
    {
        AddVectorObs(Catapult.Rotation / 360);
        AddVectorObs(Catapult.CurrentValue / Catapult.MaxValue);
        AddVectorObs(Catapult.NoOfLoadedBodies / 10);
        AddVectorObs(MyPop.noOfDead / 10);
        AddVectorObs(flyingBodies.Count);
        AddVectorObs(Catapult.leftSide ? 1 : -1);
    }

    private int resetter = 0;
    private int nextReset;
    private List<Vector3> explosions = new List<Vector3>();
    private int noOfOutPlayingArea = 0; // Dead body thrown outside the playing area
    private int previousNoOfDead = 0;

    public bool log;


    public override void AgentAction(float[] vectorAction, string textAction)
    {
        bool[] inputs = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            inputs[i] = vectorAction[i] > 0f && vectorAction[i] <= 1f;
        }
        Catapult.Controls(inputs, Catapult.leftSide ? 1 : 2);

        //SetReward(0.1f);

        //reward -= (previousNoOfDead - MyPop.noOfDead) * 1f/maxStep;
        foreach (Vector2 explosion in explosions)
        {
            if (OtherPop.bounds.Contains(explosion))
            {
                SetReward(0.3f);
            }

            //else
            //reward -= 3f / maxStep;
        }

        //reward -= noOfOutPlayingArea * 3f / maxStep;

        if (Time.timeScale < 5)
        {
            Monitor.SetActive(true);
            Monitor.Log("Reward", GetReward() *10, MonitorType.slider, transform);
            Monitor.Log("Deads", MyPop.noOfDead/10f, MonitorType.slider, transform);
            Monitor.Log("Flying", flyingBodies.Count / 10f, MonitorType.slider, transform);
        }
        
        if(MyPop.noOfDead == 0 && Catapult.NoOfLoadedBodies == 0 && flyingBodies.Count == 0)
        {
            Done();
        }
        previousNoOfDead = MyPop.noOfDead;
        explosions.Clear();
        noOfOutPlayingArea = 0;
    }

    public override void AgentReset()
    {
        MyPop.Reset();
        previousNoOfDead = MyPop.noOfDead;
        Catapult.Reset();
        explosions.Clear();
        flyingBodies.ForEach(body => Destroy(body.gameObject));
        flyingBodies.Clear();
    }

    private List<DeadBody> flyingBodies = new List<DeadBody>();

    internal void RegisterFlying(DeadBody body)
    {
        flyingBodies.Add(body);
    }

    internal void RegisterExplosion(DeadBody body, Vector3 position)
    {
        flyingBodies.Remove(body);
        explosions.Add(position);
    }

    internal void RegisterOutOfBox(DeadBody body)
    {
        flyingBodies.Remove(body);
        noOfOutPlayingArea++;
    }
}
