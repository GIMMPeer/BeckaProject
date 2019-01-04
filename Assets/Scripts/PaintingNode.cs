using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingNode : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "PaintBrush") return;

        transform.parent.gameObject.GetComponent<PaintingTask>().UpdatePaintingStatus(gameObject);
    }
}
