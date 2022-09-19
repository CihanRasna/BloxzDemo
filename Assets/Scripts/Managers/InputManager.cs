using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    public class InputManager : Singleton<InputManager>, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public bool isFirstTouch;
        private Camera _camera;
        [SerializeField] private LayerMask rayCastLayers;
        [SerializeField] private DraggableItem draggableTransform;
        private Vector3 offset;
        private Vector3 screenPoint;

        protected override void Awake()
        {
            base.Awake();
            _camera = Camera.main;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            var level = LevelManager.Instance.currentLevel as Level;

            if (!isFirstTouch)
            {
                isFirstTouch = true;
                level.LevelStarted();
            }

            if (level.state == BaseLevel.State.Started)
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 10000f, rayCastLayers))
                {
                    hit.collider.TryGetComponent(out DraggableItem draggableItem);

                    if (draggableItem.OnPlace)
                    {
                        draggableItem.TryToGetPlace(true);
                        return;
                    }

                    draggableTransform = draggableItem;
                    var draggablePosition = draggableTransform.transform.position;
                    screenPoint = _camera.WorldToScreenPoint(draggablePosition);
                    offset = draggablePosition -
                             _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                                 screenPoint.z));
                    //draggableItem.transform.position = screenPoint + offset;
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (draggableTransform)
            {
                var curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                var currentPosition = _camera.ScreenToWorldPoint(curScreenPoint) + offset;
                currentPosition.y = 2f;
                draggableTransform.transform.position = currentPosition + offset;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (draggableTransform)
            {
                draggableTransform.TryToGetPlace();
                draggableTransform = null;
            }
        }
    }
}