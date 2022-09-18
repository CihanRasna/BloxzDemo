using UnityEngine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public enum GameState
        {
            Loaded,
            Started,
            Succeed,
            Failed
        }

        protected override void Start()
        {
            base.Start();
            LevelManager.Instance.LoadNextLevel();
        }

        [SerializeField] private LetterListContainer letterListContainer;

        public TableMatrix ReturnRandomLetter()
        {
            return letterListContainer.letters[Random.Range(0, letterListContainer.letters.Count)];
        }
        
    }
}