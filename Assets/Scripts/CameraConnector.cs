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

    [SerializeField] private Transform _background1;
    [SerializeField] private Transform _background2;

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
        var oldPosition = _camera.transform.position;
        
        // Smoothly follow the player
        Vector3 vecPosition = Vector3.Lerp(_camera.transform.position, player.position, Time.fixedDeltaTime * _followSpeedMovement);
        Quaternion qtRotation = flipCamera ? Quaternion.FromToRotation(Vector3.forward, Vector3.down) * player.rotation : player.rotation;
        _camera.transform.position = new Vector3(vecPosition.x, player.position.y, _camera.transform.position.z);
        _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, qtRotation, Time.fixedDeltaTime * _followSpeedRotation);

        if (_background1 != null)
        {
            // Move the background to create a parallax effect
            Vector3 backgroundPosition = _background1.position;
            backgroundPosition.x += (_camera.transform.position.x - oldPosition.x) * 0.1f;
            backgroundPosition.y += (_camera.transform.position.y - oldPosition.y) * 0.1f;
            _background1.position = backgroundPosition;
        }

        if (_background2 != null)
        {
            Vector3 background2 = _background2.position;
            background2.x += (_camera.transform.position.x - oldPosition.x) * 0.2f;
            background2.y += (_camera.transform.position.y - oldPosition.y) * 0.2f;
            _background2.position = background2;
        }
    }
}
