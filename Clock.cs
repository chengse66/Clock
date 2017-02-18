using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Threading;
/// <summary>
/// 公用函数时间控制函数
/// </summary>
public class Clock
{
    private Dictionary<int, System.Timers.Timer> map;
    private int id = 0;
    public delegate void ClockMethod(object item);

    private Clock() {
        map = new Dictionary<int, System.Timers.Timer>();
    }
    private static Clock __clock = null;
    private static Clock _clock {
        get {
            if (__clock == null) __clock = new Clock();
            return __clock;
        }
    }

    /// <summary>
    /// SetInterval 每隔一段时间运行一段函数
    /// </summary>
    /// <param name="millisecond">毫秒数</param>
    /// <param name="m">回调函数,只有一个object参数</param>
    /// <param name="item">传入的值</param>
    /// <returns>时间ID</returns>
    public static int SetInterval(int millisecond, ClockMethod m, object item)
    {
        int currentId = ++_clock.id;
        System.Timers.Timer t = new System.Timers.Timer(millisecond);
        t.AutoReset = true;
        t.Enabled = true;
        t.Elapsed += (a, b) =>
        {
            m.Invoke(item);
        };
        _clock.map[currentId] = t;
        return currentId;
    }

    /// <summary>
    /// SetInterval 每隔一段时间运行函数
    /// </summary>
    /// <param name="millisecond">毫秒数</param>
    /// <param name="m">回调函数,只有一个object参数</param>
    /// <returns>时间ID</returns>
    public static int SetInterval(int millisecond, ClockMethod m) {
        return SetInterval(millisecond, m, null);
    }

    /// <summary>
    /// SetTimeout 等待一段时间运行函数
    /// </summary>
    /// <param name="millisecond">毫秒数</param>
    /// <param name="m">回调函数,只有一个object参数</param>
    /// <param name="item">传入的值</param>
    /// <returns>时间ID</returns>
    public static int SetTimeout(int millisecond, ClockMethod m,object item)
    {
        int currentId = ++_clock.id;
        System.Timers.Timer t = new System.Timers.Timer(millisecond);
        t.AutoReset = false;
        t.Enabled = true;
        t.Elapsed += (a, b) =>
        {
            m.Invoke(item);
            Clear(currentId);
        };
        _clock.map[currentId] = t;
        return currentId;
    }

    /// <summary>
    /// SetTimeout 等待一段时间运行函数
    /// </summary>
    /// <param name="millisecond">毫秒数</param>
    /// <param name="m">回调函数,只有一个object参数</param>
    /// <returns>时间ID</returns>
    public static int SetTimeout(int millisecond, ClockMethod m)
    {
        return SetTimeout(millisecond, m,null);
    }

    /// <summary>
    /// 根据ID清空执行对象
    /// </summary>
    /// <param name="id"></param>
    public static void Clear(int id)
    {
        if (_clock.map.ContainsKey(id))
        {
            System.Timers.Timer t = _clock.map[id];
            t.Enabled = false;
            t.Dispose();
            _clock.map.Remove(id);
        }

    }
}