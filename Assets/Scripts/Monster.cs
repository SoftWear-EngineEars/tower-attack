using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterData monsterData;
    [SerializeField] private float towerCheckRate = 0.75f; // tentative; we might want it to check slower if more lag or faster if it gets annoying

    private SpriteRenderer _spriteRenderer;

    private Vector2 _targetPosition;
    
    void Start()
    {
        StartCoroutine(FindTarget());
    }

    
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _targetPosition, monsterData.speed * Time.deltaTime);
    }

    public void Initialize(MonsterData data)
    {
        monsterData = data;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = monsterData.LoadSprite();
    }

    IEnumerator FindTarget()
    {
        while (true)
        {
            var towers = GameObject.FindGameObjectsWithTag("Tower");

            _targetPosition = towers[0].transform.position;
        
            // find the closest tower; make _targetPosition that position
            for (var i = 1; i < towers.Length; i++)
            {
                if (Vector3.Distance(towers[i].transform.position, transform.position) <
                    Vector3.Distance(_targetPosition, transform.position))
                {
                    _targetPosition = towers[i].transform.position;
                }
            }

            yield return new WaitForSeconds(towerCheckRate); 
        }
    }
}
