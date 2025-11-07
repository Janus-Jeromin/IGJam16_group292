using System;
using UnityEngine;

public class CameraConnector : MonoBehaviour
{
    // Reference to the player transform
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _followSpeed = 5.0f;

    private void Start()
    {
        // Initialize camera position and rotation
        _camera.transform.position = new Vector3(_player.position.x, _player.position.y, _camera.transform.position.z);
        _camera.transform.rotation = _player.rotation;
    }

    void Update()
    {
        // Smoothly follow the player
        Vector3 vecPosition = Vector3.Lerp(_camera.transform.position, _player.position, Time.deltaTime * _followSpeed);
        _camera.transform.position = new Vector3(vecPosition.x, vecPosition.y, _camera.transform.position.z);
        _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, _player.rotation, Time.deltaTime * _followSpeed);
    }
}
