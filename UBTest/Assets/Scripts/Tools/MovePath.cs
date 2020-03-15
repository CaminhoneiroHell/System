using UnityEngine;
using System.Collections;

public class MovePath : MonoBehaviour
{
    public EditorPath pathFollow;

    public int CurrentWayPointID = 0; // pivot
    public float speed; // velocidade d eum pivo a outro
    private float reachDistance = 1.0f; //Distancia do pivot até o proximo ponto
    public float rotationSpeed = 5.0f;
    public string pathName;

    Vector3 last_position;
    Vector3 current_position;

    void Start()
    {
        //pathFollow = GameObject.Find(pathName).GetComponent<EditorPath>();
        last_position = transform.position;
    }

    private void Update()
    {
        float distance = Vector3.Distance(pathFollow.path_objs[CurrentWayPointID].position,
            transform.position);
        transform.position = Vector3.MoveTowards(transform.position, pathFollow.path_objs[CurrentWayPointID].position,
            Time.deltaTime * speed);

        var rotation = Quaternion.LookRotation(pathFollow.path_objs[CurrentWayPointID].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        if(distance <= reachDistance)
        {
            CurrentWayPointID++;
        }

        if(CurrentWayPointID >= pathFollow.path_objs.Count)
        {
            CurrentWayPointID = 0;
        }
    }
}
