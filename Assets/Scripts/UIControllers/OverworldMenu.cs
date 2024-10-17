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
    Quit,
}


public class OverworldMenu : MonoBehaviour
{
    [SerializeField] private BattleOptions _menuOptions;
    [SerializeField] private GameObject _menuOptionsObject;

    [SerializeField] private ItemOptions _itemOptions;
    [SerializeField] private GameObject _itemOptionsObject;

    [SerializeField] private BugMenu _bugMenu;
    [SerializeField] private GameObject _bugMenuObject;

    [SerializeField] private Book _book;
    [SerializeField] private GameObject _bookObject;

    [SerializeField] private Quit _quit;
    [SerializeField] private GameObject _quitObject;

    private bool _isMenuOpen;
    private bool _isMenuClosing;
    private Player _player;
    private PlayerController _playerController;

    [Header("Audio")]
    [SerializeField] private GameObject _oneShotGO;
    [SerializeField] private AudioClip _navigateClip;
    [SerializeField] private AudioClip _selectClip;
    [SerializeField] private AudioClip _backOutClip;
    [SerializeField] private AudioClip _failClip;
    [SerializeField] private AudioClip _menuOpenClip;


    public void GetComponentsInScene(Player player, PlayerController playerController)
    {
        // _player = GameObject.FindObjectOfType<FloorController>().PlayerData;
        // _playerController = GameObject.FindObjectOfType<PlayerController>();

        _player = player;
        _playerController = playerController;
    }
    
    
    private void Update()
    {
        if (_playerController == null)
        {
            return;
        }
        
        if (!_isMenuOpen && _playerController.GetAbleToMove() && Input.GetKeyDown(Controls.OverworldMenuKey))
        {
            _isMenuOpen = true;
            _playerController.SetOverworldMenuUIBlock(true);
            
            StartPlayerMenuActionChoice();

            OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
            osc.MyClip = _menuOpenClip;
            osc.Play();
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
        _bookObject.SetActive(false);
        _quitObject.SetActive(false);
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
        
        yield return null;
        
        bool isSelected = false;
        
        while (!isSelected)
        {
            if (Input.GetKeyDown(Controls.MenuUpKey))
            {
                _menuOptions.MoveSelection(true);

                OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
                osc.MyClip = _navigateClip;
                osc.Play();
            }
            else if (Input.GetKeyDown(Controls.MenuDownKey))
            {
                _menuOptions.MoveSelection(false);

                OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
                osc.MyClip = _navigateClip;
                osc.Play();
            }
            else if (Input.GetKeyDown(Controls.MenuSelectKey))
            {
                isSelected = TrySelectMenuOption();

                if (!isSelected)
                {
                    _menuOptionsObject.SetActive(false);

                    yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage("You haven't got any!"));

                    _menuOptionsObject.SetActive(true);
                }
            }
            else if (Input.GetKeyDown(Controls.MenuBackKey))
            {
                isSelected = true;
                _isMenuClosing = true;
                SetInactiveAllMenus();

                OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
                osc.MyClip = _backOutClip;
                osc.Play();
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

        if (translatedSelection == MenuOption.Bugs && _player.GetCritters().Count == 0)
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
            StartBook();
        }
        else if (translatedSelection == MenuOption.Quit)
        {
            StartQuit();
        }

        OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
        osc.MyClip = _selectClip;
        osc.Play();

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
        yield return null;

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

                    OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
                    osc.MyClip = _failClip;
                    osc.Play();
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

                        OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
                        osc.MyClip = _selectClip;
                        osc.Play();

                        StartPlayerMenuActionChoice();
                    }

                    if (selectedItem == ItemType.MoveManual)
                    {
                        yield return StartCoroutine(ChooseMoveTeachInteraction());

                        if (_bugMenu.SelectedCritterGuid != Guid.Empty)
                        {
                            _player.TeachMoveToCritter((MoveManual)_itemOptions.GetSelectedItem(), _bugMenu.SelectedCritterGuid);
                        }

                        OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
                        osc.MyClip = _selectClip;
                        osc.Play();

                        StartPlayerMenuActionChoice();
                    }

                    if (selectedItem == ItemType.AbilityManual)
                    {
                        yield return StartCoroutine(ChooseAbilityTeachInteraction());

                        if (_bugMenu.SelectedCritterGuid != Guid.Empty)
                        {
                            _player.TeachAbilityToCritter((AbilityManual)_itemOptions.GetSelectedItem(), _bugMenu.SelectedCritterGuid);
                        }
                        OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
                        osc.MyClip = _selectClip;
                        osc.Play();

                        StartPlayerMenuActionChoice();
                    }
                }

            }
            else if (Input.GetKeyDown(Controls.MenuBackKey))
            {
                isSelected = true;
                StartPlayerMenuActionChoice();

                OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
                osc.MyClip = _backOutClip;
                osc.Play();
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

    private IEnumerator ChooseAbilityTeachInteraction()
    {
        SetInactiveAllMenus();
        _bugMenuObject.SetActive(true);
        _bugMenu.PopulateCritters(_player.GetCritters());
        _bugMenu.ShowCurrentSelection();
        
        yield return StartCoroutine(_bugMenu.PlayerInteraction(BugMenuContext.TeachAbilityWithoutCommit));
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


    private void StartBook()
    {
        SetInactiveAllMenus();
        _bookObject.SetActive(true);

        StartCoroutine(BookInteraction());
    }


    private void StartQuit()
    {
        SetInactiveAllMenus();
        _quitObject.SetActive(true);

        StartCoroutine(QuitInteraction());
    }


    private IEnumerator BookInteraction()
    {
        yield return StartCoroutine(_book.HoldBookOpen());

        StartPlayerMenuActionChoice();

        yield return null;
    }


    private IEnumerator QuitInteraction()
    {
        yield return StartCoroutine(_quit.HoldMenuOpen());

        StartPlayerMenuActionChoice();

        yield return null;
    }
}
