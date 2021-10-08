namespace Fonter
{
    partial class DialogForm
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
            this.spinWidth = new System.Windows.Forms.NumericUpDown();
            this.spinHeight = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblContent = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.spinWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // spinWidth
            // 
            this.spinWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.spinWidth.Location = new System.Drawing.Point(184, 69);
            this.spinWidth.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.spinWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinWidth.Name = "spinWidth";
            this.spinWidth.Size = new System.Drawing.Size(120, 23);
            this.spinWidth.TabIndex = 1;
            this.spinWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.spinWidth.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // spinHeight
            // 
            this.spinHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.spinHeight.Location = new System.Drawing.Point(184, 43);
            this.spinHeight.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.spinHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinHeight.Name = "spinHeight";
            this.spinHeight.Size = new System.Drawing.Size(120, 23);
            this.spinHeight.TabIndex = 0;
            this.spinHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.spinHeight.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(107, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Height:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(108, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Width:";
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblContent.Location = new System.Drawing.Point(46, 16);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(109, 17);
            this.lblContent.TabIndex = 4;
            this.lblContent.Text = "Size of the Field";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(148, 101);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "Ok";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(229, 101);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // DialogForm
            // 
            this.AcceptButton = this.btnOK;
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Dialog;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 131);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.spinHeight);
            this.Controls.Add(this.spinWidth);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 230);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 170);
            this.Name = "DialogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NewDialogForm";
            ((System.ComponentModel.ISupportInitialize)(this.spinWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown spinWidth;
        private System.Windows.Forms.NumericUpDown spinHeight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}