using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleOptions : MonoBehaviour
{
    [SerializeField] private List<GameObject> _selections;
    private int _currentSelectedIndex = 0;

    [Header("Audio")]
    [SerializeField] private GameObject _oneShotGO;
    [SerializeField] private AudioClip _navigateClip;
    [SerializeField] private AudioClip _selectClip;
    [SerializeField] private AudioClip _backOutClip;

    public void SetSelectedIndexToMove()
    {
        
    }


    public void ShowCurrentSelection()
    {
        for (int i = 0; i < _selections.Count; i++)
        {
            _selections[i].SetActive(i == _currentSelectedIndex);
        }
    }


    public void MoveSelection(bool isMovingUp)
    {
        _currentSelectedIndex += isMovingUp ? -1 : 1;
        _currentSelectedIndex = (_currentSelectedIndex + _selections.Count) % _selections.Count;
        ShowCurrentSelection();

        OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
        osc.MyClip = _navigateClip;
        osc.Play();
    }


    public BattleOption GetSelectionBattleOption()
    {
        return (BattleOption)_currentSelectedIndex;
    }
}


public enum BattleOption
{
    Move,
    Bugs,
    Item,
}