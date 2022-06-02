using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[System.Serializable]
public class Container //La caja de bombones
{
    CPResult ERROR_ZERO_MINUS = new CPResult(false, -1, "Hubo un error, mi cantidad temporal no puede ser menor a cero");
    CPResult CORRECT_PROCESS = new CPResult(true, 0, "el proceso se ejecutó correctamente");
    [SerializeField] float myTotalWeight;
    [SerializeField] List<Slot> slots;

    #region CONSTRUCTOR
    public Container(int capacity)
    {
        slots = new List<Slot>();
        for (int i = 0; i < capacity; i++)
        {
            Slot slot = new Slot(i);
            slot.SubscribeToAddAndRemove(AddToRegister, RemoveToRegister);
            slots.Add(slot);

        }
    }
    #endregion

    #region Index Register
    Dictionary<ElementData, HashSet<Slot>> register = new Dictionary<ElementData, HashSet<Slot>>();
    public void AddToRegister(ElementData _elem, Slot _slot)
    {
        if (register.ContainsKey(_elem))
        {
            if (!register[_elem].Contains(_slot))
            {
                register[_elem].Add(_slot);
                register[_elem].OrderBy(x => x);
            }
        }
        else
        {
            HashSet<Slot> index_collection = new HashSet<Slot>();
            index_collection.Add(_slot);
            register.Add(_elem, index_collection);
        }
    }
    public void RemoveToRegister(ElementData _elem, Slot _slot)
    {
        if (register.ContainsKey(_elem)) // lo tengo en el registro
        {

            if (register[_elem].Contains(_slot)) //lo tengo en el hashet
            {
                register[_elem].Remove(_slot);
                register[_elem].OrderBy(x => x);
            }

            register[_elem].RemoveWhere(x => x.Position == _slot.Position);

            if (register[_elem].Count <= 0) //el hashet esta vacio
            {
                //Debug.Log("el hashet esta vacio");
                register[_elem] = null;
                register.Remove(_elem);
            }
        }
    }
    /// <summary>
    /// si llega a retornar null es porque no encontró ninguno que cumpla
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public Slot[] GetSlotsByElementPredicate(ElementData _elem, Func<Slot, bool> predicate)
    {
        if (register.ContainsKey(_elem))
        {
            HashSet<Slot> reg = new HashSet<Slot>();
            foreach (var s in register[_elem])
            {
                if (predicate(s))
                {
                    reg.Add(s);
                    //return s;
                }
            }
            if (reg.Count > 0)
            {
                return reg.ToArray();
            }

            return null; // estaba la key pero ninguno cumplía el predicado
        }
        else
        {
            return null; //no lo encontramos, habra que buscar un espacio vacio
        }
    }
    #endregion

    public int Capacity => slots.Count;
    public Slot GetSlotByIndex(int index) => slots[index];

