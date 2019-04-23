using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private bool m_ok = true;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate
        {
            if(m_ok) {
                AudioManager.instance.Play("Ok");
            } else {
                AudioManager.instance.Play("Cancel");
            }
        });
    }

}
