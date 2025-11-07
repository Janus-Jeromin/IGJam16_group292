using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class to move the player character.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float kazooieTime;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float standAngle;
    
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private bool _grounded;
    private float _currentKazooieTime;
    private float _jumpCooldownTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _grounded = false;
        _currentKazooieTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        _currentKazooieTime -= Time.deltaTime;
        _jumpCooldownTime -= Time.deltaTime;
        
        CheckGrounded();
        
        // TODO use input actions instead of keys
        var keyboard = Keyboard.current;

        // Move left or right
        if (!keyboard.aKey.IsPressed() && keyboard.dKey.IsPressed())
        {
            // Change the speed in the local coordinate system
            Vector3 localVelocity = transform.InverseTransformVector(_rb.linearVelocity);
            localVelocity.x = horizontalSpeed;
            _rb.linearVelocity = transform.TransformVector(localVelocity);
            
            transform.Rotate(new Vector3(0, 0, 1), -rotationSpeed * Time.deltaTime);
        }
        else if (keyboard.aKey.IsPressed() && !keyboard.dKey.IsPressed())
        {
            // Change the speed in the local coordinate system
            Vector3 localVelocity = transform.InverseTransformVector(_rb.linearVelocity);
            localVelocity.x = -horizontalSpeed;
            _rb.linearVelocity = transform.TransformVector(localVelocity);
            
            transform.Rotate(new Vector3(0, 0, 1), rotationSpeed * Time.deltaTime);
        }
        
        // Jump
        if ((_grounded || _currentKazooieTime > 0.0f) && _jumpCooldownTime <= 0.0f && keyboard.spaceKey.wasPressedThisFrame)
        {
            _rb.AddForce(transform.TransformVector(Vector2.up) * jumpForce);
            
            _grounded = false;
            _currentKazooieTime = 0.0f;
            _jumpCooldownTime = jumpCooldown;
        }
    }
    
    void CheckGrounded()
    {
        ContactFilter2D filter2D = new ContactFilter2D();
        filter2D.useNormalAngle = true;
        filter2D.minNormalAngle = transform.eulerAngles.z + 90 - standAngle;
        filter2D.maxNormalAngle = transform.eulerAngles.z + 90 + standAngle;
        
        // https://discussions.unity.com/t/detect-when-player-is-touching-the-ground-for-2d/908738/9
        bool isOnGround = Physics2D.IsTouching(_collider, filter2D);
        
        if (isOnGround)
        {
            _grounded = true;
            
            // If we are on the ground and we did not just jump, also set the kazooie time
            if (_jumpCooldownTime <= 0.0f) 
                _currentKazooieTime = kazooieTime;
        }
        else
        {
            _grounded = false;
        }
    }
}
