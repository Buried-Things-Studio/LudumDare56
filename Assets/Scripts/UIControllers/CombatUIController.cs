using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public CombatVisualSteps VisualSteps;
    private Player _player;
    private CombatController _combatController;
    private float _delayBetweenSteps = 0.5f;


    public void InitializeCombatUI(CombatController combatController, Player player, Critter playerCritter, Critter npcCritter)
    {
        VisualSteps = new CombatVisualSteps();
        _combatController = combatController;
        _player = player;
        _playerBugInfoContainer.PopulateBugData(playerCritter, false);
        _npcBugInfoContainer.PopulateBugData(npcCritter, _combatController.OpponentData == null);

        SetInactiveAllMenus();
        _battleOptionsObject.SetActive(true);
        _battleOptions.SetSelectedIndexToMove();
        _battleOptionsObject.SetActive(false);
    }


    public void UpdatePlayerBugData(Critter critter)
    {
        _playerBugInfoContainer.PopulateBugData(critter, false);
    }


    public void UpdateNpcBugData(Critter critter)
    {
        _npcBugInfoContainer.PopulateBugData(critter, _combatController.OpponentData == null);
    }


    public void AddVisualStep(CombatVisualStep step)
    {
        VisualSteps.CurrentSteps.Add(step);
    }


    public void AddVisualSteps(List<CombatVisualStep> steps)
    {
        VisualSteps.CurrentSteps.AddRange(steps);
    }


    public IEnumerator ExecuteVisualSteps()
    {
        foreach (CombatVisualStep step in VisualSteps.CurrentSteps)
        {
            Debug.Log($"Step is {step.GetType()}");
            
            if (step.GetType() == typeof(HealthChangeStep))
            {
                HealthChangeStep castStep = (HealthChangeStep)step;

                if (castStep.IsPlayerCritter)
                {
                    yield return StartCoroutine(_playerBugInfoContainer.AnimateHealthBar(castStep.Level, castStep.StartingHealth, castStep.TargetHealth, castStep.MaxHealth));
                }
                else
                {
                    yield return StartCoroutine(_npcBugInfoContainer.AnimateHealthBar(castStep.Level, castStep.StartingHealth, castStep.TargetHealth, castStep.MaxHealth));
                }

                yield return new WaitForSeconds(_delayBetweenSteps);

                Debug.Log($"damage multiplier is {castStep.DamageMultiplier}");

                if (castStep.DamageMultiplier != 4)
                {
                    yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage(castStep.GetPopulatedMessage()));
                }
            }
            else if (step.GetType() == typeof(LevelGainStep) || step.GetType() == typeof(ChangeActiveStep))
            {
                yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage(step.GetPopulatedMessage()));

                _playerBugInfoContainer.PopulateBugData(_combatController.State.PlayerCritter, false); //this might not be the critter that levelled, but there's no harm with a repopulate
            }
            else if (step.GetType() == typeof(DoMoveStep))
            {
                yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage(step.GetPopulatedMessage()));

                DoMoveStep moveStep = (DoMoveStep)step;

                if (moveStep.IsAnimatingPlayerHit)
                {
                    _combatController.PlayerMesh.GetComponentInParent<Animator>().SetTrigger("Attack");
                    _combatController.NpcMesh.GetComponentInParent<Animator>().SetTrigger("TakeDamage");
                }
                else
                {
                    _combatController.NpcMesh.GetComponentInParent<Animator>().SetTrigger("Attack");
                    _combatController.PlayerMesh.GetComponentInParent<Animator>().SetTrigger("TakeDamage");
                }

                yield return new WaitForSeconds(0.75f);
            }
            else if (step.GetType() == typeof(TriedItsBestStep))
            {
                yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage(step.GetPopulatedMessage()));

                TriedItsBestStep moveStep = (TriedItsBestStep)step;

                if (moveStep.IsAnimatingPlayerHit)
                {
                    _combatController.PlayerMesh.GetComponentInParent<Animator>().SetTrigger("Attack");
                    _combatController.NpcMesh.GetComponentInParent<Animator>().SetTrigger("TakeDamage");
                }
                else
                {
                    _combatController.NpcMesh.GetComponentInParent<Animator>().SetTrigger("Attack");
                    _combatController.PlayerMesh.GetComponentInParent<Animator>().SetTrigger("TakeDamage");
                }

                yield return new WaitForSeconds(0.75f);
            }
            else if (step.GetType() == typeof(CritterSquishedStep))
            {
                yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage(step.GetPopulatedMessage()));
                
                _combatController.NpcMesh.GetComponentInParent<Animator>().SetTrigger("Squish");

                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage(step.GetPopulatedMessage()));
            }
        }

        VisualSteps.CurrentSteps.Clear();

        yield return null;
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


    public IEnumerator ChooseNewCritter()
    {
        yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("Choose a new bug..."));

        yield return StartCoroutine(PlayerChooseActiveBugInteraction());

        SetInactiveAllMenus();

        yield return null;
    }


    private IEnumerator PlayerChooseActiveBugInteraction()
    {
        SetInactiveAllMenus();
        _bugMenuObject.SetActive(true);
        _bugMenu.PopulateCritters(_player.GetCritters());
        _bugMenu.ShowCurrentSelection();
        
        yield return StartCoroutine(_bugMenu.PlayerInteraction(BugMenuContext.ForceChooseActive));

        _player.SetActiveCritter(_bugMenu.SelectedCritterGuid);
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

                if (!isSelected)
                {
                    _battleOptionsObject.SetActive(false);

                    yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("You have no combat items!"));

                    _battleOptionsObject.SetActive(true);
                }
            }

            yield return null;
        }
    }


    private bool TrySelectBattleOption()
    {
        BattleOption selection = _battleOptions.GetSelectionBattleOption();

        if (selection == BattleOption.Item && _player.GetBattleItems().Count == 0)
        {
            return false;
        }

        if (selection == BattleOption.Move)
        {
            if (_combatController.State.PlayerCritter.IsOutOfUses())
            {
                SetPlayerTriedItsBest();
            }
            else
            {
                StartPlayerMoveChoice();
            }
        }
        else if (selection == BattleOption.Bugs)
        {
            StartPlayerBugMenuInteraction();
        }
        else if (selection == BattleOption.Item)
        {
            StartPlayerItemChoice();
        }

        return true;
    }


    public void SetPlayerTriedItsBest()
    {
        _combatController.SetPlayerMove(MoveID.TriedItsBest);
        FinishInteraction();
    }


    public void StartPlayerMoveChoice()
    {
        SetInactiveAllMenus();
        _moveOptionsObject.SetActive(true);
        _moveOptions.PopulateMoves(_combatController.State.PlayerCritter);
        _moveOptions.ShowCurrentSelection();

        StartCoroutine(MoveOptionsInteraction());
    }


    private IEnumerator MoveOptionsInteraction()
    {
        yield return new WaitForEndOfFrame();
        
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
        Move selectedMove = _combatController.State.PlayerCritter.Moves.Find(move => move.ID == selectedMoveID);

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
        yield return StartCoroutine(_bugMenu.PlayerInteraction(BugMenuContext.ChooseActiveWithoutCommit));

        Guid selectedGuid = _bugMenu.SelectedCritterGuid;

        Debug.Log($"{selectedGuid} // {_combatController.State.PlayerCritter.GUID}");

        if (selectedGuid == Guid.Empty || selectedGuid == _combatController.State.PlayerCritter.GUID)
        {
            Debug.Log("No active chosen");
            StartPlayerBattleActionChoice();
        }
        else
        {
            Debug.Log("Choosing to change active!");
            _combatController.State.PlayerSelectedSwitchActiveGUID = selectedGuid;
            _combatController.SetPlayerMove(MoveID.SwitchActive);
            FinishInteraction();
        }
    }


    public void StartPlayerItemChoice()
    {
        SetInactiveAllMenus();
        _itemOptionsObject.SetActive(true);
        _itemOptions.PopulateItems(_player.GetBattleItems());
        _itemOptions.ShowCurrentSelection();

        StartCoroutine(ItemOptionsInteraction());
    }


    private IEnumerator ItemOptionsInteraction()
    {
        yield return new WaitForEndOfFrame();

        bool isSelected = false;
        
        while (!isSelected)
        {
            if (Input.GetKeyDown(Controls.MenuUpKey))
            {
                _itemOptions.MoveSelection(true);
            }
            else if (Input.GetKeyDown(Controls.MenuDownKey))
            {
                _itemOptions.MoveSelection(false);
            }
            else if (Input.GetKeyDown(Controls.MenuSelectKey))
            {
                ItemType selectedItem = _itemOptions.GetSelectedItemType();

                if (selectedItem == ItemType.MasonJar && _combatController.OpponentData != null)
                {
                    //do nothing
                }
                else
                {
                    isSelected = true;
                    
                    if (selectedItem == ItemType.MasonJar)
                    {
                        _combatController.SetPlayerMove(MoveID.ThrowMasonJar);
                        FinishInteraction();
                    }
                    else if (selectedItem == ItemType.Nectar)
                    {
                        yield return StartCoroutine(ChooseHealInteraction());

                        if (_bugMenu.SelectedCritterGuid != Guid.Empty)
                        {
                            _combatController.State.PlayerSelectedHealItemTarget = _bugMenu.SelectedCritterGuid;
                            _combatController.SetPlayerMove(MoveID.UseHealItem);
                            FinishInteraction();
                        }
                        else
                        {
                            StartPlayerItemChoice();
                        }
                    }
                }

            }
            else if (Input.GetKeyDown(Controls.MenuBackKey))
            {
                isSelected = true;
                StartPlayerBattleActionChoice();
            }

            yield return null;
        }
    }


    private IEnumerator ChooseHealInteraction()
    {
        SetInactiveAllMenus();
        _bugMenuObject.SetActive(true);
        _bugMenu.PopulateCritters(_player.GetCritters());
        _bugMenu.ShowCurrentSelection();
        
        yield return StartCoroutine(_bugMenu.PlayerInteraction(BugMenuContext.ChooseActiveWithoutCommit));
    }
}