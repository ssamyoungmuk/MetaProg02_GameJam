using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Team7_Player : MonoBehaviourPun
{
    [SerializeField]
    GameObject CandyAttack;

    float moveSpeed = 4f;
    float attackRotate = 0f;
    float mouseSpeed = 60f;

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
        Cursor.lockState = CursorLockMode.Locked; // 마우스 락
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            PlayerMove();

            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(PlayerAttack());
            }
        }
    }

    public void PlayerMove()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        rb.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));
        rb.transform.Translate(move * Time.deltaTime * moveSpeed);
        //rb.MovePosition(transform.position + move * moveSpeed * Time.deltaTime);
    }

    [PunRPC]
    IEnumerator PlayerAttack()
    {
        attackRotate = 180f;
        while (attackRotate < 360)
        {
            attackRotate += 10f;
            CandyAttack.transform.localRotation = Quaternion.Euler(new Vector3(attackRotate, rb.rotation.y, 0));
            //CandyAttack.transform.Rotate(new Vector3(attackRotate,0,0), Space.Self);

            weapon.enabled = true;
            yield return new WaitForFixedUpdate();
        }
        CandyAttack.transform.localRotation = Quaternion.Euler(new Vector3(180, rb.rotation.y, 0));
        weapon.enabled = false;
        yield break;
    }
}
