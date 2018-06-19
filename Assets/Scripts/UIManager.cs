using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public GameObject HealthUI;
    public GameObject MoneyUI;

    public Text HealthText;
    public Player Player;
    public GameObject Panel_EndLevel;
    public GameObject Panel_PlayerDies;
    public Text TimeSurvived;

    public GameObject Panel_Shop;
    public Text Shop_NumberOfCoins;
    public Text TimeText;

    public AudioClip ButtonSound;

    private void Start()
    {
        Panel_EndLevel.SetActive(false);
        Panel_PlayerDies.SetActive(false);
        Panel_Shop.SetActive(false);
    }
    private void Update()
    {
        HealthText.text = ""+Player.Health;
    }

    public void ShowPanel_EndLevel()
    {
        Panel_EndLevel.SetActive(true);
    }

    public void ShowPanel_PlayerDies()
    {
        Panel_PlayerDies.SetActive(true);

        TimeSurvived.text = "You survived: "+TimeText.text;
    }

    public void ShowPanel_Shop()
    {
        GameObject.FindObjectOfType<GameManager>().IsGameActive = false;
        Panel_Shop.SetActive(true);
        Shop_NumberOfCoins.text = GameObject.FindObjectOfType<Player>().GetMoney()+"";
    }

    public void UpdateNumberOfCoins()
    {
        Shop_NumberOfCoins.text = GameObject.FindObjectOfType<Player>().GetMoney() + "";
    }

    public void UpdateTime(string time)
    {
        TimeText.text = time;
    }

    public void HidePanel_EndLevel()
    {
        Panel_EndLevel.SetActive(false);
    }

    public void HidePanel_PlayerDies()
    {
        Panel_PlayerDies.SetActive(false);
    }

    public void HidePanel_Shop()
    {
        Panel_Shop.SetActive(false);
        GameObject.FindObjectOfType<BossSpawner>().enabled = true;

    }

    public void PlayButtonSound()
    {
        GetComponent<AudioSource>().clip = ButtonSound;
        GetComponent<AudioSource>().Play();
    }

    public void BlinkMoney()
    {
        MoneyUI.GetComponent<Animator>().Play("Blinking");
    }

}
