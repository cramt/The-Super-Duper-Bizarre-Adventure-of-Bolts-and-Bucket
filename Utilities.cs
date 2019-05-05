using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TheSuperDuperBizzareAdventureOfBoltsAndBuckets;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// a mcfuckton of function i might need or do need for something someday
/// </summary>
public static class Utilities {
    /// <summary>
    /// class to make sure Console.Writeline writes to Debug.Log
    /// it inherits TextWriter
    /// </summary>
    private class UnityLogWriter : TextWriter {
        /// <summary>
        /// define encoding type
        /// </summary>
        public override Encoding Encoding { get; } = Encoding.UTF8;
        /// <summary>
        /// the buffer of the string
        /// </summary>
        string buffer = "";
        /// <summary>
        /// when we get a char
        /// </summary>
        /// <param name="value"></param>
        public override void Write(char value) {
            base.Write(value);
            //add it to buffer
            buffer += value;
            //if we have a newline char
            if (buffer.Contains(Environment.NewLine)) {
                //debug.log it 
                Debug.Log(buffer);
                //reset buffer
                buffer = "";
            }
        }
    }
    /// <summary>
    /// variable for getting the levels we have completed
    /// its hooked to a file called "levels"
    /// </summary>
    public static int LevelsCompleted {
        get {
            //when "getted" return the file after parsing it as an int
            return int.Parse(File.ReadAllText(levelsFile));
        }
        set {
            //when "setting" write that to the file
            File.WriteAllText(levelsFile, value + "");
        }
    }
    private const string levelsFile = "levels";


    //this region here is a massive hack to trick unity into running this function when the code is loaded
    #region hack for a "main function"
    private static bool LoadFunction() {
        //if the file "levels" doesnt exists
        //make it and write 0 to it
        if (!File.Exists(levelsFile)) {
            File.Create(levelsFile).Close();
            File.WriteAllText(levelsFile, "0");
        }
        // set the Console.WriteLine to write to Debug.Log
        Console.SetOut(new UnityLogWriter());

        return true;
    }
    public static bool Load = LoadFunction();
    #endregion
    /// <summary>
    /// create a line
    /// </summary>
    /// <param name="pos1">position 1</param>
    /// <param name="pos2">position 2</param>
    /// <returns>the line</returns>
    public static LineController SpawnNewLine(Transform pos1, Transform pos2) {
        //crate a line based on the line prefab, and get its line controller
        LineController l = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("line")).GetComponent<LineController>();
        //set the positions
        l.Position1 = pos1;
        l.Position2 = pos2;
        //return it
        return l;
    }
    public static Vector3 ToVector3(this float[] f) {
        return new Vector3(f[0], f[1], f[2]);
    }
    public static Quaternion ToQuaternion(this float[] f) {
        return new Quaternion(f[0], f[1], f[2], f[3]);
    }
    public static float[] ToFloatArray(this Vector3 v) {
        return new float[] { v.x, v.y, v.z };
    }
    public static float[] ToFloatArray(this Quaternion q) {
        return new float[] { q.x, q.y, q.z, q.w };
    }
    /// <summary>
    /// gets absolute value of vector3
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector3 Abs(this Vector3 v) {
        return new Vector3(Math.Abs(v.x), Math.Abs(v.y), Math.Abs(v.z));
    }
    /// <summary>
    /// finds nearest point on line
    /// </summary>
    /// <param name="origin">the origin of the line</param>
    /// <param name="end">the end of the line</param>
    /// <param name="point">your current point</param>
    /// <returns></returns>
    public static Vector3 FindNearestPointOnLine(Vector3 origin, Vector3 end, Vector3 point) {
        Vector3 heading = end - origin;
        float magnitudeMax = heading.magnitude;
        heading.Normalize();
        Vector3 lhs = point - origin;
        float dotP = Vector3.Dot(lhs, heading);
        dotP = Mathf.Clamp(dotP, 0f, magnitudeMax);
        return origin + heading * dotP;
    }
    /// <summary>
    /// clones some component
    /// </summary>
    /// <typeparam name="T">the type of the original</typeparam>
    /// <param name="original">the original</param>
    /// <param name="destination">where the clone should be applied to</param>
    /// <returns>the new component</returns>
    public static T CloneComponent<T>(T original, GameObject destination) where T : Component {
        //get the type of the original
        Type type = original.GetType();
        //add the that type to the component
        Component copy = destination.AddComponent(type);
        //get all tis fields
        FieldInfo[] fields = type.GetFields();
        foreach (FieldInfo field in fields) {
            //get the value from the old field and apply it to the new
            field.SetValue(copy, field.GetValue(original));
        }
        //do the same thing with the properties
        PropertyInfo[] properties = type.GetProperties();
        foreach (PropertyInfo property in properties) {
            if (property.CanWrite) {
                property.SetValue(copy, property.GetValue(original));
            }
        }
        //return the copy
        return copy as T;
    }
    /// <summary>
    /// gets center point of a bunch of vector3's
    /// </summary>
    /// <param name="p">list of vectors</param>
    /// <returns>the center point</returns>
    public static Vector3 CenterPoint(this List<Vector3> p) {
        if (p.Count == 1) {
            return p[0];
        }
        p.Add(CenterPoint(p[0], p[1]));
        p.RemoveAt(0);
        p.RemoveAt(0);
        return CenterPoint(p);
    }
    /// <summary>
    /// gets center point between 2 vector3's
    /// </summary>
    /// <param name="p1">point 1</param>
    /// <param name="p2">point 2</param>
    /// <returns>the center point</returns>
    public static Vector3 CenterPoint(Vector3 p1, Vector3 p2) {
        return (p1 + p2) / 2;
    }
    /// <summary>
    /// sets the game objects in question to be between 2 vector3's
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    public static void SetBetween(this GameObject gameObject, Vector3 p1, Vector3 p2) {
        Vector3 centerPos = CenterPoint(p1, p2);

        gameObject.transform.position = centerPos;
        gameObject.transform.LookAt(p2);
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, Vector3.Distance(p1, p2));

    }
}