    public void AddNewSlots(int quantityToAdd)
    {
        for (int i = 0; i < quantityToAdd; i++)
        {
            Slot slot = new Slot(i);
            slots.Add(slot);
        }

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].ChangePosition(i);
        }
    }

    bool Check(ElementData elem)
    {
        return true;
    }

    public CPResult Remove_Element(ElementData _data, int _quantity, int _quality, bool isDry, bool isDirty, bool respect_quality = false, int specific_index = -1 )
    {
        var aux_quantity = _quantity;

        bool use_specific_index = specific_index != -1;

        #region Primero busco en los registros y obtengo un Array para laburarlo mas comodo
        var regslots = GetSlotsByElementPredicate(_data, x =>
        {
            if (respect_quality)
            {
                return
                    x.Stack.Quality == _quality &&
                    x.Stack.IsDry == isDry &&
                    x.Stack.IsDirty == isDirty && 
                    (use_specific_index && x.Position == specific_index);
            }
            else return true;
        });
        #endregion
        #region Ahora recorro el array del registro y remuevo
        if (regslots != null)
        {
            HashSet<Slot> toremove = new HashSet<Slot>();
            foreach (var s in regslots)
            {
                #region Removido generico

                var quant_in_slot = s.Stack.Quantity;

                ElementData aux_to_delete = s.Element;

                if (aux_quantity <= quant_in_slot)
                {
                    //le remuevo mi auxiliar indiscriminadamente porque mi auxiliar es mas chico, o igual
                    //vacío totalmente mi auxiliar

                    RemoveToRegister(_data, s);

                    if (!s.RemoveElement(aux_quantity)) throw new System.Exception("Hay algo mal en la cuenta");
                    aux_quantity = 0;
                }
                else
                {
                    //le quito lo que necesito a mi auxiliar y remuevo esa cantidad
                    aux_quantity -= quant_in_slot;
                    if (!s.RemoveElement(quant_in_slot)) throw new System.Exception("Hay algo mal en la cuenta");
                }

                #endregion
            }
        }
       
        #endregion

        #region Removido por fuerza bruta
        if (aux_quantity > 0) //si me sobró algo para seguir sacando
        {

            for (int i = 0; i < slots.Count; i++)
            {
                if (use_specific_index)
                {
                    if (slots[i].Position != specific_index) continue;
                }

                if (!slots[i].IsEmpty)
                {
                    if (respect_quality)
                    {
                        if (slots[i].HasSameCompleteObject(_data, _quality, isDry, isDirty))
                        {
                            #region Removido generico

                            var quant_in_slot = slots[i].Stack.Quantity;

                            if (aux_quantity <= quant_in_slot)
                            {
                                //le remuevo mi auxiliar indiscriminadamente porque mi auxiliar es mas chico, o igual
                                //vacío totalmente mi auxiliar
                                RemoveToRegister(_data, slots[i]);
                                if (!slots[i].RemoveElement(aux_quantity)) throw new System.Exception("Hay algo mal en la cuenta");
                                aux_quantity = 0;
                            }
                            else
                            {
                                //le quito lo que necesito a mi auxiliar y remuevo esa cantidad
                                aux_quantity -= quant_in_slot;
                                if (!slots[i].RemoveElement(quant_in_slot)) throw new System.Exception("Hay algo mal en la cuenta");
                            }

                            #endregion
                        }
                        else continue;
                    }
                    else
                    {
                        if (slots[i].HasSameOnlyElement(_data))
                        {
                            #region Removido generico

                            var quant_in_slot = slots[i].Stack.Quantity;

                            if (aux_quantity <= quant_in_slot)
                            {
                                //le remuevo mi auxiliar indiscriminadamente porque mi auxiliar es mas chico, o igual
                                //vacío totalmente mi auxiliar
                                RemoveToRegister(_data, slots[i]);
                                if (!slots[i].RemoveElement(aux_quantity)) throw new System.Exception("Hay algo mal en la cuenta");
                                aux_quantity = 0;
                            }
                            else
                            {
                                //le quito lo que necesito a mi auxiliar y remuevo esa cantidad
                                aux_quantity -= quant_in_slot;
                                if (!slots[i].RemoveElement(quant_in_slot)) throw new System.Exception("Hay algo mal en la cuenta");
                            }

                            #endregion
                        }
                        else continue;
                    }
                }
                else
                {
                    continue;
                }

                if (slots[i].IsEmpty)
                {
                    //Debug.Log("removiendo del registro");
                    RemoveToRegister(_data, slots[i]);
                }

                if (aux_quantity == 0) return CORRECT_PROCESS;
                if (aux_quantity < 0) return ERROR_ZERO_MINUS;
                if (aux_quantity > 0) continue;
            }
        }
        #endregion

        if (aux_quantity == 0) return CORRECT_PROCESS;
        if (aux_quantity < 0) return ERROR_ZERO_MINUS;
        if (aux_quantity > 0)
        {
            return new CPResult(false, aux_quantity, "ERROR: me sobraron elemento para eliminar");
        }

        return new CPResult(false, aux_quantity, "ERROR: pudo llegar hasta acá porque no hay un checkeo previo, al remover no deberia tener un resto");
    }

    public CPResult Add_Element(ElementData l_data, int l_quantity, int l_quality, bool _isDry, bool _isDirty)
    {
        var aux_temp = l_quantity;

        //esto sirve para decirle al CPResult, che, laburé con estos casilleros
        List<int> used_positions_temp_registry = new List<int>();
        var regslots = GetSlotsByElementPredicate(l_data, x =>
        {
            return
                    x.Stack.Quality == l_quality &&
                    x.Stack.IsDry == _isDry &&
                    x.Stack.IsDirty == _isDirty;
        });
        if (regslots != null)
        {
            foreach (var s in regslots)
            {
                #region Agregado generico
                var free_space_quant = s.Stack.FreeSpaces;

                if (aux_temp <= free_space_quant) //hago la suma directamente y corto la ejecucion porque ya agregaria todo
                {
                    if (!s.AddElement(aux_temp)) throw new System.Exception("Hay algo mal en la cuenta");
                    aux_temp = 0;
                }
                else // El valor es mayor, le quito lo que necesito al aux y relleno el resto
                {
                    aux_temp -= free_space_quant;
                    if (!s.AddElement(free_space_quant)) throw new System.Exception("Hay algo mal en la cuenta");
                }
                #endregion
            }
        }
        if (aux_temp > 0)
        {
            //busco un casillero vacio
            foreach (var slot in slots)
            {
                //if (element_register[tuple_key].Contains(slot)) continue;

                if (slot.IsEmpty) // Encontré uno que esta vacio y no tiene los mismos datos
                {
                    slot.CreateNewStack(l_data);
                    slot.Stack.Quality = l_quality;
                    slot.Stack.IsDry = _isDry;
                    slot.Stack.IsDirty = _isDirty;

                    #region Agregado generico
                    var free_space_quant = slot.Stack.FreeSpaces;

                    used_positions_temp_registry.Add(slot.Position);

                    if (aux_temp <= free_space_quant) //hago la suma directamente y corto la ejecucion porque ya agregaria todo
                    {
                        if (!slot.AddElement(aux_temp)) throw new System.Exception("Hay algo mal en la cuenta");
                        aux_temp = 0;
                    }
                    else // El valor es mayor, le quito lo que necesito al aux y relleno el resto
                    {
                        aux_temp -= free_space_quant;
                        if (!slot.AddElement(free_space_quant)) throw new System.Exception("Hay algo mal en la cuenta");
                    }
                    #endregion

                    #region TO-DO: Agregar al registro
                    //y lo agrego al registro para la proxima pasada
                    //var reg_slots = element_register[tuple_key];
                    //reg_slots.Add(slot);
                    #endregion

                    AddToRegister(slot.Element, slot);

                }
                else // como no esta vacio tengo que checkear si es lo mismo que quiero agregar
                {
                    if (slot.HasSameCompleteObject(l_data, l_quality, _isDry, _isDirty))//el elemento que encontré es de mi mismo tipo, con la misma calidad
                    {
                        #region Agregado generico
                        var free_space_quant = slot.Stack.FreeSpaces;

                        if (aux_temp <= free_space_quant) //hago la suma directamente y corto la ejecucion porque ya agregaria todo
                        {
                            if (!slot.AddElement(aux_temp)) throw new System.Exception("Hay algo mal en la cuenta");
                            aux_temp = 0;
                        }
                        else // El valor es mayor, le quito lo que necesito al aux y relleno el resto
                        {
                            aux_temp -= free_space_quant;
                            if (!slot.AddElement(free_space_quant)) throw new System.Exception("Hay algo mal en la cuenta");
                        }
                        #endregion
                        used_positions_temp_registry.Add(slot.Position);
                    }
                    else
                    {
                        continue; //este slot tiene algo ya, y no es de mi tipo
                    }
                }

                if (aux_temp == 0)
                {
                    var message = CORRECT_PROCESS;
                    message.AddSlotsEquiped(used_positions_temp_registry);
                    return message;
                }
                if (aux_temp < 0) return ERROR_ZERO_MINUS;
                if (aux_temp > 0) continue;
            }
        }
        if (aux_temp == 0)
        {
            var message = CORRECT_PROCESS;
            message.AddSlotsEquiped(used_positions_temp_registry);
            return message;
        }
        if (aux_temp < 0) return ERROR_ZERO_MINUS;
        if (aux_temp > 0)
        {
            return new CPResult(false, aux_temp, "INVENTARIO LLENO: Me sobran elementos", used_positions_temp_registry);
        }
        return new CPResult(false, aux_temp, "INVENTARIO LLENO: Me sobran elementos", used_positions_temp_registry);
    }

    public bool QueryElement(ElementData l_data, int l_quantity, int l_quality, bool _isDry, bool _isDirty, bool l_respect_quality = false)
    {
        var aux_temp = l_quantity;

        #region TO-DO: CAMBIAR POR EL REGISTRO DE HASHET<INT>
        //var tuple_key = Tuple.Create(l_data, l_quality);
        //preguntar primero al registro
        #endregion

        //busco un casillero vacio
        for (int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].IsEmpty)
            {
                if (l_respect_quality)
                {
                    if (slots[i].HasSameCompleteObject(l_data, l_quality, _isDry, _isDirty))
                    {
                        //////////////////////
                        var quant_in_slot = slots[i].Stack.Quantity;

                        if (aux_temp <= quant_in_slot)
                        {
                            //vacío totalmente mi auxiliar porque es mas chico que lo que hay
                            aux_temp = 0;
                        }
                        else
                        {
                            //le quito lo que necesito a mi auxiliar y remuevo esa cantidad
                            aux_temp -= quant_in_slot;
                        }
                    }
                }
                else
                {
                    if (slots[i].HasSameOnlyElement(l_data))
                    {
                        //////////////////////
                        var quant_in_slot = slots[i].Stack.Quantity;

                        if (aux_temp <= quant_in_slot)
                        {
                            //vacío totalmente mi auxiliar porque es mas chico que lo que hay
                            aux_temp = 0;
                        }
                        else
                        {
                            //le quito lo que necesito a mi auxiliar y remuevo esa cantidad
                            aux_temp -= quant_in_slot;
                        }
                    }
                }
            }
            else
            {
                continue;
            }
        }

        #region Checkeo final
        if (aux_temp == 0) return true;
        else
        {
            if (aux_temp > 0) { /*me faltan*/ return false; }
            else { throw new System.Exception("Hubo un error en el calculo"); }
        }
        #endregion
    }

    //esto es para lo nuevo... remplazamos el diccionario de lista de slots por hashet de indexs
    public List<Slot> GetListByIndexes(params int[] indexes)
    {
        return null;
    }

    #region Debugs Slots & Registers
    public string UpdateDebug_Slots()
    {
        string aux = "";
        foreach (var s in slots)
        {
            if (!s.IsEmpty)
            {
                aux += s.Position + " )" + s.Element.Element_Name + " :" + s.Quantity + "\n";
            }
            else
            {
                aux += s.Position + " )-\n";
            }

        }
        return aux;
    }
    public string UpdateDebug_Registers()
    {
        string aux = "";
        foreach (var r in register)
        {
            aux += r.Key.Element_Name + " :";
            foreach (var s in r.Value) aux += "[" + s.Position + "]";
            aux += "\n";
        }
        return aux;
    }
    #endregion

}

