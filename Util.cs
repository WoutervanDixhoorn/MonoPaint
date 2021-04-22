using System.ComponentModel;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace MonoPaint
{
    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name)
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }

        public static String GetDescription(this Enum value)
        {
            var description = GetAttribute<DescriptionAttribute>(value);
            return description != null ? description.Description : null;
        } 
    }

    public class Util
    {
        
        public static int Clamp(int value, int min, int max)
        {
            if(value < min)
            {
                return min;
            }else if(value > max)
            {
                return max;
            }

            return value;
        }

    }

    public static class OperatingSystem
    {
        public static bool IsWindows() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static bool IsMacOS() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public static bool IsLinux() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    }
}