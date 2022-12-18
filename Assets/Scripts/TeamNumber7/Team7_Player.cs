using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Team7_Player : MonoBehaviourPun
{
    [SerializeField]
    GameObject CandyAttack;

    [SerializeField]
    Slider myExpBar;

    [SerializeField]
    Animator myAnim;

    [SerializeField]
    GameObject myArmor;

    public bool isDead = false;
    private bool rollNow = false;

    float moveSpeed = 4f;
    float attackRotate = 0f;
    float mouseSpeed = 60f;

    float runSpeed = 8f;

    public int exp = 0;
    private int level = 1;

    Rigidbody rb = null;
    BoxCollider weapon = null;

    GameObject myCam;
    Team7_UIManager myUI;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        weapon = CandyAttack.gameObject.GetComponent<BoxCollider>();
    }

    void Start()
    {

        myUI = FindObjectOfType<Team7_UIManager>();
        CandyAttack.transform.rotation = Quaternion.Euler(0, 180, 0);
        weapon.enabled = false;
        Cursor.lockState = CursorLockMode.Locked; // 마우스 락
        //myCam = transform.Find("Main Camera").gameObject;

        if (!photonView.IsMine) gameObject.tag = "Team7_Other";
    }

    void Update()
    {
        if (photonView.IsMine && !isDead)
        {
            PlayerMove();
            MoveAnim();

            if (Input.GetMouseButtonDown(0))
            {
                photonView.RPC("AttackNow", RpcTarget.All);
            }

            if (Input.GetMouseButtonDown(1) && exp > 0 )
            {
                StartCoroutine(CO_Run());
            }

            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                photonView.RPC("AggroNow", RpcTarget.All);
            }


            else if (Input.GetKeyDown(KeyCode.Backspace)) // 게임 나가기
            {
                Cursor.lockState = CursorLockMode.None;
                PhotonNetwork.Disconnect();
                SceneManager.LoadScene("LobbyScene");
            }
        }
    }

    public void PlayerMove()
    {
        if (rollNow) return;
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        rb.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));
        rb.transform.Translate(move * Time.deltaTime * moveSpeed);
        //rb.MovePosition(transform.position + move * moveSpeed * Time.deltaTime);
    }

    public void MoveAnim()
    {
        if(Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        //if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            myAnim.SetBool("isWalk", true);
        }
        else
        {
            myAnim.SetBool("isWalk", false);
        }
    }

    [PunRPC]
    public void AttackNow()
    {
        StartCoroutine(PlayerAttack());
    }

    [PunRPC]
    public void AggroNow()
    {
        myAnim.SetTrigger("doAggro");
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
        isDead = true;
        photonView.RPC("DieAnim", RpcTarget.All);
    }

    [PunRPC]
    public void DieAnim()
    {
        myAnim.SetTrigger("doDie");
        StartCoroutine(CO_DieDelay());
    }


    IEnumerator CO_DieDelay()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        photonView.RPC("DieNow", RpcTarget.All);
    }

    [PunRPC]
    public void DieNow()
    {
        if (photonView.IsMine)
        {
            //myCam.transform.SetParent(null);
            
            PhotonNetwork.Destroy(gameObject);

            PhotonNetwork.Disconnect();
            Cursor.lockState = CursorLockMode.None; // 마우스 언락
            SceneManager.LoadScene("LobbyScene");
        }
        //Team7_GameManager.Inst.DieLog();

    }

    public void GetExp(int point)
    {
        exp += point;
        if (exp >= 100)
        {
            exp = exp - 100;
            LevelUp();
            
        }
        myExpBar.value = exp;
    }

    public void LevelUp()
    {
        level++;
        checkEvolve();
        transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
        CandyAttack.gameObject.transform.localScale += new Vector3(0, 0, 0.02f);
    }

    private void checkEvolve()
    {
        switch(level)
        {
            case 3:
                myArmor.transform.GetChild(1).gameObject.SetActive(true);
                myArmor.transform.GetChild(0).gameObject.SetActive(false);
                break;
            case 5:
                myArmor.transform.GetChild(2).gameObject.SetActive(true);
                myArmor.transform.GetChild(1).gameObject.SetActive(false);
                break;
            case 7:
                myArmor.transform.GetChild(3).gameObject.SetActive(true);
                myArmor.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 9:
                myArmor.transform.GetChild(4).gameObject.SetActive(true);
                myArmor.transform.GetChild(3).gameObject.SetActive(false);
                break;
            case 11:
                myArmor.transform.GetChild(5).gameObject.SetActive(true);
                myArmor.transform.GetChild(4).gameObject.SetActive(false);
                break;
            case 13:
                myArmor.transform.GetChild(6).gameObject.SetActive(true);
                myArmor.transform.GetChild(5).gameObject.SetActive(false);
                break;
            case 14:
                myArmor.transform.GetChild(7).gameObject.SetActive(true);
                myArmor.transform.GetChild(6).gameObject.SetActive(false);
                break;
            case 16:
                myArmor.transform.GetChild(8).gameObject.SetActive(true);
                myArmor.transform.GetChild(7).gameObject.SetActive(false);
                break;

        }
    }

    IEnumerator CO_Run() // 달리기 시작
    {
        while(exp > 0 && Input.GetMouseButton(1))
        {
            myAnim.SetBool("isRun", true);
            moveSpeed = runSpeed;
            yield return new WaitForSecondsRealtime(0.1f);
            exp -= 1;
            myExpBar.value = exp;
        }
        myAnim.SetBool("isRun", false);
        moveSpeed = 4f;
    }


    // 충돌 관련

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Team7_Wall"))
        {
            Team7_Die();
        }

        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}
