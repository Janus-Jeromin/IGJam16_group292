using System;
using DefaultNamespace;
using UnityEngine;

public class CameraConnector : MonoBehaviour
{
    public bool flipCamera = false;
    
    // Reference to the player transform
    [SerializeField] private Transform _camera;
    [SerializeField] private float _followSpeedRotation = 1.0f;
    [SerializeField] private float _followSpeedMovement = 4.0f;

    private void Start()
    {
        var player = GameLogic.Instance.Player;
        
        // Initialize camera position and rotation
        _camera.transform.position = new Vector3(player.position.x, player.position.y, _camera.transform.position.z);
        _camera.transform.rotation = player.rotation;
    }

    private void FixedUpdate()
    {
        var player = GameLogic.Instance.Player;
        
        // Smoothly follow the player
        Vector3 vecPosition = Vector3.Lerp(_camera.transform.position, player.position, Time.fixedDeltaTime * _followSpeedMovement);
        Quaternion qtRotation = flipCamera ? Quaternion.FromToRotation(Vector3.forward, Vector3.down) * player.rotation : player.rotation;
        _camera.transform.position = new Vector3(vecPosition.x, player.position.y, _camera.transform.position.z);
        _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, qtRotation, Time.fixedDeltaTime * _followSpeedRotation);
    }
}
