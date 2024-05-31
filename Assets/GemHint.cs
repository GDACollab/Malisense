using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GemHint : MonoBehaviour
{
    private List<SwitchController> carnelianGems = new List<SwitchController>();
    private SpriteRenderer arrow;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var gem in FindObjectsOfType<CarnelianEarthquake>())
        {
            carnelianGems.Add(gem.GetComponent<SwitchController>());
        }
        arrow = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (arrow.enabled)
        {
            carnelianGems = carnelianGems.OrderByDescending(x => x.IsActivated() == false).ThenBy(x => Vector3.Distance(x.transform.position, transform.position)).ToList();
            if (carnelianGems[0].IsActivated())
            {
                arrow.enabled = false;
                return;
            }
            Vector3 dir = carnelianGems[0].transform.position - transform.position;

            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.forward);
        }
    }
}