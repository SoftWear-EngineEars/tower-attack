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

    private GameObject _mainTower;
    [SerializeField] private Tier currentMaxTowerSpawnTier = Tier.I;
    [SerializeField] private Tier towerSpawnTierLimit = Tier.III;
    [SerializeField] private Range towerTierIncreaseInterval = new(50f, 100f);
    [SerializeField] private Range towerSpawnInterval = new(20f, 40f);
    [SerializeField] private Range xSpawnRange = new(-7f, 7f);
    [SerializeField] private Range ySpawnRange = new(-3f, 2.5f);
    private List<Tier> _availableTowerTiers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _availableTowerTiers = new List<Tier> { currentMaxTowerSpawnTier };
        _mainTower = EntityManager.Instance.SpawnTower(Tier.X, new Vector3(0, 0, 0));
        StartCoroutine(GraduallyIncreaseTowerTierLimit());
        StartCoroutine(RandomlySpawnTowers());
    }

    private IEnumerator GraduallyIncreaseTowerTierLimit()
    {
        while (currentMaxTowerSpawnTier < towerSpawnTierLimit)
        {
            var randomInterval = Random.Range(towerTierIncreaseInterval.Min, towerTierIncreaseInterval.Max);
            yield return new WaitForSeconds(randomInterval);

            currentMaxTowerSpawnTier = EntityManager.IncrementTier(currentMaxTowerSpawnTier);
            _availableTowerTiers.Add(currentMaxTowerSpawnTier);
        }
    }

    private IEnumerator RandomlySpawnTowers()
    {
        while (_mainTower != null)
        {
            var randomInterval = Random.Range(towerSpawnInterval.Min, towerSpawnInterval.Max);
            yield return new WaitForSeconds(randomInterval);
            
            EntityManager.Instance.SpawnTower(GetWeightedRandomTowerTier(), GetRandomSpawnPosition());
        }
    }

    private Tier GetWeightedRandomTowerTier()
    {
        // Calculate weights as powers of 2
        var count = _availableTowerTiers.Count;
        var totalWeight = Mathf.Pow(2, count) - 1; // Sum of 2^0 + 2^1 + ... + 2^(count-1)

        // Generate a random value
        var randomValue = Random.Range(0, totalWeight);

        // Find the corresponding tier
        float cumulativeWeight = 0;
        for (var i = 0; i < count; i++)
        {
            cumulativeWeight += Mathf.Pow(2, i);
            if (randomValue < cumulativeWeight)
            {
                return _availableTowerTiers[i];
            }
        }

        // Fallback (should not happen)
        return _availableTowerTiers[0];
    }

    private Vector3 GetRandomSpawnPosition()
    {
        var randomX = Random.Range(xSpawnRange.Min, xSpawnRange.Max);
        var randomY = Random.Range(ySpawnRange.Min, ySpawnRange.Max);
        return new Vector3(randomX, randomY, 0);
    }
}
