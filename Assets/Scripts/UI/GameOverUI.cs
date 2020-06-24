using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private Text gameOverText;


    [SerializeField]
    private Canvas gameOverCanvas;
    private const string SURVIVORS_WON_TEXT = "The survivors have escaped! ...";
    private const string MONSTER_WON_TEXT = "The monster has won! ...";

    public void OnSurvivorsEscapedStageEvent()
    {
        StartCoroutine(CountDown(SURVIVORS_WON_TEXT));
    }

    private IEnumerator CountDown(string gameOverString)
    {
        gameOverCanvas.enabled = true;

        for (var i = 10; i > 0; i--)
        {
            gameOverText.text = $"{gameOverString} {i}";
            yield return new WaitForSeconds(1);
        }

        gameOverCanvas.enabled = false;

        Stages.Load(StageName.Menu);
    }



    public void OnSurvivorsLost()
    {
        StartCoroutine(CountDown(MONSTER_WON_TEXT));
    }
 }
