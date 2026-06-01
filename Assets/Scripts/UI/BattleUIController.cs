using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIController : MonoBehaviour
{
    [Header("Dialogue")]
    public TMP_Text dialogueText;
    public ScrollRect dialogueScrollRect;

    [Header("Dialogue Timing")]
    public float messageDelay = 1.1f;

    private Queue<string> dialogueQueue =
    new Queue<string>();

    private bool isShowingMessage;



    [Header("Player Status Scroll")]
    public GameObject playerStatusScroll;
    public TMP_Text playerLevelText;
    public TMP_Text playerConditionText;
    public TMP_Text playerHPText;
    public Slider playerHPBar;



    [Header("Action Status Scroll")]
    public GameObject actionStatusScroll;
    public TMP_Text actionStatusText;



    [Header("Enemy HUD")]
    public TMP_Text enemyNameText;
    public TMP_Text enemyLevelText;
    public TMP_Text enemyHPText;
    public TMP_Text enemyConditionText;
    public Slider enemyHPBar;



    public void QueueBattleLog(string message)
    {
        dialogueQueue.Enqueue(message);

        if(!isShowingMessage)
        {
            StartCoroutine(ShowDialogueQueue());
        }
    }



    IEnumerator ShowDialogueQueue()
    {
        isShowingMessage = true;

        while(dialogueQueue.Count > 0)
        {
            string message =
            dialogueQueue.Dequeue();

            AddBattleLogInstant(message);

            yield return new WaitForSeconds(messageDelay);
        }

        isShowingMessage = false;
    }



    void AddBattleLogInstant(string message)
    {
        if(dialogueText.text == "")
        {
            dialogueText.text = message;
        }
        else
        {
            dialogueText.text += "\n" + message;
        }

        Canvas.ForceUpdateCanvases();

        if(dialogueScrollRect != null)
        {
            dialogueScrollRect.verticalNormalizedPosition = 0f;
        }
    }



    public void ClearDialogue()
    {
        dialogueText.text = "";
        dialogueQueue.Clear();
        isShowingMessage = false;
    }



    public void ShowPlayerStatus(BattleUnit player)
    {
        if(playerStatusScroll != null)
            playerStatusScroll.SetActive(true);

        if(actionStatusScroll != null)
            actionStatusScroll.SetActive(false);

        string conditionText = "";

        foreach(StatusEffectInstance effect in player.effects)
        {
            conditionText +=
            effect.data.effectName +
            " (" +
            effect.currentDuration +
            ")\n";
        }

        if(playerHPText != null)
            playerHPText.text = player.HP + "/" + player.maxHP;

        if(playerHPBar != null)
            playerHPBar.value = (float)player.HP / player.maxHP;

        if(playerLevelText != null)
            playerLevelText.text = "Lv." + player.level;

        if(playerConditionText != null)
            playerConditionText.text = conditionText;
    }



    public void ShowActionInfo(BattleActionData action, string currentMenu)
    {
        if(playerStatusScroll != null)
            playerStatusScroll.SetActive(false);

        if(actionStatusScroll != null)
            actionStatusScroll.SetActive(true);

        string info = action.actionName;

        if(action.type != "")
        {
            info += "\nTipo: " + action.type;
        }

        if(action.minPower > 0 || action.maxPower > 0)
        {
            string label =
            currentMenu == "Attack"
            ? "Damage"
            : "Power";

            string value =
            action.minPower == action.maxPower
            ? action.minPower.ToString()
            : action.minPower + "-" + action.maxPower;

            info += "\n" + label + ": " + value;
        }

        if(action.accuracy > 0)
        {
            info += "\nPrecisão: " + action.accuracy;
        }

        if(action.description != "")
        {
            info += "\n\n" + action.description;
        }

        if(actionStatusText != null)
            actionStatusText.text = info;
    }



    public void UpdateEnemyHUD(BattleUnit enemy)
    {
        if(enemyNameText != null)
            enemyNameText.text = enemy.unitName;

        if(enemyLevelText != null)
            enemyLevelText.text = "Lv." + enemy.level;

        if(enemyHPText != null)
            enemyHPText.text = enemy.HP + "/" + enemy.maxHP;

        if(enemyHPBar != null)
            enemyHPBar.value = (float)enemy.HP / enemy.maxHP;

        string conditions = "";

        foreach(StatusEffectInstance effect in enemy.effects)
        {
            conditions +=
            effect.data.effectName +
            " (" +
            effect.currentDuration +
            ")\n";
        }

        if(conditions == "")
        {
            conditions = "Nenhuma";
        }

        if(enemyConditionText != null)
            enemyConditionText.text = conditions;
    }
}