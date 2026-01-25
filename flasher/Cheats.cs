using System;
using System.Numerics;
using System.Threading;

public class Cheats
{
    private Vector2 oldPunch = new Vector2(0, 0);
    private const int FL_ONGROUND = 256; // Flag indicating the player is on ground
    private bool bhopEnabled = true;
    private Thread bhopThread;
    private bool isRunning = true;

    public Cheats()
    {
        // Start the bhop thread when the Cheats object is created
        bhopThread = new Thread(BunnyHopThread);
        bhopThread.IsBackground = true;
        bhopThread.Start();
    }

    public void Stop()
    {
        // Method to clean up the thread when done
        isRunning = false;
        bhopThread?.Join();
    }

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

    // Bunny hop method
    public void RunBunnyHop(IntPtr handle, IntPtr localPawn, nint flagOffset, nint jumpOffset)
    {
        if (!bhopEnabled) return;

        int flags = Memory.ReadInt(handle, localPawn + flagOffset);

        // Check if player is on ground
        if ((flags & FL_ONGROUND) == FL_ONGROUND)
        {
            // Force jump by writing 5 to jump state (4 for hold, 5 for press, 6 for release)
            Memory.WriteInt(handle, localPawn + jumpOffset, 5);
        }
        else
        {
            // Release jump when in air
            Memory.WriteInt(handle, localPawn + jumpOffset, 4);
        }
    }

    // Thread-based continuous bhop method
    private void BunnyHopThread()
    {
        while (isRunning)
        {
            // This thread would need access to handle, localPawn, and offsets
            // You would need to pass these somehow or structure your program differently
            Thread.Sleep(1); // Sleep to prevent CPU overuse
        }
    }

    // Alternative: Main loop compatible method (if you have a game loop)
    public void UpdateBunnyHop(IntPtr handle, IntPtr localPawn, nint flagOffset, nint jumpOffset)
    {
        RunBunnyHop(handle, localPawn, flagOffset, jumpOffset);
    }

    // Toggle bhop on/off
    public void ToggleBunnyHop(bool enabled)
    {
        bhopEnabled = enabled;
    }

    // Alternative simpler bhop implementation using key states
    public void RunSimpleBunnyHop(IntPtr handle, IntPtr localPawn, nint flagOffset, nint jumpOffset)
    {
        if (!bhopEnabled) return;

        int flags = Memory.ReadInt(handle, localPawn + flagOffset);

        // If space is being held and player is on ground, jump
        if ((flags & FL_ONGROUND) == FL_ONGROUND)
        {
            // You might want to check if space key is actually pressed
            // For now, we assume it's always active when bhop is enabled
            Memory.WriteInt(handle, localPawn + jumpOffset, 6); // Press and release
            Thread.Sleep(10); // Small delay
            Memory.WriteInt(handle, localPawn + jumpOffset, 4); // Reset
        }
    }
}