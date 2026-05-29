using UnityEngine;

[CreateAssetMenu(fileName = "NewStatusEffect", menuName = "Battle/Status Effect")]
public class StatusEffectData : ScriptableObject
{
    public string effectName;

    [TextArea]
    public string description;

    public int duration;

    public EffectTiming timing;

    [Header("Round Effects")]
    public int hpDamage;
    public int hpRecover;
    public int ppRecover;

    [Header("Stat Modifiers")]
    public int defenseBonus;
    public int accuracyBonus;
    public int priorityBonus;
}