using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR;
using static OpenAISpeechToText;

[RequireComponent(typeof(BaiduSettings))]
public class BaiduTextToSpeech : TTS
{
    #region ����
    /// <summary>
    /// token�ű�
    /// </summary>
    [SerializeField] private BaiduSettings m_Settings;
    /// <summary>
    /// �����ϳ�����
    /// </summary>
    [SerializeField] private PostDataSetting m_Post_Setting;
    #endregion

    private void Awake()
    {
        m_Settings = this.GetComponent<BaiduSettings>();
        m_PostURL = "http://tsn.baidu.com/text2audio";
    }

    #region Public Method


    /// <summary>
    /// �����ϳɣ����غϳ��ı�
    /// </summary>
    /// <param name="_msg"></param>
    /// <param name="_callback"></param>
    public override void Speak(string _msg, Action<AudioClip, string> _callback)
    {
        StartCoroutine(GetSpeech(_msg, _callback));
    }

    #endregion

    #region Private Method

    /// <summary>
    /// �����ϳɵķ���
    /// </summary>
    /// <param name="_msg"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator GetSpeech(string _msg, Action<AudioClip, string> _callback)
    {
        stopwatch.Restart();
        var _url = m_PostURL;
        var _postParams = new Dictionary<string, string>();
        _postParams.Add("tex", _msg);
        _postParams.Add("tok", m_Settings.m_Token);
        _postParams.Add("cuid", SystemInfo.deviceUniqueIdentifier);
        _postParams.Add("ctp", m_Post_Setting.ctp);
        _postParams.Add("lan", m_Post_Setting.lan);
        _postParams.Add("spd", m_Post_Setting.spd);
        _postParams.Add("pit", m_Post_Setting.pit);
        _postParams.Add("vol", m_Post_Setting.vol);
        _postParams.Add("per", SetSpeeker(m_Post_Setting.per));
        _postParams.Add("aue", m_Post_Setting.aue);

        //ƴ�Ӳ�����������
        int i = 0;
        foreach (var item in _postParams)
        {
            _url += i != 0 ? "&" : "?";
            _url += item.Key + "=" + item.Value;
            i++;
        }

        //�ϳ���Ƶ
        using (UnityWebRequest _speech = UnityWebRequestMultimedia.GetAudioClip(_url, AudioType.WAV))
        {
            yield return _speech.SendWebRequest();
            if (_speech.error == null)
            {
                var type = _speech.GetResponseHeader("Content-Type");
                if (type.Contains("audio"))
                {
                    var _clip = DownloadHandlerAudioClip.GetContent(_speech);
                    _callback(_clip, _msg);
                }
                else
                {
                    var _response = _speech.downloadHandler.data;
                    string _msgBack = System.Text.Encoding.UTF8.GetString(_response);
                    UnityEngine.Debug.LogError(_msgBack);
                }

            }

            stopwatch.Stop();
            UnityEngine.Debug.Log("�ٶ������ϳɺ�ʱ��" + stopwatch.Elapsed.TotalSeconds);
        }

    }
    //��������:��С��=1����С��=0������ң��������=3����ѾѾ=4
    /// ��Ʒ����:����ң����Ʒ��=5003����С¹=5118���Ȳ���=106����Сͯ=110����С��=111�����׶�=103����С��=5
    private string SetSpeeker(SpeekerRole _role)
    {
        if (_role == SpeekerRole.��С��) return "1";
        if (_role == SpeekerRole.��С��) return "0";
        if (_role == SpeekerRole.����ң) return "3";
        if (_role == SpeekerRole.��ѾѾ) return "4";
        if (_role == SpeekerRole.JP��С��) return "5";
        if (_role == SpeekerRole.JP����ң) return "5003";
        if (_role == SpeekerRole.JP��С¹) return "5118";
        if (_role == SpeekerRole.JP�Ȳ���) return "106";
        if (_role == SpeekerRole.JP��Сͯ) return "110";
        if (_role == SpeekerRole.JP��С��) return "111";
        if (_role == SpeekerRole.JP���׶�) return "5";

        return "0";//Ĭ��Ϊ��С��
    }

    #endregion

    #region ���ݸ�ʽ����



    /// <summary>
    /// �����ϳɵ�������Ϣ
    /// </summary>
    [System.Serializable]
    public class PostDataSetting
    {
        /// <summary>
        /// �ͻ�������ѡ��web����д�̶�ֵ1
        /// </summary>
        public string ctp = "1";
        /// <summary>
        /// �̶�ֵzh������ѡ��,Ŀǰֻ����Ӣ�Ļ��ģʽ����д�̶�ֵzh
        /// </summary>
        [Header("�������ã��̶�ֵzh")] public string lan = "zh";
        /// <summary>
        /// ���٣�ȡֵ0-15��Ĭ��Ϊ5������
        /// </summary>
        [Header("���٣�ȡֵ0-15��Ĭ��Ϊ5������")] public string spd = "5";
        /// <summary>
        /// ������ȡֵ0-15��Ĭ��Ϊ5�����
        /// </summary>
        [Header("������ȡֵ0-15��Ĭ��Ϊ5�����")] public string pit = "5";
        /// <summary>
        /// ������ȡֵ0-15��Ĭ��Ϊ5��������ȡֵΪ0ʱΪ������Сֵ������Ϊ������
        /// </summary>
        [Header("������ȡֵ0-15��Ĭ��Ϊ5������")] public string vol = "5";
        /// <summary>
        /// ��������:��С��=1����С��=0������ң��������=3����ѾѾ=4
        /// ��Ʒ����:����ң����Ʒ��=5003����С¹=5118���Ȳ���=106����Сͯ=110����С��=111�����׶�=103����С��=5
        /// </summary>
        [Header("�����ʶ�������")] public SpeekerRole per = SpeekerRole.��С��;
        /// <summary>
        /// 3Ϊmp3��ʽ(Ĭ��)�� 4Ϊpcm-16k��5Ϊpcm-8k��6Ϊwav������ͬpcm-16k��; ע��aue=4����6������ʶ��Ҫ��ĸ�ʽ��
        /// ������Ƶ���ݲ�������ʶ��Ҫ�����Ȼ�˷���������ʶ��Ч������Ӱ�졣
        /// </summary>
        [Header("���÷��ص���Ƶ��ʽ")] public string aue = "6";
    }
    /// <summary>
    /// ��ѡ����
    /// </summary>
    public enum SpeekerRole
    {
        ��С��,
        ��С��,
        ����ң,
        ��ѾѾ,
        JP����ң,
        JP��С¹,
        JP�Ȳ���,
        JP��Сͯ,
        JP��С��,
        JP���׶�,
        JP��С��
    }

    /// <summary>
    /// �����ϳɽ��
    /// </summary>
    public class SpeechResponse
    {
        public int error_index;
        public string error_msg;
        public string sn;
        public int idx;
        public bool Success
        {
            get { return error_index == 0; }
        }
        public AudioClip clip;
    }


    #endregion

}
