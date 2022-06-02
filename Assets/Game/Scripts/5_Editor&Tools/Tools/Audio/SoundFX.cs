using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;
using Tools.Extensions;

public class SoundFX : MonoBehaviour
{
    public static SoundFX instance;
    private void Awake() => instance = this;

    [SerializeField] AudioClip item_pickup;
    [SerializeField] AudioClip inventory_over;
    [SerializeField] AudioClip inventory_select;
    [SerializeField] AudioClip inventory_switch_item;

    [SerializeField] AudioClip menu_sello;
    [SerializeField] AudioClip menu_whoosh_characters_Enter;
    [SerializeField] AudioClip menu_whoosh_characters_Exit;
    [SerializeField] AudioClip menu_transition_enter;
    [SerializeField] AudioClip menu_transition_exit;
    [SerializeField] AudioClip menu_tentacle_idle_go;
    [SerializeField] AudioClip menu_tentacle_idle_back;
    [SerializeField] AudioClip menu_Splash_Go;

    [SerializeField] AudioClip machine_generic_open;
    [SerializeField] AudioClip machine_generic_close;
    [SerializeField] AudioClip machine_generic_begin;
    [SerializeField] AudioClip machine_generic_spit;

    [SerializeField] AudioClip dialogue_begin;
    [SerializeField] AudioClip dialogue_sucessful;
    [SerializeField] AudioClip dialogue_negate;
    [SerializeField] AudioClip dialogue_click_option;
    [SerializeField] AudioClip dialogue_end;
    [SerializeField] AudioClip dialogue_loop;

    [SerializeField] AudioClip action_hit_pinche;
    [SerializeField] AudioClip action_take_with_gloves;
    [SerializeField] AudioClip action_hit_showel;
    [SerializeField] AudioClip action_seed;

    [SerializeField] AudioClip hit_garbage;

    [SerializeField] AudioClip[] steps;
    [SerializeField] AudioClip[] dialoges;

    [SerializeField] AudioClip storage_open;
    [SerializeField] AudioClip storage_close;
    [SerializeField] AudioClip storage_open_garbage;
    [SerializeField] AudioClip storage_close_garbage;
    [SerializeField] AudioClip storage_Shake_garbage;
    [SerializeField] AudioClip storage_open_container;
    [SerializeField] AudioClip storage_close_container;

    [SerializeField] AudioClip machine_open_press;
    [SerializeField] AudioClip machine_close_press;

    [SerializeField] AudioClip machine_open_composter;
    [SerializeField] AudioClip machine_close_composter;

    [SerializeField] AudioClip machine_open_washingmachine;
    [SerializeField] AudioClip machine_close_washingmachine;

    [SerializeField] AudioClip machine_open_dryer;
    [SerializeField] AudioClip machine_close_dryer;

    [SerializeField] AudioClip machine_open_juicer;
    [SerializeField] AudioClip machine_close_juicer;

    [SerializeField] AudioClip machine_open_crafter;
    [SerializeField] AudioClip machine_close_crafter;

