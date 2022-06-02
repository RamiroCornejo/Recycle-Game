using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBehaviour : MonoBehaviour
{
    [SerializeField] DialogueUtility dialog;
    [SerializeField] GameObject[] rocaVOne = null;
    [SerializeField] GameObject[] rocaVTwo = null;
    [SerializeField] Transform root = null;

    void Start()
    {
        dialog.StartDialogue(false);
        DialogueUIDisplay.instance.SetEndDialogue(StartBehave);
    }

    public void StartBehave()
    {
        Character.TrackInput(false);
        UIManager.instance.FadeInAndOut(2, () => { Character.TrackInput(true); }, StartGame);
    }


    void StartGame()
    {
        for (int i = 0; i < rocaVOne.Length; i++)
            rocaVOne[i].SetActive(false);
        for (int i = 0; i < rocaVTwo.Length; i++)
            rocaVTwo[i].SetActive(true);
        root.eulerAngles = new Vector3(0, 180, 0);
        gameObject.SetActive(false);
    }
}
