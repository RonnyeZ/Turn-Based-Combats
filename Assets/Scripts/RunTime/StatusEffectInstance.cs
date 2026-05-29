public class StatusEffectInstance
{
    public StatusEffectData data;
    public int currentDuration;

    public StatusEffectInstance(StatusEffectData effectData)
    {
        data = effectData;
        currentDuration = effectData.duration;
    }
}

public class BattleActionInstance
{
    public BattleActionData data;

    public int currentPP;

    public BattleActionInstance(BattleActionData actionData)
    {
        data = actionData;
        currentPP = actionData.maxPP;
    }
}