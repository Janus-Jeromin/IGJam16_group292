using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor.Animations;
using Random = UnityEngine.Random;

/// <summary>
/// Class to move the player character.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float kazooieTime;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float standAngle;
    [SerializeField] private float modelRotationSpeed;
    [SerializeField] private AudioSource audioSourceJump;
    [SerializeField] private AudioSource audioSourceDeath;
    [SerializeField] private Transform modelTransform;
    [SerializeField] private SpriteRenderer rendererModel;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerCharacterCollisionHelper collisionHelper;
    [SerializeField] private ParticleSystem explosion;
    
    private Rigidbody2D _rb;
    private bool _grounded;
    private float _currentKazooieTime;
    private float _jumpCooldownTime;
    private bool _dead;
    
    // Is only valid if there is any collision at all. Is updated each Update().
    // Values refer to the local coordinate system.
    
    private float _upmostCollisionNormalAngle; // The angle between the local up vector and the closest contact normal
    private float _effectiveMovementLeft; // Depending on the slope of the ground or wall, determines the effective horizontal movement.
    private float _effectiveMovementRight; // Depending on the slope of the ground or wall, determines the effective horizontal movement.

    // Controls
    private bool _inputJump;
    private bool _inputLeft;
    private bool _inputRight;
    
    // Animation trigger
    private int _animationTriggerWalk;
    private int _animationTriggerStand;
    private int _animationTriggerFall;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _grounded = false;
        _currentKazooieTime = 0.0f;
        _jumpCooldownTime = 0.0f;
        _dead = false;
        _upmostCollisionNormalAngle = 0;
        _effectiveMovementLeft = 0;
        _effectiveMovementRight = 0;
        
        _animationTriggerWalk = Animator.StringToHash("Start Walk");
        _animationTriggerStand = Animator.StringToHash("Start Standing");
        _animationTriggerFall = Animator.StringToHash("Start Falling");
    }

    private void Update()
    {
        // TODO use input actions instead of keys
        var keyboard = Keyboard.current; 
        
        // Continuous inputs are always updated, discrete ones are set once and reset in the fixed update
        if (keyboard.spaceKey.wasPressedThisFrame)
            _inputJump = true;
        
        _inputLeft = keyboard.aKey.IsPressed() && !keyboard.dKey.IsPressed();
        _inputRight = !keyboard.aKey.IsPressed() && keyboard.dKey.IsPressed();
    }

    void FixedUpdate()
    {
        // If we are dead, there is nothing to update here
        if (_dead)
            return;
        
        // For collision detection, the helper must be at the same spot as the controller
        collisionHelper.transform.SetPositionAndRotation(transform.position, transform.rotation);
        
        // We need to do this first because the results are used in other functions
        CheckCollisionNormals();
        
        _currentKazooieTime -= Time.fixedDeltaTime;
        _jumpCooldownTime -= Time.fixedDeltaTime;
        
        CheckGrounded();

        // Move left
        if (_inputLeft)
        {
            // Change the speed in the local coordinate system.
            // If we touch the left wall at an angle, slow down the speed correspondingly.
            Vector3 localVelocity = transform.InverseTransformVector(_rb.linearVelocity);
            localVelocity.x = -horizontalSpeed * _effectiveMovementLeft;
            _rb.linearVelocity = transform.TransformVector(localVelocity);
            
            transform.Rotate(new Vector3(0, 0, 1), rotationSpeed * Time.fixedDeltaTime);
            
            // Make the model face into the correct direction
            modelTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        // Move right
        if (_inputRight)
        {
            // Change the speed in the local coordinate system.
            // If we touch the right wall at an angle, slow down the speed correspondingly.
            Vector3 localVelocity = transform.InverseTransformVector(_rb.linearVelocity);
            localVelocity.x = horizontalSpeed * _effectiveMovementRight;
            _rb.linearVelocity = transform.TransformVector(localVelocity);
            
            transform.Rotate(new Vector3(0, 0, 1), -rotationSpeed * Time.fixedDeltaTime);
            
            // Make the model face into the correct direction
            modelTransform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        
        // Jump
        if ((_grounded || _currentKazooieTime > 0.0f) && _jumpCooldownTime <= 0.0f && _inputJump)
        {
            var curse = PlayerPrefs.GetFloat("Curse", 0.0f);
            var cursedJump = Vector2.up;
            cursedJump.x = Random.value * 2.0f * curse - curse;
            _rb.AddForce(transform.TransformVector(cursedJump) * jumpForce);
            
            _grounded = false;
            _currentKazooieTime = 0.0f;
            _jumpCooldownTime = jumpCooldown;

            // Either flip ourselves
            transform.Rotate(new Vector3(0, 0, 1), 180);
            
            // Play corresponding sound
            audioSourceJump.Play();
            
            // Since we ourselves rotated, we now rotate the model too, so that it is the same after jumping.
            // We then rotate it back in the following update intervals.
            modelTransform.Rotate(new Vector3(0, 0, 1), 180);
        }
        
        // Rotate the model back if it was rotated
        modelTransform.transform.rotation = Quaternion.Lerp(modelTransform.transform.rotation, transform.rotation, Time.fixedDeltaTime * modelRotationSpeed);

        // Trigger the correct animation for the current state
        if (!_grounded)
            animator.SetTrigger(_animationTriggerFall);
        else if (_inputLeft || _inputRight)
            animator.SetTrigger(_animationTriggerWalk);
        else
            animator.SetTrigger(_animationTriggerStand);
        
        // Clear discrete input
        _inputJump = false;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check collision with a danger zone
        if (collision.gameObject.layer == LayerMask.NameToLayer("Danger Zone"))
        {
            // Play the corresponding sound
            audioSourceDeath.Play();
            
            GameLogic.Instance.KillPlayer();
        }
    }

    public void Die()
    {
        GetComponent<ParticleSystem>().Play();
        GetComponent<AudioSource>().Play();
        rendererModel.enabled = false;
        _rb.simulated = false;
        _dead = true;
    }
    
    void CheckCollisionNormals()
    {
        _upmostCollisionNormalAngle = 180;
        _effectiveMovementLeft = 1;
        _effectiveMovementRight = 1;
        
        foreach (var normals in collisionHelper._currentContactNormals)
        {
            foreach (var normal in normals.Value)
            {
                Vector2 localNormal = transform.InverseTransformDirection(normal);
                
                // Calculate the angle between the local up and the nearest normal
                {
                    float angleUp = Vector2.Angle(localNormal, Vector2.up);
                    _upmostCollisionNormalAngle = Math.Min(_upmostCollisionNormalAngle, angleUp);
                }
                
                // Calculate the slope for moving left
                if (Vector2.Dot(localNormal, Vector2.right) >= 0.0f)
                {
                    float scalarProdTangentLeft = Math.Abs(Vector2.Dot(Quaternion.AngleAxis(90, Vector3.forward) * localNormal, Vector2.left));
                    _effectiveMovementLeft = Math.Min(_effectiveMovementLeft, scalarProdTangentLeft);
                }
                
                // Calculate the slop for moving right
                if (Vector2.Dot(localNormal, Vector2.left) >= 0.0f)
                {
                    float scalarProdTangentRight = Math.Abs(Vector2.Dot(Quaternion.AngleAxis(90, Vector3.forward) * localNormal, Vector2.right));
                    _effectiveMovementRight = Math.Min(_effectiveMovementRight, scalarProdTangentRight);
                }
            }
        }
    }

    void CheckGrounded()
    {
        if (collisionHelper._currentContactNormals.Count > 0 && _upmostCollisionNormalAngle <= standAngle)
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
