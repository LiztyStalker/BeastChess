using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainTitle : MonoBehaviour
{
    [SerializeField]
    private Text _versionText;

    [SerializeField]
    private Button _challengeButton;

    [SerializeField]
    private Button _mockGameButton;

    [SerializeField]
    private Button _helpButton;

    [SerializeField]
    private Button _creditButton;

    [SerializeField]
    private Button _exitButton;

    private void Awake()
    {
        _challengeButton.onClick.AddListener(OnChallengeClicked);
        _mockGameButton.onClick.AddListener(OnMockGameClicked);
        _helpButton.onClick.AddListener(OnHelpClicked);
        _creditButton.onClick.AddListener(OnCreditClicked);
        _exitButton.onClick.AddListener(OnExitClicked);
    }


    private void Start()
    {
        _versionText.text = Application.version;
    }


    private void OnChallengeClicked()
    {
        LoadManager.SetNextSceneName("Test_MockGame");
        UnityEngine.SceneManagement.SceneManager.LoadScene(LoadManager.LoadSceneName);
    }

    private void OnMockGameClicked()
    {
        LoadManager.SetNextSceneName("Test_MockGame");
        UnityEngine.SceneManagement.SceneManager.LoadScene(LoadManager.LoadSceneName);
    }


    private void OnHelpClicked()
    {
        Debug.Log("Help");
    }


    private void OnCreditClicked()
    {
        Debug.Log("Credit");
    }


    private void OnExitClicked()
    {
        Application.Quit();
    }

}
