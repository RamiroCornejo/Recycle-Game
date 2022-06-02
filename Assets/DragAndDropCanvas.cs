using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropCanvas : MonoBehaviour
{
    public static DragAndDropCanvas instance;
    Canvas own;
    RectTransform rect;

    public Image img_to_drag;
    private void Awake()
    {
        instance = this;
        own = GetComponent<Canvas>();
        rect = img_to_drag.GetComponent<RectTransform>();
    }

    void BDrag(Sprite img, Vector2 pos)
    {
        img_to_drag.sprite = img;
        //
    }

    void UpdateDrag(Vector2 pos)
    {
        //rect.anchoredPosition =  Input.mousePosition / own.scaleFactor;

        Vector3 vec = own.worldCamera.WorldToScreenPoint(rect.position);
        vec.x = Input.mousePosition.x;
        vec.y = Input.mousePosition.y;
        //rect.position = vec / own.scaleFactor;
        rect.position = own.worldCamera.ScreenToWorldPoint(vec / own.scaleFactor) ;
    }

    void EDrag()
    {
        rect.anchoredPosition = new Vector2(9000, 9000);
    }

    #region Statics
    public static void Drag(Vector2 pos) => instance.UpdateDrag(pos);
    public static void BeginDrag(Sprite img, Vector2 pos) => instance.BDrag(img, pos);
    public static void EndDrag() => instance.EDrag();
    #endregion
}
