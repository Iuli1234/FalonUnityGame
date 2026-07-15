using NUnit.Framework;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float ArrowSpeed = 15f;
    Rigidbody2D myRigidbody;
    PlayerMovement player;
    float xSpeed;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindFirstObjectByType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * ArrowSpeed;
    }

    void Update()
    {
        FlipSprite();
        myRigidbody.linearVelocity = new Vector2(xSpeed, 0f);
    }
    void FlipSprite()
    {
        transform.localScale = new Vector2(Mathf.Sign(xSpeed), 1f); 
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Săgeata a lovit: " + other.name + " cu tag-ul: " + other.tag);
        GameSession gameSession = FindFirstObjectByType<GameSession>();
        if(other.CompareTag("Enemy"))
        {
            gameSession.AddToEnemyCount(1);
            Destroy(other.gameObject);
        }
        else if(other.CompareTag("Boss"))
        {
            BossMovement boss = other.GetComponent<BossMovement>();
            if(boss != null)
            {
                boss.TakeDamage();
            }
        }
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject, 1f);
    }
}
