using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int Cost = 10;
    public int Damage = 20;
    public float ExplosionRange = 4f;

    public GameObject ExplosionParticle;

    public void Explode()
    {
        Instantiate(ExplosionParticle, transform);
        GameObject player = GameObject.FindObjectOfType<Player>().gameObject;
        float playerX = player.transform.position.x;
        if ((playerX < transform.position.x && playerX >= transform.position.x - ExplosionRange) ||
            (playerX > transform.position.x && playerX <= transform.position.x + ExplosionRange))
            player.GetComponent<Player>().Health = player.GetComponent<Player>().Health - Damage;

        Enemy[] Enemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (var enemy in Enemies)
        {
            GameObject en = enemy.gameObject;
            float enX = en.transform.position.x;
            if ((enX < transform.position.x && enX >= transform.position.x - ExplosionRange) ||
                (enX > transform.position.x && enX <= transform.position.x + ExplosionRange))
                en.GetComponent<Enemy>().setHealth(en.GetComponent<Enemy>().getHealth() - Damage);
        }
        StartCoroutine("DestroyObject");
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(ExplosionParticle.GetComponent<ParticleSystem>().main.duration);
        Destroy(gameObject);
    }
}
