using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] Songs;

    private void Start()
    {
        PlaySongs();
    }

    private void PlaySongs()
    {
        int numSong = Random.Range(0, Songs.Length);
        GetComponent<AudioSource>().clip = Songs[numSong];
        GetComponent<AudioSource>().Play();
        StartCoroutine("StartSongsEvent");
    }

    private IEnumerator StartSongsEvent()
    {
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        PlaySongs();
    }
}
