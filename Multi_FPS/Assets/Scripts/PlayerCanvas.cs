using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    public static PlayerCanvas canvas;

    [Header("Component References")]
    [SerializeField]
    Text gameStatusText;
    [SerializeField] Text healthValue;
    [SerializeField] Text killsValue;
    public Image crossHair;

    void Awake()
    {
        if (canvas == null)
            canvas = this;
        else if (canvas != this)
            Destroy(gameObject);
    }

    public void Initialize()
    {
        gameStatusText.text = "";
    }

    public void SetKills(int amount)
    {
        killsValue.text = amount.ToString();
    }

    public void SetHealth(int amount)
    {
        healthValue.text = amount.ToString();
    }

    public void WriteGameStatusText(string text)
    {
        gameStatusText.text = text;
    }


}