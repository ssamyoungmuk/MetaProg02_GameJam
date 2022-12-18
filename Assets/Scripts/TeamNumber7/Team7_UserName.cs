using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Team7_UserName : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nickName = null;
    // Start is called before the first frame update
    void SetName(string name)
    {
        nickName.text = name;
    }

    void Update()
    {
        // rotate canvas towards to camera
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
