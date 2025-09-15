using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Game.UI;
using System;

public class UIManager : MonoBehaviour
{
  
    private Dictionary<Type, UIBase> _uiPanels = new Dictionary<Type, UIBase>();
  
    // ע�����
    public void Register<T>(T panel) where T : UIBase
    {
        var type = typeof(T);
        if (!_uiPanels.ContainsKey(type))
            _uiPanels.Add(type, panel);
    }

    // �����
    public void Show<T>() where T : UIBase
    {
        if (_uiPanels.TryGetValue(typeof(T), out var panel))
            panel.Show();
    }

    // �ر����
    public void Hide<T>() where T : UIBase
    {
        if (_uiPanels.TryGetValue(typeof(T), out var panel))
            panel.Hide();
    }

}
