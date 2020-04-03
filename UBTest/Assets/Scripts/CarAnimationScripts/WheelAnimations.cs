using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelAnimations : MonoBehaviour
{
    [SerializeField]
    float rotationAngle;

    [SerializeField] float defaultRotationAngle;
    
    [SerializeField] GameObject[] axles;
    [SerializeField] GameObject steeringWheel;

    [SerializeField] int axleTurnSpeedAnim = 3;

    WhellCarPhysicsEditor wheelCarInfo;

    void Start()
    {
        wheelCarInfo = GetComponent<WhellCarPhysicsEditor>();
    }

    void FixedUpdate()
    {
        //Axles
        for (int i = 0; i < axles.Length; i++)
        {
            //Animate wheel steering
            rotationAngle = (Time.deltaTime * axleTurnSpeedAnim * Input.GetAxis("Horizontal"));
            axles[i].transform.localRotation = Quaternion.Euler(transform.localRotation.x, rotationAngle + 90, transform.localRotation.z);

            //Animate wheel rotating fowards
            var rotation = wheelCarInfo.ReturnRPMs() * 3;
            rotation *= Time.deltaTime;
            Quaternion turnWheel = Quaternion.Euler(0f, 0f, rotation);
            axles[i].transform.Rotate(turnWheel.eulerAngles, Space.Self);
        }

        //SteeringWheel
        steeringWheel.transform.localRotation = Quaternion.Euler(-150, transform.localRotation.y, rotationAngle);

    }
}

////Rotation
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