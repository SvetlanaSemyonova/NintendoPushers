using System;
using UnityEngine;

namespace Content.Scripts.Controllers
{
    public class FinishController : MonoBehaviour
    {
        [SerializeField] private Animator m_Animator;

        public Action<EnemyController> OnFinished = controller => {};

        private void OnTriggerEnter(Collider other)
        {
            var enemyController = other.GetComponent<EnemyController>();
            if (!enemyController) return;
            
            if (OnFinished != null)
            {
                OnFinished.Invoke(enemyController);
                m_Animator.Play("Action");
            }
        }
    }
}
