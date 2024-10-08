using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Book : MonoBehaviour
{
    [SerializeField] private GameObject[] _pages;
    private int _currentPage;

    [Header("Audio")]
    [SerializeField] private GameObject _oneShotGO;
    [SerializeField] private AudioClip _pageTurnClip;
    [SerializeField] private AudioClip _backOutClip;


    public IEnumerator HoldBookOpen()
    {
        while (!Input.GetKeyDown(Controls.MenuBackKey))
        {
            yield return null;
        }

        OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
        osc.MyClip = _backOutClip;
        osc.Play();

        yield return null;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_currentPage < _pages.Length - 1)
            {
                OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
                osc.MyClip = _pageTurnClip;
                osc.PlayWithVariance();
            }

            _currentPage = Mathf.Clamp(_currentPage+1, 0, _pages.Length-1);

            foreach (GameObject g in _pages)
            {
                g.SetActive(false);
            }

            _pages[_currentPage].SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_currentPage > 0)
            {
                OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
                osc.MyClip = _pageTurnClip;
                osc.PlayWithVariance();
            }

            _currentPage = Mathf.Clamp(_currentPage - 1, 0, _pages.Length - 1);

            foreach (GameObject g in _pages)
            {
                g.SetActive(false);
            }

            _pages[_currentPage].SetActive(true);
        }
    }
}

