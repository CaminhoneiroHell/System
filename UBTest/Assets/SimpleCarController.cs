using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Axle {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}
     
public class SimpleCarController : MonoBehaviour {

    public float breakForce = 30000;
    public List<Axle> axleInfos; 
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float breakInfo_RW, breakInfo_LW;

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) {
            return;
        }
     
        Transform visualWheel = collider.transform.GetChild(0);
     
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
     
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
     
    public void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        //breakInfo
        foreach (Axle axleInfo in axleInfos) {
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor){
                RemoveBreakTorque(axleInfo);
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                print("Breaking");
                motor = 0;
                axleInfo.leftWheel.brakeTorque = maxMotorTorque * breakForce;
                axleInfo.rightWheel.brakeTorque = maxMotorTorque * breakForce;

                breakInfo_LW = axleInfo.leftWheel.brakeTorque;
                breakInfo_RW = axleInfo.rightWheel.brakeTorque;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                RemoveBreakTorque(axleInfo);
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    void RemoveBreakTorque(Axle wheel)
    {
        wheel.leftWheel.brakeTorque = 0;
        wheel.rightWheel.brakeTorque = 0;
    }
}