using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// ͨ�� UI ���� + �����¼�ϵͳ
    /// </summary>
    public abstract class UIBase : MonoBehaviour
    {
        // === UI �������� ===
        protected virtual void Awake()
        {
            GlobalGameManager.GlobalManager.Instance.uiManager.Register(this);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            OnShow();
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            OnHide();
        }

        protected virtual void OnShow() { }
        protected virtual void OnHide() { }

        // === �¼�ϵͳ ===
        private static Dictionary<string, Action<object>> _eventDic = new();

        public static void Subscribe(string key, Action<object> callback)
        {
            if (!_eventDic.ContainsKey(key))
                _eventDic[key] = delegate { };
            _eventDic[key] += callback;
        }

        public static void Unsubscribe(string key, Action<object> callback)
        {
            if (_eventDic.ContainsKey(key))
                _eventDic[key] -= callback;
        }

        public static void Publish(string key, object data = null)
        {
            if (_eventDic.ContainsKey(key))
                _eventDic[key]?.Invoke(data);
        }
    }
}
