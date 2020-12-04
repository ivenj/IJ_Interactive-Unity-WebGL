//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class WwiseMuteAll : MonoBehaviour
//{
//    public bool muteDebug;
//    public bool mute;
//    public AK.Wwise.Event muteAll;
//    public AK.Wwise.Event unMuteAll;
//    public GameObject listener;

//    bool isMuted;

//    void Start()
//    {
//        if (mute)
//        {
//            muteAll.Post(listener);
//            isMuted = true;
//        }
//    }

//    void Update()
//    {
//        if (!isMuted && Input.GetKeyDown(KeyCode.M) && muteDebug)
//        {
//            muteAll.Post(listener);
//            isMuted = true;
//        }
//        else if (isMuted && Input.GetKeyDown(KeyCode.M) && muteDebug)
//        {
//            unMuteAll.Post(listener);
//            isMuted = false;
//        }
//    }
//}
