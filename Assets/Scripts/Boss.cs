using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private void Update()
    {
        if (gameObject.GetComponent<Enemy>().health <= 0)
        {
            GameObject.FindObjectOfType<GameManager>().IsGameActive = false;
            GameObject.FindObjectOfType<BossSpawner>().StopAllCoroutines();
            GameObject.FindObjectOfType<BossSpawner>().enabled = false;
            GameObject.FindObjectOfType<GameManager>().EndLevel();
            Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
            foreach (var en in enemies)
                Destroy(en.gameObject);
        }

        Vector3 minPos = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, transform.position.z));
        Vector3 maxPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, transform.position.z));

        float minX = minPos.x;
        float maxX = maxPos.x;

        var currentX = Mathf.Clamp(gameObject.transform.position.x, minX, maxX);
        gameObject.transform.position = new Vector3(currentX, gameObject.transform.position.y, gameObject.transform.position.z);
    }

}
