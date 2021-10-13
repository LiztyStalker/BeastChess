using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadManager
{
    private static string _nextSceneName;
    public static string NextSceneName => _nextSceneName;
    public readonly static string LoadSceneName = "Test_Load";
    public static void SetNextSceneName(string nextSceneName)
    {
        _nextSceneName = nextSceneName;
    }
    public static void Dispose()
    {
        _nextSceneName = null;
    }
}

public class UILoad : MonoBehaviour
{

    [SerializeField]
    private Slider _loadSlider;

    [SerializeField]
    private Text _tipText;

    [SerializeField]
    private Image _loadImage;


    private void Start()
    {
        _tipText.text = "Test";
        _loadImage.sprite = null;        
        StartCoroutine(LoadSceneCoroutine(LoadManager.NextSceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName) 
    {
        var nextSceneName = sceneName;

        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("����� �� �̸��� �����ϴ�. ó������ ���ư��ϴ�.");
            _tipText.text = "����� �� �̸��� �����ϴ�. ó������ ���ư��ϴ�.";
            nextSceneName = "Test_MainTitle";
            yield return new WaitForSeconds(1f);
        }

        var async = SceneManager.LoadSceneAsync(nextSceneName);
        while (!async.isDone)
        {
            _loadSlider.value = async.progress;
            yield return null;
        }
        LoadManager.Dispose();
    }


}
