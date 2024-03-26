//Developed by Carl Ngu
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public static Stats Instance;

    public int level;
    public int currentExp;
    public int requiredExp;
    public int health;
    public int attack;
    public int defense;
    public int strength;
    public int dexterity;

    public bool isLoaded;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            level = 1;
            currentExp = 0;
            requiredExp = 100;
            health = 100;
            attack = 5;
            defense = 0;
            strength = 1;
            dexterity = 1;
            Debug.Log("Stats Reset");
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(Instance);
    }

}
