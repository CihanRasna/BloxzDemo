using Managers;

namespace Panels
{
    public interface ISucceedPanelDelegate : IPanelDelegate
    {
        void SucceedPanel_NextButtonTapped(SucceedPanel succeedPanel);
    }
    public class SucceedPanel : Panel<ISucceedPanelDelegate>
    {
        public void NextLevel()
        {
            LevelManager.Instance.LoadNextLevel();
        }
    }
}