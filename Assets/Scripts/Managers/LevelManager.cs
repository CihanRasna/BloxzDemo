using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class LevelManager : Singleton<LevelManager>, BaseLevel.ILevelDelegate
    {
        [SerializeField] private List<BaseLevel> levelList;
        public BaseLevel currentLevel { private set; get; }

        #region Events

        [HideInInspector] public UnityEvent levelWillLoad;
        [HideInInspector] public UnityEvent<BaseLevel> levelDidLoad;
        [HideInInspector] public UnityEvent<BaseLevel> levelDidStart;
        [HideInInspector] public UnityEvent<BaseLevel, float> levelDidSuccess;
        [HideInInspector] public UnityEvent<BaseLevel> levelDidFail;
        [HideInInspector] public UnityEvent<BaseLevel> levelWillUnload;
        [HideInInspector] public UnityEvent levelDidUnload;

        #endregion


        #region Level Loading

        public void LoadNextLevel()
        {
            if (currentLevel != null)
            {
                UnloadCurrentLevel();
            }

            levelWillLoad?.Invoke();
            var playedLevelCount = PersistManager.Instance.playedLevelCount;
            var totalLevelCount = levelList.Count;
            currentLevel = Instantiate(levelList[playedLevelCount % totalLevelCount]);
            currentLevel.listener = this;
            currentLevel.LevelLoaded();
        }
        
        public void LoadLastPlayedLevel()
        {
            var playedLastLevel = currentLevel;
            
            if (currentLevel != null)
            {
                UnloadCurrentLevel();
            }

            levelWillLoad?.Invoke();
            currentLevel = Instantiate(playedLastLevel);
            currentLevel.listener = this;
        }

        private void UnloadCurrentLevel()
        {
            if (currentLevel == null)
            {
                throw new Exception("UnloadCurrentLevel called while current level is null!");
            }

            levelWillUnload?.Invoke(currentLevel);

            currentLevel.gameObject.SetActive(false);
            Destroy(currentLevel.gameObject);
            currentLevel = null;

            levelDidUnload?.Invoke();
        }

        #endregion

        #region Level Delegates
        public void Level_DidLoad(BaseLevel baseLevel) => levelDidLoad?.Invoke(currentLevel);
        public void Level_DidStart(BaseLevel baseLevel) => levelDidStart?.Invoke(currentLevel);
        public void Level_DidSuccess(BaseLevel baseLevel, float score) => levelDidSuccess.Invoke(currentLevel, score);
        public void Level_DidFail(BaseLevel baseLevel) => levelDidFail?.Invoke(currentLevel);

        #endregion
    }
}