using System;
using DefaultNamespace;
using Unity.Mathematics.Geometry;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    private Collider2D _collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _collider = gameObject.GetComponent<Collider2D>();
        
        PhysicsMaterial2D material = new PhysicsMaterial2D();
        material.friction = 0;
        material.bounciness = 0;
        _collider.sharedMaterial = material;
    }

    /*private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.transform.position == GameLogic.Instance.Player.position)
        {
            // Make the collider bouncy when the player rotation matches the bouncer rotation
            float angleDifference = System.Math.Abs(Quaternion.Angle(GameLogic.Instance.Player.rotation, transform.rotation));
            if(angleDifference > GameLogic.Instance.WallBounceThresholdAngle)
            {
                Debug.Log("WallBounce");
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * GameLogic.Instance.WallBounce, ForceMode2D.Impulse);
            }
        }
    }*/
}
