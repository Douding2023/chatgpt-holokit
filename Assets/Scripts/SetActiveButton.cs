using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateButton : MonoBehaviour
{
   // GameObject mouseInstance = GameObject.Find("MousePrefab(Clone)"); // 要检测的物体
    public GameObject buttonToActivate; // 要激活的按钮

    void Update()
    {GameObject mouseInstance = GameObject.Find("MousePrefab(Clone)");
        if (mouseInstance != null && buttonToActivate != null)
        {
            // 检测对象是否存在于场景中
            if (mouseInstance.activeInHierarchy) // 如果 objectToDetect 在场景中
            {
                buttonToActivate.SetActive(true); // 激活按钮
            }
           
        }
    }
}