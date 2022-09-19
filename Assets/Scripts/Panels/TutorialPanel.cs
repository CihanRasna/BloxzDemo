using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Panels
{
    public interface ITutorialPanelDelegate : IPanelDelegate
    {
        void TutorialPanel_Tapped(TutorialPanel tutorialPanel);
    }
    public class TutorialPanel : Panel<ITutorialPanelDelegate>
    {
        [SerializeField] private Animator handAnimator;
        private static readonly int Scale = Animator.StringToHash("Scale");

        public void StartTutorialAnimation(bool value)
        {
            handAnimator.gameObject.SetActive(value);
            if (value)
            {
                handAnimator.transform.DOScale(1f, 2f).OnComplete(() => handAnimator.enabled = true);
            }
            else
            {
                handAnimator.transform.localScale = Vector3.zero;
                handAnimator.enabled = false;
            }
        }
    }
}