[System.Serializable]
public class Slot //Los separadores de bombones, [ESTÁTICO]
{
    [SerializeField] int position;
    public int Position { get { return position; } }
    [SerializeField] StackedPile stack;

    public StackedPile Stack => stack;
    public ElementData Element => stack.Element ? stack.Element : null;
    public int Quantity => stack.Quantity;
    public bool IsEmpty => stack.IsEmptyOrElementNull;

    Action<ElementData, Slot> OnAddToRegister;
    Action<ElementData, Slot> OnRemoveRegister;

    #region Contructor
    public Slot(int position)
    {
        this.position = position;
        stack = new StackedPile();
        //stack.SubscribeSlotToAddAndRemove(OnSlotAdded, OnSlotRemoved);
    }
    void OnSlotAdded(ElementData element) => OnAddToRegister.Invoke(element, this);
    void OnSlotRemoved(ElementData element) => OnRemoveRegister.Invoke(element, this);
    #endregion

    public void SubscribeToAddAndRemove(Action<ElementData, Slot> cbk_onAdd, Action<ElementData, Slot> cbk_onRemove)
    {
        OnAddToRegister = cbk_onAdd;
        OnRemoveRegister = cbk_onRemove;
    }

    public bool EqualsByPosition(Slot _slot)
    {
        return Position == _slot.position;
    }

