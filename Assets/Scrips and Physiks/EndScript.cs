using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScript : MonoBehaviour
{
    [SerializeField] private string creditsSceneName = "CreditsScene"; // Name der Szene, die geladen wird

    // Für Trigger (Is Trigger = true)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LoadCredits();
        }
    }

    // Für normale Collision (Is Trigger = false)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            LoadCredits();
        }
    }

    private void LoadCredits()
    {
        SceneManager.LoadScene(creditsSceneName);
    }
}
