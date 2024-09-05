<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_Notes
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_Notes))
        Me.rTxt_Notes = New System.Windows.Forms.RichTextBox()
        Me.lbl_Task = New System.Windows.Forms.Label()
        Me.lbl_System = New System.Windows.Forms.Label()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ss_Path = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ctr_Images = New Project_ToDo.ctrl_Images()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'rTxt_Notes
        '
        Me.rTxt_Notes.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rTxt_Notes.Location = New System.Drawing.Point(0, 35)
        Me.rTxt_Notes.Name = "rTxt_Notes"
        Me.rTxt_Notes.Size = New System.Drawing.Size(624, 462)
        Me.rTxt_Notes.TabIndex = 0
        Me.rTxt_Notes.Text = "Notes"
        '
        'lbl_Task
        '
        Me.lbl_Task.AutoSize = True
        Me.lbl_Task.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Task.Location = New System.Drawing.Point(4, 8)
        Me.lbl_Task.Name = "lbl_Task"
        Me.lbl_Task.Size = New System.Drawing.Size(44, 23)
        Me.lbl_Task.TabIndex = 1
        Me.lbl_Task.Text = "Task"
        '
        'lbl_System
        '
        Me.lbl_System.AutoSize = True
        Me.lbl_System.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_System.Location = New System.Drawing.Point(351, 8)
        Me.lbl_System.Name = "lbl_System"
        Me.lbl_System.Size = New System.Drawing.Size(66, 23)
        Me.lbl_System.TabIndex = 2
        Me.lbl_System.Text = "System"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ss_Path})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 500)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(867, 22)
        Me.StatusStrip1.TabIndex = 4
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ss_Path
        '
        Me.ss_Path.Name = "ss_Path"
        Me.ss_Path.Size = New System.Drawing.Size(39, 17)
        Me.ss_Path.Text = "Ready"
        '
        'ctr_Images
        '
        Me.ctr_Images.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ctr_Images.BackColor = System.Drawing.Color.Gray
        Me.ctr_Images.Location = New System.Drawing.Point(630, 35)
        Me.ctr_Images.Name = "ctr_Images"
        Me.ctr_Images.Size = New System.Drawing.Size(235, 462)
        Me.ctr_Images.TabIndex = 3
        '
        'frm_Notes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(867, 522)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.ctr_Images)
        Me.Controls.Add(Me.lbl_System)
        Me.Controls.Add(Me.lbl_Task)
        Me.Controls.Add(Me.rTxt_Notes)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frm_Notes"
        Me.Text = "Notes"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents rTxt_Notes As RichTextBox
    Friend WithEvents lbl_Task As Label
    Friend WithEvents lbl_System As Label
    Friend WithEvents ctr_Images As ctrl_Images
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ss_Path As ToolStripStatusLabel
End Class
