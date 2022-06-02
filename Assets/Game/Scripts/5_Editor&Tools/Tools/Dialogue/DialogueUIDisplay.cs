using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class DialogueUIDisplay : MonoBehaviour
{
    public static DialogueUIDisplay instance { get; private set; }

    [SerializeField] TextMeshProUGUI[] titleText = new TextMeshProUGUI[0];
    [SerializeField] GameObject titleBackground = null;
    [SerializeField] TextMeshProUGUI mainText = null;
    //[SerializeField] Image[] spritesImg = new Image[0];

    public bool IsOpen { get => isOpen; private set { isOpen = value; } }

    [SerializeField] GameObject dialogHUD = null;
    [SerializeField] AnswerButton[] answersUI = new AnswerButton[0];
    [SerializeField] GameObject presToNext = null;
    bool isOpen;
    bool inAnimation = false;

    Action<int> next;

    Action OnEndDialogue;

    Action DoWhenEndDelay;

    float delayTime;
    bool onDelay;
    float timer;
    bool inDialog;
    bool onAnswers;

    [SerializeField] float animSpeed = 0.01f;
    string originalText;
    char[] charPerChar;

    private void Awake()
    {
        instance = this;
    }

    public void SetEndDialogue(Action _OnEndDialogue)
    {
        if (!isOpen) return;

        OnEndDialogue += _OnEndDialogue;
    }

    public void ShowDialog(DialogParameters parameters, Action<int> nextIndex)
    {
        StopAllCoroutines();
        originalText = parameters.mainText;
        mainText.text = "";
        charPerChar = originalText.ToCharArray();
        inAnimation = true;
        StartCoroutine(TextAnimation());
        SoundFX.Play_Dialoge();

        //if (parameters.mySprite == null)
        //{
        //    for (int i = 0; i < spritesImg.Length; i++)
        //        spritesImg[i].gameObject.SetActive(false);
        //}
        //else
        //{
        //    for (int i = 0; i < spritesImg.Length; i++)
        //    {
        //        if (parameters.spriteDir == i)
        //        {
        //            spritesImg[i].gameObject.SetActive(true);
        //            spritesImg[i].sprite = parameters.mySprite;
        //        }
        //        else
        //            spritesImg[i].gameObject.SetActive(false);
        //    }
        //}
        next = nextIndex;
        inDialog = true;
        
        if(parameters.titleText == "")
        {
            titleBackground.SetActive(false);
        }
        else
        {
            titleBackground.SetActive(true);
            titleText[0].text = parameters.titleText;
        }
    }

    IEnumerator TextAnimation()
    {
        for (int i = 0; i < charPerChar.Length; i++)
        {
            mainText.text += charPerChar[i];

            yield return new WaitForSeconds(animSpeed);
        }

        EndAnimation();
    }

    IEnumerator RefreshButtons()
    {
        yield return new WaitForEndOfFrame();

        Canvas.ForceUpdateCanvases();
        for (int i = 0; i < answersUI.Length; i++)
        {
            if (!answersUI[i].gameObject.activeSelf) continue;
            //answersUI[i].transform.GetComponent<ContentSizeFitter>().enabled = false;
            //answersUI[i].transform.GetComponent<ContentSizeFitter>().enabled = true;
        }
    }

    public void ForceEndDelay()
    {
        if (onDelay)
            EndDelay();
    }

    void EndAnimation()
    {
        SoundFX.Stop_Dialogue_TalkLoop();
        if (!answersUI[0].gameObject.activeSelf) presToNext.SetActive(true);
        inAnimation = false;
        mainText.text = originalText;
    }

    public void DisplayDelay(float _delayTime, Action _OnEndDelay)
    {
        delayTime = _delayTime;
        DoWhenEndDelay = _OnEndDelay;
        onDelay = true;
        timer = 0;
    }

    public void ShowAnswers(List<string> answers)
    {

        for (int i = 0; i < answersUI.Length; i++)
        {
            answersUI[i].gameObject.SetActive(true);
            answersUI[i].SetAnswer(answers[i], SelectAnswer, i);
        }

        onAnswers = true;
        StartCoroutine(RefreshButtons());
    }

    void SelectAnswer(int index)
    {
        for (int i = 0; i < answersUI.Length; i++)
        {
            answersUI[i].gameObject.SetActive(false);
        }

        onAnswers = false;
        next(index);
    }

    public bool OpenDialog(bool track)
    {
        if (isOpen) return false;
        OnEndDialogue = null;
        isOpen = true;
        dialogHUD.SetActive(true);
        titleText[0].text = "";
        mainText.text = "";
        if(track)Character.TrackInput(false);
        //PauseManager.instance.Pause();
        return true;
    }

    public void CloseDialog(bool track)
    {
        if (track) Character.TrackInput(true);
        isOpen = false;
        dialogHUD.SetActive(false);
        OnEndDialogue?.Invoke();
        //PauseManager.instance.Resume();
    }

    void EndDelay()
    {
        timer = 0;
        onDelay = false;
        DoWhenEndDelay.Invoke();
    }

    private void Update()
    {
        if (onAnswers) return;

        if (onDelay)
        {
            timer += Time.deltaTime;

            if(timer >= delayTime)
            {
                EndDelay();
            }

            return;
        }

        if (!isOpen) return;

        if (Input.GetButtonDown("Submit"))
        {
            if (!inDialog) return;

            if (inAnimation)
            {
                StopAllCoroutines();
                EndAnimation();
                return;
            }

            inDialog = false;
            next?.Invoke(-1);
            presToNext.SetActive(false);
        }
    }
}
