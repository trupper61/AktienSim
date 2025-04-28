using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
namespace aktiensim
{
    public partial class Form1 : Form
    {
        public Panel loginPanel;
        public Form1()
        {
            InitializeComponent();
        }
        public void InitUi()
        {
            loginPanel = new Panel
            {
                Size = new Size(this.ClientSize.Width, this.ClientSize.Height),
                Location = new Point(0, 0),
                BackColor = Color.LightBlue
            };
            TextBox box = new TextBox
            {
                Size = new Size(100, 30),
                Location = new Point((loginPanel.Width - 80)/2, (loginPanel.Height -80)/2),
                Text = "Email..."
            };
            loginPanel.Controls.Add(box);
            Controls.Add(loginPanel);
        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            string email = emailInput.Text;
            string vName = vornameInput.Text;
            string nName = nachnameInput.Text;
            string password = passwortInput.Text;
            string passwdCheck = passwortCheck.Text;
            if (email == "" || vName == "" || nName == "" || password == "" || passwdCheck == "")
            {
                MessageBox.Show("Alle Felder wurden nicht ausgefüllt");
                return;
            }
            if (password != passwdCheck)
            {
                MessageBox.Show("Passwörter stimmen nicht über ein!");
                return;
            }
            string passHash = Hash(password);
            MessageBox.Show(passHash);
        }
        //Credits: https://stackoverflow.com/questions/17292366/hashing-with-sha1-algorithm-in-c-sharp
        static string Hash(string input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }
    }
}
