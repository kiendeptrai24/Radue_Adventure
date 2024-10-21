using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling instance;
    public List<GameObject> stack = new List<GameObject>();
    public int amountOfPool;
    public GameObject prefabs;
    private void Awake() {
        if(instance == null)
            instance = this;
        else
            Destroy(instance.gameObject);
    }
    private void Start() {
        Generate();
    }
    private void Generate()
    {
        for (int i = 0; i < amountOfPool; i++)
        {
            GameObject gameObject = Instantiate(prefabs,PlayerManager.instance.player.transform.position,Quaternion.identity,instance.transform);
            stack.Add(gameObject);
            gameObject.SetActive(false);
        }
    }
    public GameObject GetObject(int _index)
    {
        if(stack[_index] != null)
        {
            return stack[_index];
        }
        return null;
    }
    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < stack.Count; i++)
        {
            stack[i].SetActive(_isActive);
        }
    }
}
