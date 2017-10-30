using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

    public GameObject Target;

	// Update is called once per frame
	void Start () {
        transform.LookAt(Target.transform);
	}
}
