using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExpireItem
{
    public static ExpireItem empty = new ExpireItem(null);

    [SerializeField] private Item item;
    [SerializeField] private float secondsLeftToExpire;

    public bool IsEmpty() => item == null;

    public ExpireItem(Item item)
    {
        this.item = item;
        secondsLeftToExpire = item != null ? item.GetSecondsToExpire() : 0;
    }

    public void UpdateSecondsLeftToExpire()
    {
        if (item == null || secondsLeftToExpire == 0) return;
        secondsLeftToExpire -= Time.deltaTime;
        if (secondsLeftToExpire <= 0) item = null;
    }

    public float GetFillAmount()
    {
        if (item == null || item.GetSecondsToExpire() == 0) return 0;
        return 1 - secondsLeftToExpire / item.GetSecondsToExpire();
    }

    public override string ToString()
    {
        if (item == null) return "Empty";
        return item.GetName();
    }

    public static bool operator ==(ExpireItem first, ExpireItem second)
    {
        return first.item == second.item;
    }

    public static bool operator !=(ExpireItem first, ExpireItem second)
    {
        return first.item == second.item;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is ExpireItem)) return false;
        return this == (ExpireItem)obj;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
