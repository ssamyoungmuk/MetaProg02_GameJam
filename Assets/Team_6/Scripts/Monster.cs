using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] GameObject monsterPrefab;
    public List<GameObject> monsterList = new List<GameObject>();
    public int index = 0;

    float posX = 15;
    float posY = 0.5f;

    float spawnTime = 0f;


    private void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject monster = Instantiate(monsterPrefab);
            monster.transform.position = new Vector3(15, 0.5f, 0);
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;
            
            monster.gameObject.SetActive(false);
            monsterList.Add(monster);
        }
    }

    

    void Update()
    {
        spawnTime += Time.deltaTime;
        if (spawnTime >= 2)
        {
            MonsterComing();
        }
        
    }

    void MonsterComing()
    {
        if (index >= monsterList.Count) index = 0;
        monsterList[index].SetActive(true);

        index++;

        spawnTime = 0f;
    }
}
