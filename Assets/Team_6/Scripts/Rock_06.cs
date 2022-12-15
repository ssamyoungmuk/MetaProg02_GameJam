using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_06 : MonoBehaviour
{
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] Transform target = null;
    [SerializeField] Transform startPos = null;
    [SerializeField] Transform dummyPos = null;
    [SerializeField] List<GameObject> enableMonster = new List<GameObject>(5);
    Monster_06 monster = null;

    WaitForFixedUpdate time = new WaitForFixedUpdate();
    float duration = 0f;

    private void Awake()
    {
        monster = FindObjectOfType<Monster_06>();
        transform.position = startPos.position;
        StartCoroutine(FindEnableMonster());
    }

    public void Fire()
    {
        StartCoroutine(Move());
    }

    IEnumerator FindEnableMonster()
    {
        while (true)
        {
            if ((duration += Time.deltaTime) < 1.0f)
            {
                continue;
            }
            else
            {
                duration = 0f;
                for (int i = 0; i < monster.monsterList.Count; i++)
                {
                    if (monster.monsterList[i].gameObject.activeSelf == true && !enableMonster.Contains(monster.monsterList[i]))
                    {
                        enableMonster.Add(monster.monsterList[i]);
                    }
                    else if(monster.monsterList[i].gameObject.activeSelf == false && enableMonster.Contains(monster.monsterList[i]))
                    {
                        enableMonster.Remove(monster.monsterList[i]);
                    }
                }
                if (enableMonster.Count == 0)
                    target = dummyPos;
                else
                    target = enableMonster[0].transform;

                yield return time;
            }
        }
    }

    IEnumerator Move()
    {
        while (true)
        {
            if(Vector3.Distance(transform.position, target.position) <= 2f)
            {
                transform.position = startPos.position;
                target.SendMessage("Hit", SendMessageOptions.DontRequireReceiver);
                yield break;
            }
            transform.position = Vector3.Slerp(gameObject.transform.position, target.transform.position + Vector3.left, moveSpeed);
            yield return time;
        }
    }
}
