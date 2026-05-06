using UnityEngine;


public enum GameState
{
    Fishing,
    Battle
}

public class GameManager : MonoBehaviour
{
    public GameState State { get; private set; }

    public void ChangeState(GameState st)
    {
        State = st;
        if (Log.man) Debug.Log("New game state: " + State.ToString());
        
        //var p = Boot.Player;
        switch (st)
        {
            case GameState.Fishing:
                break;
            case GameState.Battle:
                break;
        }
    }

    public void EnterBattle(SBattleDatas battle)
    {
        if (State == GameState.Battle) return;
        ChangeState(GameState.Battle);
        Boot.bat.Init(battle);
    }

    public void EndBattle()
    {
        ChangeState(GameState.Fishing);
        Boot.ui.WaitMode();
        Boot.player.EndFishing();
    }
}
