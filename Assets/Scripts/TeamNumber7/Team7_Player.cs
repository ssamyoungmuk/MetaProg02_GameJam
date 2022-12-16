using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Team7_Player : MonoBehaviourPun
{
    [SerializeField]
    GameObject CandyAttack;

    float moveSpeed = 4f;
    float attackRotate = 0f;
    float mouseSpeed = 60f;

    [SerializeField] public int exp = 0;

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

    public void Team7_Die()
    {
        Debug.Log("독립 함수 실행");
        photonView.RPC("DieNow", RpcTarget.All);
    }

    [PunRPC]
    public void DieNow()
    {
        if (photonView.IsMine)
        {
            Debug.Log("RPC 실행");
            PhotonNetwork.Destroy(gameObject);
            PhotonNetwork.Disconnect();
            Cursor.lockState = CursorLockMode.None; // 마우스 언락

            Debug.Log("씬 이동");
            SceneManager.LoadScene("LobbyScene");
        }
    }


    public void GetExp(int point)
    {
        exp += point;
        if(exp >= 100)
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            exp = 0;
        }
    }
}
