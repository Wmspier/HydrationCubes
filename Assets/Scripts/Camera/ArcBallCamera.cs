using UnityEngine;

public class ArcBallCamera : MonoBehaviour

{
    public Transform Target;
    public float Distance = 5.0f;
    public float XSpeed = 120.0f;
    public float YSpeed = 120.0f;
    public float YMinLimit = -20f;
    public float YMaxLimit = 80f;
    public float DistanceMin = .5f;
	public float DistanceMax = 15f;
	public bool IsDragging;

    private Rigidbody _rigidbody;
    private Vector2 _axis;

    // Use this for initialization
    private void Start()
    {
        var angles = transform.eulerAngles;
        _axis.x = angles.y;
        _axis.y = angles.x;

        _rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (_rigidbody != null)
        {
            _rigidbody.freezeRotation = true;
        }
    }

    private void LateUpdate()
    {
        if (!Target)
            return;
        if (Input.GetMouseButton(0))
		{
			RaycastHit otherHit;
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out otherHit, 2000f))
			{
                if (otherHit.transform.gameObject.tag == "Cube" && !IsDragging)
                    return;
                else
                    IsDragging = true;
			}
			else
			{
				IsDragging = true;
			}

            _axis.x += Input.GetAxis("Mouse X") * XSpeed * Distance * 0.02f;
            _axis.y -= Input.GetAxis("Mouse Y") * YSpeed * 0.02f;
            _axis.y = ClampAngle(_axis.y, YMinLimit, YMaxLimit);
        }
        else
        {
            IsDragging = false;
        }

        var rotation = Quaternion.Euler(_axis.y, _axis.x, 0);

        Distance = Mathf.Clamp(Distance - Input.GetAxis("Mouse ScrollWheel") * 5, DistanceMin, DistanceMax);

        RaycastHit hit;
        if (Physics.Linecast(Target.position, transform.position, out hit))
        {
            Distance -= hit.distance;
        }

        var negDistance = new Vector3(0.0f, 0.0f, -Distance);
        var position = rotation * negDistance + Target.position;

        transform.rotation = rotation;
        transform.position = position;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;

        if (angle > 360F)
            angle -= 360F;

        return Mathf.Clamp(angle, min, max);
    }
}