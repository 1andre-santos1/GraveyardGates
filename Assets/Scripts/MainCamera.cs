using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public bool IsFixed = true;

    private GameObject Player;
    private float speed = 1.0f;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (IsFixed)
            return;

        if (Player != null)
        {
            float interpolation = speed * Time.deltaTime;

            Vector3 position = this.transform.position;
            position.x = Mathf.Lerp(this.transform.position.x, Player.transform.position.x, interpolation);

            float currentX = Mathf.Clamp(position.x, Player.GetComponent<MoveFromInput>().MinimumX / 4f, Player.GetComponent<MoveFromInput>().MaximumX - (Player.GetComponent<MoveFromInput>().MaximumX / 8f));
            position.x = currentX;

            this.transform.position = position;
        }
    }
}
