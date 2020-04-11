using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum RayCastDirection
{
    FOWARD,
    FOWARD2,
    BEHIND,
    LEFT,
    RIGHT,
    UP,
    DOWN
}


public class RayCaster : MonoBehaviour
{
    //const int badGround = 9;
    //const int road = 8;

    public Text stageName;
    public Image crossHair;
    public RayCastDirection rcstDir;

    #region Coordination_Variables
    float maxDistance = 100;
    float distanceTofloorFwd;
    float distanceToFloorbwd;
    float distanceToFloorUp;
    float distanceToFloorDwn;
    float distanceToFloorRight;
    float distanceToFloorLeft;
    #endregion

    #region LayerBitwise_Variables
    Int32 packed = 0;
    #endregion

    private void Start() {
        packed = (1 << 8 | 1 << 9 );
        //stagePacked = 
        //UnityEngine.Debug.Log("Pack result" + Convert.ToString(packed, 2 ).PadLeft(32, '0'));
    }

    #region raycasters

    public int groundType;
    public float distanceFromGround;
    bool missed;



    //Again...
    private float timer = 0.0f;
    private int seconds;
    void TimerCounter()
    {
        timer += Time.deltaTime;
        seconds = (int)(timer % 60);
    }

    public Vector3 CastRayDown(int layerMask)
    {
        RaycastHit hitDown;
        Vector3 fwd = transform.TransformDirection(Vector3.down);
        if (Physics.Raycast(transform.position, fwd, out hitDown, maxDistance, layerMask))
        {
            distanceFromGround = hitDown.distance;
            groundType = hitDown.collider.gameObject.layer;

            if (hitDown.collider.gameObject.layer == GameData.road){
                Debug.DrawRay(transform.position, fwd * hitDown.distance, Color.green);
            }else if (hitDown.collider.gameObject.layer == GameData.badGround)
            {
                Debug.DrawRay(transform.position, fwd * hitDown.distance, Color.yellow);
            }

            return hitDown.point;
        }
        else
        {
            print("missed");
            //missed = true; //Reset player position on the last checkpoint 
            Debug.DrawRay(transform.position, fwd * maxDistance, Color.red);
            TimerCounter();
            
            if(seconds > 2)
            {
                GetComponentInParent<PlayerController>().PlayerPositionReset();
                print("Reset Position");
            }



            return transform.position + (transform.forward * maxDistance);
        }
    }

        
    public Vector3 CastRayFoward()
    {
        RaycastHit hitFoward;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, fwd, out hitFoward, maxDistance))
        {
            Debug.DrawRay(transform.position, fwd * hitFoward.distance, Color.blue);
            //print("Hitting: " + hitFoward.collider.gameObject.name);

            return hitFoward.point;
        }
        else
        {
            Debug.DrawRay(transform.position, fwd * maxDistance, Color.blue);
            return transform.position + (transform.forward * maxDistance);

        }
    }

    public Vector3 CastRayFoward(Color color)
    {
        RaycastHit hitFoward;
        Vector3 bwd = transform.TransformDirection(Vector3.forward) * maxDistance;
        Debug.DrawRay(transform.position, bwd, color);
        if (Physics.Raycast(transform.position, bwd, out hitFoward))
        {
            distanceToFloorbwd = hitFoward.distance;
            print("The collided tag is: " + hitFoward.collider.tag);
            stageName.text = ("Stage Name: \n " + hitFoward.collider.tag);
            crossHair.color = Color.red;

            if(hitFoward.collider.tag == "Farm")
            {
                StartCoroutine(LoadFarm());
            }
            
            
            return hitFoward.point;
        }
        //print("Miss");
        StopCoroutine(LoadFarm());
        crossHair.color = Color.white;
        stageName.text = "Stage Name";
        return transform.position + (transform.forward * -maxDistance);
    }

    IEnumerator LoadFarm()
    {
        yield return new WaitForSeconds(2.5f);
        //GameManager.Instance.ChangeState(State.MooMooFarm_race);
    }



    public Vector3 CastRayBackward()
    {
        RaycastHit hitBackward;
        Vector3 bwd = transform.TransformDirection(Vector3.forward) * -maxDistance;
        Debug.DrawRay(transform.position, bwd, Color.red);
        if (Physics.Raycast(transform.position, bwd, out hitBackward))
        {
            distanceToFloorbwd = hitBackward.distance;
            Debug.Log("Distance from the collider to object who shooted raycast is: " + hitBackward.distance);
            print("The collided tag is: " + hitBackward.collider.tag);
            return hitBackward.point;
        }
        return transform.position + (transform.forward * -maxDistance);
    }

    public Vector3 CastRayUp()
    {
        RaycastHit hitUp;
        Vector3 up = transform.TransformDirection(Vector3.up) * maxDistance;
        Debug.DrawRay(transform.position, up, Color.blue);
        if (Physics.Raycast(transform.position, up, out hitUp))
        {
            hitUp.distance = distanceToFloorUp;
            Debug.Log("Distance from the collider to object who shooted raycast is: " + hitUp.distance);
            print("The collided tag is: " + hitUp.collider.tag);
            return hitUp.point;
        }
        return transform.position + (transform.up * maxDistance);
    }

    public Vector3 CastRayRight()
    {
        RaycastHit hitRight;
        Vector3 rght = transform.TransformDirection(Vector3.right) * maxDistance;
        Debug.DrawRay(transform.position, rght, Color.green);
        if (Physics.Raycast(transform.position, rght, out hitRight))
        {
            hitRight.distance = distanceToFloorRight;
            Debug.Log("Distance from the collider to object who shooted raycast is: " + hitRight.distance);
            print("The collided tag is: " + hitRight.collider.tag);
            return hitRight.point;
        }
        return transform.position + (transform.right * maxDistance);
    }

    public Vector3 CastRayLeft()
    {
        RaycastHit hitLeft;
        Vector3 lft = transform.TransformDirection(Vector3.right) * -maxDistance;
        Debug.DrawRay(transform.position, lft, Color.red);
        if (Physics.Raycast(transform.position, lft, out hitLeft))
        {
            Debug.Log("Distance from the collider to object who shooted raycast is: " + hitLeft.distance);
            //print("The collided tag is: " + hitLeft.collider.tag);
            hitLeft.distance = distanceToFloorLeft;
            return hitLeft.point;
        }
        return transform.position + (transform.right * -maxDistance);
    }

    #endregion

    void UnpackBitsFromLayerMask()
    {

    }
    private void Execute_RayCasterShooter()
    {
        switch(rcstDir)
        {
            case RayCastDirection.DOWN:
               CastRayDown(packed);
                break;
            case RayCastDirection.FOWARD:
                CastRayFoward();
                break;
            case RayCastDirection.FOWARD2:
                CastRayFoward(Color.blue);
                break;
            default:
                break;
        }
    }


    float frontAngle;
    void Update()
    {
        Execute_RayCasterShooter();

    }
    
}