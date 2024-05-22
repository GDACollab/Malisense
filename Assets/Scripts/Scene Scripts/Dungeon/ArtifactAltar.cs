using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactAltar : MonoBehaviour, ISwitchable
{
    public Artifact artifact;
    public FloorNote floorNote;
    public bool isNear = false;
    public Sprite notSelected;
    public Sprite selected;

    DungeonManager dungeonManager;
    Player playerInventory;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = GameObject.FindWithTag("Player").GetComponent<Player>();
        dungeonManager = FindObjectOfType<DungeonManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (floorNote) { floorNote.enabled = false; }
    }

    private void Update()
    {
        if (isNear)
        {
            spriteRenderer.sprite = selected; // change sprite to selectable
        }
        else
        { // change sprite to normal
            spriteRenderer.sprite = notSelected;
        }
    }

    public void SwitchInit(bool activated) { return; }
    public void SwitchInteract(bool activated)
    {
        if (artifact) { playerInventory.newInventory.artifact2 = artifact; }
        if (floorNote) { dungeonManager.ActivateNote(floorNote); }
        else { dungeonManager.DeactivateNote(); }
        StartCoroutine(WaitBeforeExitingDungeon());
    }

    IEnumerator WaitBeforeExitingDungeon()
    {
        yield return new WaitForSeconds(0.5f);
        dungeonManager.EndDungeon(false, true);
    }
}
