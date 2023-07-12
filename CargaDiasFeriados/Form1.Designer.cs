
namespace CargaDiasFeriados
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbAmbos = new System.Windows.Forms.RadioButton();
            this.rbEUA = new System.Windows.Forms.RadioButton();
            this.rbMexico = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbAño = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbMes = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpMesFestivo = new System.Windows.Forms.MonthCalendar();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.dtGrdVwFeriados = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGrdVwFeriados)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbAmbos);
            this.groupBox1.Controls.Add(this.rbEUA);
            this.groupBox1.Controls.Add(this.rbMexico);
            this.groupBox1.Location = new System.Drawing.Point(16, 134);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(993, 82);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Seleccione el pais";
            // 
            // rbAmbos
            // 
            this.rbAmbos.AutoSize = true;
            this.rbAmbos.Location = new System.Drawing.Point(596, 37);
            this.rbAmbos.Name = "rbAmbos";
            this.rbAmbos.Size = new System.Drawing.Size(84, 24);
            this.rbAmbos.TabIndex = 0;
            this.rbAmbos.TabStop = true;
            this.rbAmbos.Text = "Ambos";
            this.rbAmbos.UseVisualStyleBackColor = true;
            // 
            // rbEUA
            // 
            this.rbEUA.AutoSize = true;
            this.rbEUA.Location = new System.Drawing.Point(319, 37);
            this.rbEUA.Name = "rbEUA";
            this.rbEUA.Size = new System.Drawing.Size(147, 24);
            this.rbEUA.TabIndex = 0;
            this.rbEUA.TabStop = true;
            this.rbEUA.Text = "Estados Unidos";
            this.rbEUA.UseVisualStyleBackColor = true;
            // 
            // rbMexico
            // 
            this.rbMexico.AutoSize = true;
            this.rbMexico.Location = new System.Drawing.Point(42, 37);
            this.rbMexico.Name = "rbMexico";
            this.rbMexico.Size = new System.Drawing.Size(83, 24);
            this.rbMexico.TabIndex = 0;
            this.rbMexico.TabStop = true;
            this.rbMexico.Text = "Mexico";
            this.rbMexico.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbAño);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cmbMes);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(16, 25);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(993, 85);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Selecciones el Mes y Año a visualizar";
            // 
            // cmbAño
            // 
            this.cmbAño.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAño.FormattingEnabled = true;
            this.cmbAño.Location = new System.Drawing.Point(527, 36);
            this.cmbAño.Name = "cmbAño";
            this.cmbAño.Size = new System.Drawing.Size(224, 28);
            this.cmbAño.TabIndex = 1;
            this.cmbAño.SelectedIndexChanged += new System.EventHandler(this.cmbAño_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(459, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Año:";
            // 
            // cmbMes
            // 
            this.cmbMes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMes.FormattingEnabled = true;
            this.cmbMes.Location = new System.Drawing.Point(92, 36);
            this.cmbMes.Name = "cmbMes";
            this.cmbMes.Size = new System.Drawing.Size(224, 28);
            this.cmbMes.TabIndex = 1;
            this.cmbMes.SelectedIndexChanged += new System.EventHandler(this.cmbMes_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mes:";
            // 
            // dtpMesFestivo
            // 
            this.dtpMesFestivo.Location = new System.Drawing.Point(108, 228);
            this.dtpMesFestivo.Name = "dtpMesFestivo";
            this.dtpMesFestivo.TabIndex = 2;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(714, 510);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(140, 44);
            this.btnCancelar.TabIndex = 3;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(869, 510);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(140, 44);
            this.btnSalir.TabIndex = 3;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(559, 510);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(140, 44);
            this.btnGuardar.TabIndex = 3;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // dtGrdVwFeriados
            // 
            this.dtGrdVwFeriados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGrdVwFeriados.Location = new System.Drawing.Point(521, 228);
            this.dtGrdVwFeriados.Name = "dtGrdVwFeriados";
            this.dtGrdVwFeriados.ReadOnly = true;
            this.dtGrdVwFeriados.RowHeadersWidth = 62;
            this.dtGrdVwFeriados.RowTemplate.Height = 28;
            this.dtGrdVwFeriados.Size = new System.Drawing.Size(414, 253);
            this.dtGrdVwFeriados.TabIndex = 4;
            this.dtGrdVwFeriados.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtGrdVwFeriados_CellDoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 566);
            this.Controls.Add(this.dtGrdVwFeriados);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.dtpMesFestivo);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Carga Dias Feriados";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGrdVwFeriados)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbAmbos;
        private System.Windows.Forms.RadioButton rbEUA;
        private System.Windows.Forms.RadioButton rbMexico;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmbAño;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbMes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MonthCalendar dtpMesFestivo;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.DataGridView dtGrdVwFeriados;
    }
}

