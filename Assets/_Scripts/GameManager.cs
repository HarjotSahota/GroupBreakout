using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private Ball ball;
    [SerializeField] private Transform bricksContainer;
    public ParticleSystem ExplosionParticles; 
    public ParticleSystem FlashParticles;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text livesText;

    private int brick = 0;
    [SerializeField] private BrickCounterUI brickCounter;

    private int currentBrickCount;
    private int totalBrickCount;

    private void OnEnable()
    {
        InputHandler.Instance.OnFire.AddListener(FireBall);
        ball.ResetBall();
        totalBrickCount = bricksContainer.childCount;
        currentBrickCount = bricksContainer.childCount;
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnFire.RemoveListener(FireBall);
    }

    private void FireBall()
    {
        AudioManager.instance.PlaySfx("Fire");
        ball.FireBall();
    }

    public void OnBrickDestroyed(Vector3 position)
    {
        // Fire audio here
        AudioManager.instance.PlaySfx("Brick");
        // Implement particle effect here
        PlayHitEffects(position);
        // Add camera shake here

        // Implementing coin text
        brick ++;
        brickCounter.UpdateScore(brick);

        currentBrickCount--;
        Debug.Log($"Destroyed Brick at {position}, {currentBrickCount}/{totalBrickCount} remaining");
        if(currentBrickCount == 0) SceneHandler.Instance.LoadNextScene();
    }

    public void KillBall()
    {
        maxLives--;
        UpdateLivesUI();

        if (maxLives == 0)
        {
            StartCoroutine(GameOverSequence());
        }
        else
        {
            ball.ResetBall();
        }
    }

    IEnumerator GameOverSequence()
    {
        Time.timeScale = 0f; // Freeze time
        gameOverPanel.SetActive(true); // Show Game Over UI

        yield return new WaitForSecondsRealtime(1.5f); // Wait for 1.5 seconds in real-time

        Time.timeScale = 1f; // Restore time scale
        SceneHandler.Instance.LoadMenuScene(); // Transition to Main Menu
    }

    void UpdateLivesUI()
    {
        livesText.text = "Lives: " + Mathf.Max(0, maxLives);
    }

    
    public void PlayHitEffects(Vector3 position)
    {
        ExplosionParticles.transform.position = position;
        FlashParticles.transform.position = position;
        ExplosionParticles.Play();
        FlashParticles.Play();
    }
}  