using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserClick : MonoBehaviour
{
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if(Physics.Raycast(rayOrigin, out hitInfo))
            {
                if(hitInfo.collider.tag == "Cube")
                {
                    ICommand click = new ClickCommand(hitInfo.collider.gameObject, new Color(Random.value, Random.value, Random.value));
                    click.Execute();
                    CommandManager.Instance.AddCommandOnBuffer(click);
                }
            }
        }
    }
}
