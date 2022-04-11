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
        
        _currentMap.PreviousTimeMinutes = PlayerPrefs.GetInt(PreviousTimeMinutes);
        _currentMap.PreviousTimeMinutes = PlayerPrefs.GetInt(PreviousTimeSeconds);
        _currentMap.PreviousTimeMinutes = PlayerPrefs.GetInt(PreviousTimeMilliseconds);
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
        Debug.LogWarning("Previous time: " + $"{_currentMap.PreviousTimeMinutes}:{_currentMap.PreviousTimeMinutes}:{_currentMap.PreviousTimeMinutes}");
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

        _currentMap.PreviousTimeMinutes = passedTimeInMinutes;
        _currentMap.PreviousTimeSeconds = passedTimeInSeconds;
        _currentMap.PreviousTimeMilliseconds = passedTimeInMilliseconds;

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
        return $"{_currentMap.CurrentTimeMinutes}:{_currentMap.CurrentTimeSeconds}:{_currentMap.CurrentTimeMilliseconds}";
    }

    public string GetPreviousTimeSpan()
    {
        return $"{_currentMap.PreviousTimeMinutes}:{_currentMap.PreviousTimeSeconds}:{_currentMap.PreviousTimeMilliseconds}";
    }
}

[Serializable]
public class MapData
{
    public string MapName;
    public int LapCount;
    public int CurrentLap;

    public int PreviousTimeMinutes;
    public int PreviousTimeSeconds;
    public int PreviousTimeMilliseconds;

    public int CurrentTimeMinutes;
    public int CurrentTimeSeconds;
    public int CurrentTimeMilliseconds;
}