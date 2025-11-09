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
        [SerializeField] private AudioSource _audioSourceLevelStart;

        enum EnumGameState
        {
            eGameStateOnGoing,
            eGameStateWon,
            eGameStateLost,
        }
        
        private EnumGameState _gameState = EnumGameState.eGameStateOnGoing;
        
        private float _timeUntilReset = 1.5f;
 
        public void KillPlayer()
        {
            _player.GetComponent<PlayerCharacter>().Die();
            _gameState = EnumGameState.eGameStateLost;
            ScoreManager.Instance?.OnPlayerDeath();
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _player.position = _startPoint.position;
            _player.rotation = _startPoint.rotation;
            _audioSourceLevelStart.Play();
        }
        
        private void Update()
        {
            if (_gameState == EnumGameState.eGameStateLost)
            {
                _timeUntilReset -= Time.deltaTime;

                if (_timeUntilReset <= 0.0f)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
            
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
        
        private void OnGameWon()
        {
            if (_gameState == EnumGameState.eGameStateWon)
                return;
            
            _gameState = EnumGameState.eGameStateWon;
            
            ScoreManager.Instance?.OnLevelCleared();
            
            if (!string.IsNullOrEmpty(_nextLevelName)) 
                SceneManager.LoadSceneAsync(_nextLevelName);
            else
                Debug.Log("There is no next level.");
        }
    }
}