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

    private Quaternion _targetCubeRotation;

    private void Update()
    {
        // Input
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var direction = new Vector3(horizontal, 0f, vertical);

        // Cube
        _cubeTransform.localRotation = Quaternion.Slerp(_cubeTransform.localRotation, _targetCubeRotation, _cubeRotationSpeed * Time.deltaTime);

        // Player
        Vector3 worldVector = _cubeTransform.TransformVector(direction);
        _rb.velocity = Vector3.Slerp(_rb.velocity, worldVector * _movementSpeed, _acceleration);
    }

    private void FixedUpdate()
    {
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

            var newAngleX = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.x, connectionTrigger.Angle.x);
            var newAngleY = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.y, connectionTrigger.Angle.y);
            var newAngleZ = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.z, connectionTrigger.Angle.z);

            _targetCubeRotation = Quaternion.Euler(new Vector3(newAngleX, newAngleY, newAngleZ));
        }
    }

}
