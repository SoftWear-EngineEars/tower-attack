using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityManager : MonoBehaviour
{

    public static EntityManager Instance { get; private set; }

    private GameObject monsterPrefab;
    private GameObject towerPrefab;
    private string scriptableObjectFolder = "ScriptableObjects";
    
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

        monsterPrefab = Resources.Load<GameObject>("Prefabs/Monster");
        towerPrefab = Resources.Load<GameObject>("Prefabs/Tower");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject SpawnMonster(Tier tier, Vector3 position)
    {
        GameObject monster = Instantiate(monsterPrefab, position, Quaternion.identity);
        MonsterData monsterData = Resources.Load<MonsterData>($"{scriptableObjectFolder}/Monster{TierToInt(tier)}");
        monster.GetComponent<Monster>().Initialize(monsterData);
        return monster;
    }

    public GameObject SpawnTower(Tier tier, Vector3 position)
    {
        GameObject tower = Instantiate(towerPrefab, position, Quaternion.identity);
        TowerData towerData = Resources.Load<TowerData>($"{scriptableObjectFolder}/Tower{TierToInt(tier)}");
        tower.GetComponent<Tower>().Initialize(towerData);
        return tower;
    }

    public void DestroyTower(GameObject tower)
    {
        int goldValue = tower.GetComponent<Tower>().GetGoldValue();
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
