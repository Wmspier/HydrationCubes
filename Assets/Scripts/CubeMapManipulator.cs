using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CubeManipulationShape
{
	SquareSmall,
	SquareLarge,
	LSmall,
    LLarge,
	Total
}

public class CubeMapManipulator : MonoBehaviour {
    
	public GameObject CubePrefab;
	public GameObject[,,] CubeMap;
	public Mesh DirtMesh;
    public CubeManipulationShape Shape;

    public List<CubeIndex.Point> ShapeIndices
    {
        get { return _manipulationShape; }
    }
	private List<CubeIndex.Point> _manipulationShape;
	private List<CubeIndex.Point> _originShape;
    private int _rotationState = 0;

	private static volatile CubeMapManipulator _instance;

	public static CubeMapManipulator Instance
	{
		get
		{
			if (_instance == null)
			{
                _instance = new CubeMapManipulator();
			}
			return _instance;
		}
	}

    private void Start()
    {
        _instance = this;

        SetManipulationShape(CubeManipulationShape.SquareSmall);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
            OnMouseUp();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetManipulationShape(CubeManipulationShape.SquareSmall);
            EventSystem.instance.Dispatch(new GameEvents.ShapeChanged() { shape = CubeManipulationShape.SquareSmall });
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetManipulationShape(CubeManipulationShape.SquareLarge);
            EventSystem.instance.Dispatch(new GameEvents.ShapeChanged() { shape = CubeManipulationShape.SquareLarge });
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetManipulationShape(CubeManipulationShape.LSmall);
            EventSystem.instance.Dispatch(new GameEvents.ShapeChanged() { shape = CubeManipulationShape.LSmall });
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetManipulationShape(CubeManipulationShape.LLarge);
            EventSystem.instance.Dispatch(new GameEvents.ShapeChanged() { shape = CubeManipulationShape.LLarge });
        }
        if (Input.GetKeyDown(KeyCode.Space))
		{
            RotateManipulationShape();
		}
    }

    public void RotateManipulationShape()
    {
        var newShape = _manipulationShape;
        int iter = 0;
        switch(_rotationState)
        {
            case 0:
            {
                foreach (var point in _originShape)
				{
						newShape[iter] = new CubeIndex.Point(point.Z, point.Y, point.X);
                    iter++;
				}
				break; 
            }
			case 1:
			{
				foreach (var point in _originShape)
					{
						newShape[iter] = new CubeIndex.Point(-point.X, point.Y, point.Z);
					iter++;
				}
				break;
			}
			case 2:
			{
				foreach (var point in _originShape)
					{
						newShape[iter] = new CubeIndex.Point(-point.Z, point.Y, -point.X);
					iter++;
				}
				break;
			}
		}
		_manipulationShape = newShape;
        _rotationState++;
        if (_rotationState > 2)
			_rotationState = 0;
		EventSystem.instance.Dispatch(new GameEvents.ShapeRotated() { RotationState = _rotationState });
        var fuff = "";
		foreach (var point in _manipulationShape)
		{
            fuff += "{" + point.X + "," + point.Y + "," + point.Z + "} ";
		}
        Debug.Log(fuff);
    }

    public void SetManipulationShape(CubeManipulationShape shape)
    {
        switch(shape)
        {
            case CubeManipulationShape.SquareSmall:

                _manipulationShape = new List<CubeIndex.Point> {
                    new CubeIndex.Point(0, 0, 0),
                    new CubeIndex.Point(0, 0, 1),
                    new CubeIndex.Point(1, 0, 0),
                    new CubeIndex.Point(1, 0, 1)};
                break;
            case CubeManipulationShape.SquareLarge:

				_manipulationShape = new List<CubeIndex.Point> {
					new CubeIndex.Point(0, 0, 0),
					new CubeIndex.Point(0, 0, 1),
					new CubeIndex.Point(0, 0, 2),
					new CubeIndex.Point(1, 0, 0),
					new CubeIndex.Point(2, 0, 0),
					new CubeIndex.Point(1, 0, 1),
					new CubeIndex.Point(2, 0, 2),
					new CubeIndex.Point(1, 0, 2),
					new CubeIndex.Point(2, 0, 1)};
				break;

            case CubeManipulationShape.LSmall:

				_manipulationShape = new List<CubeIndex.Point> {
					new CubeIndex.Point(0, 0, 0),
					new CubeIndex.Point(0, 0, 1),
					new CubeIndex.Point(1, 0, 0)};
				break;
			case CubeManipulationShape.LLarge:

				_manipulationShape = new List<CubeIndex.Point> {
					new CubeIndex.Point(0, 0, 0),
					new CubeIndex.Point(0, 0, 1),
					new CubeIndex.Point(0, 0, 2),
					new CubeIndex.Point(1, 0, 0),
					new CubeIndex.Point(2, 0, 0)};
				break;
		}
        _originShape = _manipulationShape;
    }

    public void CreateCubeOnTopOf(GameObject baseCube)
    {
        if (baseCube == null)
            return;
        
        var i = baseCube.GetComponent<CubeIndex>().Index;
        var container = GameObject.FindWithTag("CubeContainer");
        var cube = Instantiate(CubePrefab);
        var cubeBounds = cube.GetComponent<MeshRenderer>().bounds.max;

        var index = baseCube.GetComponent<CubeIndex>().Index;
        var position = baseCube.transform.position;
        var newRotation = cube.transform.rotation.eulerAngles;
        newRotation.y = container.transform.rotation.eulerAngles.y;
        cube.transform.eulerAngles = newRotation;                                     

        position.y += cube.GetComponent<MeshRenderer>().bounds.max.y;
        cube.transform.position = position;
        cube.transform.SetParent(container.transform);

        cube.GetComponent<CubeIndex>().Index = new CubeIndex.Point( index.X, index.Y + 1, index.Z );
        var newMap = Util.ResizeArray<GameObject>(CubeMap, CubeMap.GetLength(0), CubeMap.GetLength(1)+1, CubeMap.GetLength(2));
        CubeMap = newMap;

        CubeMap[index.X, index.Y + 1, index.Z] = cube;
		baseCube.GetComponent<MeshFilter>().mesh = DirtMesh;
	}
	public void RemoveCube(GameObject baseCube)
	{
		if (baseCube == null)
			return;
        
        baseCube.GetComponent<CubeSelection>().enabled = false;
        Destroy(baseCube);

		var newMap = Util.ResizeArray<GameObject>(CubeMap, CubeMap.GetLength(0), CubeMap.GetLength(1) - 1, CubeMap.GetLength(2));
		CubeMap = newMap;
	}

    private void OnMouseUp()
    {
		RaycastHit hit;
		var dir = transform.position - Camera.main.transform.position;
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 2000f))
		{
			if (hit.transform.gameObject.tag != "Cube")
				return;

			foreach (var point in _manipulationShape)
			{
				var selectedPoint = (hit.transform.gameObject.GetComponent<CubeIndex>().Index + point);

                if (CubeMap.GetLength(0) <= point.X || CubeMap.GetLength(1) <= point.Y || CubeMap.GetLength(2) <= point.Z)
					continue;
				if (CubeMap.GetLength(0) <= selectedPoint.X || CubeMap.GetLength(1) <= selectedPoint.Y || CubeMap.GetLength(2) <= selectedPoint.Z)
					continue;

				var selectedCube = CubeMap[selectedPoint.X, selectedPoint.Y, selectedPoint.Z];
                if(selectedCube == null)
                {
                    var yIndex = selectedPoint.Y;
                    while(yIndex >= 0 && selectedCube == null)
                    {
                        yIndex--;
                        selectedCube = CubeMap[selectedPoint.X, yIndex, selectedPoint.Z];
                    }
                }

                CreateCubeOnTopOf(selectedCube);
			}
		} 
    }
}
