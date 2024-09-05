<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctr_ProjTimeLine
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ctr_ProjTimeLine))
        Me.lbl_Item = New System.Windows.Forms.Label()
        Me.dtp_Start = New System.Windows.Forms.DateTimePicker()
        Me.dtp_End = New System.Windows.Forms.DateTimePicker()
        Me.cbo_Status = New System.Windows.Forms.ComboBox()
        Me.pnl_TimeLine = New System.Windows.Forms.Panel()
        Me.imgLst_Status = New System.Windows.Forms.ImageList(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lbl_ActualStart = New System.Windows.Forms.Label()
        Me.lbl_PlanStart = New System.Windows.Forms.Label()
        Me.lbl_ActualEnd = New System.Windows.Forms.Label()
        Me.lbl_PlanEnd = New System.Windows.Forms.Label()
        Me.pic_Status = New System.Windows.Forms.PictureBox()
        CType(Me.pic_Status, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lbl_Item
        '
        Me.lbl_Item.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lbl_Item.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Item.Location = New System.Drawing.Point(3, 0)
        Me.lbl_Item.Name = "lbl_Item"
        Me.lbl_Item.Size = New System.Drawing.Size(146, 42)
        Me.lbl_Item.TabIndex = 0
        Me.lbl_Item.Text = "Label1"
        Me.lbl_Item.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtp_Start
        '
        Me.dtp_Start.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Start.Location = New System.Drawing.Point(151, 5)
        Me.dtp_Start.Name = "dtp_Start"
        Me.dtp_Start.Size = New System.Drawing.Size(95, 20)
        Me.dtp_Start.TabIndex = 1
        Me.dtp_Start.Visible = False
        '
        'dtp_End
        '
        Me.dtp_End.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_End.Location = New System.Drawing.Point(250, 5)
        Me.dtp_End.Name = "dtp_End"
        Me.dtp_End.Size = New System.Drawing.Size(95, 20)
        Me.dtp_End.TabIndex = 2
        Me.dtp_End.Visible = False
        '
        'cbo_Status
        '
        Me.cbo_Status.FormattingEnabled = True
        Me.cbo_Status.Location = New System.Drawing.Point(332, 2)
        Me.cbo_Status.Name = "cbo_Status"
        Me.cbo_Status.Size = New System.Drawing.Size(46, 21)
        Me.cbo_Status.TabIndex = 3
        Me.cbo_Status.Visible = False
        '
        'pnl_TimeLine
        '
        Me.pnl_TimeLine.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnl_TimeLine.Location = New System.Drawing.Point(387, 2)
        Me.pnl_TimeLine.Name = "pnl_TimeLine"
        Me.pnl_TimeLine.Size = New System.Drawing.Size(5, 31)
        Me.pnl_TimeLine.TabIndex = 4
        '
        'imgLst_Status
        '
        Me.imgLst_Status.ImageStream = CType(resources.GetObject("imgLst_Status.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgLst_Status.TransparentColor = System.Drawing.Color.Transparent
        Me.imgLst_Status.Images.SetKeyName(0, "stAtRisk.png")
        Me.imgLst_Status.Images.SetKeyName(1, "stComplete.png")
        Me.imgLst_Status.Images.SetKeyName(2, "stGood.png")
        Me.imgLst_Status.Images.SetKeyName(3, "stNotStarted.png")
        Me.imgLst_Status.Images.SetKeyName(4, "stOffTrack.png")
        Me.imgLst_Status.Images.SetKeyName(5, "stOnHold.png")
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(152, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(28, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Plan"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(152, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(37, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Actual"
        '
        'lbl_ActualStart
        '
        Me.lbl_ActualStart.AutoSize = True
        Me.lbl_ActualStart.Location = New System.Drawing.Point(189, 22)
        Me.lbl_ActualStart.Name = "lbl_ActualStart"
        Me.lbl_ActualStart.Size = New System.Drawing.Size(65, 13)
        Me.lbl_ActualStart.TabIndex = 8
        Me.lbl_ActualStart.Text = "55/55/5555"
        '
        'lbl_PlanStart
        '
        Me.lbl_PlanStart.AutoSize = True
        Me.lbl_PlanStart.Location = New System.Drawing.Point(189, 4)
        Me.lbl_PlanStart.Name = "lbl_PlanStart"
        Me.lbl_PlanStart.Size = New System.Drawing.Size(65, 13)
        Me.lbl_PlanStart.TabIndex = 7
        Me.lbl_PlanStart.Text = "55/55/5555"
        '
        'lbl_ActualEnd
        '
        Me.lbl_ActualEnd.AutoSize = True
        Me.lbl_ActualEnd.Location = New System.Drawing.Point(262, 22)
        Me.lbl_ActualEnd.Name = "lbl_ActualEnd"
        Me.lbl_ActualEnd.Size = New System.Drawing.Size(65, 13)
        Me.lbl_ActualEnd.TabIndex = 10
        Me.lbl_ActualEnd.Text = "55/55/5555"
        '
        'lbl_PlanEnd
        '
        Me.lbl_PlanEnd.AutoSize = True
        Me.lbl_PlanEnd.Location = New System.Drawing.Point(262, 4)
        Me.lbl_PlanEnd.Name = "lbl_PlanEnd"
        Me.lbl_PlanEnd.Size = New System.Drawing.Size(65, 13)
        Me.lbl_PlanEnd.TabIndex = 9
        Me.lbl_PlanEnd.Text = "55/55/5555"
        '
        'pic_Status
        '
        Me.pic_Status.Location = New System.Drawing.Point(333, 3)
        Me.pic_Status.Name = "pic_Status"
        Me.pic_Status.Size = New System.Drawing.Size(37, 33)
        Me.pic_Status.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pic_Status.TabIndex = 11
        Me.pic_Status.TabStop = False
        '
        'ctr_ProjTimeLine
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.pic_Status)
        Me.Controls.Add(Me.lbl_ActualEnd)
        Me.Controls.Add(Me.lbl_PlanEnd)
        Me.Controls.Add(Me.lbl_ActualStart)
        Me.Controls.Add(Me.lbl_PlanStart)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.pnl_TimeLine)
        Me.Controls.Add(Me.cbo_Status)
        Me.Controls.Add(Me.dtp_End)
        Me.Controls.Add(Me.dtp_Start)
        Me.Controls.Add(Me.lbl_Item)
        Me.Name = "ctr_ProjTimeLine"
        Me.Size = New System.Drawing.Size(393, 42)
        CType(Me.pic_Status, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbl_Item As System.Windows.Forms.Label
    Friend WithEvents dtp_Start As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtp_End As System.Windows.Forms.DateTimePicker
    Friend WithEvents cbo_Status As System.Windows.Forms.ComboBox
    Friend WithEvents pnl_TimeLine As System.Windows.Forms.Panel
    Friend WithEvents imgLst_Status As System.Windows.Forms.ImageList
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lbl_ActualStart As Label
    Friend WithEvents lbl_PlanStart As Label
    Friend WithEvents lbl_ActualEnd As Label
    Friend WithEvents lbl_PlanEnd As Label
    Friend WithEvents pic_Status As PictureBox
End Class
