using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TextBoxController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _mainTMP;
    [SerializeField] private GameObject _moveOnTab;
    [SerializeField] private GameObject _choiceTab;
    [SerializeField] private GameObject _imageTab;
    [SerializeField] private GameObject _moveTab;
    [SerializeField] private MoveDetails _moveDetails;

    [SerializeField] private Image _imageTabImage;
    [SerializeField] private List<GameObject> _choiceSelections;

    public bool IsSelectingYes = false;
    private int _currentSelectedIndex = 0;

    [Header("Audio")]
    [SerializeField] private GameObject _oneShotGO;
    [SerializeField] private AudioClip _popInClip;
    [SerializeField] private AudioClip _popOutClip;
    [SerializeField] private AudioClip _navClip;


    public void Start()
    {
        GlobalUI.TextBox = this;
    }


    private void ChangeShowState(bool isShowing)
    {
        _canvasGroup.alpha = isShowing ? 1 : 0;

        OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
        osc.MyClip = isShowing ? _popInClip : _popOutClip;
        osc.Play();
    }


    private void DisableAllTabs()
    {
        _moveOnTab.SetActive(false);
        _choiceTab.SetActive(false);
        _imageTab.SetActive(false);
        _moveTab.SetActive(false);
    }


    public IEnumerator ShowSimpleMessage(string message)
    {
        ChangeShowState(true);
        _mainTMP.text = message;
        DisableAllTabs();
        _moveOnTab.SetActive(true);

        yield return null;

        while (
            !Input.GetKeyDown(Controls.MenuSelectKey)
            && !Input.GetKeyDown(Controls.MenuBackKey)
            && !Input.GetKeyDown(Controls.MenuRightKey))
        {
            yield return null;
        }

        ChangeShowState(false);
    }


    public IEnumerator ShowSimpleMoveMessage(Move move, string message)
    {
        ChangeShowState(true);
        _mainTMP.text = message;
        DisableAllTabs();
        _moveOnTab.SetActive(true);
        _moveTab.SetActive(true);
        _moveDetails.PopulateMove(move);

        yield return null;

        while (
            !Input.GetKeyDown(Controls.MenuSelectKey)
            && !Input.GetKeyDown(Controls.MenuBackKey)
            && !Input.GetKeyDown(Controls.MenuRightKey))
        {
            yield return null;
        }

        ChangeShowState(false);
    }


    public IEnumerator ShowMoveYesNoChoice(Move move, string message)
    {
        ChangeShowState(true);
        _mainTMP.text = message;
        DisableAllTabs();
        _choiceTab.SetActive(true);
        _currentSelectedIndex = 0;
        ShowCurrentSelection();
        _moveTab.SetActive(true);
        _moveDetails.PopulateMove(move);

        yield return null;

        bool isSelected = false;

        while (!isSelected)
        {
            if (Input.GetKeyDown(Controls.MenuLeftKey) || Input.GetKeyDown(Controls.MenuUpKey))
            {
                MoveSelection(true);
            }
            else if (Input.GetKeyDown(Controls.MenuRightKey) || Input.GetKeyDown(Controls.MenuDownKey))
            {
                MoveSelection(false);
            }
            else if (Input.GetKeyDown(Controls.MenuSelectKey))
            {
                IsSelectingYes = _currentSelectedIndex == 0;
                isSelected = true;
            }
            
            yield return null;
        }

        ChangeShowState(false);

        yield return null;
    }


    public void ShowCurrentSelection()
    {
        for (int i = 0; i < _choiceSelections.Count; i++)
        {
            _choiceSelections[i].SetActive(i == _currentSelectedIndex);
        }
    }


    private void MoveSelection(bool isMovingUp)
    {
        OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
        osc.MyClip = _navClip;
        osc.Play();

        _currentSelectedIndex += isMovingUp ? -1 : 1;
        _currentSelectedIndex = (_currentSelectedIndex + _choiceSelections.Count) % _choiceSelections.Count;
        ShowCurrentSelection();
    }


    public IEnumerator ShowYesNoChoice(string message)
    {
        ChangeShowState(true);
        _mainTMP.text = message;
        DisableAllTabs();
        _choiceTab.SetActive(true);
        _currentSelectedIndex = 0;
        ShowCurrentSelection();

        yield return null;

        bool isSelected = false;

        while (!isSelected)
        {
            if (Input.GetKeyDown(Controls.MenuLeftKey) || Input.GetKeyDown(Controls.MenuUpKey))
            {
                MoveSelection(true);
            }
            else if (Input.GetKeyDown(Controls.MenuRightKey) || Input.GetKeyDown(Controls.MenuDownKey))
            {
                MoveSelection(false);
            }
            else if (Input.GetKeyDown(Controls.MenuSelectKey))
            {
                IsSelectingYes = _currentSelectedIndex == 0;
                isSelected = true;
            }
            
            yield return null;
        }

        ChangeShowState(false);

        yield return null;
    }


    public IEnumerator ShowStarterChoice(Critter starter)
    {
        ChangeShowState(true);
        _mainTMP.text = $"Would you like to take the {starter.Name}?";
        DisableAllTabs();
        _choiceTab.SetActive(true);
        _currentSelectedIndex = 0;
        ShowCurrentSelection();

        _imageTab.SetActive(true);
        _imageTabImage.sprite = PictureHelpers.GetProfilePicture(starter);

        yield return null;

        bool isSelected = false;

        while (!isSelected)
        {
            if (Input.GetKeyDown(Controls.MenuLeftKey) || Input.GetKeyDown(Controls.MenuUpKey))
            {
                MoveSelection(true);
            }
            else if (Input.GetKeyDown(Controls.MenuRightKey) || Input.GetKeyDown(Controls.MenuDownKey))
            {
                MoveSelection(false);
            }
            else if (Input.GetKeyDown(Controls.MenuSelectKey))
            {
                IsSelectingYes = _currentSelectedIndex == 0;
                isSelected = true;
            }
            
            yield return null;
        }

        ChangeShowState(false);

        yield return null;
    }
}
