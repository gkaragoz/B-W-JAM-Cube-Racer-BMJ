using UnityEngine;

public class RescuePlayer : MonoBehaviour
{
    public void Rescue()
    {
        GameManager.Instance.ReloadGame();
        
        // Skip Welcome Screen
        // Skip MainMenu Screen
        // Open InGameUI.
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Rescue();
    }
}
