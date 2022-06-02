using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionDialogueBinder : MonoBehaviour
{
    [SerializeField] List<StringToCondition> funcDictionary = new List<StringToCondition>();

    public bool CheckCondition(string _name)
    {
        bool condition = false;

        ConditionBool conditionBool = ContainsKey(_name);

        if (conditionBool != null)
            condition = conditionBool.Invoke();

        return condition;
    }

    ConditionBool ContainsKey(string key)
    {
        for (int i = 0; i < funcDictionary.Count; i++)
        {
            if (funcDictionary[i].key == key)
                return funcDictionary[i].value;
        }

        Debug.LogError("No está la key pa");
        return null;
    }
}
