using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceInputs : MonoBehaviour
{

    /// <summary>
    /// ¼�Ƶ���Ƶ����
    /// </summary>
    public int m_RecordingLength = 5;

    public AudioClip recording;

    /// <summary>
    /// WebGL������
    /// </summary>
    //[SerializeField]private SignalManager signalManager;
    /// <summary>
    /// ��ʼ¼������
    /// </summary>
    public void StartRecordAudio()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        signalManager.onAudioClipDone = null;
        signalManager.StartRecordBinding();
#else
        recording = Microphone.Start(null, false, m_RecordingLength, 16000);
       
        #endif
    }

    /// <summary>
    /// ����¼�ƣ�����audioClip
    /// </summary>
    /// <param name="_callback"></param>
    public void StopRecordAudio(Action<AudioClip> _callback)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        signalManager.onAudioClipDone += _callback;
        signalManager.StopRecordBinding();
#else
        Microphone.End(null);
        _callback(recording);
        
#endif

    }

}