using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoresManager : MonoBehaviour
{
    public GameObject[] Heroes;

    private DataController data;
    private Score[] scores;

    public void Start()
    {
        UpdateScores();
    }

    public void UpdateScores()
    {
        data = GameObject.FindObjectOfType<DataController>();
        data.ReadData();
        scores = data.GetScores();

        for (int i = 0; i < Heroes.Length; i++)
        {
            Heroes[i].transform.GetChild(0).GetComponent<Text>().text = scores[i].name;

            int minutes = scores[i].seconds / 60;
            int secs = scores[i].seconds % 60;
            if (secs >= 10)
                Heroes[i].transform.GetChild(1).GetComponent<Text>().text = (scores[i].seconds / 60) + ":" + (scores[i].seconds % 60);
            else
                Heroes[i].transform.GetChild(1).GetComponent<Text>().text = (scores[i].seconds / 60) + ":0" + (scores[i].seconds % 60);

            Heroes[i].transform.GetChild(2).GetComponent<Text>().text = scores[i].kills;
            Heroes[i].transform.GetChild(3).GetComponent<Text>().text = scores[i].points;
        }
    }
}
