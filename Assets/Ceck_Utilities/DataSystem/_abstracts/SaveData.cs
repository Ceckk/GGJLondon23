using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

namespace DataSystem
{
    [System.Serializable]
    public abstract class SaveData
    {
        public const string VERSION = "_version";
        public const string DATA = "_data";

        /// <summary>
        /// Change this number if there are some structural changes and the data need to be parsed in a different way
        /// </summary>
        public abstract int GetCurrentDataVersion();

        public virtual string Serialize()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add(VERSION, GetCurrentDataVersion());
            dictionary.Add(DATA, JsonUtility.ToJson(this));
            return Json.Serialize(dictionary);
        }

        public virtual bool Deserialize(string serializedData, bool force = false)
        {
            if (!string.IsNullOrEmpty(serializedData))
            {
                var json = Json.Deserialize(serializedData) as Dictionary<string, object>;
                var versionNumber = (long)json[VERSION];

                // LogManager.Log(string.Format("Saved version: {0}. Current version: {1}", versionNumber, GetCurrentDataVersion()));
                if (versionNumber == GetCurrentDataVersion())
                {
                    JsonUtility.FromJsonOverwrite((string)json[DATA], this);
                    return true;
                }
            }

            return false;
        }

        protected T GetOrCreate<T>(ref T t) where T : new()
        {
            if (t == null)
            {
                t = new T();
            }

            return t;
        }
    }
}
