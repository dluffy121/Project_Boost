﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class AudioController
{
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime, float minVol)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume >= minVol)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.volume = minVol;
    }
    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        audioSource.Play();
        audioSource.volume = 0f;
        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }
}