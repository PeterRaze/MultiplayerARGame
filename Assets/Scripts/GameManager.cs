using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameMode
    {
        Training,
        Single,
        Multiplayer
    }

    [SerializeField]
    private Player player;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private Limits limits;

    [SerializeField]
    private GameMode gameMode;

    private List<Agent> agents = new List<Agent>();
    private List<TargetObject> targetObjects = new List<TargetObject>();

    private int targetObjectAmount;
    private int agentsAmount;

    private Color playerColor;

    private float minX, minZ, maxX, maxZ;

    private bool isDisconnecting;

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

    private void Start()
    {
        switch (gameMode)
        {
            case GameMode.Training:
                InitializeTrainingMode();
                break;
            case GameMode.Single:
                InitializeSingleMode();
                break;
            case GameMode.Multiplayer:
                isDisconnecting = false;
                SetPlayerColor();
                InitializeMultiplayerMode();
                break;
        }
    }

    #region Multiplayer Mode

    public void Disconnect()
    {
        isDisconnecting = true;
    }

    private void SetPlayerColor()
    {
        playerColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    private void InitializeMultiplayerMode()
    {
        minX = limits.GetTransformExtentions(Limits.TransformExtention.Axis.MinX);
        maxX = limits.GetTransformExtentions(Limits.TransformExtention.Axis.MaxX);
        minZ = limits.GetTransformExtentions(Limits.TransformExtention.Axis.MinZ);
        maxZ = limits.GetTransformExtentions(Limits.TransformExtention.Axis.MaxZ);

        CreatePlayer();
    }

    public void CreatePlayer()
    {
        if (PhotonNetwork.IsConnected)
        {
            StartCoroutine(CreatePlayerInstance());
        }
    }

    private IEnumerator CreatePlayerInstance()
    {
        yield return new WaitForSeconds(1f);

        var playerInstance = PhotonNetwork.Instantiate(player.gameObject.name, new Vector3(Random.Range(minX, maxX), player.transform.position.y, Random.Range(minZ, maxZ)), Quaternion.identity)
            .GetComponent<Player>();

        playerInstance.transform.SetParent(target);
        playerInstance.onPlayerDestruction += ActiveDiedPanel;
        playerInstance.SetColor(playerColor);
    }

    private void ActiveDiedPanel()
    {
        if (PhotonNetwork.IsConnected && !isDisconnecting)
        {
            GamePlayUI.Instance?.ShowUI(true);
            GamePlayUI.Instance?.SetTitle("You Died");
        }
    }

    #endregion

    #region Training Mode

    private void InitializeTrainingMode()
    {
        targetObjects = new List<TargetObject>(FindObjectsOfType<TargetObject>());
        targetObjectAmount = targetObjects.Count;

        foreach (var targetObject in targetObjects)
        {
            targetObject.onTargetDestruction += ReduceTargetAmount;
        }
    }

    private void ReduceTargetAmount()
    {
        targetObjectAmount--;

        if (targetObjectAmount == 0)
        {
            Finish("Congratulations!");
        }
    }
    
    #endregion

    #region Single Mode

    private void InitializeSingleMode()
    {
        player.onPlayerDestruction += () => Finish("You Died");

        agents = new List<Agent>(FindObjectsOfType<Agent>());
        agentsAmount = agents.Count;

        foreach (var agent in agents)
        {
            agent.onAgentDestruction += ReduceAgentAmount;
        }
    }

    private void ReduceAgentAmount()
    {
        agentsAmount--;

        if (agentsAmount == 0)
        {
            Finish("Victory Achieved");
        }
    }

    #endregion


    private void Finish(string title)
    {
        GamePlayUI.Instance.SetTitle(title);
        GamePlayUI.Instance.ShowUI(true);
    }

}
