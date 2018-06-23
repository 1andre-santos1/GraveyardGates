using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public int MaxNumberOfMeteors = 8;
    public int MinNumberOfMeteors = 3;

    public GameObject Meteor;

    public void SpawnMeteors()
    {
        //encontrar bounds da camara
        Vector3 minPos = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, transform.position.z));
        Vector3 maxPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, transform.position.z));


        float minX = minPos.x;
        float maxX = maxPos.x;

        int numMeteors = Random.Range(MinNumberOfMeteors, MaxNumberOfMeteors + 1);
        for (int i = 0; i < numMeteors; i++)
        {
            Instantiate(Meteor, new Vector3(Random.Range(minX, maxX), transform.position.y,Meteor.transform.position.z), Quaternion.identity);
        }
    }
}
