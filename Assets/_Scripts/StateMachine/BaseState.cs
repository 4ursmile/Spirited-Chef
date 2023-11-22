using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public interface IBaseState
    {
        public void OnStateEnter();
        public void FrameUpdate();
        public void PhysicUpdate();
        public void OnStateExit();
        public void OnTriggerEnter(Collider other);
        public void OnCollisionEnter(Collision other);
        public void Assign(Action Callbacks);
    }
}