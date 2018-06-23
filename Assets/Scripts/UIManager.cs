using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
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
    public Text ZombiesKilled;
    public Text Points;


    public AudioClip ButtonSound;

    public GameObject UpgradePrefab;
    public GameObject ScrollPanel;
    public Sprite AmmoImg;
    public Sprite ReloadImg;
    public Sprite FireTimeImg;


    private DataController data;
    private Upgrade[] upgrades;
    private int upgradeIndex;


    private void Start()
    {
        Panel_EndLevel.SetActive(false);
        Panel_PlayerDies.SetActive(false);
        Panel_Shop.SetActive(false);

        data = GameObject.FindObjectOfType<DataController>();
        upgrades = data.GetUpgrades();

        for(int i = 0; i < upgrades.Length; i++)
        {
            GameObject u = Instantiate(UpgradePrefab, new Vector3(UpgradePrefab.transform.position.x,UpgradePrefab.transform.position.y, UpgradePrefab.transform.position.z),Quaternion.identity,ScrollPanel.transform) as GameObject;
            u.GetComponent<RectTransform>().anchoredPosition = new Vector3(UpgradePrefab.transform.position.x+(i*30), UpgradePrefab.transform.position.y, UpgradePrefab.transform.position.z);

            u.transform.GetChild(0).GetComponent<Text>().text = upgrades[i].cost;
            if (upgrades[i].type == "ammo")
                u.transform.GetChild(1).GetComponent<Image>().sprite = AmmoImg;
            else if(upgrades[i].type == "reloadTime")
                u.transform.GetChild(1).GetComponent<Image>().sprite = ReloadImg;
            else if (upgrades[i].type == "fireTime")
                u.transform.GetChild(1).GetComponent<Image>().sprite = FireTimeImg;

            u.transform.GetChild(2).GetComponent<Text>().text = upgrades[i].description;
            u.transform.GetChild(3).GetComponent<Text>().text = "("+upgrades[i].gun.ToUpper()+")";

            u.GetComponent<Button>().onClick.AddListener(delegate
            {
                int upgradeIndex = 0;
                for(int c = 0; c < ScrollPanel.transform.childCount;c++)
                    if (ScrollPanel.transform.GetChild(c) == u.transform)
                        upgradeIndex = c;
                ShootWeapon sw = GameObject.FindObjectOfType<ShootWeapon>();
                bool buyed = false;
                if (upgrades[upgradeIndex].type == "ammo")
                {
                    if (upgrades[upgradeIndex].gun == "pistol")
                    {
                        if (upgrades[upgradeIndex].percentage == "y")
                            buyed = sw.UpgradePistol_TotalAmmo(int.Parse(upgrades[upgradeIndex].cost), sw.PistolTotalAmmo * Mathf.FloorToInt(float.Parse(upgrades[upgradeIndex].amount) / 100));
                        else
                            buyed= sw.UpgradePistol_TotalAmmo(int.Parse(upgrades[upgradeIndex].cost), int.Parse(upgrades[upgradeIndex].amount));
                    }
                    else if (upgrades[upgradeIndex].gun == "shotgun")
                    {
                        if (upgrades[upgradeIndex].percentage == "y")
                            buyed = sw.UpgradeShotgun_TotalAmmo(int.Parse(upgrades[upgradeIndex].cost), sw.ShotgunTotalAmmo * Mathf.FloorToInt(float.Parse(upgrades[upgradeIndex].amount) / 100));
                        else
                            buyed = sw.UpgradeShotgun_TotalAmmo(int.Parse(upgrades[upgradeIndex].cost), int.Parse(upgrades[upgradeIndex].amount));
                    }
                }
                else if (upgrades[upgradeIndex].type == "reloadTime")
                {
                    if (upgrades[upgradeIndex].gun == "pistol")
                    {
                        if (upgrades[upgradeIndex].percentage == "y")
                           buyed = sw.UpgradePistol_ReloadTime(int.Parse(upgrades[upgradeIndex].cost), sw.PistolReloadTime*(float.Parse(upgrades[upgradeIndex].amount) / 100));
                        else
                          buyed = sw.UpgradePistol_TotalAmmo(int.Parse(upgrades[upgradeIndex].cost), int.Parse(upgrades[upgradeIndex].amount));
                    }
                    else if (upgrades[upgradeIndex].gun == "shotgun")
                    {
                        if (upgrades[upgradeIndex].percentage == "y")
                            buyed = sw.UpgradeShotgun_ReloadTime(int.Parse(upgrades[upgradeIndex].cost), sw.ShotgunReloadTime * (float.Parse(upgrades[upgradeIndex].amount) / 100));
                        else
                            buyed = sw.UpgradeShotgun_TotalAmmo(int.Parse(upgrades[upgradeIndex].cost), int.Parse(upgrades[upgradeIndex].amount));
                    }
                }
                else if (upgrades[upgradeIndex].type == "fireTime")
                {
                    if (upgrades[upgradeIndex].gun == "pistol")
                    {
                        if (upgrades[upgradeIndex].percentage == "y")
                          buyed = sw.UpgradePistol_FireTime(int.Parse(upgrades[upgradeIndex].cost), sw.PistolFireTime * (float.Parse(upgrades[upgradeIndex].amount) / 100));
                        else
                          buyed =  sw.UpgradePistol_FireTime(int.Parse(upgrades[upgradeIndex].cost), int.Parse(upgrades[upgradeIndex].amount));
                    }
                    else if (upgrades[upgradeIndex].gun == "shotgun")
                    {
                        if (upgrades[upgradeIndex].percentage == "y")
                            buyed = sw.UpgradeShotgun_FireTime(int.Parse(upgrades[upgradeIndex].cost), sw.ShotgunFireTime * (float.Parse(upgrades[upgradeIndex].amount) / 100));
                        else
                            buyed = sw.UpgradeShotgun_FireTime(int.Parse(upgrades[upgradeIndex].cost), int.Parse(upgrades[upgradeIndex].amount));
                    }
                }

                if (buyed)
                    u.GetComponent<Button>().interactable = false;
            });

        }

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

        TimeSurvived.text = TimeText.text;
        ZombiesKilled.text = GameObject.FindObjectOfType<Player>().ZombiesKilled+"";
        Points.text = GameObject.FindObjectOfType<Player>().Points+"";
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

    public void WriteNewScore(GameObject input)
    {
        string name = input.GetComponent<InputField>().text;
        string points = Player.Points+"";
        string kills = Player.ZombiesKilled + "";
        int timeSurvived = GameObject.FindObjectOfType<GameManager>().seconds;

        // Convert the instance ('this') of this class to a JSON string with "pretty print" (nice indenting).
        Score score = new Score();
        score.seconds = timeSurvived;
        score.kills = kills;
        score.name = name;
        score.points = points;

        Score[] scores = GameObject.FindObjectOfType<DataController>().GetScores();
        Array.Reverse(scores);
        Score[] newScores = new Score[5];


        for (int i = 0; i < 5; i++)
        {
            if (i == 0)
            {
                if (int.Parse(score.points) > int.Parse(scores[i].points))
                    newScores[i] = score;
                else
                    newScores[i] = scores[i];
            }
            else if (int.Parse(score.points) > int.Parse(scores[i].points))
            {
                newScores[i] = score;
                newScores[i - 1] = scores[i];
            }
            else
                newScores[i] = scores[i];
        }


        Array.Reverse(newScores);

        string str = "{\n\"scores\":[\n";

        for (int i = 0; i < newScores.Length-1; i++)
        {
            str += JsonUtility.ToJson(newScores[i], true);
            str += ",\n";
        }

        str += JsonUtility.ToJson(newScores[newScores.Length-1], true);
        str += "\n]\n}";

        string filePath = Application.dataPath+"/Resources/scores.json";
        // Write that JSON string to the specified file.
        File.WriteAllText(filePath, str);
    }
}
