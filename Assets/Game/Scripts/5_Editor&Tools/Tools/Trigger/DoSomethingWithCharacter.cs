using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DoSomethingWithCharacter : MonoBehaviour
{
    bool isEnter;
    public void CharacterEnter()
    {
        isEnter = true;
        OnCharacterEnter(Character.instance);
    }
    public void CharacterExit()
    {
        isEnter = false;
        OnCharacterExit(Character.instance);
    }

    private void Update()
    {
        if (isEnter) OnCharacterUpdate(Character.instance);
    }

    protected abstract void OnCharacterEnter(Character character);
    protected abstract void OnCharacterExit(Character character);
    protected abstract void OnCharacterUpdate(Character character);
}
