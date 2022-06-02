using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerShortcuts : MonoBehaviour
{
    public static InputManagerShortcuts instance;
    private void Awake() => instance = this;

    bool onMenues = false;

    public static void OpenMenues() => instance.onMenues = true;
    public static void CloseMenues() => instance.onMenues = false;

    public static bool MOUSE_IsMoving => Input.GetAxisRaw("Mouse X") > 0 || Input.GetAxisRaw("Mouse Y") > 0;

    //GAME INPUT
    public static float HORIZONTAL => !instance.onMenues ? Input.GetAxis("Horizontal") : 0;
    public static float VERTICAL => !instance.onMenues ? Input.GetAxis("Vertical") : 0;

    public static bool PRESS_Attack => !instance.onMenues ? Input.GetButtonDown("Fire1") : false;
    public static bool RELEASE_Attack => !instance.onMenues ? Input.GetButtonUp("Fire1") : false;

    public static bool PRESS_Throw => !instance.onMenues ? Input.GetButtonDown("Fire2") : false;
    public static bool RELEASE_Throw => !instance.onMenues ? Input.GetButtonUp("Fire2") : false;

    public static bool PRESS_JumpDash => !instance.onMenues ? Input.GetButtonDown("Jump") : false;
    public static bool RELEASE_JumpDash => !instance.onMenues ? Input.GetButtonUp("Jump") : false;

    //GENERAL INPUT
    public static bool PRESS_OpenInventory => Input.GetButtonDown("Inventory");

    //UI INPUT
    public static float UI_HorizontalAxis => instance.onMenues ? Input.GetAxis("Horizontal") : 0;
    public static float UI_VerticalAxis => instance.onMenues ? Input.GetAxis("Vertical") : 0;

    public static bool PRESS_Accept => instance.onMenues ? Input.GetButtonDown("Submit") : false;
    public static bool PRESS_Equip => instance.onMenues ? Input.GetButtonDown("Equip") : false;
    public static bool PRESS_Cancel => instance.onMenues ? Input.GetButtonDown("Cancel") : false;
}
