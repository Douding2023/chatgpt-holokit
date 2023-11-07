using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
/// <summary>
/// ��˷�ʵʱ����
/// </summary>
public class RTSpeechHandler : MonoBehaviour
{
    /// <summary>
    /// ��˷�����
    /// </summary>
    public string m_MicrophoneName = null;
    /// <summary>
    /// �����������ֵ���Ϳ�ʼ¼��
    /// </summary>
    public float m_SilenceThreshold = 0.01f;
    /// <summary>
    /// ��Ĭ����ʱ��
    /// </summary>
    [Header("���ü���û��������ֹͣ¼��")]
    public float m_RecordingTimeLimit = 2.0f;
    /// <summary>
    /// �Ի�״̬����ʱ��
    /// </summary>
    [Header("���öԻ�״̬����ʱ��")]
    public float m_LossAwakeTimeLimit = 10f;
    /// <summary>
    /// ����״̬�£�����¼��Ĭʱ��
    /// </summary>
    [SerializeField]private bool m_LockState = false;
    /// <summary>
    /// ��Ƶ
    /// </summary>
    private AudioClip m_RecordedClip;
    /// <summary>
    /// ���ѹؼ���
    /// </summary>
    [SerializeField]private string m_AwakeKeyWord=string.Empty;
    /// <summary>
    /// ����״̬
    /// </summary>
    [Header("��ʶ��ǰ�Ƿ��ڻ���״̬")]
    [SerializeField]private bool m_AwakeState = false;
    /// <summary>
    /// ����״̬
    /// </summary>
    [SerializeField] private bool m_ListeningState = false;
    /// <summary>
    /// ¼��״̬
    /// </summary>
    [SerializeField] private bool m_IsRecording = false;
    /// <summary>
    /// ��Ĭ��ʱ��
    /// </summary>
    [SerializeField]private float m_SilenceTimer = 0.0f;

    /// <summary>
    /// ����ű�
    /// </summary>
    [SerializeField]private RTChatSample m_ChatSample;
    /// <summary>
    /// ��������
    /// </summary>
    [SerializeField] private WOV m_VoiceAWake;

    private void Awake()
    {
        OnInit();
    }

    private void OnInit()
    {
        //AI�ظ������ص�
        m_ChatSample.OnAISpeakDone += SpeachDoneCallBack;

        //�󶨻��ѻص�
        m_VoiceAWake.OnBindAwakeCallBack(AwakeCallBack);

    }

    private void Start()
    {

        if (m_MicrophoneName == null)
        {
            // ���û��ָ����˷����ƣ���ʹ��ϵͳĬ����˷�
            m_MicrophoneName = Microphone.devices[0];
        }

        // ȷ����˷�׼����
        if (Microphone.IsRecording(m_MicrophoneName))
        {
            Microphone.End(m_MicrophoneName);
        }

        // ������˷����
        m_RecordedClip = Microphone.Start(m_MicrophoneName, false,30, 16000);

        while (Microphone.GetPosition(null) <= 0) { }

        // ����¼��״̬���Э��
        StartCoroutine(DetectRecording());
    }

    /// <summary>
    /// ��ʼ�������
    /// </summary>
    /// <returns></returns>
    private IEnumerator DetectRecording()
    {
        while (true)
        {
            float[] samples = new float[128]; // ѡ����ʵ�������С
            int position = Microphone.GetPosition(null);
            if (position < samples.Length)
            {
                yield return null;
                continue;
            }

           

            try { m_RecordedClip.GetData(samples, position - samples.Length); } catch { }

            float rms = 0.0f;
            foreach (float sample in samples)
            {
                rms += sample * sample;
            }

            rms = Mathf.Sqrt(rms / samples.Length);

            if (rms > m_SilenceThreshold)
            {
                m_SilenceTimer = 0.0f; // ���þ�Ĭ��ʱ��

                //�����ؼ��ʻ��Ѽ���
                if (!m_AwakeState&&!m_ListeningState)
                {
                    StartVoiceListening();
                }
                //�ѻ��ѣ�����¼��
                if (m_AwakeState&&!m_IsRecording)
                {
                    StartRecording();
                }

            }
            else
            {

                if (!m_LockState)
                {
                    m_SilenceTimer += Time.deltaTime;
                }
               
                //�������Ѵʼ���
                if (m_ListeningState&&!m_AwakeState && m_SilenceTimer >= m_RecordingTimeLimit)
                {
                    StopVoiceListening();
                }

                //����״̬������˵��
                if (m_AwakeState&&m_IsRecording && m_SilenceTimer >= m_RecordingTimeLimit)
                {
                    StopRecording();
                }

                //��Ĭʱ������������Ի�״̬��j����ȴ�����
                if (m_AwakeState && !m_IsRecording && m_SilenceTimer >= m_LossAwakeTimeLimit)
                {
                    m_AwakeState=false;
                    PrintLog("Loss->�Ի������Ѷ�ʧ");
                }

            }

            yield return null;
  
        }
    }
    
