using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum EUIPanel
    {
        None,
        Connecting,
        Lobby,
        JoinGame,
        InGame,
        Finish
    }

    [SerializeField] GameObject[] UIPanels;
    [field: SerializeField] public UIPanelLobby Lobby { private set; get; }

    public static UIManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SetUIPanel(EUIPanel.Connecting);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public EUIPanel CurrentUIPanel = EUIPanel.None;
    public void SetUIPanel(EUIPanel panel)
    {
        if (CurrentUIPanel == panel) return;
        foreach (var uiPanel in UIPanels)
        {
            uiPanel.SetActive(false);
        }
        CurrentUIPanel = panel;
        if (CurrentUIPanel != EUIPanel.None)
        {
            UIPanels[(int)CurrentUIPanel - 1].SetActive(true);
        }
    }

}
