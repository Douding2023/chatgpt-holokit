using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Wake-on-voice �������� base��
/// </summary>
public class WOV : MonoBehaviour
{

    /// <summary>
    /// �ؼ��ʻص�
    /// </summary>
    protected Action<string> OnKeywordRecognizer;
    /// <summary>
    /// �󶨻��ѻص�
    /// </summary>
    /// <param name=""></param>
    /// <param name="_callback"></param>
    public virtual void OnBindAwakeCallBack(Action<string> _callback)
    {
        OnKeywordRecognizer += _callback;
    }
    /// <summary>
    /// ��ʼʶ��
    /// </summary>
    public virtual void StartRecognizer()
    {

    }
    /// <summary>
    /// ����ʶ��
    /// </summary>
    public virtual void StopRecognizer()
    {

    }
    /// <summary>
    /// ���Ѵʻص�
    /// </summary>
    /// <param name="_msg"></param>
    protected virtual void OnAwakeOnVoice(string _msg)
    {
        if(OnKeywordRecognizer==null)
            return;

        OnKeywordRecognizer(_msg);
    }




}
