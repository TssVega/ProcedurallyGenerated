using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {

    Sprite UISprite { get; set; }
    void Interact();
}
