using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsernameBillboarding : MonoBehaviour
{
    public Transform target;
    public TextMeshPro usernameText;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetName(string name)
    {
        usernameText = GetComponent<TextMeshPro>();
        usernameText.SetText(name);
    }

    void LateUpdate()
    {
        if(target != null) transform.LookAt(target);
    }
}
