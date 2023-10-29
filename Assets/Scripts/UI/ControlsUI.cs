using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlsUI : MonoBehaviour
{
    [SerializeField]
    private Button menuBtn; 

    void Start()
    {
        menuBtn.onClick.AddListener(BackToMenu);
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

}
