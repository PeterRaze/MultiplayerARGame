using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
{
    [SerializeField]
    private GameObject canvasUI;

    [SerializeField]
    private Text title;

    [SerializeField]
    private Button leaveBtn;

    [SerializeField]
    private Button playAgainBtn;

    [SerializeField]
    private GameManager.GameMode gameMode;

    public static GamePlayUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        switch (gameMode)
        {
            case GameManager.GameMode.Training:
            case GameManager.GameMode.Single:
                leaveBtn.onClick.AddListener(LeaveMode);
                playAgainBtn.onClick.AddListener(PlayAgain);
                break;
            case GameManager.GameMode.Multiplayer:
                leaveBtn.onClick.AddListener(BackToMenu);
                playAgainBtn.onClick.AddListener(Respawn);
                break;
        }
        
    }

    #region Multiplayer

    private void Respawn()
    {
        GameManager.Instance.CreatePlayer();
        ShowUI(false);
    }

    private void BackToMenu()
    {
        StartCoroutine(Disconnect());
    }

    private IEnumerator Disconnect()
    {
        GameManager.Instance.Disconnect();

        PhotonNetwork.Disconnect();

        yield return new WaitUntil(() => PhotonNetwork.IsConnected == false);

        SceneManager.LoadScene("Menu");
    }


    #endregion

    private void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LeaveMode()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Menu");
    }

    public void ShowUI(bool show)
    {
        if (canvasUI != null)
        {
           canvasUI.SetActive(show);
        }
    }

    public void SetTitle(string title)
    {
        if (this.title != null)
        {
            this.title.text = title;
        }
    }
}
