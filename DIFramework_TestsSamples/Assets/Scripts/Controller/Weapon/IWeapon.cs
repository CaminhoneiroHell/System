using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void Shoot();
    void Charge();
}

public interface IBullet
{
    GameObject BulletComponent();
}


public class Buster : IWeapon
{
    public Buster()
    {
        Charge();
    }

    public void Charge()
    {
        Debug.Log("Buster On!");
        //throw new System.NotImplementedException();
    }

    public void Shoot()
    {
        throw new System.NotImplementedException();
    }   
}

public class MachineGun : IWeapon
{
    public void Charge()
    {
        throw new System.NotImplementedException();
    }

    public void Shoot()
    {
        throw new System.NotImplementedException();
    }
}

public class Bullet : IBullet
{
    public GameObject BulletComponent()
    {
        GameObject GO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GO.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        return GO;
    }
}