using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float scale = 1f;
    [SerializeField] private int bossHealth = 5; 

    Rigidbody2D myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        UpdateSpriteDirection();
    }

    void Update()
    {
        myRigidbody.linearVelocity = new Vector2(moveSpeed, 0f); 
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        moveSpeed = -moveSpeed; 
        UpdateSpriteDirection();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            moveSpeed = -moveSpeed; 
            UpdateSpriteDirection(); 
        }
    }
    void UpdateSpriteDirection()
    {
        if (moveSpeed > 0)
            transform.localScale = new Vector2(scale, scale);
        else
            transform.localScale = new Vector2(-scale, scale);
    }

    public void TakeDamage()
    {
        bossHealth--;
        if(bossHealth <= 0)
        {
            GameSession gameSession = FindFirstObjectByType<GameSession>();
            if(gameSession != null)
            {
                gameSession.AddToEnemyCount(1);
            }
            Destroy(gameObject);
        }
    }
}