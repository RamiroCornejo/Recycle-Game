using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBoardInteract : MonoBehaviour
{
    [SerializeField] GenericInteractable interact = null;
    public void OnInteract()
    {
        UIManager.instance.OpenBoard(ReturnToInteract);
    }

    public void ReturnToInteract()
    {
        interact.AddToInteractor();
    }
}
