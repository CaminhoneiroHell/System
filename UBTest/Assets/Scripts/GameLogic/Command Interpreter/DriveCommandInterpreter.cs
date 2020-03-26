using UnityEngine;
using UnityEngine.UI;

public class DriveCommandInterpreter : MonoBehaviour
{
    public WheelCollider[] wheelColliderArray;
    public GameObject[] wheelArray;

    public float torque = 200;
    public float maxSteerAngle = 30.0f;
    public float maxBrakeTorque = 500.0f;

    //public AudioSource skidSound;
    //public AudioSource highAccelSound;

    //public Transform skidTrailPrefab;
    //Transform[] skidTrails = new Transform[4];

    //public ParticleSystem smokePrefab;
    //ParticleSystem[] skidSmoke = new ParticleSystem[4];

    //public GameObject brakeLight;

    public Rigidbody rb;
    public float gearLength = 3.0f;
    public float currentSpeed { get { return rb.velocity.magnitude * gearLength; } }
    public float lowPitch = 1.0f;
    public float highPitch = 6.0f;
    public int numGears = 5;
    float rpm;
    int currentGear = 1;
    float currentGearPerc;
    public float maxSpeed = 200.0f;

    //public GameObject playerNamePrefab;
    //public Renderer jeepMesh;

    //public string networkName = "";

    //string[] aiNames = { "Adrian", "Lee", "Penny", "Merlin", "Tabytha", "Pauline", "John", "Kia", "Chloe", "Fiona", "Mathew" };

    //public void StartSkidTrail(int i)
    //{

    //    if (skidTrails[i] == null)
    //    {

    //        skidTrails[i] = Instantiate(skidTrailPrefab);
    //    }

    //    skidTrails[i].parent = wheelColliderArray[i].transform;
    //    skidTrails[i].localPosition = -Vector3.up * wheelColliderArray[i].radius;
    //}

    //public void EndSkidTrail(int i)
    //{

    //    if (skidTrails[i] == null) return;

    //    Transform holder = skidTrails[i];
    //    skidTrails[i] = null;
    //    holder.parent = null;
    //    Destroy(holder.gameObject, 30);
    //}

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < 4; ++i)
        {
            //skidSmoke[i] = Instantiate(smokePrefab);
            //skidSmoke[i].Stop();
        }

        //brakeLight.SetActive(false);

        //GameObject playerName = Instantiate(playerNamePrefab);
        //playerName.GetComponent<NameUIController>().target = rb.gameObject.transform;

        //if (this.GetComponent<AIController>().enabled)
        //    if (networkName != "")
        //        playerName.GetComponent<Text>().text = networkName;
        //    else
        //        playerName.GetComponent<Text>().text = aiNames[Random.Range(0, aiNames.Length)];
        //else
        //    playerName.GetComponent<Text>().text = PlayerPrefs.GetString("PlayerName");

        //playerName.GetComponent<NameUIController>().carRend = jeepMesh;
    }

    public void CalculateEngineSound()
    {

        float gearPercentage = (1 / (float)numGears);
        float targetGearFactor = Mathf.InverseLerp(gearPercentage * currentGear, gearPercentage * (currentGear + 1),
            Mathf.Abs(currentSpeed / maxSpeed));

        currentGearPerc = Mathf.Lerp(currentGearPerc, targetGearFactor, Time.deltaTime * 5.0f);

        var gearNumFactor = currentGear / (float)numGears;
        rpm = Mathf.Lerp(gearNumFactor, 1, currentGearPerc);

        float speedPercentage = Mathf.Abs(currentSpeed / maxSpeed);
        float upperGearMax = (1 / (float)numGears) * (currentGear + 1);
        float downGearMax = (1 / (float)numGears) * currentGear;

        if (currentGear > 0 && speedPercentage < downGearMax)
        {

            currentGear--;
        }

        if (speedPercentage > upperGearMax && (currentGear < (numGears - 1)))
        {

            currentGear++;
        }

        float pitch = Mathf.Lerp(lowPitch, highPitch, rpm);
        //highAccelSound.pitch = Mathf.Min(highPitch, pitch) * 0.25f;

    }

    public void CheckForSkid()
    {

        int numSkidding = 0;
        for (int i = 0; i < 4; ++i)
        {

            WheelHit wheelHit;
            wheelColliderArray[i].GetGroundHit(out wheelHit);

            if (Mathf.Abs(wheelHit.forwardSlip) >= 0.4f || Mathf.Abs(wheelHit.sidewaysSlip) >= 0.4f)
            {

                numSkidding++;
                //if (!skidSound.isPlaying)
                //{
                //    skidSound.Play();
                //}
                //// StartSkidTrail(i);
                //skidSmoke[i].transform.position = wheelColliderArray[i].transform.position - wheelColliderArray[i].transform.up * wheelColliderArray[i].radius;
                //skidSmoke[i].Emit(1);
            }
            else
            {

                // EndSkidTrail(i);
            }
        }
        //if (numSkidding == 0 && skidSound.isPlaying)
        //{

        //    skidSound.Stop();
        //}
    }



    public void Go(float accel, float steer, float brake)
    {

        accel = Mathf.Clamp(accel, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;

        if (brake != 0.0f)
        {

            //brakeLight.SetActive(true);
            Debug.Log("Braking");
        }
        else
        {

            //brakeLight.SetActive(false);
            //Debug.Log("Not Braking");
        }

        float thrustTorque = 0.0f;
        if (currentSpeed < maxSpeed)
        {

            thrustTorque = accel * torque;
        }

        for (int i = 0; i < wheelColliderArray.Length; ++i)
        {

            wheelColliderArray[i].motorTorque = thrustTorque;

            if (i < 2)
            {

                wheelColliderArray[i].steerAngle = steer;
            }
            else
            {

                wheelColliderArray[i].brakeTorque = brake;
            }

            Quaternion quat;
            Vector3 position;

            wheelColliderArray[i].GetWorldPose(out position, out quat);

            //wheelArray[i].transform.position = position;
            //wheelArray[i].transform.rotation = quat;
        }
    }
}
