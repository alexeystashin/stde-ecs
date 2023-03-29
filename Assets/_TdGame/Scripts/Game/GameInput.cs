using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TdGame
{
    [Serializable]
    public class TouchObject
    {
        public int touchId;

        public bool isProcessed;

        public bool isEnded;

        public Vector3 startTouchPos;
        public Vector3 currentTouchPos;
    }

    public class GameInput : MonoBehaviour
    {
        public List<TouchObject> touches = new List<TouchObject>();

        void OnEnable ()
        {
            //Debug.Log($"GameInput.OnEnable");
        }
        
        void OnDisable ()
        {
            //Debug.Log($"GameInput.OnDisable");
            touches.Clear();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var touch = new TouchObject
                {
                    touchId = 0,
                    isProcessed = false,
                    isEnded = false,
                    startTouchPos = Input.mousePosition,
                    currentTouchPos = Input.mousePosition
                };
                touches.Add(touch);
            }
            if (Input.GetMouseButtonUp(0))
            {
                var touch = touches.FirstOrDefault(t => t.touchId == 0);
                if (touch != null)
                {
                    touch.currentTouchPos = Input.mousePosition;
                    touch.isEnded = true;
                }
            }
            if (Input.GetMouseButton(0))
            {
                var touch = touches.FirstOrDefault(t => t.touchId == 0);
                if (touch != null)
                {
                    touch.currentTouchPos = Input.mousePosition;
                }
            }

            for (var i = 0; i < touches.Count; i++)
            {
                var touch = touches[i];
                if (touch.isEnded && touch.isProcessed)
                {
                    touches.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
