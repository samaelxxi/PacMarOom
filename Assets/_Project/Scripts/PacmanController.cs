using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PacmanController : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    [SerializeField] float _mouseSensitivity = 1f;
    [SerializeField] float _jumpForce = 2.0f;
    [SerializeField] float _gravity = 9.81f;

    CharacterController _controller;
    Vector3 _velocity;
    float _groundedTimer = 0f;
    float _verticalVelocity = 0f;


    void Awake()
    {
        _controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Jump();
        Movement();
        Rotation();
    }

    void Movement2()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 move = new(horizontalInput, 0, verticalInput);
        move = Vector3.ClampMagnitude(move, 1);
        move = transform.TransformDirection(move) * _speed;

        // Apply gravity
        _velocity.y += _gravity * Time.deltaTime;

        // // If the character is grounded and not jumping, set the vertical velocity to keep the character on the ground
        // if (_controller.isGrounded && _velocity.y < 0)
        // {
        //     _velocity.y = -0.5f;
        // }


        if (Input.GetButtonDown("Jump") && _controller.isGrounded)
        {
            move.y += Mathf.Sqrt(_jumpForce * -3.0f * _gravity);
        }

        move.y += _gravity * Time.deltaTime;
        _controller.Move(move * Time.deltaTime);
    }

    void Movement()
    {
        bool isGrounded = _controller.isGrounded;

        if (isGrounded)
            _groundedTimer = 0.2f;
        if (_groundedTimer > 0)
            _groundedTimer -= Time.deltaTime;

        if (isGrounded && _verticalVelocity < 0)
            _verticalVelocity = 0f;
    
        _verticalVelocity -= _gravity * Time.deltaTime;

        Vector3 move = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = Vector3.ClampMagnitude(move, 1);
        move = transform.TransformDirection(move) * _speed;
        _controller.Move(_speed * Time.deltaTime * move);


        if (Input.GetButtonDown("Jump"))
        {
            if (_groundedTimer > 0)
            {
                _groundedTimer = 0;
                _verticalVelocity += Mathf.Sqrt(_jumpForce * 2 * _gravity);
            }
        }

        move.y = _verticalVelocity;

        _controller.Move(move * Time.deltaTime);
    }


    void Rotation()
    {
        // Get the mouse's movement
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Apply the mouse movement as rotation
        Vector3 rotation = new Vector3(-mouseY, mouseX, 0) * _mouseSensitivity;

        // Apply the rotation to the player
        _controller.transform.Rotate(rotation);

        // Ensure the player's z rotation stays at 0
        _controller.transform.eulerAngles = new Vector3(_controller.transform.eulerAngles.x, _controller.transform.eulerAngles.y, 0);
    }

    void Jump()
    {
        if (_controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            _velocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
        }
    }
}
