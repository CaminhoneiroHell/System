using UnityEngine;
using System.Collections;

public class Hoover : MonoBehaviour
{
    Rigidbody Body;
    float DeadZone = 0.1f;
    private float groundedDrag = 3f;
    private float MaxVelocity = 50;
    private float HoverForce = 1000;
    private float GravityForce = 1000f;
    private float HoverHeight = 1.5f;
    public GameObject[] HoverPoints;

    private float ForwardAcceleration = 8000f;
    private float ReverseAcceleration = 4000f;
    float Thrust = 0f;

    private float TurnStrength = 1000f;
    float TurnValue = 0f;

    private ParticleSystem[] DustTrails = new ParticleSystem[2];

    int LayerMask;

    //public Rigidbody Body;
    //public float DeadZone;
    //public float GroundedDrag;
    //public float MaxVelocity;
    //public float HoverForce;
    //public float GravityForce;
    //public float HoverHeight;
    //[SerializeField] public GameObject[] HoverPoints;
    //public float ForwardAcceleration;
    //public float ReverseAcceleration;
    //public float Thrust;
    //public float TurnStrength;
    //public float TurnValue;
    ////public ParticleSystem[] DustTrails;
    //public int LayerMask;

    private void Start()
    {
        Body = GetComponent<Rigidbody>();
        Body.centerOfMass = Vector3.down;

            LayerMask = 1 << UnityEngine.LayerMask.NameToLayer("Vehicle");
        LayerMask = ~LayerMask;
    }

    private void Update()
    {
        // Main Thrust
        Thrust = 0.0f;
        float acceleration = Input.GetAxis("Vertical");
        if (acceleration > DeadZone)
            Thrust = acceleration * ForwardAcceleration;
        else if (acceleration < -DeadZone)
            Thrust = acceleration * ReverseAcceleration;

        // Turning
        TurnValue = 0.0f;
        float turnAxis = Input.GetAxis("Horizontal");
        if (Mathf.Abs(turnAxis) > DeadZone)
            TurnValue = turnAxis;

    }



    void FixedUpdate() { 
           //  Hover Force
            RaycastHit hit;
            bool grounded = false;
            for (int i = 0; i<HoverPoints.Length; i++)
            {
                var hoverPoint = HoverPoints[i];
                if (Physics.Raycast(hoverPoint.transform.position, -Vector3.up, out hit,HoverHeight, LayerMask))
                {
                    Body.AddForceAtPosition(Vector3.up* HoverForce* (1.0f - (hit.distance / HoverHeight)), hoverPoint.transform.position);
                    grounded = true;
                }
                else
                {
                    // Self levelling - returns the vehicle to horizontal when not grounded
                    if (transform.position.y > hoverPoint.transform.position.y)
                    {
                        Body.AddForceAtPosition(hoverPoint.transform.up* GravityForce, hoverPoint.transform.position);
                    }
                    else
                    {
                        Body.AddForceAtPosition(hoverPoint.transform.up* -GravityForce, hoverPoint.transform.position);
                    }
                }
            }

            var emissionRate = 0;
            if (grounded)
            {
                Body.drag = groundedDrag;
                emissionRate = 10;
            }
            else
            {
                Body.drag = 0.1f;
                Thrust /= 100f;
                TurnValue /= 100f;
            }
 
            //for(int i = 0; i<DustTrails.Length; i++)
            //{
            //    var emission = DustTrails[i].emission;
            //    emission.rate = new ParticleSystem.MinMaxCurve(emissionRate);
            //}

             // Handle Forward and Reverse forces
        if (Mathf.Abs(Thrust) > 0)
          Body.AddForce(transform.forward* Thrust);
 
        // Handle Turn forces
        if (TurnValue > 0)
        {
          Body.AddRelativeTorque(Vector3.up* TurnValue * TurnStrength);
        } else if (TurnValue< 0)
        {
          Body.AddRelativeTorque(Vector3.up* TurnValue * TurnStrength);
        }
 
        // Limit max velocity
        if(Body.velocity.sqrMagnitude > (Body.velocity.normalized* MaxVelocity).sqrMagnitude)
        {
            Body.velocity = Body.velocity.normalized* MaxVelocity;
        }
    }
}