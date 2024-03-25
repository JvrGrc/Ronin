using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
   
    [SerializeField]AudioMixer mixer;
    [SerializeField] GameObject audioicon;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] Sprite mute;
    [SerializeField] Sprite audiol;
    [SerializeField] Sprite audioh;
    [SerializeField] Dropdown resolutionDrop;
    [SerializeField] Slider sliderAudio;
    [SerializeField] Image toggleImage;
    private Resolution[] resolutions;
    private float volum;
    
    private void Start()
    {
        sliderAudio.value = PlayerPrefs.GetFloat("save", volum);
        resolutions = Screen.resolutions;
        resolutionDrop.ClearOptions();
        List<string> options = new List<string>();
        int currentResolution = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width+" x " + resolutions[i].height + " @ " + resolutions[i].refreshRateRatio + "hz";
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }

        }
        resolutionDrop.AddOptions(options);
        resolutionDrop.value = currentResolution;
        resolutionDrop.RefreshShownValue();
    }
    public void setVolume(float volume)
    {
        audioicon.SetActive(true);
        if (volume == -80)
        {
            
            audioicon.GetComponent<Image>().sprite = mute; 
        }else if(volume < -40)
        {
            audioicon.GetComponent<Image>().sprite = audiol;
        }
        else
        {
            audioicon.GetComponent<Image>().sprite = audioh;

        }
        mixer.SetFloat("volume", volume);
        volum = volume;
        PlayerPrefs.SetFloat("save", volum);
    }
    public void setFullScreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
        
        if (fullscreen)
        {
            toggleImage.sprite = Resources.Load<Sprite>("HUD/ToggleSelected");
        }
        else
        {
            toggleImage.sprite = Resources.Load<Sprite>("HUD/ToggleUnselected");
        }
    }
    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void ActivateSettings()
    {
        settingsPanel.SetActive(true);
    }
    public void DesactivateSettings()
    {
        settingsPanel.SetActive(false);
    }
}
