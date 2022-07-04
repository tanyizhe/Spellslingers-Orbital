using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightningBolt : MonoBehaviour
{
    [SerializeField] private GameObject LightningFieldPrefab;
    private GameObject lightningFieldObject;
    private LightningField lightningField;
    private Vector2 height;

    private void Awake()
    {
        this.lightningFieldObject = Instantiate(LightningFieldPrefab);
        this.lightningField = lightningFieldObject.GetComponent<LightningField>();
        this.height = new Vector2(0, GetComponent<SpriteRenderer>().bounds.size.y);
    }

    private void OnEnable()
    {
        int x = Random.Range(0, Screen.width);
        int y = Random.Range(0, Screen.height);
        Vector2 coords = new Vector2(x, y); 
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(coords);
        gameObject.transform.position = worldPosition + this.height / 2;
        SetLightningField(worldPosition);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void SetLightningField(Vector2 worldPosition)
    {
        this.lightningFieldObject.transform.position = worldPosition;
    }
    
    private void SummonLightningField()
    {
        this.lightningFieldObject.SetActive(true);
    }

    public void IncreaseRange()
    {
        this.lightningField.IncreaseRange();
    }
}
