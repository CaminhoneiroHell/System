using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WhellCarPhysicsEditor : MonoBehaviour
{

    //public WheelCollider[] wheelColliderArray;
    public List<WheelCollider> wheelColliders = new List<WheelCollider>();
    //int i = 0;


    [Header("Collision")]
    [SerializeField]float m_wheelMass = 20f;
    [SerializeField]float m_wheelDampingRate = 0.25f; 
    [SerializeField] float m_wheelSuspensionDistance = 0.3f;

    [Header("Suspension")]
    [SerializeField] float m_suspensionSpring = 35000f;
    [SerializeField] float m_suspensionDamper = 4500f;
    [SerializeField] float m_suspensionTargetPos = 0.5f;

    [Header("Friction Forward")]
    [SerializeField] float m_fwdFrct_xtrSlip = 0.4f;
    [SerializeField] float m_fwdFrct_xtrVal = 1f;
    [SerializeField] float m_fwdFrct_AsymSlip = 0.8f;
    [SerializeField] float m_fwdFrct_AsympVal = 0.5f;
    [SerializeField] float m_fwdFrct_Stiff = 1f;

    [Header("Friction Sideways")]
    [SerializeField] float m_sideFrct_xtrSlip = 0.2f;
    [SerializeField] float m_sideFrct_xtrVal = 1f;
    [SerializeField] float m_sideFrct_AsymSlip = 0.5f;
    [SerializeField] float m_sideFrct_AsympVal = 0.75f;
    [SerializeField] float m_sideFrct_Stiff = 1f;


    JointSpring jointSpring;
    WheelFrictionCurve frictionCurve;
    WheelFrictionCurve sideFrictionCurve;

    public IUnityService unityService;

    private void Start()
    {
        if (unityService == null)
            unityService = new UnityService();
    }

    private void Update()
    {
        foreach(WheelCollider w in wheelColliders)
        {
            //print(wheelColliders.Find(x => x.gameObject.name.Contains("F")));
            
            //Collision
            w.mass = m_wheelMass;
            w.wheelDampingRate = m_wheelDampingRate;
            w.suspensionDistance = m_wheelSuspensionDistance;

            //Suspension
            jointSpring.spring = m_suspensionSpring;
            jointSpring.damper = m_suspensionDamper;
            jointSpring.targetPosition = m_suspensionTargetPos;

            w.suspensionSpring = jointSpring;

            //Foward Friction
            frictionCurve.extremumSlip = m_fwdFrct_xtrSlip;
            frictionCurve.extremumValue = m_fwdFrct_xtrVal;
            frictionCurve.asymptoteSlip = m_fwdFrct_AsymSlip;
            frictionCurve.asymptoteValue = m_fwdFrct_AsympVal;
            frictionCurve.stiffness = m_fwdFrct_Stiff;

            w.forwardFriction = frictionCurve;

            //Sideways Friction
            sideFrictionCurve.extremumSlip = m_sideFrct_xtrSlip;
            sideFrictionCurve.extremumValue = m_sideFrct_xtrVal;
            sideFrictionCurve.asymptoteSlip = m_sideFrct_AsymSlip;
            sideFrictionCurve.asymptoteValue = m_sideFrct_AsympVal;
            sideFrictionCurve.stiffness = m_sideFrct_Stiff;

            w.sidewaysFriction = sideFrictionCurve;

        }

    }

    [SerializeField]float torque;
    [SerializeField] float breakTorque = 1000000;
    [SerializeField] float steer;

    float maxRot = 40.0f;
    float minRot = -40.0f;

    float maxVelocity = 99999, minVel = 0;
    
    [SerializeField] float rotationSpeed = 50.0F;
    private void FixedUpdate()
    {
        //Rotation
        var rotation = unityService.GetAxis("Horizontal") * rotationSpeed;
        rotation *= unityService.GetDeltaTime();
        Quaternion turn = Quaternion.Euler(0f, rotation, 0f);
        Quaternion turnWheel = Quaternion.Euler(0f, 0f, rotation);
        transform.Rotate(turn.eulerAngles, Space.Self);


        foreach (WheelCollider w in wheelColliders)
        {
            w.motorTorque = torque * 100f / 3.6f;
            if(unityService.GetAxis("Vertical") > 0 && torque < maxVelocity && unityService.GetAxis("Horizontal") == 0)
            {
                torque += 400 * Time.deltaTime;
            }else if(unityService.GetAxis("Vertical") == 0 && torque > minVel)
            {
                torque -= 650 * Time.deltaTime;
                
            }

            if (torque < minVel)
                torque = 0;

            //if (unityService.GetAxis("Horizontal") > 0 && w.steerAngle < maxRot)
            //{
            //    w.steerAngle += steer * Time.deltaTime;
            //    //torque -= 500 * Time.deltaTime;
            //}
            //else if (unityService.GetAxis("Horizontal") < 0 && w.steerAngle > minRot)
            //{
            //    w.steerAngle -= steer * Time.deltaTime;
            //    //torque -= 500 * Time.deltaTime;

            //}

            if (Input.GetKey(KeyCode.Space))
            {
                w.motorTorque = 0;
                w.brakeTorque += breakTorque * Time.deltaTime;
                print("Breaking" + w.brakeTorque);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                w.brakeTorque = 0;
                //breakTorque = 0;
            }
        }
    }
}
