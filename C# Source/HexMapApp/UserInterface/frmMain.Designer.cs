namespace UserInterface
{
    partial class frmMain
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.imgBoard = new System.Windows.Forms.PictureBox();
            this.chkGridVisible = new System.Windows.Forms.CheckBox();
            this.groupHexLabel = new System.Windows.Forms.GroupBox();
            this.cmbHexCoordinateSystem = new System.Windows.Forms.ComboBox();
            this.btnHexLabelFont = new System.Windows.Forms.Button();
            this.lblVPos = new System.Windows.Forms.Label();
            this.barVPos = new System.Windows.Forms.TrackBar();
            this.lblHPos = new System.Windows.Forms.Label();
            this.barHPos = new System.Windows.Forms.TrackBar();
            this.chkHexLabelEnabled = new System.Windows.Forms.CheckBox();
            this.groupCoordinateSystem = new System.Windows.Forms.GroupBox();
            this.btnOffsetEven = new System.Windows.Forms.RadioButton();
            this.btnOffsetOdd = new System.Windows.Forms.RadioButton();
            this.groupTopped = new System.Windows.Forms.GroupBox();
            this.btnPointyTopped = new System.Windows.Forms.RadioButton();
            this.btnFlatTopped = new System.Windows.Forms.RadioButton();
            this.lblSideLength = new System.Windows.Forms.Label();
            this.numHexLength = new System.Windows.Forms.NumericUpDown();
            this.lblMapHeight = new System.Windows.Forms.Label();
            this.numMapHeight = new System.Windows.Forms.NumericUpDown();
            this.lblMapWidth = new System.Windows.Forms.Label();
            this.numMapWidth = new System.Windows.Forms.NumericUpDown();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgBoard)).BeginInit();
            this.groupHexLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barVPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barHPos)).BeginInit();
            this.groupCoordinateSystem.SuspendLayout();
            this.groupTopped.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHexLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMapHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMapWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.imgBoard);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.chkGridVisible);
            this.splitContainer.Panel2.Controls.Add(this.groupHexLabel);
            this.splitContainer.Panel2.Controls.Add(this.groupCoordinateSystem);
            this.splitContainer.Panel2.Controls.Add(this.groupTopped);
            this.splitContainer.Panel2.Controls.Add(this.lblSideLength);
            this.splitContainer.Panel2.Controls.Add(this.numHexLength);
            this.splitContainer.Panel2.Controls.Add(this.lblMapHeight);
            this.splitContainer.Panel2.Controls.Add(this.numMapHeight);
            this.splitContainer.Panel2.Controls.Add(this.lblMapWidth);
            this.splitContainer.Panel2.Controls.Add(this.numMapWidth);
            this.splitContainer.Size = new System.Drawing.Size(896, 487);
            this.splitContainer.SplitterDistance = 737;
            this.splitContainer.TabIndex = 9;
            // 
            // imgBoard
            // 
            this.imgBoard.BackColor = System.Drawing.SystemColors.Control;
            this.imgBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgBoard.Location = new System.Drawing.Point(0, 0);
            this.imgBoard.Name = "imgBoard";
            this.imgBoard.Size = new System.Drawing.Size(735, 485);
            this.imgBoard.TabIndex = 0;
            this.imgBoard.TabStop = false;
            this.imgBoard.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imgBoard_MouseMove);
            this.imgBoard.MouseClick += new System.Windows.Forms.MouseEventHandler(this.imgBoard_MouseClick);
            this.imgBoard.SizeChanged += new System.EventHandler(this.imgBoard_SizeChanged);
            // 
            // chkGridVisible
            // 
            this.chkGridVisible.AutoSize = true;
            this.chkGridVisible.Checked = true;
            this.chkGridVisible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGridVisible.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chkGridVisible.Location = new System.Drawing.Point(7, 129);
            this.chkGridVisible.Name = "chkGridVisible";
            this.chkGridVisible.Size = new System.Drawing.Size(104, 17);
            this.chkGridVisible.TabIndex = 9;
            this.chkGridVisible.Text = "показать сетку";
            this.chkGridVisible.UseVisualStyleBackColor = true;
            // 
            // groupHexLabel
            // 
            this.groupHexLabel.Controls.Add(this.cmbHexCoordinateSystem);
            this.groupHexLabel.Controls.Add(this.btnHexLabelFont);
            this.groupHexLabel.Controls.Add(this.lblVPos);
            this.groupHexLabel.Controls.Add(this.barVPos);
            this.groupHexLabel.Controls.Add(this.lblHPos);
            this.groupHexLabel.Controls.Add(this.barHPos);
            this.groupHexLabel.Controls.Add(this.chkHexLabelEnabled);
            this.groupHexLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupHexLabel.Location = new System.Drawing.Point(7, 280);
            this.groupHexLabel.Name = "groupHexLabel";
            this.groupHexLabel.Size = new System.Drawing.Size(142, 179);
            this.groupHexLabel.TabIndex = 8;
            this.groupHexLabel.TabStop = false;
            this.groupHexLabel.Text = "Подпись гекса";
            // 
            // cmbHexCoordinateSystem
            // 
            this.cmbHexCoordinateSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHexCoordinateSystem.FormattingEnabled = true;
            this.cmbHexCoordinateSystem.Items.AddRange(new object[] {
            "Offset coordinates",
            "Cube coordinates",
            "Axial coordinates"});
            this.cmbHexCoordinateSystem.Location = new System.Drawing.Point(10, 147);
            this.cmbHexCoordinateSystem.Name = "cmbHexCoordinateSystem";
            this.cmbHexCoordinateSystem.Size = new System.Drawing.Size(121, 21);
            this.cmbHexCoordinateSystem.TabIndex = 8;
            // 
            // btnHexLabelFont
            // 
            this.btnHexLabelFont.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnHexLabelFont.Location = new System.Drawing.Point(9, 118);
            this.btnHexLabelFont.Name = "btnHexLabelFont";
            this.btnHexLabelFont.Size = new System.Drawing.Size(75, 23);
            this.btnHexLabelFont.TabIndex = 7;
            this.btnHexLabelFont.Text = "Шрифт...";
            this.btnHexLabelFont.UseVisualStyleBackColor = true;
            this.btnHexLabelFont.Click += new System.EventHandler(this.btnHexLabelFont_Click);
            // 
            // lblVPos
            // 
            this.lblVPos.AutoSize = true;
            this.lblVPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblVPos.Location = new System.Drawing.Point(7, 75);
            this.lblVPos.Name = "lblVPos";
            this.lblVPos.Size = new System.Drawing.Size(32, 13);
            this.lblVPos.TabIndex = 6;
            this.lblVPos.Text = "VPos";
            // 
            // barVPos
            // 
            this.barVPos.AutoSize = false;
            this.barVPos.Location = new System.Drawing.Point(6, 87);
            this.barVPos.Maximum = 100;
            this.barVPos.Minimum = -100;
            this.barVPos.Name = "barVPos";
            this.barVPos.Size = new System.Drawing.Size(129, 25);
            this.barVPos.TabIndex = 5;
            this.barVPos.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // lblHPos
            // 
            this.lblHPos.AutoSize = true;
            this.lblHPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblHPos.Location = new System.Drawing.Point(7, 39);
            this.lblHPos.Name = "lblHPos";
            this.lblHPos.Size = new System.Drawing.Size(33, 13);
            this.lblHPos.TabIndex = 4;
            this.lblHPos.Text = "HPos";
            // 
            // barHPos
            // 
            this.barHPos.AutoSize = false;
            this.barHPos.Location = new System.Drawing.Point(6, 51);
            this.barHPos.Maximum = 100;
            this.barHPos.Minimum = -100;
            this.barHPos.Name = "barHPos";
            this.barHPos.Size = new System.Drawing.Size(129, 25);
            this.barHPos.TabIndex = 1;
            this.barHPos.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // chkHexLabelEnabled
            // 
            this.chkHexLabelEnabled.AutoSize = true;
            this.chkHexLabelEnabled.Checked = true;
            this.chkHexLabelEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHexLabelEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chkHexLabelEnabled.Location = new System.Drawing.Point(6, 19);
            this.chkHexLabelEnabled.Name = "chkHexLabelEnabled";
            this.chkHexLabelEnabled.Size = new System.Drawing.Size(119, 17);
            this.chkHexLabelEnabled.TabIndex = 0;
            this.chkHexLabelEnabled.Text = "включить подпись";
            this.chkHexLabelEnabled.UseVisualStyleBackColor = true;
            // 
            // groupCoordinateSystem
            // 
            this.groupCoordinateSystem.Controls.Add(this.btnOffsetEven);
            this.groupCoordinateSystem.Controls.Add(this.btnOffsetOdd);
            this.groupCoordinateSystem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupCoordinateSystem.Location = new System.Drawing.Point(7, 216);
            this.groupCoordinateSystem.Name = "groupCoordinateSystem";
            this.groupCoordinateSystem.Size = new System.Drawing.Size(142, 64);
            this.groupCoordinateSystem.TabIndex = 7;
            this.groupCoordinateSystem.TabStop = false;
            this.groupCoordinateSystem.Text = "Генерация карты";
            // 
            // btnOffsetEven
            // 
            this.btnOffsetEven.AutoSize = true;
            this.btnOffsetEven.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnOffsetEven.Location = new System.Drawing.Point(6, 41);
            this.btnOffsetEven.Name = "btnOffsetEven";
            this.btnOffsetEven.Size = new System.Drawing.Size(78, 17);
            this.btnOffsetEven.TabIndex = 2;
            this.btnOffsetEven.Text = "offset even";
            this.btnOffsetEven.UseVisualStyleBackColor = true;
            // 
            // btnOffsetOdd
            // 
            this.btnOffsetOdd.AutoSize = true;
            this.btnOffsetOdd.Checked = true;
            this.btnOffsetOdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnOffsetOdd.Location = new System.Drawing.Point(6, 19);
            this.btnOffsetOdd.Name = "btnOffsetOdd";
            this.btnOffsetOdd.Size = new System.Drawing.Size(72, 17);
            this.btnOffsetOdd.TabIndex = 1;
            this.btnOffsetOdd.TabStop = true;
            this.btnOffsetOdd.Text = "offset odd";
            this.btnOffsetOdd.UseVisualStyleBackColor = true;
            // 
            // groupTopped
            // 
            this.groupTopped.Controls.Add(this.btnPointyTopped);
            this.groupTopped.Controls.Add(this.btnFlatTopped);
            this.groupTopped.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupTopped.Location = new System.Drawing.Point(6, 152);
            this.groupTopped.Name = "groupTopped";
            this.groupTopped.Size = new System.Drawing.Size(142, 64);
            this.groupTopped.TabIndex = 6;
            this.groupTopped.TabStop = false;
            this.groupTopped.Text = "Ориентация гекса";
            // 
            // btnPointyTopped
            // 
            this.btnPointyTopped.AutoSize = true;
            this.btnPointyTopped.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPointyTopped.Location = new System.Drawing.Point(6, 41);
            this.btnPointyTopped.Name = "btnPointyTopped";
            this.btnPointyTopped.Size = new System.Drawing.Size(89, 17);
            this.btnPointyTopped.TabIndex = 2;
            this.btnPointyTopped.Text = "pointy topped";
            this.btnPointyTopped.UseVisualStyleBackColor = true;
            // 
            // btnFlatTopped
            // 
            this.btnFlatTopped.AutoSize = true;
            this.btnFlatTopped.Checked = true;
            this.btnFlatTopped.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFlatTopped.Location = new System.Drawing.Point(6, 19);
            this.btnFlatTopped.Name = "btnFlatTopped";
            this.btnFlatTopped.Size = new System.Drawing.Size(75, 17);
            this.btnFlatTopped.TabIndex = 1;
            this.btnFlatTopped.TabStop = true;
            this.btnFlatTopped.Text = "flat topped";
            this.btnFlatTopped.UseVisualStyleBackColor = true;
            // 
            // lblSideLength
            // 
            this.lblSideLength.AutoSize = true;
            this.lblSideLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSideLength.Location = new System.Drawing.Point(3, 87);
            this.lblSideLength.Name = "lblSideLength";
            this.lblSideLength.Size = new System.Drawing.Size(90, 13);
            this.lblSideLength.TabIndex = 5;
            this.lblSideLength.Text = "Размер гекса";
            // 
            // numHexLength
            // 
            this.numHexLength.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numHexLength.Location = new System.Drawing.Point(6, 103);
            this.numHexLength.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numHexLength.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numHexLength.Name = "numHexLength";
            this.numHexLength.Size = new System.Drawing.Size(120, 20);
            this.numHexLength.TabIndex = 4;
            this.numHexLength.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // lblMapHeight
            // 
            this.lblMapHeight.AutoSize = true;
            this.lblMapHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblMapHeight.Location = new System.Drawing.Point(3, 48);
            this.lblMapHeight.Name = "lblMapHeight";
            this.lblMapHeight.Size = new System.Drawing.Size(91, 13);
            this.lblMapHeight.TabIndex = 3;
            this.lblMapHeight.Text = "Высота карты";
            // 
            // numMapHeight
            // 
            this.numMapHeight.Location = new System.Drawing.Point(6, 64);
            this.numMapHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMapHeight.Name = "numMapHeight";
            this.numMapHeight.Size = new System.Drawing.Size(120, 20);
            this.numMapHeight.TabIndex = 2;
            this.numMapHeight.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lblMapWidth
            // 
            this.lblMapWidth.AutoSize = true;
            this.lblMapWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblMapWidth.Location = new System.Drawing.Point(3, 9);
            this.lblMapWidth.Name = "lblMapWidth";
            this.lblMapWidth.Size = new System.Drawing.Size(92, 13);
            this.lblMapWidth.TabIndex = 1;
            this.lblMapWidth.Text = "Ширина карты";
            // 
            // numMapWidth
            // 
            this.numMapWidth.Location = new System.Drawing.Point(6, 25);
            this.numMapWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMapWidth.Name = "numMapWidth";
            this.numMapWidth.Size = new System.Drawing.Size(120, 20);
            this.numMapWidth.TabIndex = 0;
            this.numMapWidth.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(896, 487);
            this.Controls.Add(this.splitContainer);
            this.Name = "frmMain";
            this.Text = "HexMap";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgBoard)).EndInit();
            this.groupHexLabel.ResumeLayout(false);
            this.groupHexLabel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barVPos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barHPos)).EndInit();
            this.groupCoordinateSystem.ResumeLayout(false);
            this.groupCoordinateSystem.PerformLayout();
            this.groupTopped.ResumeLayout(false);
            this.groupTopped.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHexLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMapHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMapWidth)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.PictureBox imgBoard;
        private System.Windows.Forms.Label lblMapWidth;
        private System.Windows.Forms.NumericUpDown numMapWidth;
        private System.Windows.Forms.Label lblMapHeight;
        private System.Windows.Forms.NumericUpDown numMapHeight;
        private System.Windows.Forms.Label lblSideLength;
        private System.Windows.Forms.NumericUpDown numHexLength;
        private System.Windows.Forms.GroupBox groupTopped;
        private System.Windows.Forms.RadioButton btnPointyTopped;
        private System.Windows.Forms.RadioButton btnFlatTopped;
        private System.Windows.Forms.GroupBox groupHexLabel;
        private System.Windows.Forms.GroupBox groupCoordinateSystem;
        private System.Windows.Forms.RadioButton btnOffsetEven;
        private System.Windows.Forms.RadioButton btnOffsetOdd;
        private System.Windows.Forms.CheckBox chkHexLabelEnabled;
        private System.Windows.Forms.TrackBar barHPos;
        private System.Windows.Forms.Label lblHPos;
        private System.Windows.Forms.Label lblVPos;
        private System.Windows.Forms.TrackBar barVPos;
        private System.Windows.Forms.Button btnHexLabelFont;
        private System.Windows.Forms.ComboBox cmbHexCoordinateSystem;
        private System.Windows.Forms.CheckBox chkGridVisible;
    }
}

