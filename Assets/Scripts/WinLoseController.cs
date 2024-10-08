using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WinLoseController : MonoBehaviour
{
    private bool _done;
    
    
    void Update()
    {
        if (!_done && Input.GetKeyDown(KeyCode.Space))
        {
            _done = true;
            StartCoroutine(Go());
        }
    }


    private IEnumerator Go()
    {
        FloorController floorCon = GameObject.FindObjectOfType<FloorController>();

        if (floorCon != null)
        {
            GameObject.Destroy(floorCon.gameObject);
        }
        
        yield return null;

        SceneManager.LoadScene("MainGame");
    }
}
