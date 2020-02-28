using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualizerScript : MonoBehaviour
{
    public float minHeight = 15.0f;
    public float maxHeitht = 425.0f;
    public float updateSenstivity = 0.5f;
    public Color visualizerColor = Color.gray;
    [Space(26)]
    public AudioClip audioClip;
    AudioSource m_audioSource;
    [Space(26), Range(64, 8192)]
    public int visualizerSimples = 64;

    public bool loop = true;

    public VisualizerObjectScript[] visualizerObjects;

    private void Start()
    {
        visualizerObjects = GetComponentsInChildren<VisualizerObjectScript>();

        if (!audioClip)
            return;

        m_audioSource = new GameObject("AudioSource").AddComponent<AudioSource>();
        m_audioSource.loop = loop;
        m_audioSource.clip = audioClip;
        m_audioSource.Play();
    }

    private void Update()
    {
        float[] spectrumData = m_audioSource.GetSpectrumData(visualizerSimples, 0, FFTWindow.Rectangular);

        for (int i = 0; i < visualizerObjects.Length; i++)
        {
            Vector2 newSize = visualizerObjects[i].GetComponent<RectTransform>().rect.size;
            newSize.y = Mathf.Clamp(Mathf.Lerp(newSize.y, minHeight + (spectrumData[i] * (maxHeitht - minHeight) * 5.0f), updateSenstivity), minHeight, maxHeitht);
            visualizerObjects[i].GetComponent<RectTransform>().sizeDelta = newSize;
            visualizerObjects[i].GetComponent<Image>().color = visualizerColor;
        }
    }
}
