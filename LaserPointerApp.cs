using System;
using System.Drawing.Drawing2D;

namespace LaserPointer;

public class LaserPointerApp : Form
{
    private System.Windows.Forms.Timer timer;
    private bool isCtrlPressed = false;
    private LowLevelKeyboardHook keyboardHook;

    public LaserPointerApp()
    {
        this.FormBorderStyle = FormBorderStyle.None;
        this.ShowInTaskbar = false;
        this.TopMost = true;
        this.BackColor = Color.Magenta;
        this.TransparencyKey = Color.Magenta;
        this.Size = Screen.PrimaryScreen.Bounds.Size;
        this.Location = new Point(0, 0);

        // Wichtig für bessere Transparenz
        this.Opacity = 0.8;
        this.DoubleBuffered = true;
        this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        this.SetStyle(ControlStyles.UserPaint, true);

        timer = new System.Windows.Forms.Timer { Interval = 10 };
        timer.Tick += Timer_Tick;
        timer.Start();

        keyboardHook = new LowLevelKeyboardHook();
        keyboardHook.OnKeyDown += KeyboardHook_OnKeyDown;
        keyboardHook.OnKeyUp += KeyboardHook_OnKeyUp;
        keyboardHook.Install();

        var tray = new NotifyIcon
        {
            Icon = SystemIcons.Shield,
            Visible = true,
            Text = "Strg = Laser Pointer"
        };

        tray.ContextMenuStrip = new ContextMenuStrip();
        tray.ContextMenuStrip.Items.Add("Beenden", null, (s, e) => Application.Exit());
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        if (isCtrlPressed)
            this.Invalidate(false);
        else if (this.Visible)
            this.Hide();
    }

    private void KeyboardHook_OnKeyDown(object sender, LowLevelKeyboardHook.KeyEventArgs e)
    {
        if (e.KeyCode == Keys.RControlKey && !isCtrlPressed)   // Nur rechte Strg
        {
            isCtrlPressed = true;
            this.Show();
        }
    }

    private void KeyboardHook_OnKeyUp(object sender, LowLevelKeyboardHook.KeyEventArgs e)
    {
        if (e.KeyCode == Keys.RControlKey && isCtrlPressed)
        {
            isCtrlPressed = false;
            this.Hide();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (!isCtrlPressed) return;

        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.CompositingQuality = CompositingQuality.HighQuality;
        g.CompositingMode = CompositingMode.SourceOver;   // Wichtig für Transparenz

        Point p = Cursor.Position;
        int size = 26;
        
        // Stark transparenter gefüllter Kreis
        using SolidBrush brush = new SolidBrush(Color.FromArgb(75, 255, 70, 70));  // sehr transparent
        g.FillEllipse(brush, p.X - size / 2, p.Y - size / 2, size, size);

        // Leichter Glow außen
        //using SolidBrush glow = new SolidBrush(Color.FromArgb(35, 255, 80, 80));
        //g.FillEllipse(glow, p.X - size / 2 - 6, p.Y - size / 2 - 6, size + 12, size + 12);

        // Kleiner heller Mittelpunkt
        //using SolidBrush center = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
        //g.FillEllipse(center, p.X - 4, p.Y - 4, 8, 8);
    }

    protected override void Dispose(bool disposing)
    {
        timer?.Stop();
        keyboardHook?.Uninstall();
        base.Dispose(disposing);
    }
}