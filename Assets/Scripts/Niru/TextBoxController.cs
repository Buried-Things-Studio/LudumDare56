using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TextBoxController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _mainTMP;
    [SerializeField] private GameObject _moveOnTab;


    public void Start()
    {
        GlobalUI.TextBox = this;
    }


    private void ChangeShowState(bool isShowing)
    {
        _canvasGroup.alpha = isShowing ? 1 : 0;
    }


    public IEnumerator ShowSimpleMessage(string message)
    {
        ChangeShowState(true);
        _mainTMP.text = message;
        _moveOnTab.SetActive(true);

        yield return new WaitForEndOfFrame();

        while (
            !Input.GetKeyDown(Controls.MenuSelectKey)
            && !Input.GetKeyDown(Controls.MenuBackKey)
            && !Input.GetKeyDown(Controls.MenuRightKey))
        {
            yield return null;
        }

        ChangeShowState(false);
        _moveOnTab.SetActive(false);
    }
}
