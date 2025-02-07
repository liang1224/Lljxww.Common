﻿namespace Lljxww.ConsoleTool.Utils;

public class TimestampUtil
{
    private static readonly DateTime STADNARD = new(1970, 1, 1, 0, 0, 0);

    /// <summary>
    /// 查询当前的Unix时间戳
    /// </summary>
    /// <returns></returns>
    internal static string GetCurrent()
    {
        return Math.Floor((DateTime.Now - STADNARD).TotalSeconds).ToString();
    }
}
