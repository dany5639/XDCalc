namespace XDCalc
{
    partial class XDCalc
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBox_dec = new System.Windows.Forms.RichTextBox();
            this.richTextBox_hex = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox_dec
            // 
            this.richTextBox_dec.Location = new System.Drawing.Point(6, 19);
            this.richTextBox_dec.Name = "richTextBox_dec";
            this.richTextBox_dec.Size = new System.Drawing.Size(306, 140);
            this.richTextBox_dec.TabIndex = 0;
            this.richTextBox_dec.Text = "";
            // 
            // richTextBox_hex
            // 
            this.richTextBox_hex.Location = new System.Drawing.Point(6, 19);
            this.richTextBox_hex.Name = "richTextBox_hex";
            this.richTextBox_hex.Size = new System.Drawing.Size(306, 140);
            this.richTextBox_hex.TabIndex = 4;
            this.richTextBox_hex.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBox_dec);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(320, 165);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Decimal";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.richTextBox_hex);
            this.groupBox2.Location = new System.Drawing.Point(12, 183);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(320, 165);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Hexadecimal";
            // 
            // XDCalc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 359);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "XDCalc";
            this.Text = "XDCalc";
            this.Load += new System.EventHandler(this.XDCalc_load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_dec;
        private System.Windows.Forms.RichTextBox richTextBox_hex;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

