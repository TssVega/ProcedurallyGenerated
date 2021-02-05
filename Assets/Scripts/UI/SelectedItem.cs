using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectedItem : MonoBehaviour {

    public Image firstImage;
    public Image secondImage;
    public Image thirdImage;

    public RectTransform rectTransform;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetImages(Sprite first, Sprite second, Sprite third, Color firstColor, Color secondColor, Color thirdColor) {
        firstImage.sprite = first ? first : null;
        secondImage.sprite = second ? second : null;
        thirdImage.sprite = third ? third : null;
        firstImage.color = first ? firstColor : Color.clear;
        secondImage.color = second ? secondColor : Color.clear;
        thirdImage.color = third ? thirdColor : Color.clear;
    }
    public void Release() {
        firstImage.sprite = null;
        secondImage.sprite = null;
        thirdImage.sprite = null;
        firstImage.color = Color.clear;
        secondImage.color = Color.clear;
        thirdImage.color = Color.clear;
    }
}
