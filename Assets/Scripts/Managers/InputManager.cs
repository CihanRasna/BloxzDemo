namespace Managers
{
    public class InputManager : Singleton<InputManager>
    {
        public bool isFirstTouch;

        protected override void Awake()
        {
            base.Awake();
        }
    }
}