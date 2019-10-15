using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Currently AND gate by default
public class LogicGate : MonoBehaviour
{
    public List<CrystalHolder> powerSources;

    [SerializeField] private UnityEvent onPowered;
    [SerializeField] private UnityEvent onPowerLoss;

    private bool isPowered = false;
    public MeshRenderer myMesh;

    public Material poweredMaterial;
    public Material unpoweredMaterial;

    private void Awake()
    {
        // powerSources = new List<CrystalHolder>();
        // myMesh = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        bool allPowered = true;

        for (int i = 0; i < powerSources.Count; i++)
        {
            if ((powerSources[i].heldType == CrystalType.None)) { allPowered = false; }
        }

        if (allPowered && !isPowered)
        {
            onPowered.Invoke();
            myMesh.material = poweredMaterial;

        }
        else if (!allPowered && isPowered)
        {
            onPowerLoss.Invoke();
            myMesh.material = unpoweredMaterial;
        }

        isPowered = allPowered;
    }

    private void OnDrawGizmos()
    {
        if (powerSources.Count == 0) { return; }
        for (int i = 0; i < powerSources.Count; i++)
        {
            if (!powerSources[i]) { continue; }

            Color drawColor = Color.green;

            if (powerSources[i].heldType == CrystalType.None)
            {
                drawColor = Color.red;
            }

            var sourcePos = powerSources[i].transform.position;

            Debug.DrawLine(this.transform.position, sourcePos, drawColor);
        }
    }
}
