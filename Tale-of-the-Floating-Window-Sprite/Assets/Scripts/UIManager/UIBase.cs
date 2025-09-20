using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// UI 基类，所有 UI 面板都继承自它
    /// </summary>
    public abstract class UIBase : MonoBehaviour
    {

        protected virtual void Awake()
        {
            Debug.Log($"[UIBase] Awake: {GetType().Name}");
            // 注册到 UIManager
            GlobalGameManager.GlobalManager.Instance.uiManager.Register(this);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            OnShow();
        }

        public virtual void Hide()
        {
            OnHide();
            gameObject.SetActive(false);
        }

        protected virtual void OnShow() { }
        protected virtual void OnHide() { }
    }
}
