using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    ISoundManager soundManager;

    [Inject]
    IUNityServices unityServices;

    [SerializeField]
    int screenLimitLeft, screenLimitRight;

    float HorizontalAxis(){
        return unityServices.GetAxis("Horizontal");
    }

    void Update()
    {
        print((Screen.width/2));

        if (HorizontalAxis() > 0 && gameObject.transform.position.x < screenLimitRight)  
        {
            transform.Translate(new Vector3(HorizontalAxis() * unityServices.GetDeltaTime() * 5f, 0, 0), Space.World);
        }

        if (HorizontalAxis() < 0 && gameObject.transform.position.x > screenLimitLeft)
        {
            transform.Translate(new Vector3(HorizontalAxis() * unityServices.GetDeltaTime() * 5f, 0, 0), Space.World);
        }
    }
}
