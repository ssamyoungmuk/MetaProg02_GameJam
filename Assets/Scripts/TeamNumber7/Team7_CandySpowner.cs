using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Team7_CandySpowner : MonoBehaviourPunCallbacks, IPunPrefabPool
{
    [SerializeField]
    GameObject[] candyPrefab;

    public void Destroy(GameObject gameObject)
    {
        throw new System.NotImplementedException();
    }

    // Inst Candy
    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        throw new System.NotImplementedException();
    }

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
