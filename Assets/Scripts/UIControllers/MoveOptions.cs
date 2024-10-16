using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MoveOptions : MonoBehaviour
{
    [SerializeField] private List<MoveOption> _moveOptions;
    [SerializeField] private List<GameObject> _selections = new List<GameObject>();

    [SerializeField] private Image _descriptionTypeIcon;

    [SerializeField] private TextMeshProUGUI _moveDescriptionTMP;
    [SerializeField] private TextMeshProUGUI _bluntSharpTMP;
    [SerializeField] private TextMeshProUGUI _movePowerTMP;
    [SerializeField] private TextMeshProUGUI _accuracyTMP;


    private int _currentSelectedIndex = 0;

    [Header("Audio")]
    [SerializeField] private GameObject _oneShotGO;
    [SerializeField] private AudioClip _navigateClip;


    public void PopulateMoves(Critter critter)
    {
        _selections.Clear();
        
        for (int i = 0; i < _moveOptions.Count; i++)
        {
            MoveOption option = _moveOptions[i];

            if (critter.Moves.Count <= i)
            {
                option.gameObject.SetActive(false);

                continue;
            }

            option.gameObject.SetActive(true);
            _selections.Add(option.GetSelection());
            option.PopulateMove(critter.Moves[i]);
        }

        ShowCurrentSelection();
    }


    public void ShowCurrentSelection()
    {
        if (_currentSelectedIndex >= _selections.Count)
        {
            _currentSelectedIndex = 0;
        }
        
        for (int i = 0; i < _selections.Count; i++)
        {
            _selections[i].SetActive(i == _currentSelectedIndex);
        }

        Move selectedMove = _moveOptions[_currentSelectedIndex].GetMove();
        _moveDescriptionTMP.text = selectedMove.Description;
        _bluntSharpTMP.text = selectedMove.IsSharp ? "SHARP" : "BLUNT";
        _bluntSharpTMP.text = selectedMove.BasePower > 0 ? _bluntSharpTMP.text : "--";
        _movePowerTMP.text = selectedMove.BasePower > 0 ? selectedMove.BasePower.ToString() : "--";

        _accuracyTMP.text = selectedMove.Accuracy.ToString() + "%";

        _descriptionTypeIcon.sprite = PictureHelpers.GetMoveAffinityPicture(selectedMove);
        _descriptionTypeIcon.color = CritterAffinityData.GetAffinityColor(selectedMove.Affinity);

        //TODO: blunt/sharp icon
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


    public MoveID GetSelectedMove()
    {
        return _moveOptions[_currentSelectedIndex].GetMove().ID;
    }
}