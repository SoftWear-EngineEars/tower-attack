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
        GameObject monster = Instantiate(_monsterPrefab, position, Quaternion.identity);
        MonsterData monsterData = Resources.Load<MonsterData>($"{ScriptableObjectFolder}/Monster{TierToInt(tier)}");
        monster.GetComponent<Monster>().Initialize(monsterData);
        return monster;
    }

    public GameObject SpawnTower(Tier tier, Vector3 position)
    {
        GameObject tower = Instantiate(_towerPrefab, position, Quaternion.identity);
        TowerData towerData = Resources.Load<TowerData>($"{ScriptableObjectFolder}/Tower{TierToInt(tier)}");
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

    public static int TierToInt(Tier tier)
    {
        return ((int)tier) + 1;
    }

    public static Tier IncrementTier(Tier tier)
    {
        return (Tier)((int)tier + 1);
    }
}
