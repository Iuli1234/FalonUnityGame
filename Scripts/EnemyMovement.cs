using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float scale = 1f;
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

    void UpdateSpriteDirection()
    {
        if (moveSpeed > 0)
            transform.localScale = new Vector2(scale, scale);
        else
            transform.localScale = new Vector2(-scale, scale);
    }
}
