using Google.Protobuf.WellKnownTypes;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.Sentis.Layers;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class rollerAgent : Agent
{


    Rigidbody rBody;
    Rigidbody targetRbody;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        targetRbody = target.GetComponent<Rigidbody>();
    }

    public GameObject target;
    public GameObject aimObject;
    public override void OnEpisodeBegin()
    {

        target.transform.localPosition = new Vector3(Random.Range(-2, 2), 0.5f, Random.Range(-2, 2));
        aimObject.transform.localPosition = new Vector3(Random.Range(-2, 2), 0.5f, Random.Range(-2, 2));
        targetRbody.angularVelocity = Vector3.zero;
        targetRbody.velocity = Vector3.zero;
        aimObject.transform.localPosition = new Vector3(0, 0.5f, 0);

        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(Random.Range(-2, 2), 0.5f, Random.Range(-2, 2));
        //            target.transform.localPosition = new Vector3(0, 0.5f, 0);

        // If the Agent fell, zero its momentum
        if (this.transform.localPosition.y < 0 )
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(Random.Range(-2, 2), 0.5f, Random.Range(-2, 2));
        }


        // If the Agent fell, zero its momentum
        if (target.transform.localPosition.y < 0)
        {
            targetRbody.angularVelocity = Vector3.zero;
            targetRbody.velocity = Vector3.zero;
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(Random.Range(-2, 2), 0.5f, Random.Range(-2, 2));
            //            target.transform.localPosition = new Vector3(0, 0.5f, 0);
        }


    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(target.transform.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public float forceMultiplier = 5.0f;
    float speed = 10.0f;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {

        //transform.Rotate(0,angleToRotate,0, Space.World);
        //Vector3 controlSignal = Vector3.zero;
        //controlSignal.x = actionBuffers.ContinuousActions[0];
        //controlSignal.z = actionBuffers.ContinuousActions[1];
        //float forwardForce = actionBuffers.ContinuousActions[1];
        //Debug.Log(forwardForce);
        
        aimObject.transform.localPosition += new Vector3(speed * Time.deltaTime * actionBuffers.ContinuousActions[0], 0, speed * Time.deltaTime * actionBuffers.ContinuousActions[1]);

        aimObject.transform.localPosition = new Vector3(Mathf.Clamp(aimObject.transform.localPosition.x, -4f, 4f), aimObject.transform.localPosition.y, Mathf.Clamp(aimObject.transform.localPosition.z, -4f, 4f));


        Vector3 forceDir = aimObject.transform.localPosition - this.transform.localPosition;
//        Debug.DrawRay(transform.position, forceDir, Color.green);
        if (actionBuffers.DiscreteActions[0]>0) { 
            rBody.AddForce(forceDir * forceMultiplier);
        }
        // positive reward for pushing the target down.
        if (target.transform.localPosition.y < 0)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // Fell off platform
        else if (this.transform.localPosition.y < 0)
        {
          //  SetReward(-1.0f);
            EndEpisode();
        }
    }



    public override void Heuristic(in ActionBuffers actionsOut)
    {

        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");

        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.Space)) { discreteActionsOut[0] = 1; } else { discreteActionsOut[0] = 0; }

    }
}
