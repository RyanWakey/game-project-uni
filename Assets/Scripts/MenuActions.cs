using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;
public class MenuActions : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    public AudioMixer mainMixer;
    [SerializeField] private Slider volumeSlider;

private void Start()
    {
        InitializeResolutionDropdown();
        LoadValues();
    }
    public void StartGame()
    {
        GameManager.instance.LoadScene("Game");
    }

    public void OpenSettings()
    {
           
    }

    public void LoadProfile()
    {

    }

    private void InitializeResolutionDropdown()
    {
        resolutions = Screen.resolutions;

        // Clear the dropdown list
        resolutionDropdown.ClearOptions();

        // Populate the dropdown with supported resolutions
        int currentResolutionIndex = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Add the listener for when the dropdown value changes
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log("Resolution changed to: " + resolution.width + "x" + resolution.height);
    }
    
    public void SaveVolumeButton()
    {
        float volumeValue = volumeSlider.value;
        PlayerPrefs.SetFloat("VolumeValue", volumeValue);
        LoadValues();
    }
    public void LoadValues()
    {
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        volumeSlider.value = volumeValue;
        AudioListener.volume = volumeValue;
    }
}


