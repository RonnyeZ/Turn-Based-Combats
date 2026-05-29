using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleMenuController : MonoBehaviour
{
    [Header("References")]
    public BattleSystem battleSystem;
    public BattleUIController battleUI;

    [Header("Buttons")]
    public Transform contentArea;
    public GameObject optionButtonPrefab;

    private string currentMenu = null;

    private BattleActionData selectedAttack;
    private BattleActionData selectedSkill;
    private BattleActionData selectedItem;

    private Dictionary<BattleActionData, Button>
    currentButtons =
    new Dictionary<BattleActionData, Button>();

    public void OpenAttacks()
    {
        ToggleMenu
        (
            "Attack",
            battleSystem.player.attacks
        );
    }

    public void OpenSkills()
    {
        ToggleMenu
        (
            "Skill",
            battleSystem.player.skills
        );
    }

    public void OpenItems()
    {
        ToggleMenu
        (
            "Item",
            battleSystem.player.items
        );
    }

    public void Run()
    {
        battleUI.AddBattleLog("Tentou fugir!");
    }

    void ToggleMenu
    (
        string menuName,
        List<BattleActionData> actions
    )
    {
        if(currentMenu == menuName)
        {
            currentMenu = null;
            ClearButtons();
            battleUI.ShowPlayerStatus(battleSystem.player);
            return;
        }

        currentMenu = menuName;

        ShowButtons(actions);

        RestoreSelection();
    }

    void ShowButtons(List<BattleActionData> actions)
    {
        ClearButtons();

        foreach(BattleActionData action in actions)
        {
            GameObject newButton =
            Instantiate(optionButtonPrefab, contentArea);

            newButton
            .GetComponentInChildren<TMP_Text>()
            .text = action.actionName;

            Button button =
            newButton.GetComponent<Button>();

            currentButtons[action] = button;

            button.onClick.AddListener
            (
                () => SelectAction(action)
            );
        }
    }

    void SelectAction(BattleActionData action)
    {
        BattleActionData selected =
        GetCurrentSelection();

        // Primeiro clique: seleciona e mostra status.
        // Segundo clique na mesma ação: confirma a ação.
        if(selected == action)
        {
            battleSystem.ChoosePlayerAction(action);
            return;
        }

        SaveSelection(action);

        HighlightSelection();

        battleUI.ShowActionInfo(action, currentMenu);
    }

    void SaveSelection(BattleActionData action)
    {
        if(currentMenu == "Attack")
        selectedAttack = action;

        if(currentMenu == "Skill")
        selectedSkill = action;

        if(currentMenu == "Item")
        selectedItem = action;
    }

    BattleActionData GetCurrentSelection()
    {
        if(currentMenu == "Attack")
        return selectedAttack;

        if(currentMenu == "Skill")
        return selectedSkill;

        if(currentMenu == "Item")
        return selectedItem;

        return null;
    }

    void HighlightSelection()
    {
        foreach(var button in currentButtons)
        {
            button.Value.image.color = Color.white;
        }

        BattleActionData selected =
        GetCurrentSelection();

        if(selected != null && currentButtons.ContainsKey(selected))
        {
            currentButtons[selected].image.color = Color.red;
        }
    }

    void RestoreSelection()
    {
        BattleActionData selection =
        GetCurrentSelection();

        if(selection != null)
        {
            HighlightSelection();
            battleUI.ShowActionInfo(selection, currentMenu);
        }
    }

    void ClearButtons()
    {
        currentButtons.Clear();

        foreach(Transform child in contentArea)
        {
            Destroy(child.gameObject);
        }
    }
}