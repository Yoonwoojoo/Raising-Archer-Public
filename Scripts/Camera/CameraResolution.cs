using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private void Awake()
    {
        Camera cam = GetComponent<Camera>();
        Rect rect = cam.rect;

        float targetAspect = 9f / 16f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowAspect / targetAspect;
        float scalewidth = 1f / scaleheight;

        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }

        cam.rect = rect;
    }
}
