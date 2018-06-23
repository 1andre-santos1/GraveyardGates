using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool BossActive = false;
    public bool IsGameActive = true;

    public int seconds = 0;

    private UIManager uimanager;
    private int levelIndex = 1;


    private void Start()
    {
        uimanager = GameObject.FindObjectOfType<UIManager>();
        StartCoroutine("UpdateTime");

        StartCoroutine("Wait");
        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        GameObject.FindObjectOfType<ArduinoController>().WriteToArduino(levelIndex);
        levelIndex++;
    }
    public void EndLevel()
    {
        uimanager.ShowPanel_EndLevel();
        BossActive = false;
    }

    public void PlayerDies()
    {
        uimanager.ShowPanel_PlayerDies();
        BossActive = false;
        IsGameActive = false;
    }

    public void NextLevel()
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (var en in enemies)
            Destroy(en.gameObject);

        uimanager.HidePanel_Shop();
        uimanager.HidePanel_EndLevel();
        GameObject.FindObjectOfType<Player>().ResetPosition();
        GameObject.FindObjectOfType<Player>().UpdateMoneyText();
        GameObject.FindObjectOfType<BossSpawner>().StartSpawningEvent();
        IsGameActive = true;
        GameObject.FindObjectOfType<ArduinoController>().WriteToArduino(levelIndex);
        levelIndex++;
        GameObject.FindObjectOfType<Arduino>().CooldownTime /= 2;
        if (GameObject.FindObjectOfType<Arduino>().CooldownTime < 1)
            GameObject.FindObjectOfType<Arduino>().CooldownTime = 1;
    }

    private IEnumerator UpdateTime()
    {
        int minutes = seconds / 60;
        int secs = seconds % 60;
        if(secs >= 10)
            uimanager.UpdateTime((seconds/60)+":"+(seconds%60));
        else
            uimanager.UpdateTime((seconds/60)+":0"+(seconds%60));
        yield return new WaitForSeconds(1f);
        if (IsGameActive)
        {
            seconds++;
            GameObject.FindObjectOfType<Player>().Points+=1;
        }
        StartCoroutine("UpdateTime");
    }
}
