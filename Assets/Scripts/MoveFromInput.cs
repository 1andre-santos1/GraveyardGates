using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFromInput : MonoBehaviour
{
    public bool IsFixedToCameraBounds = true;

    /// <summary>
    /// Speed of this game object
    /// </summary>
    public float Speed;

    public float JumpPower = 10f;
    /// <summary>
    /// Minimum allowable X where the player can move
    /// </summary>
    public float MinimumX;
    /// <summary>
    /// Maximum allowable X where the player can move
    /// </summary>
    public float MaximumX;

    public bool isJumping = false;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) && !isJumping)
            Jump();

        //if the game object is not moving exit this frame
        if (!IsMoving())
            return;

        //move the game object
        Move();

        //keep the game object whitin the allowable are (between MinimumX & MaximumX)
        ClampPlayerPosition();
    }

    /// <summary>
    /// If it is given input to the game object on the X axis
    /// returns true
    /// </summary>
    public bool IsMoving()
    {
        return (Input.GetAxisRaw("Horizontal") != 0);
    }

    
    /// <summary>
    /// Move the game object on the X axis
    /// </summary>
    private void Move()
    {
        var movement = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

        if(movement.x < 0f)
            gameObject.transform.rotation = new Quaternion(0f,180f,0f,0f);
        else
            gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

        gameObject.transform.position += movement * Speed * Time.deltaTime;
        GameObject.FindObjectOfType<Arduino>().SpawningLight.transform.position += movement * Speed * Time.deltaTime;

        if (IsFixedToCameraBounds)
        {
            Vector3 minPos = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, transform.position.z));
            Vector3 maxPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, transform.position.z));

            float minX = minPos.x;
            float maxX = maxPos.x;

            var currentX = Mathf.Clamp(gameObject.transform.position.x, minX, maxX);
            gameObject.transform.position = new Vector3(currentX, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }

    private void Jump()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,JumpPower));
        isJumping = true;
    }

    /// <summary>
    /// Keep the game object position clamped to the allowable game area
    /// </summary>
    private void ClampPlayerPosition()
    {
        var currentX = Mathf.Clamp(gameObject.transform.position.x, MinimumX, MaximumX);
        gameObject.transform.position = new Vector3(currentX, gameObject.transform.position.y, gameObject.transform.position.z);
    }
}
