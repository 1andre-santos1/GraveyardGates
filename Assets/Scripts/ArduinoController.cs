using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoController : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM3", 9600);

    // Use this for initialization
    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (sp.IsOpen)
        {
            try
            {
                SetKeyPressed(sp.ReadByte()-'0');
                print(sp.ReadByte());
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }

    void SetKeyPressed(int keyIndex)
    {
        GetComponent<Arduino>().SetKeyPressed(keyIndex);
    }
}
