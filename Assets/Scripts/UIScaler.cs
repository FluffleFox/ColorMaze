using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaler : MonoBehaviour
{
    [System.Serializable]
    public struct UIObject
    {
        [Range(0.0f,100.0f)]
        public float Width;
        [Range(0.0f, 100.0f)]
        public float Height;
        public Vector2 Pivot;
    }

    public UIObject[] UIObjects;
    void Start()
    {
        
    }

}
