using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource source = null;

    [SerializeField] private AudioClip menuSFX = null;
    
    [SerializeField] private List<AudioClip> raceSFX = null;
    
    [SerializeField] private AudioClip completeSFX = null;

    private Tween _soundAnimation = null;

    public void PlayMenuSFX()
    {
        _soundAnimation?.Kill();

        _soundAnimation = DOTween.To(() => source.volume, 
            x => source.volume = x, 0F, .25F).OnComplete(() =>
        {
            source.clip = menuSFX;
        
            source.Play();

            _soundAnimation = DOTween.To(() => source.volume,
                x => source.volume = x, VolumeController.Instance.GetVolume(), .25F);
        });
    }

    public void PlayRandomRaceSFX()
    {
        _soundAnimation = DOTween.To(() => source.volume, 
            x => source.volume = x, 0F, .25F).OnComplete(() =>
        {
            source.clip = raceSFX[Random.Range(0, 2)];
        
            source.Play();

            _soundAnimation = DOTween.To(() => source.volume,
                x => source.volume = x, VolumeController.Instance.GetVolume(), .25F);
        });
    }

    public void PlayCompleteSFX()
    {
        _soundAnimation = DOTween.To(() => source.volume, 
            x => source.volume = x, 0F, .1F).OnComplete(() =>
        {
            source.clip = completeSFX;
        
            source.Play();

            _soundAnimation = DOTween.To(() => source.volume,
                x => source.volume = x, VolumeController.Instance.GetVolume(), .1F);
        });
    }
    
    private void UpdateVolume(float volume)
    {
        source.volume = volume;
    }
    
    private void OnEnable()
    {
        GameManager.OnVolumeChange += UpdateVolume;
    }

    private void OnDisable()
    {
        GameManager.OnVolumeChange -= UpdateVolume;
    }
}