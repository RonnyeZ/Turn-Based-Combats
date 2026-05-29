using UnityEngine;

[CreateAssetMenu(fileName = "NewStatusEffect", menuName = "Battle/Status Effect")]
public class StatusEffectData : ScriptableObject
{
    public string effectName;

    [TextArea]
    public string description;

    public int duration;

    public int power;

    public EffectTiming timing;
}