using UnityEngine;

public class ConnectionTrigger : MonoBehaviour
{
    [SerializeField] private Vector3 _angleA;
    [SerializeField] private Vector3 _angleB;
    [SerializeField] private Vector3 _cornerAngleHalf;
    [SerializeField] private string _affectAxis = "Z";

    public Vector3 AngleA => _angleA;
    public Vector3 AngleB => _angleB;
    public Vector3 CornerAngleHalf => _cornerAngleHalf;
    public string AffectAxis => _affectAxis;

}
