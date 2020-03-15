using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglePlayerController : MonoBehaviour
{
    [SerializeField]float angleX;
    public float pitchFoward;
    public bool angleCorrectnessCheck;
    public float AngleX()
    {
        angleX = Vector3.Angle(transform.up, transform.InverseTransformPoint(Vector3.up));
        return angleX;
    }

    //bool isMoreThan90Deegres;
    void ChaciNormalizer(bool isMoreThan90Deegres)
    {
        if (isMoreThan90Deegres)
        {
            if (AngleX() != 90f)
            {
                pitchFoward = 20.0f;
                //pitchFoward *= Time.deltaTime ;
            }
        }
        else
        {
            if (AngleX() != 90f)
            {
                pitchFoward = -20.0f;
            }
        }
    }

    private void Update()
    {
        pitchFoward *= Time.deltaTime;
        //print(AngleX());
        if (AngleX() > 110f)
        {
            angleCorrectnessCheck = false;
            ChaciNormalizer(true);
        }
        else if (AngleX() < 81f)
        {
            angleCorrectnessCheck = false;
            ChaciNormalizer(false);
        } else
        angleCorrectnessCheck = true;

        //print("É APROXIMADAMENTE 90 GRAU?" + );


        //Quaternion turn = Quaternion.Euler(pitchFoward, 0, 0f);
        //transform.Rotate(turn.eulerAngles, Space.Self);
    }
}
