using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EscapeUIController : MonoBehaviour
{
    private VisualElement escape_UI_Menu;
    private UIDocument escapeUI_Menu;

    private Button resumeButton;
    private Button saveButton;
    private Button loadButton;
    private Button quitButton;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        escapeUI_Menu = GetComponent<UIDocument>();

        escape_UI_Menu = root.Q<VisualElement>("Escape_UI_Menu");

        resumeButton = root.Q<Button>("Resume");
        saveButton = root.Q<Button>("Save");
        loadButton = root.Q<Button>("Load");
        quitButton = root.Q<Button>("Quit");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale != 0)
            {
                Pause();
            }
            else
            {
                Resume();
            }
            
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        escapeUI_Menu.enabled = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        escapeUI_Menu.enabled = false;
    }
}
