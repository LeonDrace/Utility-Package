using System;
using System.Text;
using UnityEngine;

namespace LeonDrace.Utility
{
    [Serializable]
    public class SerializableGuid : IEquatable<SerializableGuid>
    {
        [SerializeField] private string m_Id;
        public string Id => m_Id;

        public SerializableGuid() => m_Id = Guid.NewGuid().ToString();
        public SerializableGuid(Guid guid) => m_Id = guid.ToString();
        public SerializableGuid(string id) => m_Id = id;
        
        public static SerializableGuid Empty => new(string.Empty);
        public static SerializableGuid NewGuid() => new(Guid.NewGuid().ToString());
        public static string NewGuidAsString() => Guid.NewGuid().ToString();
        
        public static bool operator ==(SerializableGuid left, SerializableGuid right) => !ReferenceEquals(left, null) && left.Equals(right);
        public static bool operator !=(SerializableGuid left, SerializableGuid right) => !(left == right); 
        
        public Guid ToGuid() => Guid.Parse(m_Id);
        public byte[] ToByteArray() => Encoding.ASCII.GetBytes(m_Id);
        public bool Equals(SerializableGuid other)
        {
            if(ReferenceEquals(null, other)) return false;
            if(ReferenceEquals(this, other)) return true;
            return other.Id == m_Id;
        }
        public override int GetHashCode() => Id.GetHashCode();
        public override bool Equals(object obj) => obj is SerializableGuid guid && Equals(guid);
    }
}
