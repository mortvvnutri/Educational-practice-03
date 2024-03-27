namespace YP3
{
    partial class ClientRegistrationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtClientFullName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.masked_number = new System.Windows.Forms.MaskedTextBox();
            this.button_Registration = new System.Windows.Forms.Button();
            this.button_exit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtClientFullName
            // 
            this.txtClientFullName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtClientFullName.Location = new System.Drawing.Point(312, 134);
            this.txtClientFullName.Name = "txtClientFullName";
            this.txtClientFullName.Size = new System.Drawing.Size(249, 29);
            this.txtClientFullName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(106, 139);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Ваше ФИО";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(50, 220);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(243, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "Введите номер телефона";
            // 
            // masked_number
            // 
            this.masked_number.BackColor = System.Drawing.Color.White;
            this.masked_number.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.masked_number.Location = new System.Drawing.Point(312, 215);
            this.masked_number.Mask = "+7(000) 000-00-00";
            this.masked_number.Name = "masked_number";
            this.masked_number.RejectInputOnFirstFailure = true;
            this.masked_number.Size = new System.Drawing.Size(249, 29);
            this.masked_number.TabIndex = 17;
            // 
            // button_Registration
            // 
            this.button_Registration.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_Registration.Location = new System.Drawing.Point(88, 321);
            this.button_Registration.Name = "button_Registration";
            this.button_Registration.Size = new System.Drawing.Size(220, 67);
            this.button_Registration.TabIndex = 18;
            this.button_Registration.Text = "Зарегистрироваться";
            this.button_Registration.UseVisualStyleBackColor = true;
            this.button_Registration.Click += new System.EventHandler(this.button_Regisration_Click);
            // 
            // button_exit
            // 
            this.button_exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_exit.Location = new System.Drawing.Point(397, 326);
            this.button_exit.Name = "button_exit";
            this.button_exit.Size = new System.Drawing.Size(131, 56);
            this.button_exit.TabIndex = 19;
            this.button_exit.Text = "Выход";
            this.button_exit.UseVisualStyleBackColor = true;
            this.button_exit.Click += new System.EventHandler(this.button_exit_Click);
            // 
            // ClientRegistrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 461);
            this.Controls.Add(this.button_exit);
            this.Controls.Add(this.button_Registration);
            this.Controls.Add(this.masked_number);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtClientFullName);
            this.MinimumSize = new System.Drawing.Size(650, 500);
            this.Name = "ClientRegistrationForm";
            this.Text = "ClientRegistrationForm";
            this.Load += new System.EventHandler(this.ClientRegistrationForm_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ClientRegistrationForm_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtClientFullName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox masked_number;
        private System.Windows.Forms.Button button_Registration;
        private System.Windows.Forms.Button button_exit;
    }
}