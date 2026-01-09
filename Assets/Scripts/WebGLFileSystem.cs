using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class WebGLFileSystem : MonoBehaviour
{
    #if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void SyncFiles();
    #endif

    public static void Sync()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            SyncFiles();
        #endif
    }
}
