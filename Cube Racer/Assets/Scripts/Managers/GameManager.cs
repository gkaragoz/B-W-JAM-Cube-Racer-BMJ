using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static Action OnGameStart;
    public static Action OnGameFinished;
    public static Action OnGameQuit;
    public static Action OnGameReload;
    
    // 0f = Mute
    // 1f = High Volume
    public static Action<float> OnVolumeChange;

    [SerializeField] private GameObject _mapPrefab;
    
    #region Singleton

    private static GameManager _instance;

    public static GameManager Instance
    {
        get => _instance;
        set => _instance = value;
    }

    private void Awake()
    {
        if (Instance == null)
            _instance = this;
    }

    #endregion

    public void StartGame()
    {
        Instantiate(_mapPrefab);
        
        OnGameStart?.Invoke();
    }
    
    public void FinishGame()
    {
        OnGameFinished?.Invoke();
    }

    public void QuitGame()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #endif
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}