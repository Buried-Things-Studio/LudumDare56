using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;



public class CombatState
{
    public Critter PlayerCritter;
    public Critter NpcCritter;
    public bool IsPlayerPriority;

    public MoveID PlayerSelectedMoveID;
    public MoveID NpcSelectedMoveID;

    public Guid PlayerSelectedHealItemTarget;
    public Guid PlayerSelectedSwitchActiveGUID;
}


public class CombatController : MonoBehaviour
{
    public Transform PlayerMeshParent;
    public Transform NpcMeshParent;
    public GameObject PlayerMesh;
    public GameObject NpcMesh;
    [SerializeField] private CombatUIController _viz;
    
    public Player PlayerData;
    public Collector OpponentData;
    public CombatState State = new CombatState();

    private bool _isChangingScene;
    
    
    [Header("Audio")]
    [SerializeField] private GameObject _oneShotGO;
    [SerializeField] private AudioClip _whooshClip;
    [SerializeField] private AudioClip _squishClip;


    public void SetupCombat(Player playerData, Collector opponentData, Critter npcCritter)
    {
        PlayerData = playerData;
        OpponentData = opponentData;
        State.PlayerCritter = PlayerData.GetActiveCritter();
        State.NpcCritter = OpponentData == null ? npcCritter : OpponentData.GetActiveCritter();
        State.PlayerCritter.ResetTemporaryStats();
        State.NpcCritter.ResetTemporaryStats();

        
        InitializeMeshes();
        _viz.InitializeCombatUI(this, playerData, State.PlayerCritter, State.NpcCritter);

        List<Critter> playerCritters = playerData.GetCritters();
        if(opponentData != null && opponentData.IsBoss())
        {
            foreach(Critter critter in playerCritters)
            {
                if(critter.Ability.ID == AbilityID.EmergencyMedPack)
                {
                    critter.RestoreAllHealth();
                    _viz.UpdatePlayerBugData(State.PlayerCritter);
                    _viz.AddVisualStep(new FullHealStep(critter.Name));
                }
            }
        }

        StartCoroutine(InitializeTurn());
    }


    private void InitializeMeshes()
    {
        GameObject playerPrefab = Resources.Load<GameObject>("Bugs/" + State.PlayerCritter.Name.Replace(" ", "")) as GameObject;
        GameObject npcPrefab = Resources.Load<GameObject>("Bugs/" + State.NpcCritter.Name.Replace(" ", "")) as GameObject;

        PlayerMesh = GameObject.Instantiate(playerPrefab);
        PlayerMesh.transform.SetParent(PlayerMeshParent);
        PlayerMesh.transform.localPosition = Vector3.zero;
        PlayerMesh.transform.localRotation = Quaternion.Euler(Vector3.zero);

        NpcMesh = GameObject.Instantiate(npcPrefab);
        NpcMesh.transform.SetParent(NpcMeshParent);
        NpcMesh.transform.localPosition = Vector3.zero;
        NpcMesh.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }


    private IEnumerator InitializeTurn()
    {
        yield return StartCoroutine(_viz.ExecuteVisualSteps());
        ClearTurnData();
        PlayerData.ClearDeadCritters();
        yield return StartCoroutine(GetNewActiveCritters());
        DetermineStartingCritter();
        PopulateParticipant();
        PickNpcMove();
        _viz.StartPlayerBattleActionChoice();

        yield return null;
    }


