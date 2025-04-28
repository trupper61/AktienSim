namespace aktiensim
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.emailInput = new System.Windows.Forms.TextBox();
            this.vornameInput = new System.Windows.Forms.TextBox();
            this.nachnameInput = new System.Windows.Forms.TextBox();
            this.passwortInput = new System.Windows.Forms.TextBox();
            this.passwortCheck = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(292, 288);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(208, 32);
            this.button1.TabIndex = 2;
            this.button1.Text = "Register";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.RegisterBtn_Click);
            // 
            // emailInput
            // 
            this.emailInput.Location = new System.Drawing.Point(292, 96);
            this.emailInput.Name = "emailInput";
            this.emailInput.Size = new System.Drawing.Size(208, 22);
            this.emailInput.TabIndex = 4;
            // 
            // passwortInput
            // 
            this.passwortInput.Location = new System.Drawing.Point(292, 180);
            this.passwortInput.Name = "passwortInput";
            this.passwortInput.Size = new System.Drawing.Size(208, 22);
            this.passwortInput.TabIndex = 5;
            // 
            // nachnameInput
            // 
            this.nachnameInput.Location = new System.Drawing.Point(292, 152);
            this.nachnameInput.Name = "nachnameInput";
            this.nachnameInput.Size = new System.Drawing.Size(208, 22);
            this.nachnameInput.TabIndex = 6;
            // 
            // vornameInput
            // 
            this.vornameInput.Location = new System.Drawing.Point(292, 124);
            this.vornameInput.Name = "vornameInput";
            this.vornameInput.Size = new System.Drawing.Size(208, 22);
            this.vornameInput.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(196, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Email:";
            // 
            // passwortCheck
            // 
            this.passwortCheck.Location = new System.Drawing.Point(292, 208);
            this.passwortCheck.Name = "passwortCheck";
            this.passwortCheck.Size = new System.Drawing.Size(208, 22);
            this.passwortCheck.TabIndex = 9;
            this.passwortCheck.Text = "Passwort erneut eingeben...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(196, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "Vorname:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(196, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "Nachname:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(196, 186);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 16);
            this.label4.TabIndex = 12;
            this.label4.Text = "Passwort:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.passwortCheck);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.vornameInput);
            this.Controls.Add(this.nachnameInput);
            this.Controls.Add(this.passwortInput);
            this.Controls.Add(this.emailInput);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox emailInput;
        private System.Windows.Forms.TextBox vornameInput;
        private System.Windows.Forms.TextBox nachnameInput;
        private System.Windows.Forms.TextBox passwortInput;
        private System.Windows.Forms.TextBox passwortCheck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

