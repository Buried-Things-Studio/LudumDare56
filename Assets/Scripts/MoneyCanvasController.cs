using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyCanvasController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _moneyTMP;

    public void SetMoney(int money)
    {
        string moneyString = $"<#ffcc44>COINS: </color>{money.ToString()}";
        _moneyTMP.text = moneyString;
    }
 
}
