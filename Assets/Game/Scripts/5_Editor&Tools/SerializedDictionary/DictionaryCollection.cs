using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tools.EventClasses;

[Serializable]
public class String_UnityEvDictionary : SerializableDictionary<string, UnityEvent> { }

[Serializable]
public class ID_UnityEvDictionary : SerializableDictionary<int, UnityEvent> { }

[Serializable]
public class String_FuncDictionary : SerializableDictionary<string, ConditionBool> { }

[Serializable]
public class ID_FuncDictionary : SerializableDictionary<int, ConditionBool> { }


[Serializable]
public class StringToCondition
{
    public string key;
    public ConditionBool value;
}
