using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    
    [SerializeField]
    public TMP_Dropdown frameMode;
    [SerializeField]
    public TMP_Dropdown resolution;

    void Start()
    {
        // Fill resolutions
        resolution.options.RemoveAll(match => true);
        var currentRes = Screen.currentResolution;
        foreach (var res in Screen.resolutions)
        {
            resolution.options.Add(new TMP_Dropdown.OptionData(res.width + "x" + res.height));
            if (currentRes.width == res.width && currentRes.height == res.height)
            {
                this.resolution.value = resolution.options.Count - 1;
            }
        }
       
        // Add event listeners
        frameMode.onValueChanged.AddListener(onFrameModeChanged);
        resolution.onValueChanged.AddListener(onResolutionChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onFrameModeChanged(int option)
    {
        switch (option)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }

    void onResolutionChange(int option)
    {
        Resolution res = Screen.resolutions[option];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
    }
    
}
