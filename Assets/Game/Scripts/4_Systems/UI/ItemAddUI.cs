using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemAddUI : MonoBehaviour
{
    [SerializeField] float timeToDissappear = 3;
    [SerializeField] float speed = 2;
    [SerializeField] Image[] mySprites;
    [SerializeField] GameObject dirtyObj = null;
    [SerializeField] GameObject dryObj = null;

    public void SetValues(ItemValues values)
    {
        mySprites[0].sprite = values.itemSprite;
        dryObj.SetActive(!values.isDry);
        dirtyObj.SetActive(values.isDirty);
    }

    float timer;
    private void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;

        timer += Time.deltaTime;

        var lerp = 1 - timer / timeToDissappear;

        for (int i = 0; i < mySprites.Length; i++)
        {
            mySprites[i].color = new Color(mySprites[i].color.r, mySprites[i].color.g, mySprites[i].color.b, lerp);
        }

        if(timer >= timeToDissappear)
        {
            Destroy(this.gameObject);
        }
    }
}
