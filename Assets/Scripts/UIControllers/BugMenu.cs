using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class BugMenu : MonoBehaviour
{
    [SerializeField] private GameObject _bugOptionPrefab;
    [SerializeField] private GameObject _bugOptionParent;

    [SerializeField] private TextMeshProUGUI _nameTMP;
    [SerializeField] private TextMeshProUGUI _levelTMP;
    [SerializeField] private TextMeshProUGUI _healthNumbersTMP;
    [SerializeField] private TextMeshProUGUI _expTMP;
    [SerializeField] private Image _healthBarFillImage;
    [SerializeField] private Image _expBarFillImage;

    [SerializeField] private TextMeshProUGUI _bluntAttackStatTMP;
    [SerializeField] private TextMeshProUGUI _sharpAttackStatTMP;
    [SerializeField] private TextMeshProUGUI _bluntDefenseStatTMP;
    [SerializeField] private TextMeshProUGUI _sharpDefenseStatTMP;
    [SerializeField] private TextMeshProUGUI _speedStatTMP;

    [SerializeField] private TextMeshProUGUI _abilityNameTMP;
    [SerializeField] private TextMeshProUGUI _abilityDescriptionTMP;

    [SerializeField] private List<MoveDetails> _moveDetails;

    public Guid SelectedCritterGuid = Guid.Empty;
    private List<GameObject> _selections = new List<GameObject>();
    private List<BugOption> _bugOptions = new List<BugOption>();
    private int _currentSelectedIndex;


    public void PopulateCritters(List<Critter> critters)
    {
        foreach (BugOption bug in _bugOptions)
        {
            GameObject.Destroy(bug.gameObject);
        }

        _selections.Clear();
        _bugOptions.Clear();
        
        foreach (Critter critter in critters)
        {
            GameObject newBugOption = GameObject.Instantiate(_bugOptionPrefab);
            newBugOption.transform.SetParent(_bugOptionParent.transform);

            BugOption option = newBugOption.GetComponent<BugOption>();
            _bugOptions.Add(option);
            _selections.Add(option.GetSelection());
            option.PopulateCritterDetails(critter);
        }

        ShowCurrentSelection();
    }


    public void ShowCurrentSelection()
    {
        for (int i = 0; i < _selections.Count; i++)
        {
            _selections[i].SetActive(i == _currentSelectedIndex);
        }

        Critter selectedCritter = _bugOptions[_currentSelectedIndex].GetCritter();

        _nameTMP.text = selectedCritter.Name;
        _levelTMP.text = $"<size=20>lv </size>{selectedCritter.Level}";
        _healthNumbersTMP.text = $"<mspace=14>{selectedCritter.CurrentHealth}/{selectedCritter.MaxHealth}";
        _expTMP.text = selectedCritter.Level == 10 ? $"<mspace=14>MAX" : $"<mspace=14>{selectedCritter.Exp}/{CritterHelpers.ExpToNextLevel[selectedCritter.Level]}"; 
        
        _healthBarFillImage.fillAmount = (float)selectedCritter.CurrentHealth / (float)selectedCritter.MaxHealth;
        _expBarFillImage.fillAmount = selectedCritter.Level == 10 ? 1f : (float)selectedCritter.Exp / (float)CritterHelpers.ExpToNextLevel[selectedCritter.Level];
    
        _bluntAttackStatTMP.text = $"BLUNT ATT<br><size=26>{selectedCritter.MaxBluntAttack}";
        _bluntDefenseStatTMP.text = $"BLUNT DEF<br><size=26>{selectedCritter.MaxBluntDefense}";
        _sharpAttackStatTMP.text = $"SHARP ATT<br><size=26>{selectedCritter.MaxSharpAttack}";
        _sharpDefenseStatTMP.text = $"SHARP DEF<br><size=26>{selectedCritter.MaxSharpDefense}";
        _speedStatTMP.text = $"SPEED<br><size=26>{selectedCritter.MaxSpeed}";

        for (int i = 0; i < _moveDetails.Count; i++)
        {
            if (selectedCritter.Moves.Count <= i)
            {
                _moveDetails[i].gameObject.SetActive(false);
                
                continue;
            }

            _moveDetails[i].gameObject.SetActive(true);
            _moveDetails[i].PopulateMove(selectedCritter.Moves[i]);
        }

        //_abilityNameTMP = $"<#000000>ABILITY:  <#ffffff>ABILITY NAME";
        //_abilityDescriptionTMP = $"";
    }


    public void MoveSelection(bool isMovingUp)
    {
        _currentSelectedIndex += isMovingUp ? -1 : 1;
        _currentSelectedIndex = (_currentSelectedIndex + _selections.Count) % _selections.Count;
        ShowCurrentSelection();
    }


    public IEnumerator PlayerInteraction(BugMenuContext context)
    {
        if (context == BugMenuContext.ChooseActiveWithoutCommit)
        {
            yield return StartCoroutine(ChooseActiveInteraction(false));
        }
        else if (context == BugMenuContext.ForceChooseActive)
        {
            yield return StartCoroutine(ChooseActiveInteraction(true));
        }
        else if (context == BugMenuContext.SwapToActiveSlot)
        {
            yield return StartCoroutine(ChooseActiveInteraction(false));
        }
    }


    public IEnumerator ChooseActiveInteraction(bool isForcingChoice)
    {
        yield return new WaitForEndOfFrame();

        bool isClosing = false;
        SelectedCritterGuid = Guid.Empty;
        
        while (!isClosing)
        {
            if (Input.GetKeyDown(Controls.MenuUpKey))
            {
                MoveSelection(true);
            }
            else if (Input.GetKeyDown(Controls.MenuDownKey))
            {
                MoveSelection(false);
            }
            else if (Input.GetKeyDown(Controls.MenuSelectKey))
            {
                Critter selectedCritter = _bugOptions[_currentSelectedIndex].GetCritter();

                if (selectedCritter.CurrentHealth > 0)
                {
                    SelectedCritterGuid = selectedCritter.GUID;
                    isClosing = true;
                }
            }
            else if (!isForcingChoice && Input.GetKeyDown(Controls.MenuBackKey))
            {
                SelectedCritterGuid = Guid.Empty;
                isClosing = true;
            }

            yield return null;
        }
    }
}


public enum BugMenuContext
{
    None,
    SwapToActiveSlot,
    ChooseActiveWithoutCommit,
    ForceChooseActive
}