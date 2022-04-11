using System;
using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    private static TimeTracker _instance;

    public static TimeTracker Instance
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

    [SerializeField] private MapData _currentMap;
    private bool _isStarted = false;

    private DateTime _startDateTime;

    private const string PreviousTimeMinutes = "PREVIOUS_TIME_MINUTES";
    private const string PreviousTimeSeconds = "PREVIOUS_TIME_SECONDS";
    private const string PreviousTimeMilliseconds = "PREVIOUS_TIME_MILLISECONDS";

    private void Start()
    {
        GameManager.OnGameStart += OnGameStartedListener;
    }

    private void OnGameStartedListener()
    {
        StartTimer();
    }

    [ContextMenu("Start Timer")]
    public void StartTimer()
    {
        _isStarted = true;
        _startDateTime = DateTime.Now;

        Debug.LogWarning("Timer started.");
    }

    private void Update()
    {
        if (!_isStarted)
            return;
        
        var passedTimeInMinutes = GetTimeInMinutes();
        var passedTimeInSeconds = GetTimeInSeconds();
        var passedTimeInMilliseconds = GetTimeInMilliseconds();

        _currentMap.CurrentTimeMinutes = passedTimeInMinutes;
        _currentMap.CurrentTimeSeconds = passedTimeInSeconds;
        _currentMap.CurrentTimeMilliseconds = passedTimeInMilliseconds;
    }

    private int GetTimeInMinutes()
    {
        return (DateTime.Now - _startDateTime).Minutes;
    }

    private int GetTimeInSeconds()
    {
        return (DateTime.Now - _startDateTime).Seconds;
    }

    private int GetTimeInMilliseconds()
    {
        return (DateTime.Now - _startDateTime).Milliseconds;
    }

    [ContextMenu("Finish Timer")]
    public void FinishTimer()
    {
        _isStarted = false;
        
        var passedTimeInMinutes = GetTimeInMinutes();
        var passedTimeInSeconds = GetTimeInSeconds();
        var passedTimeInMilliseconds = GetTimeInMilliseconds();

        _currentMap.CurrentTimeMinutes = passedTimeInMinutes;
        _currentMap.CurrentTimeSeconds = passedTimeInSeconds;
        _currentMap.CurrentTimeMilliseconds = passedTimeInMilliseconds;

        PlayerPrefs.SetInt(PreviousTimeMinutes, passedTimeInMinutes);
        PlayerPrefs.SetInt(PreviousTimeSeconds, passedTimeInSeconds);
        PlayerPrefs.SetInt(PreviousTimeMilliseconds, passedTimeInMilliseconds);

        Debug.LogWarning("Timer finished");
        Debug.LogWarning($"{passedTimeInMinutes}:{passedTimeInSeconds}:{passedTimeInMilliseconds}");

        GameManager.Instance.FinishGame();
    }
    
    public string GetCurrentTimeSpan()
    {
        return $"{_currentMap.CurrentTimeMinutes:00}:{_currentMap.CurrentTimeSeconds:00}:{_currentMap.CurrentTimeMilliseconds:00}";
    }

    public string GetPreviousTimeSpan()
    {
        return $"{PlayerPrefs.GetInt(PreviousTimeMinutes):00}:{PlayerPrefs.GetInt(PreviousTimeSeconds):00}:{PlayerPrefs.GetInt(PreviousTimeMilliseconds):00}";
    }
}

[Serializable]
public class MapData
{
    public string MapName;
    public int LapCount;
    public int CurrentLap;

    public int CurrentTimeMinutes;
    public int CurrentTimeSeconds;
    public int CurrentTimeMilliseconds;
}