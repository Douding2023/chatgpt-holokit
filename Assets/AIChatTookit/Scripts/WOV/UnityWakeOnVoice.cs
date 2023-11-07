using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
#if UNITY_STANDALONE_WIN
using UnityEngine.Windows.Speech;
#endif
/// <summary>
/// unity������������ windows��Ч
/// </summary>
public class UnityWakeOnVoice : WOV
{
    /// <summary>
    /// �ؼ���
    /// </summary>
    [SerializeField]
    private string[] m_Keywords = { "����" };//�ؼ���
    /// <summary>
    /// �ؼ���ʶ����
    /// </summary>
#if UNITY_STANDALONE_WIN
    private KeywordRecognizer m_Recognizer;
    // Use this for initialization
    void Start()
    {
        //����һ���ؼ���ʶ����
        m_Recognizer = new KeywordRecognizer(m_Keywords);
        Debug.Log("����ʶ�����ɹ�");
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;

    }
    
    /// <summary>
    /// ��ʼʶ��
    /// </summary>
    public override void StartRecognizer()
    {
        if (m_Recognizer == null)
            return;

        m_Recognizer.Start();
    }
    /// <summary>
    /// ����ʶ��
    /// </summary>
    public override void StopRecognizer()
    {
        if (m_Recognizer == null)
            return;

        m_Recognizer.Stop();
    }

    /// <summary>
    /// ʶ��ؼ��ʻص�
    /// </summary>
    /// <param name="args"></param>
    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0}", args.text);
        string _keyWord = builder.ToString();
        Debug.Log("ʶ������׽���ؼ��ʣ�"+_keyWord);
        OnAwakeOnVoice(_keyWord);
    }
    #endif
}
