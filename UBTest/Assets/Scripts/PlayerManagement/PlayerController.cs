using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    AnglePlayerController anglePlayerController;
    Rigidbody rb;
    [SerializeField] RayCaster[] gravityPoints;
    public float gravityForce = 10000f;
    float boost = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.down;

        anglePlayerController = GetComponent<AnglePlayerController>();
    }

    [SerializeField] float throotle = 0.0f;
    [SerializeField] float maxAccelerationSpeed = 6.0f;
    [SerializeField] float minAccelerationSpeed = 0.0f;
    [SerializeField] float rotationSpeed = 50.0F;

    void Acceleration()
    {
        //transform.Translate(Vector3.forward * 100f / 3.6f * Time.deltaTime * extraSpeed);
        //if(boost < 0.1f)
            transform.Translate(Vector3.forward * 100f / 3.6f * Time.deltaTime * throotle);
        //else
        //    transform.Translate(Vector3.forward * 100f / 3.6f * Time.deltaTime * throotle * boost);
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

            if (GameData.road == gravityPoints[i].groundType)
                print("On the road!");
            else if (GameData.badGround == gravityPoints[i].groundType)
            {
                print("On badroad!");
                if(throotle > 1.5f){
                    throotle -= 0.5f * Time.deltaTime;
                }
            }
        }


        //Rotation affairs
        //print(anglePlayerController.pitchFoward);

        //Rotation

        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        rotation *= Time.deltaTime;
        Quaternion turn = Quaternion.Euler(anglePlayerController.pitchFoward, rotation, 0f);
        transform.Rotate(turn.eulerAngles, Space.Self);
        
        if(Input.GetAxis("Horizontal") == 1 || Input.GetAxis("Horizontal") == -1)
        {
            throotle -= 0.01f * Time.deltaTime;
            print("Loosing speed during curve");
        }


        Acceleration();
        
        if (Input.GetAxis("Vertical") > 0)
        {
            //print(Input.GetAxis("Vertical"));
            if(throotle < maxAccelerationSpeed){
                throotle += 0.4f * Time.deltaTime;
            }
        }

        if (Input.GetAxis("Vertical") != 1)
        {
            if(throotle > minAccelerationSpeed)
            {
                throotle -= 0.2f * Time.deltaTime;
            }
        }

    }
    void Update()
    {
        //rb.AddRelativeForce(Vector3.down * 100);
        //if (transform.position.y > transform.position.y)
        //{
        //body.AddForceAtPosition(hoverPoint.transform.up * gravityForce, hoverPoint.transform.position);
        //}
        //else
        //{
        //    body.AddForceAtPosition(hoverPoint.transform.up * -gravityForce, hoverPoint.transform.position);
        //}

        //rb.MoveRotation(rb.rotation * turn);

        //transform.Rotate(Quaternion.Euler(rotation, 0, 0));
        //transform.Translate(Vector3.right * 100f / 3.6f * Time.deltaTime);
        //}

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
}
