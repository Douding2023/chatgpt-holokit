using UnityEngine;
using System.Collections;

public class AudioMouthController : MonoBehaviour
{

    public SkinnedMeshRenderer meshRenderer; // ģ�͵�SkinnedMeshRenderer���
    public int blendShapeIndex; // blendshape����
    public float blendWeightMultiplier = 100f; // blendshapeȨ�ر�����
    public float smoothTime = 0.1f; // ƽ������ʱ��

    [SerializeField]private AudioSource audioSource; // ��ƵԴ

    private float blendWeight; // blendshapeȨ��
    private float blendWeightVelocity; // blendshapeȨ�ص��ٶ�

    void Update()
    {
        // �����Ƶ���ڲ��ţ���ƽ������blendshape��Ȩ��
        if (audioSource.isPlaying)
        {
            float amplitude = GetAmplitude();
            blendWeight = Mathf.SmoothDamp(blendWeight, amplitude * blendWeightMultiplier, ref blendWeightVelocity, smoothTime);
            meshRenderer.SetBlendShapeWeight(blendShapeIndex, blendWeight);
        }
        else
        {
            blendWeight = Mathf.SmoothDamp(blendWeight, 0f, ref blendWeightVelocity, smoothTime);
            meshRenderer.SetBlendShapeWeight(blendShapeIndex, blendWeight);
        }
    }

    // ��ȡ��Ƶ���
    float GetAmplitude()
    {
        float[] samples = new float[512];
        audioSource.GetOutputData(samples, 0);
        float sum = 0f;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += Mathf.Abs(samples[i]);
        }
        return sum / samples.Length;
    }
}
