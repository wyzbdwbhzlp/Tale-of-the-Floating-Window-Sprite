using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// UI ���࣬���� UI ��嶼�̳�����
    /// </summary>
    public abstract class UIBase : MonoBehaviour
    {

        protected virtual void Awake()
        {
            Debug.Log($"[UIBase] Awake: {GetType().Name}");
            // ע�ᵽ UIManager
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
