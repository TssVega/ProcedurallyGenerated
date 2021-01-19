﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillTreeManager : MonoBehaviour, IDragHandler {

    public List<GameObject> talentButtons;
    public Sprite skillFrame;
    public Sprite fillFrame;
    //public TextMeshProUGUI statPoints;
    //public TalentConfirmationUI confirmationPanel;
    public GameObject icon;
    public Sprite[] icons;
    private GameObject talentPanel;
    public GameObject debugText;

    private RectTransform talentPanelTransform;
    //private TalentDatabase talentDatabase;
    //private GameMaster gameMaster;
    // private int currentPanel = 0;
    private Stats playerStats;
    private float distance = 150f;  // Distance between slots in pixels
    private float distanceBetweenTiers = 140f;
    private bool slotsGenerated = false;
    private readonly int skillPathCount = 12;
    private Canvas uiCanvas;
    private float panelLimit = 1200f;

    private readonly float[] startDegrees = { 0f, 7.5f, 0f, 3.725f, 3.725f, 3.725f, 0f, 7.5f, 0f };
    private readonly float[] degreeIncrements = { 30f, 15f, 10f, 7.5f, 7.5f, 7.5f, 10f, 15f, 30f };
    private readonly int[] runtimes = { 12, 24, 36, 48, 48, 48, 36, 24, 12 };

    public GameObject skillSlot;

    private void Start() {
        //confirmationPanel.gameObject.SetActive(false);
    }
    private void OnEnable() {
        if(FindObjectOfType<Player>()) {
            playerStats = FindObjectOfType<Player>().GetComponent<Stats>();
        }
        if(playerStats) {
            //statPoints.text = playerStats.statPoints.ToString();
        }
        //gameMaster = FindObjectOfType<GameMaster>();
        //talentDatabase = gameMaster.talentDatabase;
        talentPanel = gameObject;
        GenerateTalentSlots();
        if(playerStats) {
            //statPoints.text = playerStats.statPoints.ToString();
        }
        talentPanelTransform = talentPanel.GetComponent<RectTransform>();
        uiCanvas = FindObjectOfType<UICanvas>().GetComponent<Canvas>();
    }
    private void GenerateTalentSlots() {
        if(slotsGenerated) {
            return;
        }
        PutIcons();
        int ringCount = 9;
        float currentAngle;
        int debugCounter = 0;

        for(int i = 0; i < ringCount; i++) {
            currentAngle = startDegrees[i];
            for(int j = 0; j < runtimes[i]; j++) {
                Vector2 slotPosition = new Vector2(
                    Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                    Mathf.Sin(currentAngle * Mathf.Deg2Rad)) * distance;
                GameObject clone = Instantiate(skillSlot);
                clone.transform.SetParent(talentPanel.transform);
                clone.transform.localPosition = slotPosition;
                clone.transform.localRotation = Quaternion.identity;
                currentAngle += degreeIncrements[i];
                GameObject debugTextObj = Instantiate(debugText);
                debugTextObj.GetComponent<TextMeshProUGUI>().text = debugCounter.ToString();
                debugTextObj.transform.SetParent(talentPanel.transform);
                debugTextObj.transform.localPosition = slotPosition;
                debugTextObj.transform.localRotation = Quaternion.identity;
                debugCounter++;
                talentButtons.Add(clone);
            }
            distance += distanceBetweenTiers;
        }
        // TODO: Activate when all skills are in the database
        //AssignTalentSlots();
        AssignTalents();
        slotsGenerated = true;
    }
    private void PutIcons() {
        float iconDistance = 100f;
        float currentAngle = 0f;
        for(int i = 0; i < skillPathCount; i++) {
            Vector2 slotPosition = new Vector2(
                    Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                    Mathf.Sin(currentAngle * Mathf.Deg2Rad)) * iconDistance;
            GameObject clone = Instantiate(icon);
            clone.transform.SetParent(talentPanel.transform);
            clone.transform.localPosition = slotPosition;
            clone.transform.localRotation = Quaternion.identity;
            clone.GetComponent<Image>().sprite = icons[i];
            currentAngle -= 30f;
        }
    }
    /*
    private void AssignTalentSlots() {
        for(int i = 0; i < talentButtons.Count; i++) {
            if(talentButtons[i] && talentDatabase.talents.Length > i) {
                talentButtons[i].GetComponent<TalentUI>().SetSkill(talentDatabase.talents[i], i);
            }
            else {
                Debug.LogWarning("Talent slot out of index");
            }
        }
    }*/
    private void AssignTalents() {

    }
    /*
    public void OpenConfirmationPanel(int index) {
        confirmationPanel.gameObject.SetActive(true);
        confirmationPanel.SetTalent(talentDatabase.talents[index]);
    }*/
    /*
    public bool CanConsumeSkillPoint() {
        if(playerStats && playerStats.statPoints > 0) {
            playerStats.statPoints--;
            statPoints.text = playerStats.statPoints.ToString();
            return true;
        }
        return false;
    }*/
    public void OnDrag(PointerEventData eventData) {
        talentPanelTransform.anchoredPosition += eventData.delta / uiCanvas.scaleFactor;
        if(talentPanelTransform.anchoredPosition.x > panelLimit) {
            talentPanelTransform.anchoredPosition = new Vector2(panelLimit, talentPanelTransform.anchoredPosition.y);
        }
        if(talentPanelTransform.anchoredPosition.x < -panelLimit) {
            talentPanelTransform.anchoredPosition = new Vector2(-panelLimit, talentPanelTransform.anchoredPosition.y);
        }
        if(talentPanelTransform.anchoredPosition.y > panelLimit) {
            talentPanelTransform.anchoredPosition = new Vector2(talentPanelTransform.anchoredPosition.x, panelLimit);
        }
        if(talentPanelTransform.anchoredPosition.y < -panelLimit) {
            talentPanelTransform.anchoredPosition = new Vector2(talentPanelTransform.anchoredPosition.x, -panelLimit);
        }
    }
}