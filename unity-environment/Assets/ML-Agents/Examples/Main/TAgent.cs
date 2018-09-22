using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAgent : Agent
{



    public override void CollectObservations()
    {
        AddVectorObs(transform.rotation.x);
        AddVectorObs(transform.rotation.y);
        AddVectorObs(transform.rotation.z);
        AddVectorObs(transform.rotation.w);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {

        transform.Rotate(Vector3.forward * Mathf.Clamp(vectorAction[0], -1, 1));
        SetReward(-1/1000f);
        if (transform.eulerAngles.z > TAcademy.minAngle && transform.eulerAngles.z < TAcademy.maxAngle)
        {
            SetReward(1 / 300f);
        }
        if (Time.timeScale < 5)
        {
            Monitor.SetActive(true);
            Monitor.Log("Reward", GetReward()*100, MonitorType.slider, transform);
            Monitor.Log("z", transform.rotation.z, MonitorType.slider, transform);
            Monitor.Log("w", transform.rotation.w, MonitorType.slider, transform);
            Monitor.Log("action10", vectorAction[0]/10, MonitorType.slider, transform);
            Monitor.Log("action100", vectorAction[0]/100, MonitorType.slider, transform);
        }
    }

    public override void AgentReset()
    {
        transform.eulerAngles = Vector3.forward * Random.value * 360;
    }

    public override void AgentOnDone()
    {

    }
}
