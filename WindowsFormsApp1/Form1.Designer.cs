namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.textBox14 = new System.Windows.Forms.TextBox();
            this.textBox15 = new System.Windows.Forms.TextBox();
            this.textBox16 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox17 = new System.Windows.Forms.TextBox();
            this.textBox18 = new System.Windows.Forms.TextBox();
            this.textBox19 = new System.Windows.Forms.TextBox();
            this.textBox20 = new System.Windows.Forms.TextBox();
            this.textBox21 = new System.Windows.Forms.TextBox();
            this.textBox22 = new System.Windows.Forms.TextBox();
            this.textBox23 = new System.Windows.Forms.TextBox();
            this.textBox24 = new System.Windows.Forms.TextBox();
            this.textBox25 = new System.Windows.Forms.TextBox();
            this.textBox26 = new System.Windows.Forms.TextBox();
            this.textBox27 = new System.Windows.Forms.TextBox();
            this.textBox28 = new System.Windows.Forms.TextBox();
            this.textBox29 = new System.Windows.Forms.TextBox();
            this.textBox30 = new System.Windows.Forms.TextBox();
            this.textBox31 = new System.Windows.Forms.TextBox();
            this.textBox32 = new System.Windows.Forms.TextBox();
            this.textBox33 = new System.Windows.Forms.TextBox();
            this.textBox34 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(704, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 58;
            this.button1.Text = "Oblicz";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(785, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 60;
            this.button2.Text = "Wyczyść";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 580);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(202, 20);
            this.textBox2.TabIndex = 63;
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(221, 424);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(25, 20);
            this.textBox3.TabIndex = 64;
            this.textBox3.Text = "=<0";
            // 
            // button3
            // 
            this.button3.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.button3.Location = new System.Drawing.Point(13, 606);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(112, 23);
            this.button3.TabIndex = 65;
            this.button3.Text = "Dodaj ograniczenie";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // textBox4
            // 
            this.textBox4.Enabled = false;
            this.textBox4.Location = new System.Drawing.Point(12, 308);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(99, 20);
            this.textBox4.TabIndex = 66;
            this.textBox4.Text = "Ograniczenia";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "4-x1-x2",
            "x1^2-x2",
            "x1+x2-2",
            "x1-2*x2+1",
            "x1^2/4+x2^2-1",
            "-x1-10",
            "x1-10",
            "-x2-10",
            "x2-10",
            "(x1-0.05)^2+(x2-2.5)^2-4.84",
            "4.84-x1^2-(x2-2.5)^2",
            "-x1",
            "x1-6",
            "-x2",
            "x2-6",
            "x1+x2-15",
            "5-x1+x2^2",
            "x1",
            "x1+x2-2",
            "-1-x2"});
            this.checkedListBox1.Location = new System.Drawing.Point(12, 329);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(203, 244);
            this.checkedListBox1.TabIndex = 67;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(55, 39);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 20);
            this.textBox5.TabIndex = 68;
            this.textBox5.Text = "0.001";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(55, 62);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(100, 20);
            this.textBox6.TabIndex = 69;
            this.textBox6.Text = "1";
            // 
            // textBox7
            // 
            this.textBox7.Enabled = false;
            this.textBox7.Location = new System.Drawing.Point(14, 38);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(35, 20);
            this.textBox7.TabIndex = 70;
            this.textBox7.Text = "c_min";
            // 
            // textBox8
            // 
            this.textBox8.Enabled = false;
            this.textBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBox8.Location = new System.Drawing.Point(14, 62);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(35, 20);
            this.textBox8.TabIndex = 71;
            this.textBox8.Text = "c";
            // 
            // textBox9
            // 
            this.textBox9.Enabled = false;
            this.textBox9.Location = new System.Drawing.Point(12, 113);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(100, 20);
            this.textBox9.TabIndex = 75;
            this.textBox9.Text = "Argumenty";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(140, 282);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 74;
            this.button4.Text = "Dodaj";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(13, 282);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(58, 20);
            this.textBox10.TabIndex = 73;
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(76, 282);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(58, 20);
            this.textBox11.TabIndex = 76;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.Desktop;
            this.dataGridView1.Location = new System.Drawing.Point(12, 139);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(202, 137);
            this.dataGridView1.TabIndex = 78;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Click);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Argument";
            this.Column1.MaxInputLength = 5;
            this.Column1.Name = "Column1";
            this.Column1.Width = 60;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Wartość początkowa";
            this.Column2.Name = "Column2";
            this.Column2.Width = 95;
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(256, 41);
            this.textBox12.Multiline = true;
            this.textBox12.Name = "textBox12";
            this.textBox12.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox12.Size = new System.Drawing.Size(604, 91);
            this.textBox12.TabIndex = 79;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "x1^4+x2^4-2*x1^2*x2-4*x1+3",
            "(x1-2)^2+(x2-1)^2",
            "(x1^2+x2-11)^2+(x1+x2^2-7)^2",
            "x1^4+x2^4-x1^2-x2^2"});
            this.comboBox1.Location = new System.Drawing.Point(256, 11);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(444, 21);
            this.comboBox1.TabIndex = 80;
            this.comboBox1.Text = "x1^4+x2^4-2*x1^2*x2-4*x1+3";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(55, 15);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 81;
            this.textBox1.Text = "10";
            // 
            // textBox13
            // 
            this.textBox13.Enabled = false;
            this.textBox13.Location = new System.Drawing.Point(14, 15);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(37, 20);
            this.textBox13.TabIndex = 82;
            this.textBox13.Text = "Kroki";
            // 
            // textBox14
            // 
            this.textBox14.Location = new System.Drawing.Point(256, 153);
            this.textBox14.Multiline = true;
            this.textBox14.Name = "textBox14";
            this.textBox14.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox14.Size = new System.Drawing.Size(604, 291);
            this.textBox14.TabIndex = 83;
            this.textBox14.WordWrap = false;
            // 
            // textBox15
            // 
            this.textBox15.Enabled = false;
            this.textBox15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBox15.Location = new System.Drawing.Point(13, 88);
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new System.Drawing.Size(35, 20);
            this.textBox15.TabIndex = 84;
            this.textBox15.Text = "E";
            // 
            // textBox16
            // 
            this.textBox16.Location = new System.Drawing.Point(55, 88);
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new System.Drawing.Size(100, 20);
            this.textBox16.TabIndex = 85;
            this.textBox16.Text = "0.001";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(524, 466);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(45, 23);
            this.button5.TabIndex = 86;
            this.button5.Text = "stop";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Button5_Click);
            // 
            // textBox17
            // 
            this.textBox17.Location = new System.Drawing.Point(221, 546);
            this.textBox17.Multiline = true;
            this.textBox17.Name = "textBox17";
            this.textBox17.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox17.Size = new System.Drawing.Size(127, 83);
            this.textBox17.TabIndex = 87;
            this.textBox17.WordWrap = false;
            // 
            // textBox18
            // 
            this.textBox18.Location = new System.Drawing.Point(354, 599);
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new System.Drawing.Size(31, 20);
            this.textBox18.TabIndex = 88;
            this.textBox18.Text = "e2->";
            // 
            // textBox19
            // 
            this.textBox19.Location = new System.Drawing.Point(391, 546);
            this.textBox19.Multiline = true;
            this.textBox19.Name = "textBox19";
            this.textBox19.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox19.Size = new System.Drawing.Size(127, 83);
            this.textBox19.TabIndex = 87;
            this.textBox19.WordWrap = false;
            // 
            // textBox20
            // 
            this.textBox20.Location = new System.Drawing.Point(354, 562);
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new System.Drawing.Size(31, 20);
            this.textBox20.TabIndex = 88;
            this.textBox20.Text = "<-e1";
            // 
            // textBox21
            // 
            this.textBox21.Location = new System.Drawing.Point(575, 546);
            this.textBox21.Multiline = true;
            this.textBox21.Name = "textBox21";
            this.textBox21.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox21.Size = new System.Drawing.Size(127, 83);
            this.textBox21.TabIndex = 87;
            this.textBox21.WordWrap = false;
            // 
            // textBox22
            // 
            this.textBox22.Location = new System.Drawing.Point(745, 546);
            this.textBox22.Multiline = true;
            this.textBox22.Name = "textBox22";
            this.textBox22.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox22.Size = new System.Drawing.Size(127, 83);
            this.textBox22.TabIndex = 87;
            this.textBox22.WordWrap = false;
            // 
            // textBox23
            // 
            this.textBox23.Location = new System.Drawing.Point(708, 599);
            this.textBox23.Name = "textBox23";
            this.textBox23.Size = new System.Drawing.Size(31, 20);
            this.textBox23.TabIndex = 88;
            this.textBox23.Text = "e4->";
            // 
            // textBox24
            // 
            this.textBox24.Location = new System.Drawing.Point(708, 562);
            this.textBox24.Name = "textBox24";
            this.textBox24.Size = new System.Drawing.Size(31, 20);
            this.textBox24.TabIndex = 88;
            this.textBox24.Text = "<-e3";
            // 
            // textBox25
            // 
            this.textBox25.Location = new System.Drawing.Point(221, 450);
            this.textBox25.Multiline = true;
            this.textBox25.Name = "textBox25";
            this.textBox25.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox25.Size = new System.Drawing.Size(127, 83);
            this.textBox25.TabIndex = 87;
            this.textBox25.WordWrap = false;
            // 
            // textBox26
            // 
            this.textBox26.Location = new System.Drawing.Point(391, 450);
            this.textBox26.Multiline = true;
            this.textBox26.Name = "textBox26";
            this.textBox26.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox26.Size = new System.Drawing.Size(127, 83);
            this.textBox26.TabIndex = 87;
            this.textBox26.WordWrap = false;
            // 
            // textBox27
            // 
            this.textBox27.Location = new System.Drawing.Point(575, 450);
            this.textBox27.Multiline = true;
            this.textBox27.Name = "textBox27";
            this.textBox27.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox27.Size = new System.Drawing.Size(127, 83);
            this.textBox27.TabIndex = 87;
            this.textBox27.WordWrap = false;
            // 
            // textBox28
            // 
            this.textBox28.Location = new System.Drawing.Point(745, 450);
            this.textBox28.Multiline = true;
            this.textBox28.Name = "textBox28";
            this.textBox28.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox28.Size = new System.Drawing.Size(127, 83);
            this.textBox28.TabIndex = 87;
            this.textBox28.WordWrap = false;
            // 
            // textBox29
            // 
            this.textBox29.Location = new System.Drawing.Point(354, 503);
            this.textBox29.Name = "textBox29";
            this.textBox29.Size = new System.Drawing.Size(31, 20);
            this.textBox29.TabIndex = 88;
            this.textBox29.Text = "x2->";
            // 
            // textBox30
            // 
            this.textBox30.Location = new System.Drawing.Point(708, 503);
            this.textBox30.Name = "textBox30";
            this.textBox30.Size = new System.Drawing.Size(31, 20);
            this.textBox30.TabIndex = 88;
            this.textBox30.Text = "c->";
            // 
            // textBox31
            // 
            this.textBox31.Location = new System.Drawing.Point(354, 466);
            this.textBox31.Name = "textBox31";
            this.textBox31.Size = new System.Drawing.Size(31, 20);
            this.textBox31.TabIndex = 88;
            this.textBox31.Text = "<-x1";
            // 
            // textBox32
            // 
            this.textBox32.Location = new System.Drawing.Point(708, 466);
            this.textBox32.Name = "textBox32";
            this.textBox32.Size = new System.Drawing.Size(31, 20);
            this.textBox32.TabIndex = 88;
            this.textBox32.Text = "<-f*";
            // 
            // textBox33
            // 
            this.textBox33.Location = new System.Drawing.Point(181, 62);
            this.textBox33.Name = "textBox33";
            this.textBox33.Size = new System.Drawing.Size(54, 20);
            this.textBox33.TabIndex = 81;
            this.textBox33.Text = "10";
            // 
            // textBox34
            // 
            this.textBox34.Location = new System.Drawing.Point(181, 88);
            this.textBox34.Name = "textBox34";
            this.textBox34.Size = new System.Drawing.Size(54, 20);
            this.textBox34.TabIndex = 81;
            this.textBox34.Text = "10";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 631);
            this.Controls.Add(this.textBox32);
            this.Controls.Add(this.textBox24);
            this.Controls.Add(this.textBox31);
            this.Controls.Add(this.textBox20);
            this.Controls.Add(this.textBox30);
            this.Controls.Add(this.textBox23);
            this.Controls.Add(this.textBox29);
            this.Controls.Add(this.textBox18);
            this.Controls.Add(this.textBox28);
            this.Controls.Add(this.textBox22);
            this.Controls.Add(this.textBox27);
            this.Controls.Add(this.textBox26);
            this.Controls.Add(this.textBox21);
            this.Controls.Add(this.textBox25);
            this.Controls.Add(this.textBox19);
            this.Controls.Add(this.textBox17);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.textBox16);
            this.Controls.Add(this.textBox15);
            this.Controls.Add(this.textBox14);
            this.Controls.Add(this.textBox13);
            this.Controls.Add(this.textBox34);
            this.Controls.Add(this.textBox33);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textBox12);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox13;
        private System.Windows.Forms.TextBox textBox14;
        private System.Windows.Forms.TextBox textBox15;
        private System.Windows.Forms.TextBox textBox16;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox17;
        private System.Windows.Forms.TextBox textBox18;
        private System.Windows.Forms.TextBox textBox19;
        private System.Windows.Forms.TextBox textBox20;
        private System.Windows.Forms.TextBox textBox21;
        private System.Windows.Forms.TextBox textBox22;
        private System.Windows.Forms.TextBox textBox23;
        private System.Windows.Forms.TextBox textBox24;
        private System.Windows.Forms.TextBox textBox25;
        private System.Windows.Forms.TextBox textBox26;
        private System.Windows.Forms.TextBox textBox27;
        private System.Windows.Forms.TextBox textBox28;
        private System.Windows.Forms.TextBox textBox29;
        private System.Windows.Forms.TextBox textBox30;
        private System.Windows.Forms.TextBox textBox31;
        private System.Windows.Forms.TextBox textBox32;
        private System.Windows.Forms.TextBox textBox33;
        private System.Windows.Forms.TextBox textBox34;
    }
}

