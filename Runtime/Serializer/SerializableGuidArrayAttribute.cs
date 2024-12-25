using LeonDrace.Utility;
using UnityEngine;

namespace Editor.Serializer
{
    public class SerializableGuidArrayAttribute : PropertyAttribute
    {
        public readonly string GuidPath;

        /// <summary>
        /// Provide the elements <see cref="SerializableGuid"/> fields name which functions as a path to search for it and
        /// compare its id to assure when a new element is added it gets a unique id and is not duplicated as by default.
        /// </summary>
        /// <param name="guidPath"></param>
        public SerializableGuidArrayAttribute(string guidPath)
        {
            GuidPath = guidPath;
        }
    }
}