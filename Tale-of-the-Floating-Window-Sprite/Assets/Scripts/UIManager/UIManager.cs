using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Game.UI;
using System;

public class UIManager : MonoBehaviour
{
  
    private Dictionary<Type, UIBase> _uiPanels = new Dictionary<Type, UIBase>();
  
    // 注册面板
    public void Register<T>(T panel) where T : UIBase
    {
        var type = typeof(T);
        if (!_uiPanels.ContainsKey(type))
            _uiPanels.Add(type, panel);
    }

    // 打开面板
    public void Show<T>() where T : UIBase
    {
        if (_uiPanels.TryGetValue(typeof(T), out var panel))
            panel.Show();
    }

    // 关闭面板
    public void Hide<T>() where T : UIBase
    {
        if (_uiPanels.TryGetValue(typeof(T), out var panel))
            panel.Hide();
    }

}
