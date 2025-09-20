using UnityEngine;

public class CloseGameButton : MonoBehaviour
{
    public void CloseGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); // Close the game
    }
}