    private IEnumerator GetNewActiveCritters()
    {
        if (State.PlayerCritter.CurrentHealth <= 0)
        {
            PlayerMesh.GetComponentInParent<Animator>().SetTrigger("Squish");

            OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
            osc.MyClip = _squishClip;
            osc.Play();

            yield return new WaitForSeconds(0.5f);
            
            yield return StartCoroutine(_viz.ChooseNewCritter());

            State.PlayerCritter = PlayerData.GetActiveCritter();
            string critterName = State.PlayerCritter.Name;
            string article =
                State.PlayerCritter.Name.StartsWith("A")
                || State.PlayerCritter.Name.StartsWith("E")
                || State.PlayerCritter.Name.StartsWith("I")
                || State.PlayerCritter.Name.StartsWith("O")
                || State.PlayerCritter.Name.StartsWith("U")
                ? "an"
                : "a";

            yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage($"Let's try {article} {critterName}!"));

            Debug.Log("Change active step switch animation...");
                    
            PlayerMeshParent.transform.parent.parent.GetComponent<Animator>().SetTrigger("Back");

            Debug.Log("Set Back, now waiting...");

            yield return new WaitForSeconds(0.4f);

            PlayerMeshParent.GetComponent<Animator>().SetTrigger("Idle");

            yield return new WaitForSeconds(0.1f);

            Debug.Log("Setting Out");

            GameObject newPrefab = Resources.Load<GameObject>("Bugs/" + State.PlayerCritter.Name.Replace(" ", "")) as GameObject;

            GameObject.Destroy(PlayerMesh);

            yield return null;

            PlayerMesh = GameObject.Instantiate(newPrefab);
            PlayerMesh.transform.SetParent(PlayerMeshParent);
            PlayerMesh.transform.localPosition = Vector3.zero;
            PlayerMesh.transform.localRotation = Quaternion.Euler(Vector3.zero);
            PlayerMesh.transform.localScale = Vector3.one;

            PlayerMeshParent.transform.parent.parent.GetComponent<Animator>().SetTrigger("Out");

            yield return new WaitForSeconds(1f);

            _viz.UpdatePlayerBugData(State.PlayerCritter);
        }

        if (OpponentData != null && State.NpcCritter.CurrentHealth <= 0)
        {
            State.NpcCritter = OpponentData.GetActiveCritter();
            string critterName = State.NpcCritter.Name;
            string article =
                State.PlayerCritter.Name.StartsWith("A")
                || State.PlayerCritter.Name.StartsWith("E")
                || State.PlayerCritter.Name.StartsWith("I")
                || State.PlayerCritter.Name.StartsWith("O")
                || State.PlayerCritter.Name.StartsWith("U")
                ? "an"
                : "a";

            yield return StartCoroutine(GlobalUI.TextBox.ShowSimpleMessage($"The opponent releases {article} {critterName}!"));

            Debug.Log("Change active step switch animation...");
                    
            NpcMeshParent.transform.parent.parent.GetComponent<Animator>().SetTrigger("Back");

            Debug.Log("Set Back, now waiting...");

            yield return new WaitForSeconds(0.4f);

            NpcMeshParent.GetComponent<Animator>().SetTrigger("Idle");

            yield return new WaitForSeconds(0.1f);

            Debug.Log("Setting Out");

            GameObject newPrefab = Resources.Load<GameObject>("Bugs/" + State.NpcCritter.Name.Replace(" ", "")) as GameObject;

            GameObject.Destroy(NpcMesh);

            yield return null;

            NpcMesh = GameObject.Instantiate(newPrefab);
            NpcMesh.transform.SetParent(NpcMeshParent);
            NpcMesh.transform.localPosition = Vector3.zero;
            NpcMesh.transform.localRotation = Quaternion.Euler(Vector3.zero);
            NpcMesh.transform.localScale = Vector3.one;

            NpcMeshParent.transform.parent.parent.GetComponent<Animator>().SetTrigger("Out");

            yield return new WaitForSeconds(1f);

            _viz.UpdateNpcBugData(State.NpcCritter);
        }

