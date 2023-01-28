using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataSystem
{
    public class AssetReferenceAttribute : PropertyAttribute
    {
        public System.Type type;

        public AssetReferenceAttribute()
        {
            type = typeof(Asset);
        }

        public AssetReferenceAttribute(System.Type type)
        {
            this.type = type;
        }
    }
}
