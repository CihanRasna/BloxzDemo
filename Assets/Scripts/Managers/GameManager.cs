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
    }
}