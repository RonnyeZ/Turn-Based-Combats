using System.Collections.Generic;
using UnityEngine;

public class BattleEffects : MonoBehaviour
{
    public int GetDefenseReduction(List<StatusEffectInstance> effects)
    {
        int reduction = 0;

        foreach(StatusEffectInstance effect in effects)
        {
            if(effect.data.effectName == "Defense")
            {
                reduction += effect.data.power;
            }
        }

        return reduction;
    }

    public void ApplyEffect(BattleUnit target, StatusEffectData effectData)
    {
        if(effectData == null)
        return;

        target.effects.Add
        (
            new StatusEffectInstance(effectData)
        );
    }

    public void ProcessEffects
    (
        BattleUnit target,
        EffectTiming timing,
        BattleUIController ui
    )
    {
        foreach(StatusEffectInstance effect in target.effects)
        {
            if(effect.data.timing != timing)
            continue;

            if(effect.data.effectName == "Poison")
            {
                target.HP -= effect.data.power;

                if(target.HP < 0)
                target.HP = 0;

                ui.AddBattleLog
                (
                    target.unitName +
                    " sofreu " +
                    effect.data.power +
                    " de dano venenoso!"
                );
            }
        }
    }

    public void ReduceDurations(BattleUnit unit)
    {
        foreach(StatusEffectInstance effect in unit.effects)
        {
            effect.currentDuration--;
        }

        unit.effects.RemoveAll
        (
            effect => effect.currentDuration <= 0
        );
    }
}