using UnityEngine;
using TMPro;

public class GoldTextUpdater : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        goldText = GetComponent<TextMeshProUGUI>();
        UpdateGoldText();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateGoldText();
    }

    private void UpdateGoldText()
    {
        goldText.text = "Gold: " + ResourceManager.Instance.Gold;
    }
}
