using UnityEngine;

public class BattleEffects : MonoBehaviour
{
    public int GetDefenseBonus(BattleUnit unit)
    {
        int bonus = 0;

        foreach(StatusEffectInstance effect in unit.effects)
        {
            bonus += effect.data.defenseBonus;
        }

        return bonus;
    }

    public int GetAccuracyBonus(BattleUnit unit)
    {
        int bonus = 0;

        foreach(StatusEffectInstance effect in unit.effects)
        {
            bonus += effect.data.accuracyBonus;
        }

        return bonus;
    }

    public int GetPriorityBonus(BattleUnit unit)
    {
        int bonus = 0;

        foreach(StatusEffectInstance effect in unit.effects)
        {
            bonus += effect.data.priorityBonus;
        }

        return bonus;
    }

    public void ProcessEffects
    (
        BattleUnit unit,
        EffectTiming timing,
        BattleUIController ui
    )
    {
        foreach(StatusEffectInstance effect in unit.effects)
        {
            if(effect.data.timing != timing)
                continue;

            if(effect.data.hpDamage > 0)
            {
                unit.HP -= effect.data.hpDamage;

                if(unit.HP < 0)
                    unit.HP = 0;

                ui.AddBattleLog
                (
                    unit.unitName +
                    " sofreu " +
                    effect.data.hpDamage +
                    " de dano por " +
                    effect.data.effectName +
                    "!"
                );
            }

            if(effect.data.hpRecover > 0)
            {
                unit.HP += effect.data.hpRecover;

                if(unit.HP > unit.maxHP)
                    unit.HP = unit.maxHP;

                ui.AddBattleLog
                (
                    unit.unitName +
                    " recuperou " +
                    effect.data.hpRecover +
                    " HP!"
                );
            }
        }
    }

    public void ApplyEffect(BattleUnit target, StatusEffectData effectData)
    {
        if(effectData == null)
            return;

        target.effects.Add(new StatusEffectInstance(effectData));
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