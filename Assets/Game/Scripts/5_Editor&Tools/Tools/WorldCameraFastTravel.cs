using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldCameraFastTravel : MonoBehaviour
{
    public Camera myCamera;
    public Transform target;
    bool isOpen;
    public CanvasGroup MyCanvas;
    public float initialSize = 100;
    public float minSize = 0;
    public float maxSize = 300;
    public float scrollSpeed = 3;
    public Image myFill;
    public TextMeshProUGUI value_fill_text;

    private void Start()
    {
        minSize = initialSize;
        myCamera.orthographicSize = initialSize;
        value_fill_text.text = initialSize.ToString();
        myFill.fillAmount = myCamera.orthographicSize / maxSize;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            isOpen = !isOpen;
            if (isOpen) OpenCamera();
            else CloseCamera();
        }

        if (!isOpen) return;

        Vector3 newpos = new Vector3(target.position.x, target.position.y + 50, target.position.z);
        myCamera.transform.position = newpos;

        if (Input.GetAxis("Mouse ScrollWheel") > 0.1)
        {
            myCamera.orthographicSize = myCamera.orthographicSize + scrollSpeed * Time.deltaTime;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < -0.1)
        {
            myCamera.orthographicSize = myCamera.orthographicSize - scrollSpeed * Time.deltaTime;
        }

        if (myCamera.orthographicSize < minSize) myCamera.orthographicSize = minSize;
        if (myCamera.orthographicSize > maxSize) myCamera.orthographicSize = maxSize;

        myFill.fillAmount = myCamera.orthographicSize / maxSize;
        value_fill_text.text = myCamera.orthographicSize.ToString();

        if (Input.GetMouseButtonDown(0))
        {
            var ray = myCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                target.position = hit.point;
            }

        }
    }

    public void ChangeValue(float valuetochange)
    {
        myCamera.orthographicSize = valuetochange;
    }

    public void OpenCamera()
    {
        myCamera.enabled = true;
        MyCanvas.alpha = 1;
        MyCanvas.blocksRaycasts = true;
        MyCanvas.interactable = true;
    }

    public void CloseCamera()
    {
        myCamera.enabled = false;
        MyCanvas.alpha = 0;
        MyCanvas.blocksRaycasts = false;
        MyCanvas.interactable = false;
    }
}
