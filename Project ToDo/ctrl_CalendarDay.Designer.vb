<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctrl_CalendarDay
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
        Me.lbl_Day = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lbl_Day
        '
        Me.lbl_Day.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Day.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Day.Name = "lbl_Day"
        Me.lbl_Day.Size = New System.Drawing.Size(26, 18)
        Me.lbl_Day.TabIndex = 0
        Me.lbl_Day.Text = "28"
        Me.lbl_Day.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.lbl_Day.Visible = False
        '
        'ctrl_CalendarDay
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.lbl_Day)
        Me.Name = "ctrl_CalendarDay"
        Me.Size = New System.Drawing.Size(27, 28)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lbl_Day As Label
End Class
