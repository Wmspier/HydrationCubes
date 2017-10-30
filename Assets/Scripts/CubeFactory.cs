using UnityEngine;
using System.Collections.Generic;

public class CubeFactory : MonoBehaviour {

    public GameObject CubePrefab;
    public Mesh DirtMesh;

    public int Width, Length, Height;

    public GameObject[,,] CubeMap;

    private void Start()
    {
        GenerateBasicLevel(Width, Length, Height);
    }

	private void GenerateBasicLevel(int width, int length, int height)
    {
        CubeMap = new GameObject[width,height,length];
        var cubeList = new List<GameObject>();

		var container = GameObject.FindWithTag("CubeContainer");
        for (var i = 0; i < height;i++)
		{
			for (var j = 0; j < width; j++)
            {
				for (var k = 0; k < length; k++)
				{
					var cube = Instantiate(CubePrefab);
					var cubeBounds = cube.GetComponent<MeshRenderer>().bounds.max;
					var cubePosition = transform.position;

					cubePosition.x += j * (cubeBounds.x * 2);
					cubePosition.y += i * (cubeBounds.y);
					cubePosition.z += k * (cubeBounds.z * 2);
					cube.transform.position = cubePosition;
					cubeList.Add(cube);

					if (i == height / 2 && j == width / 2 && k == length / 2)
						container.transform.position = cubePosition;

					CubeMap[j, i, k] = cube;

					if (i < height - 1)
						cube.GetComponent<MeshFilter>().mesh = DirtMesh;

                    cube.GetComponent<CubeIndex>().Index = new CubeIndex.Point(j,i,k);
				}
            }
        }
        foreach (var cube in cubeList)
            cube.transform.SetParent(container.transform);
        
        CubeMapManipulator.Instance.CubeMap = CubeMap;
    }

    private void PositionCamera(int width, int length, int height)
    {
        var cubeBounds = CubePrefab.GetComponent<MeshRenderer>().bounds.max.x;

        var position = transform.position;
        position.x += width * 3.5f * cubeBounds;
        position.y += height * 9 * cubeBounds;
        position.z += length * 3.5f * cubeBounds;
        Camera.main.transform.position = position;
    }
}
