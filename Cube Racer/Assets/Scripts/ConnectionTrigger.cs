using UnityEngine;

public class ConnectionTrigger : MonoBehaviour
{
    [SerializeField] private Vector3 _angleA;
    [SerializeField] private Vector3 _angleB;
    [SerializeField] private Vector3 _cornerAnglePart1;
    [SerializeField] private Vector3 _cornerAnglePart2;
    [SerializeField] private string _affectAxis = "Z";

    public Vector3 AngleA => _angleA;
    public Vector3 AngleB => _angleB;
    public Vector3 CornerAnglePart1 => _cornerAnglePart1;
    public Vector3 CornerAnglePart2 => _cornerAnglePart2;
    public string AffectAxis => _affectAxis;

}
