using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.Events;
public class MenuActions : MonoBehaviour
{
    [SerializeField] private UnityEvent<KeyCode> OnKeyDetected;
    [SerializeField] private TextMeshProUGUI thrustKeyText;
    [SerializeField] private TextMeshProUGUI rotateLeftKeyText;
    [SerializeField] private TextMeshProUGUI rotateRightKeyText;
    [SerializeField] private TextMeshProUGUI fireKeyText;

    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button loadProfileButton;

    public Animator panelAnimatorMenu;
    public Animator panelAnimatorOptions;
    public Animator panelAnimatorProfile;

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject LoadProfilePanel;

    private bool isListeningForKey;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    public AudioMixer mainMixer;
    [SerializeField] private Slider volumeSlider;

    private PlayerMovementController player;
    private ProfileManager profileManager;


    public void Awake()
    {
        player = FindObjectOfType<PlayerMovementController>();
        profileManager = ProfileManager.instance;
        panelAnimatorMenu = mainMenuPanel.GetComponent<Animator>();
        panelAnimatorOptions = optionsPanel.GetComponent<Animator>();
        panelAnimatorProfile = LoadProfilePanel.GetComponent<Animator>();
    }
    private void Start()
    {
        InitializeResolutionDropdown();
        volumeSlider.onValueChanged.AddListener(SetVolume);
        LoadValues();
        UpdateKeyBindings();
        UpdateButtonTexts();
    }

    public void StartGame()
    {
        GameManager.instance.LoadScene("Game");
    }

    private void InitializeResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

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
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log("Resolution changed to: " + resolution.width + "x" + resolution.height);
    }

    public void SaveOptions()
    {
        int profileIndex = ProfileManager.instance.GetProfileIndex();
        string prefix = "Profile" + profileIndex + ",";

        float volumeValue = volumeSlider.value;
        PlayerPrefs.SetFloat("VolumeValue", volumeValue);

        PlayerPrefs.SetString(prefix + "Thrust", player.thrustKeyCode.ToString());
        PlayerPrefs.SetString(prefix + "RotateLeft", player.rotateLeftKeyCode.ToString());
        PlayerPrefs.SetString(prefix + "RotateRight", player.rotateRightKeyCode.ToString());
        PlayerPrefs.SetString(prefix + "Fire", player.fireKey.ToString());

        int currentResolution = resolutionDropdown.value;
        PlayerPrefs.SetInt(prefix + "Resolution", currentResolution);

        PlayerPrefs.Save();
    }

    public void SetVolume(float volumeValue)
    {
        AudioListener.volume = volumeValue;
    }
    public void LoadValues()
    {
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        volumeSlider.value = volumeValue;
        AudioListener.volume = volumeValue;
    }

    public void UpdateKeyBindings()
    {
        player.thrustKeyCode = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Thrust", "W"));
        player.rotateLeftKeyCode = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RotateLeft", "A"));
        player.rotateRightKeyCode = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RotateRight", "D"));
        player.fireKey = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Fire", "Mouse0"));
    }

    public void StartListeningForKey()
    {
        isListeningForKey = true;
        StartCoroutine(ListenForKey());
    }

    private IEnumerator ListenForKey()
    {
        while (isListeningForKey)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    OnKeyDetected.Invoke(key);
                    isListeningForKey = false;
                    break;
                }
            }

            yield return null;
        }
    }

    public void ChangeThrustKey()
    {
        StartListeningForKey();
        OnKeyDetected.AddListener(UpdateThrustKey);
    }

    private void UpdateThrustKey(KeyCode newKey)
    {
        SetKeyBinding("Thrust", newKey);
        UpdateKeyBindings();
        OnKeyDetected.RemoveListener(UpdateThrustKey);
        UpdateButtonTexts();
    }

    public void ChangeTurnRightKey()
    {
        StartListeningForKey();
        OnKeyDetected.AddListener(UpdateTurnRightKey);
    }

    private void UpdateTurnRightKey(KeyCode newKey)
    {
        SetKeyBinding("RotateRight", newKey);
        UpdateKeyBindings();
        OnKeyDetected.RemoveListener(UpdateTurnRightKey);
        UpdateButtonTexts();
    }

    public void ChangeTurnLefttKey()
    {
        StartListeningForKey();
        OnKeyDetected.AddListener(UpdateTurnLeftKey);
    }

    private void UpdateTurnLeftKey(KeyCode newKey)
    {
        SetKeyBinding("RotateLeft", newKey);
        UpdateKeyBindings();
        OnKeyDetected.RemoveListener(UpdateTurnLeftKey);
        UpdateButtonTexts();
    }

    public void ChangeFireKey()
    {
        StartListeningForKey();
        OnKeyDetected.AddListener(UpdateFireKey);
    }

    private void UpdateFireKey(KeyCode newKey)
    {
        SetKeyBinding("Fire", newKey);
        UpdateKeyBindings();
        OnKeyDetected.RemoveListener(UpdateFireKey);
        UpdateButtonTexts();
    }

    private void UpdateButtonTexts()
    {
        thrustKeyText.text = "Thrust Key: " + PlayerPrefs.GetString("Thrust", "W");
        rotateLeftKeyText.text = "Rotate Left Key: " + PlayerPrefs.GetString("RotateLeft", "A");
        rotateRightKeyText.text = "Rotate Right Key: " + PlayerPrefs.GetString("RotateRight", "D");
        fireKeyText.text = "Fire Key: " + PlayerPrefs.GetString("Fire", "Mouse0");
    }

    private void SetKeyBinding(string action, KeyCode newKey)
    {
        PlayerPrefs.SetString(action, newKey.ToString());
    }

    public void LoadProfileA()
    {
        LoadProfile(0);
    }

    public void LoadProfileB()
    {
        LoadProfile(1);
    }

    public void LoadProfileC()
    {
        LoadProfile(2);
    }

    private void LoadProfile(int profileIndex)
    {
        ProfileManager.instance.SetProfileIndex(profileIndex);
    }


    public void ShowMainMenuPanel()
    {
        panelAnimatorMenu.SetTrigger("Show");
      
    }

    public void HideMainMenu()
    {
        panelAnimatorMenu.SetTrigger("Hide");
    }

    public void ShowOptionsPanel()
    {
        panelAnimatorMenu.SetTrigger("Show");

    }

    public void HideOptionsPanel()
    {
        panelAnimatorMenu.SetTrigger("Hide");
    }

    public void ShowProfilePanel()
    {
        panelAnimatorMenu.SetTrigger("Show");

    }

    public void HideProfilePanel()
    {
        panelAnimatorMenu.SetTrigger("Hide");
    }
}


