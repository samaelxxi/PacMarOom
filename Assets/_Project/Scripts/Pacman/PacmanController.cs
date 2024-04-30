using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PacmanController : MonoBehaviour
{
    public event Action OnShootClicked;
    public event Action<int> OnChangeWeaponClicked;

    CharacterController _controller;
    Vector3 _velocity;
    float _groundedTimer = 0f;
    float _verticalVelocity = 0f;

    PacmanStats _stats;

    Camera _camera;

    float _jumpClickedTime = 0f;
    bool _jumpRequested = false;

    public void Setup(PacmanStats stats)
    {
        _stats = stats;
    }

    void Awake()
    {
        _controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _camera = Camera.main;
        Debug.Log(_camera);
    }

    void Update()
    {
        Movement();
        Rotation();

        if (Input.GetButtonDown("Fire1"))
            OnShootClicked?.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha1))
            OnChangeWeaponClicked?.Invoke(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
            OnChangeWeaponClicked?.Invoke(1);
    }

    public void Teleport(Transform checkpoint)
    {
        _controller.transform.Rotate(checkpoint.localRotation.eulerAngles);
        _controller.enabled = false;
        transform.position = checkpoint.position;
        _controller.enabled = true;
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
    
        _verticalVelocity -= _stats.Gravity * Time.deltaTime;

        Vector3 move = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = Vector3.ClampMagnitude(move, 1);
        move = transform.TransformDirection(move);
        move = move.SetY(0);
        move.Normalize();
        // move = transform.TransformDirection(move);
        _controller.Move(_stats.Speed * Time.deltaTime * move);


        if (Input.GetButtonDown("Jump"))
        {
            _jumpClickedTime = Time.time;
            _jumpRequested = true;
        }

        if (_jumpRequested)
        {
            if (_groundedTimer > 0)
            {
                _groundedTimer = 0;
                _verticalVelocity += Mathf.Sqrt(_stats.JumpForce * 2 * _stats.Gravity);
                Game.Instance.AudioManager.Play("jump", pitch: UnityEngine.Random.Range(0.9f, 1.1f), volume: 0.7f);
                _jumpRequested = false;
            }
        }

        if (Time.time - _jumpClickedTime > _stats.JumpWindow)
            _jumpRequested = false;

        move.y = _verticalVelocity;

        _controller.Move(move * Time.deltaTime);
    }


    void Rotation()
    {
        Vector2 view = _camera.ScreenToViewportPoint( Input.mousePosition );
        bool isOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;
        if (isOutside)
            return;

        // Get the mouse's movement
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");



        // Apply the mouse movement as rotation
        Vector3 rotation = new Vector3(-mouseY, mouseX, 0) * _stats.MouseSensitivity * Time.deltaTime;

        // Apply the rotation to the player
        _controller.transform.Rotate(rotation);

        
        // Limit the rotation
        Vector3 currentAngles = _controller.transform.eulerAngles;
        if (currentAngles.x > 180f)
            currentAngles.x -= 360f;
        currentAngles.x = Mathf.Clamp(currentAngles.x, -60f, 60f);
        _controller.transform.eulerAngles = currentAngles;

        // Ensure the player's z rotation stays at 0
        _controller.transform.eulerAngles = new Vector3(_controller.transform.eulerAngles.x, _controller.transform.eulerAngles.y, 0);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // check collider layer
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Vector3 normal = hit.normal;
            Vector3 direction = (hit.gameObject.transform.position - transform.position).normalized;
            float normalDot = Vector3.Dot(normal, Vector3.up);
            float directionDot = Vector3.Dot(direction, Vector3.down);

            // Debug.Log($"Pacman hit enemy {hit.collider.gameObject.name} with normal {normal} and direction {direction} | normal dot: {normalDot} | direction dot: {directionDot}");
            if (normalDot > 0.5f && directionDot > 0.5f)
            {
                // Debug.Log("Pacman hit enemy from above");
                if (hit.collider.TryGetComponent(out Damageable damageable))
                {
                    // Debug.Log("Pacman hit enemy from above and it's damageable");
                    damageable.TakeDamage(_stats.JumpDamage);
                    _verticalVelocity = Mathf.Sqrt(_stats.JumpForce * _stats.Gravity);
                    Game.Instance.AudioManager.Play("jump", pitch: UnityEngine.Random.Range(0.9f, 1.1f), volume: 0.7f);
                }
            }
        }
    }
}
