using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatUIController : MonoBehaviour
{
    [SerializeField] private BugInfoContainer _playerBugInfoContainer;
    [SerializeField] private BugInfoContainer _npcBugInfoContainer;

    [SerializeField] private BattleOptions _battleOptions;
    [SerializeField] private GameObject _battleOptionsObject;

    [SerializeField] private MoveOptions _moveOptions;
    [SerializeField] private GameObject _moveOptionsObject;

    [SerializeField] private ItemOptions _itemOptions;
    [SerializeField] private GameObject _itemOptionsObject;


    public void PopulateCritterInfo(Critter playerCritter, Critter npcCritter)
    {
        _playerBugInfoContainer.PopulateBugData(playerCritter);
        _npcBugInfoContainer.PopulateBugData(npcCritter);
    }
}
