using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MoveOption : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moveNameTMP;
    [SerializeField] private TextMeshProUGUI _usesTMP;
    [SerializeField] private Image _typeIcon;
    [SerializeField] private Image _spikesImage;
    [SerializeField] private Image _backgroundFadeImage;
    private Move _move;


    public void PopulateMove(Move move) //TODO: use type icon
    {
        _move = move;
        
        _moveNameTMP.text = move.Name;
        _usesTMP.text = $"{move.CurrentUses}/{move.MaxUses}";

        _spikesImage.color = CritterAffinityData.GetAffinityColor(move.Affinity);
        _backgroundFadeImage.color = CritterAffinityData.GetAffinityColor(move.Affinity);
    }


    public Move GetMove()
    {
        return _move;
    }
}