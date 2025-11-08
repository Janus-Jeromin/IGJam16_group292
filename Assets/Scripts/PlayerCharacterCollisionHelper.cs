using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

/// <summary>
/// Helper for the player character that keeps track of collisions with the ground and floor.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCharacterCollisionHelper : MonoBehaviour
{
    // Maintain like here: https://stackoverflow.com/a/68654363
    public Dictionary<string, List<Vector2>> _currentContactNormals { get; private set; } // Global coordinates
    
    void Start()
    {
        _currentContactNormals = new Dictionary<string, List<Vector2>>();
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        UpdateCollision(collision, false);
    }
    
    void OnCollisionStay2D(Collision2D collision)
    {
        UpdateCollision(collision, false);
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        UpdateCollision(collision, true);
    }

    void UpdateCollision(Collision2D collision, bool bRemoveOnly)
    {
        _currentContactNormals.Remove(collision.collider.name);

        if (bRemoveOnly)
            return;
        
        List<Vector2> listContacts = new List<Vector2>();

        foreach (var contactPoint in collision.contacts)
            listContacts.Add(new Vector2(contactPoint.normal.x, contactPoint.normal.y));
        
        _currentContactNormals[collision.collider.name] = listContacts;
    }
}
