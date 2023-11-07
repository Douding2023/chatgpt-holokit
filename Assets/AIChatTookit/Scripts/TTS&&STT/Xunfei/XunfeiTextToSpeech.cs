using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;
//using UnityEngine.Networking.Types;

public class XunfeiTextToSpeech : TTS
{
    // #region ����
    // /// <summary>
    // /// Ѷ�ɵ�Ӧ������
    // /// </summary>
    // [SerializeField]private XunfeiSettings m_XunfeiSettings;
    // /// <summary>
    // /// host��ַ
    // /// </summary>
    // [SerializeField] private string m_HostUrl = "tts-api.xfyun.cn";

    // /// <summary>
    // /// ��Ƶ���룬��ѡֵ��
    // ///raw��δѹ����pcm
    // ///lame��mp3(��aue= lameʱ�贫��sfl = 1)
    // ///speex-org-wb;7�� ��׼��Դspeex��for speex_wideband����16k�����ִ���ָ��ѹ���ȼ���Ĭ�ϵȼ�Ϊ8��
    // ///speex-org-nb;7�� ��׼��Դspeex��for speex_narrowband����8k�����ִ���ָ��ѹ���ȼ���Ĭ�ϵȼ�Ϊ8��
    // ///speex;7��ѹ����ʽ��ѹ���ȼ�1 ~10��Ĭ��Ϊ7��8kѶ�ɶ���speex��
    // ///speex-wb;7��ѹ����ʽ��ѹ���ȼ�1 ~10��Ĭ��Ϊ7��16kѶ�ɶ���speex��
    // /// </summary>
    // [SerializeField] private string m_Aue = "raw";
    // /// <summary>
    // /// ������
    // /// </summary>
    // [Header("ѡ���ʶ�������")]
    // //[SerializeField] private Speaker m_Vcn = Speaker.Ѷ��С��;
    // /// <summary>
    // /// ��������ѡֵ��[0-100]��Ĭ��Ϊ50
    // /// </summary>
    // [SerializeField] private int m_Volume = 50;
    // /// <summary>
    // /// �����ߣ���ѡֵ��[0-100]��Ĭ��Ϊ50
    // /// </summary>
    // [SerializeField] private int m_Pitch = 50;
    // /// <summary>
    // /// ���٣���ѡֵ��[0-100]��Ĭ��Ϊ50
    // /// </summary>
    // [SerializeField] private int m_Speed = 50;

    // #endregion

    // private void Awake()
    // {
    //     m_XunfeiSettings = this.GetComponent<XunfeiSettings>();
    //     m_PostURL= "wss://tts-api.xfyun.cn/v2/tts";
    // }

    // /// <summary>
    // /// �����ϳɣ����غϳ��ı�
    // /// </summary>
    // /// <param name="_msg"></param>
    // /// <param name="_callback"></param>
    // public override void Speak(string _msg, Action<AudioClip, string> _callback)
    // {
    //     StartCoroutine(GetSpeech(_msg, _callback));
    // }

    // /// <summary>
    // /// websocket
    // /// </summary>
    // private ClientWebSocket m_WebSocket;
    // private CancellationToken m_CancellationToken;

    // #region ��ȡ��ȨUrl

    // /// <summary>
    // /// ��ȡ��Ȩurl
    // /// </summary>
    // /// <returns></returns>
    // private string GetUrl()
    // {
    //     //��ȡʱ���
    //     string date = DateTime.Now.ToString("r");
    //     //ƴ��ԭʼ��signature
    //     string signature_origin = string.Format("host: " + m_HostUrl + "\ndate: " + date + "\nGET /v2/tts HTTP/1.1");
    //     //hmac-sha256�㷨-ǩ������ת��Ϊbase64����
    //     string signature = Convert.ToBase64String(new HMACSHA256(Encoding.UTF8.GetBytes(m_XunfeiSettings.m_APISecret)).ComputeHash(Encoding.UTF8.GetBytes(signature_origin)));
    //     //ƴ��ԭʼ��authorization
    //     string authorization_origin = string.Format("api_key=\"{0}\",algorithm=\"hmac-sha256\",headers=\"host date request-line\",signature=\"{1}\"", m_XunfeiSettings.m_APIKey, signature);
    //     //ת��Ϊbase64����
    //     string authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes(authorization_origin));
    //     //ƴ�Ӽ�Ȩ��url
    //     string url = string.Format("{0}?authorization={1}&date={2}&host={3}", m_PostURL, authorization, date, m_HostUrl);

