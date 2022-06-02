using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ScriptedTutorial : MonoBehaviour
{
    [SerializeField] ElementData bottle = null;
    [SerializeField] int bottleQuant = 5;

    [SerializeField] MonoInvent_Base plasticChest = null;

    [SerializeField] DialogueUtility dialogue = null;

    [SerializeField] Collider cleaner = null;
    [SerializeField] Collider dryer = null;
    [SerializeField] Collider[] chests = new Collider[0];

    [SerializeField] string gameSceneName = "JUEGO";
    List<Func<bool>> funcs = new List<Func<bool>>();

    int index;

    private void Start()
    {
        funcs.Add(HaveBottleDirty);
        funcs.Add(HaveBottleWet);
        funcs.Add(HaveBottleDry);
        funcs.Add(HaveBottleSaveOnChest);

        StartCoroutine(WaitToComplete());
    }

    void Update()
    {
        if (index >= funcs.Count) return;

        if (funcs[index]())
        {
            index += 1;
            ChangeDialogueIndex();
            StartCoroutine(WaitToComplete());
        }
    }

    bool HaveBottleDirty()
    {
        return PlayerInventory.QueryElement(bottle, bottleQuant, 1, true, true, true);
    }

    bool HaveBottleWet()
    {
        return PlayerInventory.QueryElement(bottle, bottleQuant, 1, false, false, true);
    }

    bool HaveBottleDry()
    {
        return PlayerInventory.QueryElement(bottle, bottleQuant, 1, true, false, true);
    }

    bool HaveBottleSaveOnChest()
    {
        return plasticChest.QueryElement(bottle, bottleQuant, 1, true, false, true);
    }

    IEnumerator WaitToComplete()
    {
        yield return new WaitForSeconds(0.1f);
        dialogue.StartDialogue(Vector3.Distance(dialogue.transform.position, Character.instance.transform.position) < 3);
    }

    public void ChangeDialogueIndex()
    {
        dialogue.NextDialogueIndex();
    }

    public void FirstTutoComplete()
    {
        cleaner.enabled = true;
    }

    public void SecondTutoComplete()
    {
        dryer.enabled = true;
    }

    public void ThirdTutoComplete()
    {
        for (int i = 0; i < chests.Length; i++)
        {
            chests[i].enabled = true;
        }
    }

    public void EndTutorial()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
