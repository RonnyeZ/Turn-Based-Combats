using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public enum ActionEffect
{
    Damage,
    Heal,
    Defense,
    Buff,
    Debuff
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


    [TextArea]
    public string description;


    [Header("Dialogue Return")]

    [TextArea]
    public string battleLogMessage;
}



public class BattleMenuController : MonoBehaviour
{

    private string currentMenu=null;


    private BattleAction selectedAttack;

    private BattleAction selectedSkill;

    private BattleAction selectedItem;



    [Header("UI")]

    public Transform contentArea;

    public GameObject optionButtonPrefab;

    public TMP_Text statusText;

    public TMP_Text dialogueText;

    public ScrollRect dialogueScrollRect;



    [Header("Jogador")]

    public int HP=100;

    public int maxHP=100;

    public int level=7;

    public string condition="Normal";



    [Header("Inimigo")]

    public int enemyHP=50;

    public int enemyMaxHP=50;



    [Header("Ações")]

    public List<BattleAction> attacks;

    public List<BattleAction> skills;

    public List<BattleAction> items;



    Dictionary<BattleAction,Button>
    currentButtons=
    new Dictionary<BattleAction,Button>();




    void Start()
    {

        dialogueText.text="";

        AddBattleLog
        (
            "A batalha começou!"
        );

        ShowPlayerStatus();

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

            "\n\n"+

            message;

        }



        Canvas.ForceUpdateCanvases();



        dialogueScrollRect
        .verticalNormalizedPosition=0f;

    }




    void ShowPlayerStatus()
    {

        statusText.text=

        "HP: "+HP+

        "\nNível: "+level+

        "\nCondição: "+condition;

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

            ExecuteAction(action);

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


        statusText.text=info;

    }




    void ExecuteAction
    (
        BattleAction action
    )
    {

        int effectValue=0;



        if(action.maxPower>0)
        {

            effectValue=

            Random.Range
            (
                action.minPower,
                action.maxPower+1
            );

        }



        string finalMessage=
        action.battleLogMessage;



        finalMessage=

        finalMessage.Replace
        (
            "{value}",
            effectValue.ToString()
        );



        switch(action.effectType)
        {

            case ActionEffect.Damage:

                enemyHP-=effectValue;


                if(enemyHP<0)
                enemyHP=0;

            break;





            case ActionEffect.Heal:

                HP+=effectValue;


                if(HP>maxHP)
                HP=maxHP;

            break;





            case ActionEffect.Defense:

            break;





            case ActionEffect.Buff:

            break;





            case ActionEffect.Debuff:

            break;

        }



        AddBattleLog(finalMessage);

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

}