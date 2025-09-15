using UnityEngine;
namespace GloobalGameManager
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
        [Header("管理器引用")]public FountainManager fountainManager;
        public SpiritGameManager spiritGameManager;
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
            // 初始化各个管理器
            if (fountainManager == null)
                fountainManager = gameObject.AddComponent<FountainManager>();
            if(spiritGameManager==null)
                spiritGameManager=gameObject.AddComponent<SpiritGameManager>();

        }
    }
}
