using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManger : MonoBehaviour
{
    [SerializeField] private float sfxMinimumDistance;
    public static AudioManger instance{get; private set;}
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;


    public bool playBgm;
    
    private int bgmIndex;
    
    private bool canPlaySFX;

    private void Awake() {
        if(instance == null)
            instance = this;
        else
            Destroy(instance.gameObject);
            Invoke(nameof(AllowSFX),1);
    }
    private void Update() {
        if(!playBgm)
            StopAllBGM();
        else
        {

            //RandomBGM();
            if(!bgm[bgmIndex].isPlaying)
                PlayBGM(bgmIndex);
        }
    }
    public void PlayerSFX(int _sfxIndex,Transform _source)
    {
        // if(sfx[_sfxIndex].isPlaying)
        //     return;
        if(!canPlaySFX)
            return;
        if(_source && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinimumDistance)
            return;
        if(_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(.85f,1.1f); 
            sfx[_sfxIndex].Play();
        }
    }
    public void StopSFX(int _index) => sfx[_index].Stop();
    public void StopSFXWithTime(int _index) => StartCoroutine(DecreaseVolume(sfx[_index]));    
    private IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;
        while(_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f;
            yield return new WaitForSeconds(.6f);
            if(_audio.volume <= 0.1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;
                break;
            }
        }
    }
    public void RandomBGM()
    {
        bgmIndex = Random.Range(0,bgm.Length);
        PlayBGM(bgmIndex);
    }
    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex =_bgmIndex;
        StopAllBGM();
        bgm[bgmIndex].Play();
    }
    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
    public void AllowSFX() => canPlaySFX= true;
    public bool BGMisplaying()
    {
        foreach (var audio in bgm)
        {
            if(audio.isPlaying)
                return true;
        }
        return false;
    }
}
