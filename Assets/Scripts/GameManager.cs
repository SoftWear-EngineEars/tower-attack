using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    [System.Serializable]
    private struct Range
    {
        public float Min;
        public float Max;

        public Range(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }

    private GameObject mainTower;
    [SerializeField] private Tier currentMaxTowerSpawnTier = Tier.I;
    [SerializeField] private Tier towerSpawnTierLimit = Tier.III;
    [SerializeField] private Range towerTierIncreaseInterval = new Range(50f, 100f);
    [SerializeField] private Range towerSpawnInterval = new Range(20f, 40f);
    [SerializeField] private Range xSpawnRange = new Range(-7f, 7f);
    [SerializeField] private Range ySpawnRange = new Range(-3f, 2.5f);
    private List<Tier> availableTowerTiers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        availableTowerTiers = new List<Tier> { currentMaxTowerSpawnTier };
        mainTower = EntityManager.Instance.SpawnTower(Tier.X, new Vector3(0, 0, 0));
        StartCoroutine(GraduallyIncreaseTowerTierLimit());
        StartCoroutine(RandomlySpawnTowers());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator GraduallyIncreaseTowerTierLimit()
    {
        while (currentMaxTowerSpawnTier < towerSpawnTierLimit)
        {
            float randomInterval = Random.Range(towerTierIncreaseInterval.Min, towerTierIncreaseInterval.Max);
            yield return new WaitForSeconds(randomInterval);

            currentMaxTowerSpawnTier = EntityManager.IncrementTier(currentMaxTowerSpawnTier);
            availableTowerTiers.Add(currentMaxTowerSpawnTier);
        }
    }

    private IEnumerator RandomlySpawnTowers()
    {
        while (mainTower != null)
        {
            float randomInterval = Random.Range(towerSpawnInterval.Min, towerSpawnInterval.Max);
            yield return new WaitForSeconds(randomInterval);
            
            EntityManager.Instance.SpawnTower(GetWeightedRandomTowerTier(), GetRandomSpawnPosition());
        }
    }

    private Tier GetWeightedRandomTowerTier()
    {
        // Calculate weights as powers of 2
        int count = availableTowerTiers.Count;
        float totalWeight = Mathf.Pow(2, count) - 1; // Sum of 2^0 + 2^1 + ... + 2^(count-1)

        // Generate a random value
        float randomValue = Random.Range(0, totalWeight);

        // Find the corresponding tier
        float cumulativeWeight = 0;
        for (int i = 0; i < count; i++)
        {
            cumulativeWeight += Mathf.Pow(2, i);
            if (randomValue < cumulativeWeight)
            {
                return availableTowerTiers[i];
            }
        }

        // Fallback (should not happen)
        return availableTowerTiers[0];
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(xSpawnRange.Min, xSpawnRange.Max);
        float randomY = Random.Range(ySpawnRange.Min, ySpawnRange.Max);
        return new Vector3(randomX, randomY, 0);
    }
}
