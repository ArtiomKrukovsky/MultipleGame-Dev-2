using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anim : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.D))
        {
            gameObject.GetComponent<Animator>().SetTrigger("right");
        }
        else {
            gameObject.GetComponent<Animator>().SetTrigger("idle");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            gameObject.GetComponent<Animator>().SetTrigger("left");
        }
        else
        {
            gameObject.GetComponent<Animator>().SetTrigger("idle");
        }

	}
}
