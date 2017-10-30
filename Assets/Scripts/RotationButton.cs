using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationButton : Button {

    public Vector3 Direction;
    public Transform Target;

    private void Update()
    {
        if(currentSelectionState == SelectionState.Pressed)
		{
			Debug.Log("ugghh");
			Target.Rotate(Direction.x, Direction.y, Direction.z, Space.Self);
        }
    }
}
