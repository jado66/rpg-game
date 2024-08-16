using System;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
    public static string ToJson<T>(T obj)
    {
        return JsonUtility.ToJson(new Wrapper<T> { items = obj });
    }

    public static T FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.items;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T items;
    }
}