    public void CreateNewStack(ElementData element)
    {
        stack = new StackedPile();
        stack.SetElement(element);
    }

    public bool AddElement(int quantity)
    {
        return stack.Add_SAFE(quantity);
    }

    public bool RemoveElement(int quantity)
    {
        return stack.Remove_SAFE(quantity);
    }

    //public bool HasSameData(Tuple<ElementData, int> tuple_key)
    //{
    //    return stack.Full_Element_Is_Equal(tuple_key.Item1, tuple_key.Item2);
    //}

    public bool HasSameCompleteObject(ElementData elem, int quality, bool _isDry, bool _isDirty)
    {
        return stack.Full_Element_Is_Equal(elem, quality, _isDry, _isDirty);
    }

    public bool HasSameOnlyElement(ElementData elem)
    {
        return stack.Only_Element_Is_Equal(elem);
    }

    public void ChangePosition(int position)
    {
        this.position = position;
    }

    #region Drag & Drop Functions
    public void OverrideStack(StackedPile _stack)
    {
        this.stack.SetElement(_stack.Element);
        this.stack.Quantity = _stack.Quantity;
        this.stack.Quality = _stack.Quality;
        this.stack.IsDry = _stack.IsDry;
        this.stack.IsDirty = _stack.IsDirty;
        this.stack.IsTabbed = _stack.IsTabbed;
    }

