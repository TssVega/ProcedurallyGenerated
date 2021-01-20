using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public List<GameObject> panelsToClose;
    public List<GameObject> panelsToOpen;
    public List<GameObject> panelsToToggle;

    public void Do() {
        for(int i = 0; i < panelsToClose.Count; i++) {
            panelsToClose[i].SetActive(false);
        }
        for(int i = 0; i < panelsToOpen.Count; i++) {
            panelsToOpen[i].SetActive(true);
        }
        for(int i = 0; i < panelsToToggle.Count; i++) {
            panelsToToggle[i].SetActive(!panelsToToggle[i].activeSelf);
        }
    }
}
