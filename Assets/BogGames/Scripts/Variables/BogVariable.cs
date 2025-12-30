using UnityEngine;
using System;

namespace BogGames.Variables
{
    // Mathematical operations that can be performed on variables.
    public enum SetOperator
    {
        /// <summary> = operator. </summary>
        Assign,
        /// <summary> =! operator. </summary>
        Negate,
        /// <summary> += operator. </summary>
        Add,
        /// <summary> -= operator. </summary>
        Subtract,
        /// <summary> *= operator. </summary>
        Multiply,
        /// <summary> /= operator. </summary>
        Divide
    }

    public abstract class BogVariable : ScriptableObject
    {
        public abstract object GetValue();
        public abstract void OnReset();
        public abstract void Add(object value);
        public abstract void Subtract(object value);
    }

    public abstract class BogBaseVariable<T> : BogVariable
    {
        [SerializeField] protected T value;

        public virtual T Value { get { return this.value; } set { this.value = value; } }

        public override object GetValue()
        {
            return Value;
        }

        protected T startValue;

        public override void OnReset()
        {
            Value = startValue;
        }

        protected virtual void OnEnable()
        {
            //cache the start value to use later on
            startValue = Value;
        }

        public override void Add(object value)
        {
            if (value is T || value == null)
                Add((T)value);
            else if (value is BogBaseVariable<T>)
            {
                var bv = value as BogBaseVariable<T>;
                Add(bv.value);
            }
            else
            {
                Debug.LogError("Cannot do Add on variable, as object type: " + value.GetType().Name + " is incompatible with " + typeof(T).Name);
            }
        }

        public virtual void Add(T value)
        {
        }

        public override void Subtract(object value)
        {
            if (value is T || value == null)
                Subtract((T)value);
            else if (value is BogBaseVariable<T>)
            {
                var bv = value as BogBaseVariable<T>;
                Subtract(bv.value);
            }
            else
            {
                Debug.LogError("Cannot do Subtract on variable, as object type: " + value.GetType().Name + " is incompatible with " + typeof(T).Name);
            }
        }

        public virtual void Subtract(T value)
        {
        }   
    }
}
