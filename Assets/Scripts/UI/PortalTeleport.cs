using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport : MonoBehaviour {

    private Vector2Int coordinates;

    private Player player;
    private WorldGeneration worldGen;

    private MapPanel mapPanel;

    private void Awake() {
        player = FindObjectOfType<Player>();
        worldGen = FindObjectOfType<WorldGeneration>();
    }

    public void SetPortal(Vector2Int coordinates, MapPanel mapPanel) {
        this.coordinates = coordinates;
        this.mapPanel = mapPanel;
    }
    public void UsePortal() {
        //Debug.Log("Trying to use portal");
        if(player.Interaction is Portal) {
            //Debug.Log($"Using portal to {coordinates}");
            worldGen.ChangeCurrentCoordinates(coordinates);
            worldGen.ChangeLastCoordinates(coordinates);
            LevelGeneration levelGen = null;
            for(int i = 0; i < worldGen.levels.Count; i++) {
                if(worldGen.levels[i].layout.worldCoordinates == coordinates) {
                    levelGen = worldGen.levels[i];
                    //Debug.Log("Got the level");
                    break;
                }
            }
            if(levelGen != null && levelGen.poolGeneration.portals[0] != null) {
                //Debug.Log("Teleporting");
                player.transform.position = Vector3.down + levelGen.poolGeneration.portals[0].transform.position;
                mapPanel.gameObject.SetActive(false);
            }            
        }
        else {
            AudioSystem.audioManager.PlaySound("inGameButton", 0f);
        }
    }
}
