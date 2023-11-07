using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloInteractive.XR.HoloKit.iOS;
using UnityEngine.XR;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using HoloInteractive.XR.HoloKit;

public class GoogleEyeManager : MonoBehaviour
    {
    [SerializeField] HandGestureRecognitionManager m_HandGestureRecognitionManager;

    public GameObject googleEyePrefab;
    public GameObject buttonToActivate;

    public GameObject finger;
    
    public GameObject mouse;    
    public GameObject mouse2; 
    
    public float lerpSpeed = 50f;

    private GameObject currentGoogleEye = null;

    private float lastCreateEyeTime = 0f;

    private List<GameObject> eyes = new();

    private Transform centerEyePose;

    private const float CREATE_EYE_COOLDOWN = 3f;

    private const float EYE_DRAG_PROTECTION_TIME = 1f;

    enum State{
        FingerPinched,
        None
    }

   // State state;

    int numOfEyes = 0;

    private void Start()
    {
        // Register the callback
        m_HandGestureRecognitionManager.OnHandGestureChanged += OnHandGestureChanged;

        var holokitCameraManager = FindObjectOfType<HoloKitCameraManager>();
        centerEyePose = holokitCameraManager.CenterEyePose;
    }

    private void OnHandGestureChanged(HandGesture handGesture)
    {
        // todo: 
        switch(handGesture){
            case HandGesture.Five:
                Debug.Log("Five");
                break;
            case HandGesture.Pinched:
                Debug.Log("Piched");
                CreateGoogleEye();
                //n++;
                break;
            case HandGesture.None:
                Debug.Log("None");
                if (currentGoogleEye != null) {
                    if (Time.time - lastCreateEyeTime > EYE_DRAG_PROTECTION_TIME) {
                        eyes.Add(currentGoogleEye);
                        currentGoogleEye = null;
                        // New Google Eye placed
                        numOfEyes++;
                        if (numOfEyes  == 2) {
                            // TODO: A new pair of eye created
                            // The first eye of this pair
                            GameObject firstEye = eyes[eyes.Count - 2];
                            // The second eye of this pair
                            GameObject secondEye = eyes[eyes.Count - 1];

                            
                             
                            // Create the corresponding mouse
                          //  var mouseInstance = Instantiate(mousePrefab);
                            Vector3 newPosition = (firstEye.transform.position + secondEye.transform.position) / 2f + new Vector3(0f, -0.1f, 0f);
                            mouse.transform.position = newPosition;
                        }

                          if(numOfEyes == 4){

                          GameObject firstEye2 = eyes[eyes.Count - 2];
                            // The second eye of this pair
                         GameObject secondEye2 = eyes[eyes.Count - 1];
                             
                            // Create the corresponding mouse
                           // var mouseInstance2 = Instantiate(mousePrefab2);
                            Vector3 newPosition2 = (firstEye2.transform.position + secondEye2.transform.position) / 2f + new Vector3(0f, -0.1f, 0f);
                            mouse2.transform.position = newPosition2;
                            
                           }
                           
                           
                           
                           
                          //   if (mouseInstance != null && buttonToActivate != null)*/
        //{
                             // 检测对象是否存在于场景中
                          /*if (mouseInstance.activeInHierarchy) // 如果 objectToDetect 在场景中
                              {
                                     buttonToActivate.SetActive(true); // 激活按钮
                                 }*/
           
        //}
                            // Mouse.transform.forward = firstEye.transform.forward;
                            // Mouse.transform.right = (firstEye.transform.position+secondEye.transform.position) / 2f;
                            // Mouse.transform.rotation = firstEye.transform.rotation;
                        
                    }
                }
                break;
        }
    }

    void Update() {
        if (m_HandGestureRecognitionManager.HandGesture == HandGesture.Pinched) 
        {
            if (currentGoogleEye != null) {
                Vector3 targetPosition = finger.transform.position;
                currentGoogleEye.transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
                currentGoogleEye.transform.LookAt(centerEyePose.position);
                             
            
                // TODO: LOOK AT
            }
        }

        // Make all eyes look at the user camera            
        foreach (var googleEye in eyes) 
        {
            googleEye.transform.LookAt(centerEyePose.position);
        }
    }

    private void CreateGoogleEye()
    {
        if (Time.time - lastCreateEyeTime < CREATE_EYE_COOLDOWN) return;

        Debug.Log("Create eye");
        currentGoogleEye = Instantiate(googleEyePrefab, finger.transform.position, Quaternion.identity);
        lastCreateEyeTime = Time.time;
        
    }
} 
