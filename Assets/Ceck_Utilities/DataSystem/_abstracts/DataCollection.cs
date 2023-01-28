// Colletion of AssetData.
// Acts like ax extended list that does not allow to have in the list 2 AssetData that refer to the same Asset

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DataSystem
{
    [System.Serializable]
    public abstract class DataCollection<T> where T : AssetData
    {
        [SerializeField] protected List<T> _dataList = new List<T>();

        public Action OnCollectionChanged;

        public ReadOnlyCollection<T> DataList { get => _dataList.AsReadOnly(); }

        public void Remove(Asset asset)
        {
            Remove(Get(asset));
        }

        public void Remove(T assetData)
        {
            _dataList.Remove(assetData);
            assetData.OnDataChanged -= SendCollectionChangedEvent;
            SendCollectionChangedEvent();
        }

        public void Add(T assetData)
        {
            if (!_dataList.Contains(assetData))
            {
                _dataList.Add(assetData);
                assetData.OnDataChanged += SendCollectionChangedEvent;
                SendCollectionChangedEvent();
            }
        }

        public T Get(Asset asset)
        {
            return GetByID(asset.Id);
        }

        public T GetByID(string assetID)
        {
            for (int i = 0; i < _dataList.Count; i++)
            {
                T data = _dataList[i];
                if (data != null && data.Id == assetID)
                {
                    return data;
                }
            }

            return null;
        }

        public void RegisterCollectionEvents()
        {
            for (int i = 0; i < _dataList.Count; i++)
            {
                var data = _dataList[i];

                // you can safely unregister first, and then register again, even if the handler is not registered at all
                data.OnDataChanged -= SendCollectionChangedEvent;
                data.OnDataChanged += SendCollectionChangedEvent;
            }
        }

        [Button]
        protected void SendCollectionChangedEvent()
        {
            if (OnCollectionChanged != null)
                OnCollectionChanged();
        }
    }
}