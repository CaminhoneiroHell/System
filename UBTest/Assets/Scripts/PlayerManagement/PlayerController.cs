using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    public IUnityService unityService;

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


        if (unityService == null)
            unityService = new UnityService();
    }

    [SerializeField] float throotle = 0.0f;
    [SerializeField] float maxAccelerationSpeed = 6.0f;
    [SerializeField] float minAccelerationSpeed = 0.0f;
    [SerializeField] float maxAccelerationSpeedBackwards = 1.5f;
    [SerializeField] float rotationSpeed = 50.0F;

    //float wheelrotationSpeed = 80.0f;

    void Acceleration()
    {
        transform.Translate(Vector3.forward * 100f / 3.6f * unityService.GetDeltaTime() * throotle);
    }

    void GravityManagement()
    {

        for (int i = 0; i < gravityPoints.Length; i++)
        {
            var wheelPos = gravityPoints[i];
            if (gravityPoints[i].distanceFromGround > 1.6f)
            {
                isGrounded = false;
                if (gravityPoints[i].distanceFromGround > 1f)
                {
                    rb.AddRelativeForce(Vector3.down * gravityForce);
                }
            }
            else
            {
                isGrounded = true;
            }

            if (GameData.badGround == gravityPoints[i].groundType)
            {
                //print("On badroad!");
                if (throotle > 1.5f)
                {
                    throotle -= 0.5f * unityService.GetDeltaTime();
                }
            }
        }
    }

    [SerializeField] bool isGrounded;
    private void FixedUpdate()
    {
        //Acceleration();
        //GravityManagement();


        //Rotation
        //var rotation = unityService.GetAxis("Horizontal") * rotationSpeed;
        //rotation *= unityService.GetDeltaTime();
        //Quaternion turn = Quaternion.Euler(anglePlayerController.pitchFoward, rotation, 0f);
        //Quaternion turnWheel = Quaternion.Euler(0f, 0f, rotation);
        //transform.Rotate(turn.eulerAngles, Space.Self);

        //if (unityService.GetAxis("Horizontal") > 0 || unityService.GetAxis("Horizontal") < 0)
        //{
        //    throotle -= 0.01f * unityService.GetDeltaTime();
        //    cartAnimation.steeringWheel.transform.Rotate(turnWheel.eulerAngles, Space.Self); //Anim steering wheel

        //    print("Loosing speed during curve");
        //}




        ////Front
        //if (unityService.GetAxis("Vertical") > 0)
        //{
        //    //print(Input.GetAxis("Vertical"));
        //    if (throotle < maxAccelerationSpeed)
        //    {
        //        throotle += 0.4f * unityService.GetDeltaTime();
        //    }
        //}

        //Back
        //if (unityService.GetAxis("Vertical") == -1)
        //{
        //    if (throotle < maxAccelerationSpeedBackwards)
        //    {
        //        throotle += 0.4f * unityService.GetDeltaTime();
        //        transform.Translate(-Vector3.forward * 100f / 3.6f * unityService.GetDeltaTime() * throotle);
        //    }
        //}

        //Lose speed during curves
        //if (unityService.GetAxis("Vertical") != 1)
        //{
        //    if (throotle > minAccelerationSpeed)
        //    {
        //        throotle -= 0.2f * unityService.GetDeltaTime();
        //    }
        //}


        //if (gameObject.transform.position.y < 4.8)
        //{
        //    timer += unityService.GetDeltaTime();
        //    if (timer > 1.0f)
        //    {
        //        PlayerPositionReset();
        //        print("Reset Position");
        //    }
        //}


        //Boost

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPositionReset();
        }
    }

    float timer;

    void Update()
    {
    }

    [SerializeField] GameObject respawnPoint;
    public Transform PlayerPositionReset()
    {
        throotle = 0;
        //gameObject.transform.position = new Vector3(playerPositionReset.position);
        gameObject.transform.position = new Vector3(respawnPoint.transform.position.x, respawnPoint.transform.position.y, respawnPoint.transform.position.z);
        gameObject.transform.rotation = new Quaternion(respawnPoint.transform.rotation.x, respawnPoint.transform.rotation.y, respawnPoint.transform.rotation.z, respawnPoint.transform.rotation.w);
        return gameObject.transform;
    }
}
