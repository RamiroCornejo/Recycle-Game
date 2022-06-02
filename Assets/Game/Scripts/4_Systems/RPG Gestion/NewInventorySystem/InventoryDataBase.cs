using UnityEngine;
public class InventoryDataBase : MonoBehaviour
{
    // 1 Add carne
    // 2 Add zanahoria
    // 3 Add zapallo
    private void Awake() => instance = this;
    public static InventoryDataBase instance;
    public ElementData[] database;
    private void Update()
    {
    }
}

