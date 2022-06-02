using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class TextEffect : MonoBehaviour
{
    protected List<Word> myWords = new List<Word>();
    protected bool active;

    public abstract void UpdateEffect(Mesh mesh, TMP_Text text);

    public abstract void ShutDown(Mesh mesh);

    public virtual void StartEffect(List<Word> words, Mesh mesh)
    {
        if (active) return;
        OnStartEffect(mesh);
        myWords.Clear();

        for (int i = 0; i < words.Count; i++)
        {
            myWords.Add(words[i]);
        }
        active = true;
    }

    public abstract void OnStartEffect(Mesh mesh);
}

public struct Word
{
    public int index;
    public int length;
}
