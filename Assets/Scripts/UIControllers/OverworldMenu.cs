using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


public enum MenuOption
{
    Bugs,
    Item,
    Book,
}


public class OverworldMenu : MonoBehaviour
{
    [SerializeField] private BattleOptions _menuOptions;
    [SerializeField] private GameObject _menuOptionsObject;

    [SerializeField] private ItemOptions _itemOptions;
    [SerializeField] private GameObject _itemOptionsObject;

    [SerializeField] private BugMenu _bugMenu;
    [SerializeField] private GameObject _bugMenuObject;
    
    [SerializeField] private GameObject _bookObject;
    
    private bool _isMenuOpen;
    private bool _isMenuClosing;
    private Player _player;
    private PlayerController _playerController;


    public void GetComponentsInScene(Player player, PlayerController playerController)
    {
        // _player = GameObject.FindObjectOfType<FloorController>().PlayerData;
        // _playerController = GameObject.FindObjectOfType<PlayerController>();

        _player = player;
        _playerController = playerController;
    }
    
    
    private void Update()
    {
        if (!_isMenuOpen && _playerController.GetAbleToMove() && Input.GetKeyDown(Controls.OverworldMenuKey))
        {
            _isMenuOpen = true;
            _playerController.SetOverworldMenuUIBlock(true);
            
            StartPlayerMenuActionChoice();
        }

        if (_isMenuOpen && _isMenuClosing)
        {
            _playerController.SetOverworldMenuUIBlock(false);
            _isMenuOpen = false;
            _isMenuClosing = false;
        }
    }


    private void SetInactiveAllMenus()
    {
        _menuOptionsObject.SetActive(false);
        _itemOptionsObject.SetActive(false);
        _bugMenuObject.SetActive(false);
        //_bookObject.SetActive(false);
    }


    public void StartPlayerMenuActionChoice()
    {
        SetInactiveAllMenus();
        _menuOptionsObject.SetActive(true);
        _menuOptions.ShowCurrentSelection();

        StartCoroutine(MenuOptionsInteraction());
    }


    private IEnumerator MenuOptionsInteraction()
    {
        SetInactiveAllMenus();
        _menuOptionsObject.SetActive(true);
        _menuOptions.ShowCurrentSelection();
        
        yield return new WaitForEndOfFrame();
        
        bool isSelected = false;
        
        while (!isSelected)
        {
            if (Input.GetKeyDown(Controls.MenuUpKey))
            {
                _menuOptions.MoveSelection(true);
            }
            else if (Input.GetKeyDown(Controls.MenuDownKey))
            {
                _menuOptions.MoveSelection(false);
            }
            else if (Input.GetKeyDown(Controls.MenuSelectKey))
            {
                isSelected = TrySelectMenuOption();

                if (!isSelected)
                {
                    _menuOptionsObject.SetActive(false);

                    yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("You have no items!"));

                    _menuOptionsObject.SetActive(true);
                }
            }
            else if (Input.GetKeyDown(Controls.MenuBackKey))
            {
                isSelected = true;
                _isMenuClosing = true;
                SetInactiveAllMenus();
            }

            yield return null;
        }
    }


    private bool TrySelectMenuOption()
    {
        BattleOption selection = _menuOptions.GetSelectionBattleOption();
        MenuOption translatedSelection = (MenuOption)selection;

        if (translatedSelection == MenuOption.Item && _player.GetItems().Count == 0)
        {
            return false;
        }

        if (translatedSelection == MenuOption.Item)
        {
            StartPlayerItemChoice();
        }
        else if (translatedSelection == MenuOption.Bugs)
        {
            StartPlayerBugMenuInteraction();
        }
        else if (translatedSelection == MenuOption.Book)
        {
            //StartPlayerItemChoice();
        }

        return true;
    }


    public void StartPlayerItemChoice()
    {
        SetInactiveAllMenus();
        _itemOptionsObject.SetActive(true);
        _itemOptions.PopulateItems(_player.GetItems());
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

                if (selectedItem == ItemType.MasonJar)
                {
                    //do nothing
                }
                else
                {
                    isSelected = true;
                    
                    if (selectedItem == ItemType.Nectar)
                    {
                        yield return StartCoroutine(ChooseHealInteraction());

                        if (_bugMenu.SelectedCritterGuid != Guid.Empty)
                        {
                            Critter critter = _player.GetCritters().Find(critter => critter.GUID == _bugMenu.SelectedCritterGuid);
                            critter.IncreaseHealth(30);
                            _player.RemoveItemFromInventory(ItemType.Nectar);
                        }

                        StartPlayerMenuActionChoice();
                    }

                    if (selectedItem == ItemType.MoveManual)
                    {
                        yield return StartCoroutine(ChooseMoveTeachInteraction());

                        if (_bugMenu.SelectedCritterGuid != Guid.Empty)
                        {
                            _player.TeachMoveToCritter((MoveManual)_itemOptions.GetSelectedItem(), _bugMenu.SelectedCritterGuid);
                            StartPlayerMenuActionChoice();
                        }
                    }
                }

            }
            else if (Input.GetKeyDown(Controls.MenuBackKey))
            {
                isSelected = true;
                StartPlayerMenuActionChoice();
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


    private IEnumerator ChooseMoveTeachInteraction()
    {
        SetInactiveAllMenus();
        _bugMenuObject.SetActive(true);
        _bugMenu.PopulateCritters(_player.GetCritters());
        _bugMenu.ShowCurrentSelection();
        
        yield return StartCoroutine(_bugMenu.PlayerInteraction(BugMenuContext.TeachMoveWithoutCommit));
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

        if (selectedGuid == Guid.Empty)
        {
            Debug.Log("No active chosen");
        }
        else
        {
            Debug.Log("Choosing to change active!");
            _player.SetActiveCritter(selectedGuid);
        }

        StartPlayerMenuActionChoice();

        yield return null;
    }
}
