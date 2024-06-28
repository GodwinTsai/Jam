using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    public float SpawnInterval = 2;
    public float SpawnTimer = 0;

    public List<GameObject> MonsterPrefabPool;

    public List<Monster> Monsters = new List<Monster>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        return;
        SpawnTimer += Time.deltaTime;
        if (SpawnTimer > SpawnInterval)
        {
            SpawnTimer = 0;
            SpawnMonster();
        }
    }

    void SpawnMonster()
    {
        int index = Random.Range(0, MonsterPrefabPool.Count);
        GameObject obj = Instantiate(MonsterPrefabPool[index], transform);
        Monster script = obj.GetComponent<Monster>();
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = new Vector3(22f, 1.5f, 0);
        Monsters.Add(script);
    }
}