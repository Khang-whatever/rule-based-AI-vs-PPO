using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class KartAI : MonoBehaviour
{
    public CheckpointManager _checkpointManager;
    private KartController _controller;
    private Checkpoint checkpoint;
    public Transform kartNormal;

    private float time;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<KartController>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;
        if (_checkpointManager == null || _checkpointManager.nextCheckPointToReach == null)
            return;

        checkpoint = _checkpointManager.nextCheckPointToReach;
        if (checkpoint == null)
            return;

        Vector3 target = checkpoint.transform.position;
        target.y = kartNormal.position.y;
        var distance = Vector3.Distance(kartNormal.transform.position, target);
        Vector3 dir = (target - kartNormal.transform.position).normalized;
        float dotProduct = Vector3.Dot(dir, kartNormal.transform.forward);
        var cross = Vector3.Cross(dir, kartNormal.transform.forward);
        float angleDegrees = Vector3.Angle(dir, kartNormal.transform.forward);

        _controller.ApplyAcceleration(1f);
        

        if (cross.y > 0)
        {
            ApplySteer(angleDegrees, -1);
        }
        else if (cross.y < 0)
        {
            ApplySteer(angleDegrees, 1);
        }
    }

    private void ApplySteer(float angle, float a)
    {
        if (angle > 60)
            _controller.Steer(0.99f * a);
        else if (angle > 30)
            _controller.Steer(0.75f * a);
        else if (angle > 0)
            _controller.Steer(0.5f * a);
    }

    public void Reset()
    {
        _checkpointManager.ResetCheckpoints();
        _controller.Respawn();
        Debug.Log(time);
    }

    private void OnDrawGizmos()
    {
        //Handles.color = Color.magenta;
        //Handles.DrawLine(kartNormal.transform.position, checkpoint.transform.position, 5f);
        //Gizmos.DrawLine
    }
}