    //     return url;
    // }

    // #endregion

    // #region �����ϳ�

    // /// <summary>
    // /// ��Ƶ����
    // /// </summary>
    // private int m_AudioLenth;
    // /// <summary>
    // /// ���ݶ���
    // /// </summary>
    // Queue<float> m_AudioQueue = new Queue<float>();

    // /// <summary>
    // /// ��ȡ�����ϳ�
    // /// </summary>
    // /// <param name="_text"></param>
    // /// <param name="_callback"></param>
    // /// <returns></returns>
    // public IEnumerator GetSpeech(string _text, Action<AudioClip, string> _callback)
    // {
    //     stopwatch.Restart();
    //     yield return null;

    //     if (m_WebSocket != null) { m_WebSocket.Abort(); }

    //     ConnectHost(_text);
    //     AudioClip _audioClip = AudioClip.Create("audio", 16000 * 60, 1, 16000, true, OnAudioRead);

    //     //�ص�
    //     _callback(_audioClip, _text);

    //     stopwatch.Stop();
    //     UnityEngine.Debug.Log("Ѷ�������ϳɺ�ʱ��" + stopwatch.Elapsed.TotalSeconds);
    // }
    // void OnAudioRead(float[] data)
    // {
    //     for (int i = 0; i < data.Length; i++)
    //     {
    //         if (m_AudioQueue.Count > 0)
    //         {
    //             data[i] = m_AudioQueue.Dequeue();
    //         }
    //         else
    //         {
    //             if (m_WebSocket == null || m_WebSocket.State != WebSocketState.Aborted) m_AudioLenth++;
    //             data[i] = 0;
    //         }
    //     }
    // }


    // /// <summary>
    // /// ���ӷ��������ϳ�����
    // /// </summary>
    // private async void ConnectHost(string text)
    // {
    //     try
    //     {
    //         m_WebSocket = new ClientWebSocket();
    //         m_CancellationToken = new CancellationToken();
    //         Uri uri = new Uri(GetUrl());
    //         await m_WebSocket.ConnectAsync(uri, m_CancellationToken);
    //         text = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
    //         //���͵�����
    //         PostData _postData = new PostData()
    //         {
    //             common = new CommonTag(m_XunfeiSettings.m_AppID),
    //             business = new BusinessTag(m_Aue, GetVoice(m_Vcn), m_Volume, m_Pitch, m_Speed),
    //             data = new DataTag(2, text)
    //         };
    //         //ת��json��ʽ
    //         string _jsonData = JsonUtility.ToJson(_postData);
    //         await m_WebSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(_jsonData)), WebSocketMessageType.Binary, true, m_CancellationToken); //��������
    //         StringBuilder sb = new StringBuilder();
    //         //���Ŷ���.Clear();
    //         while (m_WebSocket.State == WebSocketState.Open)
    //         {
    //             var result = new byte[4096];
    //             await m_WebSocket.ReceiveAsync(new ArraySegment<byte>(result), m_CancellationToken);//��������
    //             List<byte> list = new List<byte>(result); while (list[list.Count - 1] == 0x00) list.RemoveAt(list.Count - 1);//ȥ�����ֽ�  
    //             var str = Encoding.UTF8.GetString(list.ToArray());
    //             sb.Append(str);
    //             if (str.EndsWith("}"))
    //             {
    //                 //��ȡ���ص�����
    //                 ResponseData _responseData = JsonUtility.FromJson<ResponseData>(sb.ToString());
    //                 sb.Clear();

