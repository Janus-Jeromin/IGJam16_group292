using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class GameLogic : MonoBehaviour
    {
        public static GameLogic Instance;
        
        public Transform Player => _player;

        public float WallBounce => _wallBounce;
        public float WallBounceThresholdAngle => _wallBounceThresholdAngle;
        
        
        [SerializeField] private Transform _player;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;
        [SerializeField] private float _wallBounceThresholdAngle = 45.0f;
        [SerializeField] private float _wallBounce = 10.0f;
        
        [SerializeField] private float _endPointDetectionRadius = 1.0f;
        [SerializeField] private string _nextLevelName;

        private bool _hasWon = false;
        
        private void Start()
        {
            _player.position = _startPoint.position;
            _player.rotation = _startPoint.rotation;
            
            Instance = this;
        }
        
        private void Update()
        {
            // Check if player reached the end point
            float distanceToEndPoint = Vector3.Distance(_player.position, _endPoint.position);
            if (distanceToEndPoint <= _endPointDetectionRadius)
            {
                OnGameWon();
            }
        }
        
        private void OnGameWon()
        {
            if (_hasWon)
                return;
            
            _hasWon = true;
            
            if (!string.IsNullOrEmpty(_nextLevelName)) 
                SceneManager.LoadScene(_nextLevelName);
            else
                Debug.Log("There is no next level.");
        }
    }
}