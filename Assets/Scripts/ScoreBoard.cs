using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

/// <summary>
/// Displays the player's stats at the ending of the game.
/// </summary>
public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private string mainMenuScene = "Main Menu";
    [SerializeField] private AudioSource audioSourceCongrats;
    [SerializeField] private TextMeshProUGUI textLevelsCleared;
    [SerializeField] private TextMeshProUGUI textDeaths;
    [SerializeField] private TextMeshProUGUI textTime;

    public void Start()
    {
        if (ScoreManager.Instance == null)
        {
            GoBackToMainMenu();
            gameObject.SetActive(false);
        }
        else
        {
            ScoreManager.Instance.StopScore();
            
            var clearTime = ScoreManager.ClearTime;
            
            textLevelsCleared.text = ScoreManager.LevelsCleared.ToString();
            textDeaths.text = ScoreManager.PlayerDeaths.ToString();
            textTime.text = string.Format("{0:00}:{1:00}:{2:00}", clearTime.Hours, clearTime.Minutes, clearTime.Seconds);
            audioSourceCongrats.Play();
        }
    }

    public void Update()
    {
        // TODO use input actions instead
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            GoBackToMainMenu();
        }
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadSceneAsync(mainMenuScene);
    }
}
