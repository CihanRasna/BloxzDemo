using Managers;

namespace Panels
{
    public interface IFailedPanelDelegate : IPanelDelegate
    {
        void FailedPanel_RetryButtonTapped(FailedPanel failedPanel);
    }
    public class FailedPanel : Panel<IFailedPanelDelegate>
    {
        public void Retry()
        {
            LevelManager.Instance.LoadNextLevel();
        }
    }
}