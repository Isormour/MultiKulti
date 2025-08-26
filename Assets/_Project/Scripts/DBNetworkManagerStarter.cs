using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class DBNetworkManagerStarter : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    List<Lobby> rooms = null;
    Lobby currentLobby = null;
    int maxPlayers = 16;
    bool isLobbyHost = false;
    void Start()
    {
        SetText("Auto Starting.");
        Debug.Log("AutoStarting");
        if (!DBNetworkManager.Instance.isUnityLoggedIn)
        {
            DBNetworkManager.Instance.LowApiLoggedIn += LowApiLoggedIn;
        }
        else
        {
            LowApiLoggedIn();
        }
    }

    private void LowApiLoggedIn()
    {
        SetText("Low api Logged In.");
        Debug.Log("low api getting rooms , or creating server");
        StartCoroutine(Waiter());
        GetRooms();
    }
    private async Task GetRooms()
    {
        var lobbies = await LobbyService.Instance.QueryLobbiesAsync();
        rooms = lobbies.Results;
        Debug.Log("Rooms found: " + rooms.Count);
    }


    void Update()
    {

    }

    public IEnumerator Waiter()
    {
        SetText("Discovering servers..");

        // we have set this as 3.1 seconds, default discovery scan is 3 seconds, allows some time if host and client are started at same time
        yield return new WaitForSeconds(3.1f);
        if (rooms == null || rooms.Count == 0)
        {
            Debug.Log("Starting server");
            StartServer();
        }
        else
        {
            StartClient();
        }
    }
    private async Task StartClient()
    {
        SetText("Server found, starting as Client.");
        Lobby lobby = rooms[0];
        Lobby lob = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id);
        //DBNetworkManager.Instance.relayJoinCode = lob.Data["JoinCode"].Value;
        //DBNetworkManager.Instance.JoinRelayServer();
    }
    async void StartServer()
    {
        SetText("No Servers found, starting as Host.");
        //DBNetworkManager.Instance.StartRelayHost(maxPlayers);
        DBNetworkManager.Instance.OnServerCreated = OnServerCreated;
    }
    async void OnServerCreated()
    {
        DBNetworkManager.Instance.OnServerCreated = null;
        CreateLobbyOptions optionsCreator = new CreateLobbyOptions();
        optionsCreator.Data = new Dictionary<string, DataObject>()
        {
            //{ "JoinCode", new DataObject(DataObject.VisibilityOptions.Member, DBNetworkManager.Instance.relayJoinCode) }
        };
        currentLobby = await LobbyService.Instance.CreateOrJoinLobbyAsync("HostLobby" + Random.Range(0, 1000), "Lobby", maxPlayers, optionsCreator);
        isLobbyHost = true;
        StartCoroutine(HearthBeat());
    }
    private void OnDestroy()
    {
        if (isLobbyHost)
        {
            LobbyService.Instance.DeleteLobbyAsync(currentLobby.Id);
        }
    }

    public void SetText(string text)
    {
        infoText.text = text;
        Debug.Log(text);
    }

    internal void Kill()
    {
        StopAllCoroutines();
    }
    IEnumerator HearthBeat()
    {
        while (true)
        {
            yield return new WaitForSeconds(15);
            LobbyService.Instance.SendHeartbeatPingAsync(currentLobby.Id);
        }
    }
}
