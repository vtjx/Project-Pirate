using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class Crabby : MonoBehaviour
{
    [Header("Variables")]
    public int healthValue;
    [SerializeField]
    private float moveSpd;
    [SerializeField]
    private float atkDelayValue;
    [SerializeField]
    private float knockbackValue;
    [SerializeField]
    private float hitDelayValue;
    [SerializeField]
    private float deathDelayValue;
    [SerializeField]
    [Header("References")]
    private GameObject player;
    [SerializeField]
    private SpriteRenderer playerSR;
    [SerializeField]
    private GameMaster gameMaster;
    [SerializeField]
    private Player playerS;
    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private AudioClip atkSound;
    [SerializeField]
    private AudioClip deathSound;

    private int health;
    private float atkDelay;
    private float hitDelay;
    private float deathDelay;
    private float distanceDiff;
    private bool hit;
    private bool attacked;
    private bool dead;

    SpriteRenderer sr;
    Animator anim;
    Rigidbody2D rb;
    AudioSource aS;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        playerSR = player.GetComponent<SpriteRenderer>();
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        playerS = player.GetComponent<Player>();
        aS = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        health = healthValue;
        atkDelay = atkDelayValue;
        hitDelay = hitDelayValue;
        deathDelay = deathDelayValue;
    }

    // Update is called once per frame
    void Update()
    {
        Behavior();
        Attack();
    }

    private void Behavior()
    {
        if (transform.position.x > player.transform.position.x)
        {
            if (!dead)
            {
                sr.flipX = false;
            }
            
            if (!hit && !attacked && !dead && !playerS.dead)
            {
                anim.SetBool("Run", true);
                rb.velocity = new Vector2(-moveSpd, rb.velocity.y);
            }
            else
            {
                anim.SetBool("Run", false);
                hitDelay -= Time.deltaTime;

                if (hitDelay < 0)
                {
                    hit = false;
                    hitDelay = hitDelayValue;
                }
            }
        }
        else
        {
            if (!dead)
            {
                sr.flipX = true;
            }
            
            if (!hit && !attacked && !dead && !playerS.dead)
            {
                anim.SetBool("Run", true);
                rb.velocity = new Vector2(moveSpd, rb.velocity.y);
            }
            else
            {
                anim.SetBool("Run", false);
                hitDelay -= Time.deltaTime;

                if (hitDelay < 0)
                {
                    hit = false;
                    hitDelay = hitDelayValue;
                }
            }
        }
        
        if (health <= 0 && !dead)
        {
            dead = true;
            anim.Play("Dead");
            aS.PlayOneShot(deathSound);
        }

        if (dead)
        {
            deathDelay -= Time.deltaTime;
            if (deathDelay <= 0)
            {
                gameMaster.enemiesRemaining -= 1; 
            }
        }
    }

    private void LateUpdate()
    {
        if (deathDelay <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Attack()
    {
        distanceDiff = Vector2.Distance(transform.position, player.transform.position);
        if (distanceDiff <= 0.3f && !attacked && !hit && !dead && !playerS.dead)
        {
            anim.Play("Attack");
            aS.PlayOneShot(atkSound);
            attacked = true;
        }

        if (attacked)
        {
            atkDelay -= Time.deltaTime;
            if (atkDelay < 0)
            {
                attacked = false;
                atkDelay = atkDelayValue;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Attack Hitbox" && !dead)
        {
            hit = true;
            health -= playerS.atkDmg;
            anim.Play("Hit");
            aS.PlayOneShot(hitSound);
            if (playerSR.flipX == false)
            {
                rb.AddForce(transform.right * knockbackValue, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(transform.right * -knockbackValue, ForceMode2D.Impulse);
            }
        }

        if ((collision.gameObject.name == "DeathZoneL" || collision.gameObject.name == "DeathZoneR") && !dead)
        {
            aS.PlayOneShot(deathSound);
            gameMaster.enemiesRemaining -= 1;
            var delay = 0.1f;
            delay -= Time.deltaTime;
            if (delay <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
