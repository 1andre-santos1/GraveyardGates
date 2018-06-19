using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawner : MonoBehaviour
{
    public GameObject CrateHealth;
    public GameObject CrateShield;


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            Instantiate(CrateHealth, transform);
        }
        if (Input.GetKeyUp(KeyCode.Alpha9))
        {
            Instantiate(CrateShield, transform);
        }
    }
}
