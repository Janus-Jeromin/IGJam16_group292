using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _loadOverlay;
    [SerializeField] private GameObject _main;
    [SerializeField] private Slider _curseSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private AudioSource _audioSourceVolumeChanged;
    [SerializeField] private AudioMixer _audioMixer;

    private List<GameObject> _listSubmenus;
    private bool _wasAwaken = false;

    private void Awake()
    {
        _main.SetActive(true);
        _loadOverlay.SetActive(false);
        
        _curseSlider.value = PlayerPrefs.GetFloat("Curse", 0.0f);
        _musicSlider.value = PlayerPrefs.GetFloat("Volume Music", 1.0f);
        _sfxSlider.value = PlayerPrefs.GetFloat("Volume SFX", 1.0f);
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
    
    public void OnVolumeMusicSliderChanged()
    {
        PlayerPrefs.SetFloat("Volume Music", _musicSlider.value);
        PlayerPrefs.Save();
        
        _audioMixer.SetFloat("Volume Music", CalcAttenuationForVolume(_musicSlider.value));
    }
    
    public void OnVolumeSFXSliderChanged()
    {
        PlayerPrefs.SetFloat("Volume SFX", _sfxSlider.value);
        PlayerPrefs.Save();

        _audioMixer.SetFloat("Volume SFX", CalcAttenuationForVolume(_sfxSlider.value));
        
        // Do not play the sound before we even started
        if (_wasAwaken && !_audioSourceVolumeChanged.isPlaying) 
            _audioSourceVolumeChanged.Play();
    }

    private float CalcAttenuationForVolume(float volume)
    {
        // TODO explain the maths here. Something about perceived volume is logarithmic
        return (float)Math.Log10(Math.Max(0.000001f, volume)) * 20;
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