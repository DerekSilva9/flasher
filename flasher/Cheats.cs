using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;

public class Cheats
{
    [DllImport("user32.dll")]
    public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

    const int KEYEVENTF_KEYUP = 0x0002;
    private Vector2 oldPunch = new Vector2(0, 0);
    private bool jumpToggle = false;

    public void RunNoFlash(IntPtr handle, IntPtr localPawn, nint offset)
    {
        Memory.WriteFloat(handle, localPawn + offset, 0f);
    }
    public void RunRCS(IntPtr handle, IntPtr localPawn, IntPtr clientDll, nint shotsOffset, nint punchOffset, nint viewOffset)
    {
        int shots = Memory.ReadInt(handle, localPawn + shotsOffset);
        if (shots > 1)
        {
            Vector2 punch = Memory.ReadVec2(handle, localPawn + punchOffset);
            Vector2 viewAngles = Memory.ReadVec2(handle, clientDll + viewOffset);
            Vector2 newAngles = new Vector2
            {
                X = viewAngles.X + (oldPunch.X - punch.X) * 2.0f,
                Y = viewAngles.Y + (oldPunch.Y - punch.Y) * 2.0f
            };
            Memory.WriteVec2(handle, clientDll + viewOffset, newAngles);
            oldPunch = punch;
        }
        else { oldPunch = new Vector2(0, 0); }
    }
    public void RunBunnyHop(IntPtr handle, IntPtr clientDll, nint jumpOffset)
    {
        if ((Program.GetAsyncKeyState(0x20) & 0x8000) != 0)
        {
            if (!jumpToggle)
            {
                Memory.WriteInt(handle, clientDll + jumpOffset, 65537);
                jumpToggle = true;
            }
            else
            {
                Memory.WriteInt(handle, clientDll + jumpOffset, 256);
                jumpToggle = false;
            }
        }
        else { Memory.WriteInt(handle, clientDll + jumpOffset, 256); }
    }
}