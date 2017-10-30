using UnityEngine;

public interface BaseEvent{}

namespace GameEvents{
	public struct CubeSelected : BaseEvent {
        public GameObject cube;
	}
	public struct DeselectAllCubes : BaseEvent
	{
	}
	public struct ShapeChanged : BaseEvent
	{
        public CubeManipulationShape shape;
	}
	public struct ShapeRotated : BaseEvent
	{
		public int RotationState;
	}
}
