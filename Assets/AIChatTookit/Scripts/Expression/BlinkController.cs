using System.Collections;
using System.Linq;
using UnityEngine;

public class BlinkController : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public int blinkBlendIndex; // գ�۱�����BlendShapes�е�����
    public float blinkWeight = 0.0f; // գ�۱���ĳ�ʼȨ��ֵ
    public float blinkDuration = 0.2f; // գ�۶�������ʱ��
    public float blinkInterval = 3.0f; // գ�ۼ��ʱ��

    private float blinkTimer = 0.0f; // ��ʱ�������ڿ���գ�ۼ��
    void Start()
    {
        // ����գ�۱���ĳ�ʼȨ��ֵ
        skinnedMeshRenderer.SetBlendShapeWeight(blinkBlendIndex, blinkWeight);
    }

    void Update()
    {
        blinkTimer += Time.deltaTime;

        // �����ʱ��������գ�ۼ��ʱ�䣬�ʹ���գ�۶���
        if (blinkTimer >= blinkInterval)
        {
            StartCoroutine(BlinkCoroutine());
            blinkTimer = 0.0f; // ���ü�ʱ��
        }
    }

    IEnumerator BlinkCoroutine()
    {
        // ��գ�۱����Ȩ��ֵ�𽥱�Ϊ100��Ȼ�����𽥻ָ�Ϊ0��ʵ��գ�۶���
        for (float t = 0.0f; t < blinkDuration; t += Time.deltaTime)
        {
            float weight = Mathf.Lerp(blinkWeight, 100.0f, t / blinkDuration);
 
            skinnedMeshRenderer.SetBlendShapeWeight(blinkBlendIndex, weight);
            yield return null;
        }

        for (float t = 0.0f; t < blinkDuration; t += Time.deltaTime)
        {
            float weight = Mathf.Lerp(100.0f, blinkWeight, t / blinkDuration);
            skinnedMeshRenderer.SetBlendShapeWeight(blinkBlendIndex, weight);
            yield return null;
        }

        // ��գ�۱����Ȩ��ֵ�ָ�Ϊ��ʼֵ
        skinnedMeshRenderer.SetBlendShapeWeight(blinkBlendIndex, blinkWeight);
    }
}
