using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class FireballCooldown : MonoBehaviour
    {
        [SerializeField] private SpellCaster _spellCaster;
        [SerializeField] private Canvas _spellCooldownCanvas;
        [SerializeField] private Image _cooldownImage;
        [SerializeField] private GameObject _fireBallAbility;
        [SerializeField] private float _cooldownDuration = 5f;

        private float _cooldownTimer;
        private bool _isCoolingDown;

        private void Start()
        {
            _spellCaster.OnSpellCast += SpellCastHandler;
            _cooldownImage.fillAmount = 0f;
            _spellCooldownCanvas.enabled = false;
            _fireBallAbility.SetActive(false);
        }

        private void Update()
        {
            if (_spellCaster.SpellData == null)
            {
                //_cooldownImage.fillAmount = 1f;
                //_spellCooldownCanvas.enabled = true;
                _fireBallAbility.SetActive(false);
                return;
            }
            else
            {
                _fireBallAbility.SetActive(true);
            }

            if (_isCoolingDown)
            {
                _cooldownTimer -= Time.deltaTime;
                _cooldownImage.fillAmount = _cooldownTimer / _cooldownDuration;

                if (_cooldownTimer <= 0f)
                {
                    _isCoolingDown = false;
                    _spellCooldownCanvas.enabled = false;
                }
            }
        }

        private void SpellCastHandler(bool isCasted)
        {
            if (!isCasted)
            {
                _cooldownTimer = _cooldownDuration;
                _cooldownImage.fillAmount = 1f;
                _isCoolingDown = true;
                _spellCooldownCanvas.enabled = true;
            }
        }
    }
}