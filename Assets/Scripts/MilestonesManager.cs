using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Animations;
using TMPro;

public class MilestonesManager : MonoBehaviour
{
    public GameObject achievementPanel;
    public TMP_Text title;
    public TMP_Text desc;
    public Image img;

    private float timerStart;
    private float timerDuration = 4f;
    public List<Milestone> milestones;
    List<Milestone> remainingMilestones = new();
    List<Milestone> completedMilestones = new();

    private bool isPlayingAnimation;

    [SerializeField] private int unlockedElementNumber;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.NewMergedElement += UpdateState;
        achievementPanel.SetActive(false);
        remainingMilestones = milestones;
    }

    void UpdateState()
    {
        unlockedElementNumber++;

        for (int i = 0; i<remainingMilestones.Count; i++)
        {
            ProcessMilestone(remainingMilestones[i]);
        }
    }

    private void UpdateSuccessUI(Milestone milestone)
    {
        title.text = milestone.GetName();
        desc.text = milestone.GetDescription();
        img.sprite = milestone.GetSprite();
    }

    private void ProcessMilestone(Milestone milestone)
    {
        if(milestone.condition.isMet(GameManager.instance.worldElements))
        {
            UpdateSuccessUI(milestone);
            DisplayMilestonePopUp(milestone);
            GetReward(milestone);
            UpdateList(milestone);
        }
    }

    private void UpdateList(Milestone milestone)
    {
        completedMilestones.Add(milestone);
        remainingMilestones.Remove(milestone);
    }

    private void DisplayMilestonePopUp(Milestone milestone)
    {
        achievementPanel.SetActive(true);
        isPlayingAnimation = true;
        timerStart = Time.time;
        return;

    }
    private void GetReward(Milestone milestone)
    {
        GameManager.instance.SpawnElement(milestone.reward);
    }

    private void Update()
    {
        if(isPlayingAnimation)
        { if (timerStart + timerDuration <= Time.time) { achievementPanel.SetActive(false); isPlayingAnimation = false; } }
    }
}
