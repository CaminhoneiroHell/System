using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Animator anim;  //Use animator logic to enable or disable walk transforms
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public float speed = 1;
    // Update is called once per frame
    void Update()
    {
        if (!anim.GetBool("Walk"))
            return;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(
                Vector3.left.x * Time.deltaTime * speed,
                0,
                0);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(
               Vector3.right.x * Time.deltaTime * speed,
               0,
               0);
        }


        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(0,
                0,
                Vector3.forward.z * Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(0,
                   0,
                   Vector3.back.z * Time.deltaTime * speed);
        }
    }
}
