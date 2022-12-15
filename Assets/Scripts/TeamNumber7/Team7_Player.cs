using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Team7_Player : MonoBehaviourPun
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

    void Start()
    {
        CandyAttack.transform.rotation = Quaternion.Euler(0, 180, 0);
        weapon.enabled = false;
    }

    void Update()
    {
<<<<<<< HEAD
        if(photonView.IsMine)
=======
        if (photonView.IsMine)
>>>>>>> Team_7_
        {
            PlayerMove();

            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(PlayerAttack());
            }
        }
<<<<<<< HEAD
        
=======

>>>>>>> Team_7_
    }

    public void PlayerMove()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        rb.MovePosition(transform.position + move * moveSpeed * Time.deltaTime);
    }

    [PunRPC]
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
