using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] GameObject monsterPrefab;
    [SerializeField] GameObject monsterPrefab2;
    public List<GameObject> monsterList = new List<GameObject>();
    public List<GameObject> monsterList2 = new List<GameObject>();
    public int index = 0;
    public int index2 = 0;

    float spawnTime = 0f;
    float spawnTime2 = 0f;


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

        for (int i = 0; i < 5; i++)
        {
            GameObject monster = Instantiate(monsterPrefab2);
            monster.transform.position = new Vector3(15, 0.5f, 0);
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;

            monster.gameObject.SetActive(false);
            monsterList2.Add(monster);
        }
    }

    

    void Update()
    {
        spawnTime += Time.deltaTime;
        spawnTime2 += Time.deltaTime;
       
        if (spawnTime >= 2)
        {
            MonsterComing();
        }
        if (spawnTime2 >= 3)
        {
            MonsterComing2();
        }

    }

    void MonsterComing()
    {
        if (index >= monsterList.Count) index = 0;
        monsterList[index].SetActive(true);

        index++;

        spawnTime = 0f;
    }

    void MonsterComing2()
    {
        if (index2 >= monsterList2.Count) index2 = 0;
        monsterList2[index2].SetActive(true);

        index2++;

        spawnTime2 = 0f;
    }
}
