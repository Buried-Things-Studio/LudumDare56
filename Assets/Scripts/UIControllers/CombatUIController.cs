using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatUIController : MonoBehaviour
{
    [SerializeField] private BugInfoContainer _playerBugInfoContainer;
    [SerializeField] private BugInfoContainer _npcBugInfoContainer;

    [SerializeField] private BattleOptions _battleOptions;
    [SerializeField] private GameObject _battleOptionsObject;

    [SerializeField] private MoveOptions _moveOptions;
    [SerializeField] private GameObject _moveOptionsObject;

    [SerializeField] private ItemOptions _itemOptions;
    [SerializeField] private GameObject _itemOptionsObject;

    [SerializeField] private BugMenu _bugMenu;
    [SerializeField] private GameObject _bugMenuObject;

    private Player _player;
    private CombatController _combatController;


    public void InitializeCombatUI(CombatController combatController, Player player, Critter playerCritter, Critter npcCritter)
    {
        _combatController = combatController;
        _player = player;
        _playerBugInfoContainer.PopulateBugData(playerCritter);
        _npcBugInfoContainer.PopulateBugData(npcCritter);

        SetInactiveAllMenus();
        _battleOptionsObject.SetActive(true);
        _battleOptions.SetSelectedIndexToMove();
        _battleOptionsObject.SetActive(false);
    }


    private void FinishInteraction()
    {
        SetInactiveAllMenus();
    }


    private void SetInactiveAllMenus()
    {
        _battleOptionsObject.SetActive(false);
        _moveOptionsObject.SetActive(false);
        _itemOptionsObject.SetActive(false);
        _bugMenuObject.SetActive(false);
    }


    public void StartPlayerBattleActionChoice()
    {
        SetInactiveAllMenus();
        _battleOptionsObject.SetActive(true);
        _battleOptions.ShowCurrentSelection();

        StartCoroutine(BattleOptionsInteraction());
    }


    private IEnumerator BattleOptionsInteraction()
    {
        yield return new WaitForEndOfFrame();
        
        bool isSelected = false;
        
        while (!isSelected)
        {
            if (Input.GetKeyDown(Controls.MenuUpKey))
            {
                _battleOptions.MoveSelection(true);
            }
            else if (Input.GetKeyDown(Controls.MenuDownKey))
            {
                _battleOptions.MoveSelection(false);
            }
            else if (Input.GetKeyDown(Controls.MenuSelectKey))
            {
                isSelected = TrySelectBattleOption();
            }

            yield return null;
        }
    }


    private bool TrySelectBattleOption()
    {
        BattleOption selection = _battleOptions.GetSelectionBattleOption();

        if (selection == BattleOption.Item && _player.GetItems().Count == 0)
        {
            return false;
        }

        if (selection == BattleOption.Move)
        {
            StartPlayerMoveChoice();
        }
        else if (selection == BattleOption.Bugs)
        {
            StartPlayerBugMenuInteraction();
        }

        //TODO: allow item selection
        //TODO: allow active switch

        return true;
    }


    public void StartPlayerMoveChoice()
    {
        SetInactiveAllMenus();
        _moveOptionsObject.SetActive(true);
        _moveOptions.PopulateMoves(_player.GetActiveCritter());
        _moveOptions.ShowCurrentSelection();

        StartCoroutine(MoveOptionsInteraction());
    }


    private IEnumerator MoveOptionsInteraction()
    {
        yield return new WaitForEndOfFrame();
        
        //TODO: handle no PP on any move
        
        bool isSelected = false;
        
        while (!isSelected)
        {
            if (Input.GetKeyDown(Controls.MenuUpKey))
            {
                _moveOptions.MoveSelection(true);
            }
            else if (Input.GetKeyDown(Controls.MenuDownKey))
            {
                _moveOptions.MoveSelection(false);
            }
            else if (Input.GetKeyDown(Controls.MenuSelectKey))
            {
                isSelected = TrySelectMoveOption();
            }
            else if (Input.GetKeyDown(Controls.MenuBackKey))
            {
                isSelected = true;
                StartPlayerBattleActionChoice();
            }

            yield return null;
        }
    }


    private bool TrySelectMoveOption()
    {
        MoveID selectedMoveID = _moveOptions.GetSelectedMove();
        Move selectedMove = _player.GetActiveCritter().Moves.Find(move => move.ID == selectedMoveID);

        if (selectedMove.CurrentUses <= 0)
        {
            return false;
        }

        _combatController.SetPlayerMove(selectedMoveID);
        FinishInteraction();

        return true;
    }


    private void StartPlayerBugMenuInteraction()
    {
        SetInactiveAllMenus();
        _bugMenuObject.SetActive(true);
        _bugMenu.PopulateCritters(_player.GetCritters());
        _bugMenu.ShowCurrentSelection();

        StartCoroutine(BugMenuInteraction());
    }


    private IEnumerator BugMenuInteraction()
    {
        yield return StartCoroutine(_bugMenu.PlayerInteraction());

        StartPlayerBattleActionChoice(); //TODO: handle menu choices
    }
}
