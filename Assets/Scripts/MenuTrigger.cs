using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTrigger : MonoBehaviour
{
    [SerializeField] List<Animator> Enter;
    [SerializeField] List<Animator> Exit;

    public void OnClick()
    {
        foreach (var anim in Enter)
            anim.SetTrigger("Enter");

        foreach (var anim in Exit)
            anim.SetTrigger("Exit");
    }
}
