using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] AudioClip coinPickUpSFX;
    [SerializeField] int coinValue = 100;
    bool wasCollected = false;
    void OnTriggerEnter2D(Collider2D other) 
    {
        {
            if(other.CompareTag("Player") && !wasCollected)
            {
                wasCollected = true;
                FindFirstObjectByType<GameSession>().AddToScore(coinValue);
                AudioSource.PlayClipAtPoint(coinPickUpSFX, transform.position);
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
