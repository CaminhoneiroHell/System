using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Axle {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
    public bool boost;
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
        float torque = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        //breakInfo
        foreach (Axle axleInfo in axleInfos) {
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor){
                RemoveBreakTorque(axleInfo);
                axleInfo.leftWheel.motorTorque = torque;
                axleInfo.rightWheel.motorTorque = torque;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                print("Breaking");
                torque = 0;
                axleInfo.leftWheel.brakeTorque = maxMotorTorque * breakForce;
                axleInfo.rightWheel.brakeTorque = maxMotorTorque * breakForce;

                breakInfo_LW = axleInfo.leftWheel.brakeTorque;
                breakInfo_RW = axleInfo.rightWheel.brakeTorque;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                RemoveBreakTorque(axleInfo);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                StartCoroutine(BoostRoutine());
                //axleInfo.leftWheel.motorTorque = maxMotorTorque * 111;
                //axleInfo.rightWheel.motorTorque = maxMotorTorque * 111;
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    IEnumerator BoostRoutine()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * 150, ForceMode.Acceleration);
        print("Giving force: " + Time.time);
        yield return new WaitForSeconds(4f);
    }

    void RemoveBreakTorque(Axle wheel)
    {
        wheel.leftWheel.brakeTorque = 0;
        wheel.rightWheel.brakeTorque = 0;
    }
}