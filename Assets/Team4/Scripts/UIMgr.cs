using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PicoPark
{
    public class UIMgr : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI[] peopleCount;

        GameMgr gameMgr;
        [SerializeField] float peopleNum;

        public void CountPeopleNum() { peopleNum += 1; peopleCount[0].text = "현재인원 : " + peopleNum + " / 4"; }
        public void MinusPeopleNum() { peopleNum -= 1; peopleCount[0].text = "현재인원 : " + peopleNum + " / 4"; }

    }
       
}
