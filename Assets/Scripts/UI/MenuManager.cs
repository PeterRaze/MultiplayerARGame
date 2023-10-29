using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [Header("Buttons")]

    [SerializeField]
    private Button singleModeBtn;

    [SerializeField]
    private Button trainingModeBtn;

    [SerializeField]
    private Button multiplayerBtn;

    [SerializeField]
    private Button exitBtn;

    [SerializeField]
    private Button howToPlayBtn;

    [SerializeField]
    private Button closePanelBtn;


    [Header("Panels")]

    [SerializeField]
    private GameObject howToPlayPanel;

    //private readonly string SingleMode = "SingleMode";
    private readonly string SingleMode = "SM_AR_OFF";
    //private readonly string TrainingMode = "TrainingMode";
    private readonly string TrainingMode = "TM_AR_OFF";
    private readonly string Lobby = "Lobby";

    void Start()
    {
        PhotonNetwork.OfflineMode = true;
        singleModeBtn.onClick.AddListener(JoinSingleModeRoom);
        trainingModeBtn.onClick.AddListener(JoinTrainingModeRoom);
        multiplayerBtn.onClick.AddListener(LoadLobbyScene);
        exitBtn.onClick.AddListener(ExitApplication);
        closePanelBtn.onClick.AddListener(OpenHowToPlayPanel);
        howToPlayBtn.onClick.AddListener(OpenHowToPlayPanel);
    }

    private void OpenHowToPlayPanel()
    {
        howToPlayPanel.SetActive(!howToPlayPanel.activeInHierarchy);
    }

    private void ExitApplication()
    {
        Application.Quit();
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.Name == SingleMode)
        {
            PhotonNetwork.LoadLevel(SingleMode);
        }

        if (PhotonNetwork.CurrentRoom.Name == TrainingMode)
        {
            PhotonNetwork.LoadLevel(TrainingMode);
        }

    }

    private void JoinSingleModeRoom()
    {
        PhotonNetwork.JoinRoom(SingleMode);
    }

    private void JoinTrainingModeRoom()
    {
        PhotonNetwork.JoinRoom(TrainingMode);
    }

    private void LoadLobbyScene()
    {
        SceneManager.LoadScene(Lobby);
    }

}
