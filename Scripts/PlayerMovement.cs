using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 14f;
    [SerializeField] float climbSpeed = 4f;
    Vector2 deathKick = new Vector2(3f, 5f);
    [SerializeField] GameObject arrow;
    [SerializeField] Transform bow;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip shootSFX;
    [SerializeField] AudioClip climbSFX;
    float climbVolume = 0.1f;
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    public Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;
    bool isAlive = true;
    AudioSource myAudioSource;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myAudioSource = GetComponent<AudioSource>();

        gravityScaleAtStart = myRigidbody.gravityScale;
    }
    void Update()
    {
        if(!isAlive) { return; }    
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        if(!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            return;
        if(value.isPressed)
        {
            myRigidbody.linearVelocity += new Vector2(0f, jumpSpeed);
            AudioSource.PlayClipAtPoint(jumpSFX, transform.position);
        }
    }   
    
    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.linearVelocity.y);
        myRigidbody.linearVelocity = playerVelocity;

        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", hasHorizontalSpeed);
    }
    void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        if(hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.linearVelocity.x), 1f); 
        }
    }
    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            
            if (myAudioSource.clip == climbSFX)
            {
                myAudioSource.Stop();
                myAudioSource.clip = null; 
            }
            return;
        }

        Vector2 climbVelocity = new Vector2(myRigidbody.linearVelocity.x, moveInput.y * climbSpeed);
        myRigidbody.linearVelocity = climbVelocity;
        myRigidbody.gravityScale = 0f;

        bool hasVerticalSpeed = Mathf.Abs(myRigidbody.linearVelocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", hasVerticalSpeed);

        if (hasVerticalSpeed)
        {
            if (!myAudioSource.isPlaying || myAudioSource.clip != climbSFX)
            {
                myAudioSource.clip = climbSFX;
                myAudioSource.volume = climbVolume;
                myAudioSource.loop = true;
                myAudioSource.Play();
            }
        }
        else
        {
            if (myAudioSource.clip == climbSFX) 
            {
                myAudioSource.Stop();
            }
        }
    }    
    void OnAttack(InputValue value)
    {
        if(!isAlive){return;}
        myAnimator.SetTrigger("Shoot");
        Instantiate(arrow, bow.position, transform.rotation);
        AudioSource.PlayClipAtPoint(shootSFX, transform.position);
    }
    void Die()
    {
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards", "Water")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.linearVelocity = deathKick;
            
            AudioSource.PlayClipAtPoint(deathSFX, transform.position);
            
            StartCoroutine(DeathSequence());
        }
    }

    IEnumerator DeathSequence()
    {
        yield return new WaitForEndOfFrame();
        float animationLength = myAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength-0.1f);
        FindAnyObjectByType<GameSession>().ProcessPlayerDeath();
    }
}
