using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Panels
{
    public interface ISafeAreaTrackerDelegate
    {
        void SafeAreaTracker_DidDetectChange(SafeAreaTracker safeAreaTracker);
    }

    [Serializable]
    public class SafeAreaTracker
    {
        public Rect safeArea { private set; get; } = Rect.zero;
        public Vector2Int screenSize { private set; get; } = Vector2Int.zero;
        public ScreenOrientation orientation { private set; get; } = ScreenOrientation.AutoRotation;
        public float bannerAdHeight { private set; get; } = 0;

        private List<Object> listeners = new List<Object>();
        private bool shouldInvokeCallbackImmediately = false;


        #region Listeners

        public void AddListener(ISafeAreaTrackerDelegate listener)
        {
            var obj = listener as Object;

            if (obj == null || listeners.Contains(obj))
            {
                throw new Exception("Invalid SafeAreaTrackerDelegate! Listener: " + listener);
            }

            listeners.Add(obj);

            if (shouldInvokeCallbackImmediately)
            {
                InvokeListeners();
            }
        }

        private void InvokeListeners()
        {
            listeners.RemoveAll(l =>
            {
                if (l is ISafeAreaTrackerDelegate listener)
                {
                    listener.SafeAreaTracker_DidDetectChange(this);
                    return false;
                }

                return true;
            });
        }

        #endregion


        #region Screen Tracking

        public void Refresh()
        {
            var currentSafeArea = Screen.safeArea;

            if (currentSafeArea == safeArea &&
                Screen.width == screenSize.x &&
                Screen.height == screenSize.y &&
                Screen.orientation == orientation) return;
            screenSize = new Vector2Int(Screen.width, Screen.height);
            orientation = Screen.orientation;
            safeArea = currentSafeArea;

            InvokeListeners();
            shouldInvokeCallbackImmediately = true;
        }

        #endregion
    }
}