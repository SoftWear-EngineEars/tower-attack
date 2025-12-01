using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityManager : MonoBehaviour
{

    public static EntityManager Instance { get; private set; }

    private GameObject _monsterPrefab;
    private GameObject _towerPrefab;
    private const string ScriptableObjectFolder = "ScriptableObjects";

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

        _monsterPrefab = Resources.Load<GameObject>("Prefabs/Monster");
        _towerPrefab = Resources.Load<GameObject>("Prefabs/Tower");
    }

    public GameObject SpawnMonster(Tier tier, Vector3 position)
    {
        var monster = Instantiate(_monsterPrefab, position, Quaternion.identity);
        var monsterData = Resources.Load<MonsterData>($"{ScriptableObjectFolder}/Monster{TierToInt(tier)}");
        monster.GetComponent<Monster>().Initialize(monsterData);
        return monster;
    }

    public GameObject SpawnTower(Tier tier, Vector3 position)
    {
        var tower = Instantiate(_towerPrefab, position, Quaternion.identity);
        var towerData = Resources.Load<TowerData>($"{ScriptableObjectFolder}/Tower{TierToInt(tier)}");
        tower.GetComponent<Tower>().Initialize(towerData);
        return tower;
    }

    public void DestroyTower(GameObject tower)
    {
        var goldValue = tower.GetComponent<Tower>().GetGoldValue();
        ResourceManager.Instance.GainGold(goldValue);
        // Optional: Add nice death effect with coroutine
        Destroy(tower);
    }
    
    public void ResetState() // If you win the game and want to go back you gotta reset the state of the entities
    {
        foreach (var tower in GameObject.FindGameObjectsWithTag("Tower"))
        {
            if (tower != null)
            {
                Destroy(tower);
            }
        }

        foreach (var monster in GameObject.FindGameObjectsWithTag("Monster"))
        {
            if (monster != null)
            {
                Destroy(monster);
            }
        }
    }

    public static int TierToInt(Tier tier)
    {
        return ((int)tier) + 1;
    }

    public static Tier IncrementTier(Tier tier)
    {
        return (Tier)((int)tier + 1);
    }
}
