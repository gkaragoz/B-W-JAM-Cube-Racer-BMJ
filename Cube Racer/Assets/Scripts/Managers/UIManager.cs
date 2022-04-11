using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private CanvasGroup mainMenu = null;
    [SerializeField] private CanvasGroup inGameMenu = null;
    [SerializeField] private CanvasGroup settingsMenu = null;
    [SerializeField] private CanvasGroup pauseMenu = null;
    
    [SerializeField] private TextMeshProUGUI _txtLapCount;
    [SerializeField] private TextMeshProUGUI _txtCurrentTimeSpan;
    [SerializeField] private TextMeshProUGUI _txtPreviousBestTimeSpan;
    
    [SerializeField] private TextMeshProUGUI _txtPauseMenuPreviousBestTimeSpan;

    private void Start()
    {
        DOVirtual.DelayedCall(.1F, ShowMainMenu);
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
        }
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
}