using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private CanvasGroup welcomeMenu = null;
    [SerializeField] private CanvasGroup mainMenu = null;
    [SerializeField] private CanvasGroup inGameMenu = null;
    [SerializeField] private CanvasGroup settingsMenu = null;
    [SerializeField] private CanvasGroup pauseMenu = null;
    [SerializeField] private CanvasGroup keyBindingsMenu = null;
    [SerializeField] private CanvasGroup levelCompleteMenu = null;
    
    [SerializeField] private TextMeshProUGUI _txtLapCount;
    [SerializeField] private TextMeshProUGUI _txtCurrentTimeSpan;
    [SerializeField] private TextMeshProUGUI _txtPreviousBestTimeSpan;
    [SerializeField] private TextMeshProUGUI _txtPauseMenuPreviousBestTimeSpan;    
    [SerializeField] private TextMeshProUGUI _txtCurrentTimeSpanForLevelComplete;
    [SerializeField] private TextMeshProUGUI _txtPreviousBestSpanForLevelComplete;

    private void Start()
    {
        if (PlayerPrefs.HasKey("restart"))
        {
            PlayerPrefs.DeleteKey("restart");
            
            StartDirectTransition();
            
            GameManager.Instance.StartGame();

            TimeTracker.Instance.OnGameStartedListener();
        }

        else
        {
            DOVirtual.DelayedCall(2F, () =>
            {
                HideWelcomeScreen();
            
                ShowMainMenu();
            });
        }
    }

    private void Update()
    {
        if (LapChecker.Instance != null)
        {
            _txtLapCount.text = LapChecker.Instance.ToString();
        }

        if (TimeTracker.Instance != null)
        {
            _txtCurrentTimeSpan.text = TimeTracker.Instance.GetCurrentTimeSpan();
            _txtPreviousBestTimeSpan.text = TimeTracker.Instance.GetPreviousTimeSpan();
            _txtPauseMenuPreviousBestTimeSpan.text = "PREVIOUS BEST: " + TimeTracker.Instance.GetPreviousTimeSpan();
            _txtCurrentTimeSpanForLevelComplete.text = TimeTracker.Instance.GetCurrentTimeSpan();
            _txtPreviousBestSpanForLevelComplete.text = "PREVIOUS BEST: " + TimeTracker.Instance.GetPreviousTimeSpan();
        }
    }

    public void ShowCompleteScreen()
    {
        DOTween.To(() => levelCompleteMenu.alpha,
            x => levelCompleteMenu.alpha = x, 1F, .5F)
            .OnComplete(()=> levelCompleteMenu.blocksRaycasts = true);
    }

    public void HideCompleteScreen()
    {
        DOTween.To(() => levelCompleteMenu.alpha,
            x => levelCompleteMenu.alpha = x, 0F, .5F);
    }
    
    private void ShowWelcomeScreen()
    {
        DOTween.To(() => welcomeMenu.alpha,
            x => welcomeMenu.alpha = x, 1F, 1F);
    }
    
    private void HideWelcomeScreen()
    {
        DOTween.To(() => welcomeMenu.alpha,
            x => welcomeMenu.alpha = x, 0F, 1F);
    }

    private void ShowMainMenu()
    {
        DOTween.To(() => mainMenu.alpha,
            x => mainMenu.alpha = x, 1F, .5F)
            .OnComplete(()=> mainMenu.blocksRaycasts = true);
    }
    
    public void HideMainMenu()
    {
        mainMenu.blocksRaycasts = false;
        
        DOTween.To(() => mainMenu.alpha,
            x => mainMenu.alpha = x, 0F, .5F);
    }

    public void ShowInGameMenu()
    {
        DOTween.To(() => inGameMenu.alpha, 
                x => inGameMenu.alpha = x, 1F, .5F)
            .OnComplete(() => inGameMenu.blocksRaycasts = true);
        
        SoundManager.Instance.PlayRandomRaceSFX();
    }

    public void HideInGameMenu()
    {
        inGameMenu.blocksRaycasts = false;

        DOTween.To(() => inGameMenu.alpha,
            x => inGameMenu.alpha = x, 0F, .5F);
    }

    public void ShowPauseMenu()
    {
        DOTween.To(() => pauseMenu.alpha, 
                x => pauseMenu.alpha = x, 1F, .5F).
            OnComplete(()=> pauseMenu.blocksRaycasts = true);
        
        SoundManager.Instance.PlayMenuSFX();
    }

    public void HidePauseMenu()
    {
        pauseMenu.blocksRaycasts = false;

        DOTween.To(() => pauseMenu.alpha,
            x => pauseMenu.alpha = x, 0F, .5F);
    }

    public void OnClickSettings()
    {
        mainMenu.blocksRaycasts = false;

        DOTween.To(() => settingsMenu.alpha, 
            x => settingsMenu.alpha = x, 1F, .5F).OnComplete(() =>
        {
            settingsMenu.blocksRaycasts = true;
        });
    }

    public void SettingsToMainMenu()
    {
        settingsMenu.blocksRaycasts = false;

        DOTween.To(() => settingsMenu.alpha,
            x => settingsMenu.alpha = x, 0F, .5F);
        
        ShowMainMenu();
    }

    public void PauseToSettings()
    {
        pauseMenu.blocksRaycasts = false;
        
        DOTween.To(() => settingsMenu.alpha, 
            x => settingsMenu.alpha = x, 1F, .5F).OnComplete(() =>
        {
            settingsMenu.blocksRaycasts = true;
        });
    }

    public void PauseToMainMenu()
    {
        GameManager.Instance.ReloadGame();
    }

    public void ShowKeyBindingsMenu()
    {
        DOTween.To(() => keyBindingsMenu.alpha, 
                x => keyBindingsMenu.alpha = x, 1F, .5F).
            OnComplete(()=> keyBindingsMenu.blocksRaycasts = true);
    }

    public void HideKeyBindingsMenu()
    {
        keyBindingsMenu.blocksRaycasts = false;

        DOTween.To(() => keyBindingsMenu.alpha,
            x => keyBindingsMenu.alpha = x, 0F, .5F);
    }
    
    public void StartDirectTransition()
    {
        welcomeMenu.alpha = 0F;
        welcomeMenu.blocksRaycasts = false;
        mainMenu.alpha = 0F;
        mainMenu.blocksRaycasts = false;
        ShowInGameMenu();
    }
}