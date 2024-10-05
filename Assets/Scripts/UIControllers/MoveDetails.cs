using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MoveDetails : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moveNameTMP;
    [SerializeField] private TextMeshProUGUI _usesTMP;
    [SerializeField] private TextMeshProUGUI _powerTMP;
    [SerializeField] private TextMeshProUGUI _bluntSharpTMP;
    [SerializeField] private TextMeshProUGUI _descriptionTMP;

    [SerializeField] private Image _typeIcon;
    [SerializeField] private Image _spikesImage;
    [SerializeField] private Image _backgroundFadeImage;
    private Move _move;


    public void PopulateMove(Move move) //TODO: use type icon
    {
        _move = move;
        
        _moveNameTMP.text = move.Name;
        _usesTMP.text = $"{move.CurrentUses}/{move.MaxUses}";
        _bluntSharpTMP.text = move.IsSharp ? "SHARP" : "BLUNT";
        _descriptionTMP.text = move.Description;
        _powerTMP.text = move.BasePower.ToString();

        _spikesImage.color = CritterHelpers.GetAffinityColor(move.Affinity);
        _backgroundFadeImage.color = CritterHelpers.GetAffinityColor(move.Affinity);
    }


    public Move GetMove()
    {
        return _move;
    }
}
