using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestRequirement : MonoBehaviour
{
    public abstract bool IsRequireComplete();

    public abstract void RequireRemove(bool isOneShot);
}