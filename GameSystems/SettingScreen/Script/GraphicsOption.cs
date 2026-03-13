using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GraphicsOption : MonoBehaviour
{
    // Resolution
    [SerializeField] private TMP_Dropdown resolutionDropDown;
    List<Resolution> resolutions = new List<Resolution>();

    // ScreenMode
    [SerializeField] private TMP_Dropdown screenModeDropDown;
    FullScreenMode screenMode;

    private void Start()
    {
        Init();
    }

    void Init()
    {
        resolutions.AddRange(Screen.resolutions);
        resolutionDropDown.options.Clear();

        int dropdownCnt = 0;
        foreach(Resolution temp in resolutions)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = temp.width + " X " + temp.height + " " + temp.refreshRateRatio + "Hz";
            resolutionDropDown.options.Add(optionData);
            if (temp.width == Screen.width && temp.height == Screen.height) resolutionDropDown.value = dropdownCnt;
            dropdownCnt++;
        }
        resolutionDropDown.RefreshShownValue();

        screenModeDropDown.options.Clear();

        List<TMP_Dropdown.OptionData> screenModeOptions = new List<TMP_Dropdown.OptionData>
        {
            new TMP_Dropdown.OptionData("Fullscreen"),
            new TMP_Dropdown.OptionData("Fullscreen Window"),
            new TMP_Dropdown.OptionData("Windowed")
        };
        screenModeDropDown.options = screenModeOptions;
        screenModeDropDown.value = GetScreenModeIndex(Screen.fullScreenMode);
    }

    public void ResolutionChange(int x)
    {
        Screen.SetResolution(resolutions[x].width, resolutions[x].height, screenMode);
    }
    public void ScreenModeChange(int x)
    {
        screenMode = Screen.fullScreenMode = GetFullScreenMode(x);
    }

    private FullScreenMode GetFullScreenMode(int index)
    {
        switch (index)
        {
            case 0:
                return FullScreenMode.ExclusiveFullScreen;
            case 1:
                return FullScreenMode.FullScreenWindow;
            case 2:
                return FullScreenMode.Windowed;
            default:
                return FullScreenMode.FullScreenWindow; 
        }
    }
    private int GetScreenModeIndex(FullScreenMode mode)
    {
        switch (mode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                return 0;
            case FullScreenMode.FullScreenWindow:
                return 1;
            case FullScreenMode.Windowed:
                return 2;
            default:
                return 0;
        }
    }

}
