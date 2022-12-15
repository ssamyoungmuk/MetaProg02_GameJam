using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team7_Player : MonoBehaviour
{
    [SerializeField]
    GameObject CandyAttack;

    float moveSpeed = 20f;
    float attackRotate = 0f;

    Rigidbody rb = null;
    BoxCollider weapon = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        weapon = CandyAttack.gameObject.GetComponent<BoxCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CandyAttack.transform.rotation = Quaternion.Euler(0, 180, 0);
        weapon.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("코루틴 호출");
            StartCoroutine(PlayerAttack());
        }
    }

    public void PlayerMove()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        rb.MovePosition(transform.position + move * moveSpeed * Time.deltaTime);
    }

    IEnumerator PlayerAttack()
    {
        attackRotate = 0f;
        while (attackRotate > -180)
        {
            attackRotate -= 10f;
            Debug.Log(attackRotate);
            CandyAttack.transform.rotation = Quaternion.Euler(new Vector3(attackRotate, 180, 0));
            weapon.enabled = true;
            yield return new WaitForFixedUpdate();
        }
        CandyAttack.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        weapon.enabled = false;
        yield break;
    }
}
