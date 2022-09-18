using TMPro;
using UnityEngine;

namespace Panels
{
    public interface IGamePanelDelegate : IPanelDelegate
    {
            
    }
    
    public class GamePanel : Panel<IGamePanelDelegate>
    {
        [SerializeField] private TextMeshProUGUI levelText;

        public void GetLevelIdx(int idx)
        {
            //levelText.text = $"Level {idx}";
        }
    }
}
