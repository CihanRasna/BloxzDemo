namespace Panels
{
    public interface ITutorialPanelDelegate : IPanelDelegate
    {
        void TutorialPanel_Tapped(TutorialPanel tutorialPanel);
    }
    public class TutorialPanel : Panel<ITutorialPanelDelegate>
    {
        
    }
}