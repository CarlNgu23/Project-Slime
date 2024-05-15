using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    public Slider slider;
    public void Awake()
    {
       // slider.maxValue = Stats.Instance.health;
    }

    public void Update()
    {
        slider.maxValue = Stats.Instance.requiredExp;
        slider.value = Stats.Instance.currentExp;
    }
}