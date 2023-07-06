using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class KartAgent : Agent
{
    private float time = 0;

   public CheckpointManager _checkpointManager;
   private KartController _kartController;
   
   //called once at the start
   public override void Initialize()
   {
      _kartController = GetComponent<KartController>();   
   }
   
   //Called each time it has timed-out or has reached the goal
   public override void OnEpisodeBegin()
   {
      _checkpointManager.ResetCheckpoints();
      _kartController.Respawn();
        Debug.Log(time);
   }

    private void Update()
    {
        time += Time.deltaTime;
    }
    #region Edit this region!

    //Collecting extra Information that isn't picked up by the RaycastSensors
    public override void CollectObservations(VectorSensor sensor)
      {
        Vector3 diff = _checkpointManager.nextCheckPointToReach.transform.position - transform.position;
        sensor.AddObservation(diff / 20f);

        Vector3 tmp;
        if (_checkpointManager.lastCheckpoint == null)
        {
            tmp = _kartController.transform.forward;
        }
        else
        {
            tmp = _checkpointManager.lastCheckpoint.transform.position;
        }
        Vector3 dis = _checkpointManager.nextCheckPointToReach.transform.position - tmp;
        dis.y = 0;
        Vector3 dir = _kartController.transform.forward;
        dir.y = 0;
        if (Vector3.Angle(dis, dir) > 90)
            AddReward(-0.005f);

        AddReward(-0.001f);
      }

      //Processing the actions received
      public override void OnActionReceived(ActionBuffers actions)
      {
        var input = actions.ContinuousActions;

        _kartController.ApplyAcceleration(input[1]);
        _kartController.Steer(input[0]);
      }
      
      //For manual testing with human input, the actionsOut defined here will be sent to OnActionRecieved
      public override void Heuristic(in ActionBuffers actionsOut)
      {
        var action = actionsOut.ContinuousActions;

        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        //action[1] = Input.GetKey(KeyCode.W) ? 1f : 0f;
      }
      
   #endregion
}
