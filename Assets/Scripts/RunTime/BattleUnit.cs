using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [Header("Unit Data")]
    public string unitName;
    public int level;

    [Header("HP")]
    public int HP;
    public int maxHP;

    [Header("Actions")]
    public List<BattleActionData> attacks;
    public List<BattleActionData> skills;
    public List<BattleActionData> items;
    public List<BattleActionData> enemyActions;

    [HideInInspector]
    public List<StatusEffectInstance> effects =
    new List<StatusEffectInstance>();

    public bool IsDead()
    {
        return HP <= 0;
    }
}