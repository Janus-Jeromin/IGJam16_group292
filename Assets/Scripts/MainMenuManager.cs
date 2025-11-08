using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _loadOverlay;
    [SerializeField] private GameObject _main;
    [SerializeField] private Slider _curseSlider;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private AudioSource _audioSourceVolumeChanged;

    private List<GameObject> _listSubmenus;
    private bool _wasAwaken = false;

    private void Awake()
    {
        _main.SetActive(true);
        _loadOverlay.SetActive(false);
        
        _curseSlider.value = PlayerPrefs.GetFloat("Curse", 0.0f);
        _volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1.0f);
        _listSubmenus =  new List<GameObject>();

        _wasAwaken = true;
    }

    public void OnStartLevelClicked(string levelName)
    {
        _loadOverlay.SetActive(true);
        SceneManager.LoadSceneAsync(levelName);
        gameObject.SetActive(false);
    }

    public void OnOpenSubmenuClicked(GameObject submenu)
    {
        if (!_listSubmenus.Contains(submenu)) 
            _listSubmenus.Add(submenu);
        
        submenu.SetActive(true);
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

        AudioListener.volume = _volumeSlider.value;
        
        // Do not play the sound before we even started
        if (_wasAwaken && !_audioSourceVolumeChanged.isPlaying) 
            _audioSourceVolumeChanged.Play();
    }

    public void OnCloseSubmenuClicked()
    {
        foreach (var submenu in _listSubmenus)
        {
            submenu.SetActive(false);
        }
        _main.SetActive(true);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}