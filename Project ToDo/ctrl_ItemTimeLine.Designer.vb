<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ctrl_ItemTimeLine
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lbl_Item = New System.Windows.Forms.Label()
        Me.lbl_System = New System.Windows.Forms.Label()
        Me.pic_Notes = New System.Windows.Forms.PictureBox()
        CType(Me.pic_Notes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lbl_Item
        '
        Me.lbl_Item.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl_Item.AutoSize = True
        Me.lbl_Item.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Item.Location = New System.Drawing.Point(27, -3)
        Me.lbl_Item.Name = "lbl_Item"
        Me.lbl_Item.Size = New System.Drawing.Size(92, 18)
        Me.lbl_Item.TabIndex = 0
        Me.lbl_Item.Text = "Task / Activity"
        Me.lbl_Item.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbl_System
        '
        Me.lbl_System.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl_System.AutoSize = True
        Me.lbl_System.ForeColor = System.Drawing.Color.DimGray
        Me.lbl_System.Location = New System.Drawing.Point(41, 15)
        Me.lbl_System.Name = "lbl_System"
        Me.lbl_System.Size = New System.Drawing.Size(41, 13)
        Me.lbl_System.TabIndex = 1
        Me.lbl_System.Text = "System"
        '
        'pic_Notes
        '
        Me.pic_Notes.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pic_Notes.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pic_Notes.Image = Global.Project_ToDo.My.Resources.Resources.Notes_small
        Me.pic_Notes.Location = New System.Drawing.Point(267, 3)
        Me.pic_Notes.Name = "pic_Notes"
        Me.pic_Notes.Size = New System.Drawing.Size(22, 22)
        Me.pic_Notes.TabIndex = 2
        Me.pic_Notes.TabStop = False
        Me.pic_Notes.Visible = False
        '
        'ctrl_ItemTimeLine
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.Controls.Add(Me.pic_Notes)
        Me.Controls.Add(Me.lbl_System)
        Me.Controls.Add(Me.lbl_Item)
        Me.Name = "ctrl_ItemTimeLine"
        Me.Size = New System.Drawing.Size(293, 28)
        CType(Me.pic_Notes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lbl_Item As Label
    Friend WithEvents lbl_System As Label
    Friend WithEvents pic_Notes As PictureBox
End Class
