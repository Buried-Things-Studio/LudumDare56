using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BugPicJiggle : MonoBehaviour
{
    private RectTransform _rt;
    private Vector3 _initPos;

    void Start()
    {
        _rt = GetComponent<RectTransform>();
        _initPos = _rt.anchoredPosition;
        StartCoroutine(Jiggle());
    }

    
    private IEnumerator Jiggle()
    {
        Vector3 startPos = _rt.anchoredPosition;
        Vector3 endPos = _initPos + new Vector3(Random.Range(-4, 4), Random.Range(-4, 4), Random.Range(-4, 4));

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / 0.1f;

            _rt.anchoredPosition = Vector3.Lerp(startPos, endPos, t); 

            yield return null;
        }

        StartCoroutine(Jiggle());
    }
}
