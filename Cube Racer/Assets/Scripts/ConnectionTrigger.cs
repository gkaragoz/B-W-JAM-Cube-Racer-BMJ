using UnityEngine;

public class ConnectionTrigger : MonoBehaviour
{
    [SerializeField] private Vector3 _angleA;
    [SerializeField] private Vector3 _cornerAngleHalf;
    [SerializeField] private Vector3 _angleB;
    [SerializeField] private string _affectAxis = "Z";
    [SerializeField] private string _sideA = "";
    [SerializeField] private string _sideB = "";

    public Vector3 AngleA => _angleA;
    public Vector3 CornerAngleHalf => _cornerAngleHalf;
    public Vector3 AngleB => _angleB;
    public string AffectAxis => _affectAxis;
    public string ToSide => SideCalculator.Instance.Area == _sideA ? _sideB : _sideA;

}
