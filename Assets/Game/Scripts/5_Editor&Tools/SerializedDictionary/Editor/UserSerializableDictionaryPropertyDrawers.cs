using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(String_FuncDictionary))]
[CustomPropertyDrawer(typeof(String_UnityEvDictionary))]
//[CustomPropertyDrawer(typeof(DamageType_ModelsList))]
public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}

//[CustomPropertyDrawer(typeof(ModelsList))]
public class AnySerializableDictionaryStoragePropertyDrawer: SerializableDictionaryStoragePropertyDrawer {}
