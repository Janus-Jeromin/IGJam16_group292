using System;
using UnityEngine;

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
                ObGameWon();
            }
        }
        
        private void ObGameWon()
        {
            throw new NotImplementedException();
        }
    }
}