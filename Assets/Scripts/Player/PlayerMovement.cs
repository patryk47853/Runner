using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")] [SerializeField]
    private float speed;

    [SerializeField] private float jumpPower;

    [Header("Coyote Time")] [SerializeField]
    private float coyoteTime; //Czas jaki uzytkownik podczas skoku moze przebywac w powietrzu

    private float coyoteCounter; //Czas jaki uplynal od odskoku z sciany

    [Header("Multiple Jumps")] [SerializeField]
    private int extraJumps;

    private int jumpCounter;

    [Header("Wall Jumping")] [SerializeField]
    private float wallJumpX; //Sila skoku na osi X

    [SerializeField] private float wallJumpY; //Sila skoku na osi Y

    [Header("Layers")] [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Sounds")] [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //Obracanie gracza gdy chodzi lewo/prawo
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        //Animacje gracza
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //Skok
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        //Dostosowywanie skoku
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        if (onWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; //Resetuj czas bezwladnosci
                jumpCounter = extraJumps; //Resetuj licznik skokow
            }
            else
                coyoteCounter -= Time.deltaTime; //Zmniejszanie stanu bezwladnosci gdy gracz nie jest na ziemi
        }
    }

    private void Jump()
    {
        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return;
        // Jesli licznik bezwladnosci jest <= 0 i nie jest na scianie to wykonaj return

        SoundManager.instance.PlaySound(jumpSound);

        if (onWall())
            WallJump();
        else
        {
            if (isGrounded())
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
            {
                //Jesli gracz nie jest na ziemi i licznik bezwladnosci jest >=0 skocz
                if (coyoteCounter > 0)
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                else
                {
                    //Jesli gracz ma dodatkowe skoki, to skocz i zmniejsz licznik skokow
                    if (jumpCounter > 0)
                    {
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }
            
            //Resetuj licznik bezwladnosci, zeby nie moc skakac wielokrotnie bez konca
            coyoteCounter = 0;
        }
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down,
            0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0,
            new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}