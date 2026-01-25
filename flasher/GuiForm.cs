using System;
using System.Drawing;
using System.Windows.Forms;

public class GuiForm : Form
{
    public bool RcsEnabled = false;
    public bool NoFlashEnabled = false;
    public bool TriggerEnabled = false;

    public int TriggerPrecision = 25;
    public int TriggerCadence = 20;
    public int TriggerHoldTime = 10;

    private CheckBox chkRcs, chkNoFlash, chkTrigger;
    private TrackBar sliderPrecision, sliderCadence, sliderHoldTime;
    private Label lblPrecisionValue, lblCadenceValue, lblHoldValue;
    private Label lblRcsStatus, lblFlashStatus, lblTriggerStatus;
    private Button btnPresetPistol, btnPresetRifle, btnPresetAwp;

    public GuiForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Text = "CS2 Multi-Hack Controller";
        this.Size = new Size(480, 620);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(25, 25, 30);
        this.ForeColor = Color.White;

        int y = 20;

        Label lblHeader = new Label
        {
            Text = "═══ COUNTER-STRIKE 2 CONTROLLER ═══",
            Location = new Point(20, y),
            Size = new Size(440, 25),
            Font = new Font("Consolas", 12, FontStyle.Bold),
            ForeColor = Color.FromArgb(51, 204, 255),
            TextAlign = ContentAlignment.MiddleCenter
        };
        this.Controls.Add(lblHeader);
        y += 40;

        GroupBox grpFeatures = new GroupBox
        {
            Text = "Features",
            Location = new Point(20, y),
            Size = new Size(435, 150),
            ForeColor = Color.LightGray,
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };

        chkRcs = new CheckBox
        {
            Text = "Recoil Control System (RCS)",
            Location = new Point(15, 30),
            Size = new Size(250, 25),
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.White
        };
        chkRcs.CheckedChanged += (s, e) => { RcsEnabled = chkRcs.Checked; UpdateStatus(); };
        grpFeatures.Controls.Add(chkRcs);

        lblRcsStatus = new Label
        {
            Text = "F1",
            Location = new Point(270, 32),
            Size = new Size(150, 20),
            ForeColor = Color.Gray,
            Font = new Font("Segoe UI", 8)
        };
        grpFeatures.Controls.Add(lblRcsStatus);

        chkNoFlash = new CheckBox
        {
            Text = "No Flash",
            Location = new Point(15, 65),
            Size = new Size(250, 25),
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.White
        };
        chkNoFlash.CheckedChanged += (s, e) => { NoFlashEnabled = chkNoFlash.Checked; UpdateStatus(); };
        grpFeatures.Controls.Add(chkNoFlash);

        lblFlashStatus = new Label
        {
            Text = "F2",
            Location = new Point(270, 67),
            Size = new Size(150, 20),
            ForeColor = Color.Gray,
            Font = new Font("Segoe UI", 8)
        };
        grpFeatures.Controls.Add(lblFlashStatus);

        chkTrigger = new CheckBox
        {
            Text = "Triggerbot",
            Location = new Point(15, 100),
            Size = new Size(250, 25),
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.White
        };
        chkTrigger.CheckedChanged += (s, e) => { TriggerEnabled = chkTrigger.Checked; UpdateStatus(); };
        grpFeatures.Controls.Add(chkTrigger);

        lblTriggerStatus = new Label
        {
            Text = "F3 | ALT para atirar",
            Location = new Point(270, 102),
            Size = new Size(150, 20),
            ForeColor = Color.Gray,
            Font = new Font("Segoe UI", 8)
        };
        grpFeatures.Controls.Add(lblTriggerStatus);

        this.Controls.Add(grpFeatures);
        y += 165;

        GroupBox grpTrigger = new GroupBox
        {
            Text = "Triggerbot Settings",
            Location = new Point(20, y),
            Size = new Size(435, 240),
            ForeColor = Color.LightGray,
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };

        Label lblPrecision = new Label
        {
            Text = "Ajuste de Precisão:",
            Location = new Point(15, 30),
            Size = new Size(200, 20),
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.LightGray
        };
        grpTrigger.Controls.Add(lblPrecision);

        lblPrecisionValue = new Label
        {
            Text = "25ms",
            Location = new Point(360, 30),
            Size = new Size(60, 20),
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.FromArgb(51, 204, 255),
            Font = new Font("Consolas", 9, FontStyle.Bold)
        };
        grpTrigger.Controls.Add(lblPrecisionValue);

        sliderPrecision = new TrackBar
        {
            Location = new Point(15, 55),
            Size = new Size(405, 45),
            Minimum = 0,
            Maximum = 100,
            Value = 25,
            TickFrequency = 10
        };
        sliderPrecision.ValueChanged += (s, e) =>
        {
            TriggerPrecision = sliderPrecision.Value;
            lblPrecisionValue.Text = $"{TriggerPrecision}ms";
        };
        grpTrigger.Controls.Add(sliderPrecision);

        Label lblHold = new Label
        {
            Text = "Tempo de Segurar:",
            Location = new Point(15, 100),
            Size = new Size(200, 20),
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.LightGray
        };
        grpTrigger.Controls.Add(lblHold);

