using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

[ExecuteAlways]

public class TranslatorStorage
{
    [ExecuteAlways]
    private static TranslatorStorage _instance = null;

    public static TranslatorStorage Instance
    {
        get
        {
            if (_instance == null)
                _instance = new TranslatorStorage();
            return _instance;
        }
    }

    Dictionary<string, JsonData> _dic = new Dictionary<string, JsonData>();
    

    //���� ������ ��������
    public string GetTranslator<T>(string key, string verb)
    {
        if (!_dic.ContainsKey(typeof(T).Name))
        {
            var textAsset = DataStorage.Instance.GetDataOrNull<TextAsset>(typeof(T).Name, null, "Translate");
            _dic.Add(typeof(T).Name, JsonMapper.ToObject(textAsset.text));
        }


        if (_dic.ContainsKey(typeof(T).Name))
        {
            var dic = _dic[typeof(T).Name];
            if (dic.ContainsKey(key))
            {
                var jData = dic[key];
                if (jData.ContainsKey(SystemLanguage.Korean.ToString() + "_" + verb))
                    return jData[(SystemLanguage.Korean.ToString() + "_" + verb)].ToString();
            }
        }
        return null;
    }
}
