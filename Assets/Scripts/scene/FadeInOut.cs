using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInOut : MonoBehaviour
{
    public Image _image; // ���̵� ȿ���� �� Image (UI Panel)
    public AnimationCurve _fadeCurve; // ���̵� ȿ���� ������ � (Ease In/Out)
    public float _fadeTime = 1f; // ���̵� ���� �ð�

    public void StartFadeIn()
    {
        StartCoroutine(Fade(1f, 0f)); // ������� ȿ��
    }

    public void StartFadeOut()
    {
        StartCoroutine(Fade(0f, 1f)); // ��ο����� ȿ��
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
        return _fadeTime; // ���̵� �ð� ��ȯ
    }
}
