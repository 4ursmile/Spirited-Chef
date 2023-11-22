using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Behaviour
{
    public interface IClickable
    {
        ObjectType Type { get; }
        Transform transform { get; }
        Vector3 Position { get; }
        void OnClick();
    }
    public interface ISelectable
    {
        void Select();
        void Deselect();
        public String Name { get; }
        public string Description { get; }
    }
    public interface IInteractable
    {

        bool Interact(CharacterControllerS controllerS, GameController gameController);
        void StopInteract();
        public string Name { get; }
        public string Description { get; }
    }

    public enum ObjectType
    {
        None,
        Object,
        Ground,
        Character,
        Interactive, 
        NoneObject
    }
}
