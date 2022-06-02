using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    ItemRecolectablePool itemPool;
    [SerializeField] ItemRecolectable baseItem = null;
    [SerializeField] int questRequired = 6;

    [SerializeField] Collider interactCollider = null;

    int currentQuestCompleted = 0;


    private void Awake()
    {
        instance = this;

        itemPool = new ItemRecolectablePool(baseItem, transform);

    }

    public void QuestComplete()
    {
        currentQuestCompleted += 1;

        if (currentQuestCompleted >= questRequired)
            AllRequestCompleted();

    }

    public void AllRequestCompleted()
    {
        interactCollider.enabled = true;
    }

    public void WinScreen()
    {
        UIManager.instance.FadeInAndOut(1, () => { }, () => { SceneManager.LoadScene(3); });
        Character.TrackInput(true);
    }

    public void ReturnToMenu()
    {
        UIManager.instance.FadeInAndOut(1, () => { }, () => { SceneManager.LoadScene(0); });
        Character.TrackInput(true);
    }

    public ItemRecolectable GetItem(Vector3 spawnPos) => itemPool.SpawnItem(spawnPos);

    public void ReturnItem(ItemRecolectable item) => itemPool.ReturnItem(item);
}