        yield return null;
    }


    private void ClearTurnData()
    {
        State.PlayerSelectedMoveID = MoveID.None;
        State.NpcSelectedMoveID = MoveID.None;
        State.PlayerSelectedHealItemTarget = Guid.Empty;
        State.PlayerSelectedSwitchActiveGUID = Guid.Empty;
    }


    private void DetermineStartingCritter()
    {
        int playerSpeed = CritterHelpers.GetEffectiveSpeed(State.PlayerCritter);
        int npcSpeed = CritterHelpers.GetEffectiveSpeed(State.NpcCritter);
        
        if (playerSpeed == npcSpeed)
        {
            State.IsPlayerPriority = UnityEngine.Random.Range(0, 2) == 0;

            return;
        }

        State.IsPlayerPriority = playerSpeed > npcSpeed;
    }


    private void PopulateParticipant()
    {
        if (!State.NpcCritter.Participants.Contains(State.PlayerCritter.GUID))
        {
            State.NpcCritter.Participants.Add(State.PlayerCritter.GUID);
        }
    }


    private void PickNpcMove()
    {
        if (OpponentData == null)
        {
            State.NpcSelectedMoveID = NpcLogic.GetWildMoveChoice(State).ID;
        }
        else if (OpponentData.IsBoss())
        {
            State.NpcSelectedMoveID = NpcLogic.GetBossMoveChoice(State).ID;
        }
        else
        {
            State.NpcSelectedMoveID = NpcLogic.GetCollectorMoveChoice(State).ID;
        }
    }


    public void SetPlayerMove(MoveID moveID)
    {
        State.PlayerSelectedMoveID = moveID;
        ExecuteMoves();
    }


    private void ExecuteMoves()
    {
        Critter priorityCritter = State.IsPlayerPriority ? State.PlayerCritter : State.NpcCritter;
        Critter nonPriorityCritter = State.IsPlayerPriority ? State.NpcCritter : State.PlayerCritter;
        MoveID priorityMoveID = State.IsPlayerPriority ? State.PlayerSelectedMoveID : State.NpcSelectedMoveID;
        MoveID nonPriorityMoveID = State.IsPlayerPriority ? State.NpcSelectedMoveID : State.PlayerSelectedMoveID;
        Move priorityMove = priorityCritter.Moves.Find(move => move.ID == priorityMoveID);
        Move nonPriorityMove = nonPriorityCritter.Moves.Find(move => move.ID == nonPriorityMoveID);

        if (State.PlayerSelectedMoveID == MoveID.SwitchActive || State.PlayerSelectedMoveID == MoveID.ThrowMasonJar || State.PlayerSelectedMoveID == MoveID.UseHealItem)
        {
            State.IsPlayerPriority = true;
            priorityCritter = State.PlayerCritter;
            nonPriorityCritter = State.NpcCritter;
            priorityMoveID = State.PlayerSelectedMoveID;
            priorityMove = null;
            nonPriorityMoveID = State.NpcSelectedMoveID;
            nonPriorityMove = State.NpcCritter.Moves.Find(move => move.ID == nonPriorityMoveID);
        }
        else if (State.NpcSelectedMoveID == MoveID.SwitchActive || State.NpcSelectedMoveID == MoveID.UseHealItem)
        {
            State.IsPlayerPriority = false;
            priorityCritter = State.NpcCritter;
            nonPriorityCritter = State.PlayerCritter;
            priorityMoveID = State.NpcSelectedMoveID;
            priorityMove = null;
            nonPriorityMoveID = State.PlayerSelectedMoveID;
            nonPriorityMove = State.PlayerCritter.Moves.Find(move => move.ID == nonPriorityMoveID);
        }

        bool isNonPriorityDead = false;
        bool isWildCatchAttempt = false;
        bool isEndingCombat = false;

        if (priorityMove == null)
        {
            if (priorityMoveID == MoveID.TriedItsBest)
            {
                if(priorityCritter.Ability.ID == AbilityID.StruggleBetter)
                {
                    List<Move> allMoves = MasterCollection.GetAllMoves();
                    Move randomMove = allMoves[UnityEngine.Random.Range(0, allMoves.Count)];
                    TryExecuteMove(priorityCritter, randomMove, State.IsPlayerPriority);
                    if (CheckDeath())
                    {
                        isEndingCombat = ExecuteDeath();
                        
                        isNonPriorityDead = true;
                    }

                }
                else{
                    TryExecuteMove(priorityCritter, new TriedItsBest(), State.IsPlayerPriority);

                    if (CheckDeath())
                    {
                        isEndingCombat = ExecuteDeath();
                        
                        isNonPriorityDead = true;
                    }
                }
            }
            else
            {
                isWildCatchAttempt = ExecuteBattleAction(priorityMoveID, priorityCritter);
            }
        }
        else
        {
            TryExecuteMove(priorityCritter, priorityMove, State.IsPlayerPriority);

            if (CheckDeath())
            {
                isEndingCombat = ExecuteDeath();
                
                isNonPriorityDead = true;
            }
        }

        if (!isNonPriorityDead && !isWildCatchAttempt)
        {
            if (nonPriorityMove == null)
            {
                if (nonPriorityMoveID == MoveID.TriedItsBest)
                {
                    if(nonPriorityCritter.Ability.ID == AbilityID.StruggleBetter)
                    {
                        List<Move> allMoves = MasterCollection.GetAllMoves();
                        Move randomMove = allMoves[UnityEngine.Random.Range(0, allMoves.Count)];
                        TryExecuteMove(nonPriorityCritter, randomMove, !State.IsPlayerPriority);
                        if (CheckDeath())
                        {
                            isEndingCombat = isEndingCombat || ExecuteDeath();
                        }

                    } else {
                        TryExecuteMove(nonPriorityCritter, new TriedItsBest(), !State.IsPlayerPriority);

                        if (CheckDeath())
                        {
                            isEndingCombat = isEndingCombat || ExecuteDeath();
                        }
                    }
                }
                else
                {
                    ExecuteBattleAction(nonPriorityMoveID, nonPriorityCritter);
                }
            }
            else
            {
                TryExecuteMove(nonPriorityCritter, nonPriorityMove, !State.IsPlayerPriority);

                if (CheckDeath())
                {
                    isEndingCombat = isEndingCombat || ExecuteDeath();
                }
            }
        }

        if (isWildCatchAttempt || isEndingCombat)
        {
            StartCoroutine(GoToMainGame());
        }
        else
        {
            StartCoroutine(ShowTurn());
        }
    }


    private IEnumerator ShowTurn()
    {
        if (!_isChangingScene)
        {
            yield return StartCoroutine(_viz.ExecuteVisualSteps());
            
            StartCoroutine(InitializeTurn());
        }
    }


    private bool ExecuteBattleAction(MoveID ID, Critter activeCritter)
    {
        if (ID == MoveID.SwitchActive)
        {
            PlayerSwitchActive();
        }
        else if (ID == MoveID.ThrowMasonJar)
        {
            return PlayerThrowMasonJar(activeCritter);
        }
        else if (ID == MoveID.UseHealItem)
        {
            PlayerUseHealItem();
        }

        return false;
    }


    private bool PlayerThrowMasonJar(Critter activeCritter)
    {
        PlayerData.RemoveItemFromInventory(ItemType.MasonJar);
        
        bool isCatchSuccessful =
            OpponentData == null
            && PlayerData.GetCritters().Count < CritterHelpers.MaxTeamSize
            && State.NpcCritter.CurrentHealth <= CritterHelpers.GetCatchHealthThreshold(State.NpcCritter);
        
        if (OpponentData != null)
        {
            _viz.AddVisualStep(new TryCatchCollectorCritterStep());
        }
        else if (PlayerData.GetCritters().Count >= CritterHelpers.MaxTeamSize)
        {
            if(activeCritter.Ability.ID == AbilityID.BugMuncher && State.NpcCritter.CurrentHealth <= CritterHelpers.GetCatchHealthThreshold(State.NpcCritter))
            {
                List<string> stats = CritterHelpers.GetAllCritterStats();
                string statToIncrease = stats[UnityEngine.Random.Range(0, stats.Count)];
                activeCritter.IncreaseSingleStat(statToIncrease);
                _viz.AddVisualStep(new BugMuncherStep(activeCritter.Name, statToIncrease));

            }
            else 
            {
                _viz.AddVisualStep(new TryCatchTooFullCritterStep());
            }
        }
        else if(State.NpcCritter.CurrentHealth > CritterHelpers.GetCatchHealthThreshold(State.NpcCritter))
        {
            _viz.AddVisualStep(new TryCatchStep(State.NpcCritter.Name, isCatchSuccessful));
        }

        if (isCatchSuccessful)
        {
            if(activeCritter.Ability.ID == AbilityID.BugMuncher)
            {
                List<string> stats = CritterHelpers.GetAllCritterStats();
                string statToIncrease = stats[UnityEngine.Random.Range(0, stats.Count)];
                activeCritter.IncreaseSingleStat(statToIncrease);
                _viz.AddVisualStep(new BugMuncherStep(activeCritter.Name, statToIncrease));
            }
            else 
            {
                int chancesForAbility = 0;
                bool getsRandomAbility = false;
                foreach(Critter critter in PlayerData.GetCritters())
                {
                    if(critter.Ability.ID == AbilityID.SkillfulSavages)
                    {
                        chancesForAbility ++;
                    }
                }
                for(int i = 0; i < chancesForAbility; i++)
                {
                    int chance = UnityEngine.Random.Range(0, 2);
                    if(chance < 1)
                    {
                        getsRandomAbility = true;
                    }
                }
                PlayerData.AddCritter(State.NpcCritter);
                State.NpcCritter.RestoreAllHealth();
                State.NpcCritter.RestoreAllMoveUses();
                if(getsRandomAbility)
                {
                    List<Ability> allAbilities = MasterCollection.GetAllAbilities();
                    Ability randomAbility = allAbilities[UnityEngine.Random.Range(0, allAbilities.Count)];
                    State.NpcCritter.AddAbility(randomAbility);
                    _viz.AddVisualStep(new SkillfulSavagesStep(State.NpcCritter.Name, randomAbility.Name));

                }
                
            }
        }

        return OpponentData == null;
    }


    private void PlayerUseHealItem()
    {
        PlayerData.RemoveItemFromInventory(ItemType.Nectar);
        Critter healTarget = PlayerData.GetCritters().Find(critter => critter.GUID == State.PlayerSelectedHealItemTarget);

        int startingHealth = healTarget.CurrentHealth;
        healTarget.IncreaseHealth(30);

        _viz.AddVisualStep(new HealMessageStep(healTarget.Name, healTarget.CurrentHealth - startingHealth));
        _viz.AddVisualStep(new HealthChangeStep(true, healTarget.Level, startingHealth, healTarget.CurrentHealth, healTarget.MaxHealth));
    }


    private void PlayerSwitchActive()
    {
        State.PlayerCritter.ResetTemporaryStats();
        PlayerData.SetActiveCritter(State.PlayerSelectedSwitchActiveGUID);
        State.PlayerCritter = PlayerData.GetActiveCritter();

        PopulateParticipant();

        _viz.AddVisualStep(new ChangeActiveStep(State.PlayerCritter, State.PlayerCritter.Name));
    }


    private void TryExecuteMove(Critter user, Move move, bool isPlayerUser)
    {
        if (user.StatusEffects.Exists(status => status.StatusType == StatusEffectType.Confuse))
        {
            bool isNoLongerConfused = user.ReduceConfuseTurnsRemaining();
            _viz.AddVisualStep(new ConfuseStatusUpdateStep(user.Name, !isNoLongerConfused));

            if (!isNoLongerConfused && UnityEngine.Random.Range(0, 2) == 0)
            {
                _viz.AddVisualStep(new ConfuseCheckFailureStep(user.Name));
                FailConfusionCheck(user, isPlayerUser);

                return;
            }
        }

        if (move.ID != MoveID.TriedItsBest)
        {
            _viz.AddVisualStep(new DoMoveStep(user.Name, move.Name, move.BasePower > 0, isPlayerUser, !isPlayerUser, !move.IsSharp));
        }
        else
        {
            _viz.AddVisualStep(new TriedItsBestStep(user.Name, isPlayerUser, !isPlayerUser));
        }
        
        if (move.ID == MoveID.TriedItsBest || !move.IsTargeted || UnityEngine.Random.Range(0, 100) < move.Accuracy)
        {
            List<CombatVisualStep> steps = move.ExecuteMove(State, isPlayerUser);
            _viz.AddVisualSteps(steps);
        }
        else
        {
            _viz.AddVisualStep(new MoveAccuracyCheckFailureStep(user.Name));
        }

        if(user.Ability.ID == AbilityID.PPOrNotPP)
        {
            int randomInt = UnityEngine.Random.Range(0,2);
            if(randomInt > 0)
            {
                _viz.AddVisualStep(new NoPPLossStep(user.Name, move.Name));
            }
            else
            {
                move.CurrentUses--;
            }

        }
        else
        {
            move.CurrentUses--;
        }

    }


    private void FailConfusionCheck(Critter critter, bool isPlayerCritter)
    {
        int startingHealth = critter.CurrentHealth;
        critter.DealDamage(CritterHelpers.GetConfusionDamage(critter));
        _viz.AddVisualStep(new HealthChangeStep(isPlayerCritter, critter.Level, startingHealth, critter.CurrentHealth, critter.MaxHealth));
    }


    private bool CheckDeath()
    {
        return State.PlayerCritter.CurrentHealth <= 0 || State.NpcCritter.CurrentHealth <= 0;
    }


    private bool ExecuteDeath()
    {
        if (State.NpcCritter.CurrentHealth <= 0)
        {
            _viz.AddVisualStep(new CritterSquishedStep(State.NpcCritter.Name));
            
            List<Critter> crittersReceivingExp = State.NpcCritter.Participants
                .Select(participantGuid => PlayerData.GetCritters().Find(teamCritter => teamCritter.GUID == participantGuid))
                .Where(critter => critter != null && critter.CurrentHealth > 0)
                .ToList();
            
            foreach (Critter critter in crittersReceivingExp)
            {
                List<CombatVisualStep> expSteps = critter.IncreaseExp(State.NpcCritter.Level * 250 / crittersReceivingExp.Count);
                _viz.AddVisualSteps(expSteps);
            }
        }

        if (!PlayerData.GetCritters().Exists(critter => critter.CurrentHealth > 0))
        {
            StartCoroutine(GoToLose());

            return false;
        }
        else if (OpponentData != null && !OpponentData.GetCritters().Exists(critter => critter.CurrentHealth > 0))
        {
            int winnings = 150 * OpponentData.GetCritters().Count;
            PlayerData.AddMoney(winnings);

            _viz.AddVisualStep(new WinningsStep(winnings));

            StartCoroutine(GoToMainGame());

            return false;
        }
        else if (OpponentData == null && State.NpcCritter.CurrentHealth <= 0)
        {
            //StartCoroutine(GoToMainGame());
            return true;
        }

        return false;
    }


    private IEnumerator GoToWin()
    {
        _isChangingScene = true;
        
        yield return StartCoroutine(_viz.ExecuteVisualSteps());

        GameObject.Find("PixelVolume").GetComponent<Animator>().SetTrigger("Dissolve");
        yield return new WaitForSeconds(0.5f);

        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("WinGame");

        while (!sceneLoading.isDone)
        {
            yield return null;
        }
    }


    private IEnumerator GoToLose()
    {
        _isChangingScene = true;
        
        yield return StartCoroutine(_viz.ExecuteVisualSteps());

        GameObject.Find("PixelVolume").GetComponent<Animator>().SetTrigger("Dissolve");
        yield return new WaitForSeconds(0.5f);

        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("LoseGame");

        while (!sceneLoading.isDone)
        {
            yield return null;
        }
    }


    private IEnumerator GoToMainGame()
    {
        _isChangingScene = true;
        
        yield return StartCoroutine(_viz.ExecuteVisualSteps());

        OneShotController osc = Instantiate(_oneShotGO).GetComponent<OneShotController>();
        osc.MyClip = _whooshClip;
        osc.Play();

        GameObject.Find("PixelVolume").GetComponent<Animator>().SetTrigger("Dissolve");
        yield return new WaitForSeconds(0.5f);

        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("MainGame");

        while (!sceneLoading.isDone)
        {
            yield return null;
        }
    }
}
