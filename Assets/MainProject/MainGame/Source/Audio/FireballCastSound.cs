using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class FireballCastSound : MonoBehaviour
    {
        [Header("Audio Clips")]
        [SerializeField] private AudioClip startCastClip;
        [SerializeField] private AudioClip endCastClip;

        [Header("Player Info")]
        [SerializeField] SpellCaster _spellCaster;

        [Header("Settings")]
        [SerializeField] private AudioSource audioSource;

        private void Awake()
        {
            _spellCaster.OnSpellCast += CastSkillHandler;
        }

        public void CastSkillHandler(bool isCasting)
        {
            if (!isCasting)
            {
                audioSource.clip = startCastClip;
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
                audioSource.loop = false;
                audioSource.clip = endCastClip;
                audioSource.Play();
            }
        }
    }
}
