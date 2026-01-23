using System;
using System.Numerics;

public class Cheats
{
    private Vector2 oldPunch = new Vector2(0, 0);

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
                X = viewAngles.X + (oldPunch.X - punch.X) * 2.0f, // Força do RCS (2.0f)
                Y = viewAngles.Y + (oldPunch.Y - punch.Y) * 2.0f
            };
            Memory.WriteVec2(handle, clientDll + viewOffset, newAngles);
            oldPunch = punch;
        }
        else { oldPunch = new Vector2(0, 0); }
    }
}