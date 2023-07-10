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
    [SerializeField] GameObject HighLight_selected;

    private void Awake()
    {
        camera_ = GetComponent<Camera>();
        target_ = new Vector3(0, 0, 0);
    }

    /*private void Start()
    {
        Vector3 left = camera_.ScreenToWorldPoint(new Vector3(0, 0, camera_.nearClipPlane));
        Vector3 right = camera_.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camera_.nearClipPlane));

        Vector3 position = (right - left) / 2.0f + left;

        Vector3 center = (right - left) / 2.0f;

        left = GameManager.VectorRotate(left, center, 225);
        right = GameManager.VectorRotate(right, center, 225);

        Debug.Log(left);
        Debug.Log(right);

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = position - new Vector3(0, position.y, 0);
        cube.transform.localScale = new Vector3((right.x - left.x) / 2.0f, 100, (right.z - left.z) / 2.0f);

        cube.transform.eulerAngles = new Vector3(0, cube.transform.eulerAngles.y + 45, 0);
    }*/

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
            HighLight_selected.SetActive(true);
            HighLight_selected.transform.position = GameManager.instance.Player.transform.GetChild(2).GetComponent<Attack>().getTarget().transform.position;
            HighLight_selected.transform.position = new Vector3(HighLight_selected.transform.position.x, 1.72f, HighLight_selected.transform.position.z);
        }
        else
            HighLight_selected.SetActive(false);

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
