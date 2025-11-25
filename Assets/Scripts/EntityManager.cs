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
        // Example of spawning a monster and a tower
        SpawnMonster(Tier.IV, new Vector3(0, 0, 0));
        SpawnTower(Tier.X, new Vector3(2, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnMonster(Tier tier, Vector3 position)
    {
        GameObject monster = Instantiate(monsterPrefab, position, Quaternion.identity);
        MonsterData monsterData = Resources.Load<MonsterData>($"{scriptableObjectFolder}/Monster{TierToInt(tier)}");
        monster.GetComponent<Monster>().Initialize(monsterData);
    }

    public void SpawnTower(Tier tier, Vector3 position)
    {
        GameObject tower = Instantiate(towerPrefab, position, Quaternion.identity);
        TowerData towerData = Resources.Load<TowerData>($"{scriptableObjectFolder}/Tower{TierToInt(tier)}");
        tower.GetComponent<Tower>().Initialize(towerData);
    }

    public static int TierToInt(Tier tier)
    {
        return ((int) tier) + 1;
    }
}
