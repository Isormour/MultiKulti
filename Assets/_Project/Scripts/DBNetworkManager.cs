using System;
using System.Collections;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Utp;

public class DBNetworkManager : RelayNetworkManager
{
    [SerializeField] GameplaySession tankSession;
    public bool isUnityLoggedIn { private set; get; } = false;
    public static DBNetworkManager Instance { get; private set; }
    public event Action OnUnityLoggedIn;
    public Action OnServerCreated;
    public override void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
            UnityLogin();
        }
        else
        {
            Destroy(gameObject);
        }
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
        Debug.LogError("Try Unity Login");
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.LogError("Logged into Unity, player ID: " + AuthenticationService.Instance.PlayerId);
            isUnityLoggedIn = true;
            OnUnityLoggedIn?.Invoke();

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
