using UnityEngine;

[CreateAssetMenu(fileName = "NewBattleAction", menuName = "Battle/Action")]
public class BattleActionData : ScriptableObject
{
    public string actionName;

    public string type;

    [Header("Power/Damage")]
    public int minPower;
    public int maxPower;

    [Header("PP")]
    public bool usePP;
    public int maxPP;

    [Header("Accuracy")]
    public int accuracy;

    [Header("Combat")]
    public ActionEffect effectType;
    public TargetType targetType;
    public int priority;

    [Header("Status Effect")]
    public bool applyEffect;
    public StatusEffectData effectData;

    [TextArea]
    public string description;

    [Header("Dialogue Return")]
    [TextArea]
    public string battleLogMessage;
}