using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class MoveOptions : MonoBehaviour
{
    [SerializeField] private List<MoveOption> _moveOptions;
    [SerializeField] private List<GameObject> _selections;

    [SerializeField] private TextMeshProUGUI _moveDescriptionTMP;
    [SerializeField] private TextMeshProUGUI _bluntSharpTMP;
    [SerializeField] private TextMeshProUGUI _movePowerTMP;

    private int _currentSelectedIndex = 0;
    private int _selectionCount;
    
    
    public void PopulateMoves(Critter critter)
    {
        for (int i = 0; i < _moveOptions.Count; i++)
        {
            MoveOption option = _moveOptions[i];

            if (critter.Moves.Count <= i)
            {
                option.gameObject.SetActive(false);

                continue;
            }

            option.gameObject.SetActive(true);
            option.PopulateMove(critter.Moves[i]);
        }

        _selectionCount = critter.Moves.Count;
        ShowCurrentSelection();
    }


    public void ShowCurrentSelection()
    {
        for (int i = 0; i < _selections.Count; i++)
        {
            _selections[i].SetActive(i == _currentSelectedIndex);
        }

        Move selectedMove = _moveOptions[_currentSelectedIndex].GetMove();
        _moveDescriptionTMP.text = selectedMove.Description;
        _bluntSharpTMP.text = selectedMove.IsSharp ? "SHARP" : "BLUNT";
        _bluntSharpTMP.text = selectedMove.BasePower > 0 ? _bluntSharpTMP.text : "--";
        _movePowerTMP.text = selectedMove.BasePower > 0 ? selectedMove.BasePower.ToString() : "--";
        //TODO: blunt/sharp icon
    }


    public void MoveSelection(bool isMovingUp)
    {
        _currentSelectedIndex += isMovingUp ? -1 : 1;
        _currentSelectedIndex = (_currentSelectedIndex + _selectionCount) % _selectionCount;
        ShowCurrentSelection();
    }


    public MoveID GetSelectedMove()
    {
        return _moveOptions[_currentSelectedIndex].GetMove().ID;
    }
}