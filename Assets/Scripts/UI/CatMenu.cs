using System;
using UnityEngine;
using UnityEngine.UI;



public class CatMenu : MonoBehaviour
{
    [SerializeField] GameObject[] buts;


    private void Awake()
    {
        Close();
    }

    public void Open()
    {
        (transform as RectTransform).anchoredPosition = Helpers.WorldToScreen(Boot.player.transform.position);
        foreach (var b in buts)
        {
            b.SetActive(true);
        }
    }

    public void Close()
    {
        foreach (var b in buts)
        {
            b.SetActive(false);
        }
    }
}
