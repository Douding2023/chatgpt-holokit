using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChatBaidu : LLM
{

    public ChatBaidu()
    {
        url = "https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/eb-instant";
    }

    void Awake()
    {
        OnInit();
    }

    /// <summary>
    /// token�ű�
    /// </summary>
    [SerializeField] private BaiduSettings m_Settings;
    /// <summary>
    /// ��ʷ�Ի�
    /// </summary>
    private List<message> m_History = new List<message>();
    /// <summary>
    /// ѡ���ģ������
    /// </summary>
    [Header("����ģ������")]
    public ModelType m_ModelType = ModelType.ERNIE_Bot_turbo;

    /// <summary>
    /// ��ʼ��
    /// </summary>
    private void OnInit()
    {
        m_Settings = this.GetComponent<BaiduSettings>();
        GetEndPointUrl();
    }




    /// <summary>
    /// ������Ϣ
    /// </summary>
    /// <returns></returns>
    public override void PostMsg(string _msg, Action<string> _callback)
    {
        base.PostMsg(_msg, _callback);
    }


    /// <summary>
    /// ��������
    /// </summary> 
    /// <param name="_postWord"></param>
    /// <param name="_callback"></param>
    /// <returns></returns>
    public override IEnumerator Request(string _postWord, System.Action<string> _callback)
    {
        stopwatch.Restart();

        string _postUrl = url + "?access_token=" + m_Settings.m_Token;
        m_History.Add(new message("user", _postWord));
        RequestData _postData = new RequestData
        {
            messages = m_History
        };

        using (UnityWebRequest request = new UnityWebRequest(_postUrl, "POST"))
        {
            string _jsonData = JsonUtility.ToJson(_postData);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(_jsonData);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.responseCode == 200)
            {
                string _msg = request.downloadHandler.text;
                ResponseData response = JsonConvert.DeserializeObject<ResponseData>(_msg);

                //��ʷ��¼
                string _responseText = response.result;
                m_History.Add(new message("assistant", response.result));

                //��Ӽ�¼
                m_DataList.Add(new SendData("assistant", response.result));
                //�ص�
                _callback(response.result);

            }

        }


        stopwatch.Stop();
        Debug.Log("chat�ٶ�-��ʱ��" + stopwatch.Elapsed.TotalSeconds);
    }


    /// <summary>
    /// ��ȡ��Դ·��
    /// </summary>
    private void GetEndPointUrl()
    {
        url = "https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/" + CheckModelType(m_ModelType);
    }

    /// <summary>
    /// ��ȡ��Դ
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    private string CheckModelType(ModelType _type)
    {
        if (_type == ModelType.ERNIE_Bot){
            return "completions";
        }
        if (_type == ModelType.ERNIE_Bot_turbo)
        {
            return "eb-instant";
        }
        if (_type == ModelType.BLOOMZ_7B)
        {
            return "bloomz_7b1";
        }
        if (_type == ModelType.Qianfan_BLOOMZ_7B_compressed)
        {
            return "qianfan_bloomz_7b_compressed";
        }
        if (_type == ModelType.ChatGLM2_6B_32K)
        {
            return "chatglm2_6b_32k";
        }
        if (_type == ModelType.Llama_2_7B_Chat)
        {
            return "llama_2_7b";
        }
        if (_type == ModelType.Llama_2_13B_Chat)
        {
            return "llama_2_13b";
        }
        if (_type == ModelType.Llama_2_70B_Chat)
        {
            return "llama_2_70b";
        }
        if (_type == ModelType.Qianfan_Chinese_Llama_2_7B)
        {
            return "qianfan_chinese_llama_2_7b";
        }
        if (_type == ModelType.AquilaChat_7B)
        {
            return "aquilachat_7b";
        }
        return "";
    }


    #region ���ݶ���
    //���͵�����
    [Serializable]
    private class RequestData
    {
        public List<message> messages = new List<message>();//���͵���Ϣ
        public bool stream = false;//�Ƿ���ʽ���
        public string user_id=string.Empty;
    }
    [Serializable]
    private class message
    {
        public string role=string.Empty;//��ɫ
        public string content = string.Empty;//�Ի�����
        public message() { }
        public message(string _role,string _content)
        {
            role = _role;
            content = _content;
        }
    }

    //���յ�����
    [Serializable]
    private class ResponseData
    {
        public string id = string.Empty;//���ֶԻ���id
        public int created;
        public int sentence_id;//��ʾ��ǰ�Ӿ����š�ֻ������ʽ�ӿ�ģʽ�»᷵�ظ��ֶ�
        public bool is_end;//��ʾ��ǰ�Ӿ��Ƿ������һ�䡣ֻ������ʽ�ӿ�ģʽ�»᷵�ظ��ֶ�
        public bool is_truncated;//��ʾ��ǰ�Ӿ��Ƿ������һ�䡣ֻ������ʽ�ӿ�ģʽ�»᷵�ظ��ֶ�
        public string result = string.Empty;//���ص��ı�
        public bool need_clear_history;//��ʾ�û������Ƿ���ڰ�ȫ
        public int ban_round;//��need_clear_historyΪtrueʱ�����ֶλ��֪�ڼ��ֶԻ���������Ϣ������ǵ�ǰ���⣬ban_round=-1
        public Usage usage = new Usage();//tokenͳ����Ϣ��token�� = ������+������*1.3 
    }
    [Serializable]
    private class Usage
    {
        public int prompt_tokens;//����tokens��
        public int completion_tokens;//�ش�tokens��
        public int total_tokens;//tokens����
    }

    #endregion

    /// <summary>
    /// ģ������
    /// </summary>
    public enum ModelType
    {
        ERNIE_Bot,
        ERNIE_Bot_turbo,
        BLOOMZ_7B,
        Qianfan_BLOOMZ_7B_compressed,
        ChatGLM2_6B_32K,
        Llama_2_7B_Chat,
        Llama_2_13B_Chat,
        Llama_2_70B_Chat,
        Qianfan_Chinese_Llama_2_7B,
        AquilaChat_7B,
    }


}
