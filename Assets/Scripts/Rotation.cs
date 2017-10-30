using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

    public float RotationSpeed = 5f;
    private Vector3 _origin;

    private void Start()
    {
        _origin = transform.position;
    }

	// Update is called once per frame
	void Update()
	{
        //transform.RotateAround(_origin, Vector3.up, 20 * Time.deltaTime);
        //return;

        var rotationMod = Vector3.zero;
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rotationMod.y -= RotationSpeed;
        }
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{

			rotationMod.y += RotationSpeed;
		}

        if (Mathf.Abs(rotationMod.y)>0)
        {
            transform.Rotate(0,rotationMod.y, 0, Space.Self);
        }
	}
}
