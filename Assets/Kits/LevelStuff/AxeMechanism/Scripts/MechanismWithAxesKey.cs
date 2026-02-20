using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct AxeKeyMechanismData
{
    public AxeKeyMechanism axeKeyMechanism;
    public int orderToActivate;
}

public class MechanismWithAxesKey : MonoBehaviour
{
    [SerializeField] private AxeKeyMechanismData[] axeKeyMechanismData;
    [SerializeField] private UnityEvent onAllActivatedEvent;

    private AxeKeyMechanism[] _sortedAxeKeyMechanisms;
    private Stack<AxeKeyMechanism> _activatedAxeKeyMechanismStack = new Stack<AxeKeyMechanism>();

    private void Awake()
    {
        _sortedAxeKeyMechanisms = new AxeKeyMechanism[axeKeyMechanismData.Length];
        Array.Sort(axeKeyMechanismData, (a, b) => a.orderToActivate.CompareTo(b.orderToActivate));
        for (int i = 0; i < axeKeyMechanismData.Length; i++)
        {
            _sortedAxeKeyMechanisms[i] = axeKeyMechanismData[i].axeKeyMechanism;
        }
    }
    
    public void OnAxeKeyMechanismActivated(AxeKeyMechanism axeKeyMechanism)
    {
        if (_activatedAxeKeyMechanismStack.Count < _sortedAxeKeyMechanisms.Length &&
            axeKeyMechanism == _sortedAxeKeyMechanisms[_activatedAxeKeyMechanismStack.Count])
        {
            _activatedAxeKeyMechanismStack.Push(axeKeyMechanism);
            if (_activatedAxeKeyMechanismStack.Count == _sortedAxeKeyMechanisms.Length)
            {
                onAllActivatedEvent.Invoke();
            }
        }
        else
        {
            ResetAll();
            axeKeyMechanism.DeactiveMechanism();
        }
    }

    private void ResetAll()
    {
        while (_activatedAxeKeyMechanismStack.Count > 0)
        {
            AxeKeyMechanism axeKeyMechanism = _activatedAxeKeyMechanismStack.Pop();
            axeKeyMechanism.DeactiveMechanism();
        }
    }
}
