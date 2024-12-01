using System;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace LeonDrace.Utility.InterfaceSerializer
{
    [Serializable]
    public class InterfaceReference<TInterface, TObject> where TObject : Object where TInterface : class
    {
        [SerializeField, HideInInspector] private TObject m_UnderlyingValue;

        public TInterface Value
        {
            get => m_UnderlyingValue switch
            {
                null => null,
                TInterface @interface => @interface,
                _ => throw new InvalidOperationException(
                    $"{m_UnderlyingValue} needs to implement interface {nameof(TInterface)}.")
            };
            set => m_UnderlyingValue = value switch
            {
                null => null,
                TObject newValue => newValue,
                _ => throw new ArgumentException($"{value} needs to be of type {typeof(TObject)}.", string.Empty)
            };
        }

        public TObject UnderlyingValue
        {
            get => m_UnderlyingValue;
            set => m_UnderlyingValue = value;
        }

        public InterfaceReference()
        {
        }

        public InterfaceReference(TObject target) => m_UnderlyingValue = target;

        public InterfaceReference(TInterface @interface) => m_UnderlyingValue = @interface as TObject;

        public static implicit operator TInterface(InterfaceReference<TInterface, TObject> obj) => obj.Value;
    }

    [Serializable]
    public class InterfaceReference<TInterface> : InterfaceReference<TInterface, Object> where TInterface : class
    {
    }
}