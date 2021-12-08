using UnityEngine;
using UnityEngine.UI;

namespace Content.Scripts.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private FinishController m_finishController;
        [SerializeField] private Transform m_Player;

        [Header("Settings")]
        [SerializeField] private int m_MaxEnemies = 10;

        [SerializeField] private EnemyController[] m_Enemies;

        [Header("UI")]
        [SerializeField] private Text m_ProgressText;
        
        private int currentEnemies;
        private Vector3 startPlayer;
        private Vector3[] enemyPositions;

        private void Awake()
        {
            Init();
            StartGame();
        }

        private void Init()
        {
            enemyPositions = new Vector3[m_Enemies.Length];
       
            for (var index = 0; index < m_Enemies.Length; index++)
            {
                enemyPositions[index] = m_Enemies[index].transform.position;
            }

            startPlayer = m_Player.position;
            m_finishController.OnFinished += OnEnemyFinished;
        }

        public void StartGame()
        {
            m_Player.position = startPlayer;
            for (var index = 0; index < m_Enemies.Length; index++)
            {
                m_Enemies[index].transform.position = enemyPositions[index];
            }

            if (m_MaxEnemies > m_Enemies.Length)
                m_MaxEnemies = m_Enemies.Length;

            var liveEnemies = (m_Enemies.Length - m_MaxEnemies) - 1;

            for (var index = liveEnemies; index >=0; index--)
            {
                m_Enemies[index].gameObject.SetActive(false);
            }

            currentEnemies = m_MaxEnemies;
            UpdateProgressStatus();
        }

        private void UpdateProgressStatus()
        {
            m_ProgressText.text = $"{m_MaxEnemies - currentEnemies} / {m_MaxEnemies}";
        }

        private void OnEnemyFinished(EnemyController enemy)
        {
            enemy.gameObject.SetActive(false);
            currentEnemies--;
            UpdateProgressStatus();

            if (currentEnemies <= 0)
                m_ProgressText.text = "GAME ENDED";
        }

        private void OnDestroy()
        {
            m_finishController.OnFinished -= OnEnemyFinished;
        }
    }
}