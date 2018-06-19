using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject Boss;
    public int SpawnDelay = 60;

    private void Start()
    {
        StartSpawningEvent();
    }

    public void SpawnBoss()
    {
        Instantiate(Boss, new Vector3(transform.position.x, transform.position.y, transform.position.z),Quaternion.identity);
        GameObject.FindObjectOfType<GameManager>().BossActive = true;
        ProtectionShield[] shields = GameObject.FindObjectsOfType<ProtectionShield>();
        foreach (var shield in shields)
            Destroy(shield.gameObject);
    }

    IEnumerator SpawnEvent()
    {
        yield return new WaitForSeconds(SpawnDelay);
        if(GameObject.FindObjectOfType<Boss>() == null)
            SpawnBoss();
        StartCoroutine(SpawnEvent());
    }

    public void StartSpawningEvent()
    {
        StartCoroutine("SpawnEvent");
    }
}
