using System;

public class GameManager : Singleton<GameManager>
{
    public static Action OnGameStart;
    public static Action OnGameQuit;
    public static Action OnGameReload;
    
    // 0f = Mute
    // 1f = High Volume
    public static Action<float> OnVolumeChange;

    public void StartGame()
    {
        OnGameStart?.Invoke();
    }

    public void QuitGame()
    {
        OnGameQuit?.Invoke();
    }
}