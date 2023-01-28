using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DataSystem
{
    public abstract class DataBase : ScriptableObject
    {
        private const string OBSOLETE_FOLDER_NAME = "_obsolete";
        private const string OBSOLETE_PREFIX = "_obsolete_";

        protected virtual string ID { get => "_id"; }
        protected virtual string IMPORT { get => "_import"; }

        [SerializeField] protected TextAsset[] _csvTextAssets;
        [SerializeField] protected List<Asset> _assets = new List<Asset>();

        public ReadOnlyCollection<Asset> Assets { get => _assets != null ? _assets.AsReadOnly() : null; }
        public TextAsset[] CSVTextAssets { get => _csvTextAssets; }

        public static Asset GetAssetById(string id)
        {
            var databases = Resources.LoadAll<DataBase>("");

            foreach (var db in databases)
            {
                foreach (var asset in db.Assets)
                {
                    if (asset.Id == id)
                    {
                        return asset;
                    }
                }
            }

            return null;
        }


        [Button]
        protected virtual void Sync()
        {
            ParseTextAssets(_csvTextAssets);
        }

        public void ParseTextAssets(TextAsset[] textAssetArray)
        {
            var backup = new List<Asset>(_assets);

            try
            {
                var result = new List<Dictionary<string, string>>();
                foreach (var t in textAssetArray)
                {
                    result = result.Concat(TextAssetParser.ReadAsCSV(t)).ToList();
                }

                LogManager.Log(string.Format("STARTED: Parsing of {0}", name));
                RemoveObsoleteAssets(result);
                UpdateOrCreateAssets(result);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
                LogManager.Log(string.Format("ENDED: Parsing of {0}", name));
            }
            catch (Exception e)
            {
                _assets = backup;
                LogManager.LogException(e);
            }
        }

        protected void RemoveObsoleteAssets(List<Dictionary<string, string>> result)
        {
            var idList = new List<string>();

            foreach (var entry in result)
            {
                var import = !entry.ContainsKey(IMPORT) || entry[IMPORT].ConvertToBool(true);
                if (import)
                {
                    idList.Add((string)entry[ID]);
                }
            }

            var filteredAssets = new List<Asset>();

            foreach (var asset in _assets)
            {
                if (idList.Contains(asset.Id))
                {
                    filteredAssets.Add(asset);
                }
                else
                {
                    MoveAssetToObsoleteFolder(asset);
                }
            }

            _assets = filteredAssets;
        }

        protected void MoveAssetToObsoleteFolder(Asset asset)
        {
#if UNITY_EDITOR
            var assetPath = UnityEditor.AssetDatabase.GetAssetPath(asset);
            var assetFolder = GetAssetsFolder();
            var assetName = System.IO.Path.GetFileName(assetPath);
            var targetFolder = string.Format("{0}{1}{2}{1}", assetFolder, System.IO.Path.DirectorySeparatorChar, OBSOLETE_FOLDER_NAME);


            if (!System.IO.Directory.Exists(targetFolder))
            {
                UnityEditor.AssetDatabase.CreateFolder(assetFolder, OBSOLETE_FOLDER_NAME);
            }

            var result = UnityEditor.AssetDatabase.MoveAsset(assetPath, targetFolder + OBSOLETE_PREFIX + assetName);
            if (!string.IsNullOrEmpty(result))
            {
                LogManager.LogError(result);
            }
            else
            {
                LogManager.Log(string.Format("Moved {0} to {1} folder", assetName, OBSOLETE_FOLDER_NAME));
            }
#endif
        }

        protected abstract void UpdateOrCreateAssets(List<Dictionary<string, string>> result);

        protected string GetAssetsFolder()
        {
            return string.Format("Assets{0}ScriptableObject", System.IO.Path.DirectorySeparatorChar);
        }

        [Button]
        private void GenerateIdFromName()
        {
            for (int i = 0; i < _assets.Count; i++)
            {
                var asset = _assets[i];
                asset.Id = asset.name;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(asset);
#endif
            }
        }

    }

    public abstract class DataBase<T> : DataBase where T : Asset
    {
        private static DataBase<T> _instance;

        public static DataBase<T> Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.LoadAll<DataBase<T>>("")[0];
                }

                return _instance;
            }
        }

        public new T GetAssetById(string id)
        {
            for (int i = 0; i < _assets.Count; i++)
            {
                if (_assets[i].Id == id)
                    return _assets[i] as T;
            }

            return null;
        }

#if UNITY_EDITOR
        protected T CreateAsset(string assetName)
        {
            try
            {
                var asset = ScriptableObject.CreateInstance<T>();
                var assetPath = string.Format("{0}{1}{2}.asset", GetAssetsFolder(), System.IO.Path.DirectorySeparatorChar, assetName);
                UnityEditor.AssetDatabase.CreateAsset(asset, assetPath);
                LogManager.Log(string.Format("Created {0} inside {1} folder", assetName, GetAssetsFolder()));
                return asset;
            }
            catch (Exception e)
            {
                LogManager.LogException(e);
                return null;
            }
        }
#endif
    }
}
