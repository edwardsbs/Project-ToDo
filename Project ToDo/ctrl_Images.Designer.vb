<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctrl_Images
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ctrl_Images))
        Me.Panel8 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tStrip_Picture = New System.Windows.Forms.ToolStrip()
        Me.tsBtn_AddImage = New System.Windows.Forms.ToolStripButton()
        Me.tsBtn_ReplaceImage = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsBtn_Snippit = New System.Windows.Forms.ToolStripButton()
        Me.tsBtn_PasteImage = New System.Windows.Forms.ToolStripButton()
        Me.tsBtn_SaveNewImage = New System.Windows.Forms.ToolStripButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.panel_ImageSpinner = New System.Windows.Forms.Panel()
        Me.label_Image = New System.Windows.Forms.Label()
        Me.label_ImageTotQty = New System.Windows.Forms.Label()
        Me.label_ImageNum = New System.Windows.Forms.Label()
        Me.btn_ImageNext = New System.Windows.Forms.Button()
        Me.btn_ImagePrev = New System.Windows.Forms.Button()
        Me.lvw_Images = New System.Windows.Forms.ListView()
        Me.imgList_Images = New System.Windows.Forms.ImageList(Me.components)
        Me.btn_ImageZoomFit = New System.Windows.Forms.Button()
        Me.btn_ImageZoomOut = New System.Windows.Forms.Button()
        Me.panel_Image = New System.Windows.Forms.Panel()
        Me.pic_PartImage = New System.Windows.Forms.PictureBox()
        Me.txt_ImageZoom = New System.Windows.Forms.TextBox()
        Me.btn_ImageZoomIn = New System.Windows.Forms.Button()
        Me.rtxt_ImageNotes = New System.Windows.Forms.RichTextBox()
        Me.cms_ctrImageTools = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CopyImageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel8.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.tStrip_Picture.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.panel_ImageSpinner.SuspendLayout()
        Me.panel_Image.SuspendLayout()
        CType(Me.pic_PartImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cms_ctrImageTools.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel8
        '
        Me.Panel8.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel8.BackColor = System.Drawing.Color.White
        Me.Panel8.Controls.Add(Me.Panel4)
        Me.Panel8.Controls.Add(Me.Panel1)
        Me.Panel8.Location = New System.Drawing.Point(4, 4)
        Me.Panel8.Name = "Panel8"
        Me.Panel8.Size = New System.Drawing.Size(338, 421)
        Me.Panel8.TabIndex = 28
        '
        'Panel4
        '
        Me.Panel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel4.BackColor = System.Drawing.Color.White
        Me.Panel4.Controls.Add(Me.Label1)
        Me.Panel4.Controls.Add(Me.tStrip_Picture)
        Me.Panel4.Location = New System.Drawing.Point(3, 2)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(335, 48)
        Me.Panel4.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.DodgerBlue
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 19)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Images"
        '
        'tStrip_Picture
        '
        Me.tStrip_Picture.Dock = System.Windows.Forms.DockStyle.None
        Me.tStrip_Picture.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.tStrip_Picture.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsBtn_AddImage, Me.tsBtn_ReplaceImage, Me.ToolStripSeparator1, Me.tsBtn_Snippit, Me.tsBtn_PasteImage, Me.tsBtn_SaveNewImage})
        Me.tStrip_Picture.Location = New System.Drawing.Point(1, 20)
        Me.tStrip_Picture.Name = "tStrip_Picture"
        Me.tStrip_Picture.Size = New System.Drawing.Size(138, 27)
        Me.tStrip_Picture.TabIndex = 8
        Me.tStrip_Picture.Text = "ToolStrip2"
        '
        'tsBtn_AddImage
        '
        Me.tsBtn_AddImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsBtn_AddImage.Image = Global.Project_ToDo.My.Resources.Resources._1495073887_1
        Me.tsBtn_AddImage.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsBtn_AddImage.Name = "tsBtn_AddImage"
        Me.tsBtn_AddImage.Size = New System.Drawing.Size(24, 24)
        Me.tsBtn_AddImage.Text = "Add Image"
        '
        'tsBtn_ReplaceImage
        '
        Me.tsBtn_ReplaceImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsBtn_ReplaceImage.Image = CType(resources.GetObject("tsBtn_ReplaceImage.Image"), System.Drawing.Image)
        Me.tsBtn_ReplaceImage.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsBtn_ReplaceImage.Name = "tsBtn_ReplaceImage"
        Me.tsBtn_ReplaceImage.Size = New System.Drawing.Size(24, 24)
        Me.tsBtn_ReplaceImage.Text = "Replace Image"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 27)
        '
        'tsBtn_Snippit
        '
        Me.tsBtn_Snippit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsBtn_Snippit.Enabled = False
        Me.tsBtn_Snippit.Image = CType(resources.GetObject("tsBtn_Snippit.Image"), System.Drawing.Image)
        Me.tsBtn_Snippit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsBtn_Snippit.Name = "tsBtn_Snippit"
        Me.tsBtn_Snippit.Size = New System.Drawing.Size(24, 24)
        Me.tsBtn_Snippit.Text = "ToolStripButton1"
        '
        'tsBtn_PasteImage
        '
        Me.tsBtn_PasteImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsBtn_PasteImage.Enabled = False
        Me.tsBtn_PasteImage.Image = CType(resources.GetObject("tsBtn_PasteImage.Image"), System.Drawing.Image)
        Me.tsBtn_PasteImage.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsBtn_PasteImage.Name = "tsBtn_PasteImage"
        Me.tsBtn_PasteImage.Size = New System.Drawing.Size(24, 24)
        Me.tsBtn_PasteImage.Text = "ToolStripButton3"
        '
        'tsBtn_SaveNewImage
        '
        Me.tsBtn_SaveNewImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsBtn_SaveNewImage.Enabled = False
        Me.tsBtn_SaveNewImage.Image = Global.Project_ToDo.My.Resources.Resources.Data_storage_floppy_save_small
        Me.tsBtn_SaveNewImage.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsBtn_SaveNewImage.Name = "tsBtn_SaveNewImage"
        Me.tsBtn_SaveNewImage.Size = New System.Drawing.Size(24, 24)
        Me.tsBtn_SaveNewImage.Text = "Save New Image"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.panel_ImageSpinner)
        Me.Panel1.Controls.Add(Me.lvw_Images)
        Me.Panel1.Controls.Add(Me.btn_ImageZoomFit)
        Me.Panel1.Controls.Add(Me.btn_ImageZoomOut)
        Me.Panel1.Controls.Add(Me.panel_Image)
        Me.Panel1.Controls.Add(Me.txt_ImageZoom)
        Me.Panel1.Controls.Add(Me.btn_ImageZoomIn)
        Me.Panel1.Controls.Add(Me.rtxt_ImageNotes)
        Me.Panel1.Location = New System.Drawing.Point(3, 52)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(332, 369)
        Me.Panel1.TabIndex = 3
        '
        'panel_ImageSpinner
        '
        Me.panel_ImageSpinner.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panel_ImageSpinner.Controls.Add(Me.label_Image)
        Me.panel_ImageSpinner.Controls.Add(Me.label_ImageTotQty)
        Me.panel_ImageSpinner.Controls.Add(Me.label_ImageNum)
        Me.panel_ImageSpinner.Controls.Add(Me.btn_ImageNext)
        Me.panel_ImageSpinner.Controls.Add(Me.btn_ImagePrev)
        Me.panel_ImageSpinner.Location = New System.Drawing.Point(164, 1)
        Me.panel_ImageSpinner.Name = "panel_ImageSpinner"
        Me.panel_ImageSpinner.Size = New System.Drawing.Size(162, 21)
        Me.panel_ImageSpinner.TabIndex = 6
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
        Me.label_ImageTotQty.ForeColor = System.Drawing.Color.DodgerBlue
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
        Me.label_ImageNum.ForeColor = System.Drawing.Color.DodgerBlue
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
        'lvw_Images
        '
        Me.lvw_Images.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvw_Images.LargeImageList = Me.imgList_Images
        Me.lvw_Images.Location = New System.Drawing.Point(168, 8)
        Me.lvw_Images.MultiSelect = False
        Me.lvw_Images.Name = "lvw_Images"
        Me.lvw_Images.Size = New System.Drawing.Size(160, 178)
        Me.lvw_Images.StateImageList = Me.imgList_Images
        Me.lvw_Images.TabIndex = 9
        Me.lvw_Images.UseCompatibleStateImageBehavior = False
        Me.lvw_Images.Visible = False
        '
        'imgList_Images
        '
        Me.imgList_Images.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.imgList_Images.ImageSize = New System.Drawing.Size(128, 128)
        Me.imgList_Images.TransparentColor = System.Drawing.Color.Transparent
        '
        'btn_ImageZoomFit
        '
        Me.btn_ImageZoomFit.BackgroundImage = Global.Project_ToDo.My.Resources.Resources.Fit_to_Window
        Me.btn_ImageZoomFit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btn_ImageZoomFit.Location = New System.Drawing.Point(119, 1)
        Me.btn_ImageZoomFit.Margin = New System.Windows.Forms.Padding(0)
        Me.btn_ImageZoomFit.Name = "btn_ImageZoomFit"
        Me.btn_ImageZoomFit.Size = New System.Drawing.Size(25, 21)
        Me.btn_ImageZoomFit.TabIndex = 13
        Me.btn_ImageZoomFit.Tag = "Fit to Window"
        Me.btn_ImageZoomFit.UseVisualStyleBackColor = True
        '
        'btn_ImageZoomOut
        '
        Me.btn_ImageZoomOut.BackgroundImage = Global.Project_ToDo.My.Resources.Resources.Image_Size_Decrease
        Me.btn_ImageZoomOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btn_ImageZoomOut.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_ImageZoomOut.Location = New System.Drawing.Point(6, 1)
        Me.btn_ImageZoomOut.Margin = New System.Windows.Forms.Padding(0)
        Me.btn_ImageZoomOut.Name = "btn_ImageZoomOut"
        Me.btn_ImageZoomOut.Size = New System.Drawing.Size(25, 22)
        Me.btn_ImageZoomOut.TabIndex = 12
        Me.btn_ImageZoomOut.UseVisualStyleBackColor = True
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
        Me.panel_Image.Location = New System.Drawing.Point(3, 23)
        Me.panel_Image.Name = "panel_Image"
        Me.panel_Image.Size = New System.Drawing.Size(324, 239)
        Me.panel_Image.TabIndex = 11
        '
        'pic_PartImage
        '
        Me.pic_PartImage.ContextMenuStrip = Me.cms_ctrImageTools
        Me.pic_PartImage.Location = New System.Drawing.Point(2, 4)
        Me.pic_PartImage.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.pic_PartImage.Name = "pic_PartImage"
        Me.pic_PartImage.Size = New System.Drawing.Size(316, 230)
        Me.pic_PartImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pic_PartImage.TabIndex = 7
        Me.pic_PartImage.TabStop = False
        '
        'txt_ImageZoom
        '
        Me.txt_ImageZoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_ImageZoom.Location = New System.Drawing.Point(34, 2)
        Me.txt_ImageZoom.Name = "txt_ImageZoom"
        Me.txt_ImageZoom.Size = New System.Drawing.Size(39, 20)
        Me.txt_ImageZoom.TabIndex = 10
        '
        'btn_ImageZoomIn
        '
        Me.btn_ImageZoomIn.BackgroundImage = Global.Project_ToDo.My.Resources.Resources.Image_Size_Increase
        Me.btn_ImageZoomIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btn_ImageZoomIn.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_ImageZoomIn.Location = New System.Drawing.Point(76, 1)
        Me.btn_ImageZoomIn.Margin = New System.Windows.Forms.Padding(0)
        Me.btn_ImageZoomIn.Name = "btn_ImageZoomIn"
        Me.btn_ImageZoomIn.Size = New System.Drawing.Size(25, 22)
        Me.btn_ImageZoomIn.TabIndex = 9
        Me.btn_ImageZoomIn.UseVisualStyleBackColor = True
        '
        'rtxt_ImageNotes
        '
        Me.rtxt_ImageNotes.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtxt_ImageNotes.Location = New System.Drawing.Point(3, 268)
        Me.rtxt_ImageNotes.Name = "rtxt_ImageNotes"
        Me.rtxt_ImageNotes.Size = New System.Drawing.Size(325, 96)
        Me.rtxt_ImageNotes.TabIndex = 4
        Me.rtxt_ImageNotes.Text = ""
        '
        'cms_ctrImageTools
        '
        Me.cms_ctrImageTools.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyImageToolStripMenuItem})
        Me.cms_ctrImageTools.Name = "cms_ctrImageTools"
        Me.cms_ctrImageTools.Size = New System.Drawing.Size(153, 48)
        '
        'CopyImageToolStripMenuItem
        '
        Me.CopyImageToolStripMenuItem.Name = "CopyImageToolStripMenuItem"
        Me.CopyImageToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.CopyImageToolStripMenuItem.Text = "Copy Image"
        '
        'ctrl_Images
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.DodgerBlue
        Me.Controls.Add(Me.Panel8)
        Me.Name = "ctrl_Images"
        Me.Size = New System.Drawing.Size(347, 430)
        Me.Panel8.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.tStrip_Picture.ResumeLayout(False)
        Me.tStrip_Picture.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.panel_ImageSpinner.ResumeLayout(False)
        Me.panel_ImageSpinner.PerformLayout()
        Me.panel_Image.ResumeLayout(False)
        CType(Me.pic_PartImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cms_ctrImageTools.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel8 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents panel_ImageSpinner As System.Windows.Forms.Panel
    Friend WithEvents label_Image As System.Windows.Forms.Label
    Friend WithEvents label_ImageTotQty As System.Windows.Forms.Label
    Friend WithEvents label_ImageNum As System.Windows.Forms.Label
    Friend WithEvents btn_ImageNext As System.Windows.Forms.Button
    Friend WithEvents btn_ImagePrev As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tStrip_Picture As System.Windows.Forms.ToolStrip
    Friend WithEvents tsBtn_AddImage As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsBtn_Snippit As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsBtn_PasteImage As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsBtn_SaveNewImage As System.Windows.Forms.ToolStripButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btn_ImageZoomFit As System.Windows.Forms.Button
    Friend WithEvents btn_ImageZoomOut As System.Windows.Forms.Button
    Friend WithEvents panel_Image As System.Windows.Forms.Panel
    Friend WithEvents pic_PartImage As System.Windows.Forms.PictureBox
    Friend WithEvents txt_ImageZoom As System.Windows.Forms.TextBox
    Friend WithEvents btn_ImageZoomIn As System.Windows.Forms.Button
    Friend WithEvents rtxt_ImageNotes As System.Windows.Forms.RichTextBox
    Friend WithEvents lvw_Images As System.Windows.Forms.ListView
    Friend WithEvents imgList_Images As System.Windows.Forms.ImageList
    Friend WithEvents tsBtn_ReplaceImage As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cms_ctrImageTools As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents CopyImageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
