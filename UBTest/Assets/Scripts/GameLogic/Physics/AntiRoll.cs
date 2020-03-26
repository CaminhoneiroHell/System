using UnityEngine;

public class AntiRoll : MonoBehaviour
{

    public float antiRoll = 5000.0f;
    public WheelCollider wheelLeftFront;
    public WheelCollider wheelLeftBack;
    public WheelCollider wheelRightFront;
    public WheelCollider wheelRightBack;
    public GameObject centerOfMass;
    public Rigidbody rb;
    public bool groundedL, groundedR;

    float lastTimeChecked;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.transform.localPosition;
    }

    void RightCar()
    {

        this.transform.position += Vector3.up;
        this.transform.rotation = Quaternion.LookRotation(this.transform.forward);
    }

    void GroundWheels(WheelCollider WL, WheelCollider WR)
    {

        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        groundedL = WL.GetGroundHit(out hit);
        if (groundedL)
        {
            //travelL = (-WL.transform.InverseTransformPoint(hit.point).y - WL.radius) / WL.suspensionDistance;
        }

        groundedR = WR.GetGroundHit(out hit);
        if (groundedL)
        {

            //travelR = (-WR.transform.InverseTransformPoint(hit.point).y - WR.radius) / WR.suspensionDistance;
        }

        float antiRollForce = (travelL - travelR) * antiRoll;

        if (groundedL)
        {
            //this.transform.rotation.z = 0;
            //rb.AddForceAtPosition(WL.transform.up * -antiRollForce, WL.transform.position);
        }

        if (groundedR)
        {

            //rb.AddForceAtPosition(WR.transform.up * antiRollForce, WR.transform.position);
        }

    }

    //private void Update()
    //{
    //    //RightCar();

    //    if (transform.up.y > 0.5f || rb.velocity.magnitude > 1.0f)
    //    {

    //        lastTimeChecked = Time.time;
    //    }

    //    if (Time.time > lastTimeChecked + 3.0f)
    //    {

    //        RightCar();
    //    }
    //}

    void FixedUpdate()
    {

        //print(this.transform.rotation.z);
        var rotationCorrectorSpeed = 2f;
        if (this.transform.localEulerAngles.z > 0.3f || this.transform.localEulerAngles.z < -0.3f)
        {
            print("Tranformation correction" + rotationCorrectorSpeed);
            rotationCorrectorSpeed *= Time.deltaTime * 100;
            Quaternion turn = Quaternion.Euler(0, 0, rotationCorrectorSpeed);
            this.transform.Rotate(turn.eulerAngles, Space.Self);
        }

        GroundWheels(wheelLeftFront, wheelRightFront);
        GroundWheels(wheelLeftBack, wheelRightBack);
    }
}
