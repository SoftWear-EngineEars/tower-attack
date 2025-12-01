using UnityEngine;
using System.Collections;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public int Gold { get; private set; } = 0;
    
    [SerializeField]
    private int goldPerSecond = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        StartCoroutine(IncrementGoldOverTime());
    }

    public void GainGold(int amount)
    {
        Gold += amount;
    }

    public bool SpendGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator IncrementGoldOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / goldPerSecond);
            GainGold(1);
        }
    }
}
