using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// How fitting, a pool of raindrops.
public class GameObjectPool<T> where T : MonoBehaviour
{
    List<T> items;
    List<T> inactiveItems;
    GameObject template;
    public Transform InstantiateParent { get; set; }

    public GameObjectPool(int capacity, GameObject template)
    {
        items = new List<T>(capacity);
        inactiveItems = new List<T>(capacity);
        this.template = template;
    }

    public IEnumerable<T> Items
    {
        get
        {
            return items;
        }
    }

    public T Make(Vector3 pos)
    {
        T item;
        if (inactiveItems.Count != 0)
        {
            item = inactiveItems[0];
            inactiveItems.Remove(item);
            items.Add(item);
            item.gameObject.SetActive(true);
            item.transform.position = pos;
        }
        else
        {
            GameObject newGO;
            if (InstantiateParent != null)
            {
                newGO = GameObject.Instantiate(template, pos, Quaternion.identity, InstantiateParent);
            }
            else
            {
                newGO = GameObject.Instantiate(template, pos, Quaternion.identity);
            }
            item = newGO.GetComponent<T>();
            items.Add(item);
        }
        return item;
    }

    public void MakeInactive(T obj)
    {
        obj.gameObject.SetActive(false);
        items.Remove(obj);
        inactiveItems.Add(obj);
    }
}
