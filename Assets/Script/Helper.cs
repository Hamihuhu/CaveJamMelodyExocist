using System;
using UnityEngine;

internal static class Helper
{
    public static Vector3 RandomPosition(Vector3 _posCurrent, float minVal, float maxVal)
    {
        return _posCurrent + new Vector3(UnityEngine.Random.Range(minVal, maxVal),
                                         UnityEngine.Random.Range(minVal, maxVal),
                                          UnityEngine.Random.Range(minVal, maxVal));
    }
    public static string SafeSubstring(this string str, int startIndex, int length)
    {
        if (startIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(startIndex), "Start index cannot be negative.");
        }

        if (string.IsNullOrEmpty(str) || startIndex >= str.Length)
        {
            return string.Empty;
        }

        int remainingLength = Math.Min(length, str.Length - startIndex);
        return str.Substring(startIndex, remainingLength);
    }

    public static string CreateSubstring(string originalString, int startIndex, int length)
    {
        if (startIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(startIndex), "Start index cannot be negative.");
        }

        int originalLength = originalString.Length;

        if (originalLength < length)
        {
            originalString = originalString.PadRight(length, ' ');
        }

        return originalString.Substring(startIndex, Math.Min(length, originalLength - startIndex));
    }
}
