using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ResizeCamera : MonoBehaviour
{
    public SpriteRenderer sp;
    //Assign this Camera in the Inspector
    private void Awake()
    {
        // Resize();
        Fitcam(sp);
    }
    void Resize()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        transform.localScale = new Vector3(1, 1, 1);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;


        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        Vector3 xWidth = transform.localScale;
        xWidth.x = worldScreenWidth / width;
        transform.localScale = xWidth;

        Vector3 yHeight = transform.localScale;
        yHeight.y = worldScreenHeight / height;
        transform.localScale = yHeight;

    }
    public void Fitcam(SpriteRenderer sp)
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = sp.bounds.size.x / sp.bounds.size.y;
        float differenceInSize = targetRatio / screenRatio;
        float orthosize = (sp.bounds.size.y / 2.1f * differenceInSize);
        orthosize += (orthosize * 1) / 100;
        if (screenRatio > 0.50f) orthosize = 5f;
        transform.GetComponent<Camera>().orthographicSize = orthosize;
    }
}