    public void Start()
    {
        if (item_pickup) AudioManager.instance.GetSoundPoolFastRegistry2D(item_pickup);

        if (hit_garbage) AudioManager.instance.GetSoundPoolFastRegistry2D(hit_garbage);

        if (inventory_over) AudioManager.instance.GetSoundPoolFastRegistry2D(inventory_over);
        if (inventory_select) AudioManager.instance.GetSoundPoolFastRegistry2D(inventory_select);
        if (inventory_switch_item) AudioManager.instance.GetSoundPoolFastRegistry2D(inventory_switch_item);

        if (machine_generic_open) AudioManager.instance.GetSoundPool(machine_generic_open.name, AudioManager.SoundDimesion.ThreeD, machine_generic_open);
        if (machine_generic_close) AudioManager.instance.GetSoundPool(machine_generic_close.name, AudioManager.SoundDimesion.ThreeD, machine_generic_close);
        if (machine_generic_begin) AudioManager.instance.GetSoundPool(machine_generic_begin.name, AudioManager.SoundDimesion.ThreeD, machine_generic_begin);
        if (machine_generic_spit) AudioManager.instance.GetSoundPool(machine_generic_spit.name, AudioManager.SoundDimesion.ThreeD, machine_generic_spit);

        if (menu_sello) AudioManager.instance.GetSoundPoolFastRegistry2D(menu_sello);
        if (menu_whoosh_characters_Enter) AudioManager.instance.GetSoundPoolFastRegistry2D(menu_whoosh_characters_Enter);
        if (menu_whoosh_characters_Exit) AudioManager.instance.GetSoundPoolFastRegistry2D(menu_whoosh_characters_Exit);
        if (menu_transition_enter) AudioManager.instance.GetSoundPoolFastRegistry2D(menu_transition_enter);
        if (menu_transition_exit) AudioManager.instance.GetSoundPoolFastRegistry2D(menu_transition_exit);
        if (menu_tentacle_idle_go) AudioManager.instance.GetSoundPoolFastRegistry2D(menu_tentacle_idle_go);
        if (menu_tentacle_idle_back) AudioManager.instance.GetSoundPoolFastRegistry2D(menu_tentacle_idle_back);
        if (menu_Splash_Go) AudioManager.instance.GetSoundPoolFastRegistry2D(menu_Splash_Go);

        if (dialogue_begin) AudioManager.instance.GetSoundPoolFastRegistry2D(dialogue_begin);
        if (dialogue_sucessful) AudioManager.instance.GetSoundPoolFastRegistry2D(dialogue_sucessful);
        if (dialogue_negate) AudioManager.instance.GetSoundPoolFastRegistry2D(dialogue_negate);
        if (dialogue_click_option) AudioManager.instance.GetSoundPoolFastRegistry2D(dialogue_click_option);
        if (dialogue_end) AudioManager.instance.GetSoundPoolFastRegistry2D(dialogue_end);
        if (dialogue_loop) AudioManager.instance.GetSoundPool(dialogue_loop.name, AudioManager.SoundDimesion.TwoD, dialogue_loop, true);

        if (action_hit_pinche) AudioManager.instance.GetSoundPoolFastRegistry2D(action_hit_pinche);
        if (action_take_with_gloves) AudioManager.instance.GetSoundPoolFastRegistry2D(action_take_with_gloves);
        if (action_hit_showel) AudioManager.instance.GetSoundPoolFastRegistry2D(action_hit_showel);
        if (action_seed) AudioManager.instance.GetSoundPoolFastRegistry2D(action_seed);

        if (storage_open) AudioManager.instance.GetSoundPoolFastRegistry2D(storage_open);
        if (storage_close) AudioManager.instance.GetSoundPoolFastRegistry2D(storage_close);
        if (storage_open_garbage) AudioManager.instance.GetSoundPoolFastRegistry2D(storage_open_garbage);
        if (storage_close_garbage) AudioManager.instance.GetSoundPoolFastRegistry2D(storage_close_garbage);
        if (storage_Shake_garbage) AudioManager.instance.GetSoundPoolFastRegistry2D(storage_Shake_garbage);
        if (storage_open_container) AudioManager.instance.GetSoundPoolFastRegistry2D(storage_open_container);
        if (storage_close_container) AudioManager.instance.GetSoundPoolFastRegistry2D(storage_close_container);

        if (machine_open_press) AudioManager.instance.GetSoundPoolFastRegistry2D(machine_open_press);
        if (machine_close_press) AudioManager.instance.GetSoundPoolFastRegistry2D(machine_close_press);

        if (machine_open_composter) AudioManager.instance.GetSoundPoolFastRegistry2D(machine_open_composter);
        if (machine_close_composter) AudioManager.instance.GetSoundPoolFastRegistry2D(machine_close_composter);

        if (machine_open_washingmachine) AudioManager.instance.GetSoundPoolFastRegistry2D(machine_open_washingmachine);
        if (machine_close_washingmachine) AudioManager.instance.GetSoundPoolFastRegistry2D(machine_close_washingmachine);

        if (machine_open_dryer) AudioManager.instance.GetSoundPoolFastRegistry2D(machine_open_dryer);
        if (machine_close_dryer) AudioManager.instance.GetSoundPoolFastRegistry2D(machine_close_dryer);

        if (machine_open_juicer) AudioManager.instance.GetSoundPoolFastRegistry2D(machine_open_juicer);
        if (machine_close_juicer) AudioManager.instance.GetSoundPoolFastRegistry2D(machine_close_juicer);

        if (machine_open_crafter) AudioManager.instance.GetSoundPoolFastRegistry2D(machine_open_crafter);
        if (machine_close_crafter) AudioManager.instance.GetSoundPoolFastRegistry2D(machine_close_crafter);

        for (int i = 0; i < steps.Length; i++)
            AudioManager.instance.GetSoundPoolFastRegistry2D(steps[i]);

        for (int i = 0; i < dialoges.Length; i++)
            AudioManager.instance.GetSoundPoolFastRegistry2D(dialoges[i]);
    }

    public static void Play_Item_PickUp() => AudioManager.instance.PlaySound(instance.item_pickup.name);

    public static void Play_HitGarbage() => AudioManager.instance.PlaySound(instance.hit_garbage.name);

    public static void Play_Inventory_Over() => AudioManager.instance.PlaySound(instance.inventory_over.name);
    public static void Play_Inventory_Select() => AudioManager.instance.PlaySound(instance.inventory_select.name);
    public static void Play_Inventory_Switch_Item() => AudioManager.instance.PlaySound(instance.inventory_switch_item.name);

    public static void Play_Menu_Sello() => AudioManager.instance.PlaySound(instance.menu_sello.name);
    public static void Play_Menu_WhoosCharacterEnter() => AudioManager.instance.PlaySound(instance.menu_whoosh_characters_Enter.name);
    public static void Play_Menu_WhoosCharacterExit() => AudioManager.instance.PlaySound(instance.menu_whoosh_characters_Exit.name);
    public static void Play_Transition_Enter() => AudioManager.instance.PlaySound(instance.menu_transition_enter.name);
    public static void Play_Transition_Exit() => AudioManager.instance.PlaySound(instance.menu_transition_exit.name);
    public static void Play_Tentacle_Idle_Go() => AudioManager.instance.PlaySound(instance.menu_tentacle_idle_go.name);
    public static void Play_Tentacle_Idle_Back() => AudioManager.instance.PlaySound(instance.menu_tentacle_idle_back.name);
    public static void Play_Splash_GO() => AudioManager.instance.PlaySound(instance.menu_Splash_Go.name);

