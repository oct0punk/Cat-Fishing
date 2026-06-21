using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public interface IScreenUI
{
    public void Display();
    public void Hide();
}

[Serializable]
public struct PlayScreen : IScreenUI
{
    public void Display()
    {
        Boot.ui.PrintLog("Play screen");
    }

    public void Hide()
    {
        Boot.ui.PrintLog("Hide play screen");
    }
}

[Serializable]
public struct Menu : IScreenUI
{
    [SerializeField] GameObject parent;
    [SerializeField] GameObject title;
    [SerializeField] GameObject touch;
    [SerializeField] GameObject credits;


    public void PlayTitle()
    {
        title.SetActive(true);
        credits.SetActive(true);
        touch.SetActive(false);
        if (Boot.Logs.ui) Debug.Log("Play Title", title);

    }
    public void TouchHint()
    {
        parent.SetActive(true);
        title.SetActive(false);
        credits.SetActive(false);
        touch.SetActive(true);
        if (Boot.Logs.ui) Debug.Log("Touch Hint", touch);
    }
    public void Display()
    {
        parent.SetActive(true);
        PlayTitle();
    }
    public void Hide()
    {
        parent.SetActive(false);
        //if (!isOn) return;
        title.SetActive(false);
        credits.SetActive(false);
        touch.SetActive(false);
        if (Boot.Logs.ui) Debug.Log("Hide menu");
    }
}

[Serializable]
public struct StaminaBar : IScreenUI
{
    [SerializeField] GameObject parent;
    [SerializeField] Image bar;


    public void Display()
    {
        parent.SetActive(true);
        if (Boot.Logs.ui)
        {
            Debug.Log("Show stamina bar", parent);
        }
    }
    public void Hide()
    {
        parent.SetActive(false);
        if (Boot.Logs.ui)
        {
            Debug.Log("Hide stamina bar", parent);
        }
    }
    public void SetValue(float prog)
    {
        bar.fillAmount = prog;
    }
}

[Serializable]
public struct CatchScreen : IScreenUI
{
    [SerializeField] GameObject parent;


    public void Display()
    {
        parent.SetActive(true);
    }
    public void Hide()
    {
        parent.SetActive(false);
    }
}


public class UIManager : MonoBehaviour
{
    private bool hasBegunFishing = false;
    private UnityAction removeMenu;
    private IScreenUI[] screens;

    [Header("Sprites")]
    [SerializeField] Sprite catchIm;
    [Header("Refs")]
    [SerializeField] Canvas canvas;
    [SerializeField] Menu menu;
    [SerializeField] StaminaBar bar;
    [SerializeField] CatchScreen catchScreen;
    [Space]
    [SerializeField] CatMenu catMenu;
    [SerializeField] LogText log;
    [SerializeField] bool disableLog;


    private void Awake()
    {
        screens = new IScreenUI[4] { menu, bar, new PlayScreen(), catchScreen };
        MenuUI();
    }

    void Focus<T>() where T : IScreenUI
    {
        foreach (IScreenUI scr in screens)
        {
            if (scr is T)
                scr.Display();
            else
                scr.Hide();
        }
    }
    void NoScreen()
    {
        Focus<PlayScreen>();
    }

    void ShowTouchHintIfNoInput()
    {
        if (hasBegunFishing) return;
        if (Boot.Logs.ui) Debug.Log("Show hint if no touch");
        var p = Boot.player;
        if (p == null) throw Boot.MissingPlayerReference();
        if (p != null) p.onStartFishing.RemoveListener(removeMenu);
        if (p.isFishing) { menu.Hide(); return; }

        menu.TouchHint();
        removeMenu = () => HideTouchIfInput();
        p.onStartFishing.AddListener(removeMenu);

    }
    void HideTouchIfInput()
    {
        if (Boot.Logs.ui) Debug.Log("Remove hint if touch");
        menu.Hide();
        var p = Boot.player;
        if (p != null) p.onStartFishing.RemoveListener(removeMenu);
    }

    public void MenuUI()
    {
        Focus<Menu>();
        removeMenu = () => {
            hasBegunFishing = true;
            menu.Hide();
            var p = Boot.player;
            if (p != null) p.onStartFishing.RemoveListener(removeMenu);
        };
        Boot.player.onStartFishing.AddListener(removeMenu);
        Invoke(nameof(ShowTouchHintIfNoInput), 3.0f);
    }
    public void FishingUI() => NoScreen();
    public void BattleUI() => Focus<StaminaBar>();
    public void CatchUI() => Focus<CatchScreen>();

    public void SetStamina(float progress)
    {
        bar.SetValue(progress);
    }

    public void PrintLog(string mes)
    {
        if (disableLog) return;
        log.PrintLog(mes);
    }

    public void SpawnCatchText(Vector3 pos)
    {
        var tmp = SpawnText("Catch!!");
        tmp.fontSize = 96;
        tmp.fontStyle = FontStyles.Bold;
        tmp.color = Color.pink;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.textWrappingMode = TextWrappingModes.NoWrap;

        var spawnPos = pos;
        spawnPos += Boot.cam.main.transform.up * 2.0f;
        spawnPos += 2.4f * Mathf.Sin(Time.timeSinceLevelLoad) * Boot.cam.main.transform.right;
        tmp.rectTransform.anchoredPosition = Helpers.WorldToScreen(spawnPos);
        tmp.rectTransform.rotation = Quaternion.LookRotation(Vector3.forward, spawnPos - pos);

        Boot.twn.Oscillate(tmp.transform, Boot.Datas.BattlePreDuration);
        Destroy(tmp.gameObject, Boot.Datas.BattlePreDuration);
    }

    public void SpawnCatchImage()
    {
        var ig = SpawnImage(catchIm);
        ig.rectTransform.pivot = Vector2.down;

        Vector3 spawnPos = Boot.player.transform.position;
        spawnPos += Boot.cam.main.transform.up * 0.4f;
        spawnPos += 2f * Mathf.Sin(Time.timeSinceLevelLoad) * Boot.cam.main.transform.right;
        ig.rectTransform.position = Helpers.WorldToScreen(spawnPos);
        ig.rectTransform.localScale = Vector3.one * 2f;

        ig.CrossFadeAlpha(0f, 0f, true);
        ig.CrossFadeAlpha(1f, Boot.Datas.BattlePreDuration/2, true);
        Destroy(ig.gameObject, Boot.Datas.BattlePreDuration);
    }

    TextMeshProUGUI SpawnText(string txt)
    {
        var go = new GameObject(txt, typeof(RectTransform));
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.rectTransform.anchorMin = Vector2.zero;
        tmp.rectTransform.anchorMax = Vector2.zero;
        tmp.text = txt;
        go.transform.SetParent(canvas.transform, false);
        return tmp;
    }

    Image SpawnImage(Sprite spr)
    {
        var go = new GameObject("Image", typeof(RectTransform));
        var ig = go.AddComponent<Image>();
        ig.sprite = spr;
        ig.rectTransform.anchorMin = Vector2.zero;
        ig.rectTransform.anchorMax = Vector2.zero;
        go.transform.SetParent(canvas.transform, false);
        return ig;
    }

    public void OpenMenu()
    {
        //if (Boot.Logs.ui)        
        Debug.Log("OpenMenu");
        catMenu.Open();
    }

    public void CloseMenu()
    {
        //if (Boot.Logs.ui)
        Debug.Log("CloseMenu");
        catMenu.Close();
    }
}