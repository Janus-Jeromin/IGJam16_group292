using System;
using UnityEngine;

/// <summary>
/// During a playthrough, stores the stats of the 
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get;  private set; }

    private int _levelsCleared;
    private int _playerDeaths;
    private DateTime _startTime;
    private DateTime _endTime;
    
    public static int LevelsCleared => Instance._levelsCleared;
    public static int PlayerDeaths => Instance._playerDeaths;
    public static TimeSpan ClearTime => Instance._endTime - Instance._startTime;
    
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            
            // This will mostly be overwritten when the user starts a new game from the menu,
            // but it doesn't hurt to initialise the variables
            StartScore();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartScore()
    {
        _levelsCleared = 0;
        _playerDeaths = 0;
        _startTime = DateTime.Now;
    }

    public void OnPlayerDeath()
    {
        ++_playerDeaths;
    }
    
    public void OnLevelCleared()
    {
        ++_levelsCleared;
    }

    public void StopScore()
    {
        _endTime = DateTime.Now;
    }
}
