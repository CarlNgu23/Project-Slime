using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EscapeUIController : MonoBehaviour
{
    [SerializeField] private GameObject escape_UI;
    [SerializeField] private GameObject save_UI;
    [SerializeField] private GameObject load_UI;
    //private Button resumeButton;
    //private Button saveButton;
    //private Button loadButton;


    private void Awake()
    {   //game loads up all assets, so setting the SetActive to false to become inactive 
        escape_UI.SetActive(false);
        save_UI.SetActive(false);
        load_UI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !save_UI.activeSelf && !load_UI.activeSelf)
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
        escape_UI.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        escape_UI.SetActive(false);
    }
}
