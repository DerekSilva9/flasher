using System;
using System.Drawing;
using System.Windows.Forms;

public class GuiForm : Form
{
    // Variáveis de Controle
    public bool RcsEnabled = false;
    public bool NoFlashEnabled = false;
    public bool TriggerEnabled = false;
    public bool BhopEnabled = false;

    public int TriggerPrecision = 25;
    public int TriggerCadence = 20;
    public int TriggerHoldTime = 10;

    // Componentes da UI
    private CheckBox chkRcs, chkNoFlash, chkTrigger, chkBhop;
    private TrackBar sliderPrecision, sliderCadence, sliderHoldTime;
    private Label lblPrecisionValue, lblCadenceValue, lblHoldValue;
    private Label lblRcsStatus, lblFlashStatus, lblTriggerStatus, lblBhopStatus;

    public GuiForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Text = "Flasher CS2 External - v1.2";
        this.Size = new Size(480, 620);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.BackColor = Color.FromArgb(18, 18, 22);
        this.ForeColor = Color.White;
        this.Font = new Font("Segoe UI", 9);

        // --- HEADER ---
        Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(30, 30, 35) };
        Label lblHeader = new Label
        {
            Text = "FLASHER EXTERNAL CONTROLLER",
            Dock = DockStyle.Fill,
            Font = new Font("Consolas", 14, FontStyle.Bold),
            ForeColor = Color.FromArgb(51, 204, 255),
            TextAlign = ContentAlignment.MiddleCenter
        };
        pnlHeader.Controls.Add(lblHeader);
        this.Controls.Add(pnlHeader);

        // --- MAIN CONTAINER ---
        FlowLayoutPanel mainPanel = new FlowLayoutPanel
        {
            Location = new Point(10, 70),
            Size = new Size(445, 500),
            FlowDirection = FlowDirection.TopDown
        };

        // --- GROUP: FEATURES ---
        GroupBox grpFeatures = CreateGroupBox("Módulos de Combate", 160);

        chkRcs = CreateCheckBox("Recoil Control (RCS)", 30, grpFeatures);
        chkRcs.CheckedChanged += (s, e) => { RcsEnabled = chkRcs.Checked; UpdateStatus(); };
        lblRcsStatus = CreateStatusLabel("F1", 32, grpFeatures);

        chkNoFlash = CreateCheckBox("Anti-Flashbang", 60, grpFeatures);
        chkNoFlash.CheckedChanged += (s, e) => { NoFlashEnabled = chkNoFlash.Checked; UpdateStatus(); };
        lblFlashStatus = CreateStatusLabel("F2", 62, grpFeatures);

        chkTrigger = CreateCheckBox("Triggerbot (Alt)", 90, grpFeatures);
        chkTrigger.CheckedChanged += (s, e) => { TriggerEnabled = chkTrigger.Checked; UpdateStatus(); };
        lblTriggerStatus = CreateStatusLabel("F3", 92, grpFeatures);

        chkBhop = CreateCheckBox("Bunny Hop", 120, grpFeatures);
        chkBhop.CheckedChanged += (s, e) => { BhopEnabled = chkBhop.Checked; UpdateStatus(); };
        lblBhopStatus = CreateStatusLabel("Ativo", 122, grpFeatures);

        mainPanel.Controls.Add(grpFeatures);

        // --- GROUP: TRIGGER SETTINGS ---
        GroupBox grpTrigger = CreateGroupBox("Ajustes de Disparo", 240);

        AddSlider(grpTrigger, "Delay de Reação:", ref sliderPrecision, 0, 100, 25, ref lblPrecisionValue, "ms", 30);
        sliderPrecision.ValueChanged += (s, e) => TriggerPrecision = sliderPrecision.Value;

        AddSlider(grpTrigger, "Duração do Clique:", ref sliderHoldTime, 5, 50, 10, ref lblHoldValue, "ms", 95);
        sliderHoldTime.ValueChanged += (s, e) => TriggerHoldTime = sliderHoldTime.Value;

        AddSlider(grpTrigger, "Cadência de Tiro:", ref sliderCadence, 0, 200, 20, ref lblCadenceValue, "ms", 160);
        sliderCadence.ValueChanged += (s, e) => TriggerCadence = sliderCadence.Value;

        mainPanel.Controls.Add(grpTrigger);

        // --- PRESETS ---
        Panel pnlPresets = new Panel { Size = new Size(440, 50) };
        Button btnPistol = CreateButton("Pistola", 0, pnlPresets, Color.FromArgb(40, 40, 45));
        btnPistol.Click += (s, e) => ApplyPreset(15, 8, 10);

        Button btnRifle = CreateButton("Rifle", 145, pnlPresets, Color.FromArgb(40, 40, 45));
        btnRifle.Click += (s, e) => ApplyPreset(35, 12, 30);

        Button btnAwp = CreateButton("AWP", 290, pnlPresets, Color.FromArgb(40, 40, 45));
        btnAwp.Click += (s, e) => ApplyPreset(50, 15, 100);

        mainPanel.Controls.Add(pnlPresets);

        this.Controls.Add(mainPanel);
        UpdateStatus();
    }

    // --- MÉTODOS AUXILIARES ---
    private GroupBox CreateGroupBox(string title, int height)
    {
        return new GroupBox
        {
            Text = title,
            Size = new Size(440, height),
            ForeColor = Color.FromArgb(51, 204, 255),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Margin = new Padding(0, 10, 0, 10)
        };
    }

    private CheckBox CreateCheckBox(string text, int y, Control parent)
    {
        CheckBox cb = new CheckBox
        {
            Text = text,
            Location = new Point(15, y),
            Size = new Size(220, 25),
            ForeColor = Color.White
        };
        parent.Controls.Add(cb);
        return cb;
    }

    private Label CreateStatusLabel(string text, int y, Control parent)
    {
        Label lbl = new Label
        {
            Text = text,
            Location = new Point(280, y),
            Size = new Size(140, 20),
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.Gray
        };
        parent.Controls.Add(lbl);
        return lbl;
    }

    private void AddSlider(Control parent, string text, ref TrackBar tb, int min, int max, int val, ref Label valLbl, string unit, int y)
    {
        Label title = new Label { Text = text, Location = new Point(15, y), Size = new Size(200, 20), ForeColor = Color.LightGray };
        valLbl = new Label { Text = val + unit, Location = new Point(350, y), Size = new Size(70, 20), TextAlign = ContentAlignment.MiddleRight, ForeColor = Color.FromArgb(51, 204, 255) };

        tb = new TrackBar { Location = new Point(10, y + 20), Size = new Size(410, 45), Minimum = min, Maximum = max, Value = val, TickStyle = TickStyle.None };
        TrackBar localTb = tb;
        Label localLbl = valLbl;
        tb.ValueChanged += (s, e) => localLbl.Text = localTb.Value + unit;

        parent.Controls.Add(title);
        parent.Controls.Add(valLbl);
        parent.Controls.Add(tb);
    }

    private Button CreateButton(string text, int x, Control parent, Color backColor)
    {
        Button btn = new Button { Text = text, Location = new Point(x, 5), Size = new Size(125, 35), BackColor = backColor, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
        btn.FlatAppearance.BorderSize = 0;
        parent.Controls.Add(btn);
        return btn;
    }

    private void ApplyPreset(int precision, int hold, int cadence)
    {
        sliderPrecision.Value = precision;
        sliderHoldTime.Value = hold;
        sliderCadence.Value = cadence;
    }

    private void UpdateStatus()
    {
        UpdateLabel(lblRcsStatus, RcsEnabled, "F1");
        UpdateLabel(lblFlashStatus, NoFlashEnabled, "F2");
        UpdateLabel(lblTriggerStatus, TriggerEnabled, "F3");
        UpdateLabel(lblBhopStatus, BhopEnabled, "SPACE");
    }

    private void UpdateLabel(Label lbl, bool enabled, string hotkey)
    {
        lbl.Text = enabled ? $"● ON ({hotkey})" : $"○ OFF ({hotkey})";
        lbl.ForeColor = enabled ? Color.FromArgb(50, 255, 100) : Color.Gray;
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.F1) chkRcs.Checked = !chkRcs.Checked;
        if (keyData == Keys.F2) chkNoFlash.Checked = !chkNoFlash.Checked;
        if (keyData == Keys.F3) chkTrigger.Checked = !chkTrigger.Checked;
        return base.ProcessCmdKey(ref msg, keyData);
    }
}