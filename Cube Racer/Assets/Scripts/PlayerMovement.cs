using MVC.Base.Runtime.Extensions;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _cubeTransform;

    [SerializeField] private float _movementSpeed = 1f;
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

        if (SideCalculator.Instance.Area == "C_Center")
            _inputDirection = new Vector3(horizontal, 0f, vertical);
        else if (SideCalculator.Instance.Area == "F_Center")
            _inputDirection = new Vector3(0f, -1 * horizontal, vertical);
        else if (SideCalculator.Instance.Area == "B_Center")
            _inputDirection = new Vector3(horizontal, vertical, 0f);
        else if (SideCalculator.Instance.Area == "D_Center")
            _inputDirection = new Vector3(-1 * vertical, -1 * horizontal, 0f);
        else if (SideCalculator.Instance.Area == "E_Center")
            _inputDirection = new Vector3(0f, vertical, -1 * horizontal);
        else if (SideCalculator.Instance.Area == "A_Center")
            _inputDirection = new Vector3(-1 * vertical, 0f, -1 * horizontal);

        // Player rotation reset.
        transform.rotation = Quaternion.Euler(Vector3.zero);

        //transform.localPosition += _inputDirection * _movementSpeed * Time.deltaTime;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("StartFinishArea"))
            LapChecker.Instance.ReachToFinish("StartFinishArea");
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
            else if (connectionTrigger.AffectAxis == "YZ")
                distance = other.transform.position.With(other.transform.position.x, 0f, 0f) - transform.position.With(transform.position.x, 0f, 0f);
            else if (connectionTrigger.AffectAxis == "YX")
                distance = other.transform.position.With(0f, 0f, other.transform.position.z) - transform.position.With(0f, 0f, transform.position.z);

            var minValue = 0.5f;
            var maxValue = 0f;

            var distanceMagnitude = Mathf.Clamp(distance.magnitude, maxValue, minValue);

            if (Mathf.Abs(_previousDistance - distanceMagnitude) < _cubeRotationThreshold)
                return;

            _previousDistance = distance.magnitude;

            var newAngleX = 0f;
            var newAngleY = 0f;
            var newAngleZ = 0f;

            if (SideCalculator.Instance.Area == "C_Center" && (connectionTrigger.ToSide == "F_Center" || connectionTrigger.ToSide == "B_Center"))
            {
                minValue = 0.5f;
                maxValue = 0f;

                newAngleX = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.x, connectionTrigger.CornerAngleHalf.x);
                newAngleY = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.y, connectionTrigger.CornerAngleHalf.y);
                newAngleZ = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.z, connectionTrigger.CornerAngleHalf.z);
            }
            else if (SideCalculator.Instance.Area == "F_Center" && connectionTrigger.ToSide == "C_Center")
            {
                minValue = 0f;
                maxValue = 0.5f;

                newAngleX = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.CornerAngleHalf.x, connectionTrigger.AngleB.x);
                newAngleY = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.CornerAngleHalf.y, connectionTrigger.AngleB.y);
                newAngleZ = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.CornerAngleHalf.z, connectionTrigger.AngleB.z);
            }
            else if (SideCalculator.Instance.Area == "F_Center" && connectionTrigger.ToSide == "D_Center")
            {
                minValue = 0.5f;
                maxValue = 0f;

                newAngleX = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.x, connectionTrigger.CornerAngleHalf.x);
                newAngleY = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.y, connectionTrigger.CornerAngleHalf.y);
                newAngleZ = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.z, connectionTrigger.CornerAngleHalf.z);
            }
            else if (SideCalculator.Instance.Area == "D_Center" && connectionTrigger.ToSide == "F_Center")
            {
                minValue = 0f;
                maxValue = 0.5f;

                newAngleX = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.CornerAngleHalf.x, connectionTrigger.AngleB.x);
                newAngleY = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.CornerAngleHalf.y, connectionTrigger.AngleB.y);
                newAngleZ = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.CornerAngleHalf.z, connectionTrigger.AngleB.z);
            }
            else if (SideCalculator.Instance.Area == "B_Center" && connectionTrigger.ToSide == "E_Center")
            {
                minValue = 0.5f;
                maxValue = 0f;

                newAngleX = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.x, connectionTrigger.CornerAngleHalf.x);
                newAngleY = -90f;
                newAngleZ = -90f;
            }
            else if (SideCalculator.Instance.Area == "B_Center" && connectionTrigger.ToSide == "C_Center")
            {
                minValue = 0f;
                maxValue = 0.5f;

                newAngleX = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.CornerAngleHalf.x, connectionTrigger.AngleB.x);
                newAngleY = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.CornerAngleHalf.y, connectionTrigger.AngleB.y);
                newAngleZ = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.CornerAngleHalf.z, connectionTrigger.AngleB.z);
            }
            else if (SideCalculator.Instance.Area == "E_Center" && connectionTrigger.ToSide == "B_Center")
            {
                minValue = 0f;
                maxValue = 0.5f;

                newAngleX = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.CornerAngleHalf.x, connectionTrigger.AngleB.x);
                newAngleY = -90f;
                newAngleZ = -90f;
            }
            else if (SideCalculator.Instance.Area == "E_Center" && connectionTrigger.ToSide == "A_Center")
            {
                minValue = 0.5f;
                maxValue = 0f;

                newAngleX = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.x, connectionTrigger.CornerAngleHalf.x);
                newAngleY = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.y, connectionTrigger.CornerAngleHalf.y);
                newAngleZ = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.z, connectionTrigger.CornerAngleHalf.z);
            }
            else if (SideCalculator.Instance.Area == "A_Center" && connectionTrigger.ToSide == "E_Center")
            {
                minValue = 0f;
                maxValue = 0.5f;

                newAngleX = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.CornerAngleHalf.x, connectionTrigger.AngleB.x);
                newAngleY = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.CornerAngleHalf.y, connectionTrigger.AngleB.y);
                newAngleZ = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.CornerAngleHalf.z, connectionTrigger.AngleB.z);
            }
            else if (SideCalculator.Instance.Area == "A_Center" && connectionTrigger.ToSide == "D_Center")
            {
                minValue = 0.5f;
                maxValue = 0f;

                newAngleX = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.x, connectionTrigger.CornerAngleHalf.x);
                newAngleY = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.y, connectionTrigger.CornerAngleHalf.y);
                newAngleZ = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.AngleA.z, connectionTrigger.CornerAngleHalf.z);
            }
            else if (SideCalculator.Instance.Area == "D_Center" && connectionTrigger.ToSide == "A_Center")
            {
                minValue = 0f;
                maxValue = 0.5f;

                newAngleX = ExtensionMethods.Map(distanceMagnitude, minValue, maxValue, connectionTrigger.CornerAngleHalf.x, connectionTrigger.AngleB.x);
                newAngleY = -90f;
                newAngleZ = -180f;
            }

            _targetCubeRotation = Quaternion.Euler(new Vector3(newAngleX, newAngleY, newAngleZ));
        }
    }

}
