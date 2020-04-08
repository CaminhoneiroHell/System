using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float rotSpeed;
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(Vector3.right.x * Time.deltaTime * speed,
            0,
            0,
            Space.Self);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(Vector3.left.x * Time.deltaTime * speed,
            0,
            0,
            Space.Self);
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            transform.Translate(0,
            0,
            Vector3.forward.z * Time.deltaTime * speed,
            Space.Self);
        }

        if (Input.GetAxis("Vertical") < 0)
        {

            transform.Translate(0,
            0,
            Vector3.back.z * Time.deltaTime * speed,
            Space.Self);
        }



        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.Rotate(0,
                Vector3.right.x * Time.deltaTime * rotSpeed,
                0);
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.Rotate(0,
                Vector3.left.x * Time.deltaTime * rotSpeed,
                0);
        }
    }
}
