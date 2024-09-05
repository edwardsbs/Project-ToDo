Public Class frm_Image

    Private Sub tStrip_Picture_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles tStrip_Picture.ItemClicked
        
    End Sub

    Private Sub tsBtn_Snippit_Click(sender As Object, e As EventArgs) Handles tsBtn_Snippit.Click
        Dim bmp = SnippingTool.Snip()

        If bmp IsNot Nothing Then
            Me.pic_PartImage.Image = bmp 'Clipboard.GetImage
            'Me.tsBtn_Save.BackColor = Color.OrangeRed
        End If
    End Sub

    Private Sub tsBtn_PasteImage_Click(sender As Object, e As EventArgs) Handles tsBtn_PasteImage.Click
        If Clipboard.ContainsImage = True Then
            Me.pic_PartImage.Image = Clipboard.GetImage
            'frm_Main.tsbtn_Save.BackColor = Color.OrangeRed
        Else
            MessageBox.Show("No picture")
        End If
    End Sub

    Private Sub frm_Image_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '' ''Load Images & Image Notes
        ' ''pic_PartImage.Image = Nothing
        ' ''panel_ImageSpinner.Enabled = False
        ' ''If Not String.IsNullOrEmpty(drItem.Item("ImageIDs").ToString) Then
        ' ''    Get_Images(frm_Image)
        ' ''    label_ImageNum.Text = "1"
        ' ''    txt_ImageZoom.Text = "1"
        ' ''    Call Get_Pic()
        ' ''    rTxt_ImageNotes.Enabled = True
        ' ''Else
        ' ''    rTxt_ImageNotes.Enabled = False
        ' ''End If
    End Sub

    Sub Get_Pic()

        Me.pic_PartImage.Image = Nothing
        rtxt_ImageNotes.Text = ""

        If String.IsNullOrEmpty(label_ImageNum.Text) Then Exit Sub

        Try
            Dim srch As String = "Indx=" & label_ImageNum.Text
            If frm_Main.dtCurrImages.Select(srch).Any Then
                Dim dr As DataRow = frm_Main.dtCurrImages.Select(srch).CopyToDataTable.Rows(0)
                label_Image.Text = dr.Item("Id").ToString
                srch = "ImageID=" & label_Image.Text & ""

                If frm_Main.dtImages.Select(srch).Any Then
                    Dim iRow As DataRow = frm_Main.dtImages.Select(srch).CopyToDataTable.Rows(0)
                    If Not IsDBNull(iRow.Item("Image")) Then

                        Dim mybytearray As Byte() 'this should contain your data
                        mybytearray = iRow.Item("Image")
                        Dim myimage As Image
                        Dim ms As System.IO.MemoryStream = New System.IO.MemoryStream(mybytearray)
                        myimage = Image.FromStream(ms)

                        Dim imgWid As Long = myimage.Size.Width
                        Dim imgHgt As Long = myimage.Size.Height

                        pic_PartImage.Width = panel_Image.Width - 20
                        pic_PartImage.Height = panel_Image.Height - 20
                        'txt_ImageZoom.Text = "100%" 'zm '* 100 & "%"
                        'End If

                        Me.pic_PartImage.Image = Image.FromStream(ms) 'displayImage '


                        If Not IsDBNull(iRow.Item("ImageNotes")) Then rTxt_ImageNotes.Rtf = _
                            ConvertTextToRTF(iRow.Item("ImageNotes"))

                    End If
                End If
            End If

        Catch ex As Exception
            Me.pic_PartImage.Image = Nothing
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub btn_ImageNext_Click(sender As Object, e As EventArgs) Handles btn_ImageNext.Click
        If frm_Main.dtCurrImages.Rows.Count > 0 AndAlso CInt(label_ImageNum.Text) < frm_Main.dtCurrImages.Rows.Count Then
            label_ImageNum.Text = label_ImageNum.Text + 1
            Call Get_Pic()
        End If
    End Sub
End Class