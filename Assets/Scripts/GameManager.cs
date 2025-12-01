using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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
    
    private Coroutine _spawningCoroutine; // To prevent towers from spawning in the game over scene

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    } 
    
    // Called every time a scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
        {
            InitializeGame();
        }
    }

     void InitializeGame() // What was in start before
    {
        // Stop any previous coroutines just in case
        if (_spawningCoroutine != null)
        {
            StopCoroutine(_spawningCoroutine);
        }
        StopAllCoroutines(); 

        // Reset singletons
        if (EntityManager.Instance != null)
        {
            EntityManager.Instance.ResetState();
        }
        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.ResetState();
        }

        // Initialize game state
        availableTowerTiers = new List<Tier> { currentMaxTowerSpawnTier };
        mainTower = EntityManager.Instance.SpawnTower(Tier.X, new Vector3(0, 0, 0));
        
        // Start new coroutines
        StartCoroutine(GraduallyIncreaseTowerTierLimit());
        _spawningCoroutine = StartCoroutine(RandomlySpawnTowers());
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (mainTower == null && _spawningCoroutine != null)
        {
            StopCoroutine(_spawningCoroutine);
            _spawningCoroutine = null;
        }
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
