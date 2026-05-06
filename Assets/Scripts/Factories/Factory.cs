using System.Collections.Generic;
using UnityEngine;

public interface IProduct
{
    //public abstract void OnSpawn();
}

public abstract class Factory<T> : MonoBehaviour where T : MonoBehaviourWithEvents, IProduct
{
    public List<T> all = new();

    public T            prefab;

    public T Create()
    {
        T product = Instantiate(prefab);
        product.onDestroy.AddListener(() => Unregister(product));
        //product.OnSpawn();
        OnCreate(product);
        all.Add(product);
        return product;
    }

    public void Unregister(T iter)
    {
        all.Remove(iter);
        if (Log.fac) Debug.Log("remove an iter: " + all.Count);
    }

    public abstract void OnCreate(T inst);
}
