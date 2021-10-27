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

        _versionText.text = Application.version;
    }

    private void OnDestroy()
    {
        _challengeButton.onClick.RemoveListener(OnChallengeClicked);
        _mockGameButton.onClick.RemoveListener(OnMockGameClicked);
        _helpButton.onClick.RemoveListener(OnHelpClicked);
        _creditButton.onClick.RemoveListener(OnCreditClicked);
        _exitButton.onClick.RemoveListener(OnExitClicked);

        _versionText.text = Application.version;
    }


    private void Start()
    {
        AudioManager.ActivateAudio("BGMMain", AudioManager.TYPE_AUDIO.BGM, true);
        BattleFieldOutpost.Dispose();
    }


    private void OnChallengeClicked()
    {
        AudioManager.InactiveAudio("BGMMain", AudioManager.TYPE_AUDIO.BGM);
        BattleFieldOutpost.InitializeBattleFieldOutpost();
        BattleFieldOutpost.Current.SetChallenge(true);
        BattleFieldOutpost.Current.ClearChallengeLevel();
        LoadManager.SetNextSceneName("Test_MockGame");
        UnityEngine.SceneManagement.SceneManager.LoadScene(LoadManager.LoadSceneName);
    }

    private void OnMockGameClicked()
    {
        AudioManager.InactiveAudio("BGMMain", AudioManager.TYPE_AUDIO.BGM);
        BattleFieldOutpost.InitializeBattleFieldOutpost();
        BattleFieldOutpost.Current.SetChallenge(false);
        BattleFieldOutpost.Current.ClearChallengeLevel();
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
        var ui = UICommon.Current.GetUICommon<UIPopup>();
        ui.ShowOkAndCancelPopup("정말로 종료하시겠습니까?", "종료", "취소", delegate { Application.Quit(); }, null);
    }

}
