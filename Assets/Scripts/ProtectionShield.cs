using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionShield : MonoBehaviour
{
    public AudioClip ON;
    public AudioClip OFF;
    public int SecondsTillDestroy = 5;

    private Player player;
    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        GetComponent<AudioSource>().clip = ON;
        GetComponent<AudioSource>().Play();
        StartCoroutine("Destroy");

    }

    private void Update()
    {
        gameObject.transform.position = new Vector3(player.transform.position.x,transform.position.y,transform.position.z);
        
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(SecondsTillDestroy);
        GetComponent<AudioSource>().clip = OFF;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        Destroy(gameObject);
    }
}
