<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_NewItem
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
        Me.btn_AddNew = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txt_Item = New System.Windows.Forms.TextBox()
        Me.combo_Parent = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.combo_System = New System.Windows.Forms.ComboBox()
        Me.txt_Path = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.chk_AddPhases = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btn_AddNew
        '
        Me.btn_AddNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_AddNew.Location = New System.Drawing.Point(324, 143)
        Me.btn_AddNew.Name = "btn_AddNew"
        Me.btn_AddNew.Size = New System.Drawing.Size(103, 23)
        Me.btn_AddNew.TabIndex = 2
        Me.btn_AddNew.Text = "Add New Item"
        Me.btn_AddNew.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(4, 117)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(27, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Item"
        '
        'txt_Item
        '
        Me.txt_Item.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txt_Item.BackColor = System.Drawing.Color.LemonChiffon
        Me.txt_Item.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_Item.Location = New System.Drawing.Point(48, 114)
        Me.txt_Item.Name = "txt_Item"
        Me.txt_Item.Size = New System.Drawing.Size(379, 20)
        Me.txt_Item.TabIndex = 0
        '
        'combo_Parent
        '
        Me.combo_Parent.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.combo_Parent.BackColor = System.Drawing.Color.LemonChiffon
        Me.combo_Parent.Enabled = False
        Me.combo_Parent.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.combo_Parent.FormattingEnabled = True
        Me.combo_Parent.Location = New System.Drawing.Point(48, 60)
        Me.combo_Parent.Name = "combo_Parent"
        Me.combo_Parent.Size = New System.Drawing.Size(235, 21)
        Me.combo_Parent.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Enabled = False
        Me.Label1.Location = New System.Drawing.Point(4, 63)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Parent"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(4, 36)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(41, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "System"
        '
        'combo_System
        '
        Me.combo_System.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.combo_System.BackColor = System.Drawing.Color.LemonChiffon
        Me.combo_System.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.combo_System.FormattingEnabled = True
        Me.combo_System.Location = New System.Drawing.Point(48, 33)
        Me.combo_System.Name = "combo_System"
        Me.combo_System.Size = New System.Drawing.Size(235, 21)
        Me.combo_System.TabIndex = 3
        '
        'txt_Path
        '
        Me.txt_Path.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txt_Path.BackColor = System.Drawing.Color.LemonChiffon
        Me.txt_Path.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_Path.Location = New System.Drawing.Point(47, 85)
        Me.txt_Path.Name = "txt_Path"
        Me.txt_Path.Size = New System.Drawing.Size(379, 20)
        Me.txt_Path.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 88)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(29, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Path"
        '
        'chk_AddPhases
        '
        Me.chk_AddPhases.AutoSize = True
        Me.chk_AddPhases.Location = New System.Drawing.Point(48, 147)
        Me.chk_AddPhases.Name = "chk_AddPhases"
        Me.chk_AddPhases.Size = New System.Drawing.Size(119, 17)
        Me.chk_AddPhases.TabIndex = 1
        Me.chk_AddPhases.Text = "Add Project Phases"
        Me.chk_AddPhases.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri Light", 11.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(4, 5)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(395, 18)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "* If NEW System, type system name into the System combo box."
        '
        'frm_NewItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(439, 171)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.chk_AddPhases)
        Me.Controls.Add(Me.txt_Path)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.combo_System)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.combo_Parent)
        Me.Controls.Add(Me.txt_Item)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btn_AddNew)
        Me.Name = "frm_NewItem"
        Me.Text = "frm_NewItem"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btn_AddNew As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txt_Item As System.Windows.Forms.TextBox
    Friend WithEvents combo_Parent As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents combo_System As System.Windows.Forms.ComboBox
    Friend WithEvents txt_Path As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents chk_AddPhases As System.Windows.Forms.CheckBox
    Friend WithEvents Label5 As Label
End Class
