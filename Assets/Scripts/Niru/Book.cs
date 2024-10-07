using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    [SerializeField] private GameObject[] _pages;
    private int _currentPage;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _currentPage = Mathf.Clamp(_currentPage+1, 0, _pages.Length-1);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _currentPage = Mathf.Clamp(_currentPage - 1, 0, _pages.Length - 1);
        }

        foreach(GameObject g in _pages)
        {
            g.SetActive(false);
        }

        _pages[_currentPage].SetActive(true);
    }
}

