using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    public int CoinValue = 5;
    public int DiamondValue = 50;
    public int CrateHealthIncrement = 50;
    public Text ammoText;
    public Text MoneyText;
    public Text HealthText;
    public AudioClip SoundHurt;
    public AudioClip SoundDie;
    public AudioClip Coin;

    public GameObject Shield;


    private bool isMoving;
    private int health = 100;
    private int extraHealth = 0;
    public int money = 0;

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            if (value < health)
            {
                GetComponent<AudioSource>().clip = SoundHurt;
                GetComponent<AudioSource>().Play();
            }
            health = value;
            if (health <= 0)
            {
                Destroy(gameObject);
                GameObject.FindObjectOfType<GameManager>().PlayerDies();
            }
        }
    }

    public int GetMoney()
    {
        return money;
    }

    public void SetMoney(int value)
    {
        this.money = value;
    }

    private void Update()
    {
        isMoving = (GetComponent<MoveFromInput>().IsMoving()) ? true : false;

        gameObject.transform.GetComponent<Animator>().SetBool("isMoving", isMoving);

        if(GetComponent<ShootWeapon>().currentWeapon == 0)
            ammoText.text = GetComponent<ShootWeapon>().PistolActualAmmo + "/" + GetComponent<ShootWeapon>().PistolTotalAmmo;
        else if(GetComponent<ShootWeapon>().currentWeapon == 1)
            ammoText.text = GetComponent<ShootWeapon>().ShotgunActualAmmo + "/" + GetComponent<ShootWeapon>().ShotgunTotalAmmo;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hole")
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
        if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);
            money += CoinValue;
            UpdateMoneyText();
            GetComponent<AudioSource>().clip = Coin;
            GetComponent<AudioSource>().Play();
            GameObject.FindObjectOfType<UIManager>().BlinkMoney();
        }
        if (collision.gameObject.tag == "Diamond")
        {
            Destroy(collision.gameObject);
            money += DiamondValue;
            UpdateMoneyText();
            GetComponent<AudioSource>().clip = Coin;
            GetComponent<AudioSource>().Play();
            GameObject.FindObjectOfType<UIManager>().BlinkMoney();
        }
        if (collision.gameObject.tag == "BossTrigger")
        {
            GameObject.FindObjectOfType<MainCamera>().IsFixed = true;
            GameObject.FindObjectOfType<MoveFromInput>().IsFixedToCameraBounds = true;
            GameObject.FindObjectOfType<GameManager>().BossActive = true;
            GameObject.FindObjectOfType<BossSpawner>().SpawnBoss();
            Destroy(collision.gameObject);
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hole")
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            GetComponent<MoveFromInput>().isJumping = false;
        }
        if (collision.gameObject.tag == "CrateHealth")
        {
            Destroy(collision.gameObject);
            health += CrateHealthIncrement;
            if (health > 100)
                health = 100;
            UpdateHealthText();
        }
        if (collision.gameObject.tag == "CrateShield")
        {
            Destroy(collision.gameObject);
            Instantiate(Shield,new Vector3(transform.position.x,Shield.transform.position.y,Shield.transform.position.z),Quaternion.identity);
        }
        if (collision.gameObject.tag == "ProtectionShield")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    public void UpdateMoneyText()
    {
        MoneyText.text = money + "";
    }

    public void UpdateHealthText()
    {
        HealthText.text = health + "";
    }

    public void ResetPosition()
    {
        transform.position = new Vector2(-9.8f,transform.position.y);
    }

    private void OnDestroy()
    {
        GameObject.FindObjectOfType<GameManager>().PlayerDies();
    }
}
