using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogText : MonoBehaviour
{
    [SerializeField] Image bg;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float dur = 3.0f;
    float t = 0.0f;
    Color col;

    private void Start()
    {
        col = bg.color;
    }

    // Update is called once per frame
    void Update()
    {
        t -= Time.deltaTime;
        bg.color = Helpers.ColAlpha(col, Mathf.Min(t, col.a));
        text.alpha = t;

        if (t <= 0.0f)
        {
            enabled = false;
            return;
        }
    }

    public void PrintLog(string mes)
    {
        text.text = mes + "\n" + text.text;
        t = dur;
        enabled = true;
        if (Log.ui) Debug.Log("log: " + mes);
    }
}
