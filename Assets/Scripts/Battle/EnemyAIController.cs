using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    public int enemyLowHPThreshold = 15;

    public BattleActionData ChooseAction(BattleUnit enemy)
    {
        if(enemy.enemyActions.Count <= 0)
        return null;

        List<BattleActionData> possibleActions =
        new List<BattleActionData>();

        // IA simples: se o HP estiver baixo, tenta escolher uma cura.
        if(enemy.HP <= enemyLowHPThreshold)
        {
            foreach(BattleActionData action in enemy.enemyActions)
            {
                if(action.effectType == ActionEffect.Heal)
                {
                    possibleActions.Add(action);
                }
            }
        }

        if(possibleActions.Count == 0)
        {
            possibleActions = enemy.enemyActions;
        }

        return possibleActions
        [
            Random.Range(0, possibleActions.Count)
        ];
    }
}