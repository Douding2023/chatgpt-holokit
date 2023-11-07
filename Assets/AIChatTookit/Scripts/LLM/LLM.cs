using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using UnityEngine;

public class LLM:MonoBehaviour
{
    /// <summary>
    /// api��ַ
    /// </summary>
    [SerializeField] protected string url;
    /// <summary>
    /// ��ʾ�ʣ�����Ϣһ����
    /// </summary>
    [Header("���͵���ʾ���趨")]
    [SerializeField] protected string m_Prompt = string.Empty;
    /// <summary>
    /// ����
    /// </summary
    [Header("���ûظ�������")]
    [SerializeField] protected string lan="����";
    /// <summary>
    /// �����ı�������
    /// </summary>
    [Header("�����ı�������")]
    [SerializeField] protected int KeepCount = 15;
    /// <summary>
    /// ����Ի�
    /// </summary>
    [SerializeField] public List<SendData> m_DataList = new List<SendData>();
    /// <summary>
    /// ���㷽�����õ�ʱ��
    /// </summary>
    [SerializeField] protected Stopwatch stopwatch=new Stopwatch();
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public virtual void PostMsg(string _msg,Action<string> _callback) {
        //��������������
        CheckHistory();
        //��ʾ�ʴ���
        string message = "��ǰΪ��ɫ�������趨��" + m_Prompt +
            " �ش�����ԣ�" + lan +
            " ���������ҵ����ʣ�" + _msg;

        //���淢�͵���Ϣ�б�
        m_DataList.Add(new SendData("user", message));

        StartCoroutine(Request(message, _callback));
    }

    public virtual IEnumerator Request(string _postWord, System.Action<string> _callback)
    {
        yield return new WaitForEndOfFrame();
          
    }

    /// <summary>
    /// ���ñ�������������������ֹ̫��
    /// </summary>
    public virtual void CheckHistory()
    {
        if(m_DataList.Count> KeepCount)
        {
            m_DataList.RemoveAt(0);
        }
    }

    [Serializable]
    public class SendData
    {
        [SerializeField] public string role;
        [SerializeField] public string content;
        public SendData() { }
        public SendData(string _role, string _content)
        {
            role = _role;
            content = _content;
        }

    }

}
