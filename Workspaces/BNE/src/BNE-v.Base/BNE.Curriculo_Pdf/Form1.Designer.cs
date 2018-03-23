namespace BNE.Curriculo_Pdf
{
    partial class Form1
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
            this.IdfOrigem = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtInicio = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.DtFim = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.lblOrigem = new System.Windows.Forms.Label();
            this.pbCurriculos = new System.Windows.Forms.ProgressBar();
            this.lblFinal = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txbCidade = new System.Windows.Forms.TextBox();
            this.lblCidade = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // IdfOrigem
            // 
            this.IdfOrigem.Location = new System.Drawing.Point(79, 16);
            this.IdfOrigem.Name = "IdfOrigem";
            this.IdfOrigem.Size = new System.Drawing.Size(128, 20);
            this.IdfOrigem.TabIndex = 0;
            this.IdfOrigem.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            this.IdfOrigem.Leave += new System.EventHandler(this.IdfOrigem_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "IDF Origem";
            // 
            // dtInicio
            // 
            this.dtInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtInicio.Location = new System.Drawing.Point(79, 67);
            this.dtInicio.Name = "dtInicio";
            this.dtInicio.Size = new System.Drawing.Size(104, 20);
            this.dtInicio.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Data Início";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(213, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Data Fim";
            // 
            // DtFim
            // 
            this.DtFim.CausesValidation = false;
            this.DtFim.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtFim.Location = new System.Drawing.Point(278, 67);
            this.DtFim.Name = "DtFim";
            this.DtFim.Size = new System.Drawing.Size(104, 20);
            this.DtFim.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(425, 73);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Processar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblOrigem
            // 
            this.lblOrigem.AutoSize = true;
            this.lblOrigem.Location = new System.Drawing.Point(216, 22);
            this.lblOrigem.Name = "lblOrigem";
            this.lblOrigem.Size = new System.Drawing.Size(0, 13);
            this.lblOrigem.TabIndex = 7;
            // 
            // pbCurriculos
            // 
            this.pbCurriculos.Location = new System.Drawing.Point(16, 93);
            this.pbCurriculos.Name = "pbCurriculos";
            this.pbCurriculos.Size = new System.Drawing.Size(366, 23);
            this.pbCurriculos.Step = 1;
            this.pbCurriculos.TabIndex = 8;
            // 
            // lblFinal
            // 
            this.lblFinal.AutoSize = true;
            this.lblFinal.Location = new System.Drawing.Point(16, 102);
            this.lblFinal.Name = "lblFinal";
            this.lblFinal.Size = new System.Drawing.Size(0, 13);
            this.lblFinal.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Cidade";
            // 
            // txbCidade
            // 
            this.txbCidade.Location = new System.Drawing.Point(79, 41);
            this.txbCidade.Name = "txbCidade";
            this.txbCidade.Size = new System.Drawing.Size(128, 20);
            this.txbCidade.TabIndex = 11;
            this.txbCidade.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txbCidade_KeyPress);
            this.txbCidade.Leave += new System.EventHandler(this.txbCidade_Leave);
            // 
            // lblCidade
            // 
            this.lblCidade.AutoSize = true;
            this.lblCidade.Location = new System.Drawing.Point(219, 43);
            this.lblCidade.Name = "lblCidade";
            this.lblCidade.Size = new System.Drawing.Size(0, 13);
            this.lblCidade.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 126);
            this.Controls.Add(this.lblCidade);
            this.Controls.Add(this.txbCidade);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblFinal);
            this.Controls.Add(this.pbCurriculos);
            this.Controls.Add(this.lblOrigem);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DtFim);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtInicio);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.IdfOrigem);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox IdfOrigem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtInicio;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker DtFim;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblOrigem;
        private System.Windows.Forms.ProgressBar pbCurriculos;
        private System.Windows.Forms.Label lblFinal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txbCidade;
        private System.Windows.Forms.Label lblCidade;
    }
}

