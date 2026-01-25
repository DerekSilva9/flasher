using System;
using System.Runtime.InteropServices;
using System.Numerics; // Necessário para o Vector2

public static class Memory
{
    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(IntPtr h, IntPtr adr, byte[] buf, int sz, out int rd);

    [DllImport("kernel32.dll")]
    public static extern bool WriteProcessMemory(IntPtr h, IntPtr adr, byte[] buf, int sz, out int wr);

    // --- LEITURAS ---
    public static int ReadInt(IntPtr h, IntPtr a) { byte[] b = new byte[4]; ReadProcessMemory(h, a, b, 4, out _); return BitConverter.ToInt32(b, 0); }
    public static IntPtr ReadPtr(IntPtr h, IntPtr a) { byte[] b = new byte[8]; ReadProcessMemory(h, a, b, 8, out _); return (IntPtr)BitConverter.ToInt64(b, 0); }
    public static float ReadFloat(IntPtr h, IntPtr a) { byte[] b = new byte[4]; ReadProcessMemory(h, a, b, 4, out _); return BitConverter.ToSingle(b, 0); }

    // Adicionando ReadVec2 que estava faltando
    public static Vector2 ReadVec2(IntPtr h, IntPtr a)
    {
        byte[] b = new byte[8];
        ReadProcessMemory(h, a, b, 8, out _);
        return new Vector2(BitConverter.ToSingle(b, 0), BitConverter.ToSingle(b, 4));
    }

    // --- ESCRITAS ---
    // Ajustado para retornar 'bool' e resolver o erro de conversão implícita
    public static bool WriteInt(IntPtr h, IntPtr a, int v)
    {
        byte[] b = BitConverter.GetBytes(v);
        return WriteProcessMemory(h, a, b, b.Length, out _);
    }

    public static bool WriteBool(IntPtr h, IntPtr a, bool v)
    {
        byte[] b = new byte[] { v ? (byte)1 : (byte)0 };
        return WriteProcessMemory(h, a, b, 1, out _);
    }

    public static bool WriteFloat(IntPtr h, IntPtr a, float v)
    {
        byte[] b = BitConverter.GetBytes(v);
        return WriteProcessMemory(h, a, b, 4, out _);
    }

    // Adicionando WriteVec2 que estava faltando
    public static bool WriteVec2(IntPtr h, IntPtr a, Vector2 v)
    {
        byte[] x = BitConverter.GetBytes(v.X);
        byte[] y = BitConverter.GetBytes(v.Y);
        byte[] b = new byte[8];
        Buffer.BlockCopy(x, 0, b, 0, 4);
        Buffer.BlockCopy(y, 0, b, 4, 4);
        return WriteProcessMemory(h, a, b, 8, out _);
    }
}