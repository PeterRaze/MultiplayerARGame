using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ServerManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject loadingPanel;

    [SerializeField]
    private InputField roomsName;

    [SerializeField]
    private Button btnJoinRoom;

    [SerializeField]
    private Button btnCreateRoom;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();

        btnCreateRoom.onClick.AddListener(CreateRoom);
        btnJoinRoom.onClick.AddListener(JoinRoom);
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
        PhotonNetwork.LoadLevel("Game");
    }

    #region Create/Join Rooms

    private void CreateRoom()
    {
        PhotonNetwork.CreateRoom(roomsName.text);
    }

    private void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomsName.text);
    }

    #endregion

}
