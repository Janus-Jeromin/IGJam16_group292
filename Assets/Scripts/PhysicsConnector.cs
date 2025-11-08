using System;
using DefaultNamespace;
using UnityEngine;

public class PhysicsConnector : MonoBehaviour
{
    // Reference to the player transform
    [SerializeField] private float _maxGravity = 9.81f;
    [SerializeField] private float _gravityLerpSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        var player = GameLogic.Instance.Player;
        
        // Change the direction of gravity based on the player's up vector
        var vecTarget = new Vector2(-player.up.x, -player.up.y) * _maxGravity;
        Physics2D.gravity = Vector2.Lerp(Physics2D.gravity, vecTarget, Time.deltaTime * _gravityLerpSpeed);
    }
}
