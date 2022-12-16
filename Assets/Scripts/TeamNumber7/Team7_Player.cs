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

    public int exp;

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
        Cursor.lockState = CursorLockMode.Locked; // ¸¶¿ì½º ¶ô

        if (!photonView.IsMine) gameObject.tag = "Team7_Other";
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            PlayerMove();

            if (Input.GetMouseButtonDown(0))
            {
                photonView.RPC("AttackNow", RpcTarget.All);
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
    public void AttackNow()
    {
        StartCoroutine(PlayerAttack());
    }

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

    [PunRPC]
    public void Team7_Die()
    {
        PhotonNetwork.Destroy(this.gameObject);

        QuitRoom();
    }

    private void QuitRoom()
    {
        Debug.Log("Á×¾úÀ¸´Ï ¹æ¿¡¼­ ÅðÀå");
        PhotonNetwork.LeaveRoom();
        Debug.Log("¿¬°áµµ ²÷°í");
        PhotonNetwork.Disconnect();
        Debug.Log("¾À ÀÌµ¿");
        PhotonNetwork.LoadLevel("LobbyScene");
    }
}
