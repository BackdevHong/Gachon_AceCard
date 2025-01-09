using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInOut : MonoBehaviour
{
    public Image _image; // 페이드 효과를 줄 Image (UI Panel)
    public AnimationCurve _fadeCurve; // 페이드 효과를 제어할 곡선 (Ease In/Out)
    public float _fadeTime = 1f; // 페이드 지속 시간

    public void StartFadeIn()
    {
        StartCoroutine(Fade(1f, 0f)); // 밝아지는 효과
    }

    public void StartFadeOut()
    {
        StartCoroutine(Fade(0f, 1f)); // 어두워지는 효과
    }

    private IEnumerator Fade(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1f)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / _fadeTime;

            Color color = _image.color;
            color.a = Mathf.Lerp(start, end, _fadeCurve.Evaluate(percent));
            _image.color = color;

            yield return null;
        }
    }

    public float GetFadeTime()
    {
        return _fadeTime; // 페이드 시간 반환
    }
}
