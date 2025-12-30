using UnityEngine;

namespace BogGames.UI
{
    public class CanvasGroupFloatAndFade : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float lifetime = 1.2f;
        [SerializeField] private float yAdjustment = 60f;
        [SerializeField] private float preFadeAlpha = 1f;
        [SerializeField] private float targetAlpha = 0f;
        [SerializeField] private bool resetOnEnd = true;

        private RectTransform rect;
        private float elapsed;
        private float startAlpha;
        private Vector2 startPos;
        private Vector2 endPos;
        private bool playing;

        private Vector2 initialPos;
        private float initialAlpha;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
            initialPos = rect.anchoredPosition;
            initialAlpha = canvasGroup.alpha;
        }

        void Update()
        {
            if (!playing) return;

            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / lifetime);

            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);

            if (t >= 1f)
            {
                playing = false;
                if (resetOnEnd) ResetToInitial();
            }
        }

        public void Play()
        {
            canvasGroup.alpha = preFadeAlpha;
            startAlpha = preFadeAlpha;
            startPos = rect.anchoredPosition;
            endPos = startPos + new Vector2(0f, yAdjustment);
            elapsed = 0f;
            playing = true;
        }

        public void SetAndPlay(Vector2 anchoredPos)
        {
            rect.anchoredPosition = anchoredPos;
            Play();
        }

        public void ResetToInitial()
        {
            playing = false;
            rect.anchoredPosition = initialPos;
            canvasGroup.alpha = initialAlpha;
        }

        public void SetParams(float newYAdjustment, float newPreFadeAlpha, float newTargetAlpha, float newLifetime)
        {
            yAdjustment = newYAdjustment;
            preFadeAlpha = newPreFadeAlpha;
            targetAlpha = newTargetAlpha;
            lifetime = newLifetime;
        }
    }
}
