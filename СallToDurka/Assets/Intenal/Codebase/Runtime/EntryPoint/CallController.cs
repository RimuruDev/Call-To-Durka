﻿using System.Collections.Generic;
using System.Linq;
using RimuruDev.Plugins.Audio.Core;
using UnityEngine;

namespace RimuruDev.Intenal.Codebase.Runtime.EntryPoint
{
    public sealed class CallController : MonoBehaviour
    {
        private CallPanel callPanel;
        private GeneralGameSettings generalGameSettings;
        private List<CharacterData> characterDatas;
        private int currentCharacter = -1;

        [SerializeField] private List<SourceAudio> sourceAudios;

        public void Init(CallPanel callPanel, GeneralGameSettings generalGameSettings,
            List<CharacterData> characterDatas)
        {
            this.callPanel = callPanel;
            this.generalGameSettings = generalGameSettings;
            this.characterDatas = characterDatas;
        }

        public void StartCall(int callID)
        {
            OpenCallPanel(callID);
        }

        public void PickUpPhone()
        {
            generalGameSettings.SourceAudio.Loop = false;
            generalGameSettings.SourceAudio.Stop();

            var character = characterDatas.First(character => character.ID == currentCharacter);
            character.AudioClip.Play(character.AudioClipKey);
            character.AudioClip.Loop = true;

            callPanel.pickUpPhoneButton.gameObject.SetActive(false);
        }

        public void HangUpPhone()
        {
            StopAllAudioSources();

            CloseCallPanel();
        }

        private void OpenCallPanel(int callID)
        {
            currentCharacter = callID;

            SetActivePanelState(isActive: true);

            callPanel.callCharacter.sprite = characterDatas.First(character => character.ID == currentCharacter).Sprite;
            callPanel.characterName.text = characterDatas.First(character => character.ID == currentCharacter).Name;

            generalGameSettings.SourceAudio.Play(generalGameSettings.SourceAudioKey);
            generalGameSettings.SourceAudio.Loop = true;
        }

        private void CloseCallPanel()
        {
            var character = characterDatas.First(character => character.ID == currentCharacter);
            character.AudioClip.Stop();
            character.AudioClip.Loop = false;

            generalGameSettings.SourceAudio.Stop();
            generalGameSettings.SourceAudio.Loop = false;

            callPanel.pickUpPhoneButton.gameObject.SetActive(true);
            callPanel.pickUpPhoneButton.onClick.RemoveAllListeners();

            YandexGame.ScriptsYG.YandexGame.FullscreenShow();

            SetActivePanelState(isActive: false);
        }

        private void SetActivePanelState(bool isActive)
        {
            callPanel.callPanel.gameObject.SetActive(isActive);
        }

        private void StopAllAudioSources()
        {
            var audioSources = FindObjectsOfType<AudioSource>();
            foreach (var audioSource in audioSources)
            {
                audioSource.Stop();
            }
        }
    }
}