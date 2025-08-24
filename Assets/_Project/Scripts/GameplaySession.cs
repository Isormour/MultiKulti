using Mirror;
using System;
using UnityEngine;
public class GameplaySession : NetworkBehaviour
{
    [System.Serializable]
    public struct PlayerData
    {
        public string playerName;
        public int score;
    }

    public enum ESessionState
    {
        None,
        Connecting,
        Lobby,
        InGame,
        Finished
    }

    [SyncVar(hook = nameof(SetState))]
    public ESessionState CurrentState = ESessionState.None;

    public class SyncListPlayerData : SyncList<PlayerData> { }
    public SyncListPlayerData playerList { private set; get; } = new SyncListPlayerData();
    public static GameplaySession Instance { private set; get; }
    public event Action OnGameStarted;
    public GameObject TankPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        playerList.OnChange += OnPlayersChanged;
    }
    void OnPlayersChanged(SyncList<PlayerData>.Operation op, int index, PlayerData newData)
    {
        Debug.Log($"Zmiana w liœcie graczy: {op} na indexie {index}, nowy gracz: {newData.playerName}, punkty: {newData.score}");
    }
    public override void OnStartClient()
    {
        SetState(CurrentState, CurrentState);
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        SetState(CurrentState, ESessionState.Lobby);
    }



    [Server]
    public void AddPlayer()
    {
        string name = PlayerPrefs.GetString(UIPanelLobby.PLAYER_NAME_KEY, "Player" + UnityEngine.Random.Range(1000, 9999));
        playerList.Add(new PlayerData { playerName = name, score = 0 });
    }
    [Server]
    public void StartGame()
    {
        SetState(CurrentState, ESessionState.InGame);
        GameStarted();
        SpawnTanks();
    }
    [Server]
    private void SpawnTanks()
    {
        foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
        {
            GameObject Tank = Instantiate(TankPrefab, Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn(Tank);
            Tank.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
            DBPlayerTank playerTank = Tank.GetComponent<DBPlayerTank>();
        }
    }

    [ClientRpc]
    void GameStarted()
    {
        OnGameStarted?.Invoke();
    }
    public void EndGame()
    {
        playerList.Clear();
    }
    void SetState(ESessionState oldState, ESessionState newState)
    {
        CurrentState = newState;
        switch (newState)
        {
            case ESessionState.Lobby:
                UIManager.Instance.SetUIPanel(UIManager.EUIPanel.Lobby);
                UIManager.Instance.Lobby.SetIsHost(isServer);
                break;
            case ESessionState.InGame:
                UIManager.Instance.SetUIPanel(UIManager.EUIPanel.InGame);
                break;
            case ESessionState.Finished:
                UIManager.Instance.SetUIPanel(UIManager.EUIPanel.Finish);
                break;
            default:
                UIManager.Instance.SetUIPanel(UIManager.EUIPanel.None);
                break;
        }
        DBDebugText.Instance.SetText($"Stan gry: {newState}");
    }

}

