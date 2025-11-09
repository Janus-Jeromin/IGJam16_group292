using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// For the hampter wheel level, this fake goal enables a list of gameobjects/components
/// when it is collided
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class FakeGoal : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToEnable;
    [SerializeField] private List<Behaviour> componentsToEnable;
    [SerializeField] private Rigidbody2D rigidBodyToEnable;

    private bool _wasAlreadyTriggered;

    private void Start()
    {
        _wasAlreadyTriggered = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_wasAlreadyTriggered)
            return;

        _wasAlreadyTriggered = true;

        foreach (var gameObjectToEnable in objectsToEnable)
        {
            gameObjectToEnable.SetActive(true);
        }
        foreach (var gameComponent in componentsToEnable)
        {
            gameComponent.enabled = true;
        }
        
        rigidBodyToEnable.bodyType = RigidbodyType2D.Dynamic;
        rigidBodyToEnable.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }
}
