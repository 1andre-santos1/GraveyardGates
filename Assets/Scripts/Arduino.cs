using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arduino : MonoBehaviour
{
    public GameObject SpawningLight;

    public int CooldownTime = 5;
    public float DistanceFromPlayer = 10f;

    private EnemySpawner enemySpawner;
    private GameObject player;
    private bool canSpawn = true;

    private int keyPressed = 0;

    private void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        player = GameObject.FindObjectOfType<Player>().gameObject;
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }
        if (GameObject.FindObjectOfType<GameManager>().BossActive)
        {
            SpawningLight.SetActive(false);
            return;
        }

        Vector3 pos = Camera.main.WorldToViewportPoint(SpawningLight.transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        SpawningLight.transform.position = Camera.main.ViewportToWorldPoint(pos);

        if (SpawningLight.transform.position.x <= player.transform.position.x && SpawningLight.transform.position.x >= player.transform.position.x - DistanceFromPlayer)
            SpawningLight.transform.position = new Vector2(player.transform.position.x - DistanceFromPlayer, SpawningLight.transform.position.y);

        if (SpawningLight.transform.position.x >= player.transform.position.x && SpawningLight.transform.position.x <= player.transform.position.x + DistanceFromPlayer)
            SpawningLight.transform.position = new Vector2(player.transform.position.x + DistanceFromPlayer, SpawningLight.transform.position.y);

        if (Input.GetKeyUp(KeyCode.Space) || keyPressed == 9)
        {
            SpawningLight.SetActive(!SpawningLight.activeInHierarchy);

            keyPressed = 0;
        }

        if (!SpawningLight.activeInHierarchy)
            return;

        if (Input.GetKeyUp(KeyCode.L) || keyPressed == 8)
        {
            SpawningLight.transform.position += new Vector3(2f, 0f, 0f);
            if(SpawningLight.transform.position.x <= player.transform.position.x + DistanceFromPlayer && SpawningLight.transform.position.x >= player.transform.position.x - DistanceFromPlayer)
                SpawningLight.transform.position = new Vector2(player.transform.position.x+DistanceFromPlayer, SpawningLight.transform.position.y);
            keyPressed = 0;
        }
        else if (Input.GetKeyUp(KeyCode.K) || keyPressed == 7)
        {
            SpawningLight.transform.position += new Vector3(-2f, 0f, 0f);
            if (SpawningLight.transform.position.x <= player.transform.position.x + DistanceFromPlayer && SpawningLight.transform.position.x >= player.transform.position.x - DistanceFromPlayer)
                SpawningLight.transform.position = new Vector2(player.transform.position.x - DistanceFromPlayer, SpawningLight.transform.position.y);
            keyPressed = 0;

        }

        else if (Input.GetKeyUp(KeyCode.Alpha1) && canSpawn || keyPressed == 1 && canSpawn)
        {
            Debug.Log("Normal Zombie Spawned!");
            enemySpawner.SpawnNormalZombie(SpawningLight.transform.position.x);
            canSpawn = false;
            StartCoroutine("CooldownSpawn");

            keyPressed = 0;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2) && canSpawn || keyPressed == 2 && canSpawn)
        {
            Debug.Log("Quick Zombie Spawned!");
            enemySpawner.SpawnQuickZombie(SpawningLight.transform.position.x);
            canSpawn = false;
            StartCoroutine("CooldownSpawn");

            keyPressed = 0;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3) && canSpawn || keyPressed == 3 && canSpawn)
        {
            Debug.Log("Troll Spawned!");
            enemySpawner.SpawnTroll(SpawningLight.transform.position.x);
            canSpawn = false;
            StartCoroutine("CooldownSpawn");

            keyPressed = 0;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4) && canSpawn)
        {
            enemySpawner.SpawnHole(SpawningLight.transform.position.x);
            canSpawn = false;
            StartCoroutine("CooldownSpawn");
        }

        else if (Input.GetKeyUp(KeyCode.Alpha5) && canSpawn)
        {
            enemySpawner.SpawnBomb(SpawningLight.transform.position.x);
            canSpawn = false;
            StartCoroutine("CooldownSpawn");
        }

        else if (Input.GetKeyUp(KeyCode.Alpha6) && canSpawn)
        {
            enemySpawner.SpawnMeteors();
            canSpawn = false;
            StartCoroutine("CooldownSpawn");
        }
    }

    private IEnumerator CooldownSpawn()
    {
        yield return new WaitForSeconds(CooldownTime);
        canSpawn = true;
    }

    public void SetKeyPressed(int index)
    {
        this.keyPressed = index;
    }

    public int GetKeyPressed()
    {
        return this.keyPressed;
    }
}
