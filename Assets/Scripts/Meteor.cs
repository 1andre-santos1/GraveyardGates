using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public int Damage = 50;

    public GameObject ExplosionParticle;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Explode();
        }
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            Explode();
        }
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Health = collision.gameObject.GetComponent<Player>().Health - Damage;
            Explode();
        }
    }

    public void Explode()
    {
        Instantiate(ExplosionParticle);
        Destroy(gameObject);
    }
}
