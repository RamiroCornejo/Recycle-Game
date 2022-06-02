﻿using UnityEngine;
using Tools.Extensions;
using System.Collections.Generic;

namespace Tools.Structs
{
    public struct posrot
    {
        internal Vector3 pos;
        internal Vector3 rot;
        public posrot(Vector3 pos, Vector3 rot)
        {
            this.pos = pos;
            this.rot = rot;
        }
    }

    

    //INVENTORY SYSTEM VIEJO
    //#region ITEM SYSTEM
    //[System.Serializable]
    //public struct Stacked_255_Object
    //{
    //    const byte MIN = 0;
    //    const byte MAX = 255;
    //    [SerializeField] string name;
    //    [SerializeField] byte cant;
    //    public string Name { get => name; }
    //    public byte Cant { get => cant;
    //        set
    //        {
    //            cant = ExtensionsAndUtils.Clamp(value, MIN, MAX);
    //        }
    //    }
    //    public Stacked_255_Object(byte cant = 0, string name = "defaultobject")
    //    {
    //        this.cant = cant;
    //        this.name = name;
    //    }
    //    public void Modify_Set_Cant(byte cant) => this.Cant = cant;
    //    public void Modify_Add_Cant(byte cant) => this.Cant += cant;
    //    public void Modify_Add_One() => this.Cant++;
    //    public void Modify_Remove_Cant(byte cant) => this.Cant -= cant;
    //    public void Modify_Remove_One() => this.Cant--;
    //    public void Modify_Clear() => this.Cant = 0;
    //    public void Modify_FillToMax() => this.Cant = 255;
    //}
    //
    //[System.Serializable]
    //public struct Item_255
    //{
    //    #region Variables
    //    const byte MIN = 0;
    //    const byte MAX = 255;
    //    [SerializeField] byte id;
    //    [SerializeField] string name;
    //    [SerializeField] byte cant;
    //    #endregion
    //    #region Getters & Setters
    //    public string Name { get => name; }
    //    public byte Cant
    //    {
    //        get => cant;
    //        set
    //        {
    //            cant = ExtensionsAndUtils.Clamp(value, MIN, MAX);
    //        }
    //    }
    //    public byte Id { get => id; }
    //    #endregion
    //    #region Contructor
    //    public Item_255(byte cant = 0, string name = "defaultobject", byte id = 0)
    //    {
    //        this.cant = cant;
    //        this.name = name;
    //        this.id = id;
    //    }
    //    #endregion
    //    #region Modifiers
    //    public void Modify_ID(byte val) => this.id = val;
    //    public void Modify_Set_Cant(byte cant) => this.Cant = cant;
    //    public void Modify_Add_Cant(byte cant) => this.Cant += cant;
    //    public void Modify_Add_One() => this.Cant++;
    //    public void Modify_Remove_Cant(byte cant) => this.Cant -= cant;
    //    public void Modify_Remove_One() => this.Cant--;
    //    public void Modify_Clear() => this.Cant = 0;
    //    public void Modify_FillToMax() => this.Cant = 255;
    //    #endregion
    //}

    //[System.Serializable]
    //public struct Item_Data_Base_Builds_Recipes
    //{
    //    public int ID_Element_To_Build;
    //    public int Cant_To_Recive;
    //    public Item_Recipe[] recipe;
    //}
    //[System.Serializable]
    //public struct Item_Recipe
    //{
    //    public ResourceType resource;
    //    public int Cant;
    //}

    //[System.Serializable]
    //public struct Item_Data_Base_Element
    //{
    //    public byte ID;
    //    public string Name;
    //}
    //#endregion
}
