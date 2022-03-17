using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LogoVideoControl : MonoBehaviour
{
    [SerializeField] private VideoClip _start = null, _middle = null, _end = null;
    private VideoPlayer _video = null;

    private bool _playing = true;
    
    private VideoPlayer _videoPlayer
    {
        get => _video = _video ?? GetComponent<VideoPlayer>();
    }

    private void Start()
    {
        _videoPlayer.clip = _start;
        _videoPlayer.Play();

        StartCoroutine(SwitchClip());
    }

    private IEnumerator SwitchClip()
    {
        yield return new WaitForSecondsRealtime((float)_videoPlayer.clip.length + 1);
        _videoPlayer.clip = _middle;
        _videoPlayer.Play();
        _videoPlayer.isLooping = true;
    }
}
