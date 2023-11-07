using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using WebGLSupport;

public class ChatSample : MonoBehaviour
{
    /// <summary>
    /// ��������
    /// </summary>
    [SerializeField] private ChatSetting m_ChatSettings;
    #region UI����
    /// <summary>
    /// ����UI��
    /// </summary>
    //[SerializeField] private GameObject m_ChatPanel;
    /// <summary>
    /// �������Ϣ
    /// </summary>
    [SerializeField] public InputField m_InputWord;
    /// <summary>
    /// ���ص���Ϣ
    /// </summary>
    [SerializeField] private Text m_TextBack;
    /// <summary>
    /// ��������
    /// </summary>
    //[SerializeField] private AudioSource m_AudioSource;
    /// <summary>
    /// ������Ϣ��ť
    /// </summary>
    [SerializeField] private Button m_CommitMsgBtn;

    #endregion

    #region ��������
    /// <summary>
    /// ����������
    /// </summary>
    [SerializeField] private Animator m_Animator;
    /// <summary>
    /// ����ģʽ������Ϊfalse,��ͨ�������ϳ�
    /// </summary>
    [Header("�����Ƿ�ͨ�������ϳɲ����ı�")]
    [SerializeField] private bool m_IsVoiceMode = true;

    #endregion

    private void Awake()
    {
        // m_CommitMsgBtn.onClick.AddListener(delegate { SendData(); });
         RegistButtonEvent();
        // InputSettingWhenWebgl();

       // InputText("Hello");
       // TestTwoModels();
    }

    #region ��Ϣ����

    /// <summary>
    /// webglʱ������֧����������
    /// </summary>
    private void InputSettingWhenWebgl()
    {
#if UNITY_WEBGL
        m_InputWord.gameObject.AddComponent<WebGLSupport.WebGLInput>();
#endif
    }


    /// <summary>
    /// ������Ϣ
    /// </summary>
    public void SendData()
    {
        if (m_InputWord.text.Equals(""))
            return;

        //���Ӽ�¼����
        m_ChatHistory.Add(m_InputWord.text);
        //��ʾ��
        string _msg = m_InputWord.text;

        //��������
        m_ChatSettings.m_ChatModel.PostMsg(_msg, CallBack);

        m_InputWord.text = "";
        m_TextBack.text = "����˼����...";

        //�л�˼������
        SetAnimator("state", 1);
    }
    /// <summary>
    /// �����ַ���
    /// </summary>
    /// <param name="_postWord"></param>
    public void SendData(string _postWord)
    {
        if (_postWord.Equals(""))
            return;

        //���Ӽ�¼����
        m_ChatHistory.Add(_postWord);
        //��ʾ��
        string _msg = _postWord;

        //��������
        m_ChatSettings.m_ChatModel.PostMsg(_msg, CallBack);

        m_InputWord.text = "";
        m_TextBack.text = "����˼����...";

        //�л�˼������
        SetAnimator("state", 1);
    }

    /// <summary>
    /// AI�ظ�����Ϣ�Ļص�
    /// </summary>
    /// <param name="_response"></param>
    private void CallBack(string _response)
    {
        _response = _response.Trim();
        m_TextBack.text = "";

        
        Debug.Log("�յ�AI�ظ���"+ _response);

        //��¼����
        m_ChatHistory.Add(_response);

        if (!m_IsVoiceMode||m_ChatSettings.m_TextToSpeech == null)
        {
            //��ʼ�����ʾ���ص��ı�
            StartTypeWords(_response);
            return;
        }


        m_ChatSettings.m_TextToSpeech.Speak(_response, PlayVoice);
    }

#endregion

#region ��������
    /// <summary>
    /// ����ʶ�𷵻ص��ı��Ƿ�ֱ�ӷ�����LLM
    /// </summary>
    [SerializeField] private bool m_AutoSend = true;
    /// <summary>
    /// ��������İ�ť
    /// </summary>
    [SerializeField] private Button m_VoiceInputBotton;
    /// <summary>
    /// ¼����ť���ı�
    /// </summary>
    [SerializeField]private Text m_VoiceBottonText;
    /// <summary>
    /// ¼������ʾ��Ϣ
    /// </summary>
    [SerializeField] private Text m_RecordTips;
    /// <summary>
    /// �������봦����
    /// </summary>
    [SerializeField] private VoiceInputs m_VoiceInputs;
   
    /// <summary>
    /// ע�ᰴť�¼�
    /// </summary>
    private void RegistButtonEvent()
    {
        if (m_VoiceInputBotton == null || m_VoiceInputBotton.GetComponent<EventTrigger>())
            return;

        EventTrigger _trigger = m_VoiceInputBotton.gameObject.AddComponent<EventTrigger>();

        //���Ӱ�ť���µ��¼�
        EventTrigger.Entry _pointDown_entry = new EventTrigger.Entry();
        _pointDown_entry.eventID = EventTriggerType.PointerDown;
        _pointDown_entry.callback = new EventTrigger.TriggerEvent();

        //���Ӱ�ť�ɿ��¼�
        EventTrigger.Entry _pointUp_entry = new EventTrigger.Entry();
        _pointUp_entry.eventID = EventTriggerType.PointerUp;
        _pointUp_entry.callback = new EventTrigger.TriggerEvent();

        //����ί���¼�
        _pointDown_entry.callback.AddListener(delegate { StartRecord(); });
        _pointUp_entry.callback.AddListener(delegate { StopRecord(); });

        _trigger.triggers.Add(_pointDown_entry);
       _trigger.triggers.Add(_pointUp_entry);
    }

