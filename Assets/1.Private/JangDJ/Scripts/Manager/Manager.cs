using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager _instance { get; set; }
    [SerializeField] public GameObject Cursor;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        if(_instance == null)
        {
            _instance = FindAnyObjectByType<Manager>();

            if (_instance == null)
                _instance = Instantiate(Resources.Load<Manager>("Manager"));

            DontDestroyOnLoad(_instance);

            _instance.InitManagers();
            _instance.ProgramSetting();
        }
    }

    private void Awake()
    {
        if (_instance != this && _instance != null)
        {
            Destroy(this.gameObject);
        }
    }

    private void InitManagers()
    {
        IManager[] managers = GetComponents<IManager>();

        foreach (var manager in managers)
        {
            manager.Init();
        }
    }

    private void ProgramSetting()
    {
        Application.targetFrameRate = 60;
    }

    public static void SetCursorVisible(bool active)
    {
        Manager._instance.Cursor.SetActive(active);
    }
}