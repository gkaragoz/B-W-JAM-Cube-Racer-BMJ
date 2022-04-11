using UnityEngine;

public class RescuePlayer : MonoBehaviour
{
    public void Rescue()
    {
        PlayerPrefs.SetInt("restart", 0);
        
        GameManager.Instance.ReloadGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Rescue();
    }
}
