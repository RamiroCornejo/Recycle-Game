using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PingPongExample : MonoBehaviour
{
    List<PingPongLerp> anims = new List<PingPongLerp>();

    [SerializeField] RectTransform parent;
    [SerializeField] Ui_PingPongDebugger model;

    void Start()
    {
        anims.Add(Create("Common No Params").Play());
        anims.Add(Create("Fast Flip Flop").Configure_TEMPLATE_FLIP_FLOP(0.5f).Play());
        anims.Add(Create("Only Flip").Configure_TEMPLATE_ONLY_FLIP(0.5f).Play());
        anims.Add(Create("Only Flop").Configure_TEMPLATE_ONLY_FLOP(0.5f).Play());
        anims.Add(Create("OneShot").Configure_TEMPLATE_ONESHOT().Play());
        anims.Add(Create("OneShot Explosion").Configure_TEMPLATE_ONESHOT_EXPLOSION(0.1f, 10f).Play());
        anims.Add(Create("Loop Explosion").Configure_TEMPLATE_LOOP_EXPLOSION(0.1f, 1f).Play());
        anims.Add(Create("Loop CD timer").Configure_TEMPLATE_LOOP_CD_TIMER(1f).Play());
    }

    PingPongLerp Create(string name)
    {
        var ui = Instantiate(model, parent);
        ui.Configure(name);

        PingPongLerp elem = new PingPongLerp();
        elem = elem
            .Configure_Callback(ui.LerpValue)
            .ConfigureCallbacksAuxiliars(ui.ExecutePing, ui.ExecutePong);

        return elem;
    }

    void Update()
    {
        foreach (var anim in anims)
        {
            anim.Tick(Time.deltaTime);
        }
    }
}
