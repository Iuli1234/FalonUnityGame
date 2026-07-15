using UnityEngine;

public class ApplePickUp : MonoBehaviour
{
    [SerializeField] AudioClip ApplePickUpSFX;
    [SerializeField] int appleValue = 1;
    int playerLives;
    bool wasCollected = false;
    void OnTriggerEnter2D(Collider2D other) 
    {
        playerLives = FindFirstObjectByType<GameSession>().GetCurrentPlayerLives();
        if(other.CompareTag("Player") && !wasCollected && playerLives < 5)
        {
            wasCollected = true;
            FindFirstObjectByType<GameSession>().AddLife(appleValue);
            AudioSource.PlayClipAtPoint(ApplePickUpSFX, transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
