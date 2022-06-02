using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_TP : MonoBehaviour
{
    [SerializeField] GameObject npcModel = null;
    [SerializeField] GameObject pointToTP = null;
    [SerializeField] Animator anim = null;
    [SerializeField] string rewardAnimName = "";

    public void StartTP()
    {
        UIManager.instance.FadeInAndOut(1, ReturnToGame, TP);
        StartCoroutine(TrackInputAgain());
    }

    IEnumerator TrackInputAgain()
    {
        yield return new WaitForEndOfFrame();
        Character.TrackInput(false);
    }

    void TP()
    {
        npcModel.transform.position = pointToTP.transform.position;
        anim.Play(rewardAnimName);
    }

    void ReturnToGame()
    {
        Character.TrackInput(true);
        GameManager.instance.QuestComplete();
    }
}
