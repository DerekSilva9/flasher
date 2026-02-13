using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

class Program
{
    [DllImport("user32.dll")] public static extern short GetAsyncKeyState(int v);
    [DllImport("user32.dll")] public static extern void mouse_event(uint f, uint x, uint y, uint d, int e);

    static nint dwLocalPlayerPawn;
    static nint dwJump;
    static nint dwViewAngles;
    static nint m_iIDEntIndex;
    static nint m_aimPunchAngle;
    static nint m_iShotsFired;
    static nint m_flFlashMaxAlpha;
    static nint m_fFlags;
    static async Task LoadOffsets()
    {
        var o = await OffsetManager.LoadAsync();

        dwLocalPlayerPawn = o["dwLocalPlayerPawn"];
        dwJump = o["jump"];
        dwViewAngles = o["dwViewAngles"];
        m_iIDEntIndex = o["m_iIDEntIndex"];
        m_aimPunchAngle = o["m_aimPunchAngle"];
        m_iShotsFired = o["m_iShotsFired"];
        m_flFlashMaxAlpha = o["m_flFlashMaxAlpha"];
        m_fFlags = o["m_fFlags"];
    }

    static GuiForm? gui;

    [STAThread]
    static void Main()
    {
        LoadOffsets().GetAwaiter().GetResult();

        try
        {
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