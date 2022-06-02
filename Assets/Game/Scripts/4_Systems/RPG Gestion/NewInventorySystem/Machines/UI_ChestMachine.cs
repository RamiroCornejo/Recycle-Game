using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_ChestMachine : MonoBehaviour
{
    public static UI_ChestMachine instance;
    private void Awake() => instance = this;

    [SerializeField] UI_ContainerManager myUI;
    public CanvasGroup myGroup;

    public CanvasGroup myGroupProgress;
    public CanvasGroup myGroupCounter;
    public Image ProgressCounter;
    public TextMeshProUGUI counterNameTask;
    public TextMeshProUGUI countervalue;
    public TextMeshProUGUI description;

    [SerializeField] Image progressbar;
    [SerializeField] Image element_processed;
    [SerializeField] TextMeshProUGUI taskvalue;

    public void EnableProgressBar(bool val) => myGroupProgress.alpha = val ? 1 : 0;
    public void ProgressProcess(float lerp) => progressbar.fillAmount = lerp;
    public void UpdateElementObject(Sprite photo) => element_processed.sprite = photo;
    public void TaskValue(string val) => taskvalue.text = val;
    public void SetDescription(string val)
    {
        description.text = val;
    }

    public void Redraw(MonoInvet_Handler handler, Container container, bool redraw = true)
    {
        myUI.Build(handler, container, redraw);
    }

    public void Refresh(bool redraw = false)
    {
        myUI.Refresh(redraw);
    }

    public void OpenChestMachine(string storagename, Action OnFinishFade)
    {
       // Debug.Log("OpenMachine");
        myGroup.alpha = 1;
        myGroup.blocksRaycasts = true;
        myGroup.interactable = true;
        myUI.SetName(storagename);
        myUI.OpenUIs(OnFinishFade);
    }
    public void CloseChestMachine(Action OnFinishFade)
    {
       // Debug.Log("CloseMachine");
        myGroup.alpha = 0;
        myGroup.blocksRaycasts = false;
        myGroup.interactable = false;
        myUI.CloseUIs(OnFinishFade);
    }

    public void OpenCounter()
    {
        myGroupCounter.alpha = 1;
    }
    public void CloseCounter()
    {
        myGroupCounter.alpha = 0;
    }
    public void SetCounter(int current, int max, float progress)
    {
        ProgressCounter.fillAmount = progress;
        countervalue.text = current + "/" + max;
    }
    public void SetCounterNameTask(string nametask)
    {
        counterNameTask.text = nametask;
    }

    public void OnElementsEnter()
    {
        

    }
}
