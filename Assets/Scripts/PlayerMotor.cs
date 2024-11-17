﻿using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private float playerSpeed = 5.0f;
    private bool isGrounded;
    public float gravity=-9.8f;
    public float jumpHeight = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        isGrounded = controller.isGrounded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //recieves input from the input manager
    public void ProcessMove(Vector2 input )
    {
        Vector3 moveDirection=Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(moveDirection * playerSpeed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
        Debug.Log(playerVelocity.y);
    }
    public void Jump()
    {
        if (controller.isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
