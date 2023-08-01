using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [Header("Variables")]
    public int healthValue;
    public int atkDmg;
    public float moveSpd;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float atkDelayValue;
    [SerializeField]
    private float knockbackValue;
    [SerializeField]
    private float hitDelayValue;
    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private AudioClip atkSound;
    [SerializeField]
    private AudioClip jumpSound;
    [HideInInspector]
    public bool dead;

    [HideInInspector]
    public int health;
    private float atkDelay;
    private float hitDelay;
    public bool hit;
    private bool isGrounded;
    private bool attacked;

    SpriteRenderer sr;
    Animator anim;
    Rigidbody2D rb;
    AudioSource aS;
    GameObject Atk1;
    GameMaster gm;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        aS = GetComponent<AudioSource>();
        Atk1 = transform.GetChild(0).gameObject;
        gm = GameObject.Find("GameMaster").GetComponent<GameMaster>();
    }

    // Start is called before the first frame update
    void Start()
    {
        health = healthValue;
        atkDelay = atkDelayValue;
        hitDelay = hitDelayValue;
        isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        Behavior();
        Movement();
        Jump();
        Attack();
    }

    private void Behavior()
    {
        if (health <= 0)
        {
            dead = true;
            anim.SetBool("Dead", true);
            gm.gameOver = true;
        }
    }

    private void Movement()
    {
        var x = Input.GetAxis("Horizontal") * moveSpd;

        if (!hit && !attacked && !dead)
        {
            rb.velocity = new Vector2(x, rb.velocity.y);
        }
        else
        {
            hitDelay -= Time.deltaTime;
            if (hitDelay < 0)
            {
                hit = false;
                hitDelay = hitDelayValue;
            }
        }
        

        if (x > 0 && !dead)
        {
            sr.flipX = false;
            Atk1.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (x < 0 && !dead)
        {
            sr.flipX = true;
            Atk1.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        

        if (x != 0)
        {
            anim.SetBool("Move", true);
        }
        else
        {
            anim.SetBool("Move", false);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !dead)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            aS.PlayOneShot(jumpSound);
        }

        if (isGrounded)
        {
            anim.SetBool("Jump", false);
        }
        else
        {
            anim.SetBool("Jump", true);
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.C) && !attacked && !hit && !dead)
        {
            anim.Play("Attack");
            aS.PlayOneShot(atkSound);
            attacked = true;
        }

        if (attacked)
        {
            atkDelay -= Time.deltaTime;
            if (atkDelay <= 0)
            {
                attacked = false;
                atkDelay = atkDelayValue;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            isGrounded = true;
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Crabby Attack Hitbox")
        {
            hit = true;
            health -= 1;
            anim.Play("Hit");
            aS.PlayOneShot(hitSound);
            if (collision.gameObject.GetComponentInParent<SpriteRenderer>().flipX == false)
            {
                rb.AddForce(transform.right * -knockbackValue, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(transform.right * knockbackValue, ForceMode2D.Impulse);
            }
        }

        if (collision.gameObject.name == "Fierce Tooth Attack Hitbox")
        {
            hit = true;
            health -= 1;
            anim.Play("Hit");
            aS.PlayOneShot(hitSound);
            if (collision.gameObject.GetComponentInParent<SpriteRenderer>().flipX == false)
            {
                rb.AddForce(transform.right * -knockbackValue, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(transform.right * knockbackValue, ForceMode2D.Impulse);
            }
        }

        if (collision.gameObject.name == "Pink Star Attack Hitbox")
        {
            hit = true;
            health -= 1;
            anim.Play("Hit");
            aS.PlayOneShot(hitSound);
            if (collision.gameObject.GetComponentInParent<SpriteRenderer>().flipX == false)
            {
                rb.AddForce(transform.right * -knockbackValue, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(transform.right * knockbackValue, ForceMode2D.Impulse);
            }
        }
    }
}
