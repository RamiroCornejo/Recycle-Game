using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    
    [SerializeField] ItemAddUI itemUI = null;
    [SerializeField] float itemOffset = 1;
    [SerializeField] Transform parentCanvas = null;

    [SerializeField] GameObject gloves = null;
    [SerializeField] GameObject shovel = null;
    [SerializeField] UIMesseage uiMessage = null;

    [SerializeField] GameObject uiBoard = null;

    [SerializeField] UIFade fade = null;
    [SerializeField] bool startWithFade = true;
    bool isSpawned;

    Transform character;
    Queue<ItemValues> queue = new Queue<ItemValues>();

    Action OnBoardClose;

    float timer;

    private void Awake()
    {
        instance = this;
        fade.FadeOut(() => { });
    }

    private void Start()
    {
        character = Character.instance.transform;
    }


    private void LateUpdate()
    {
        timer += Time.deltaTime;

        if (timer < 0.3) return;

        isSpawned = false;
        timer = 0;
        if (queue.Count > 0)
        {
            AddItem(queue.Dequeue());
        }
    } 


    public void AddItem(ItemValues values)
    {
        if (isSpawned)
        {
            queue.Enqueue(values);
        }
        else
        {
            var item = Instantiate(itemUI, Camera.main.WorldToScreenPoint(new Vector3(character.position.x, character.position.y + itemOffset, character.position.z)), Quaternion.identity,parentCanvas);
            item.SetValues(values);
            isSpawned = true;
        }
    }

    public void EquipTool(CharacterTools tool)
    {
        if (tool == CharacterTools.Gloves)
            gloves.SetActive(true);
        else if (tool == CharacterTools.Shovel)
            shovel.SetActive(true);

    }

    public void SendText(string mess)
    {
        uiMessage.SendMesseage(mess);
    }

    public void FadeInAndOut(float timeToFadeOut, Action OnFadeOutOver, Action OnFadeInOver)
    {
        fade.FadeIn(() => StartCoroutine(CounterToFadeOut(timeToFadeOut, OnFadeOutOver, OnFadeInOver)));
    }

    IEnumerator CounterToFadeOut(float timeToFadeOut, Action OnFadeOutOver, Action OnFadeInOver)
    {
        OnFadeInOver?.Invoke();
        yield return new WaitForSeconds(timeToFadeOut);
        fade.FadeOut(OnFadeOutOver);
    }

    public void OpenBoard(Action _OnBoardClose)
    {
        OnBoardClose = _OnBoardClose;
        Character.TrackInput(false);
        uiBoard.SetActive(true);
    }

    public void CloseBoard()
    {
        if (DialogueUIDisplay.instance.IsOpen) return;

        OnBoardClose?.Invoke();
        Character.TrackInput(true);
        uiBoard.SetActive(false);
    }
}

public struct ItemValues
{
    public Sprite itemSprite;
    public bool isDry;
    public bool isDirty;

    public ItemValues(Sprite itemSprite, bool isDry, bool isDirty)
    {
        this.itemSprite = itemSprite;
        this.isDry = isDry;
        this.isDirty = isDirty;
    }
}
