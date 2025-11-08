using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _loadOverlay;
    [SerializeField] private string _startLevel;
    [SerializeField] private GameObject _main;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _creditsMenu;
    [SerializeField] private Slider _curseSlider;
    [SerializeField] private Slider _volumeSlider;

    private void Awake()
    {
        _main.SetActive(true);
        _loadOverlay.SetActive(false);
        _optionsMenu.SetActive(false);
        _creditsMenu.SetActive(false);
        
        _curseSlider.value = PlayerPrefs.GetFloat("Curse", 0.0f);
        _volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1.0f);
    }

    public void OnStartClicked()
    {
        _loadOverlay.SetActive(true);
        SceneManager.LoadSceneAsync(_startLevel);
        this.gameObject.SetActive(false);
    }

    public void OnOptionsClicked()
    {
        _optionsMenu.SetActive(true);
        _main.SetActive(false);
    }

    public void OnCurseSliderChanged()
    {
        PlayerPrefs.SetFloat("Curse", _curseSlider.value);
        PlayerPrefs.Save();
    }
    
    public void OnVolumeSliderChanged()
    {
        PlayerPrefs.SetFloat("Volume", _volumeSlider.value);
        PlayerPrefs.Save();
    }

    public void OnCreditsClicked()
    {
        _creditsMenu.SetActive(true);
        _main.SetActive(false);
    }

    public void OnCloseSubmenuClicked()
    {
        _optionsMenu.SetActive(false);
        _creditsMenu.SetActive(false);
        _main.SetActive(true);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}