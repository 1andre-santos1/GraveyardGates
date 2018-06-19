using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject NormalZombie;
    public GameObject QuickZombie;
    public GameObject Troll;
    public GameObject Hole;
    public GameObject Bomb;

    public int SecToDestroyHole = 5;

    public void SpawnNormalZombie(float posX)
    {
        GameObject zombie = Instantiate(NormalZombie) as GameObject;
        zombie.transform.position = new Vector3(posX,transform.position.y,transform.position.z);
        zombie.transform.rotation = new Quaternion(0, 180, 0, 0);
    }

    public void SpawnQuickZombie(float posX)
    {
        GameObject zombie = Instantiate(QuickZombie) as GameObject;
        zombie.transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        zombie.transform.rotation = new Quaternion(0, 180, 0, 0);
    }

    public void SpawnTroll(float posX)
    {
        GameObject zombie = Instantiate(Troll) as GameObject;
        zombie.transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        zombie.transform.rotation = new Quaternion(0, 180, 0, 0);
    }

    public void SpawnHole(float posX)
    {
        GameObject hole = Instantiate(Hole) as GameObject;
        hole.transform.position = new Vector3(posX, Hole.transform.position.y, Hole.transform.position.z);
        hole.transform.rotation = new Quaternion(0, 180, 0, 0);
        Destroy(hole,SecToDestroyHole);
    }

    public void SpawnBomb(float posX)
    {
        GameObject bomb = Instantiate(Bomb) as GameObject;
        bomb.transform.position = new Vector3(posX, Bomb.transform.position.y, Bomb.transform.position.z);
        bomb.transform.rotation = new Quaternion(0, 180, 0, 0);
    }

    public void SpawnMeteors()
    {
        GameObject.FindObjectOfType<MeteorSpawner>().SpawnMeteors();
    }
}