    /// <summary>
    /// ��ʼ¼��
    /// </summary>
    public void StartRecord()
    {
       // m_VoiceBottonText.text = "456"; 
        m_VoiceInputs.StartRecordAudio();
       // Debug.Log(0);
    }
    /// <summary>
    /// ����¼��
    /// </summary>
    public void StopRecord()
    {
       // m_VoiceBottonText.text = "123"; 
       // m_RecordTips.text = "¼������������ʶ��...";
        m_VoiceInputs.StopRecordAudio(AcceptClip);
       // Debug.Log(m_RecordTips.text);
    }

    /// <summary>
    /// ����¼�Ƶ���Ƶ����
    /// </summary>
    /// <param name="_data"></param>
    private void AcceptData(byte[] _data)
    {
        if (m_ChatSettings.m_SpeechToText == null)
            return;

        m_ChatSettings.m_SpeechToText.SpeechToText(_data, DealingTextCallback);
    }

    /// <summary>
    /// ����¼�Ƶ���Ƶ����
    /// </summary>
    /// <param name="_data"></param>
    private void AcceptClip(AudioClip _audioClip)
    {
        if (m_ChatSettings.m_SpeechToText == null)
            return;

        m_ChatSettings.m_SpeechToText.SpeechToText(_audioClip, DealingTextCallback2);
    }
    /// <summary>
    /// ����ʶ�𵽵��ı�
    /// </summary>
    /// <param name="_msg"></param>
    private void DealingTextCallback(string _msg)
    {
        
        //m_RecordTips.text = _msg;
        StartCoroutine(SetTextVisible(m_RecordTips));
        //�Զ�����
        if (m_AutoSend)
        {
            SendData(_msg);
            return;
        }

        m_InputWord.text = _msg;
    }

     private void DealingTextCallback2(string _msg)
    {
        
        StartConversation(_msg);
    }

    private IEnumerator SetTextVisible(Text _textbox)
    {
        yield return new WaitForSeconds(3f);
        //_textbox.text = "";
    }

#endregion

#region �����ϳ�

    private void PlayVoice(AudioClip _clip, string _response)
    {
       // m_AudioSource.clip = _clip;
      //  m_AudioSource.Play();
       // Debug.Log("��Ƶʱ����" + _clip.length);
        //��ʼ�����ʾ���ص��ı�
        StartTypeWords(_response);
        //�л���˵������
        SetAnimator("state", 2);
    }

#endregion

#region ���������ʾ
    //������ʾ��ʱ����
    [SerializeField] private float m_WordWaitTime = 0.2f;
    //�Ƿ���ʾ���
    [SerializeField] private bool m_WriteState = false;

    /// <summary>
    /// ��ʼ�����ӡ
    /// </summary>
    /// <param name="_msg"></param>
    private void StartTypeWords(string _msg)
    {
        if (_msg == "")
            return;

        m_WriteState = true;
        StartCoroutine(SetTextPerWord(_msg));
    }

    private IEnumerator SetTextPerWord(string _msg)
    {
        int currentPos = 0;
        while (m_WriteState)
        {
            yield return new WaitForSeconds(m_WordWaitTime);
            currentPos++;
            //������ʾ������
            m_TextBack.text = _msg.Substring(0, currentPos);

            m_WriteState = currentPos < _msg.Length;

        }

        //�л����ȴ�����
        SetAnimator("state",0);
    }

#endregion

#region �����¼
    //���������¼
    [SerializeField] private List<string> m_ChatHistory;
    //�����Ѵ�������������
    [SerializeField] private List<GameObject> m_TempChatBox;
    //�����¼��ʾ��
    [SerializeField] private GameObject m_HistoryPanel;
    //�����ı����õĲ�
    [SerializeField] private RectTransform m_rootTrans;
    //������������
    [SerializeField] private ChatPrefab m_PostChatPrefab;
    //�ظ�����������
    [SerializeField] private ChatPrefab m_RobotChatPrefab;
    //������
   // [SerializeField] private ScrollRect m_ScroTectObject;
    //��ȡ�����¼
    public void OpenAndGetHistory()
    {
        //m_ChatPanel.SetActive(false);
        m_HistoryPanel.SetActive(true);

        ClearChatBox();
        StartCoroutine(GetHistoryChatInfo());
    }
    //����
    public void BackChatMode()
    {
        //m_ChatPanel.SetActive(true);
        m_HistoryPanel.SetActive(false);
    }

