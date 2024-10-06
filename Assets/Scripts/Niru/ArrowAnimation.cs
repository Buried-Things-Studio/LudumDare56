using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowAnimation : MonoBehaviour
{
    [SerializeField] private AnimationCurve _bounceCurve;
    [SerializeField] private RectTransform _arrowRT;

    [SerializeField] private Vector2 _startPos;
    [SerializeField] private Vector2 _endPos;


    void Start()
    {
        StartCoroutine(Bounce());
    }


    private IEnumerator Bounce()
    {
        yield return new WaitForSeconds(1);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;

            _arrowRT.anchoredPosition = Vector2.Lerp(_startPos, _endPos, _bounceCurve.Evaluate(t));

            yield return null;
        }

        StartCoroutine(Bounce());
    }
}
