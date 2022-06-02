using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitoideMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float minTimeToHike = 5;
    [SerializeField] float maxTimeToHike = 10;

    [SerializeField] float hideSpeed = 1;
    [SerializeField] float hikeSpeed = 0.5f;

    [SerializeField] float desireXPos = 300;

    bool hiding;
    float currentTimeToHike;
    float timerToHike;

    bool hiking;
    Vector3 hidePos;
    Vector3 hikePos;

    private void Start()
    {
        ResetHike();
    }

    void ResetHike()
    {
        timerToHike = 0;
        currentTimeToHike = Random.Range(minTimeToHike, maxTimeToHike);
        hiking = false;
        hiding = false;
    }

    private void Update()
    {
        if (hiding)
        {
            transform.position += Vector3.right * hideSpeed * Time.deltaTime;

            if(transform.position.x >= hidePos.x)
            {
                ResetHike();
                transform.position = hidePos;
            }
            return;
        }

        if (hiking)
        {
            transform.position += Vector3.left * hikeSpeed * Time.deltaTime;

            if (transform.position.x <= hikePos.x)
            {
                hiking = false;
                transform.position = hikePos;
            }

            return;
        }

        if (timerToHike >= currentTimeToHike)
            return;


        timerToHike += Time.deltaTime;

        if (timerToHike >= currentTimeToHike)
        {
            hiking = true;
            hidePos = transform.position;
            hikePos = transform.position + Vector3.left * desireXPos;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hiding = true;
        hiking = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
