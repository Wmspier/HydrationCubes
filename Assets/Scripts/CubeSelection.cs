using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSelection : MonoBehaviour {

    private CubeIndex _index;
    private CubeIndex.Point _selectionOrigin;

    private void Start()
	{
		_index = GetComponent<CubeIndex>();
		EventSystem.instance.Connect<GameEvents.CubeSelected>(OnCubeSelected);
		EventSystem.instance.Connect<GameEvents.DeselectAllCubes>(OnDeselectAllCubes);
        EventSystem.instance.Connect<GameEvents.ShapeRotated>(OnShapeRotated);
	}

    public void SetSelected(bool selected)
	{
        if (this == null)
            return;
        
		transform.GetChild(0).gameObject.SetActive(selected);
    }

    private void OnCubeSelected( GameEvents.CubeSelected e)
	{
        _selectionOrigin = e.cube.GetComponent<CubeIndex>().Index; 
        SetSelected(WithinSelection(_selectionOrigin));
    }
    private void OnShapeRotated(GameEvents.ShapeRotated e)
	{
        if (!Input.GetMouseButton(0))
            return;
		SetSelected(WithinSelection(_selectionOrigin));
	}
    private void OnDeselectAllCubes(GameEvents.DeselectAllCubes e)
	{
        SetSelected(false);
    }
    private bool WithinSelection(CubeIndex.Point selected)
    {
        foreach (var point in CubeMapManipulator.Instance.ShapeIndices)
		{
			var selectedPoint = selected + point;
            if ((selectedPoint.X == _index.Index.X) && (selectedPoint.Z == _index.Index.Z))
                return true;
		}
        return false;
    }

    private void OnMouseUp()
    {
        EventSystem.instance.Dispatch(new GameEvents.DeselectAllCubes());
	}
    private void OnMouseDown()
	{
		EventSystem.instance.Dispatch(new GameEvents.CubeSelected() { cube = gameObject });
	}
	private void OnMouseEnter()
	{
        var ArcBall = FindObjectOfType<ArcBallCamera>();
        if (!Input.GetMouseButton(1) && !Input.GetMouseButton(0) || ArcBall.IsDragging)
    	    return;
        
        EventSystem.instance.Dispatch(new GameEvents.CubeSelected(){cube = gameObject});
	}
}
