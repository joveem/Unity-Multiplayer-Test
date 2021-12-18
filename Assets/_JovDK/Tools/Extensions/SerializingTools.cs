using System;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

using Newtonsoft.Json;

using NetworkModels;

public static class SerializingTools
{
    public static string SerializeObjectToJSON(this object _object)
    {

        string _value = "";

        try
        {

            _value = JsonConvert.SerializeObject(_object);

        }
        catch (System.Exception _error)
        {

            UnityEngine.Debug.LogError("001 | error: " + _error);
            _value = "ERROR";
            throw;

        }

        return _value;

    }

    public static T DeserializeJsonToObject<T>(this string _text)
    {

        return JsonConvert.DeserializeObject<T>(_text);

    }

    public static byte[] SerializeObjectToByte(this object _object)
    {

        //return (byte[])Convert.ChangeType(_object, typeof(byte[]));

        ///*
        if (_object == null)
            return null;

        BinaryFormatter _binaryFormatter = new BinaryFormatter();
        MemoryStream _memoryStream = new MemoryStream();
        _binaryFormatter.Serialize(_memoryStream, _object);

        return _memoryStream.ToArray();
        //*/

    }

    public static T DeserializeByteToObject<T>(this byte[] _data)
    {

        /*
        using (MemoryStream _memoryStream = new MemoryStream(_data))
        {

            IFormatter _iFormatter = new BinaryFormatter();
            return (T)_iFormatter.Deserialize(_memoryStream);

        }
        */


        /*
        T _Value = (T)new object;
        byte[] NewValue = new byte[];

        object Temp = _Value; // temp to avoid casting issues

        if (_Value is Int32) Temp =
        BitConverter.ToInt32(NewValue, 0);
        else if (_Value is Boolean) Temp =
        BitConverter.ToBoolean(NewValue, 0);
        else if (_Value is Byte) Temp = NewValue[0];
        else if (_Value is Double) Temp =
        BitConverter.ToDouble(NewValue, 0);
        else if (_Value is String) Temp =
        BitConverter.ToString(NewValue);
        else if (_Value is UInt32) Temp =
        BitConverter.ToUInt32(NewValue, 0);
        else if (_Value is UInt16) Temp =
        BitConverter.ToUInt16(NewValue, 0);
        else throw new InvalidTypeException("Unsupported type" + _Value.GetType().ToString());

        _Value = (T)Temp;
        */



        //return (T)Convert.ChangeType(_data, typeof(T));

        
        MemoryStream _memoryStream = new MemoryStream();
        BinaryFormatter _binaryFormatter = new BinaryFormatter();
        _memoryStream.Write(_data, 0, _data.Length);
        _memoryStream.Seek(0, SeekOrigin.Begin);
        object _object = _binaryFormatter.Deserialize(_memoryStream);

        return (T)_object;


    }

}

[System.Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public Vector3 Vector3
    {
        get
        {
            return new Vector3(x, y, z);
        }
        set
        {

            x = value.x;
            y = value.y;
            z = value.z;

        }
    }

    public SerializableVector3()
    {

        x = 0f;
        y = 0f;
        z = 0f;

    }

    public SerializableVector3(float _x, float _y, float _z)
    {

        x = _x;
        y = _y;
        z = _z;

    }

    public SerializableVector3(Vector3 _vector3)
    {

        x = _vector3.x;
        y = _vector3.y;
        z = _vector3.z;

    }
}
