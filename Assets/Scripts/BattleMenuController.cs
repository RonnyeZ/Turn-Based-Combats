using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public enum BattleState
{
    Start,
    PlayerTurn,
    EnemyTurn,
    EndTurn,
    Victory,
    Defeat
}



public enum EffectTiming
{
    StartRound,
    EndRound
}



public enum ActionEffect
{
    Damage,
    Heal,
    Defense,
    Buff,
    Debuff
}



    [System.Serializable]
    public class StatusEffect
    {

    public string effectName;

    [TextArea]
    public string description;

    public int duration;

    public int power;

    public EffectTiming timing;

    }



    [System.Serializable]
    public class BattleAction
    {

    public string actionName;

    public string type;



    [Header("Power/Damage")]

    public int minPower;

    public int maxPower;



    public int accuracy;



    public ActionEffect effectType;



    [Header("Combat")]

    public int priority;

    public bool targetsEnemy=true;



    [Header("Status Effect")]

    public bool applyEffect;

    public StatusEffect effectData;



    [TextArea]
    public string description;



    [Header("Dialogue Return")]

    [TextArea]
    public string battleLogMessage;

}



public class BattleMenuController : MonoBehaviour
{

    #region Battle System

    public BattleState currentState;



    private BattleAction selectedPlayerAction;

    private BattleAction selectedEnemyAction;



    private List<StatusEffect>
    playerEffects=
    new List<StatusEffect>();



    private List<StatusEffect>
    enemyEffects=
    new List<StatusEffect>();

    #endregion





    private string currentMenu=null;



    private BattleAction selectedAttack;

    private BattleAction selectedSkill;

    private BattleAction selectedItem;



    [Header("UI")]

    public Transform contentArea;

    public GameObject optionButtonPrefab;

    public TMP_Text dialogueText;

    public ScrollRect dialogueScrollRect;



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
    
    public TMP_Text enemyHPValueText;

    public TMP_Text enemyConditionText;

    public Slider enemyHPBar;



    [Header("Jogador")]

    public int HP=100;

    public int maxHP=100;

    public int level=7;



    [Header("Enemy Data")]

    public string enemyName="Goblin";

    public int enemyLevel=3;



    [Header("Inimigo")]

    public int enemyHP=50;

    public int enemyMaxHP=50;


    [Header("Enemy AI")]

    public int enemyLowHPThreshold = 15;


    [Header("Ações do Jogador")]

    public List<BattleAction> attacks;

    public List<BattleAction> skills;

    public List<BattleAction> items;


    [Header("Ações do Inimigo")]

    public List<BattleAction> enemyActions;


    [Header("Player Pattern Memory")]

    public int playerDamageActionsUsed;
    public int playerHealActionsUsed;
    public int playerDefenseActionsUsed;
    public int playerBuffActionsUsed;
    public int playerDebuffActionsUsed;



    Dictionary<BattleAction,Button>
    currentButtons=
    new Dictionary<BattleAction,Button>();





    void Start()
    {

        currentState=
        BattleState.Start;



        dialogueText.text="";



        StartBattle();

    }





    void StartBattle()
    {

        AddBattleLog
        (
            "A batalha começou!"
        );



        UpdateEnemyHUD();



        StartRound();

    }





    void StartRound()
    {

        ProcessEffects
        (
            playerEffects,
            true,
            EffectTiming.StartRound
        );



        ProcessEffects
        (
            enemyEffects,
            false,
            EffectTiming.StartRound
        );



        currentState=
        BattleState.PlayerTurn;



        ShowPlayerStatus();

        UpdateEnemyHUD();

    }





    void EndRound()
    {

        ProcessEffects
        (
            playerEffects,
            true,
            EffectTiming.EndRound
        );



        ProcessEffects
        (
            enemyEffects,
            false,
            EffectTiming.EndRound
        );



        ReduceEffectDuration
        (
            playerEffects
        );



        ReduceEffectDuration
        (
            enemyEffects
        );



        RemoveExpiredEffects
        (
            playerEffects
        );



        RemoveExpiredEffects
        (
            enemyEffects
        );



        CheckBattleState();



        if(currentState!=BattleState.Victory
        &&
        currentState!=BattleState.Defeat)
        {

            StartRound();

        }

    }





