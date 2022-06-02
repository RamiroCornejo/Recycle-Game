using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEffectUpdater : MonoBehaviour
{
    public bool on = true;
    bool shutDown;

    public TextEffect[] myeffects = new TextEffect[0];
    TMP_Text myText;
    List<Word> words = new List<Word>();

    private void Start()
    {
        myText = GetComponent<TMP_Text>();
        SeparateWords();
    }

    void SeparateWords()
    {
        Word firstWord = new Word();
        firstWord.index = 0;
        words.Add(firstWord);

        string s = myText.text;
        for (int index = s.IndexOf(' '); index > -1; index = s.IndexOf(' ', index + 1))
        {
            Word word = new Word();
            word.index = words[words.Count - 1].index;
            word.length = index - words[words.Count - 1].index;
            words[words.Count - 1] = word;

            Word newWord = new Word();
            newWord.index = index + 1;
            words.Add(newWord);
        }
        Word lastWord = new Word();
        lastWord.index = words[words.Count - 1].index;
        lastWord.length = s.Length - words[words.Count - 1].index;
        words[words.Count - 1] = lastWord;

        StartCoroutine(StartCor());
    }

    IEnumerator StartCor()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < myeffects.Length; i++)
        {
            myeffects[i].StartEffect(new List<Word>() { words[0], words[1], words[2], words[6], }, myText.mesh);
        }
    }

    void Update()
    {
        if (!on)
        {
            if (!shutDown)
            {
                myText.ForceMeshUpdate();
                var meshReset = myText.mesh;
                for (int i = 0; i < myeffects.Length; i++)
                {
                    myeffects[i].ShutDown(meshReset);
                }
                myText.canvasRenderer.SetMesh(meshReset);
                shutDown = true;
            }
            return;
        }
        else
        {
            if (shutDown)
            {
                for (int i = 0; i < myeffects.Length; i++)
                {
                    myeffects[i].StartEffect(new List<Word>() { words[0], words[1], words[2], words[6], }, myText.mesh);
                }
                shutDown = false;
            }
        }
        myText.ForceMeshUpdate();
        var mesh = myText.mesh;

        for (int i = 0; i < myeffects.Length; i++)
        {
            myeffects[i].UpdateEffect(mesh, myText);
        }

        myText.canvasRenderer.SetMesh(mesh);
    }
}
