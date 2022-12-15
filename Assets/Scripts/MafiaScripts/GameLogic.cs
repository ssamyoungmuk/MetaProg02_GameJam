using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MafiaGame
{ 
    public class GameLogic : Singleton<GameLogic>
    {
        [Header("GameUI")]
        [SerializeField] GameObject GameStartUI;
        [SerializeField] TextMeshProUGUI DayText;
        [SerializeField] TextMeshProUGUI debate_Text;
        [SerializeField] TextMeshProUGUI debateTime_Text;

        int day = 0;
        float time = 0;

        public void GameStart()
        {
            GameStartUI.SetActive(true);
            StartCoroutine(GameStart_Delay());
        }

        IEnumerator GameStart_Delay()
        {
            Fade(GameStartUI, fade.All);
            yield return new WaitForSeconds(2f);
            StartCoroutine(Day_Morning());
        }

        IEnumerator Day_Morning()
        {
            day++;
            DayText.text = day + DayText.text + " Morning";
            Fade(DayText.gameObject, fade.All);
            yield return new WaitForSeconds(2f);
            StartCoroutine(StartDebate());
        }

        IEnumerator StartDebate()
        {
            time = 180f;
            debateTime_Text.gameObject.SetActive(true);
            debateTime_Text.text = time.ToString();
            while (time >= 0)
            {
                time--;
                yield return new WaitForSeconds(1f);
                debateTime_Text.text = time.ToString();
            }
            debateTime_Text.gameObject.SetActive(false);

            debate_Text.text = "투표 시작";

            Fade(debate_Text.gameObject, fade.All);
            yield return new WaitForSeconds(2f);
            
            //StartCoroutine();

        }








        CanvasGroup canvasGroup;
        public void Fade(GameObject fadeIn, fade fd)
        {
            canvasGroup = fadeIn.GetComponent<CanvasGroup>();
            StartCoroutine(Fade_Delay(fadeIn, fd));
        }
        IEnumerator Fade_Delay(GameObject fadeIn, fade fd)
        {
            if (fd == fade.In || fd == fade.All)
            {
                canvasGroup.alpha = 0;
                fadeIn.SetActive(true);
                for (int i = 0; i < 100; i++)
                {
                    canvasGroup.alpha += 0.01f;
                    yield return new WaitForSeconds(0.01f);
                }
            }
            if (fd == fade.Out || fd == fade.All)
            {

                canvasGroup.alpha = 1;
                for (int i = 0; i < 100; i++)
                {
                    canvasGroup.alpha -= 0.01f;
                    yield return new WaitForSeconds(0.01f);
                }
                fadeIn.SetActive(false);
            }
        }

    }
}