    //                 if (_responseData.code != 0)
    //                 {
    //                     //���ش���
    //                     PrintErrorLog(_responseData.code);
    //                     m_WebSocket.Abort();
    //                     break;
    //                 }
    //                 //���û���ݣ�ֱ�ӽ���
    //                 if (_responseData.data == null)
    //                 {
    //                     Debug.LogError("���ص���Ƶ����Ϊ��");
    //                     m_WebSocket.Abort();
    //                     break;
    //                 }
    //                 //�õ���Ƶ����
    //                 float[] fs = BytesToFloat(Convert.FromBase64String(_responseData.data.audio));
    //                 m_AudioLenth += fs.Length;
    //                 foreach (float f in fs) m_AudioQueue.Enqueue(f);

    //                 if (_responseData.data.status == 2)
    //                 {

    //                     m_WebSocket.Abort();
    //                     break;
    //                 }
    //             }
    //         }

    //     }
    //     catch (Exception ex)
    //     {
    //         Debug.LogError("������Ϣ: " + ex.Message);
    //         m_WebSocket.Dispose();
    //     }
    // }


    // #endregion



    // #region ���߷���
    // /// <summary>
    // /// ��ӡ������־
    // /// </summary>
    // /// <param name="status"></param>
    // private void PrintErrorLog(int status)
    // {
    //     if (status == 10005)
    //     {
    //         Debug.LogError("appid��Ȩʧ��");
    //         return;
    //     }
    //     if (status == 10006)
    //     {
    //         Debug.LogError("����ȱʧ��Ҫ����");
    //         return;
    //     }
    //     if (status == 10007)
    //     {
    //         Debug.LogError("����Ĳ���ֵ��Ч");
    //         return;
    //     }
    //     if (status == 10010)
    //     {
    //         Debug.LogError("������Ȩ����");
    //         return;
    //     }
    //     if (status == 10109)
    //     {
    //         Debug.LogError("�����ı����ȷǷ�");
    //         return;
    //     }
    //     if (status == 10019)
    //     {
    //         Debug.LogError("session��ʱ");
    //         return;
    //     }
    //     if (status == 10101)
    //     {
    //         Debug.LogError("����Ự�ѽ���");
    //         return;
    //     }
    //     if (status == 10313)
    //     {
    //         Debug.LogError("appid����Ϊ��");
    //         return;
    //     }
    //     if (status == 10317)
    //     {
    //         Debug.LogError("�汾�Ƿ�");
    //         return;
    //     }
    //     if (status == 11200)
    //     {
    //         Debug.LogError("û��Ȩ��");
    //         return;
    //     }
    //     if (status == 11201)
    //     {
    //         Debug.LogError("�����س���");
    //         return;
    //     }
    //     if (status == 10160)
    //     {
    //         Debug.LogError("�������ݸ�ʽ�Ƿ�");
    //         return;
    //     }
    //     if (status == 10161)
    //     {
    //         Debug.LogError("base64����ʧ��");
    //         return;
    //     }
    //     if (status == 10163)
    //     {
    //         Debug.LogError("ȱ�ٱش����������߲������Ϸ�������ԭ�����ϸ������");
    //         return;
    //     }
    //     if (status == 10200)
    //     {
    //         Debug.LogError("��ȡ���ݳ�ʱ");
    //         return;
    //     }
    //     if (status == 10222)
    //     {
    //         Debug.LogError("�����쳣");
    //         return;
    //     }
    // }

