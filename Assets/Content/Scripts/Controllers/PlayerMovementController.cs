using Content.Scripts.Settings;
using UnityEngine;

namespace Content.Scripts.Controllers
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private PlayerSettings m_Settings;
        [SerializeField] private Rigidbody Rigidbody;

        private void FixedUpdate()
        {
            Processing();
        }

        private void Processing()
        {
            Move();
            Rotate();
        }

        private void Move()
        {
            var moveVertical = Input.GetAxis("Vertical");
            var movement = transform.forward * moveVertical;
            Rigidbody.AddForce(movement * m_Settings.Speed);
        }

        private void Rotate()
        {
            var moveHorizontal = Input.GetAxis("Horizontal");
            transform.rotation *= Quaternion.Euler(0f, moveHorizontal * m_Settings.RotateSpeed, 0f);
        }
    }
}