    public StackedPile GetFullCopy()
    {
        return this.stack.ManualCopy();
    }

    public bool CanStackWith(StackedPile _otherstack, int slot_index_position)
    {
        return this.stack.CanStackWith(_otherstack) && this.Position != slot_index_position;
    }

    public override bool Equals(object obj)
    {
        return stack.Equals(((Slot)obj).stack);
    }

    public StackedPile DropStack(StackedPile origin_stack)
    {
        if (this.stack.IsEmptyOrElementNull)
        {
            var result = this.stack.ManualCopy();
            this.stack = origin_stack;
            return result;
        }

        if (this.stack.CanStackWith(origin_stack))
        {
            var result = origin_stack.ManualCopy();

            int bigger = Mathf.Max(this.stack.Quantity, origin_stack.Quantity);
            int smaller = Mathf.Min(this.stack.Quantity, origin_stack.Quantity);

            int raw_result = bigger + smaller;
            int diference = raw_result - this.stack.MaxStack;

            Debug.Log("acá si entra wey");
            //le paso los valores crudos porque internamente se encarga de hacer el recorte
            this.stack.ModifyQuantity(raw_result);
            result.ModifyQuantity(diference);

            return result;
        }
        else
        {
            var result = this.stack.ManualCopy();
            this.stack = origin_stack;
            return result;
        }
    }
    #endregion

    public void Empty()
    {
        stack.Force_to_Empty();
    }
}

[System.Serializable]
public class StackedPile //La bolsita del bombon
{
    /// <summary>
    /// ///////////////////////////////PONER LO DE LA CALIDAD ACÁ
    /// </summary>

    #region Vars
    [SerializeField] int quant = 0;
    [SerializeField] ElementData element = null; //El Bombon
    [SerializeField] float weight = 0f;
    [SerializeField] int quality = 1;
    [SerializeField] bool isDry = true;
    [SerializeField] bool isDirty = false;
    public bool IsTabbed = false;

    public bool CanStackWith(StackedPile other)
    {
        if (element.Equals(other.element) &&
            quality == other.Quality &&
            IsDry == other.isDry &&
            IsDirty == other.IsDirty)
        {
            return true;
        }
        else return false;
    }

    Action<ElementData> OnAddedElementCbk = delegate { };
    Action<ElementData> OnRemovedElementCbk = delegate { };
    public void SubscribeSlotToAddAndRemove(Action<ElementData> addcbk, Action<ElementData> removecbk)
    {
        OnAddedElementCbk = addcbk;
        OnRemovedElementCbk = removecbk;
    }
    #endregion

    #region Getters & Setters
    public ElementData Element => element;
    public float Weight => weight;
    public int MaxStack => element.MaxStack;
    public bool IsEmptyOrElementNull => Quantity <= 0 || element == null;
    public bool IsFull => Quantity >= MaxStack;
    public int Quantity
    {
        get => quant;
        set
        {
            if (quant == 0)
            {
                if (value > 0)
                {
                    OnAddedElementCbk.Invoke(Element);
                }
            }
            quant = value;
            if (quant <= 0)
            {
                OnRemovedElementCbk.Invoke(Element);
                element = null;
                quant = 0;
                quality = -1;
            }
            weight = element != null ? quant * element.Weight : 0;
        }
    }
    public int FreeSpaces => MaxStack - Quantity;
    public bool Full_Element_Is_Equal(ElementData element, int quality, bool isdry, bool isdirty)
    {
        return this.element.Equals(element) && this.quality == quality && this.IsDry == isdry && this.IsDirty == isdirty;
    }
    public bool Only_Element_Is_Equal(ElementData element)
    {
        return this.element.Equals(element);
    }
    public bool IsEqualByDebuffs(StackedPile other)
    {
        return isDirty == other.isDirty && IsDry == other.IsDry;
    }
    public int Quality
    {
        get => quality;
        set
        {
            quality = value;
            //quality = Mathf.Clamp(value, 1, element.MaxQuality);
        }
    }
    public bool IsDry
    {
        get => isDry;
        set
        {
            isDry = value;
        }
    }
    public bool IsDirty
    {
        get => isDirty;
        set
        {
            isDirty = value;
        }
    }

