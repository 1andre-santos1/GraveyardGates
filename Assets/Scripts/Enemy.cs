using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public int Speed = 2;
    public int Cooldown = 5;
    public int Cost = 10;
    public float JumpPower = 800f;
    public int ProbabilityOfFalling = 50;
    public int MaxValueInCoins = 5;
    public int ProbabilityOfSpawningDiamond = 10;
    public int SecondsTillDestroy = 10;

    public GameObject HealthBar;
    public GameObject Coin;
    public GameObject Diamond;
    public AudioClip SoundAppearence;
    public AudioClip SoundHurt;
    public AudioClip SoundDie;


    private Animator anim;
    private Player PlayerBehaviour;
    private GameObject Player;

    private bool isMoving;
    private bool isAttacking;
    private bool isDying;
    private bool isAlreadyAttacking;
    private bool isCollidingWithPlayer = false;
    private Transform healthBar;

    private bool isBoss = false;

    private void Start()
    {
        isMoving = true;
        isAttacking = false;
        isDying = false;
        isAlreadyAttacking = false;
        if (GetComponent<Boss>() != null)
            isBoss = true;


        PlayerBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Player = GameObject.FindGameObjectWithTag("Player");
        anim = gameObject.transform.GetComponent<Animator>();
        healthBar = (Instantiate(HealthBar, transform) as GameObject).transform;
        healthBar.localScale = new Vector3((health*25f)/150,healthBar.localScale.y);
        GetComponent<AudioSource>().clip = SoundAppearence;
        GetComponent<AudioSource>().Play();

        updateAnimatorState();

    }

    private void updateAnimatorState()
    {
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("isDying", isDying);
    }

    public void setHealth(int value)
    {
        GetComponent<AudioSource>().clip = SoundHurt;
        GetComponent<AudioSource>().Play();
        this.health = value;
        healthBar.localScale = new Vector3((health*25f)/150,healthBar.localScale.y);


        if (this.health <= 0)
        {
            GetComponent<AudioSource>().clip = SoundDie;
            GetComponent<AudioSource>().Play();
            healthBar.localScale = new Vector3(0f, healthBar.localScale.y);

            int numberOfCoins = Random.Range(1, MaxValueInCoins+1);
            for(int i = 0; i < numberOfCoins; i++)
                Instantiate(Coin, new Vector2(Random.Range(transform.position.x-2f, transform.position.x + 2f),Coin.transform.position.y),Quaternion.identity);
            if(Random.Range(0f,1f) < ProbabilityOfSpawningDiamond/100f)
                Instantiate(Diamond, new Vector2(Random.Range(transform.position.x - 2f, transform.position.x + 2f), Diamond.transform.position.y), Quaternion.identity);


            health = 0;

            isDying = true;
            isMoving = false;
            isAttacking = false;

            updateAnimatorState();

            gameObject.transform.GetComponent<Rigidbody2D>().gravityScale = 0f;
            gameObject.transform.GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

            StartCoroutine("DestroyObject");

        }
    }
    public int getHealth()
    {
        return this.health;
    }

    private void Update()
    {
        if(isDying == false)
        {
            if (Player == null)
            {
                gameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            }
            else if (Player.gameObject.transform.position.x < gameObject.transform.position.x)
                gameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            else
                gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }

        if (isMoving && !isAttacking && !isDying)
        {
            if (!isBoss)
            {
                if (Player == null)
                {
                    gameObject.transform.position += new Vector3(-Speed * Time.deltaTime, 0f, 0f);
                }
                else if (Player.gameObject.transform.position.x < gameObject.transform.position.x)
                {
                    gameObject.transform.position += new Vector3(-Speed * Time.deltaTime, 0f, 0f);
                }
                else
                {
                    gameObject.transform.position += new Vector3(Speed * Time.deltaTime, 0f, 0f);
                }
            }
            else
            {
                if (GameObject.FindObjectOfType<Arduino>().GetKeyPressed() == 8)
                {
                    gameObject.transform.position += new Vector3(Speed * Time.deltaTime, 0f, 0f);
                    GameObject.FindObjectOfType<Arduino>().SetKeyPressed(0);
                }
                else if(GameObject.FindObjectOfType<Arduino>().GetKeyPressed() == 7)
                {
                    gameObject.transform.position += new Vector3(-Speed * Time.deltaTime, 0f, 0f);
                    GameObject.FindObjectOfType<Arduino>().SetKeyPressed(0);
                }
                if (GameObject.FindObjectOfType<Arduino>().GetKeyPressed() == 6)
                {
                    isMoving = false;
                    isAttacking = true;
                    updateAnimatorState();
                    GameObject.FindObjectOfType<Arduino>().SetKeyPressed(0);
                    StartCoroutine("StopAttack");


                }
            }
        }


        if (isAttacking && !isMoving && !isAlreadyAttacking)
        {
            isAlreadyAttacking = true;
        }
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(1);
        if(isAttacking)
            PlayerBehaviour.Health = PlayerBehaviour.Health - 20;
        if (isAttacking == false)
        {
            StopCoroutine(this.Damage());
            isAlreadyAttacking = false;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isBoss)
        {
            if (collision.gameObject.tag == "Player")
            {
                isMoving = false;
                isAttacking = true;
                updateAnimatorState();
            }
        }
        else
        {
            if (collision.gameObject.tag == "Player")
                isCollidingWithPlayer = true;
        }
        
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "CrateHealth")
        {
            Physics2D.IgnoreCollision(collision.collider,gameObject.GetComponent<Collider2D>());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isMoving = true;
            isAttacking = false;
            updateAnimatorState();
            isCollidingWithPlayer = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hole")
        {
            if(Random.Range(0f,1f) < (ProbabilityOfFalling/100f))
                GetComponent<BoxCollider2D>().isTrigger = true;
            else
            {
                if(collision.gameObject.transform.position.x > transform.position.x)
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(JumpPower / 4, JumpPower));
                else
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(-JumpPower / 4, JumpPower));
            }
        }

        if (collision.gameObject.tag == "ProtectionShield")
        {
            setHealth(0);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hole")
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(SecondsTillDestroy);
        Destroy(gameObject);
    }

    IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
        isAttacking = false;
        isMoving = true;
        updateAnimatorState();
    }

    public void DamagePlayer()
    {
        if(isCollidingWithPlayer)
            PlayerBehaviour.Health = PlayerBehaviour.Health - 20;
    }
}
