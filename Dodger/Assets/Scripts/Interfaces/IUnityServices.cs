using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IUNityServices
{
    float GetDeltaTime();
    float GetAxis(string axisName);
}

public class UnityServices : IUNityServices
{
    public float GetAxis(string axisName)
    {
        return Input.GetAxis(axisName);
    }

    public float GetDeltaTime()
    {
        return Time.deltaTime;
    }
}
