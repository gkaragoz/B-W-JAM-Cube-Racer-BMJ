using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LapChecker : MonoBehaviour
{
    private static LapChecker _instance;

    public static LapChecker Instance
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

    [SerializeField] private int _lapCount = 0;
    [SerializeField] private int _currentLap = 0;

    [Header("AutoFill in Gameplay")]
    [SerializeField] private List<string> _lapCheckpointSides = new List<string>();

    private void Start()
    {
        _currentLap = 0;

        SetAllCheckpoints();
    }

    private void Update()
    {
        if (SideCalculator.Instance == null)
            return;
        
        var playerCurrentSide = SideCalculator.Instance.Area;

        PassASide(playerCurrentSide);
    }

    public override string ToString()
    {
        return $"LAP {_currentLap}/{_lapCount}";
    }

    public void SetAllCheckpoints()
    {
        var sideObjects = GameObject.FindGameObjectsWithTag("Side");

        _lapCheckpointSides = new List<string>();

        foreach (var item in sideObjects)
            _lapCheckpointSides.Add(item.name);

        _lapCheckpointSides.Add("StartFinishArea");
    }

    public void CompleteLap()
    {
        Debug.LogWarning("Lap Completed");

        _currentLap++;

        if (_currentLap == _lapCount)
        {
            TimeTracker.Instance.FinishTimer();
            Debug.LogWarning("Level Finished");
            SoundManager.Instance.PlayCompleteSFX();
            UIManager.Instance.ShowCompleteScreen();
        }
        else
        {
            SetAllCheckpoints();
        }
    }

    public void PassASide(string side)
    {
        if (!_lapCheckpointSides.Contains(side))
            return;

        _lapCheckpointSides.Remove(side);

        if (_lapCheckpointSides.Count == 0)
            CompleteLap();
    }

    public void ReachToFinish(string finishSide)
    {
        if (_lapCheckpointSides.Count == 1)
        {
            _lapCheckpointSides.Remove(finishSide);

            CompleteLap();
        }
    }
}
