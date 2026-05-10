using System.Collections;
using UnityEngine;


public struct SBattleDatas
{
    public Fish fish;

    public SBattleDatas(Fish fish)
    {
        this.fish = fish;
    }
}


public class BattleManager : MonoBehaviour
{
    public float maxHp;
    public float curHp;
    public SBattleDatas conf;


    void Update()
    {
        if (Boot.game.State != GameState.Battle) return;

        IsoSprite.TickAll();

        curHp -= Time.deltaTime;

        if (curHp < 0.0f)
            OnCatch();
        else
            Boot.ui.SetStamina(curHp / maxHp);
    }

    public void Init(SBattleDatas c)
    {
        conf = c;
        Fish f = conf.fish;
        Boot.fish.FleeAll(f);
        maxHp = curHp = f.dat.hp;
        StartCoroutine(OnBattleRoutine());
    }
    public void Cancel()
    {
        OnEnd();
    }

    public void OnFishChangingDirection(Vector3 dir)
    {
        var ang = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        Boot.cam.Rotate(ang);
    }

    void OnEnd()
    {
        Boot.ui.PrintLog("OnEndBattle");
        enabled = false;
        StartCoroutine(OnEndBattleRoutine());
    }
    void OnCatch()
    {
        Boot.fish.Catch(conf.fish);
        OnEnd();
    }


    IEnumerator OnBattleRoutine()
    {
        PlayerController p = Boot.player;
        Fish f = conf.fish;
        UIManager ui = Boot.ui;
        CameraManager cam = Boot.cam;
        TweenManager tw = Boot.twn;

        ui.PrintLog("OnHook");
        ui.SpawnCatchText(f.transform.position);
        ui.SpawnCatchImage();
        cam.Follow(f);

        yield return Helpers.Wait(Boot.Datas.PreBattleDuration);
        Boot.game.BattleMode();
        f.Battle();
    }

    IEnumerator OnEndBattleRoutine()
    {
        yield return Helpers.Wait(Boot.Datas.PostBattleDuration);
        Boot.cam.Reset();
        Boot.fish.Kill(conf.fish);
        Boot.game.EndBattle();
        //Boot.ui.CatchMode();
        Boot.ui.PrintLog("EndBattleState");
    }
}
