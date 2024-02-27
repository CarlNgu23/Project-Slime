using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ecape_UI_Buttons_Controller : MonoBehaviour
{
    private Button saveButton;
    private Button loadButton;
    private Button quitButton;
    [SerializeField] private GameObject save_UI;
    [SerializeField] private GameObject load_UI;
    // Start is called before the first frame update

    private void Start()
    {
        
    }

    private void Update()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        saveButton = root.Q<Button>("Save");
        loadButton = root.Q<Button>("Load");
        quitButton = root.Q<Button>("Quit");
        saveButton.clicked += Save;
        loadButton.clicked += Load;
        quitButton.clicked += Quit;

        if (Input.GetKeyDown(KeyCode.Escape) && (save_UI.activeSelf || load_UI.activeSelf))
        {
            if (save_UI.activeSelf)
            {
                save_UI.SetActive(false);
            }
            else if (load_UI.activeSelf)
            {
                load_UI.SetActive(false);
            }
        }
    }

    private void Save()
    {
        //Debug.Log("Save");
        save_UI.SetActive(true);
    }

    private void Load()
    {
        //Debug.Log("Load");
        load_UI.SetActive(true);
    }

    private void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
