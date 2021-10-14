using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICommon : MonoBehaviour
{

    private static readonly string UI_COMMON_KEY = "UI@Common";

    private static UICommon _current = null;

    public static UICommon Current
    {
        get
        {
            if(_current == null)
            {
                _current = FindObjectOfType<UICommon>(true);
                if (_current == null)
                {
                    var common = DataStorage.Instance.GetDataOrNull<GameObject>(UI_COMMON_KEY, null, null);
                    if (common != null)
                    {
                        var instanceObject = Instantiate(common);
                        _current = instanceObject.GetComponent<UICommon>();
                    }
                    else
                    {
                        Debug.LogError("���� UI�� ã�� ���߽��ϴ�. ���α׷��� �����մϴ�");
                        Application.Quit();                           
                    }
                }
                DontDestroyOnLoad(_current.gameObject);
            }
            return _current;
        }
    }

    private List<ICanvas> _canvasList = new List<ICanvas>();

    private void Awake()
    {
        var children = transform.GetComponentsInChildren<ICanvas>(true);
        for(int i = 0; i < children.Length; i++)
        {
            children[i].Initialize();
            _canvasList.Add(children[i]);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _canvasList.Count; i++)
        {
            _canvasList[i].CleanUp();
        }
        _canvasList.Clear();
        _current = null;
    }



    /// <summary>
    /// UICommon ���� UI�� �����ɴϴ�
    /// ������ NullReferenceException�� ��ȯ�մϴ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetUICommon<T>() where T : ICanvas
    {
        for (int i = 0; i < _canvasList.Count; i++)
        {
            if (_canvasList[i] is T)
                return (T)_canvasList[i];
        }
        throw new System.NullReferenceException($"{typeof(T).Name}�� ã�� �� �����ϴ�");
    }

    public bool IsCanvasActivated()
    {
        for (int i = 0; i < _canvasList.Count; i++)
        {
            if (_canvasList[i].isActiveAndEnabled) return true;
        }
        return false;
    }    
    
    public bool IsCanvasActivated<T>() where T : ICanvas
    {
        for (int i = 0; i < _canvasList.Count; i++)
        {
            if (_canvasList[i] is T && _canvasList[i].isActiveAndEnabled) return true;
        }
        return false;
    }

    public void NowCanvasHide()
    {
        for (int i = 0; i < _canvasList.Count; i++)
        {
            if (_canvasList[i].isActiveAndEnabled)
            {
                _canvasList[i].Hide();
                break;
            }
        }
    }

}
