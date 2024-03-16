//Developed by Carl Ngu
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Main_Menu_Controller : MonoBehaviour
{
    [SerializeField] private GameObject load_UI;

    private Button new_game_button;
    private Button load_Button;
    private Button quit_Button;

    private void Awake()
    {
        load_UI.SetActive(false);
    }

    private void Start()
    {
        var main_menu_root = GetComponent<UIDocument>().rootVisualElement;
        new_game_button = main_menu_root.Q<Button>("New_Game");
        load_Button = main_menu_root.Q<Button>("Load");
        quit_Button = main_menu_root.Q<Button>("Quit");
        new_game_button.clicked += New_Game;
        load_Button.clicked += Load;
        quit_Button.clicked += Quit;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && load_UI.activeSelf)
        {
            load_UI.SetActive(false);
        }

    }

    private void New_Game()
    {
        SceneManager.LoadScene(1);
    }

    private void Load()
    {
        load_UI.SetActive(true);
    }

    private void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
