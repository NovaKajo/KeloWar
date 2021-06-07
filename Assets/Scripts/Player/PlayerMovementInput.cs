using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kelo.Player
{

    [RequireComponent(typeof(PlayerInput))]
public class PlayerMovementInput : MonoBehaviour
{
        public Vector2 _move;
        public Vector2 _look;

        public Action<Vector2> Move = delegate { };
        

        public float speed = 1f;

        public Rigidbody rb; // podria separar esto pero es muy poco

        public void OnMove(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>().normalized;
            Move(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _look = context.ReadValue<Vector2>();
        }

       
        private void FixedUpdate() {
            rb.velocity = new Vector3(_move.x*speed,0f, _move.y*speed );
            
        }

    }



}