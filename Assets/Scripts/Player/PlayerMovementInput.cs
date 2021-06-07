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
        public bool _isdashing;
        private float _dashTime;
        public float _startDashTime =1f ;

        public Action<Vector2> Move = delegate { };
        public Action<bool> Dash = delegate{};
        

        public float speed = 1f;
        public float speedDash = 14f;

        public Rigidbody rb; // podria separar esto pero es muy poco

        private void Start() {
           _dashTime = _startDashTime;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>().normalized;
            Move(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _look = context.ReadValue<Vector2>();
        }
        public void OnDash(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                Dash(true);
                DashMovement();
            }
         
   
        }

       private void Update() {
           
           if(_isdashing == true)
           {
                if(_dashTime <= 0)
                {
                    
                    _dashTime = _startDashTime;
                    _isdashing = false;
                }else{

                    _dashTime -= Time.deltaTime;
                }
           }
       }
        private void FixedUpdate() {
           if(_isdashing == false)
           {
            
             rb.velocity = new Vector3(_move.x*speed,0f, _move.y*speed ); // podria validar que no se genere un vector3 cada vez 

           }else{

            if (_move.x == 0 && _move.y == 0)
            {
                
                rb.velocity = transform.forward.normalized * speedDash;
            }
            else
            {

                Vector3 direction = new Vector3(_move.x, 0, _move.y).normalized;
                rb.velocity = direction * speedDash;

            }
           }

            
        }
        private void DashMovement()
        {
            _isdashing = true;
            if (_move.x == 0 && _move.y == 0)
            {
                _dashTime += 0.05f;
            }
            
            
        }

    }



}