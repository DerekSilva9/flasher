using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Linq;

class Program
{
    [DllImport("user32.dll")] public static extern short GetAsyncKeyState(int v);
    [DllImport("user32.dll")] public static extern void mouse_event(uint f, uint x, uint y, uint d, int e);
    
    const nint dwLocalPlayerPawn = 0x2064AE0;
    const nint dwJump = 0x205DD70;
    const nint dwViewAngles = 0x2314F98;
    const nint m_iIDEntIndex = 0x3EAC;
    const nint m_aimPunchAngle = 0x16CC;
    const nint m_iShotsFired = 0x270C;
    const nint m_flFlashMaxAlpha = 0x15F4;
    const nint m_fFlags = 0x400;


    static GuiForm? gui;

    [STAThread]
    static void Main()
    {
        try
        {
            // Remova a linha ApplicationConfiguration.Initialize();

            // Use estas 3 linhas que funcionam em qualquer versão do .NET:
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Process game = Process.GetProcessesByName("cs2")[0];
            IntPtr clientDll = GetModule(game, "client.dll");

            Cheats hackMudar = new Cheats();

            Thread hackThread = new Thread(() => HackLoop(game.Handle, clientDll, hackMudar))
            {
                IsBackground = true
            };
            hackThread.Start();

            gui = new GuiForm();
            Application.Run(gui);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    static void HackLoop(IntPtr gameHandle, IntPtr clientDll, Cheats hackMudar)
    {
        while (gui == null) Thread.Sleep(100);
        
        while (!gui.IsDisposed)
        {
            try
            {
                IntPtr localPawn = Memory.ReadPtr(gameHandle, clientDll + dwLocalPlayerPawn);
                if (localPawn == IntPtr.Zero) { Thread.Sleep(100); continue; }
                
                if (gui.NoFlashEnabled) 
                    hackMudar.RunNoFlash(gameHandle, localPawn, m_flFlashMaxAlpha);
                
                if (gui.RcsEnabled) 
                    hackMudar.RunRCS(gameHandle, localPawn, clientDll, m_iShotsFired, m_aimPunchAngle, dwViewAngles);
                
                if (gui.TriggerEnabled && (GetAsyncKeyState(0x12) & 0x8000) != 0)
                {
                    int id = Memory.ReadInt(gameHandle, localPawn + m_iIDEntIndex);
                    if (id > 0 && id < 1000)
                    {
                        Thread.Sleep(gui.TriggerPrecision);
                        mouse_event(0x0002, 0, 0, 0, 0);
                        Thread.Sleep(gui.TriggerHoldTime);
                        mouse_event(0x0004, 0, 0, 0, 0);
                        Thread.Sleep(gui.TriggerCadence);
                    }
                }

                if (gui.BhopEnabled)
                {
                    // Passamos apenas: o Handle do processo, a Base da DLL e o Offset do Pulo
                    hackMudar.RunBunnyHop(gameHandle, clientDll, dwJump);
                }



                Thread.Sleep(1);
            }
            catch { Thread.Sleep(1000); }
        }
    }
    
    static IntPtr GetModule(Process p, string n) => 
        p.Modules.Cast<ProcessModule>().First(m => m.ModuleName == n).BaseAddress;
}