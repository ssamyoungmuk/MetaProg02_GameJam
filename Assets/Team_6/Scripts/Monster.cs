using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] GameObject[] monsterPrefab;
    public List<GameObject> monsterList = new List<GameObject>();
    public int index = 0;
    public int index2 = 0;

    float spawnTime = 0f;
    float stageTime = 0f;

    int stage = 1;


    private void Awake()
    {
        int randNum = 0;
        for (int i = 0; i < 5; i++)
        {
            randNum = Random.Range(0, 2);
            GameObject monster = Instantiate(monsterPrefab[randNum]);
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
        stageTime += Time.deltaTime;

        if (stage == 1)
        {
            if (spawnTime >= 2)
            {
                MonsterComing();

                if (stageTime >= 10)
                {
                    stage++;
                }
            }
        }

        if (stage == 2)
        {

            if (spawnTime >= 1.5f)
            {
                MonsterComing();
            }
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
