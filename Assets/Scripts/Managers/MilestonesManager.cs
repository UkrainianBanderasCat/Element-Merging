using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

using UnityEngine.Animations;
using TMPro;

public class MilestonesManager : MonoBehaviour
{
    public static MilestonesManager instance;

    public GameObject achievementPanel;
    public TMP_Text title;
    public TMP_Text desc;
    public Image img;

    private float timerStart;
    private float timerDuration = 4f;
    public List<Milestone> milestones;
    public List<Milestone> remainingMilestones = new();
    public List<Milestone> completedMilestones = new();

    private bool isPlayingAnimation;

    public List<GameObject> unlockablePanels = new();

    [SerializeField] private int unlockedElementNumber;

    List<Milestone> heldMilestones = new();

    public MilestonesManager() => instance = this;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.NewMergedElement += UpdateState;
        achievementPanel.SetActive(false);
        remainingMilestones = milestones;
    }

    public void InitMilestones()
    {
        foreach(Milestone milestone in milestones)
        {
            milestone.Init();
        }
    }

    void UpdateState()
    {
        unlockedElementNumber++;

        for (int i = 0; i<remainingMilestones.Count; i++)
        {
            ProcessMilestone(remainingMilestones[i]);
        }

        ReleaseMilestones();
    }

    private void UpdateSuccessUI(Milestone milestone)
    {
        title.text = milestone.GetName();
        desc.text = milestone.GetDescription();
        img.sprite = milestone.GetSprite();
    }

    private void ProcessMilestone(Milestone milestone)
    {
        if (milestone.GetCondition().isMet(GameManager.instance.worldElements))
        {
            foreach (Milestone m in completedMilestones)
            {
                if (m == milestone)
                {
                    return;
                }
            }
            
            UpdateSuccessUI(milestone);
            DisplayMilestonePopUp(milestone);
            GetReward(milestone);
            UpdateList(milestone);
            SaveManager.instance.Save();
        }
    }

    public void UpdateList(Milestone milestone)
    {
        milestone.IsCompleted = true;

        if(milestone.UnlockedPanel != -1)
            unlockablePanels[milestone.UnlockedPanel].SetActive(true);
            
        completedMilestones.Add(milestone);
        HoldMilestone(milestone);
    }

    void HoldMilestone(Milestone milestone)
    {
        heldMilestones.Add(milestone);
    }

    public void ReleaseMilestones()
    {
        foreach(Milestone milestone in heldMilestones)
        {
            remainingMilestones.Remove(milestone);
        }
        heldMilestones.Clear();
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
        GameObject element = GameManager.instance.CreateElement(milestone.GetReward(), new Vector2(0f, 0f), true);
        GameManager.instance.Release();
    }

    public Milestone GetMilestoneByName(string name)
    {
        foreach (Milestone milestone in milestones)
        {
            if(milestone.name == name)
            {
                return milestone;
            }
        }

        Debug.LogError($"Unable to find element with name \"{name}\". Returning null in place of the milestone. Consider carefully. ");
        return null;
    }

    private void Update()
    {
        if(isPlayingAnimation)
        { if (timerStart + timerDuration <= Time.time) { achievementPanel.SetActive(false); isPlayingAnimation = false; } }
    }
}
