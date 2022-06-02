using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestContainer : MonoBehaviour
{
    public Container myInventory;

    public ElementData element_to_add;
    public ElementData element_to_add_2;
    public int ID_to_Add;

    private void Start()
    {
        myInventory = new Container(10);
    }

    private void Update()
    {
        
    }
}
