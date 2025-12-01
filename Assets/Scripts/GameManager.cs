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

    private GameObject _mainTower;

    [SerializeField] private Tier towerSpawnTierLimit = Tier.III;
    [SerializeField] private Range towerTierIncreaseInterval = new(50f, 100f);
    [SerializeField] private Range towerSpawnInterval = new(20f, 40f);
    [SerializeField] private Range xSpawnRange = new(-7f, 7f);
    [SerializeField] private Range ySpawnRange = new(-3f, 2.5f);

    private const Tier LowestTowerTier = Tier.I;

    private Tier _currentMaxTowerSpawnTier;
    private List<Tier> _availableTowerTiers;

    private Coroutine _spawningCoroutine;

    private void InitializeGame()
    {
        _currentMaxTowerSpawnTier = LowestTowerTier;
        _availableTowerTiers = new List<Tier> { _currentMaxTowerSpawnTier };
        _mainTower = EntityManager.Instance.SpawnTower(Tier.X, new Vector3(0, 0, 0));
        StartCoroutine(GraduallyIncreaseTowerTierLimit());
        _spawningCoroutine = StartCoroutine(RandomlySpawnTowers());
    }
    



    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // making sure the OnSceneLoaded veriable works with Unity
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("MainScene"))
        {
            Debug.Log("Scene loaded: " + scene.name + " in mode: " + mode);

            InitializeGame(); // re-initialize game
            ResourceManager.Instance.ResetState(); // reset gold
        }
    }




    // Update is called once per frame
    void Update()
    {
        if (_mainTower == null && _spawningCoroutine != null)
        {
            StopCoroutine(_spawningCoroutine);
            _spawningCoroutine = null;
        }
    }

    private IEnumerator GraduallyIncreaseTowerTierLimit()
    {
        while (_currentMaxTowerSpawnTier < towerSpawnTierLimit)
        {
            var randomInterval = Random.Range(towerTierIncreaseInterval.Min, towerTierIncreaseInterval.Max);
            yield return new WaitForSeconds(randomInterval);

            _currentMaxTowerSpawnTier = EntityManager.IncrementTier(_currentMaxTowerSpawnTier);
            _availableTowerTiers.Add(_currentMaxTowerSpawnTier);
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
