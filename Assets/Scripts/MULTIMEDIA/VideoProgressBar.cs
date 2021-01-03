using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
namespace GameSpace {
    public class VideoProgressBar : MonoBehaviour, IDragHandler, IPointerDownHandler {
        [SerializeField]
        private VideoPlayer videoPlayer;
        [SerializeField]
        private GameObject skiptext;

        private Image progress;

        private void Awake () {
            progress = GetComponent<Image> ();
            skiptext.GetComponent<TextMeshProUGUI> ().text = LangDataset.getText ("skip");
        }

        private void Update () {
            if (videoPlayer.frameCount > 0)
                progress.fillAmount = (float) videoPlayer.frame / (float) videoPlayer.frameCount;
        }

        public void OnDrag (PointerEventData eventData) {
            TrySkip (eventData);
        }

        public void OnPointerDown (PointerEventData eventData) {
            TrySkip (eventData);
        }

        private void TrySkip (PointerEventData eventData) {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle (
                    progress.rectTransform, eventData.position, null, out localPoint)) {
                float pct = Mathf.InverseLerp (progress.rectTransform.rect.xMin, progress.rectTransform.rect.xMax, localPoint.x);
                SkipToPercent (pct);
            }
        }

        private void SkipToPercent (float pct) {
            var frame = videoPlayer.frameCount * pct;
            videoPlayer.frame = (long) frame;
        }

        public void skipVideo () {
            SceneManagerController.ChangeSceneMenu ();
        }
    }
}