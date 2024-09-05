<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctrl_CalendarSmall
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
        Me.lbl_Current = New System.Windows.Forms.Label()
        Me.pnl_CalDays = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btn_MthPrev = New System.Windows.Forms.Button()
        Me.btn_MthNext = New System.Windows.Forms.Button()
        Me.lbl_Today = New System.Windows.Forms.Label()
        Me.pnl_CalDays.SuspendLayout()
        Me.SuspendLayout()
        '
        'lbl_Current
        '
        Me.lbl_Current.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Current.Location = New System.Drawing.Point(28, 20)
        Me.lbl_Current.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_Current.Name = "lbl_Current"
        Me.lbl_Current.Size = New System.Drawing.Size(209, 37)
        Me.lbl_Current.TabIndex = 0
        Me.lbl_Current.Text = "SUN, DEC 31"
        Me.lbl_Current.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnl_CalDays
        '
        Me.pnl_CalDays.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnl_CalDays.Controls.Add(Me.Label9)
        Me.pnl_CalDays.Controls.Add(Me.Label8)
        Me.pnl_CalDays.Controls.Add(Me.Label7)
        Me.pnl_CalDays.Controls.Add(Me.Label6)
        Me.pnl_CalDays.Controls.Add(Me.Label5)
        Me.pnl_CalDays.Controls.Add(Me.Label4)
        Me.pnl_CalDays.Controls.Add(Me.Label3)
        Me.pnl_CalDays.Location = New System.Drawing.Point(4, 57)
        Me.pnl_CalDays.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pnl_CalDays.Name = "pnl_CalDays"
        Me.pnl_CalDays.Size = New System.Drawing.Size(254, 224)
        Me.pnl_CalDays.TabIndex = 2
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(217, 0)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(29, 17)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Sat"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(192, 0)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(24, 17)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "Fri"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(152, 0)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(38, 17)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Thur"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(109, 0)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(37, 17)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Wed"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(73, 0)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(33, 17)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Tue"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(36, -1)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(35, 17)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Mon"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(1, -1)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(33, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Sun"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btn_MthPrev
        '
        Me.btn_MthPrev.Location = New System.Drawing.Point(8, 22)
        Me.btn_MthPrev.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btn_MthPrev.Name = "btn_MthPrev"
        Me.btn_MthPrev.Size = New System.Drawing.Size(23, 28)
        Me.btn_MthPrev.TabIndex = 3
        Me.btn_MthPrev.Text = "<"
        Me.btn_MthPrev.UseVisualStyleBackColor = True
        '
        'btn_MthNext
        '
        Me.btn_MthNext.Location = New System.Drawing.Point(233, 22)
        Me.btn_MthNext.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btn_MthNext.Name = "btn_MthNext"
        Me.btn_MthNext.Size = New System.Drawing.Size(23, 28)
        Me.btn_MthNext.TabIndex = 4
        Me.btn_MthNext.Text = ">"
        Me.btn_MthNext.UseVisualStyleBackColor = True
        '
        'lbl_Today
        '
        Me.lbl_Today.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Today.Location = New System.Drawing.Point(11, 0)
        Me.lbl_Today.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_Today.Name = "lbl_Today"
        Me.lbl_Today.Size = New System.Drawing.Size(248, 18)
        Me.lbl_Today.TabIndex = 5
        Me.lbl_Today.Text = "SUN, DEC 31"
        Me.lbl_Today.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ctrl_CalendarSmall
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.lbl_Today)
        Me.Controls.Add(Me.btn_MthNext)
        Me.Controls.Add(Me.btn_MthPrev)
        Me.Controls.Add(Me.lbl_Current)
        Me.Controls.Add(Me.pnl_CalDays)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MinimumSize = New System.Drawing.Size(264, 0)
        Me.Name = "ctrl_CalendarSmall"
        Me.Size = New System.Drawing.Size(262, 285)
        Me.pnl_CalDays.ResumeLayout(False)
        Me.pnl_CalDays.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lbl_Current As Label
    Friend WithEvents pnl_CalDays As Panel
    Friend WithEvents Label9 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents btn_MthPrev As Button
    Friend WithEvents btn_MthNext As Button
    Friend WithEvents lbl_Today As Label
End Class