    public static void Play_Machine_Generic_Open(Transform track) => AudioManager.instance.PlaySound(instance.machine_generic_open.name, track);
    public static void Play_Machine_Generic_Close(Transform track) => AudioManager.instance.PlaySound(instance.machine_generic_close.name, track);
    public static void Play_Machine_Generic_Begin_Process(Transform track) => AudioManager.instance.PlaySound(instance.machine_generic_begin.name, track);
    public static void Play_Machine_Generic_Spit(Transform track) => AudioManager.instance.PlaySound(instance.machine_generic_spit.name, track);

    public static void Play_Dialogue_Begin() => AudioManager.instance.PlaySound(instance.dialogue_begin.name);
    public static void Play_Dialogue_Sucessful() => AudioManager.instance.PlaySound(instance.dialogue_sucessful.name);
    public static void Play_Dialogue_Negate() => AudioManager.instance.PlaySound(instance.dialogue_negate.name);
    public static void Play_Dialogue_Click_Option() => AudioManager.instance.PlaySound(instance.dialogue_click_option.name);
    public static void Play_Dialogue_End() => AudioManager.instance.PlaySound(instance.dialogue_end.name);
    public static void Play_Dialogue_TalkLoop() => AudioManager.instance.PlaySound(instance.dialogue_loop.name);
    public static void Stop_Dialogue_TalkLoop()
    {
        AudioManager.instance.StopAllSounds(instance.currentdialoge);
        instance.currentdialoge = "";
    }

    public static void Play_Action_Pinche() => AudioManager.instance.PlaySound(instance.action_hit_pinche.name);
    public static void Play_Action_Take_With_Gloves() => AudioManager.instance.PlaySound(instance.action_take_with_gloves.name);
    public static void Play_Action_Showel_Hit() => AudioManager.instance.PlaySound(instance.action_hit_showel.name);
    public static void Play_Action_Seed() => AudioManager.instance.PlaySound(instance.action_seed.name);

    public static void Play_Storage_Open() => AudioManager.instance.PlaySound(instance.storage_open.name);
    public static void Play_Storage_Close() => AudioManager.instance.PlaySound(instance.storage_close.name);
    public static void Play_Storage_Open_Garbage() => AudioManager.instance.PlaySound(instance.storage_open_garbage.name);
    public static void Play_Storage_Close_Garbage() => AudioManager.instance.PlaySound(instance.storage_close_garbage.name);
    public static void Play_Storage_Shake_Garbage() => AudioManager.instance.PlaySound(instance.storage_Shake_garbage.name);
    public static void Play_Storage_Open_Container() => AudioManager.instance.PlaySound(instance.storage_open_container.name);
    public static void Play_Storage_Close_Container() => AudioManager.instance.PlaySound(instance.storage_close_container.name);

    public static void Play_Machine_Open_Press() => AudioManager.instance.PlaySound(instance.machine_open_press.name);
    public static void Play_Machine_Close_Press() => AudioManager.instance.PlaySound(instance.machine_close_press.name);

    public static void Play_Machine_Open_Composter() => AudioManager.instance.PlaySound(instance.machine_open_composter.name);
    public static void Play_Machine_Close_Composter() => AudioManager.instance.PlaySound(instance.machine_close_composter.name);

    public static void Play_Machine_Open_WashingMachine() => AudioManager.instance.PlaySound(instance.machine_open_washingmachine.name);
    public static void Play_Machine_Close_WashingMachine() => AudioManager.instance.PlaySound(instance.machine_close_washingmachine.name);

    public static void Play_Machine_Open_Dryer() => AudioManager.instance.PlaySound(instance.machine_open_dryer.name);
    public static void Play_Machine_Close_Dryer() => AudioManager.instance.PlaySound(instance.machine_close_dryer.name);

    public static void Play_Machine_Open_Juicer() => AudioManager.instance.PlaySound(instance.machine_open_juicer.name);
    public static void Play_Machine_Close_Juicer() => AudioManager.instance.PlaySound(instance.machine_close_juicer.name);

    public static void Play_Machine_Open_Crafter() => AudioManager.instance.PlaySound(instance.machine_open_crafter.name);
    public static void Play_Machine_Close_Crafter() => AudioManager.instance.PlaySound(instance.machine_close_crafter.name);

    public static void Play_NextStep() => instance.PlayNextStep();
    void PlayNextStep()
    {
        AudioManager.instance.PlaySound(steps[Random.Range(0, steps.Length-1)].name);
    }

    public static void Play_Dialoge() => instance.PlayNextDialoge();
    string currentdialoge;
    void PlayNextDialoge()
    {
        currentdialoge = dialoges[Random.Range(0, dialoges.Length-1)].name;
        AudioManager.instance.PlaySound(currentdialoge);
    }


}
