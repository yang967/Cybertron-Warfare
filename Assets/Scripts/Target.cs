using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Target : MonoBehaviour
{
    Camera camera_;
    Vector3 target_;

    private void Awake()
    {
        camera_ = GetComponent<Camera>();
        target_ = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(1))
            click();
    }

    bool click()
    {
        Ray ray = camera_.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayhit;
        if (EventSystem.current.IsPointerOverGameObject())
            return false;

        if(Physics.Raycast(ray, out rayhit))
        {
            if (rayhit.collider.gameObject.layer == 6 && rayhit.collider.transform.parent.parent.GetComponent<Control>().getTeam() != GameManager.instance.Player.GetComponent<Control>().getTeam())
            {
                GameManager.instance.Player.GetComponent<InstructionQueue>().Clear_and_Execute(new Instruction(1, rayhit.collider.transform.parent.parent.gameObject));
            }
            else if (rayhit.collider.GetComponent<TurretController>() != null && rayhit.collider.transform.GetChild(3).GetComponent<Turret>().getTeam() != GameManager.instance.Player.GetComponent<Control>().getTeam())
                GameManager.instance.Player.GetComponent<InstructionQueue>().Clear_and_Execute(new Instruction(1, rayhit.collider.transform.gameObject));
            else
                GameManager.instance.Player.GetComponent<InstructionQueue>().Clear_and_Execute(new Instruction(0, rayhit.point));
            return true;
        }

        return false;
    }
}
