using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;

class Program
{
    [DllImport("user32.dll")] public static extern short GetAsyncKeyState(int v);
    [DllImport("user32.dll")] public static extern void mouse_event(uint f, uint x, uint y, uint d, int e);

    const nint dwLocalPlayerPawn = 0x2061850;
    const nint dwViewAngles = 0x2311A68;
    const nint m_iIDEntIndex = 0x3EAC;
    const nint m_aimPunchAngle = 0x16CC;
    const nint m_iShotsFired = 0x270C;
    const nint m_flFlashMaxAlpha = 0x15F4;

    static void Main()
    {
        Console.Title = "CS2 Multi-Hack Organizado";
        Process game = Process.GetProcessesByName("cs2")[0];
        IntPtr clientDll = GetModule(game, "client.dll");

        Cheats hackMudar = new Cheats(); // Criamos a "ferramenta" de hacks

        bool rcsOn = false, flashOn = false, triggerOn = false;

        Console.WriteLine("F1: RCS | F2: NoFlash | F3: Trigger | ALT: Atirar");

        while (true)
        {
            // Atalhos para ligar/desligar
            if ((GetAsyncKeyState(0x70) & 1) != 0) { rcsOn = !rcsOn; Console.Beep(rcsOn ? 800 : 400, 100); }
            if ((GetAsyncKeyState(0x71) & 1) != 0) { flashOn = !flashOn; Console.Beep(flashOn ? 800 : 400, 100); }
            if ((GetAsyncKeyState(0x72) & 1) != 0) { triggerOn = !triggerOn; Console.Beep(triggerOn ? 800 : 400, 100); }

            IntPtr localPawn = Memory.ReadPtr(game.Handle, clientDll + dwLocalPlayerPawn);
            if (localPawn == IntPtr.Zero) continue;

            // Roda NoFlash e RCS
            if (flashOn) hackMudar.RunNoFlash(game.Handle, localPawn, m_flFlashMaxAlpha);
            if (rcsOn) hackMudar.RunRCS(game.Handle, localPawn, clientDll, m_iShotsFired, m_aimPunchAngle, dwViewAngles);

            // Sua lógica de Triggerbot (Segura ALT)
            if (triggerOn && (GetAsyncKeyState(0x12) & 0x8000) != 0)
            {
                int id = Memory.ReadInt(game.Handle, localPawn + m_iIDEntIndex);
                if (id > 0)
                {
                    mouse_event(0x0002, 0, 0, 0, 0); // Down
                    Thread.Sleep(20);
                    mouse_event(0x0004, 0, 0, 0, 0); // Up
                    Thread.Sleep(150);
                }
            }
            Thread.Sleep(1);
        }
    }

    static IntPtr GetModule(Process p, string n) => p.Modules.Cast<ProcessModule>().First(m => m.ModuleName == n).BaseAddress;
}