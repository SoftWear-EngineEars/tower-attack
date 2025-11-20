using UnityEngine;
using TMPro;

public class GoldTextUpdater : MonoBehaviour
{

    public TextMeshProUGUI goldText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goldText = GetComponent<TextMeshProUGUI>();
        UpdateGoldText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGoldText();
    }

    private void UpdateGoldText()
    {
        goldText.text = "Gold: " + ResourceManager.Instance.Gold;
    }
}