    void UpdateEnemyHUD()
    {

        if(enemyNameText!=null)
        enemyNameText.text=
        enemyName;



        if(enemyLevelText!=null)
        enemyLevelText.text=
        "Lv."+enemyLevel;



        if(enemyHPText!=null)
        enemyHPText.text=

        enemyHP+

        "/"+

        enemyMaxHP;



        if(enemyHPBar!=null)
        enemyHPBar.value=

        (float)enemyHP/

        enemyMaxHP;



        string conditions=
        "";



        foreach(StatusEffect effect
                in enemyEffects)
        {

            conditions +=

            effect.effectName+

            " ("+

            effect.duration+

            ")\n";

        }



        if(conditions=="")
        {

            conditions=
            "Nenhuma";

        }



        if(enemyConditionText!=null)
        enemyConditionText.text=
        conditions;

        if(enemyHPValueText != null)
        {
            enemyHPValueText.text =
            enemyHP +
            "/" +
            enemyMaxHP;
        }

    }





    void AddBattleLog
    (
        string message
    )
    {

        if(dialogueText.text=="")
        {

            dialogueText.text=
            message;

        }

        else
        {

            dialogueText.text +=

            "\n" +

            message;

        }



        Canvas.ForceUpdateCanvases();



        if(dialogueScrollRect!=null)
        {

            dialogueScrollRect
            .verticalNormalizedPosition=0f;

        }

    }





    void ShowPlayerStatus()
    {

        if(playerStatusScroll!=null)
        playerStatusScroll.SetActive(true);



        if(actionStatusScroll!=null)
        actionStatusScroll.SetActive(false);



        string conditionText=
        "";



        foreach(StatusEffect effect
                in playerEffects)
        {

            conditionText +=

            effect.effectName+

            " ("+

            effect.duration+

            ")\n";

        }



        if(conditionText=="")
        {
            conditionText="";
        }



        if(playerHPText!=null)
        {

            playerHPText.text=

            HP+

            "/"+

            maxHP;

        }



        if(playerHPBar!=null)
        {

            playerHPBar.value=

            (float)HP/

            maxHP;

        }



        if(playerLevelText!=null)
        {

            playerLevelText.text=

            "Lv."+level;

        }



        if(playerConditionText!=null)
        {

            playerConditionText.text=
            conditionText;

        }

    }





    public void OpenAttacks()
    {
        ToggleMenu
        (
            "Attack",
            attacks
        );
    }



    public void OpenSkills()
    {
        ToggleMenu
        (
            "Skill",
            skills
        );
    }



    public void OpenItems()
    {
        ToggleMenu
        (
            "Item",
            items
        );
    }



    public void Run()
    {

        AddBattleLog
        (
            "Tentou fugir!"
        );

    }





    void ToggleMenu
    (
        string menuName,
        List<BattleAction> actions
    )
    {

        if(currentMenu==menuName)
        {

            currentMenu=null;

            ClearButtons();

            ShowPlayerStatus();

            return;

        }


        currentMenu=menuName;

        ShowButtons(actions);

        RestoreSelection();

    }





    void ShowButtons
    (
        List<BattleAction> actions
    )
    {

        ClearButtons();


        foreach
        (
            BattleAction action
            in actions
        )
        {

            GameObject newButton=

            Instantiate
            (
                optionButtonPrefab,
                contentArea
            );


            newButton
            .GetComponentInChildren<TMP_Text>()
            .text=
            action.actionName;



            Button button=

            newButton
            .GetComponent<Button>();



            currentButtons[action]=button;



            button.onClick
            .AddListener
            (
                ()=>SelectAction(action)
            );

        }

    }





