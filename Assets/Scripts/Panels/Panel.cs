using System;
using Managers;
using UnityEngine;

namespace Panels
{

    public interface IPanelDelegate //Could be useful?
    {
        
    }

    [RequireComponent(typeof(RectTransform))]
    public class Panel : MonoBehaviour , ISafeAreaTrackerDelegate
    {

        [Flags]
        public enum SafeAreaConformation
        {
            None = 0,
            Top = 1 << 0,
            Right = 1 << 1,
            Bottom = 1 << 2,
            Left = 1 << 3,
            Banner = 1 << 4,
            All = ~0
        }

        [SerializeField, HideInInspector] public SafeAreaConformation _safeAreaConformation = SafeAreaConformation.All;
        public SafeAreaConformation safeAreaConformation
        {
            get => _safeAreaConformation;
            set
            {
                _safeAreaConformation = value;
                if (Application.isPlaying)
                {
                    SafeAreaTracker_DidDetectChange(UIManager.Instance.safeAreaTracker);
                }
            }
        }

        public RectTransform rectTransform { private set; get; }

        #region Life Cycle

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            UIManager.Instance.panels.Add(this);
        }

        protected virtual void Start()
        {
            UIManager.Instance.safeAreaTracker.AddListener(this);
        }

        #endregion



    #region Safe Area

        public void SafeAreaTracker_DidDetectChange(SafeAreaTracker safeAreaTracker)
        {
            var safeArea = safeAreaTracker.safeArea;
            var screenSize = safeAreaTracker.screenSize;

            if (!safeAreaConformation.HasFlag(SafeAreaConformation.Left))
            {
                safeArea.width += safeArea.xMin;
                safeArea.x = 0;
            }
            
            if (!safeAreaConformation.HasFlag(SafeAreaConformation.Right))
            {
                safeArea.width += screenSize.x - safeArea.xMax;
            }

            if (!safeAreaConformation.HasFlag(SafeAreaConformation.Bottom))
            {
                safeArea.height += safeArea.yMin;
                safeArea.y = 0;
            }
            
            if (!safeAreaConformation.HasFlag(SafeAreaConformation.Top))
            {
                safeArea.height += screenSize.y - safeArea.yMax;
            }

            if (safeAreaConformation.HasFlag(SafeAreaConformation.Banner))
            {
                var bannerHeight = safeAreaTracker.bannerAdHeight;
                safeArea.y += bannerHeight;
                safeArea.height -= bannerHeight;
            }
            
            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= screenSize.x;
            anchorMin.y /= screenSize.y;
            anchorMax.x /= screenSize.x;
            anchorMax.y /= screenSize.y;

            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            
            DidApplySafeArea();
        }

    #endregion



    #region Appearance

        public void Display()
        {
            WillDisplay();
            gameObject.SetActive(true);
            DidDisplay();
        }

        public void Hide()
        {
            WillHide();
            gameObject.SetActive(false);
            DidHide();
        }

    #endregion



    #region Events

        protected virtual void DidApplySafeArea()
        {
            
        }

        protected virtual void WillDisplay()
        {
            
        }

        protected virtual void DidDisplay()
        {
            
        }

        protected virtual void WillHide()
        {
            
        }

        protected virtual void DidHide()
        {
            
        }

    #endregion

    }
    
    public class Panel<T> : Panel where T : IPanelDelegate
    {
        public T listener;
    }

}