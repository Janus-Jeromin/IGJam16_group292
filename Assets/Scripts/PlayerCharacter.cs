using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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
    
    // Maintain like here: https://stackoverflow.com/a/68654363
    private Dictionary<string, Collision2D> _currentCollisions;
    
    // Is only valid if there is any collision at all. Is updated each Update().
    // Values refer to the local coordinate system.
    
    private float _upmostCollisionNormalAngle; // The angle between the local up vector and the closest contact normal
    private float _leftmostCollisionNormalScalarProd; // The effect of the leftmost contact normal on the player. Is at least 0.
    private float _rightmostCollisionNormalScalarProd; // The effect of the rightmost contact normal on the player. Is at least 0.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _grounded = false;
        _currentKazooieTime = 0.0f;
        _jumpCooldownTime = 0.0f;
        _currentCollisions = new Dictionary<string, Collision2D>();
        _upmostCollisionNormalAngle = 0;
        _leftmostCollisionNormalScalarProd = -1;
        _rightmostCollisionNormalScalarProd = -1;
    }

    // Update is called once per frame
    void Update()
    {
        // We need to do this first because the results are used in other functions
        CheckCollisionNormals();
        
        _currentKazooieTime -= Time.deltaTime;
        _jumpCooldownTime -= Time.deltaTime;
        
        CheckGrounded();
        
        // TODO use input actions instead of keys
        var keyboard = Keyboard.current;

        // Move left
        if (keyboard.aKey.IsPressed() && !keyboard.dKey.IsPressed())
        {
            // Change the speed in the local coordinate system.
            // If we touch the left wall at an angle, slow down the speed correspondingly.
            Vector3 localVelocity = transform.InverseTransformVector(_rb.linearVelocity);
            localVelocity.x = -horizontalSpeed * (1.0f - _rightmostCollisionNormalScalarProd);
            _rb.linearVelocity = transform.TransformVector(localVelocity);
            
            transform.Rotate(new Vector3(0, 0, 1), rotationSpeed * Time.deltaTime);
        }
        // Move right
        if (!keyboard.aKey.IsPressed() && keyboard.dKey.IsPressed())
        {
            // Change the speed in the local coordinate system.
            // If we touch the right wall at an angle, slow down the speed correspondingly.
            Vector3 localVelocity = transform.InverseTransformVector(_rb.linearVelocity);
            localVelocity.x = horizontalSpeed * (1.0f - _leftmostCollisionNormalScalarProd);
            _rb.linearVelocity = transform.TransformVector(localVelocity);
            
            transform.Rotate(new Vector3(0, 0, 1), -rotationSpeed * Time.deltaTime);
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
    
    void OnCollisionEnter2D (Collision2D collision)
    {
        _currentCollisions[collision.collider.name] = collision;
    }
    
    void OnCollisionStay2D(Collision2D collision)
    {
        _currentCollisions[collision.collider.name] = collision;
    }
    
    void OnCollisionExit2D (Collision2D collision)
    {
        _currentCollisions.Remove(collision.collider.name);
    }

    void CheckCollisionNormals()
    {
        _upmostCollisionNormalAngle = 180;
        _leftmostCollisionNormalScalarProd = 0;
        _rightmostCollisionNormalScalarProd = 0;
        
        // TODO the list of collisions seems weird. When I expect two different contact point normals,
        //      there is only one normal but twice.
        foreach (var collision in _currentCollisions)
        {
            ContactPoint2D[] contactPoints = new ContactPoint2D[collision.Value.contactCount];
            collision.Value.GetContacts(contactPoints);
            
            foreach (var contactPoint in contactPoints)
            {
                Vector2 localNormal = transform.InverseTransformDirection(contactPoint.normal);
                
                float angleUp = Vector2.Angle(localNormal, Vector2.up);
                float scalarProdLeft = Vector2.Dot(localNormal, Vector2.left);
                float scalarProdRight = Vector2.Dot(localNormal, Vector2.right);

                _upmostCollisionNormalAngle = Math.Min(_upmostCollisionNormalAngle, angleUp);
                _leftmostCollisionNormalScalarProd = Math.Max(_leftmostCollisionNormalScalarProd, scalarProdLeft);
                _rightmostCollisionNormalScalarProd = Math.Max(_rightmostCollisionNormalScalarProd, scalarProdRight);
            }
        }
    }

    void CheckGrounded()
    {
        if (_currentCollisions.Count > 0 && _upmostCollisionNormalAngle <= standAngle)
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
