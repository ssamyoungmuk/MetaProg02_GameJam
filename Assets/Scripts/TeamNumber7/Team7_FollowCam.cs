using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team7_FollowCam : MonoBehaviour
{
    private GameObject player;
    private Vector3 camPos;

    [Header("CamPos")]
    float camZ = 0.2f;
    float camY = 1.45f;

    [Header("CamRot")]
    [SerializeField] float rotX;
    [SerializeField] float rotY;
    [SerializeField] float rotZ;


    public void SetCam()
    {
        player = GameObject.FindWithTag("Team7_Me");
        gameObject.transform.SetParent(player.transform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localPosition += -Vector3.forward * camZ + Vector3.up * camY;
        gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

}
