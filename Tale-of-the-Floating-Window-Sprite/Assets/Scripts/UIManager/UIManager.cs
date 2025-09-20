using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// UI 管理器：负责所有 UI 的注册、显示、隐藏
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        private Dictionary<Type, UIBase> _uiPanels = new Dictionary<Type, UIBase>();

        private void Start()
        {
            var panels = FindObjectsOfType<UIBase>(true); // true 表示包括禁用的对象
            foreach (var panel in panels)
            {
                Register(panel);
            }
            Debug.Log($"[UIManager] 自动注册 {panels.Length} 个 UI");
        }
        public void Register<T>(T panel) where T : UIBase
        {
            var type = panel.GetType(); 
            if (!_uiPanels.ContainsKey(type))
            {
                _uiPanels.Add(type, panel);
                Debug.Log($"[UIManager] 注册 UI: {type.Name}");
            }
        }

        public void Show<T>() where T : UIBase
        {
            if (_uiPanels.TryGetValue(typeof(T), out var panel))
            {
                Debug.Log($"[UIManager] 打开 UI: {typeof(T).Name}");
                panel.Show();
            }
            else
            {
                Debug.LogError($"[UIManager] 找不到 UI: {typeof(T).Name}");
            }
        }

        public void Hide<T>() where T : UIBase
        {
            if (_uiPanels.TryGetValue(typeof(T), out var panel))
            {
                panel.Hide();
                Debug.Log($"[UIManager] 隐藏 UI: {typeof(T).Name}");
            }
        }
    }
}
