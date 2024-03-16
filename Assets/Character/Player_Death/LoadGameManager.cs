using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoadGameManager : MonoBehaviour, ILoad
{
    public static int c_Scene;
    public static LoadGameManager Instance;

    private Button loadSlotButton_1;
    private Button loadSlotButton_2;
    private Button loadSlotButton_3;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            LoadGameManager.Instance = this;
        }
        var root = GetComponent<UIDocument>().rootVisualElement;

        loadSlotButton_1 = root.Q<Button>("Load_Slot_1");
        loadSlotButton_2 = root.Q<Button>("Load_Slot_2");
        loadSlotButton_3 = root.Q<Button>("Load_Slot_3");
        loadSlotButton_1.clicked += () => LoadDataFromJson(Instance, "SaveData1.dat");
        loadSlotButton_2.clicked += () => LoadDataFromJson(Instance, "SaveData2.dat");
        loadSlotButton_3.clicked += () => LoadDataFromJson(Instance, "SaveData3.dat");
    }

    // Update is called once per frame
    void Update()
    {
    }

    private static void LoadDataFromJson(LoadGameManager i_LoadGameManager, string filename)
    {
        if (FileManager.ReadFromFile(filename, out var in_data))
        {
            SaveData data = new SaveData();
            data.LoadFromJson(in_data);

            i_LoadGameManager.Load_Data(data);
        }
        SceneManager.LoadScene(c_Scene);
        Time.timeScale = 1f;
        Debug.Log(filename + " Load Successfully");
    }

    public void Load_Data(SaveData saveData)
    {
        Stats.Instance.level = saveData.s_PlayerLevel;
        Stats.Instance.currentExp = saveData.s_currentExp;
        Stats.Instance.requiredExp = saveData.s_requiredExp;
        Stats.Instance.health = saveData.s_health;
        Stats.Instance.attack = saveData.s_attack;
        Stats.Instance.defense = saveData.s_defense;
        Stats.Instance.strength = saveData.s_strength;
        Stats.Instance.dexterity = saveData.s_dexterity;
        c_Scene = saveData.s_Scene;
        Debug.Log("Load Stats");
    }
}

