using UnityEngine;

public interface IUnityService
{
    float GetDeltaTime();
    float GetAxis(string axisName);
}

public class UnityService : IUnityService
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