namespace DatasetExpertSystem
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonReadDataset = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.comboBoxVariable = new System.Windows.Forms.ComboBox();
            this.labelAnswer = new System.Windows.Forms.Label();
            this.labelColumn = new System.Windows.Forms.Label();
            this.buttonAnswer = new System.Windows.Forms.Button();
            this.dataGridViewChance = new System.Windows.Forms.DataGridView();
            this.ColumnClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnChance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelMaxChance = new System.Windows.Forms.Label();
            this.labelMaxChanceAnswer = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewChance)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonReadDataset
            // 
            this.buttonReadDataset.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonReadDataset.Location = new System.Drawing.Point(15, 12);
            this.buttonReadDataset.Name = "buttonReadDataset";
            this.buttonReadDataset.Size = new System.Drawing.Size(92, 28);
            this.buttonReadDataset.TabIndex = 1;
            this.buttonReadDataset.Text = "Открыть";
            this.buttonReadDataset.UseVisualStyleBackColor = true;
            this.buttonReadDataset.Click += new System.EventHandler(this.buttonReadDataset_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "\"Файлы csv|*.csv|Все файлы|*.csv*\"";
            // 
            // comboBoxVariable
            // 
            this.comboBoxVariable.Enabled = false;
            this.comboBoxVariable.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxVariable.FormattingEnabled = true;
            this.comboBoxVariable.Location = new System.Drawing.Point(15, 123);
            this.comboBoxVariable.Name = "comboBoxVariable";
            this.comboBoxVariable.Size = new System.Drawing.Size(121, 24);
            this.comboBoxVariable.TabIndex = 2;
            // 
            // labelAnswer
            // 
            this.labelAnswer.AutoSize = true;
            this.labelAnswer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelAnswer.Location = new System.Drawing.Point(12, 60);
            this.labelAnswer.Name = "labelAnswer";
            this.labelAnswer.Size = new System.Drawing.Size(206, 17);
            this.labelAnswer.TabIndex = 3;
            this.labelAnswer.Text = "Выберите значение фактора:";
            // 
            // labelColumn
            // 
            this.labelColumn.AutoSize = true;
            this.labelColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelColumn.Location = new System.Drawing.Point(12, 88);
            this.labelColumn.Name = "labelColumn";
            this.labelColumn.Size = new System.Drawing.Size(23, 17);
            this.labelColumn.TabIndex = 4;
            this.labelColumn.Text = "---";
            // 
            // buttonAnswer
            // 
            this.buttonAnswer.Enabled = false;
            this.buttonAnswer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAnswer.Location = new System.Drawing.Point(142, 119);
            this.buttonAnswer.Name = "buttonAnswer";
            this.buttonAnswer.Size = new System.Drawing.Size(97, 28);
            this.buttonAnswer.TabIndex = 5;
            this.buttonAnswer.Text = "Применить";
            this.buttonAnswer.UseVisualStyleBackColor = true;
            this.buttonAnswer.Click += new System.EventHandler(this.buttonAnswer_Click);
            // 
            // dataGridViewChance
            // 
            this.dataGridViewChance.AllowUserToAddRows = false;
            this.dataGridViewChance.AllowUserToDeleteRows = false;
            this.dataGridViewChance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewChance.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnClass,
            this.ColumnChance});
            this.dataGridViewChance.Dock = System.Windows.Forms.DockStyle.Right;
            this.dataGridViewChance.Location = new System.Drawing.Point(280, 0);
            this.dataGridViewChance.Name = "dataGridViewChance";
            this.dataGridViewChance.ReadOnly = true;
            this.dataGridViewChance.Size = new System.Drawing.Size(223, 203);
            this.dataGridViewChance.TabIndex = 6;
            // 
            // ColumnClass
            // 
            this.ColumnClass.HeaderText = "Классы";
            this.ColumnClass.Name = "ColumnClass";
            this.ColumnClass.ReadOnly = true;
            // 
            // ColumnChance
            // 
            this.ColumnChance.HeaderText = "Вероятности";
            this.ColumnChance.Name = "ColumnChance";
            this.ColumnChance.ReadOnly = true;
            this.ColumnChance.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnChance.Width = 80;
            // 
            // labelMaxChance
            // 
            this.labelMaxChance.AutoSize = true;
            this.labelMaxChance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelMaxChance.Location = new System.Drawing.Point(12, 160);
            this.labelMaxChance.Name = "labelMaxChance";
            this.labelMaxChance.Size = new System.Drawing.Size(195, 17);
            this.labelMaxChance.TabIndex = 7;
            this.labelMaxChance.Text = "Наиболее вероятный класс:";
            // 
            // labelMaxChanceAnswer
            // 
            this.labelMaxChanceAnswer.AutoSize = true;
            this.labelMaxChanceAnswer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelMaxChanceAnswer.Location = new System.Drawing.Point(12, 177);
            this.labelMaxChanceAnswer.Name = "labelMaxChanceAnswer";
            this.labelMaxChanceAnswer.Size = new System.Drawing.Size(0, 17);
            this.labelMaxChanceAnswer.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 203);
            this.Controls.Add(this.labelMaxChanceAnswer);
            this.Controls.Add(this.labelMaxChance);
            this.Controls.Add(this.dataGridViewChance);
            this.Controls.Add(this.buttonAnswer);
            this.Controls.Add(this.labelColumn);
            this.Controls.Add(this.labelAnswer);
            this.Controls.Add(this.comboBoxVariable);
            this.Controls.Add(this.buttonReadDataset);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Экспертная система";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewChance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonReadDataset;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox comboBoxVariable;
        private System.Windows.Forms.Label labelAnswer;
        private System.Windows.Forms.Label labelColumn;
        private System.Windows.Forms.Button buttonAnswer;
        private System.Windows.Forms.DataGridView dataGridViewChance;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnChance;
        private System.Windows.Forms.Label labelMaxChance;
        private System.Windows.Forms.Label labelMaxChanceAnswer;
    }
}

