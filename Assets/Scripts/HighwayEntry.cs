using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighwayEntry : MonoBehaviour
{
    bool cool_down_;
    [SerializeField] GameObject car_pos_;
    [SerializeField] Highway highway_;

    private void Awake()
    {
        cool_down_ = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (cool_down_)
            return;

        if (other.gameObject.layer != 6 || other.GetComponent<CarControl>() == null)
            return;

        cool_down_ = true;
        other.GetComponent<CarControl>().HighWay(car_pos_);
        highway_.initialize(other.transform.parent.parent.gameObject);
    }

    public void reseted()
    {
        cool_down_ = false;
    }
}
