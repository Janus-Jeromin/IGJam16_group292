using UnityEngine;

/// <summary>
/// A singular game object that simply plays a song in a loop throughout the game
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer _instance;
    
    private void Awake()
    {
        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
