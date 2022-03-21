using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogsKiller
{
    public partial class Form1 : Form
    {
        int mov;
        int movX;
        int movY;

        public Form1()
        {
            InitializeComponent();
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                ProcessStartInfo info = new ProcessStartInfo(Process.GetCurrentProcess().ProcessName.ToString() + ".exe");
                info.UseShellExecute = true;
                info.Verb = "runas";
                Process.Start(info);
                Process process = Process.GetCurrentProcess();
                process.Kill();
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mov == 1)
            {
                this.SetDesktopLocation(MousePosition.X - movX, MousePosition.Y - movY);
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movX = e.X;
            movY = e.Y;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process process = Process.GetCurrentProcess();
            process.Kill();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("All Windows Logs Files Will Be Deleted Are You Agree?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                foreach (EventLog file in EventLog.GetEventLogs())
                {
                    try
                    {
                        EventLog.Delete(file.Log);
                        textBox1.Text += "Deleted: " + file.Log + Environment.NewLine;
                    }
                    catch (Exception ee)
                    {
                        textBox1.Text += ee.Message + ": " + file.Log + Environment.NewLine;
                    }
                }
            }
        }
    }
}