        lblHoldValue = new Label
        {
            Text = "10ms",
            Location = new Point(360, 100),
            Size = new Size(60, 20),
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.FromArgb(51, 204, 255),
            Font = new Font("Consolas", 9, FontStyle.Bold)
        };
        grpTrigger.Controls.Add(lblHoldValue);

        sliderHoldTime = new TrackBar
        {
            Location = new Point(15, 125),
            Size = new Size(405, 45),
            Minimum = 5,
            Maximum = 50,
            Value = 10,
            TickFrequency = 5
        };
        sliderHoldTime.ValueChanged += (s, e) =>
        {
            TriggerHoldTime = sliderHoldTime.Value;
            lblHoldValue.Text = $"{TriggerHoldTime}ms";
        };
        grpTrigger.Controls.Add(sliderHoldTime);

        Label lblCadence = new Label
        {
            Text = "Cadência entre Tiros:",
            Location = new Point(15, 170),
            Size = new Size(200, 20),
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.LightGray
        };
        grpTrigger.Controls.Add(lblCadence);

        lblCadenceValue = new Label
        {
            Text = "20ms",
            Location = new Point(360, 170),
            Size = new Size(60, 20),
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.FromArgb(51, 204, 255),
            Font = new Font("Consolas", 9, FontStyle.Bold)
        };
        grpTrigger.Controls.Add(lblCadenceValue);

        sliderCadence = new TrackBar
        {
            Location = new Point(15, 195),
            Size = new Size(405, 45),
            Minimum = 0,
            Maximum = 200,
            Value = 20,
            TickFrequency = 20
        };
        sliderCadence.ValueChanged += (s, e) =>
        {
            TriggerCadence = sliderCadence.Value;
            lblCadenceValue.Text = $"{TriggerCadence}ms";
        };
        grpTrigger.Controls.Add(sliderCadence);

        this.Controls.Add(grpTrigger);
        y += 255;

        Label lblPresets = new Label
        {
            Text = "Presets Rápidos:",
            Location = new Point(20, y),
            Size = new Size(150, 20),
            Font = new Font("Segoe UI", 9, FontStyle.Bold)
        };
        this.Controls.Add(lblPresets);
        y += 25;

        btnPresetPistol = new Button
        {
            Text = "🎯 Pistola Rápida",
            Location = new Point(20, y),
            Size = new Size(135, 35),
            BackColor = Color.FromArgb(51, 204, 255),
            ForeColor = Color.Black,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btnPresetPistol.FlatAppearance.BorderSize = 0;
        btnPresetPistol.Click += (s, e) => ApplyPreset(15, 8, 10);
        this.Controls.Add(btnPresetPistol);

        btnPresetRifle = new Button
        {
            Text = "🔫 Rifle Preciso",
            Location = new Point(165, y),
            Size = new Size(135, 35),
            BackColor = Color.FromArgb(51, 204, 255),
            ForeColor = Color.Black,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btnPresetRifle.FlatAppearance.BorderSize = 0;
        btnPresetRifle.Click += (s, e) => ApplyPreset(35, 12, 30);
        this.Controls.Add(btnPresetRifle);

        btnPresetAwp = new Button
        {
            Text = "💥 AWP/Scout",
            Location = new Point(310, y),
            Size = new Size(145, 35),
            BackColor = Color.FromArgb(51, 204, 255),
            ForeColor = Color.Black,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btnPresetAwp.FlatAppearance.BorderSize = 0;
        btnPresetAwp.Click += (s, e) => ApplyPreset(50, 15, 100);
        this.Controls.Add(btnPresetAwp);

        y += 50;

        Label lblFooter = new Label
        {
            Text = "Desenvolvido para fins educacionais • Use com -insecure",
            Location = new Point(20, y),
            Size = new Size(435, 20),
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.Gray,
            Font = new Font("Segoe UI", 8)
        };
        this.Controls.Add(lblFooter);
    }

    private void ApplyPreset(int precision, int hold, int cadence)
    {
        sliderPrecision.Value = precision;
        sliderHoldTime.Value = hold;
        sliderCadence.Value = cadence;
    }

    private void UpdateStatus()
    {
        lblRcsStatus.Text = RcsEnabled ? "● ATIVO (F1)" : "F1";
        lblRcsStatus.ForeColor = RcsEnabled ? Color.FromArgb(50, 255, 100) : Color.Gray;

        lblFlashStatus.Text = NoFlashEnabled ? "● ATIVO (F2)" : "F2";
        lblFlashStatus.ForeColor = NoFlashEnabled ? Color.FromArgb(50, 255, 100) : Color.Gray;

        lblTriggerStatus.Text = TriggerEnabled ? "● ATIVO (F3 | ALT)" : "F3 | ALT para atirar";
        lblTriggerStatus.ForeColor = TriggerEnabled ? Color.FromArgb(50, 255, 100) : Color.Gray;
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.F1) { chkRcs.Checked = !chkRcs.Checked; return true; }
        if (keyData == Keys.F2) { chkNoFlash.Checked = !chkNoFlash.Checked; return true; }
        if (keyData == Keys.F3) { chkTrigger.Checked = !chkTrigger.Checked; return true; }
        return base.ProcessCmdKey(ref msg, keyData);
    }
}