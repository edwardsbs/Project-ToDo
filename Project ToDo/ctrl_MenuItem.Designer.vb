<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctrl_MenuItem
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.panel_Tab = New System.Windows.Forms.Panel()
        Me.pic_Icon = New System.Windows.Forms.PictureBox()
        Me.lab_Index = New System.Windows.Forms.Label()
        Me.pan_Selected = New System.Windows.Forms.Panel()
        Me.label_TabText = New System.Windows.Forms.Label()
        Me.panel_Tab.SuspendLayout()
        CType(Me.pic_Icon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'panel_Tab
        '
        Me.panel_Tab.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panel_Tab.Controls.Add(Me.pic_Icon)
        Me.panel_Tab.Controls.Add(Me.lab_Index)
        Me.panel_Tab.Controls.Add(Me.pan_Selected)
        Me.panel_Tab.Controls.Add(Me.label_TabText)
        Me.panel_Tab.Location = New System.Drawing.Point(0, 1)
        Me.panel_Tab.Name = "panel_Tab"
        Me.panel_Tab.Size = New System.Drawing.Size(151, 46)
        Me.panel_Tab.TabIndex = 0
        '
        'pic_Icon
        '
        Me.pic_Icon.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pic_Icon.Location = New System.Drawing.Point(15, 2)
        Me.pic_Icon.Name = "pic_Icon"
        Me.pic_Icon.Size = New System.Drawing.Size(40, 42)
        Me.pic_Icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pic_Icon.TabIndex = 2
        Me.pic_Icon.TabStop = False
        '
        'lab_Index
        '
        Me.lab_Index.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lab_Index.Location = New System.Drawing.Point(123, 1)
        Me.lab_Index.Name = "lab_Index"
        Me.lab_Index.Size = New System.Drawing.Size(28, 14)
        Me.lab_Index.TabIndex = 1
        Me.lab_Index.Text = "Label1"
        Me.lab_Index.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lab_Index.Visible = False
        '
        'pan_Selected
        '
        Me.pan_Selected.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pan_Selected.BackColor = System.Drawing.Color.OrangeRed
        Me.pan_Selected.Location = New System.Drawing.Point(4, 2)
        Me.pan_Selected.Name = "pan_Selected"
        Me.pan_Selected.Size = New System.Drawing.Size(8, 41)
        Me.pan_Selected.TabIndex = 0
        '
        'label_TabText
        '
        Me.label_TabText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.label_TabText.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label_TabText.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.label_TabText.Location = New System.Drawing.Point(57, 3)
        Me.label_TabText.Name = "label_TabText"
        Me.label_TabText.Size = New System.Drawing.Size(88, 40)
        Me.label_TabText.TabIndex = 3
        Me.label_TabText.Text = "Label2"
        Me.label_TabText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ctrl_MenuItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.DarkGray
        Me.Controls.Add(Me.panel_Tab)
        Me.Name = "ctrl_MenuItem"
        Me.Size = New System.Drawing.Size(156, 49)
        Me.panel_Tab.ResumeLayout(False)
        CType(Me.pic_Icon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents panel_Tab As System.Windows.Forms.Panel
    Friend WithEvents pic_Icon As System.Windows.Forms.PictureBox
    Friend WithEvents lab_Index As System.Windows.Forms.Label
    Friend WithEvents pan_Selected As System.Windows.Forms.Panel
    Friend WithEvents label_TabText As System.Windows.Forms.Label

End Class
