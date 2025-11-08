using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _loadOverlay;
    
    public void OnStartClicked()
    {
        _loadOverlay.SetActive(true);
        SceneManager.LoadSceneAsync("Level1");
        this.gameObject.SetActive(false);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }

    private void Start()
    {
        _loadOverlay.SetActive(false);
    }
}