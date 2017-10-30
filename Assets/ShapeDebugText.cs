using UnityEngine;
using UnityEngine.UI;

public class ShapeDebugText : MonoBehaviour {

    private string _shape = "SquareSmall";
    private int _rotation = 0;

	void Start () {
		EventSystem.instance.Connect<GameEvents.ShapeChanged>(OnShapeChange);
        EventSystem.instance.Connect<GameEvents.ShapeRotated>(OnShapeRotated);
	}
	
    private void OnShapeChange (GameEvents.ShapeChanged e) {
        _shape = e.shape.ToString();
        GetComponent<Text>().text = "Shape: " + _shape + "\n Rotation: " + _rotation;
	}

    private void OnShapeRotated(GameEvents.ShapeRotated e)
	{
        _rotation = e.RotationState;
        GetComponent<Text>().text = "Shape: " + _shape + "\n Rotation: " + _rotation;
	}
}
