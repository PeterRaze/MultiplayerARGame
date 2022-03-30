using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System.Collections;
using UnityEngine.SceneManagement;

public class ServerManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject loadingPanel;

    [SerializeField]
    private InputField roomsName;

    [SerializeField]
    private Button joinRoomBtn;

    [SerializeField]
    private Button createRoomBtn;

    [SerializeField]
    private Button backToMenuBtn;

    [SerializeField]
    private Button tryAgainBtn;

    [SerializeField]
    private Button leaveBtn;

    [SerializeField]
    private Text loadingText;

    [SerializeField]
    private Text errorMessage;

    private void Start()
    {
        PhotonNetwork.OfflineMode = false;
        PhotonNetwork.ConnectUsingSettings();

        createRoomBtn.onClick.AddListener(CreateRoom);
        joinRoomBtn.onClick.AddListener(JoinRoom);
        backToMenuBtn.onClick.AddListener(BackToMenu);
        leaveBtn.onClick.AddListener(BackToMenu);
        tryAgainBtn.onClick.AddListener(TryAgain);
    }

    private void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void BackToMenu()
    {
        StartCoroutine(Disconnect());
    }

    private IEnumerator Disconnect()
    {
        PhotonNetwork.Disconnect();

        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }

        SceneManager.LoadScene("Menu");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        loadingPanel.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Multiplayer");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ShowErrorMessage(message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        ShowErrorMessage(message);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        ShowErrorMessage("Failed to Connect");

        loadingText.text = "";

        tryAgainBtn.gameObject.SetActive(true);
        leaveBtn.gameObject.SetActive(true);
    }

    #region Create/Join Rooms

    private void CreateRoom()
    {
        if (!IsRoomNameEmpty())
        { 
            PhotonNetwork.CreateRoom(roomsName.text);
            
        }
    }

    private void JoinRoom()
    {
        if (!IsRoomNameEmpty())
        {
            PhotonNetwork.JoinRoom(roomsName.text);
        }
    }

    private bool IsRoomNameEmpty()
    {
        if (string.IsNullOrEmpty(roomsName.text))
        {
            ShowErrorMessage("Name's room cannot be empty");
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    private void ShowErrorMessage(string message)
    {
        errorMessage.text = message;
    }

}
