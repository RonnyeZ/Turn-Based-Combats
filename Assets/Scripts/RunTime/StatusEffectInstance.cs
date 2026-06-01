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