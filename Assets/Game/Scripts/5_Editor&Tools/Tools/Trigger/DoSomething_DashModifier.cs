using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoSomething_DashModifier : DoSomethingWithCharacter
{
    float timer = 0;
    public float time_to_cancel = 5;
    bool animate = false;

    public float dash_multiplier = 3;

    protected override void OnCharacterEnter(Character character)
    {
        animate = true;
        timer = 0;
    }
    protected override void OnCharacterExit(Character character) { }
    protected override void OnCharacterUpdate(Character character) { }

    private void Update()
    {
        if (animate)
        {
            if (timer < time_to_cancel)
            {
                timer = timer + 1 * Time.deltaTime;
            }
            else
            {
                timer = 0;
                animate = false;
            }
        }
        
    }
}
