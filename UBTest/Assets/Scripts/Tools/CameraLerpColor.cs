using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerpColor : MonoBehaviour
{
    public Color color1 = Color.black;
    public Color color2 = Color.white;
    public float duration;
    private float deltaTime = 0.0f;

    public Camera camera;

    void Start()
    {
        duration = 3.0f;
        deltaTime = 0.0f;
    }

    void Update()
    {
        StartCoroutine(StartCameraFade());
    }

    IEnumerator StartCameraFade()
    {
        yield return new WaitForSeconds(5f);
        deltaTime += Time.deltaTime;
        if (deltaTime < duration)
        {
            camera.backgroundColor = Color.Lerp(color1, color2, deltaTime);
        }
        //yield return new WaitForSeconds(1f);
        //gameObject.GetComponent<Animator>().SetBool("Glitch", true);
    }
}