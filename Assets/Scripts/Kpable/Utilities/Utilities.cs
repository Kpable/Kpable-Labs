using UnityEngine;
using System.Collections;
using System;

namespace Kpable.Utilities
{
    public enum Direction { Up, Left, Down, Right }
    public enum Axis { X, Y, Z, XY, XZ, YZ, XYZ }

    [Serializable]
    public class IntRange
    {
        public int m_Min;       // The minimum value in this range.
        public int m_Max;       // The maximum value in this range.


        public IntRange(int min, int max)
        {
            m_Min = min;
            m_Max = max;
        }
    }

    public static class EnumTwoExtensions
    {
        //Extension method to get the Vector3 based on the direction. 
        public static Vector3 Vec(this Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return Vector3.up;
                case Direction.Left:
                    return Vector3.left;
                case Direction.Down:
                    return Vector3.down;
                case Direction.Right:
                    return Vector3.right;
                default:
                    return Vector3.up;
            }
        }

        //Extension method to get the Vector2 based on the direction. 
        public static Vector2 Vec2(this Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return Vector2.up;
                case Direction.Left:
                    return Vector2.left;
                case Direction.Down:
                    return Vector2.down;
                case Direction.Right:
                    return Vector2.right;
                default:
                    return Vector2.up;
            }
        }

        //Extension method to get the Vector3 based on the Transform's direction. 
        public static Vector3 Trans(this Direction dir, Transform t)
        {
            switch (dir)
            {
                case Direction.Up:
                    return t.up;
                case Direction.Left:
                    return t.right * -1;
                case Direction.Down:
                    return t.up * -1;
                case Direction.Right:
                    return t.right;
                default:
                    return t.up;
            }
        }
    }

    public class Utilties
    {

        public static Vector2 Vec32(Vector3 v3)
        {
            return new Vector2(v3.x, v3.y);
        }

        public static Vector3 Vec23(Vector2 v2, float z)
        {
            return new Vector3(v2.x, v2.y, z);
        }

        public static Vector3 Vec23(Vector2 v2)
        {
            return Vec23(v2, 0);
        }

    }
}