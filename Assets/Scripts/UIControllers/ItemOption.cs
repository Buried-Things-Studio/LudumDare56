using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemOption : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _itemNameTMP;
    [SerializeField] TextMeshProUGUI _itemCountTMP;
    [SerializeField] GameObject _selection;

    private Item _item;


    public void PopulateItemDetails(Item item)
    {
        _item = item;

        if (item.ID == ItemType.MoveManual)
        {
            MoveManual moveManual = (MoveManual)item;
            _itemNameTMP.text = $"Scroll: {moveManual.TeachableMove.Name}";
        }
        else
        {
            _itemNameTMP.text = item.Name;
        }
        
        _itemCountTMP.text = item.OwnedCount.ToString();
        //TODO: item icon
    }


    public GameObject GetSelection()
    {
        return _selection;
    }


    public Item GetItem()
    {
        return _item;
    }
}
