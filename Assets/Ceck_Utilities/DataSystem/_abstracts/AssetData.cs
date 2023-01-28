using Sirenix.OdinInspector;
using UnityEngine;

namespace DataSystem
{
    public abstract class AssetData : Data
    {
        [SerializeField, ReadOnly, AssetReference] protected string _id;
        public string Id { get => _id; }

        public Asset Asset
        {
            get
            {
                return DataBase.GetAssetById(_id);
            }
        }

        public AssetData(string id)
        {
            _id = id;
        }
    }

    [System.Serializable]
    public abstract class AssetData<T> : AssetData where T : Asset
    {
        public new T Asset
        {
            get
            {
                return DataBase<T>.Instance.GetAssetById(_id);
            }
        }

        public AssetData(T asset) : base(asset.Id)
        {
        }
    }
}