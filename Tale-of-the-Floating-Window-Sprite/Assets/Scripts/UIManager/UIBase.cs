using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 通用 UI 基类 + 简易事件系统
    /// </summary>
    public abstract class UIBase : MonoBehaviour
    {
        // === UI 生命周期 ===
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

        // === 事件系统 ===
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