    #endregion



    #region Constructor
    public StackedPile()
    {
        Quantity = 0;
    }
    public void ResetStack()
    {
        Quantity = 0;
    }
    #endregion

    #region Copy
    public StackedPile ManualCopy()
    {
        StackedPile copy = new StackedPile();
        copy.quality = quality;
        copy.element = element;
        copy.Quantity = Quantity;
        copy.weight = weight;
        copy.IsDry = IsDry;
        copy.isDirty = isDirty;
        copy.IsTabbed = IsTabbed;
        return copy;
    }
    #endregion

    #region Modifiers
    public void SetElement(ElementData element)
    {
        this.element = element;
    }

    public void ModifyQuantity(int quant_to_modify)
    {
        if (quant_to_modify > MaxStack) Quantity = MaxStack;
        else Quantity = quant_to_modify;
    }

    #endregion

    #region ADD FUNCTIONS
    public bool Add_SAFE(int quant_to_add = 1)
    {
        var aux = Quantity + quant_to_add;
        if (aux > MaxStack) return false;
        Quantity = aux;
        return true;
    }
    public void Add_RAW(int quant_to_add = 1)
    {
        Quantity = quant_to_add;
        if (Quantity > MaxStack) Quantity = MaxStack;
    }
    public void Add_UNSAFE(int quant_to_add = 1)
    {
        Quantity = quant_to_add;
    }
    #endregion

    #region REMOVE FUNCTIONS
    public bool Remove_SAFE(int quant_to_remove = 1)
    {
        var aux = Quantity - quant_to_remove;
        if (aux < 0) return false;
        Quantity = aux;
        return true;
    }
    public void Remove_RAW(int quant_to_remove = 1)
    {
        Quantity = quant_to_remove;
    }
    #endregion

    #region Fill or Empty
    public void Force_to_Fill()
    {
        Quantity = MaxStack;
    }
    public void Force_to_Empty()
    {
        Quantity = 0;
    }
    #endregion

    #region Object Override
    public override bool Equals(object obj)
    {
        var stack = (StackedPile)obj;

        if (stack.element == null && element != null) return false; //el primero es null y el otro no, son distintos
        if (stack.element != null && element == null) return false; //el segundo es null y el otro no, son distintos
        if (stack.element == null && element == null) return false; //ambos son nulos, pero igual no queremos que prosiga

        return
            stack.element.Equals(element) &&
            stack.quality == quality &&
            stack.IsDry == IsDry &&
            stack.IsDirty == IsDirty;
    }
    #endregion
}

public static class ContainerExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Owner">este seria a donde queremos que se rellene</param>
    /// <param name="DropValue">este seria a donde dirigimos las sobras</param>
    /// <param name="MaxValue">este es el maximo que se puede stackear</param>
    public static void Drop(ref this int Owner, ref int DropValue, int MaxValue)
    {
        int total = Owner + DropValue; // primero sumamos las dos cantidades
        int remain = total - MaxValue; // luego las restamos por el maximo para obtener el resto

        //si sobró
        if (remain > 0)
        {
            Owner = MaxValue;
            DropValue = remain;
        }
        else
        {
            Owner = total;
            DropValue = 0;
        }
    }
}

public struct CPResult
{
    [SerializeField] bool process_successfull;
    [SerializeField] int remainder_quantity;
    [SerializeField] string message;
    List<int> slots_equiped;

    public CPResult(bool process_successfull, int remainder_quantity, string message, List<int> slots = default)
    {
        this.process_successfull = process_successfull;
        this.remainder_quantity = remainder_quantity;
        this.message = message;
        this.slots_equiped = slots;
    }

    public void AddSlotsEquiped(List<int> slots) => this.slots_equiped = slots;

    public bool Process_Successfull { get => process_successfull; }
    public int Remainder_Quantity { get => remainder_quantity; }
    public string Message { get => message; }
    public List<int> SlotsEquiped { get => slots_equiped; }
}
