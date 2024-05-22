using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Journal", menuName = "Inventory/Journal")]
public class Journal : ScriptableObject
{
    [System.Serializable]
    private class FloorNoteObj
    {
        public string ID;
        [field: TextArea(1, 5)]
        [field: SerializeField] public string Body { get; private set; }
        public bool Obtained;

        public FloorNoteObj(string id, string body)
        {
            ID = id;
            Body = body;
            Obtained = false;
        }
    }

    [SerializeField][Tooltip("Disable if adding custom notes")] private bool regenerateOnPlay = true;
    [SerializeField] private List<TextAsset> textNotes = new List<TextAsset>();
    [SerializeField] private List<FloorNoteObj> floorNotes = new List<FloorNoteObj>();

    public void CreateFloorNotes(bool force = false)
    {
        if (regenerateOnPlay || force)
        {
            floorNotes.Clear();
            foreach (TextAsset text in textNotes)
            {
                floorNotes.Add(new FloorNoteObj(text.name, text.ToString()));
            }
        }
        else
        {
            foreach (FloorNoteObj note in floorNotes)
            {
                note.Obtained = false;
            }
        }
    }

    public string ReadFloorNote(string id)
    {
        int index = floorNotes.FindIndex(x => x.ID == id);
        if (index >= 0)
        {
            return floorNotes[index].Body;
        }
        return "Failed";
    }

    public bool ObtainFloorNote(string id)
    {
        int index = floorNotes.FindIndex(x => x.ID == id);
        if (index >= 0)
        {
            floorNotes[index].Obtained = true;
            return true;
        }
        return false;
    }

    public bool CheckFloorNote(string id)
    {
        int index = floorNotes.FindIndex(x => x.ID == id);
        if (index >= 0 && floorNotes[index].Obtained)
        {
            return true;
        }
        return false;
    }

    public List<string> GetObtainedFloorNotes()
    {
        List<string> notes = new List<string>();
        foreach (FloorNoteObj note in floorNotes.FindAll(x => x.Obtained == true))
        {
            notes.Add(note.Body);
        }
        return notes;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Journal))]
public class JournalEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create Floor Notes"))
        {
            Journal journal = (Journal)target;
            journal.CreateFloorNotes(true);
        }
    }
}
#endif