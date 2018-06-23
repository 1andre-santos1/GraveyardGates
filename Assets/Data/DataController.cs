using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataController : MonoBehaviour
{
    private string filename = "/Resources/upgrades.json";
    private string scoresFile = "/Resources/scores.json";

    [SerializeField]
    private TextAsset upgradeFile;

    [SerializeField] private TextAsset scoresText;

    private Upgrades upgrades;
    private Scores scores;

	// Use this for initialization
	void Awake ()
	{
        ReadData();
	}

    public void ReadData()
    {
        string contents = upgradeFile.text;
        upgrades = JsonUtility.FromJson<Upgrades>(contents);

        string scoresContent = scoresText.text;
        scores = JsonUtility.FromJson<Scores>(scoresContent);
    }

    public Upgrade[] GetUpgrades()
    {
        return upgrades.upgrades;
    }

    public Score[] GetScores()
    {
        return scores.scores;
    }
}
