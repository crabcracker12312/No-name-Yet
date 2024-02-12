using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpPower;

    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _velocity;

    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;
    public Animator slash;

    [SerializeField] private float smoothTime = 0.05f;
    private float _currentVelocity;

    [SerializeField] private float speed;

    private void Start()
    {
        slash = gameObject.GetComponent<Animator>();
    }
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            slash.SetTrigger("Active");
        }
        
        ApplyGravity();
        ApplyRotation();
        ApplyMovement();
        
    }
    private void ApplyGravity()
    {
        if (IsGrounded() &&  _velocity < 0.0f)
        {
            _velocity = -1.0f;
        }
        else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }
       
        _direction.y = _velocity;
    }
    private void ApplyRotation()
    {
        if (_input.sqrMagnitude == 0) return;

        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

    }
    private void ApplyMovement()
    {
        _characterController.Move(_direction * speed * Time.deltaTime);
    }
    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0.0f, _input.y);
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!IsGrounded()) return;

        _velocity += jumpPower;
    }
    private bool IsGrounded() => _characterController.isGrounded;
}
