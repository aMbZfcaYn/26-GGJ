using System;
using UnityEngine;

public class EnemySetMaterial : MonoBehaviour
{
    [SerializeField] private Material defaultMat;
    [SerializeField] private Material selectedMat;

    private float xRange = 1;
    private float yRange = 1;

    private void Update()
    {
        SetSelected(CheckSelected());
    }

    private bool CheckSelected()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return mousePos.x >= transform.position.x - xRange && mousePos.x <= transform.position.x + xRange &&
               mousePos.y >= transform.position.y - yRange && mousePos.y <= transform.position.y + yRange;
    }

    public void SetSelected(bool isSelected)
    {
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer)
        {
            renderer.material = isSelected ? selectedMat : defaultMat;
        }
    }
}