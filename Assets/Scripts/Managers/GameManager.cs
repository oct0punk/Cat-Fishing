using System;
using UnityEngine;


public enum GameState
{
    Fishing,
    Battle,
    Catch,
    Menu,
}

public class GameManager : MonoBehaviour
{
    public GameState State { get; private set; }


    public void ChangeState(GameState st)
    {
        State = st;
        if (Boot.Logs.man) Debug.Log("New game state: " + State.ToString());
        
        //var p = Boot.Player;
        //switch (st)
    }

    public void Fishing()
    {
        if (Boot.Logs.game) Debug.Log("Fishing State");
        Boot.con.RemoveAllListeners();
        ChangeState(GameState.Fishing);
        Boot.player.FishingMode();
        Boot.ui.FishingUI();
        Boot.con.WaitForNoTouch();
    }

    public void BeforeBattle(SBattleDatas battle)
    {
        if (State == GameState.Battle) return;
        ChangeState(GameState.Battle);
        Boot.bat.Init(battle);
    }
    public void Battle()
    {
        if (Boot.Logs.game) Debug.Log("Battle State");
        Boot.ui.PrintLog("BeginBattle!!");
        Boot.ui.BattleUI();
        Boot.player.BattleMode();
        Boot.bat.enabled = true;
    }

    public void Catch()
    {
        if (Boot.Logs.game) Debug.Log("Catch State");
        ChangeState(GameState.Catch);
        Boot.ui.CatchUI();
        Boot.player.CatchMode();
        Boot.con.WaitForNoTouch();
        Boot.con.onTouchUp.AddListener(Fishing);
    }

    public void OpenMenu()
    {
        if (State == GameState.Menu) { 
            CloseMenu(); 
            return; }
        ChangeState(GameState.Menu);
        Boot.ui.OpenMenu();
        Boot.player.FishingMode();
    }

    public void CloseMenu()
    {
        Boot.con.WaitForNoTouch();
        ChangeState(GameState.Fishing);
        Boot.ui.CloseMenu();
    }

    public void GoToAquarium()
    {
        CloseMenu();
    }
}
