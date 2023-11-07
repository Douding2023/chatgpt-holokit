using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WhisperSpeechToText : STT
{
    private void Awake()
    {
        m_SpeechRecognizeURL = GetPostUrl();
    }


    /// <summary>
    /// �����ַ
    /// </summary>
    [SerializeField] private string m_ServerSetting = "http://localhost:9000";
    /// <summary>
    /// ��������
    /// </summary>
    [SerializeField] private string m_TaskType = "transcribe";
    /// <summary>
    /// ����������ĵ���ʽ
    /// </summary>
    [SerializeField] private OutputType m_OutputType = OutputType.json;

    /// <summary>
    /// ��Դ��ַ
    /// </summary>
    /// <returns></returns>
    private string GetPostUrl()
    {
        string _url = string.Format("{0}/asr?task={1}&encode=true&output={2}", m_ServerSetting, m_TaskType, m_OutputType);
        return _url;
    }

    /// <summary>
    /// openai����ʶ��
    /// </summary>
    /// <param name="_clip"></param>
    /// <param name="_callback"></param>
    public override void SpeechToText(AudioClip _clip, Action<string> _callback)
    {
        byte[] _audioData = WavUtility.FromAudioClip(_clip);
        StartCoroutine(SendAudioData(_audioData, _callback));
    }


    /// <summary>
    /// �������ݵ�api
    /// </summary>
    /// <param name="audioBytes"></param>
    /// <param name="_callback"></param>
    /// <returns></returns>
    private IEnumerator SendAudioData(byte[] audioBytes, Action<string> _callback)
    {
        stopwatch.Restart();
        WWWForm form = new WWWForm();
        form.AddBinaryData("audio_file", audioBytes, "test.mp3", "audio/mpeg");
        UnityWebRequest www = UnityWebRequest.Post(m_SpeechRecognizeURL, form);
        www.SetRequestHeader("accept", "application/json");
        
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error sending audio file: " + www.error);
        }
        else
        {
            string _responseText = www.downloadHandler.text;
            Response _response = ResponseSetting(_responseText);
            //string _textback = www.downloadHandler.text;
            _callback(_response.text);
        }

        stopwatch.Stop();
        Debug.Log("Whisper����ʶ���ʱ��" + stopwatch.Elapsed.TotalSeconds);

    }

    /// <summary>
    /// ���ݷ������ͣ�������ֵ
    /// </summary>
    /// <param name="_msg"></param>
    /// <returns></returns>
    private Response ResponseSetting(string _msg)
    {
        Response _response = new Response();

        if (m_OutputType == OutputType.json) {
            //json
            _response = JsonUtility.FromJson<Response>(_msg);
        }
        else if(m_OutputType == OutputType.txt)
        {
            //txt
            _response.text= _msg;
        }
        else
        {
            //������ʽ��������չ
            _response.text = _msg;
        }


        return _response;
    }




    #region ���ݶ���

    [Serializable]
    public class Response
    {
        /// <summary>
        /// �������ı�
        /// </summary>
        [SerializeField] public string text = string.Empty;
        /// <summary>
        /// ���ӷֶ�
        /// </summary>
        [SerializeField] public List<Segment> segments = new List<Segment>();
        /// <summary>
        /// ʶ�𵽵��������ͱ���
        /// </summary>
        [SerializeField] public string language = string.Empty;
    }

    [Serializable]
    public class Segment
    {
        [SerializeField] public int id;
        [SerializeField] public int seek;
        [SerializeField] public int start;
        [SerializeField] public int end;
        [SerializeField] public string text = string.Empty;
        [SerializeField] public int temperature;
    }

    /// <summary>
    /// ʶ��ɹ�������ĵ�����
    /// </summary>
    public enum OutputType
    {
        txt,
        json,
        vtt,
        srt,
        tsv
    }

    #endregion

}
