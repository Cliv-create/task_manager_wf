namespace ProductsWinForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listView1 = new ListView();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            button1 = new Button();
            label4 = new Label();
            label5 = new Label();
            button2 = new Button();
            SuspendLayout();
            // 
            // listView1
            // 
            listView1.Location = new Point(12, 12);
            listView1.Name = "listView1";
            listView1.Size = new Size(510, 434);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(528, 12);
            label1.Name = "label1";
            label1.Size = new Size(18, 15);
            label1.TabIndex = 1;
            label1.Text = "ID";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(528, 27);
            label2.Name = "label2";
            label2.Size = new Size(39, 15);
            label2.TabIndex = 2;
            label2.Text = "Name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(528, 42);
            label3.Name = "label3";
            label3.Size = new Size(76, 15);
            label3.TabIndex = 3;
            label3.Text = "Window Title";
            // 
            // button1
            // 
            button1.Location = new Point(695, 423);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 5;
            button1.Text = "Details";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(528, 57);
            label4.Name = "label4";
            label4.Size = new Size(92, 15);
            label4.TabIndex = 6;
            label4.Text = "Window Handle";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(529, 72);
            label5.Name = "label5";
            label5.Size = new Size(79, 15);
            label5.TabIndex = 7;
            label5.Text = "Thread Count";
            // 
            // button2
            // 
            button2.Location = new Point(614, 423);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 8;
            button2.Text = "Terminate";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(782, 458);
            Controls.Add(button2);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(button1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(listView1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Магазин \"АТБ\"";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView listView1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button button1;
        private Label label4;
        private Label label5;
        private Button button2;
    }
}
