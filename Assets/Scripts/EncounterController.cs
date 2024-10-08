using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


public class EncounterController : MonoBehaviour
{
    public Player PlayerData;
    private float _encounterChance = 0.125f;
    private int _critterTypesAvailablePerFloor = 5;
    private List<Type> _critterTypesAvailableOnFloor = new List<Type>();
    private Vector2Int _wildEncounterLevelRange;
    public bool IsStarterChosen;

    [Header("Animation")]
    public Animator PixelAnimator;
    public Animator ExclaimAnimator;
    public GameObject _wallsAndDoorsGO;

    [Header("Audio")]
    [SerializeField] private GameObject _oneShotGO;
    [SerializeField] private AudioClip _alertClip;


    public void SetAvailableCrittersOnFloor(Vector2Int levelRange)
    {
        _wildEncounterLevelRange = levelRange;
        
        _critterTypesAvailableOnFloor.Clear();
        List<Type> availableCritterTypes = new List<Type>(MasterCollection.GetAllCritterTypes());

        Debug.Log("CRITTERS AVAILABLE ON FLOOR:");

        for (int i = 0; i < _critterTypesAvailablePerFloor; i++)
        {
            if (availableCritterTypes.Count == 0)
            {
                break;
            }
            
            int randomIndex = UnityEngine.Random.Range(0, availableCritterTypes.Count);
            _critterTypesAvailableOnFloor.Add(availableCritterTypes[randomIndex]);

            availableCritterTypes.RemoveAt(randomIndex);
        }
    }
    
    
    public bool CheckRandomEncounter(bool forceCombat = false)
    {
        bool isEnteringEncounter = UnityEngine.Random.Range(0f, 1f) < _encounterChance;

        if (isEnteringEncounter || forceCombat)
        {
            Type randomCritterType = _critterTypesAvailableOnFloor[UnityEngine.Random.Range(0, _critterTypesAvailableOnFloor.Count)];
            Critter randomCritter = Activator.CreateInstance(randomCritterType) as Critter;
            randomCritter.SetStartingLevel(UnityEngine.Random.Range(_wildEncounterLevelRange.x, _wildEncounterLevelRange.y + 1));

            StartCoroutine(DoCombat(randomCritter));
        }

        return isEnteringEncounter;
    }


    public IEnumerator DoCombat(Critter opponent)
    {
        //Pixel dissolve and musical sting
        ExclaimAnimator.SetTrigger("Exclaim");

        OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
        osc.MyClip = _alertClip;
        osc.Play();

        yield return new WaitForSeconds(0.8f);

        PixelAnimator = GameObject.Find("PixelVolume").GetComponent<Animator>();
        PixelAnimator.SetTrigger("Dissolve");

        yield return new WaitForSeconds(0.5f);

        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("Combat");

        while (!sceneLoading.isDone)
        {
            yield return null;
        }

        _wallsAndDoorsGO.SetActive(false);

        Debug.Log("looking for combat controller");

        CombatController combatController = GameObject.FindObjectOfType<CombatController>();

        combatController.SetupCombat(PlayerData, null, opponent);

        yield return null;
    }


    public void StartCollectorCombat(Collector collector)
    {
        StartCoroutine(DoCollectorCombat(collector));
    }


    public IEnumerator DoCollectorCombat(Collector collector)
    {
        //Pixel dissolve and musical sting
        PixelAnimator = GameObject.Find("PixelVolume").GetComponent<Animator>();
        PixelAnimator.SetTrigger("Dissolve");

        yield return new WaitForSeconds(0.5f);

        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("Combat");

        while (!sceneLoading.isDone)
        {
            yield return null;
        }

        _wallsAndDoorsGO.SetActive(false);

        CombatController combatController = GameObject.FindObjectOfType<CombatController>();

        Debug.Log($"This collector has {collector.GetCritters().Count} bugs");

        foreach (Critter critter in collector.GetCritters())
        {
            Debug.Log($"The collector has a {critter.Name}");
        }

        combatController.SetupCombat(PlayerData, collector, null);

        yield return null;
    }



}
