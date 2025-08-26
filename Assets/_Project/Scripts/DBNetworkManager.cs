using Mirror;
using Mirror.WebRTC;
using System;
using System.Collections;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class DBNetworkManager : NetworkManager

{
    [SerializeField] GameplaySession tankSession;
    [SerializeField] RTCWebSocketSignaler webSocketSignaler;
    public bool isUnityLoggedIn { private set; get; } = false;
    public static DBNetworkManager Instance { get; private set; }
    public event Action LowApiLoggedIn;
    public Action OnServerCreated;

    public override void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
            StartCoroutine(LowLevelLoginCorr(LoginToWebSocketSerwer));
            //UnityLogin();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void LoginToWebSocketSerwer()
    {
        webSocketSignaler.Connect("Player" + UnityEngine.Random.Range(0, 100));
    }
    IEnumerator LowLevelLoginCorr(Action loginMeth)
    {
        loginMeth();
        yield return new WaitForSeconds(0.2f);
        LowApiLoggedIn?.Invoke();
    }
    public override void OnStartServer()
    {
        OnServerCreated?.Invoke();
        base.OnStartServer();
        Debug.Log("Server started");
        tankSession.gameObject.SetActive(true);
    }
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        tankSession.gameObject.SetActive(true);
    }
    private async void UnityLogin()
    {
        Debug.Log("Try Unity Login");
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Logged into Unity, player ID: " + AuthenticationService.Instance.PlayerId);
            isUnityLoggedIn = true;
            LowApiLoggedIn?.Invoke();

            //StartCoroutine(OnLoggedIn());
        }
        catch (Exception e)
        {
            isUnityLoggedIn = false;
            Debug.LogError(e);
        }
    }
    IEnumerator OnLoggedIn()
    {
        yield return new WaitForSeconds(0.2f);

    }
}
