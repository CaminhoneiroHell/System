using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    Rigidbody rb;
    CartAnimationController cartAnimation;
    AnglePlayerController anglePlayerController;
    public Transform playerPositionReset;

    [SerializeField] RayCaster[] gravityPoints;
    
    public float gravityForce = 10000f;
    float boost = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.down;

        anglePlayerController = GetComponent<AnglePlayerController>();
        cartAnimation = GetComponent<CartAnimationController>();
    }

    [SerializeField] float throotle = 0.0f;
    [SerializeField] float maxAccelerationSpeed = 6.0f;
    [SerializeField] float minAccelerationSpeed = 0.0f;
    [SerializeField] float maxAccelerationSpeedBackwards = 1.5f;
    [SerializeField] float rotationSpeed = 50.0F;

    //float wheelrotationSpeed = 80.0f;

    void Acceleration()
    {
        transform.Translate(Vector3.forward * 100f / 3.6f * Time.deltaTime * throotle);
    }

    [SerializeField] bool isGrounded;
    private void FixedUpdate()
    {
        for (int i = 0; i < gravityPoints.Length; i++)
        {

            var wheelPos = gravityPoints[i];
            if (gravityPoints[i].distanceFromGround > 1.6f)
            {
                isGrounded = false;
                //print("Out of ground" + gravityPoints[i].distanceFromGround);
                
                if (gravityPoints[i].distanceFromGround > 1f)
                {
                    rb.AddRelativeForce(Vector3.down * gravityForce);
                }
            }
            else
            {
                isGrounded = true;
            }

            //print("Current distance from the ground: " + gravityPoints[i].distanceFromGround);

            //if (GameData.road == gravityPoints[i].groundType)
            //    print("On the road!");
            if (GameData.badGround == gravityPoints[i].groundType)
            {
                //print("On badroad!");
                if(throotle > 1.5f){
                    throotle -= 0.5f * Time.deltaTime;
                }
            }
        }


        //Rotation affairs
        //print(anglePlayerController.pitchFoward);

        //Rotation

        var rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        //var wheelRotation = Input.GetAxis("Horizontal") * wheelrotationSpeed;
        rotation *= Time.deltaTime;
        Quaternion turn = Quaternion.Euler(anglePlayerController.pitchFoward, rotation, 0f);
        Quaternion turnWheel = Quaternion.Euler(0f, 0f, rotation);
        transform.Rotate(turn.eulerAngles, Space.Self);
        
        if(Input.GetAxis("Horizontal") == 1 || Input.GetAxis("Horizontal") == -1)
        {
            throotle -= 0.01f * Time.deltaTime;
            cartAnimation.steeringWheel.transform.Rotate(turnWheel.eulerAngles, Space.Self); //Anim steering wheel
            //cartAnimation.wheelBack.transform.Rotate(turn.eulerAngles, Space.Self); //Anim steering wheel
            //cartAnimation.wheelFront.transform.Rotate(turn.eulerAngles, Space.Self); //Anim steering wheel
            print("Loosing speed during curve");
        }


        Acceleration();

        //Front
        if (Input.GetAxis("Vertical") > 0)
        {
            //print(Input.GetAxis("Vertical"));
            if(throotle < maxAccelerationSpeed){
                throotle += 0.4f * Time.deltaTime;
            }
        }

        //Back
        if (Input.GetAxis("Vertical") == -1)
        {
            if (throotle < maxAccelerationSpeedBackwards)
            {
                throotle += 0.4f * Time.deltaTime;
                transform.Translate(- Vector3.forward * 100f / 3.6f * Time.deltaTime * throotle);
            }
        }

        if (Input.GetAxis("Vertical") != 1)
        {
            if(throotle > minAccelerationSpeed)
            {
                throotle -= 0.2f * Time.deltaTime;
            }
        }


        if(gameObject.transform.position.y < 4.8)
        {
            timer += Time.deltaTime;
            if(timer > 1.0f)
            {
                PlayerPositionReset();
                print("Reset Position");
            }
        }

    }

    float timer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            boost = 5f;
            if(boost > 0.1f)
            {
                boost -= 0.2f * Time.deltaTime;
                //SpawnParticles
            }
        }
        else
            boost = 0.1f;

        if (Input.GetAxis("Horizontal") > 0)
        {
            //transform.Translate(Vector3.left * 100f / 3.6f * Time.deltaTime);
        }
    }

    public Vector3 PlayerPositionReset()
    {
        throotle = 0;
        return gameObject.transform.position = playerPositionReset.position;
    }
}
