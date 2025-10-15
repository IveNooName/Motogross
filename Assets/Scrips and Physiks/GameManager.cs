using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private Transform player;
    private Vector3 startPosition;
    private int score = 0;

    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI gameOverText;
    private TextMeshProUGUI highscoreText;

    [Header("Finish Panel")]
    [SerializeField] private GameObject finishPanel;

    private bool isGameOver = false;
    private bool isFinished = false;

    void Start()
    {
        // 🔎 Player automatisch finden
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            startPosition = player.position;
        }
        else
        {
            Debug.LogError("⚠ Kein Objekt mit Tag 'Player' gefunden!");
        }

        // 🔎 ScoreText
        scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        if (scoreText == null) Debug.LogError("⚠ ScoreText nicht gefunden!");

        // 🔎 GameOverText (TMP) auch wenn deaktiviert
        gameOverText = FindInActiveObjectByName("GameOverText")?.GetComponent<TextMeshProUGUI>();
        if (gameOverText != null)
            gameOverText.enabled = false;
        else
            Debug.LogWarning("⚠ GameOverText nicht gefunden!");

        // 🔎 FinishPanel
        if (finishPanel == null)
            finishPanel = FindInActiveObjectByName("FinishPanel");

        if (finishPanel != null)
            finishPanel.SetActive(false);
        else
            Debug.LogWarning("⚠ FinishPanel nicht gefunden!");

        // 🔎 HighscoreText
        highscoreText = GameObject.Find("HighscoreText")?.GetComponent<TextMeshProUGUI>();
        if (highscoreText != null)
        {
            int savedHighscore = PlayerPrefs.GetInt("Highscore", 0);
            highscoreText.text = "Highscore: " + savedHighscore;
        }
    }

    void Update()
    {
        if (!isGameOver && !isFinished && player != null)
        {
            // Score berechnen
            float distance = player.position.x - startPosition.x;
            score = Mathf.Max(0, Mathf.RoundToInt(distance));

            if (scoreText != null)
                scoreText.text = "Score: " + score;
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        if (gameOverText != null)
            gameOverText.enabled = true;

        UpdateHighscore();
        Time.timeScale = 0f;
    }

    public void TriggerFinish()
    {
        if (isFinished) return;

        isFinished = true;
        if (finishPanel != null)
            finishPanel.SetActive(true);

        UpdateHighscore();
        Time.timeScale = 0f;
    }

    private void UpdateHighscore()
    {
        int savedHighscore = PlayerPrefs.GetInt("Highscore", 0);
        if (score > savedHighscore)
        {
            PlayerPrefs.SetInt("Highscore", score);
            PlayerPrefs.Save();

            if (highscoreText != null)
                highscoreText.text = "Highscore: " + score;
        }
    }

    // Sucht auch deaktivierte Objekte
    private GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform obj in objs)
        {
            if (obj.hideFlags == HideFlags.None && obj.name == name)
                return obj.gameObject;
        }
        return null;
    }
}
