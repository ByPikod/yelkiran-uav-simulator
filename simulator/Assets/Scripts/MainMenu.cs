using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Canvas canvas;
    CanvasGroup menuGroup;
    Animator settingsAnimator;

    [SerializeField]
    public GameObject menu;
    [SerializeField]
    public GameObject settings;
    [SerializeField]
    public Image blocked;

    // Start is called before the first frame update
    void Start()
    {
        this.canvas = this.gameObject.GetComponent<Canvas>();
        this.menuGroup = this.menu.GetComponent<CanvasGroup>();
        this.settingsAnimator = this.settings.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        this.escapeControl();
    }

    public void escapeControl()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (this.areSettingsOpened())
            {
                this.toggleSettings();
                return;
            }
            this.toggleMainmenu();
        }
    }

    bool areSettingsOpened()
    {
        return this.settingsAnimator.GetBool("enabled");
    }
    
    public void toggleMainmenu()
    {
        canvas.enabled = !canvas.enabled;
    }

    public void toggleSettings()
    {
        var enabled = !this.areSettingsOpened();
        this.settingsAnimator.SetBool("enabled", enabled);
        this.blocked.enabled = enabled;
        this.menuGroup.interactable = !enabled;
    }

    public void quit()
    {
        Application.Quit(0);
    }

}