    //����Ѵ����ĶԻ���
    private void ClearChatBox()
    {
        while (m_TempChatBox.Count != 0)
        {
            if (m_TempChatBox[0])
            {
                Destroy(m_TempChatBox[0].gameObject);
                m_TempChatBox.RemoveAt(0);
            }
        }
        m_TempChatBox.Clear();
    }

    //��ȡ�����¼�б�
    private IEnumerator GetHistoryChatInfo()
    {

        yield return new WaitForEndOfFrame();

        for (int i = 0; i < m_ChatHistory.Count; i++)
        {
            if (i % 2 == 0)
            {
                ChatPrefab _sendChat = Instantiate(m_PostChatPrefab, m_rootTrans.transform);
                _sendChat.SetText(m_ChatHistory[i]);
                m_TempChatBox.Add(_sendChat.gameObject);
                continue;
            }

            ChatPrefab _reChat = Instantiate(m_RobotChatPrefab, m_rootTrans.transform);
            _reChat.SetText(m_ChatHistory[i]);
            m_TempChatBox.Add(_reChat.gameObject);
        }

        //���¼��������ߴ�
        LayoutRebuilder.ForceRebuildLayoutImmediate(m_rootTrans);
        //StartCoroutine(TurnToLastLine());
    }

   /* private IEnumerator TurnToLastLine()
    {
        yield return new WaitForEndOfFrame();
        //�������������Ϣ
        m_ScroTectObject.verticalNormalizedPosition = 0;
    }*/


#endregion

    private void SetAnimator(string _para,int _value)
    {
        if (m_Animator == null)
            return;

        m_Animator.SetInteger(_para, _value);
    }

    // ----------------------------------------------------------------------------- 自己写的
    // 向 ChatGPT 输入一段文字
    [SerializeField] private LLM m_Model2;

    private void TestTwoModels() 
    {
        InputTextToModel1("hello");
    }

    public void StartConversation(string text) {
         InputTextToModel1(text);
    }

    public void InputText(string speechText) 
    {
        if (speechText.Equals(""))
            return;

        //���Ӽ�¼����
        m_ChatHistory.Add(speechText);
        //��ʾ��
        string _msg = speechText;

        //��������
        m_ChatSettings.m_ChatModel.PostMsg(speechText, (string reply) => { 
            Debug.Log($"ChatGPT reply: {reply}"); 

            // TODO: 将 ChatGPT 的回答传给 Text to Speech 系统
            reply = reply.Trim();
            m_ChatHistory.Add(reply);
            m_ChatSettings.m_TextToSpeech.Speak(reply, PlayVoice2);
        });
    }

    public void InputTextToModel1(string speechText) 
    {
        if (speechText.Equals(""))
            return;

        //���Ӽ�¼����
        m_ChatHistory.Add(speechText);
        //��ʾ��
        string _msg = speechText;

        //��������
        m_ChatSettings.m_ChatModel.PostMsg(speechText, (string reply) => { 
            Debug.Log($"Model 1's reply: {reply}");
            // TODO: 把回应转成语音播放，再输入给 Model2
            m_ChatSettings.m_TextToSpeech.Speak(reply, PlayModel1Voice);
        });
    }

    public void InputTextToModel2(string speechText) 
    {
        if (speechText.Equals(""))
            return;

        //���Ӽ�¼����
        m_ChatHistory.Add(speechText);
        //��ʾ��
        string _msg = speechText;

        //��������
        m_Model2.PostMsg(speechText, (string reply) => { 
            Debug.Log($"Model 2's reply: {reply}"); 
            m_ChatSettings.m_TextToSpeech.Speak(reply, PlayModel2Voice);
        });
    }

    private void PlayModel2Voice(AudioClip clip, string response) {
       // AudioSourceForMouse2 = GetComponent<AudioSource>();
        AudioSourceForMouse2.clip = clip;
        AudioSourceForMouse2.Play();
        StartCoroutine(InputTextToModel1WithDelay(response, clip.length));
    }

    private IEnumerator InputTextToModel1WithDelay(string text, float delay) {
        yield return new WaitForSeconds(delay);
        InputTextToModel1(text);
    }

    private void PlayModel1Voice(AudioClip clip, string response) {
        //AudioSourceForMouse1 = GetComponent<AudioSource>();
        AudioSourceForMouse1.clip = clip;
        AudioSourceForMouse1.Play();
        StartCoroutine(InputTextToModel2WithDelay(response, clip.length));
    }

    private IEnumerator InputTextToModel2WithDelay(string text, float delay) {
        yield return new WaitForSeconds(delay);
        InputTextToModel2(text);
    }

    private void PlayVoice2(AudioClip _clip, string _response)
    {
       // m_AudioSource2 = GetComponent<AudioSource>();
       // m_AudioSource2.clip = _clip;
       // m_AudioSource2.Play();
        //Debug.Log("��Ƶʱ����" + _clip.length);
        //��ʼ�����ʾ���ص��ı�
        StartTypeWords(_response);
        //�л���˵������
        SetAnimator("state", 2);
    }

    public AudioSource AudioSourceForMouse1;

    public AudioSource AudioSourceForMouse2;

    // 当 ChatGPT 生成答案时运行
    public UnityEvent<string> OnGeneratedReply;

}
