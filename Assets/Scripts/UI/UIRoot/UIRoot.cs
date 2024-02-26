using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BaseFramework
{
    public class UIRoot : BaseMonoBehaviour
    {
        public Transform LayerRoot;
        public Camera UICamera;
        public RectTransform AdaptResolution;

        private CanvasScaler canvasScaler;

        private void Awake()
        {
#if UNITY_ANDROID || UNITY_IOS
            canvasScaler = GetComponent<CanvasScaler>();

            if (Screen.height / Screen.width > canvasScaler.referenceResolution.y / canvasScaler.referenceResolution.x)
            {
                AdaptResolution.offsetMin = new Vector2(AdaptResolution.offsetMin.x, AdaptResolution.offsetMin.y + 44f);

                AdaptResolution.offsetMax = new Vector2(AdaptResolution.offsetMax.x, AdaptResolution.offsetMax.y - 44f);
            }
#endif
        }
    }

}