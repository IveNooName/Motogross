using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private float autoReturnTime = 10f; // nach wie vielen Sekunden zurück zum Menü
    [SerializeField] private string mainMenuSceneName = "MainMenu"; 

    void Start()
    {
        // Nach X Sekunden automatisch ins Hauptmenü zurück
        Invoke(nameof(ReturnToMainMenu), autoReturnTime);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
