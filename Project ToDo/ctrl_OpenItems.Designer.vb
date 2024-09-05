<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctrl_OpenItems
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
        Me.pnl_Months = New System.Windows.Forms.Panel()
        Me.pnl_Schedule = New System.Windows.Forms.Panel()
        Me.pnl_Items = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.pic_Refresh = New System.Windows.Forms.PictureBox()
        Me.pnl_Details = New System.Windows.Forms.Panel()
        Me.Panel1.SuspendLayout()
        CType(Me.pic_Refresh, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_Details.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_Months
        '
        Me.pnl_Months.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnl_Months.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Months.Location = New System.Drawing.Point(433, 6)
        Me.pnl_Months.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pnl_Months.Name = "pnl_Months"
        Me.pnl_Months.Size = New System.Drawing.Size(346, 59)
        Me.pnl_Months.TabIndex = 8
        '
        'pnl_Schedule
        '
        Me.pnl_Schedule.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Schedule.Location = New System.Drawing.Point(433, 1)
        Me.pnl_Schedule.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pnl_Schedule.Name = "pnl_Schedule"
        Me.pnl_Schedule.Size = New System.Drawing.Size(343, 196)
        Me.pnl_Schedule.TabIndex = 5
        '
        'pnl_Items
        '
        Me.pnl_Items.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Items.Location = New System.Drawing.Point(3, 1)
        Me.pnl_Items.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pnl_Items.Name = "pnl_Items"
        Me.pnl_Items.Size = New System.Drawing.Size(429, 196)
        Me.pnl_Items.TabIndex = 11
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.pic_Refresh)
        Me.Panel1.Controls.Add(Me.pnl_Months)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(812, 63)
        Me.Panel1.TabIndex = 12
        '
        'pic_Refresh
        '
        Me.pic_Refresh.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pic_Refresh.Image = Global.Project_ToDo.My.Resources.Resources.Refresh_Small
        Me.pic_Refresh.Location = New System.Drawing.Point(15, 11)
        Me.pic_Refresh.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pic_Refresh.Name = "pic_Refresh"
        Me.pic_Refresh.Size = New System.Drawing.Size(40, 37)
        Me.pic_Refresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pic_Refresh.TabIndex = 9
        Me.pic_Refresh.TabStop = False
        '
        'pnl_Details
        '
        Me.pnl_Details.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnl_Details.AutoScroll = True
        Me.pnl_Details.Controls.Add(Me.pnl_Items)
        Me.pnl_Details.Controls.Add(Me.pnl_Schedule)
        Me.pnl_Details.Location = New System.Drawing.Point(0, 60)
        Me.pnl_Details.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pnl_Details.Name = "pnl_Details"
        Me.pnl_Details.Size = New System.Drawing.Size(808, 198)
        Me.pnl_Details.TabIndex = 13
        '
        'ctrl_OpenItems
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.pnl_Details)
        Me.Controls.Add(Me.Panel1)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "ctrl_OpenItems"
        Me.Size = New System.Drawing.Size(812, 262)
        Me.Panel1.ResumeLayout(False)
        CType(Me.pic_Refresh, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_Details.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnl_Months As Panel
    Friend WithEvents pnl_Schedule As Panel
    Friend WithEvents pnl_Items As Panel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents pnl_Details As Panel
    Friend WithEvents pic_Refresh As PictureBox
End Class
