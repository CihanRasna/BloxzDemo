using System.Collections.Generic;
using Panels;
using UnityEngine;

namespace Managers
{
    public sealed class UIManager : Singleton<UIManager>, IGamePanelDelegate,
        IFailedPanelDelegate, ISucceedPanelDelegate, ITutorialPanelDelegate
    {
        public SafeAreaTracker safeAreaTracker { get; private set; }
        [field: SerializeField] public Canvas canvas { get; private set; }

        [field: Header("Panels"), SerializeField]
        public GamePanel gamePanel { get; private set; }
        [field: SerializeField] public SucceedPanel succeedPanel { get; private set; }
        [field: SerializeField] public FailedPanel failedPanel { get; private set; }
        [field: SerializeField] public TutorialPanel tutorialPanel { get; private set; }

        public List<Panel> panels;

        protected override void Awake()
        {
            base.Awake();
            safeAreaTracker = new SafeAreaTracker();

            LevelManager.Instance.levelDidLoad.AddListener(LevelDidLoad);
            LevelManager.Instance.levelDidStart.AddListener(LevelDidStart);
            LevelManager.Instance.levelDidSuccess.AddListener(LevelDidSuccess);
            LevelManager.Instance.levelDidFail.AddListener(LevelDidFail);
            LevelManager.Instance.levelWillUnload.AddListener(LevelWillUnload);
        }

        protected override void Start()
        {
            base.Start();
            
            succeedPanel.listener = this;
            tutorialPanel.listener = this;
            failedPanel.listener = this;
            succeedPanel.listener = this;
        }

        protected override void OnApplicationQuit()
        {
            if (Quitting == false)
            {
                LevelManager.Instance.levelDidLoad.RemoveListener(LevelDidLoad);
                LevelManager.Instance.levelDidStart.RemoveListener(LevelDidStart);
                LevelManager.Instance.levelDidSuccess.RemoveListener(LevelDidSuccess);
                LevelManager.Instance.levelDidFail.RemoveListener(LevelDidFail);
                LevelManager.Instance.levelWillUnload.RemoveListener(LevelWillUnload);
            }

            base.OnApplicationQuit();
        }

        protected override void OnDestroy()
        {
            if (Quitting == false)
            {
                LevelManager.Instance.levelDidLoad.RemoveListener(LevelDidLoad);
                LevelManager.Instance.levelDidStart.RemoveListener(LevelDidStart);
                LevelManager.Instance.levelDidSuccess.RemoveListener(LevelDidSuccess);
                LevelManager.Instance.levelDidFail.RemoveListener(LevelDidFail);
                LevelManager.Instance.levelWillUnload.RemoveListener(LevelWillUnload);
            }
            
            base.OnDestroy();
        }

        private void Update()
        {
            safeAreaTracker.Refresh();
        }
        
        public void HideAllPanels()
        {
            foreach (var panel in panels)
            {
                panel.Hide();
            }
        }

        #region Level Events

        private void LevelDidLoad(BaseLevel level)
        {
            canvas.sortingOrder = -1;
            HideAllPanels();
            tutorialPanel.Display();
            var count = PersistManager.Instance.playedLevelCount + 1;
            gamePanel.GetLevelIdx(count);
            tutorialPanel.StartTutorialAnimation(true);
        }

        private void LevelDidStart(BaseLevel level)
        {
            tutorialPanel.StartTutorialAnimation(false);
            tutorialPanel.Hide();
            gamePanel.Display();
        }

        private void LevelDidSuccess(BaseLevel level, float score)
        {
            canvas.sortingOrder = 1;
            HideAllPanels();
            succeedPanel.Display();
        }

        private void LevelDidFail(BaseLevel level)
        {
            canvas.sortingOrder = 1;
            HideAllPanels();
            failedPanel.Display();
        }

        private void LevelWillUnload(BaseLevel level)
        {
        }

        #endregion

        #region Panel Interfaces

        public void FailedPanel_RetryButtonTapped(FailedPanel failedPanel)
        {
        }

        public void SucceedPanel_NextButtonTapped(SucceedPanel succeedPanel)
        {
        }

        public void TutorialPanel_Tapped(TutorialPanel tutorialPanel)
        {
        }

        #endregion
    }
}