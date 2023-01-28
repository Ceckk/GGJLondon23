using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace DataSystem
{
    public class CustomProperties : OdinAttributeProcessor
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new AssetReferenceAttribute());
        }
    }
}
