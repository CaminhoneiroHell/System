using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;
    public float gravityForce = 1000f;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.down;
    }

    [SerializeField] float throotle = 0.0f;
    [SerializeField] float maxAccelerationSpeed = 6.0f;
    [SerializeField] float minAccelerationSpeed = 0.0f;
    [SerializeField] float rotationSpeed = 50.0F;

    void Acceleration()
    {
        //transform.Translate(Vector3.forward * 100f / 3.6f * Time.deltaTime * extraSpeed);
        transform.Translate(Vector3.forward * 100f / 3.6f * Time.deltaTime * throotle);
    }
    void FixedUpdate()
    {
        Acceleration();

        //rb.AddRelativeForce(Vector3.down * 100);
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
        
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        rotation *= Time.deltaTime;
        Quaternion turn = Quaternion.Euler(0f, rotation, 0f);
        transform.Rotate(turn.eulerAngles, Space.Self);

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

        //right
        if (Input.GetAxis("Horizontal") > 0)
        {
            //transform.Translate(Vector3.left * 100f / 3.6f * Time.deltaTime);
        }
    }
}
