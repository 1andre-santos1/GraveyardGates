using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Text LoadingText;
    public Text TipText;
    public int MinLoadingTime = 3;
    public int MaxLoadingTime = 10;

    public string[] tips;

    private int loadingState = 0;
    private void Start()
    {
        TipText.text = tips[Random.Range(0, tips.Length)];

        StartCoroutine("AnimateLoadingText");
        StartCoroutine("StartGame");
    }

    IEnumerator AnimateLoadingText()
    {
        if (loadingState == 0)
            LoadingText.text = "LOADING.";
        else if(loadingState == 1)
            LoadingText.text = "LOADING..";
        else if (loadingState == 2)
            LoadingText.text = "LOADING";

        loadingState++;
        if (loadingState > 2)
            loadingState = 0;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(AnimateLoadingText());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(Random.Range(MinLoadingTime,MaxLoadingTime+1));
        GameObject.FindObjectOfType<LevelManager>().LoadLevel(1);
    }
}
