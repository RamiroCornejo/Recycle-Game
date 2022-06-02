using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtensionsTests : MonoBehaviour
{

    private void Start()
    {
        Debugear(3, 5);
        Debugear(5, 5);
        Debugear(6, 7);
        Debugear(10, 10);
        Debugear(9, 7);
        Debugear(7, 9);
        Debugear(8, 0);
        Debugear(3, 8);
    }


    void Debugear(int memory, int clicked)
    {
        print("<----------------------------");
        print("Inicio> memory: " + memory + " clicked: " + clicked);
        clicked.Drop(ref memory, 10);
        print("Fin> memory: " + memory + " clicked: " + clicked);
        print("---------------------------->");
    }

}
