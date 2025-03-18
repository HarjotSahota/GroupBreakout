using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball")) // Ensure the Ball has the "Ball" tag
        {
            GameManager.Instance.KillBall(); // Reduce lives
        }
    }
}