    // /// <summary>
    // /// byte[]����ת��ΪAudioClip�ɶ�ȡ��float[]����
    // /// </summary>
    // /// <param name="byteArray"></param>
    // /// <returns></returns>
    // public float[] BytesToFloat(byte[] byteArray)
    // {
    //     float[] sounddata = new float[byteArray.Length / 2];
    //     for (int i = 0; i < sounddata.Length; i++)
    //     {
    //         sounddata[i] = BytesToFloat(byteArray[i * 2], byteArray[i * 2 + 1]);
    //     }
    //     return sounddata;
    // }

    // private float BytesToFloat(byte firstByte, byte secondByte)
    // {
    //     //С�˺ʹ��˳��Ҫ����
    //     short s;
    //     if (BitConverter.IsLittleEndian)
    //         s = (short)((secondByte << 8) | firstByte);
    //     else
    //         s = (short)((firstByte << 8) | secondByte);
    //     // convert to range from -1 to (just below) 1
    //     return s / 32768.0F;
    // }


    // #endregion


    // #region ���ݶ���
    // /// <summary>
    // /// ���͵�����
    // /// </summary>
    // [Serializable]
    // public class PostData
    // {
    //     [SerializeField] public CommonTag common;
    //     [SerializeField] public BusinessTag business;
    //     [SerializeField] public DataTag data;
    // }


    // [Serializable]
    // public class CommonTag
    // {
    //     [SerializeField] public string app_id = string.Empty;
    //     public CommonTag(string app_id)
    //     {
    //         this.app_id = app_id;
    //     }
    // }
    // [Serializable]
    // public class BusinessTag
    // {
    //     [SerializeField] public string aue = string.Empty;
    //     [SerializeField] public string vcn = string.Empty;
    //     [SerializeField] public int volume = 50;
    //     [SerializeField] public int pitch = 50;
    //     [SerializeField] public int speed = 50;
    //     [SerializeField] public string tte = "UTF8";
    //     public BusinessTag(string aue, string vcn, int volume, int pitch, int speed)
    //     {
    //         this.aue = aue;
    //         this.vcn = vcn;
    //         this.volume = volume;
    //         this.pitch = pitch;
    //         this.speed = speed;
    //     }
    // }

    // [Serializable]
    // public class DataTag
    // {
    //     [SerializeField] public int status = 2;
    //     [SerializeField] public string text = string.Empty;
    //     public DataTag(int status, string text)
    //     {
    //         this.status = status;
    //         this.text = text;
    //     }
    // }

    // [Serializable]
    // public class ResponseData
    // {
    //     [SerializeField] public int code = 0;
    //     [SerializeField] public string message = string.Empty;
    //     [SerializeField] public string sid = string.Empty;
    //     [SerializeField] public ResponcsedataTag data;
    // }
    // [Serializable]
    // public class ResponcsedataTag
    // {
    //     [SerializeField] public string audio = string.Empty;
    //     [SerializeField] public string ced = string.Empty;
    //     [SerializeField] public int status = 2;
    // }

    // #endregion

    // #region ������
    // public enum Speaker
    // {
    //     Ѷ��С��,
    //     Ѷ������,
    //     Ѷ��СƼ,
    //     Ѷ��С�,
    //     Ѷ����С��
    // }
    // /// <summary>
    // /// ��������
    // /// </summary>
    // /// <param name="_speeker"></param>
    // /// <returns></returns>
    // private string GetVoice(Speaker _speeker)
    // {
    //     if (_speeker == Speaker.Ѷ��С��)
    //     {
    //         return "xiaoyan";
    //     }
    //     if (_speeker == Speaker.Ѷ������)
    //     {
    //         return "aisjiuxu";
    //     }
    //     if (_speeker == Speaker.Ѷ��СƼ)
    //     {
    //         return "aisxping";
    //     }
    //     if (_speeker == Speaker.Ѷ��С�)
    //     {
    //         return "aisjinger";
    //     }
    //     if (_speeker == Speaker.Ѷ����С��)
    //     {
    //         return "aisbabyxu";
    //     }

    //     return "xiaoyan";
    // }
    // #endregion
}