    [SerializeField]private AudioSource m_Greeting;
    [SerializeField] private AudioClip m_GreatingVoice;
    /// <summary>
    /// �ؼ��ʼ����ص�
    /// </summary>
    /// <param name="_msg"></param>
    private void AwakeCallBack(string _msg)
    {
        if (_msg == m_AwakeKeyWord&&!m_AwakeState)
        {
            m_AwakeState = true;
            Debug.Log("ʶ�𵽹ؼ��ʣ�" + _msg);
            PrintLog("Link->�ѽ����Ի�����");
            if (m_Greeting)
            {
                m_Greeting.clip = m_GreatingVoice;
                m_Greeting.Play();
            }
        }
    }
    /// <summary>
    /// ��ʼ���Ѽ���
    /// </summary>
    private void StartVoiceListening()
    {
        m_ListeningState = true;
        m_VoiceAWake.StartRecognizer();
        PrintLog("��ʼ->ʶ���ѹؼ���");
 
    }

    /// <summary>
    /// ֹͣ���Ѽ���
    /// </summary>
    private void StopVoiceListening()
    {
        m_ListeningState = false;
        m_VoiceAWake.StopRecognizer();
        PrintLog("����->���ѹؼ���ʶ��");
        //StartCoroutine(WaitAndStopListen());
    }
 
    private IEnumerator WaitAndStopListen()
    {
        yield return new WaitForSeconds(1);
        m_ListeningState = false;
    }

    /// <summary>
    /// ��ʼ����˵������
    /// </summary>
    private void StartRecording()
    {
        m_SilenceTimer = 0.0f; // ���þ�Ĭ��ʱ��
        m_IsRecording = true;
        PrintLog("����¼�ƶԻ�...");
        //ֹͣ�����������¿�ʼ¼�ƣ��ᵼ�»��ѵ���һ֡������ʧ
        Microphone.End(m_MicrophoneName);
        m_RecordedClip = Microphone.Start(m_MicrophoneName, false, 30, 16000);
    }
    /// <summary>
    /// ����˵��
    /// </summary>
    private void StopRecording()
    {
        m_IsRecording = false;

        PrintLog("�Ự¼�ƽ���...");

        // ֹͣ��˷����
        Microphone.End(m_MicrophoneName);

        //������Ƶ����
        SetRecordedAudio();


    }

    /// <summary>
    /// ��ʼ¼�Ƽ���
    /// </summary>
    public void ReStartRecord()
    {
        m_RecordedClip = Microphone.Start(m_MicrophoneName, true, 30, 16000);
  
        m_LockState = false;
    }


    private void SetRecordedAudio()
    {
        m_LockState = true;

        m_ChatSample.AcceptClip(m_RecordedClip);
        //AudioSource.clip = m_RecordedClip;
        //AudioSource.Play();
    }

    /// <summary>
    /// �Ի������ص���������˷���
    /// </summary>
    private void SpeachDoneCallBack()
    {
        ReStartRecord();
    }

    [SerializeField] private Text m_PrintText;
    /// <summary>
    /// ��ӡ��־
    /// </summary>
    /// <param name="_log"></param>
    private void PrintLog(string _log)
    {
        m_PrintText.text = _log;
        //Debug.Log(_log);
    }

}
