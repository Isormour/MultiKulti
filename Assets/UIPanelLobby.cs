using Mirror;
using TMPro;
using UnityEngine;

public class UIPanelLobby : MonoBehaviour
{

    public const string PLAYER_NAME_KEY = "PLAYER_NAME_KEY";
    bool isHost = false;
    [SerializeField] GameObject ButtonStartGame;
    [SerializeField] TMP_InputField InputFieldPlayerName;
    [SerializeField] GameplaySession gameSession;
    [SerializeField] TextMeshProUGUI labelScores;

    public void SetIsHost(bool isHost)
    {
        this.isHost = isHost;
        ButtonStartGame.SetActive(isHost);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputFieldPlayerName.SetTextWithoutNotify(PlayerPrefs.GetString(PLAYER_NAME_KEY, "Player" + Random.Range(1000, 9999)));
        InputFieldPlayerName.onValueChanged.AddListener((value) =>
        {
            PlayerPrefs.SetString(PLAYER_NAME_KEY, value);
            PlayerPrefs.Save();
        });

        gameSession.playerList.OnChange += OnPlayersChanged;
    }

    private void OnPlayersChanged(SyncList<GameplaySession.PlayerData>.Operation operation, int arg2, GameplaySession.PlayerData data)
    {
        string scoreText = "";
        foreach (var item in gameSession.playerList)
        {
            scoreText += $"{item.playerName} : {item.score}\n";
        }
        labelScores.text = scoreText;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