    void SelectAction
    (
        BattleAction action
    )
    {

        BattleAction selected=
        GetCurrentSelection();



        if(selected==action)
        {

            selectedPlayerAction=
            action;

            RegisterPlayerAction(action);


            AddBattleLog
            (
                "Você escolheu "
                +action.actionName
            );



            currentState=
            BattleState.EnemyTurn;



            EnemyChooseAction();

            return;

        }


        SaveSelection(action);

        HighlightSelection();

        ShowActionInfo(action);

    }





    void ShowActionInfo
    (
        BattleAction action
    )
    {

        if(playerStatusScroll!=null)
        playerStatusScroll.SetActive(false);



        if(actionStatusScroll!=null)
        actionStatusScroll.SetActive(true);



        string info="";


        info += action.actionName;



        if(action.type!="")
        {
            info +=
            "\nTipo: "
            + action.type;
        }



        if(action.minPower>0 || action.maxPower>0)
        {

            string powerLabel=
            "Power";


            if(currentMenu=="Attack")
            {
                powerLabel=
                "Damage";
            }


            string powerValue;


            if(action.minPower==
            action.maxPower)
            {

                powerValue=
                action.minPower
                .ToString();

            }

            else
            {

                powerValue=

                action.minPower+

                "-"+

                action.maxPower;

            }



            info +=

            "\n"+

            powerLabel+

            ": "+

            powerValue;

        }



        if(action.accuracy>0)
        {

            info +=

            "\nPrecisão: "+

            action.accuracy;

        }



        if(action.description!="")
        {

            info +=

            "\n\n"+

            action.description;

        }



        if(actionStatusText!=null)
        {

            actionStatusText.text=
            info;

        }

    }





    void EnemyChooseAction()
    {
        if(enemyActions.Count <= 0)
        {
            AddBattleLog("O inimigo não possui ações!");
            return;
        }

        List<BattleAction> possibleActions =
        new List<BattleAction>();

        if(enemyHP <= enemyLowHPThreshold)
        {
            foreach(BattleAction action in enemyActions)
            {
                if(action.effectType == ActionEffect.Heal)
                {
                    possibleActions.Add(action);
                }
            }
        }

        if(possibleActions.Count == 0)
        {
            possibleActions = enemyActions;
        }

        selectedEnemyAction =
        possibleActions
        [
            Random.Range
            (
                0,
                possibleActions.Count
            )
        ];

        AddBattleLog("O inimigo escolheu uma ação!");

        currentState =
        BattleState.EndTurn;

        ExecuteTurn();
    }





    void ExecuteTurn()
    {

        List<BattleAction> turnActions=
        new List<BattleAction>();



        turnActions.Add
        (
            selectedPlayerAction
        );



        turnActions.Add
        (
            selectedEnemyAction
        );



        turnActions.Sort
        (
            (a,b)=>
            b.priority.CompareTo(a.priority)
        );



        foreach(BattleAction action
        in turnActions)
        {
            bool isPlayerAction =
            action == selectedPlayerAction;

            ExecuteAction
            (
                action,
                isPlayerAction
            );
        }



        EndRound();

    }





