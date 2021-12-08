using System.Collections;
using Content.Scripts.Settings;
using UnityEngine;

namespace Content.Scripts.Controllers
{
    public class EnemyController: MonoBehaviour
    {
        [SerializeField] private EnemySettings m_Settings;
        [SerializeField] private Rigidbody Rigidbody;

        private EnemyState enemyState;
        private float m_currentSpeed;
        private Vector3 m_targetRun;
        private Coroutine m_prevCoroutine;

        private void Awake()
        {
            Rotate();
            m_currentSpeed = m_Settings.IdleSpeed;
        }

        public void FixedUpdate()
        {
            Processing();
        }
        
        public void Catch(PlayerCatcher player)
        {
            if (enemyState == EnemyState.Run)
            {
                return;
            }

            enemyState = EnemyState.Run;
            m_currentSpeed = m_Settings.RunSpeed;

            FindDirection(player);
        }

        public void Free(PlayerCatcher player)
        {
            enemyState = EnemyState.Free;
        }

        private void SetIdleState()
        {
            enemyState = EnemyState.Idle;
            m_currentSpeed = m_Settings.IdleSpeed;
        }

        private void Processing()
        {
            if (GetDistance(transform.forward) < 2f)
            {
                Rotate();

                if (enemyState == EnemyState.Free)
                    SetIdleState();
            }
            else
            {
                MoveForward();
            }
        }

        private void FindDirection(Component player)
        {
            var moveDir = transform.position - player.transform.position;
            var rotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = rotation;

            if (m_prevCoroutine != null)
                StopCoroutine(m_prevCoroutine);

            m_prevCoroutine = StartCoroutine(FindRunDirection());
        }

        private IEnumerator FindRunDirection()
        {
            while (GetDistance(transform.forward) < 2f)
            {
                Rotate();

                yield return null;
            }
        }

        private void Rotate()
        {
            var range = Random.Range(0, 360);
            transform.rotation *= Quaternion.Euler(0f, range, 0f);
        }

        private float GetDistance(Vector3 dir)
        {
            RaycastHit hit;
            var downRay = new Ray(transform.position, dir);

            if (Physics.Raycast(downRay, out hit, 10, 1 << 0))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                return hit.distance;
            }

            return 10;
        }

        private void MoveForward()
        {
            Rigidbody.AddForce(transform.forward * m_currentSpeed);
        }
    }

    public enum EnemyState
    {
        Idle,
        Free,
        Run
    }
}