using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreButton : MonoBehaviour
{
    [SerializeField] int type;

    bool store = false;

    public bool isActive {
        get { return store; }
    }

    [SerializeField] Animator animator;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && store)
            GameManager.instance.ResetSFB();
    }

    public void OnClick()
    {
        if(store)
        {
            GameManager.instance.ResetSFB();
            store = false;
            return;
        }
        if (type == 0)
            GameManager.instance.getStore();
        else if (type == 1)
            GameManager.instance.getFuse();
        else
            GameManager.instance.getBag();
    }


    public void Click()
    {
        if (!store)
        {
            store = true;
            animator.SetTrigger("Enter");
        }
        else
        {
            store = false;
            animator.SetTrigger("Exit");
        }
    }
}
