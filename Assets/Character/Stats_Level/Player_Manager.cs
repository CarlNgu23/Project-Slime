using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player_Manager : MonoBehaviour
{
    public Player_Manager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
    }

    //When a monster die, the ExpManager will become enabled and references ExpCheck.
    private void OnEnable()
    {
        ExpManager.Instance.OnReward += ExpCheck;
    }
    //The ExpManager will become disabled when nothing happens.
    private void OnDisable()
    {
        ExpManager.Instance.OnReward -= ExpCheck;
    }

    private void ExpCheck(int expReward)
    {
        Stats.Instance.currentExp += expReward;
        while (Stats.Instance.currentExp >= Stats.Instance.requiredExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        UpdateStats();
    }

    public void UpdateStats()
    {
        Stats.Instance.level += 1;
        Stats.Instance.health += 50;
        Stats.Instance.attack += 1;
        Stats.Instance.defense += 1;
        Stats.Instance.strength += 1;
        Stats.Instance.dexterity += 1;
        Stats.Instance.currentExp -= Stats.Instance.requiredExp;
        Stats.Instance.requiredExp += Stats.Instance.requiredExp + ((int)Math.Pow(Stats.Instance.level, 4) / 4);
    }
}
