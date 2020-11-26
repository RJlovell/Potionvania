using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public string menuScene;

    public AudioMixer audioMixer;
    public string audioMixerVariableName;
    public Slider slider;
    

    public Dropdown resolutionDropdown;
    Resolution[] resolutions;

    private void Start()
    {
        ///Gets the audio mixers values and outputs them into the outVolume variable
        audioMixer.GetFloat(audioMixerVariableName, out float outVolume);
        ///The sliders valume becomes the value that is stored in the audio mixer
        slider.value = outVolume;

        ///Gets the screen resolutions that the users computer can do and then goes through a for loop,
        ///which will display all the options a user can change their screen resolution to through the drop down menu.
        resolutions = Screen.resolutions;
        
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    /// <summary>
    /// To return to the main menu
    /// </summary>
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(menuScene);
    }
    /// <summary>
    /// Allows the user to change screen resolution
    /// </summary>
    /// <param name="resolutionIndex"></param>
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    /// <summary>
    /// Allows the user to change the sound of music playing throughout the game.
    /// Only accessible through the volume sliders in the Settings Scene and the Main Game Scene
    /// </summary>
    /// <param name="sliderValue"></param>
    public void SetVolume(float sliderValue)
    {
        audioMixer.SetFloat(audioMixerVariableName, sliderValue);
    }

    /// <summary>
    /// To set the screen to full screen or not
    /// </summary>
    /// <param name="isFullScreen"></param>
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
