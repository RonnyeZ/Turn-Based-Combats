using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BattleSystem : MonoBehaviour
{
    [Header("References")]
    public BattleUIController battleUI;
    public BattleEffects battleEffects;
    public EnemyAIController enemyAI;

    [Header("Units")]
    public BattleUnit player;
    public BattleUnit enemy;

    [Header("State")]
    public BattleState currentState;

    private BattleActionData selectedPlayerAction;
    private BattleActionData selectedEnemyAction;

    [Header("Player Pattern Memory")]
    public int playerDamageActionsUsed;
    public int playerHealActionsUsed;
    public int playerDefenseActionsUsed;
    public int playerBuffActionsUsed;
    public int playerDebuffActionsUsed;

    [Header("Battle Timing")]
    public float messageDelay = 1.0f;
    public float actionDelay = 1.2f;
    public float roundEndDelay = 0.8f;

    private bool isResolvingTurn;

    void Start()
    {
        currentState = BattleState.Start;

        if(battleUI.dialogueText != null)
        battleUI.dialogueText.text = "";

        StartBattle();
    }

    void StartBattle()
    {
        battleUI.QueueBattleLog("A batalha começou!");

        battleUI.UpdateEnemyHUD(enemy);

        StartRound();
    }

    void StartRound()
    {
        // Efeitos que ativam no começo da rodada.
        battleEffects.ProcessEffects
        (
            player,
            EffectTiming.StartRound,
            battleUI
        );

        battleEffects.ProcessEffects
        (
            enemy,
            EffectTiming.StartRound,
            battleUI
        );

        currentState = BattleState.PlayerTurn;

        battleUI.ShowPlayerStatus(player);
        battleUI.UpdateEnemyHUD(enemy);
    }

    void EndRound()
    {
        // Efeitos que ativam no fim da rodada.
        battleEffects.ProcessEffects
        (
            player,
            EffectTiming.EndRound,
            battleUI
        );

        battleEffects.ProcessEffects
        (
            enemy,
            EffectTiming.EndRound,
            battleUI
        );

        battleEffects.ReduceDurations(player);
        battleEffects.ReduceDurations(enemy);

        CheckBattleState();

        if(currentState != BattleState.Victory &&
           currentState != BattleState.Defeat)
        {
            StartRound();
        }
    }

    public void ChoosePlayerAction(BattleActionData action)
    {
        if(currentState != BattleState.PlayerTurn)
        return;

        selectedPlayerAction = action;

        RegisterPlayerAction(action);


        if(action.consumesTurn == false)
        {
            ExecuteAction
            (
                new BattleActionExecution
                (
                    action,
                    player,
                    GetTarget(player, enemy, action),
                    true
                )
            );

            battleUI.ShowPlayerStatus(player);
            battleUI.UpdateEnemyHUD(enemy);

            currentState = BattleState.PlayerTurn;

            return;
        }

        currentState = BattleState.EnemyTurn;

        EnemyChooseAction();
    }

    void EnemyChooseAction()
    {
        selectedEnemyAction =
        enemyAI.ChooseAction(enemy);

        if(selectedEnemyAction == null)
        {
            battleUI.QueueBattleLog("O inimigo não possui ações!");
            return;
        }

        currentState = BattleState.EndTurn;

        StartCoroutine(ExecuteTurnRoutine());
    }

    IEnumerator ExecuteTurnRoutine()
    {
        if(isResolvingTurn)
        yield break;

        isResolvingTurn = true;

        List<BattleActionExecution> turnActions =
        new List<BattleActionExecution>();

        turnActions.Add
        (
            new BattleActionExecution
            (
                selectedPlayerAction,
                player,
                GetTarget(player, enemy, selectedPlayerAction),
                true
            )
        );

        turnActions.Add
        (
            new BattleActionExecution
            (
                selectedEnemyAction,
                enemy,
                GetTarget(enemy, player, selectedEnemyAction),
                false
            )
        );

        turnActions.Sort
        (
            (a,b) =>
            b.action.priority.CompareTo(a.action.priority)
        );

        foreach(BattleActionExecution execution in turnActions)
        {
            if(execution.isPlayerAction)
            {
                battleUI.QueueBattleLog
                (
                    "Você usou " +
                    execution.action.actionName +
                    "!"
                );
            }
            else
            {
                battleUI.QueueBattleLog
                (
                    enemy.unitName +
                    " usou " +
                    execution.action.actionName +
                    "!"
                );
            }

            yield return new WaitForSeconds(messageDelay);

            ExecuteAction(execution);

            yield return new WaitForSeconds(actionDelay);

            CheckBattleState();

            if(currentState == BattleState.Victory ||
            currentState == BattleState.Defeat)
            {
                isResolvingTurn = false;
                yield break;
            }
        }

        yield return new WaitForSeconds(roundEndDelay);

        EndRound();

        isResolvingTurn = false;
    }

    BattleUnit GetTarget
    (
        BattleUnit user,
        BattleUnit opponent,
        BattleActionData action
    )
    {
        if(action.targetType == TargetType.Self)
        return user;

        return opponent;
    }

    void ExecuteAction(BattleActionExecution execution)
    {
        BattleActionData action = execution.action;
        BattleUnit user = execution.user;
        BattleUnit target = execution.target;

        int effectValue = 0;

        if(action.maxPower > 0)
        {
            effectValue =
            Random.Range
            (
                action.minPower,
                action.maxPower + 1
            );
        }

        // Accuracy funciona como porcentagem.
        // Accuracy 0 significa que a ação não testa erro.
        if(action.accuracy > 0)
        {
            int roll = Random.Range(1, 101);

            if(roll > action.accuracy)
            {
                battleUI.QueueBattleLog(action.actionName + " errou!");
                return;
            }
        }

        string finalMessage =
        action.battleLogMessage.Replace
        (
            "{value}",
            effectValue.ToString()
        );

        switch(action.effectType)
        {
            case ActionEffect.Damage:

                int reduction =
                battleEffects.GetDefenseBonus(target);
                
                int finalDamage =
                effectValue - reduction;

                if(finalDamage < 0)
                finalDamage = 0;

                target.HP -= finalDamage;

                if(target.HP < 0)
                target.HP = 0;

                finalMessage =
                action.battleLogMessage.Replace
                (
                    "{value}",
                    finalDamage.ToString()
                );

            break;

            case ActionEffect.Heal:

                target.HP += effectValue;

                if(target.HP > target.maxHP)
                target.HP = target.maxHP;

            break;
        }

        if(action.applyEffect)
        {
            battleEffects.ApplyEffect
            (
                target,
                action.effectData
            );
        }

        battleUI.QueueBattleLog(finalMessage);

        battleUI.ShowPlayerStatus(player);
        battleUI.UpdateEnemyHUD(enemy);
    }

    void CheckBattleState()
    {
        if(enemy.IsDead())
        {
            currentState = BattleState.Victory;
            battleUI.QueueBattleLog("Vitória!");
            return;
        }

        if(player.IsDead())
        {
            currentState = BattleState.Defeat;
            battleUI.QueueBattleLog("Derrota!");
        }
    }

    void RegisterPlayerAction(BattleActionData action)
    {
        switch(action.effectType)
        {
            case ActionEffect.Damage:
                playerDamageActionsUsed++;
            break;

            case ActionEffect.Heal:
                playerHealActionsUsed++;
            break;

            case ActionEffect.Defense:
                playerDefenseActionsUsed++;
            break;

            case ActionEffect.Buff:
                playerBuffActionsUsed++;
            break;

            case ActionEffect.Debuff:
                playerDebuffActionsUsed++;
            break;
        }
    }
}