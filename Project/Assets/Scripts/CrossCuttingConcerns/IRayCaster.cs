using UnityEngine;
using System.Collections;

public interface IRayCaster
{
    Vector3 CastRayFoward();

    Vector3 CastRayBackward();

    Vector3 CastRayUp();

    Vector3 CastRayDown();

    Vector3 CastRayRight();

    Vector3 CastRayLeft();
}


public class RayCaster : MonoBehaviour, IRayCaster
{

    #region variables
    readonly float maxDistance = 100;
    //float distanceTofloorFwd;
    //float distanceToFloorbwd;
    //float distanceToFloorUp;
    //float distanceToFloorDwn;
    //float distanceToFloorRight;
    //float distanceToFloorLeft;
    #endregion

    //void Update()
    //{
    //    Execute_RayCasterShooter();
    //}
    
    //public void Execute_RayCasterShooter()
    //{
    //    //CastRayFoward();
    //    //CastRayBackward();
    //    //CastRayDown();
    //    //CastRayLeft();
    //    //CastRayRight();
    //    //CastRayUp();
    //}

    #region raycasters

    public Vector3 CastRayFoward()
    {
        RaycastHit hitFoward;
        Vector3 fwd = transform.TransformDirection(Vector3.forward) * maxDistance;
        Debug.DrawRay(transform.position, fwd, Color.yellow);
        if (Physics.Raycast(transform.position, fwd, out hitFoward))
        {
            //Debug.Log("We hit: " + hit.transform.name);
            //distanceTofloorFwd = hitFoward.distance;
            Debug.Log("Distance from the collider to object who shooted raycast is: " + hitFoward.distance);
            print("The collided tag is: " + hitFoward.collider.tag);

            return hitFoward.point;
        }

        return transform.position + (transform.forward * maxDistance);
    }

    public Vector3 CastRayBackward()
    {
        RaycastHit hitBackward;
        Vector3 bwd = transform.TransformDirection(Vector3.forward) * -maxDistance;
        Debug.DrawRay(transform.position, bwd, Color.red);
        if (Physics.Raycast(transform.position, bwd, out hitBackward))
        {
            //distanceToFloorbwd = hitBackward.distance;
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
            //hitUp.distance = distanceToFloorUp;
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
            //hitDown.distance = distanceToFloorDwn;
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
            //hitRight.distance = distanceToFloorRight;
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
            //hitLeft.distance = distanceToFloorLeft;
            return hitLeft.point;
        }
        return transform.position + (transform.right * -maxDistance);
    }

    #endregion
}