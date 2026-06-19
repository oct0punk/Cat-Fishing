using UnityEngine;
using UnityEngine.UI;

public class Aquarium : MonoBehaviour
{
    public Button backButton;

    private void Awake()
    {
        if (Boot.game)
        {
            Debug.Log("Boot game found");
            backButton.onClick.AddListener(Boot.game.ExitAquarium);
        }
        else
            Debug.LogWarning("Boot game missing");
    }
}
