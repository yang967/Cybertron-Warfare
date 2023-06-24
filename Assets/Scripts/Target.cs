using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Target : MonoBehaviour
{
    Camera camera_;
    Vector3 target_;
    [SerializeField] LayerMask include;
    [SerializeField] GameObject HighLight;

    private void Awake()
    {
        camera_ = GetComponent<Camera>();
        target_ = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        check();
        if (Input.GetMouseButtonUp(1))
            click();
    }

    void check()
    {
        Instruction i = GameManager.instance.Player.GetComponent<InstructionQueue>().getCurrentInstruction();
        if (i != null && (i.getInstructionType() == 2 || i.getInstructionType() == 1)) {
            HighLight.transform.position = GameManager.instance.Player.transform.GetChild(2).GetComponent<Attack>().getTarget().transform.position;
            HighLight.transform.position = new Vector3(HighLight.transform.position.x, 1.72f, HighLight.transform.position.z);
            return;
        }

        Ray ray = camera_.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayhit;
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Physics.Raycast(ray, out rayhit, Mathf.Infinity, include)) {
            if (rayhit.collider.gameObject.layer == 6 && rayhit.collider.transform.parent.parent.GetComponent<Control>().getTeam() != GameManager.instance.Player.GetComponent<Control>().getTeam()) {
                Transform target = rayhit.collider.transform;
                HighLight.gameObject.SetActive(true);
                HighLight.transform.position = new Vector3(target.position.x, 1.72f, target.position.z);
            }
            else
                HighLight.gameObject.SetActive(false);
            /*else if (rayhit.collider.GetComponent<TurretController>() != null && rayhit.collider.transform.GetChild(3).GetComponent<Turret>().getTeam() != GameManager.instance.Player.GetComponent<Control>().getTeam())
                GameManager.instance.Player.GetComponent<InstructionQueue>().Clear_and_Execute(new Instruction(1, rayhit.collider.transform.gameObject));
            else if (rayhit.collider.TryGetComponent<PlayerBase>(out PlayerBase b) && b.getTeam() != GameManager.instance.Player.GetComponent<Control>().getTeam())
                GameManager.instance.Player.GetComponent<InstructionQueue>().Clear_and_Execute(new Instruction(1, rayhit.collider.transform.gameObject));*/
        }
    }

    bool click()
    {
        Ray ray = camera_.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayhit;
        if (EventSystem.current.IsPointerOverGameObject())
            return false;

        if(Physics.Raycast(ray, out rayhit, Mathf.Infinity, include))
        {
            if (rayhit.collider.gameObject.layer == 6 && rayhit.collider.transform.parent.parent.GetComponent<Control>().getTeam() != GameManager.instance.Player.GetComponent<Control>().getTeam())
            {
                GameManager.instance.Player.GetComponent<InstructionQueue>().Clear_and_Execute(new Instruction(1, rayhit.collider.transform.parent.parent.gameObject));
            }
            else if (rayhit.collider.GetComponent<TurretController>() != null && rayhit.collider.transform.GetChild(3).GetComponent<Turret>().getTeam() != GameManager.instance.Player.GetComponent<Control>().getTeam())
                GameManager.instance.Player.GetComponent<InstructionQueue>().Clear_and_Execute(new Instruction(1, rayhit.collider.transform.gameObject));
            else if (rayhit.collider.TryGetComponent<PlayerBase>(out PlayerBase b) && b.getTeam() != GameManager.instance.Player.GetComponent<Control>().getTeam())
                GameManager.instance.Player.GetComponent<InstructionQueue>().Clear_and_Execute(new Instruction(1, rayhit.collider.transform.gameObject));
            else
                GameManager.instance.Player.GetComponent<InstructionQueue>().Clear_and_Execute(new Instruction(0, rayhit.point));
            return true;
        }

        return false;
    }
}
