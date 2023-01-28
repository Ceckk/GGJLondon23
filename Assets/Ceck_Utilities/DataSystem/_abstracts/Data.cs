using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DataSystem
{
    public abstract class Data
    {
        private bool _dataLocked = false;
        public Action OnDataChanged;

        [Button]
        protected void SendDataChangedEvent()
        {
            _dataLocked = true;
            if (OnDataChanged != null)
                OnDataChanged();
            _dataLocked = false;
        }

        protected bool CanWrite()
        {
            if (_dataLocked)
            {
                Debug.LogError("Cannot write data while locked");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
