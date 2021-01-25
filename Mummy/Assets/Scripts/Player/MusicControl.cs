using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicControl : MonoBehaviour
{

    [Header("Sounds")]
    public AudioSource[] sounds = new AudioSource[2];
    public Button muteButton;
    public Sprite muteSprite;
    public Sprite muteEffectSprite;
    public Sprite unmuteSprite;
    public Sprite unmuteEffectSprite;
    private bool isMuted; 
    private SpriteState spriteState;
    private float musicVolume = 1f;
    
    // Start is called before the first frame update
    private void Start()
    {
        isMuted = false;
        spriteState = new SpriteState();
    }

    // Update is called once per frame
    void Update()
    {
        sounds[0].volume = musicVolume;
        sounds[1].volume = musicVolume;
    }

    public void UpdateVolume(float volume)
    {
        musicVolume = volume;
    }
    
    public void PressMute()
    {
        // mute the music. 
        sounds[0].mute = !sounds[0].mute;
        sounds[1].mute = !sounds[1].mute;
        isMuted = !isMuted;  // change state. 
        
        // change sprites of button. 
        if (isMuted)
        {
            spriteState.highlightedSprite = unmuteEffectSprite;
            spriteState.pressedSprite = unmuteSprite;
            spriteState.selectedSprite = unmuteEffectSprite;
            spriteState.disabledSprite = unmuteEffectSprite;
        }
        else
        {
            spriteState.highlightedSprite = muteEffectSprite;
            spriteState.pressedSprite = muteSprite;
            spriteState.selectedSprite = muteEffectSprite;
            spriteState.disabledSprite = muteEffectSprite;
        }
        muteButton.spriteState = spriteState;

    }
}
