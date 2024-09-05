<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ctrl_Schedule
    Inherits UserControl

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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ctrl_Schedule))
        Me.pnl_Schedule = New Panel()
        Me.panel_Schedule = New Panel()
        Me.pnl_Months = New Panel()
        Me.Panel1 = New Panel()
        Me.pnl_Header = New Panel()
        Me.lbl_Task = New Label()
        Me.lbl_Priority = New Label()
        Me.iList_Gauge = New ImageList(Me.components)
        Me.Panel1.SuspendLayout()
        Me.pnl_Header.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_Schedule
        '
        Me.pnl_Schedule.BorderStyle = BorderStyle.FixedSingle
        Me.pnl_Schedule.Location = New Point(4, 52)
        Me.pnl_Schedule.Name = "pnl_Schedule"
        Me.pnl_Schedule.Size = New Size(384, 296)
        Me.pnl_Schedule.TabIndex = 9
        '
        'panel_Schedule
        '
        Me.panel_Schedule.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.panel_Schedule.BorderStyle = BorderStyle.FixedSingle
        Me.panel_Schedule.Location = New Point(388, 52)
        Me.panel_Schedule.Name = "panel_Schedule"
        Me.panel_Schedule.Size = New Size(236, 293)
        Me.panel_Schedule.TabIndex = 5
        '
        'pnl_Months
        '
        Me.pnl_Months.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.pnl_Months.BorderStyle = BorderStyle.FixedSingle
        Me.pnl_Months.Location = New Point(387, 4)
        Me.pnl_Months.Name = "pnl_Months"
        Me.pnl_Months.Size = New Size(239, 48)
        Me.pnl_Months.TabIndex = 8
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.Panel1.AutoScroll = True
        Me.Panel1.BackColor = Color.White
        Me.Panel1.Controls.Add(Me.panel_Schedule)
        Me.Panel1.Controls.Add(Me.pnl_Schedule)
        Me.Panel1.Controls.Add(Me.pnl_Header)
        Me.Panel1.Location = New Point(0, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New Size(628, 348)
        Me.Panel1.TabIndex = 12
        '
        'pnl_Header
        '
        Me.pnl_Header.Controls.Add(Me.pnl_Months)
        Me.pnl_Header.Controls.Add(Me.lbl_Task)
        Me.pnl_Header.Controls.Add(Me.lbl_Priority)
        Me.pnl_Header.Dock = DockStyle.Top
        Me.pnl_Header.Location = New Point(0, 0)
        Me.pnl_Header.Name = "pnl_Header"
        Me.pnl_Header.Size = New Size(628, 53)
        Me.pnl_Header.TabIndex = 14
        '
        'lbl_Task
        '
        Me.lbl_Task.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left), AnchorStyles)
        Me.lbl_Task.Font = New Font("Microsoft Sans Serif", 14.25!, FontStyle.Bold, GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Task.ForeColor = Color.MidnightBlue
        Me.lbl_Task.Location = New Point(45, 11)
        Me.lbl_Task.Name = "lbl_Task"
        Me.lbl_Task.Size = New Size(335, 27)
        Me.lbl_Task.TabIndex = 13
        Me.lbl_Task.Text = "Project Name"
        Me.lbl_Task.TextAlign = ContentAlignment.MiddleLeft
        '
        'lbl_Priority
        '
        Me.lbl_Priority.AutoSize = True
        Me.lbl_Priority.Font = New Font("Microsoft Sans Serif", 12.0!, FontStyle.Bold, GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Priority.ForeColor = Color.MidnightBlue
        Me.lbl_Priority.Location = New Point(9, 23)
        Me.lbl_Priority.Name = "lbl_Priority"
        Me.lbl_Priority.Size = New Size(30, 20)
        Me.lbl_Priority.TabIndex = 12
        Me.lbl_Priority.Text = "Pri"
        '
        'iList_Gauge
        '
        Me.iList_Gauge.ImageStream = CType(resources.GetObject("iList_Gauge.ImageStream"), ImageListStreamer)
        Me.iList_Gauge.TransparentColor = Color.Transparent
        Me.iList_Gauge.Images.SetKeyName(0, "gauge-high.png")
        Me.iList_Gauge.Images.SetKeyName(1, "gauge-low.png")
        Me.iList_Gauge.Images.SetKeyName(2, "gauge-medium.png")
        '
        'ctrl_Schedule
        '
        Me.BackColor = SystemColors.Control
        Me.Controls.Add(Me.Panel1)
        Me.Name = "ctrl_Schedule"
        Me.Size = New Size(628, 355)
        Me.Panel1.ResumeLayout(False)
        Me.pnl_Header.ResumeLayout(False)
        Me.pnl_Header.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnl_Schedule As Panel
    Friend WithEvents pnl_Months As Panel
    Friend WithEvents panel_Schedule As Panel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents lbl_Priority As Label
    Friend WithEvents lbl_Task As Label
    Friend WithEvents iList_Gauge As ImageList
    Friend WithEvents pnl_Header As Panel
End Class
