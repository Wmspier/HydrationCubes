using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util {

	public static T[,,] ResizeArray<T>(T[,,] original, int xSize, int ySize, int zSize)
	{
		var newArray = new T[xSize, ySize, zSize];
		var xMin = Mathf.Min(xSize, original.GetLength(0));
		var yMin = Mathf.Min(ySize, original.GetLength(1));
		var zMin = Mathf.Min(zSize, original.GetLength(2));
		for (var x = 0; x < xMin; x++)
			for (var y = 0; y < yMin; y++)
				for (var z = 0; z < zMin; z++)
					newArray[x, y, z] = original[x, y, z];
		return newArray;
	}
}
