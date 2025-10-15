using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gm = FindFirstObjectByType<GameManager>();
            if (gm != null)
                gm.TriggerFinish(); // <--- hier FinishReached() durch TriggerFinish() ersetzen
        }
    }
}
