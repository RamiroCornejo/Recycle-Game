using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InventoryDataBase : MonoBehaviour
{
    public static UI_InventoryDataBase instance;
    private void Awake() => instance = this;

    [Header("Models")]
    [SerializeField] UI_Slot UI_Slot_Model;

    [Header("Sprites")]
    [SerializeField] Sprite sprite_slot_enter;
    [SerializeField] Sprite sprite_slot_normal;
    [SerializeField] Sprite sprite_slot_drag_origin;
    [SerializeField] Sprite sprite_slot_drop_destiny;
    [SerializeField] Sprite sprite_slot_special;
    [SerializeField] Sprite sprite_element_quality_normal;
    [SerializeField] Sprite sprite_element_quality_good;
    [SerializeField] Sprite sprite_element_quality_epic;
    [SerializeField] Sprite sprite_error;
    [SerializeField] Sprite sprite_dirty;
    [SerializeField] Sprite sprite_wet;

    public static UI_Slot SlotUIModel => instance.UI_Slot_Model;

    public static Sprite SlotEnter => instance.sprite_slot_enter;
    public static Sprite SlotNormal => instance.sprite_slot_normal;
    public static Sprite SlotDragOrigin => instance.sprite_slot_drag_origin;
    public static Sprite SlotDropDestiny => instance.sprite_slot_drop_destiny;
    public static Sprite SlotSpecial => instance.sprite_slot_special;
    public static Sprite Element_Quality_Normal => instance.sprite_element_quality_normal;
    public static Sprite Element_Quality_Good => instance.sprite_element_quality_good;
    public static Sprite Element_Quality_Epic => instance.sprite_element_quality_epic;
    public static Sprite Element_Dirty => instance.sprite_dirty;
    public static Sprite Element_Wet => instance.sprite_wet;

    public static Sprite GetElementByQuality(int val) {
        switch (val) {
            case 1: return Element_Quality_Normal;
            case 2: return Element_Quality_Good;
            case 3: return Element_Quality_Epic;
            default: return instance.sprite_error;
        }
    }


}
