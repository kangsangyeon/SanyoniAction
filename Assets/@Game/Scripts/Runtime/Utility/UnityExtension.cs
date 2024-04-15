using System;
using UnityEngine;
using Object = UnityEngine.Object;

public static class UnityExtension
{
    public static GameObject Add(this GameObject parent, string name)
    {
        var _go = new GameObject(name);
        var _tf = _go.transform;
        _tf.parent = parent.transform;
        _go.layer = parent.layer;
        _tf.localPosition = Vector3.zero;
        _tf.localRotation = Quaternion.identity;
        _tf.localScale = Vector3.one;
        return _go;
    }

    public static T GetOrAddComponent<T>(this Object uo) where T : Component
    {
        return uo.GetComponent<T>() ?? uo.AddComponent<T>();
    }

    public static T GetComponent<T>(this Object uo)
    {
        if (uo is GameObject)
        {
            return ((GameObject)uo).GetComponent<T>();
        }
        else if (uo is Component)
        {
            return ((Component)uo).GetComponent<T>();
        }
        else
        {
            throw new NotSupportedException();
        }
    }

    public static T AddComponent<T>(this Object uo) where T : Component
    {
        if (uo is GameObject)
        {
            return ((GameObject)uo).AddComponent<T>();
        }
        else if (uo is Component)
        {
            return ((Component)uo).gameObject.AddComponent<T>();
        }
        else
        {
            throw new NotSupportedException();
        }
    }
}