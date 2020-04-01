using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelAnimations : MonoBehaviour
{
    float rotationAngle;
    
    [SerializeField] GameObject[] wheels;
    
    [SerializeField] float minAngle, maxAngle;
    [SerializeField] Quaternion euler;
    void Update()
    {

        for (int i = 0; i < wheels.Length; i++)
        {
            euler = transform.rotation;
             
        }


        if (Input.GetAxis("Horizontal") > 0)
        {
            print("Right");
            for (int i = 0; i < wheels.Length; i++)
            {
                if((rotationAngle) < maxAngle)
                {
                    rotationAngle = Time.time * 1;
                    wheels[i].transform.localRotation = Quaternion.Euler(0, rotationAngle + 90, 0);
                }
            }
        }

        if (Input.GetAxis("Horizontal") < 0)
        {

        }


        //for (int i = 0; i < wheels.Length; i++)
        //{
        //    rotationAngle = (Mathf.Cos(Time.time) * 180) / Mathf.PI * 0.5f;
        //    rotationAngle = Mathf.Clamp(rotationAngle, minAngle, maxAngle); 
        //    wheels[i].transform.localRotation = Quaternion.Euler(0, rotationAngle + 90, 0);
        //}

    }
}