using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    // base attribute
    private int baseHp = 100;          //base hp
    private int baseAttackDmg = 10;  // base attack damage
    private int baseDefense = 5;     // base defense reduce your damage taken by 1

    //attribute, get from leveling or gear
    private int stamina = 0;    // each stamaina point add 4 hp to your base hp
    private int speed = 1;      // each speed point add 0.01 to your moving speed
    private int strength = 0;      // each power point add 1 base attack damage
    private int reduction = 0;  // each reduction point reduce the damage you take by 1

    //player final  attribute
    private int hp = 100;
    private int AttackDmg = 10;
    private int Defense = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
