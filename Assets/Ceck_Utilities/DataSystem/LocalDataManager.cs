using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace DataSystem
{
    public static class LocalDataManager
    {
        public static void Save(string key, string serializedData)
        {
            var save = new Save(serializedData);
            var bf = new BinaryFormatter();
            var file = File.Create(GetFilePath(key));
            bf.Serialize(file, save);
            file.Close();

            LogManager.Log(string.Format("SAVED {0}: {1}", key, serializedData));
        }

        public static string Load(string key)
        {
            if (File.Exists(GetFilePath(key)))
            {
                var bf = new BinaryFormatter();
                var file = File.Open(GetFilePath(key), FileMode.Open);
                Save save = (Save)bf.Deserialize(file);
                file.Close();

                LogManager.Log(string.Format("LOADED {0}: {1}", key, save.serializedData));

                return save.serializedData;
            }
            else
            {
                return null;
            }
        }

        public static void Delete(string key)
        {
            File.Delete(GetFilePath(key));
            LogManager.Log(string.Format("DELETED {0}", key));
        }

        private static string GetFilePath(string key)
        {
            return string.Format("{0}/{1}.save", Application.persistentDataPath, key);
        }
    }
}
