using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel; // Panel des Menüs
    [SerializeField] private TextMeshProUGUI highscoreText; // Highscore-Text

    void Start()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true); // Menü beim Start aktiv

        UpdateHighscore();
    }

    // Start Game – hier wird die Szene einfach vom Button selbst geladen
    public void StartGame()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        Time.timeScale = 1f; // Zeit sicher starten
    }

    // Quit Game
    public void QuitGame()
    {
        Debug.Log("Quit Game"); // sichtbar im Editor
        Application.Quit(); // Im Build beendet es das Spiel
    }

    private void UpdateHighscore()
    {
        int highscore = PlayerPrefs.GetInt("Highscore", 0);
        if (highscoreText != null)
            highscoreText.text = "Highscore: " + highscore;
    }
}
