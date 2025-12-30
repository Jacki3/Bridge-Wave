using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace BogGames.Controls
{
    public enum SwipeDirection { Left, Right }

    [System.Serializable] public class SwipeEvent : UnityEvent<SwipeDirection> {}

    public class SwipeAnywhere : MonoBehaviour
    {
        public float minSwipeInches = 0.25f;
        public float minSwipePixelsFallback = 160f;
        [Range(0f, 89f)] public float maxAngleFromHorizontal = 30f;
        public bool ignoreIfOverUI = true;

        public UnityEvent onSwipeLeft;
        public UnityEvent onSwipeRight;
        public SwipeEvent onSwipe;

        bool tracking;
        int pointerId;
        Vector2 startPos;
        float startTime;

        float TanMax => Mathf.Tan(maxAngleFromHorizontal * Mathf.Deg2Rad);

        void Update()
        {
    #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            HandleMouse();
    #endif
            HandleTouch();
        }

        void HandleMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (ignoreIfOverUI && EventSystem.current && EventSystem.current.IsPointerOverGameObject(-1)) return;
                tracking = true; pointerId = -1; startPos = Input.mousePosition; startTime = Time.unscaledTime;
            }
            if (tracking && pointerId == -1 && Input.GetMouseButtonUp(0))
            {
                TryCommitSwipe(Input.mousePosition); tracking = false;
            }
        }

        void HandleTouch()
        {
            if (Input.touchCount == 0) return;
            if (!tracking)
            {
                foreach (var t in Input.touches)
                {
                    if (t.phase == TouchPhase.Began)
                    {
                        if (ignoreIfOverUI && EventSystem.current && EventSystem.current.IsPointerOverGameObject(t.fingerId)) return;
                        tracking = true; pointerId = t.fingerId; startPos = t.position; startTime = Time.unscaledTime; return;
                    }
                }
            }
            if (tracking)
            {
                foreach (var t in Input.touches)
                {
                    if (t.fingerId != pointerId) continue;
                    if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                    {
                        TryCommitSwipe(t.position); tracking = false; return;
                    }
                }
            }
        }

        void TryCommitSwipe(Vector2 endPos)
        {
            Vector2 delta = endPos - startPos;
            if (Mathf.Abs(delta.x) <= 0.0001f) return;
            if (Mathf.Abs(delta.y) > TanMax * Mathf.Abs(delta.x)) return;
            if (delta.magnitude < MinPixels()) return;

            var dir = delta.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
            if (dir == SwipeDirection.Left) onSwipeLeft?.Invoke(); else onSwipeRight?.Invoke();
            onSwipe?.Invoke(dir);
        }

        float MinPixels()
        {
            float dpi = Screen.dpi;
            return dpi <= 0f ? minSwipePixelsFallback : Mathf.Max(8f, minSwipeInches * dpi);
        }
    }
}
