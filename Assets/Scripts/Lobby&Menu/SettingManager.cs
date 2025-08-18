using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;



public class SettingManager : MonoBehaviour
{
    [SerializeField] private Slider sensSlider;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    
    private Resolution[] resolutions;
    private int selectedResolution;
    private List<Resolution> selectedResolutionList =  new List<Resolution>();
    private float _sensitivity;
    private bool isFullscreen;
    private float masterVolume;
    
    void Start()
    {
        _sensitivity = PlayerPrefs.GetFloat("sensitivity", 1.0f);
        sensSlider.value = _sensitivity;

        isFullscreen = true;
        fullscreenToggle.isOn = isFullscreen;
        
        masterVolume = masterSlider.value;
        audioMixer.SetFloat("master", Mathf.Log10(masterVolume) * 20);
        
        resolutions = Screen.resolutions;
        
        List<string> resolutionStringList = new List<string>();
        string newRes;
        foreach (Resolution resolution in resolutions)
        {
            newRes = resolution.width.ToString() + " x " + resolution.height.ToString();
            if (!resolutionStringList.Contains(newRes))
            {
                resolutionStringList.Add(newRes);
                selectedResolutionList.Add(resolution);
            }
        }
        resolutionStringList.Reverse();
        selectedResolutionList.Reverse();
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionStringList);
        
        selectedResolution = resolutionDropdown.value;
        Screen.SetResolution(selectedResolutionList[selectedResolution].width, selectedResolutionList[selectedResolution].height, isFullscreen);
    }
    
    public void OnSensitivityChanged(float value)
    {
        _sensitivity = value;
        PlayerPrefs.SetFloat("sensitivity", _sensitivity);
        PlayerPrefs.Save();
    }

    public void OnFullscreenChanged(bool value)
    {
        isFullscreen = !isFullscreen;   
        Screen.fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void OnVolumeChanged()
    {
        masterVolume = masterSlider.value;
        audioMixer.SetFloat("master", Mathf.Log10(masterVolume) * 20);
    }

    public void OnResolutionChanged()
    {
        selectedResolution = resolutionDropdown.value;
        Screen.SetResolution(selectedResolutionList[selectedResolution].width, selectedResolutionList[selectedResolution].height, isFullscreen);
    }
}
