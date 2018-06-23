using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject[] Minions;
    public int NumberOfMinions = 3;
    public int MinionsDelay = 5;
    public int MeteorDelay = 10;

    private bool canSpawnMinions = true;
    private bool canSpawnMeteors = true;

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

        if (GameObject.FindObjectOfType<Arduino>().GetKeyPressed() == 5)
        {
            if (!canSpawnMinions)
                return;
            for (int i = 0; i < NumberOfMinions; i++)
                Instantiate(Minions[Random.Range(0,Minions.Length)], transform.position, Quaternion.identity);
            StartCoroutine("DeactivateSpawning");
        }
        else if (GameObject.FindObjectOfType<Arduino>().GetKeyPressed() == 4)
        {
            if (!canSpawnMeteors)
                return;
            GameObject.FindObjectOfType<EnemySpawner>().SpawnMeteors();
            StartCoroutine("DeactivateMeteorsSpawning");
        }
    }

    IEnumerator DeactivateSpawning()
    {
        canSpawnMinions = false;
        yield return new WaitForSeconds(MinionsDelay);
        canSpawnMinions = true;
    }

    IEnumerator DeactivateMeteorsSpawning()
    {
        canSpawnMeteors = false;
        yield return new WaitForSeconds(MeteorDelay);
        canSpawnMeteors = true;
    }
}
