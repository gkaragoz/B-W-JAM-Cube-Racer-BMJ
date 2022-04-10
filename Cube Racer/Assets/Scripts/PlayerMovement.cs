using MVC.Base.Runtime.Extensions;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _cubeTransform;

    [SerializeField] private float _movementSpeed = 1f;
    [SerializeField] private float _acceleration = 2f;
    [SerializeField] private Rigidbody _rb = null;

    [SerializeField] private float _maxSpeed = 5f;

    [Header("Cube Properties")]
    [SerializeField] private float _cubeRotationSpeed = 2f;
    [SerializeField] private float _cubeRotationThreshold = 0.01f;
    private float _previousDistance = 0f;

    private Quaternion _targetCubeRotation;
    private Vector3 _inputDirection;

    private void Update()
    {
        // Input
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        _inputDirection = new Vector3(horizontal, 0f, vertical);

        // Player rotation reset.
        transform.rotation = Quaternion.Euler(Vector3.zero);

        // Cube
        _cubeTransform.localRotation = Quaternion.Slerp(_cubeTransform.localRotation, _targetCubeRotation, _cubeRotationSpeed);
    }

    private void FixedUpdate()
    {
        // Player
        Vector3 worldVector = _cubeTransform.TransformVector(_inputDirection);
        _rb.velocity = worldVector * _movementSpeed;

        if (_rb.velocity.magnitude > _maxSpeed)
            _rb.velocity = _rb.velocity.normalized * _maxSpeed;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Connection")) {
            var connectionTrigger = other.GetComponent<ConnectionTrigger>();

            var distance = Vector3.zero;
            if (connectionTrigger.AffectAxis == "X")
                distance = other.transform.position.WithX(0) - transform.position.WithX(0);
            else if (connectionTrigger.AffectAxis == "Y")
                distance = other.transform.position.WithY(0) - transform.position.WithY(0);
            else if (connectionTrigger.AffectAxis == "Z")
                distance = other.transform.position.WithZ(0) - transform.position.WithZ(0);

            var minValue = 0.5f;
            var maxValue = 0f;

            var distanceMagnitude = Mathf.Clamp(distance.magnitude, maxValue, minValue);

            if (Mathf.Abs(_previousDistance - distance.magnitude) < _cubeRotationThreshold)
                return;

            _previousDistance = distance.magnitude;

            var newAngleX = 0f;
            var newAngleY = 0f;
            var newAngleZ = 0f;

            if (SideCalculator.Instance.Area == "F_Center")
            {
                newAngleX = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.x, connectionTrigger.CornerAnglePart2.x);
                newAngleY = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.y, connectionTrigger.CornerAnglePart2.y);
                newAngleZ = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.z, connectionTrigger.CornerAnglePart2.z);
            }
            else if (SideCalculator.Instance.Area == "C_Center")
            {
                newAngleX = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.x, connectionTrigger.CornerAnglePart1.x);
                newAngleY = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.y, connectionTrigger.CornerAnglePart1.y);
                newAngleZ = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.z, connectionTrigger.CornerAnglePart1.z);
            }

            newAngleX = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.x, connectionTrigger.CornerAnglePart1.x);
            newAngleY = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.y, connectionTrigger.CornerAnglePart1.y);
            newAngleZ = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.z, connectionTrigger.CornerAnglePart1.z);

            _targetCubeRotation = Quaternion.Euler(new Vector3(newAngleX, newAngleY, newAngleZ));
        }
    }

}
