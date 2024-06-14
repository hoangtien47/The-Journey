using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private SpriteRenderer sprite;

    private float dirX = 0f;

    public float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private LayerMask jumpablesGround;

    private enum MovementState { idle,running,jumping,falling};

    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource runSoundEffect;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = rb.GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(dirX * moveSpeed,rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2( rb.velocity.x, jumpForce);
        }
        AnimationUpdate();



    }
    private void AnimationUpdate()
    {
        MovementState state;
        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
            if (!runSoundEffect.isPlaying)
            {
                runSoundEffect.Play();
            }
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
            if (!runSoundEffect.isPlaying)
            {
                runSoundEffect.Play();
            }
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if(rb.velocity.y < -.1f)
        {
            state = MovementState.falling;

        }

        animator.SetInteger("state",(int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f,jumpablesGround);
    }
}
