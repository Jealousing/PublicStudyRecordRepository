using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 작성자: 서동주
public class DropFlask : MonoBehaviour
{
    public bool IsDrop;
    public int NoiseLv;
    private void Start()
    {

        IsDrop = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsDrop)
            return;
        if (collision.collider.CompareTag("Ground"))
        {
            this.GetComponent<NoiseManager>().CreateNoise(NoiseLv, this.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Slave"))
        {
            StartCoroutine(ActiveSetting());
        }
    }

    IEnumerator ActiveSetting()
    {
        yield return new WaitForSeconds(1.0f);
        this.gameObject.SetActive(false);
    }
}
