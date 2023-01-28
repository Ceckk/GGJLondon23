using UnityEngine;

namespace DataSystem
{
    [System.Serializable]
    public struct Save
    {
        [SerializeField] public string serializedData;

        public Save(string serializedData)
        {
            this.serializedData = serializedData;
        }
    }
}
