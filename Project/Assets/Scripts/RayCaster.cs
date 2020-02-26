using System;
using UnityEngine;
using System.Collections;

public class RayCaster : MonoBehaviour
{

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
    static int layerMask_cylinder = 1 << 11;
    static int layerMask_capsule = 1 << 10;
    static int layerMask_sphere = 1 << 9;
    // int bitLayerPack_cylinder = Convert.ToInt32(layerMask_cylinder);
    // int bitLayerPack_capsule = Convert.ToInt32(layerMask_capsule);
    // int bitLayerPack_sphere = Convert.ToInt32(layerMask_sphere);

    Int32 packed = 0;
    #endregion
    private void Start() {
        // layerMask &= ~(layerMask); 
        
        // packed = packed | layerMask_cylinder << 20;
        // packed = packed | (layerMask_cylinder << 21);
        // packed = packed | (bitLayerPack_capsule << 21);
        // packed = packed | (bitLayerPack_sphere << 12);


        packed = (1 << 11 | 1 << 9 );
        UnityEngine.Debug.Log("Pack result" + Convert.ToString(packed, 2 ).PadLeft(32, '0'));
    }


    void Update()
    {
        Execute_RayCasterShooter();
    }
    
    public void Execute_RayCasterShooter()
    {
        CastRayFoward(Color.green, packed);
        //CastRayBackward();
        //CastRayDown();
        //CastRayLeft();
        // CastRayRight();
        //CastRayUp();
    }
    #region raycasters

    void UnpackBitsFromLayerMask()
    {

    }

    public Vector3 CastRayFoward(Color color, int layerMask)
    {
        RaycastHit hitFoward;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, fwd, out hitFoward, maxDistance, layerMask ))
        {
            //Debug.Log("We hit: " + hit.transform.name);
            // distanceTofloorFwd = hitFoward.collider.gameObject.layer = layerMask;
            // maxDistance = distanceTofloorFwd;
            print(Convert.ToString(layerMask, 2).PadLeft(32, '0'));

            Debug.DrawRay(transform.position, fwd * hitFoward.distance, color);
            Debug.Log("Distance from the collider to object who shooted raycast is: " + hitFoward.distance);
            // print("The collided tag is: " + hitFoward.collider.tag);
            print("The collided layer is: " + hitFoward.collider.gameObject.layer);
            print("Hitting: " + hitFoward.collider.gameObject.name);
            return hitFoward.point;
        }
        else
        {
            print("missed");
            Debug.DrawRay(transform.position, fwd * maxDistance, Color.red);
            return transform.position + (transform.forward * maxDistance);
        }
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

    public Vector3 CastRayDown()
    {
        RaycastHit hitDown;
        Vector3 down = transform.TransformDirection(Vector3.up) * -maxDistance;
        if (Physics.Raycast(transform.position, down, out hitDown))
        {
            hitDown.distance = distanceToFloorDwn;
            Debug.Log("Distance from the collider to object who shooted raycast is: " + hitDown.distance);
            //print("The collided tag is: " + hitDown.collider.tag);
            return hitDown.point;
        }

        return transform.position + (transform.up * -maxDistance);
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
}