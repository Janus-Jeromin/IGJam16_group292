using System;
using UnityEngine;
using UnityEngine.InputSystem;
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
        [SerializeField] private string _menuSceneName = "Main Menu";

        private bool _hasWon = false;

        public void KillPlayer()
        {
            ResetPlayer();
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ResetPlayer();
        }
        
        private void Update()
        {
            // TODO use input actions instead
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (!string.IsNullOrEmpty(_menuSceneName))
                {
                    SceneManager.LoadScene(_menuSceneName);
                    return;
                }
                else
                    Debug.Log("There is no main menu set.");
            }
            
            // Check if player reached the end point
            float distanceToEndPoint = Vector3.Distance(Player.position, _endPoint.position);
            if (distanceToEndPoint <= _endPointDetectionRadius)
            {
                OnGameWon();
            }
        }

        private void ResetPlayer()
        {
            _player.GetComponent<PlayerCharacter>().ResetPlayer(_startPoint.position, _startPoint.rotation);
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