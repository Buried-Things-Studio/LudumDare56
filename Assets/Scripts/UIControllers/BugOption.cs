using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BugOption : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _critterNameTMP;
    [SerializeField] private TextMeshProUGUI _critterLevelTMP;
    [SerializeField] private TextMeshProUGUI _critterHealthNumbersTMP;
    [SerializeField] private Image _critterHealthFillImageTMP;
    [SerializeField] private GameObject _selection;
    private Critter _critter;


    public void PopulateCritterDetails(Critter critter)
    {
        _critter = critter;

        _critterNameTMP.text = critter.Name;
        _critterLevelTMP.text = $"<size=18>Lv </size><mspace=24>{critter.Level}";
        _critterHealthNumbersTMP.text = $"<mspace=14>{critter.CurrentHealth}/{critter.MaxHealth}";
        _critterHealthFillImageTMP.fillAmount = (float)critter.CurrentHealth / (float)critter.MaxHealth;

        //TODO: picture
    }


    public GameObject GetSelection()
    {
        return _selection;
    }


    public Critter GetCritter()
    {
        return _critter;
    }
}