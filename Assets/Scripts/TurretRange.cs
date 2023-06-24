using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRange : MonoBehaviour
{
    [SerializeField] GameObject Range;
    [SerializeField] Turret turret;

    // Start is called before the first frame update
    void Start()
    {
        Range.GetComponent<MeshRenderer>().material = Instantiate(Range.GetComponent<MeshRenderer>().material);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.Player.GetComponent<PlayerControl>().getHP() <= 0) {
            Range.SetActive(false);
            return;
        }
        if (Range.activeSelf) {
            if (turret.getTarget() == GameManager.instance.Player)
                Range.GetComponent<MeshRenderer>().material.color = Color.red;
            else
                Range.GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6 && other.transform.parent.parent.gameObject == GameManager.instance.Player && turret.getTeam() != GameManager.instance.Player.GetComponent<Control>().getTeam())
            Range.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6 && other.transform.parent.parent.gameObject == GameManager.instance.Player && turret.getTeam() != GameManager.instance.Player.GetComponent<Control>().getTeam())
            Range.SetActive(false);
    }
}
