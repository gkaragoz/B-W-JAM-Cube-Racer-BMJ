using UnityEngine;

public class SideCalculator : MonoBehaviour
{
    [SerializeField] private Transform[] _sideTransforms;
    [SerializeField] private Transform _playerTransform;

    private static SideCalculator _instance;

    public static SideCalculator Instance
    {
        get
        {
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    private void Awake()
    {
        if (Instance == null)
            _instance = this;
    }

    public string Area { get; private set; }

    private void Update()
    {
        float closestDistance = Mathf.Infinity;
        int closestIndex = 0;

        for (int ii = 0; ii < _sideTransforms.Length; ii++)
        {
            var possibleDistance = Vector3.Distance(_playerTransform.position, _sideTransforms[ii].position);
            if (possibleDistance <= closestDistance)
            {
                closestDistance = possibleDistance;
                closestIndex = ii;
            }
        }

        Area = _sideTransforms[closestIndex].name;
    }
}
