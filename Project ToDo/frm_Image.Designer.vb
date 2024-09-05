<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_Image
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_Image))
        Me.tStrip_Picture = New System.Windows.Forms.ToolStrip()
        Me.tsBtn_Snippit = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsBtn_PasteImage = New System.Windows.Forms.ToolStripButton()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.rTxt_ImageNotes = New System.Windows.Forms.RichTextBox()
        Me.panel_ImageSpinner = New System.Windows.Forms.Panel()
        Me.label_Image = New System.Windows.Forms.Label()
        Me.label_ImageTotQty = New System.Windows.Forms.Label()
        Me.label_ImageNum = New System.Windows.Forms.Label()
        Me.btn_ImageNext = New System.Windows.Forms.Button()
        Me.btn_ImagePrev = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ts_System = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ts_Item = New System.Windows.Forms.ToolStripStatusLabel()
        Me.panel_Image = New System.Windows.Forms.Panel()
        Me.pic_PartImage = New System.Windows.Forms.PictureBox()
        Me.btn_ImageZoomFit = New System.Windows.Forms.Button()
        Me.btn_ImageZoomOut = New System.Windows.Forms.Button()
        Me.txt_ImageZoom = New System.Windows.Forms.TextBox()
        Me.btn_ImageZoomIn = New System.Windows.Forms.Button()
        Me.tStrip_Picture.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.panel_ImageSpinner.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.panel_Image.SuspendLayout()
        CType(Me.pic_PartImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tStrip_Picture
        '
        Me.tStrip_Picture.Dock = System.Windows.Forms.DockStyle.None
        Me.tStrip_Picture.ImageScalingSize = New System.Drawing.Size(30, 30)
        Me.tStrip_Picture.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsBtn_Snippit, Me.ToolStripSeparator2, Me.tsBtn_PasteImage})
        Me.tStrip_Picture.Location = New System.Drawing.Point(0, 0)
        Me.tStrip_Picture.Name = "tStrip_Picture"
        Me.tStrip_Picture.Size = New System.Drawing.Size(86, 37)
        Me.tStrip_Picture.TabIndex = 6
        Me.tStrip_Picture.Text = "ToolStrip2"
        '
        'tsBtn_Snippit
        '
        Me.tsBtn_Snippit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsBtn_Snippit.Image = CType(resources.GetObject("tsBtn_Snippit.Image"), System.Drawing.Image)
        Me.tsBtn_Snippit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsBtn_Snippit.Name = "tsBtn_Snippit"
        Me.tsBtn_Snippit.Size = New System.Drawing.Size(34, 34)
        Me.tsBtn_Snippit.Text = "ToolStripButton1"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 37)
        '
        'tsBtn_PasteImage
        '
        Me.tsBtn_PasteImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsBtn_PasteImage.Image = CType(resources.GetObject("tsBtn_PasteImage.Image"), System.Drawing.Image)
        Me.tsBtn_PasteImage.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsBtn_PasteImage.Name = "tsBtn_PasteImage"
        Me.tsBtn_PasteImage.Size = New System.Drawing.Size(34, 34)
        Me.tsBtn_PasteImage.Text = "ToolStripButton3"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 40)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.panel_Image)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.rTxt_ImageNotes)
        Me.SplitContainer1.Size = New System.Drawing.Size(1007, 478)
        Me.SplitContainer1.SplitterDistance = 348
        Me.SplitContainer1.TabIndex = 7
        '
        'rTxt_ImageNotes
        '
        Me.rTxt_ImageNotes.Location = New System.Drawing.Point(0, 0)
        Me.rTxt_ImageNotes.Name = "rTxt_ImageNotes"
        Me.rTxt_ImageNotes.Size = New System.Drawing.Size(1007, 132)
        Me.rTxt_ImageNotes.TabIndex = 0
        Me.rTxt_ImageNotes.Text = ""
        '
        'panel_ImageSpinner
        '
        Me.panel_ImageSpinner.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panel_ImageSpinner.Controls.Add(Me.label_Image)
        Me.panel_ImageSpinner.Controls.Add(Me.label_ImageTotQty)
        Me.panel_ImageSpinner.Controls.Add(Me.label_ImageNum)
        Me.panel_ImageSpinner.Controls.Add(Me.btn_ImageNext)
        Me.panel_ImageSpinner.Controls.Add(Me.btn_ImagePrev)
        Me.panel_ImageSpinner.Location = New System.Drawing.Point(845, 12)
        Me.panel_ImageSpinner.Name = "panel_ImageSpinner"
        Me.panel_ImageSpinner.Size = New System.Drawing.Size(162, 21)
        Me.panel_ImageSpinner.TabIndex = 8
        '
        'label_Image
        '
        Me.label_Image.AutoSize = True
        Me.label_Image.BackColor = System.Drawing.Color.Transparent
        Me.label_Image.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label_Image.ForeColor = System.Drawing.Color.CornflowerBlue
        Me.label_Image.Location = New System.Drawing.Point(131, 5)
        Me.label_Image.Name = "label_Image"
        Me.label_Image.Size = New System.Drawing.Size(14, 13)
        Me.label_Image.TabIndex = 4
        Me.label_Image.Text = "1"
        Me.label_Image.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'label_ImageTotQty
        '
        Me.label_ImageTotQty.AutoSize = True
        Me.label_ImageTotQty.BackColor = System.Drawing.Color.Transparent
        Me.label_ImageTotQty.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label_ImageTotQty.ForeColor = System.Drawing.Color.Black
        Me.label_ImageTotQty.Location = New System.Drawing.Point(47, 5)
        Me.label_ImageTotQty.Name = "label_ImageTotQty"
        Me.label_ImageTotQty.Size = New System.Drawing.Size(38, 13)
        Me.label_ImageTotQty.TabIndex = 3
        Me.label_ImageTotQty.Text = "of  55"
        '
        'label_ImageNum
        '
        Me.label_ImageNum.BackColor = System.Drawing.Color.Transparent
        Me.label_ImageNum.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label_ImageNum.ForeColor = System.Drawing.Color.Black
        Me.label_ImageNum.Location = New System.Drawing.Point(29, 5)
        Me.label_ImageNum.Name = "label_ImageNum"
        Me.label_ImageNum.Size = New System.Drawing.Size(21, 18)
        Me.label_ImageNum.TabIndex = 2
        Me.label_ImageNum.Text = "1"
        Me.label_ImageNum.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'btn_ImageNext
        '
        Me.btn_ImageNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_ImageNext.Location = New System.Drawing.Point(95, 1)
        Me.btn_ImageNext.Name = "btn_ImageNext"
        Me.btn_ImageNext.Size = New System.Drawing.Size(19, 19)
        Me.btn_ImageNext.TabIndex = 1
        Me.btn_ImageNext.Text = ">"
        Me.btn_ImageNext.UseVisualStyleBackColor = True
        '
        'btn_ImagePrev
        '
        Me.btn_ImagePrev.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_ImagePrev.Location = New System.Drawing.Point(6, 1)
        Me.btn_ImagePrev.Name = "btn_ImagePrev"
        Me.btn_ImagePrev.Size = New System.Drawing.Size(19, 19)
        Me.btn_ImagePrev.TabIndex = 0
        Me.btn_ImagePrev.Text = "<"
        Me.btn_ImagePrev.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ts_System, Me.ts_Item})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 521)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1010, 22)
        Me.StatusStrip1.TabIndex = 9
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ts_System
        '
        Me.ts_System.Name = "ts_System"
        Me.ts_System.Size = New System.Drawing.Size(53, 17)
        Me.ts_System.Text = "[System]"
        '
        'ts_Item
        '
        Me.ts_Item.Name = "ts_Item"
        Me.ts_Item.Size = New System.Drawing.Size(39, 17)
        Me.ts_Item.Text = "[Item]"
        '
        'panel_Image
        '
        Me.panel_Image.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panel_Image.AutoScroll = True
        Me.panel_Image.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.panel_Image.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panel_Image.Controls.Add(Me.pic_PartImage)
        Me.panel_Image.Location = New System.Drawing.Point(3, 3)
        Me.panel_Image.Name = "panel_Image"
        Me.panel_Image.Size = New System.Drawing.Size(1001, 342)
        Me.panel_Image.TabIndex = 12
        '
        'pic_PartImage
        '
        Me.pic_PartImage.Location = New System.Drawing.Point(2, 3)
        Me.pic_PartImage.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.pic_PartImage.Name = "pic_PartImage"
        Me.pic_PartImage.Size = New System.Drawing.Size(962, 306)
        Me.pic_PartImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pic_PartImage.TabIndex = 7
        Me.pic_PartImage.TabStop = False
        '
        'btn_ImageZoomFit
        '
        Me.btn_ImageZoomFit.Location = New System.Drawing.Point(528, 16)
        Me.btn_ImageZoomFit.Margin = New System.Windows.Forms.Padding(0)
        Me.btn_ImageZoomFit.Name = "btn_ImageZoomFit"
        Me.btn_ImageZoomFit.Size = New System.Drawing.Size(25, 21)
        Me.btn_ImageZoomFit.TabIndex = 17
        Me.btn_ImageZoomFit.Text = "O"
        Me.btn_ImageZoomFit.UseVisualStyleBackColor = True
        '
        'btn_ImageZoomOut
        '
        Me.btn_ImageZoomOut.Location = New System.Drawing.Point(485, 16)
        Me.btn_ImageZoomOut.Margin = New System.Windows.Forms.Padding(0)
        Me.btn_ImageZoomOut.Name = "btn_ImageZoomOut"
        Me.btn_ImageZoomOut.Size = New System.Drawing.Size(25, 21)
        Me.btn_ImageZoomOut.TabIndex = 16
        Me.btn_ImageZoomOut.Text = "-"
        Me.btn_ImageZoomOut.UseVisualStyleBackColor = True
        '
        'txt_ImageZoom
        '
        Me.txt_ImageZoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_ImageZoom.Location = New System.Drawing.Point(443, 17)
        Me.txt_ImageZoom.Name = "txt_ImageZoom"
        Me.txt_ImageZoom.Size = New System.Drawing.Size(39, 20)
        Me.txt_ImageZoom.TabIndex = 15
        '
        'btn_ImageZoomIn
        '
        Me.btn_ImageZoomIn.Location = New System.Drawing.Point(415, 16)
        Me.btn_ImageZoomIn.Margin = New System.Windows.Forms.Padding(0)
        Me.btn_ImageZoomIn.Name = "btn_ImageZoomIn"
        Me.btn_ImageZoomIn.Size = New System.Drawing.Size(25, 21)
        Me.btn_ImageZoomIn.TabIndex = 14
        Me.btn_ImageZoomIn.Text = "+"
        Me.btn_ImageZoomIn.UseVisualStyleBackColor = True
        '
        'frm_Image
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1010, 543)
        Me.Controls.Add(Me.btn_ImageZoomFit)
        Me.Controls.Add(Me.btn_ImageZoomOut)
        Me.Controls.Add(Me.txt_ImageZoom)
        Me.Controls.Add(Me.btn_ImageZoomIn)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.panel_ImageSpinner)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.tStrip_Picture)
        Me.Name = "frm_Image"
        Me.Text = "frm_Image"
        Me.tStrip_Picture.ResumeLayout(False)
        Me.tStrip_Picture.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.panel_ImageSpinner.ResumeLayout(False)
        Me.panel_ImageSpinner.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.panel_Image.ResumeLayout(False)
        CType(Me.pic_PartImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tStrip_Picture As System.Windows.Forms.ToolStrip
    Friend WithEvents tsBtn_Snippit As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsBtn_PasteImage As System.Windows.Forms.ToolStripButton
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents rTxt_ImageNotes As System.Windows.Forms.RichTextBox
    Friend WithEvents panel_ImageSpinner As System.Windows.Forms.Panel
    Friend WithEvents label_Image As System.Windows.Forms.Label
    Friend WithEvents label_ImageTotQty As System.Windows.Forms.Label
    Friend WithEvents label_ImageNum As System.Windows.Forms.Label
    Friend WithEvents btn_ImageNext As System.Windows.Forms.Button
    Friend WithEvents btn_ImagePrev As System.Windows.Forms.Button
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ts_System As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ts_Item As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents panel_Image As System.Windows.Forms.Panel
    Friend WithEvents pic_PartImage As System.Windows.Forms.PictureBox
    Friend WithEvents btn_ImageZoomFit As System.Windows.Forms.Button
    Friend WithEvents btn_ImageZoomOut As System.Windows.Forms.Button
    Friend WithEvents txt_ImageZoom As System.Windows.Forms.TextBox
    Friend WithEvents btn_ImageZoomIn As System.Windows.Forms.Button
End Class
