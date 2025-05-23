using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class EggController : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] GameObject _full;
        [SerializeField] GameObject _damaged;
        [SerializeField] private ParticleSystem _particleSystem;

        private QuestEvents _questEvents;

        private void Awake()
        {
            _questEvents = ServiceLocator.Instance.GetService<QuestEvents>();
            _questEvents.OnQuestStateChange += (Quest quest) =>
            {
                if (quest.QuestInfo.QuestId == "DestroyEggsQuest"
                    &&
                    quest.QuestState == QuestState.InProgress)
                {
                    QuestStartHandler();
                }
            };
        }

        public void Destroy()
        {
            _full.gameObject.SetActive(false);
            _damaged.gameObject.SetActive(true);
            _particleSystem.Play();
        }

        private void QuestStartHandler()
        {
            _collider.enabled = true;
        }
    }
}