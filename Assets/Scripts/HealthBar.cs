using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(player.Health, gameObject.GetComponent<RectTransform>().sizeDelta.y); 
    }

}
