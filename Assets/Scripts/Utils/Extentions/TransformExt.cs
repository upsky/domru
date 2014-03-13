using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public static class TransformExt
{
    public static void SetX(this Transform transform, float x)
    {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    public static void SetY(this Transform transform, float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    public static void SetZ(this Transform transform, float z)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }

    public static void SetLocalX(this Transform transform, float x)
    {
        transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
    }

    public static void SetLocalY(this Transform transform, float y)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
    }

    public static void SetLocalZ(this Transform transform, float z)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
    }

    public static void SetRotationEulerY(this Transform transform, float y)
    {
        transform.rotation =
            Quaternion.Euler(transform.rotation.eulerAngles.x, y, transform.rotation.eulerAngles.z);
    }



    /// <summary>
    /// ����� localPosition � localRotation � ���� � localScale � 1
    /// </summary>
    public static void LocalReset(this Transform tr)
    {
        tr.localPosition = Vector3.zero;
        tr.localRotation = Quaternion.identity;
        tr.localScale = Vector3.one;
    }

    /// <summary>
    /// ����������� � ������� transform �������� Position, Rotation, Scale, Parent �� ������� transform-�
    /// </summary>
    /// <param name="doCloneScale">��� true ��������� ����������� Scale</param>
    /// <param name="doCloneParent">��� true �������� Parent ��������������� ����� �� ��� � ����������� transform-�</param>
    public static void CloneTransform(this Transform tr, Transform from, bool doCloneScale=false, bool doCloneParent=false)
    {
        tr.position = from.position;
        tr.rotation = from.rotation;
        if (doCloneScale)
            tr.localScale = from.localScale;
        if (doCloneParent)
            tr.parent = from.parent;
    }

    /// <summary>
    /// ����� ���� �������� � ����� �� ���������� ���� �� ���� �������� �������� �������� �������
    /// </summary>
    /// <param name="excludeCurrentTransform">��� true ��������� ����� ������ � �������� ��������, �� ������� � ���������� ������ ����</param>
    public static Transform[] FindChildsRecursive(this Transform obj, bool excludeCurrentTransform = false, params string[] names)
    {
        if (excludeCurrentTransform)
            return obj.GetComponentsInChildren<Transform>().Where(tr => names.Contains(tr.name) && obj != tr).ToArray();
        else
            return obj.GetComponentsInChildren<Transform>().Where(tr => names.Contains(tr.name)).ToArray();
    }

}