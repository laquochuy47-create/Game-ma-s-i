namespace WerewolfClient;

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
        button2 = new Button();
        fileSystemWatcher1 = new FileSystemWatcher();
        checkBox1 = new CheckBox();
        ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).BeginInit();
        SuspendLayout();
        // 
        // button2
        // 
        button2.Location = new Point(355, 142);
        button2.Name = "button2";
        button2.Size = new Size(94, 29);
        button2.TabIndex = 0;
        button2.Text = "button2";
        button2.UseVisualStyleBackColor = true;
        button2.Click += button2_Click;
        // 
        // fileSystemWatcher1
        // 
        fileSystemWatcher1.EnableRaisingEvents = true;
        fileSystemWatcher1.SynchronizingObject = this;
        // 
        // checkBox1
        // 
        checkBox1.AutoSize = true;
        checkBox1.Location = new Point(605, 167);
        checkBox1.Name = "checkBox1";
        checkBox1.Size = new Size(101, 24);
        checkBox1.TabIndex = 1;
        checkBox1.Text = "checkBox1";
        checkBox1.UseVisualStyleBackColor = true;
        checkBox1.CheckedChanged += checkBox1_CheckedChanged;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(checkBox1);
        Controls.Add(button2);
        Name = "Form1";
        Text = "Form1";
        ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button button1;
    private Button button2;
    private FileSystemWatcher fileSystemWatcher1;
    private CheckBox checkBox1;
}
