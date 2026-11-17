using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{   
    public float speed = 5f;
    public float powerJump = 10f;
    bool onFloor = true;
    bool onJumpingZone = true;
    bool walk = true;
    public bool muerto = false;
    public Animator animator;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    
    public Sprite jumping;
    public Sprite standing;
    public Sprite walking;

    private DobleSalto dobleSalto;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        dobleSalto = GetComponent<DobleSalto>();
        if (dobleSalto == null)
        {
            dobleSalto = gameObject.AddComponent<DobleSalto>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !onJumpingZone && dobleSalto.IntentarSaltoExtra())
        {
            rb.linearVelocityY = powerJump;
        }

        if(Input.GetKey(KeyCode.Space) && onJumpingZone)
        {
            rb.linearVelocityY = powerJump;
            onFloor = false;
            onJumpingZone = false;
        }

        else if(Input.GetKey(KeyCode.D))
        {
            rb.linearVelocityX = speed;
            sr.sprite = walking;
            sr.flipX = false;
            walk = true;
        }

        else if (Input.GetKey(KeyCode.A))
        {
            rb.linearVelocityX = -speed;
            sr.sprite = walking;
            sr.flipX = true;
            walk = true;
        }

        else if (!walk && !onFloor)
        {   
            sr.sprite = jumping;
        }

        else
        {
            sr.sprite=standing;
            rb.linearVelocityX = 0;
            walk = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("jumpingZone"))
        {
            onFloor = true;
            onJumpingZone = true;
        }
    }

    private void OnTgerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Dead"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void Respawn(Vector3 posicionCheckpoint){
        muerto = false;

        transform.position = posicionCheckpoint;

        rb.linearVelocity = Vector2.zero;
        animator.SetBool("muerto", false);
    }
}