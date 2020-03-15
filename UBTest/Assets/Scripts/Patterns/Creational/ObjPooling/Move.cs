using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    public Vector3 velocity;
    public AudioSource exp;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "asteroid")
        {
            collision.gameObject.SetActive(false);
            exp.Play();
        }
    }

    // Update is called once per frame
    void Update () {
        this.transform.Translate(velocity);
	}
}
