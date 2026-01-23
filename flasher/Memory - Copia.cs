using System;
using System.Runtime.InteropServices;
using System.Numerics;
using System.Linq;

public static class Memory
{
    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(IntPtr h, IntPtr adr, byte[] buf, int sz, out int rd);
    [DllImport("kernel32.dll")]
    public static extern bool WriteProcessMemory(IntPtr h, IntPtr adr, byte[] buf, int sz, out int wr);

    public static int ReadInt(IntPtr h, IntPtr a) { byte[] b = new byte[4]; ReadProcessMemory(h, a, b, 4, out _); return BitConverter.ToInt32(b, 0); }
    public static IntPtr ReadPtr(IntPtr h, IntPtr a) { byte[] b = new byte[8]; ReadProcessMemory(h, a, b, 8, out _); return (IntPtr)BitConverter.ToInt64(b, 0); }
    public static float ReadFloat(IntPtr h, IntPtr a) { byte[] b = new byte[4]; ReadProcessMemory(h, a, b, 4, out _); return BitConverter.ToSingle(b, 0); }
    public static void WriteFloat(IntPtr h, IntPtr a, float v) { WriteProcessMemory(h, a, BitConverter.GetBytes(v), 4, out _); }

    public static Vector2 ReadVec2(IntPtr h, IntPtr a) { byte[] b = new byte[8]; ReadProcessMemory(h, a, b, 8, out _); return new Vector2(BitConverter.ToSingle(b, 0), BitConverter.ToSingle(b, 4)); }
    public static void WriteVec2(IntPtr h, IntPtr a, Vector2 v)
    {
        byte[] b = BitConverter.GetBytes(v.X).Concat(BitConverter.GetBytes(v.Y)).ToArray();
        WriteProcessMemory(h, a, b, 8, out _);
    }
}