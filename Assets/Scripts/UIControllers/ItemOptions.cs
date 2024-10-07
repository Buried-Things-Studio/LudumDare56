using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemOptions : MonoBehaviour
{
    [SerializeField] private GameObject _itemOptionPrefab;
    [SerializeField] private GameObject _itemOptionParent;
    [SerializeField] private TextMeshProUGUI _itemDescriptionTMP;

    private List<GameObject> _selections = new List<GameObject>();
    private List<ItemOption> _itemOptions = new List<ItemOption>();
    private int _currentSelectedIndex;


    public void PopulateItems(List<Item> items)
    {
        foreach (ItemOption item in _itemOptions)
        {
            GameObject.Destroy(item.gameObject);
        }

        _selections.Clear();
        _itemOptions.Clear();
        
        foreach (Item item in items)
        {
            GameObject newItemOption = GameObject.Instantiate(_itemOptionPrefab);
            newItemOption.transform.SetParent(_itemOptionParent.transform);

            ItemOption option = newItemOption.GetComponent<ItemOption>();
            _itemOptions.Add(option);
            _selections.Add(option.GetSelection());
            option.PopulateItemDetails(item);

            Debug.Log($"Added item! It's a {item.Name}");
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

        Item selectedItem = _itemOptions[_currentSelectedIndex].GetItem();
        _itemDescriptionTMP.text = selectedItem.Description;
    }


    public void MoveSelection(bool isMovingUp)
    {
        _currentSelectedIndex += isMovingUp ? -1 : 1;
        _currentSelectedIndex = (_currentSelectedIndex + _selections.Count) % _selections.Count;
        ShowCurrentSelection();
    }


    public ItemType GetSelectedItemType()
    {
        return _itemOptions[_currentSelectedIndex].GetItem().ID;
    }
}
