using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassPatch : MonoBehaviour
{
    [SerializeField] private GameObject _grassBladeGO;
    [SerializeField] private Transform _splats;
    [SerializeField] private AnimationCurve _scaleCurve;


    void Start()
    {
        MakeGrass();
        StartCoroutine(ScaleIn());
    }


    void MakeGrass()
    {
        _splats.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0, 4) * 90);

        int grassBlades = Random.Range(12, 16);

        float xAdd = 0;
        float zAdd = 0;

        for (int i = 0; i < grassBlades; i++)
        {
            xAdd += Random.Range(0.47f / (float)grassBlades * 0.5f, 0.47f / (float)grassBlades * 0.95f);
            zAdd += Random.Range(0.47f / (float)grassBlades * 0.5f, 0.47f / (float)grassBlades * 0.95f);

            GrassBlade blade = Instantiate(_grassBladeGO, transform).GetComponent<GrassBlade>();
            blade.myLineRenderer.SetPosition(1, Vector3.up * Random.Range(1.0f, 2.4f));

            float grassWidth = Random.Range(0.08f, 0.14f);
            blade.myLineRenderer.startWidth = grassWidth;
            blade.myLineRenderer.endWidth = grassWidth;

            int flip = (i % 2 == 0) ? -1 : 1;

            blade.transform.localPosition = new Vector3(Random.Range(-0.47f, 0.47f), 0, zAdd * flip);
        }
    }


    private IEnumerator ScaleIn()
    {
        float randomSpeed = Random.Range(0.5f, 0.9f);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / randomSpeed;

            transform.localScale = Vector3.one * _scaleCurve.Evaluate(t);

            yield return null;
        }

        transform.localScale = Vector3.one;
    }
}
