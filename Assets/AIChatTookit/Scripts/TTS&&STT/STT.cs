using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class STT : MonoBehaviour
{

    /// <summary>
    /// ����ʶ��api��ַ
    /// </summary>
    [SerializeField] protected string m_SpeechRecognizeURL = String.Empty;
    /// <summary>
    /// ���㷽�����õ�ʱ��
    /// </summary>
    [SerializeField] protected Stopwatch stopwatch = new Stopwatch();
    /// <summary>
    /// ����ʶ��
    /// </summary>
    /// <param name="_clip"></param>
    /// <param name="_callback"></param>
    public virtual void SpeechToText(AudioClip _clip,Action<string> _callback)
    {
       
    }

    /// <summary>
    /// ����ʶ��
    /// </summary>
    /// <param name="_audioData"></param>
    /// <param name="_callback"></param>
    public virtual void SpeechToText(byte[] _audioData, Action<string> _callback)
    {

    }


}
