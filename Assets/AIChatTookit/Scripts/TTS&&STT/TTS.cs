using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TTS : MonoBehaviour
{
    /// <summary>
    /// �����ϳɵ�api��ַ
    /// </summary>
    [SerializeField] protected string m_PostURL = string.Empty;
    /// <summary>
    /// ���㷽�����õ�ʱ��
    /// </summary>
    [SerializeField] protected Stopwatch stopwatch = new Stopwatch();
    /// <summary>
    /// �����ϳɣ�������Ƶ
    /// </summary>
    /// <param name="_msg"></param>
    /// <param name="_callback"></param>
    public virtual void Speak(string _msg,Action<AudioClip> _callback) {}
    /// <summary>
    /// �ϳ�����������Ƶ��ͬʱ���غϳɵ��ı�
    /// </summary>
    /// <param name="_msg"></param>
    /// <param name="_callback"></param>
    public virtual void Speak(string _msg, Action<AudioClip,string> _callback) { }


}
