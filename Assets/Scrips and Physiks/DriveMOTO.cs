using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DriveMotoArcade : MonoBehaviour
{
    [SerializeField] private Rigidbody2D Frame;
    [SerializeField] private Rigidbody2D frontWheel;
    [SerializeField] private Rigidbody2D rearWheel;

    [Header("Speed Settings")]
    [SerializeField] private float maxSpeed = 150f;       
    [SerializeField] private float acceleration = 200f;   
    [SerializeField] private float deceleration = 150f;   
    [SerializeField] private float brakeForce = 400f;     

    [Header("Rotation Settings")]
    [SerializeField] private float _rotationSpeed = 300f; 

    [Header("Crash Settings")]
    [SerializeField] private float fallThresholdY = -10f;

    [Header("References")]
    [SerializeField] private GameManager gm;
    [SerializeField] private GameObject gameOverPanel; // Optional: eigenes Panel für GameOver

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip idleClip;
    [SerializeField] private AudioClip accelClip;
    [SerializeField] private AudioClip decelClip;

    private float moveInput;
    private float currentSpeed;
    private bool isGameOver = false;

    private float lastMoveInput;

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = true;
        audioSource.clip = idleClip;
        audioSource.Play();
    }

    void Update()
    {
        if (isGameOver) return;

        // Prioritize mobile UI emulator keys: Gas (LeftArrow) -> forward, Bremse (RightArrow) -> backward
        if (InputEmulator.GetKey(KeyCode.LeftArrow))
        {
            moveInput = 1f; // Gas
        }
        else if (InputEmulator.GetKey(KeyCode.RightArrow))
        {
            moveInput = -1f; // Bremse
        }
        else
        {
            moveInput = Input.GetAxisRaw("Horizontal");
        }

        if (moveInput != 0)
        {
            Debug.Log($"[DriveMOTO] moveInput: {moveInput}");
        }

        // --- Audio-State wechseln ---
        HandleEngineSound();

        // --- Fall Check ---
        if (gm != null && transform.position.y < fallThresholdY)
        {
            TriggerGameOver();
        }
    }

    void FixedUpdate()
    {
        if (isGameOver) return;

        float targetSpeed = moveInput * maxSpeed;

        if (moveInput != 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rearWheel.angularVelocity = Mathf.MoveTowards(rearWheel.angularVelocity, 0, brakeForce * Time.fixedDeltaTime);
                frontWheel.angularVelocity = Mathf.MoveTowards(frontWheel.angularVelocity, 0, brakeForce * Time.fixedDeltaTime);
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brakeForce * Time.fixedDeltaTime);
            }
            else
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.fixedDeltaTime);
            }
        }

        rearWheel.AddTorque(-currentSpeed * Time.fixedDeltaTime);
        frontWheel.AddTorque(-currentSpeed * Time.fixedDeltaTime);
        Frame.AddTorque(moveInput * _rotationSpeed * Time.fixedDeltaTime);
    }

    private void HandleEngineSound()
    {
        // Gas geben
        if (moveInput > 0.1f && lastMoveInput <= 0.1f)
        {
            SwitchSound(accelClip);
        }
        // Gas loslassen
        else if (moveInput <= 0.1f && lastMoveInput > 0.1f)
        {
            SwitchSound(decelClip);
        }

        // Wenn Sound fertig → Idle
        if (!audioSource.isPlaying)
        {
            SwitchSound(idleClip);
        }

        // Optional: Tonhöhe ändert sich leicht mit Geschwindigkeit
        audioSource.pitch = 0.8f + Mathf.Abs(currentSpeed / maxSpeed) * 0.4f;

        lastMoveInput = moveInput;
    }

    private void SwitchSound(AudioClip newClip)
    {
        if (audioSource.clip == newClip || newClip == null) return;
        audioSource.clip = newClip;
        audioSource.Play();
    }

    private void TriggerGameOver()
    {
        isGameOver = true;

        // GameManager GameOver auslösen
        if (gm != null)
        {
            gm.TriggerGameOver();
        }

        // Optional: eigenes GameOver-Panel anzeigen
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Zeit anhalten
        Time.timeScale = 0f;
    }

    // Methode für Restart-Button
    public void RestartGame()
    {
        Time.timeScale = 1f; // Zeit zurücksetzen
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Szene neu laden
    }
}
