using UnityEngine;

namespace GlobalGameManager
{
    public class GlobalManager : MonoBehaviour
    {
        private static GlobalManager _instance;
        public static GlobalManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GlobalManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("GlobalManager");
                        _instance = go.AddComponent<GlobalManager>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        [Header("管理器引用")]
        public FountainManager fountainManager;
        public SpiritGameManager spiritGameManager;
        public CurrencyManager currencyManager;
        public UIManager uiManager;
        public SpiritProductionManager spiritProductionManager;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeManagers();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void InitializeManagers()
        {
            if (fountainManager == null)
                fountainManager = gameObject.AddComponent<FountainManager>();

            if (spiritGameManager == null)
                spiritGameManager = gameObject.AddComponent<SpiritGameManager>();

            if (currencyManager == null)
                currencyManager = gameObject.AddComponent<CurrencyManager>();

            if (uiManager == null)
                uiManager = gameObject.AddComponent<UIManager>();
            if(spiritProductionManager==null)
                spiritProductionManager =gameObject.AddComponent<SpiritProductionManager>();
        }
    }
}
