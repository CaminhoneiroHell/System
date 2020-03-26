using UnityEngine;

public class PlayerInputInterpreter : MonoBehaviour
{
    DriveCommandInterpreter m_driveCmd;
    float lastTimeMoving = 0.0f;
    Vector3 lastPosition;
    Quaternion lastRotation;

    //CheckpointManager cpm;
    float finishSteer;

    void ResetLayer()
    {

        m_driveCmd.rb.gameObject.layer = 0;
        //this.GetComponent<Ghost>().enabled = false;
    }

    void Start()
    {

        m_driveCmd = this.GetComponent<DriveCommandInterpreter>();
        //this.GetComponent<Ghost>().enabled = false;
        lastPosition = m_driveCmd.rb.gameObject.transform.position;
        lastRotation = m_driveCmd.rb.gameObject.transform.rotation;
        finishSteer = Random.Range(-1.0f, 1.0f);
    }

    void Update()
    {

        //if (cpm == null)
        //{

        //    cpm = m_driveCmd.rb.GetComponent<CheckpointManager>();
        //}

        //if (cpm.lap == RaceMonitor.totalLaps + 1)
        //{

        //    m_driveCmd.highAccel.Stop();
        //    m_driveCmd.Go(0.0f, finishSteer, 0.0f);
        //}

        float a = Input.GetAxis("Vertical");
        float s = Input.GetAxis("Horizontal");
        float b = Input.GetAxis("Jump");

        //if (m_driveCmd.rb.velocity.magnitude > 1.0f || !RaceMonitor.racing)
        //{

            lastTimeMoving = Time.time;
        //}

        RaycastHit hit;
        if (Physics.Raycast(m_driveCmd.rb.gameObject.transform.position, -Vector3.up, out hit, 10))
        {

            //if (hit.collider.gameObject.tag == "road")
            //{

                lastPosition = m_driveCmd.rb.gameObject.transform.position;
                lastRotation = m_driveCmd.rb.gameObject.transform.rotation;
            //}
        }

        //if (Time.time > lastTimeMoving + 4 || m_driveCmd.rb.gameObject.transform.position.y < -5.0f)
        //{


        //    m_driveCmd.rb.gameObject.transform.position = cpm.lastCP.transform.position + Vector3.up * 2;
        //    m_driveCmd.rb.gameObject.transform.rotation = cpm.lastCP.transform.rotation;
        //    m_driveCmd.rb.gameObject.layer = 8;
        //    this.GetComponent<Ghost>().enabled = true;
        //    Invoke("ResetLayer", 3);
        //}

        //if (!RaceMonitor.racing) a = 0.0f;

        m_driveCmd.Go(a, s, b);
        m_driveCmd.CheckForSkid();
        m_driveCmd.CalculateEngineSound();
    }
}
