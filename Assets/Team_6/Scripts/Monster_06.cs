using System.Collections.Generic;
using UnityEngine;

public class Monster_06 : MonoBehaviour
{
    [SerializeField] GameObject[] monsterPrefab = null;
    public List<GameObject> monsterList = new List<GameObject>();
    Monster_Move_06 monsterMove;
    public int index = 0;
    int stage = 1;
    float spawnTime = 0f;
    float stageTimer = 0f;

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
        stageTimer += Time.deltaTime;
        spawnTime += Time.deltaTime;

        if (stageTimer >= 15f)
        {
            stage++;

            stageTimer = 0f;
        }

        if (stage == 1)
        {
            GameManager_06.Instance.stage = 1;
            if (spawnTime >= 2)
            {
                MonsterComing();
            }
        }

        if (stage == 2)
        {
            GameManager_06.Instance.stage = 2;
            
            if (spawnTime >= 1.5)
            {
                MonsterComing();
            }
        }

        if (stage == 3)
        {
            GameManager_06.Instance.stage = 3;
           
            if (spawnTime >= 1.15)
            {
                MonsterComing();
            }
        }

        if (stage == 4)
        {
            GameManager_06.Instance.stage = 4;
        
            if (spawnTime >= 1)
            {  
                MonsterComing();
            }
        }

        if (stage == 5)
        {
            GameManager_06.Instance.stage = 5;

            if (spawnTime >= 0.85)
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

    void MonsterMoveFaster(int addAmount)
    {
        for (int i = 0; i < monsterList.Count; i++)
        {
            monsterList[i].GetComponent<Monster_Move_06>().Speed += addAmount;
        }
    }
}