    void ExecuteAction
    (
        BattleAction action,
        bool isPlayerAction
    )
    {
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

        if(action.accuracy > 0)
        {
            int roll = Random.Range(1, 101);

            if(roll > action.accuracy)
            {
                AddBattleLog(action.actionName + " errou!");
                return;
            }
        }

        string finalMessage =
        action.battleLogMessage;

        finalMessage =
        finalMessage.Replace
        (
            "{value}",
            effectValue.ToString()
        );



        switch(action.effectType)
        {

            case ActionEffect.Damage:

                if(isPlayerAction)
                {
                    int reduction =
                    GetDefenseReduction
                    (
                        enemyEffects
                    );

                    int finalDamage =
                    effectValue - reduction;

                    if(finalDamage < 0)
                    finalDamage = 0;

                    enemyHP -= finalDamage;

                    if(enemyHP < 0)
                    enemyHP = 0;

                    effectValue = finalDamage;
                }

                else
                {
                    int reduction =
                    GetDefenseReduction
                    (
                        playerEffects
                    );

                    int finalDamage =
                    effectValue - reduction;

                    if(finalDamage < 0)
                    finalDamage = 0;

                    HP -= finalDamage;

                    if(HP < 0)
                    HP = 0;

                    effectValue = finalDamage;
                }

            break;

            case ActionEffect.Heal:

                if(isPlayerAction)
                {
                    HP += effectValue;

                    if(HP > maxHP)
                    HP = maxHP;
                }

                else
                {
                    enemyHP += effectValue;

                    if(enemyHP > enemyMaxHP)
                    enemyHP = enemyMaxHP;
                }

            break;

        }



        AddBattleLog(finalMessage);

        ShowPlayerStatus();

        UpdateEnemyHUD();
    }





    void ProcessEffects
    (
        List<StatusEffect> effects,
        bool isPlayer,
        EffectTiming timing
    )
    {

        foreach(StatusEffect effect
                in effects)
        {

            if(effect.timing!=timing)
            continue;



            switch(effect.effectName)
            {

                case "Poison":

                    if(isPlayer)
                    {

                        HP-=effect.power;

                        AddBattleLog
                        (
                            "O jogador sofreu "
                            +effect.power+
                            " de dano venenoso!"
                        );

                    }

                    else
                    {

                        enemyHP-=effect.power;

                        AddBattleLog
                        (
                            "O inimigo sofreu "
                            +effect.power+
                            " de dano venenoso!"
                        );

                    }

                break;

            }

        }

    }





    void ReduceEffectDuration
    (
        List<StatusEffect> effects
    )
    {

        foreach(StatusEffect effect
                in effects)
        {

            effect.duration--;

        }

    }





    void RemoveExpiredEffects
    (
        List<StatusEffect> effects
    )
    {

        effects.RemoveAll
        (
            effect => effect.duration<=0
        );

    }





    void CheckBattleState()
    {

        if(enemyHP<=0)
        {

            currentState=
            BattleState.Victory;

            AddBattleLog
            (
                "Vitória!"
            );

            return;

        }



        if(HP<=0)
        {

            currentState=
            BattleState.Defeat;

            AddBattleLog
            (
                "Derrota!"
            );

        }

    }





    void SaveSelection
    (
        BattleAction action
    )
    {

        if(currentMenu=="Attack")
        selectedAttack=
        action;



        if(currentMenu=="Skill")
        selectedSkill=
        action;



        if(currentMenu=="Item")
        selectedItem=
        action;

    }





    BattleAction GetCurrentSelection()
    {

        if(currentMenu=="Attack")
        return selectedAttack;


        if(currentMenu=="Skill")
        return selectedSkill;


        if(currentMenu=="Item")
        return selectedItem;


        return null;

    }





    void HighlightSelection()
    {

        foreach
        (
            var button
            in currentButtons
        )
        {

            button
            .Value
            .image
            .color=

            Color.white;

        }



        BattleAction selected=

        GetCurrentSelection();



        if(selected!=null)
        {

            currentButtons
            [selected]
            .image
            .color=

            Color.red;

        }

    }





    void RestoreSelection()
    {

        if(GetCurrentSelection()!=null)
        {

            HighlightSelection();

            ShowActionInfo
            (
                GetCurrentSelection()
            );

        }

    }





    void ClearButtons()
    {

        currentButtons.Clear();


        foreach
        (
            Transform child
            in contentArea
        )
        {

            Destroy
            (
                child.gameObject
            );

        }

    }

    int GetDefenseReduction(List<StatusEffect> effects)
    {
        int reduction = 0;

        foreach(StatusEffect effect in effects)
        {
            if(effect.effectName == "Defense")
            {
                reduction += effect.power;
            }
        }

        return reduction;
    }
    

    void RegisterPlayerAction
    (
        BattleAction action
    )
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