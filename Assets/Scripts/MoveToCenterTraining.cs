using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToCenterTraining : Agent
{
    [SerializeField] PlayerControl control;
    [SerializeField] Transform center;
    [SerializeField] Transform Base;
    [SerializeField] Transform AIWall;
    [SerializeField] float distance = 10;

    public override void OnEpisodeBegin()
    {
        foreach (Transform t in AIWall)
            t.gameObject.SetActive(true);
        transform.parent.parent.position = new Vector3(-74.0999985f, 9.5f, 725.400024f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.parent.parent.position / 100.0f);
        sensor.AddObservation(center.position / 100.0f);
        sensor.AddObservation(Base.position / 100.0f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0] * distance;
        float moveY = actions.ContinuousActions[1] * distance;

        control.setDestination(transform.parent.parent.position + new Vector3(moveX, 0, moveY));
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousAction = actionsOut.ContinuousActions;
        continuousAction[0] = Input.GetAxisRaw("Horizontal");
        continuousAction[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14) {
            AddReward(5 + 1.0f / Time.realtimeSinceStartup * 10);
            other.gameObject.SetActive(false);
            if (other.name == "Autobot 5") {
                Debug.Log("end");
                EndEpisode();
            }
        }
        if(other.gameObject.layer == 13 && other.name != "SpawnWall") {
            AddReward(-1);
            EndEpisode();
        }
    }
}
