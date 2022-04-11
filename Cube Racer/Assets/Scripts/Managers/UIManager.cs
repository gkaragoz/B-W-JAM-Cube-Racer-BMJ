using DG.Tweening;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private CanvasGroup mainMenu = null;
    [SerializeField] private CanvasGroup settingsMenu = null;
    [SerializeField] private CanvasGroup pauseMenu = null;

    private void Start()
    {
        DOVirtual.DelayedCall(.1F, ShowMainMenu);
    }

    private void ShowMainMenu()
    {
        DOTween.To(() => mainMenu.alpha, 
            x => mainMenu.alpha = x, 1F, 1F);
    }
    
    public void HideMainMenu()
    {
        DOVirtual.DelayedCall(.1F, () =>
        {
            DOTween.To(() => mainMenu.alpha, 
                x => mainMenu.alpha = x, 0F, .5F);
        });
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
        DOTween.To(() => settingsMenu.alpha, 
            x => settingsMenu.alpha = x, 1F, .5F).OnComplete(() =>
        {
            mainMenu.blocksRaycasts = false;
            settingsMenu.blocksRaycasts = true;
        });
    }

    public void SettingsToMainMenu()
    {
        DOTween.To(() => settingsMenu.alpha, 
            x => settingsMenu.alpha = x, 0F, .5F).OnComplete(() =>
        {
            mainMenu.blocksRaycasts = true;
            settingsMenu.blocksRaycasts = false;
        });
    }

    public void PauseToSettings()
    {
        DOTween.To(() => settingsMenu.alpha, 
            x => settingsMenu.alpha = x, 1F, .5F).OnComplete(() =>
        {
            settingsMenu.blocksRaycasts = true;
        });
    }

    public void PauseToMainMenu()
    {
        GameManager.OnGameReload?.Invoke();
    }
}