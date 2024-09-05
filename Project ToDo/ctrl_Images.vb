Imports System.IO
Imports System.Data.SqlClient

Public Class ctrl_Images
    Public dtImagesctrl As DataTable
    Public Sys As String
    Public Par As String
    Public Itm As String
    Private m_PanStartPoint As New Point
    Public imgReplace As Boolean = False


    Private Sub btn_ImageZoomIn_Click(sender As Object, e As EventArgs) Handles btn_ImageZoomIn.Click
        pic_PartImage.Width = pic_PartImage.Width + 50
        pic_PartImage.Height = pic_PartImage.Height + 50 '* zm

        txt_ImageZoom.Text = Math.Round(pic_PartImage.Width / (panel_Image.Width - 20), 2) * 100 & "%"

        panel_Image.Refresh()
    End Sub

    Private Sub txt_ImageZoom_TextChanged(sender As Object, e As EventArgs) Handles txt_ImageZoom.TextAlignChanged

    End Sub

    Private Sub pic_PartImage_MouseWheel(sender As Object, e As MouseEventArgs) Handles pic_PartImage.MouseWheel

        Dim scrlPnt As New Drawing.Point(panel_Image.AutoScrollPosition.X, panel_Image.AutoScrollPosition.Y)

        'check to see if mousepointer is over the picturebox
        'check if control is being held down
        If My.Computer.Keyboard.CtrlKeyDown Or My.Computer.Keyboard.ShiftKeyDown Then

            '1) Give the picturebox focus first

            panel_Image.SuspendLayout()
            pic_PartImage.Select()
            'panel_Image.AutoScrollPosition = New Drawing.Point(-scrlPnt.X, -scrlPnt.Y)
            panel_Image.ResumeLayout()

            '2) Scroll
            If e.Delta < 0 Then 'scroll down
                'scrlPnt = New Drawing.Point(panel_Image.AutoScrollPosition.X, panel_Image.AutoScrollPosition.Y)
                scrlPnt = New Drawing.Point(panel_Image.AutoScrollPosition.X, panel_Image.AutoScrollPosition.Y + 120)
                pic_PartImage.Width = pic_PartImage.Width - 20
                pic_PartImage.Height = pic_PartImage.Height - 20
                txt_ImageZoom.Text = Math.Round(pic_PartImage.Width / (panel_Image.Width - 20), 2) * 100 & "%"
                'panel_Image.AutoScrollPosition = New Drawing.Point(-scrlPnt.X, -scrlPnt.Y)
                'panel_Image.Refresh()
            ElseIf e.Delta > 0 Then 'scroll up
                'scrlPnt = New Drawing.Point(panel_Image.AutoScrollPosition.X, panel_Image.AutoScrollPosition.Y)
                scrlPnt = New Drawing.Point(panel_Image.AutoScrollPosition.X, panel_Image.AutoScrollPosition.Y - 120)
                pic_PartImage.Width = pic_PartImage.Width + 20
                pic_PartImage.Height = pic_PartImage.Height + 20
                txt_ImageZoom.Text = Math.Round(pic_PartImage.Width / (panel_Image.Width - 20), 2) * 100 & "%"
                'panel_Image.AutoScrollPosition = New Drawing.Point(-scrlPnt.X, -scrlPnt.Y)
                'panel_Image.Refresh()
            End If

        End If
        'End If

        panel_Image.AutoScrollPosition = New Drawing.Point(-scrlPnt.X, -scrlPnt.Y)
    End Sub

    Private Sub pic_PartImage_MouseMove(sender As Object, e As MouseEventArgs) Handles pic_PartImage.MouseMove
        'Verify Left Button is pressed while the mouse is moving
        If e.Button = Windows.Forms.MouseButtons.Left Then

            'Here we get the change in coordinates.
            Dim DeltaX As Integer = (m_PanStartPoint.X - e.X)
            Dim DeltaY As Integer = (m_PanStartPoint.Y - e.Y)

            'Then we set the new autoscroll position.
            'ALWAYS pass positive integers to the panels autoScrollPosition method
            panel_Image.AutoScrollPosition = _
            New Drawing.Point((DeltaX - panel_Image.AutoScrollPosition.X), _
                            (DeltaY - panel_Image.AutoScrollPosition.Y))
        End If
    End Sub

    Private Sub pic_PartImage_MouseEnter(sender As Object, e As EventArgs) Handles pic_PartImage.MouseEnter

    End Sub

    Private Sub pic_PartImage_MouseDown(sender As Object, e As MouseEventArgs) Handles pic_PartImage.MouseDown
        m_PanStartPoint = New Point(e.X, e.Y)
    End Sub

    Private Sub pic_PartImage_DoubleClick(sender As Object, e As EventArgs) Handles pic_PartImage.DoubleClick

    End Sub

    Private Sub pic_PartImage_Click(sender As Object, e As EventArgs) Handles pic_PartImage.Click
        'When image clicked, Set the focus to the picturebox
        Dim scrlPnt As New Drawing.Point(panel_Image.AutoScrollPosition.X, panel_Image.AutoScrollPosition.Y)
        panel_Image.SuspendLayout()
        pic_PartImage.Select()
        panel_Image.AutoScrollPosition = New Drawing.Point(-scrlPnt.X, -scrlPnt.Y)
        panel_Image.ResumeLayout()
    End Sub
    Private Sub btn_ImageZoomOut_Click(sender As Object, e As EventArgs) Handles btn_ImageZoomOut.Click
        pic_PartImage.Width = pic_PartImage.Width - 50
        pic_PartImage.Height = pic_PartImage.Height - 50 '* zm

        txt_ImageZoom.Text = Math.Round(pic_PartImage.Width / (panel_Image.Width - 20), 2) * 100 & "%"

        panel_Image.Refresh()
    End Sub

    Private Sub btn_ImageZoomFit_Click(sender As Object, e As EventArgs) Handles btn_ImageZoomFit.Click
        pic_PartImage.Width = panel_Image.Width - 20
        pic_PartImage.Height = panel_Image.Height - 20
        txt_ImageZoom.Text = "100%"
    End Sub

    Private Sub tsBtn_SaveNewImage_Click(sender As Object, e As EventArgs) Handles tsBtn_SaveNewImage.Click


        'Get Next Image ID Number
        Dim imgID As Integer = 1

        If imgReplace = True Then
            imgID = CInt(label_Image.Text)
        Else
            Dim dtMax As New DataTable
            Dim Sql As String = "SELECT Max(ImageID) FROM tbl_ToDoImages " '& _
            '"WHERE Architect ='" & ss_User.Text & "' "
            Call Load_Data(Sql, dtMax)

            If dtMax.Rows.Count > 0 Then
                imgID = CInt(dtMax.Rows(0).Item(0)) + 1
            End If
        End If

        label_Image.Text = imgID

        'Save Image
        Call Save_Image(imgID)

        'Write Image ID to ToDo's table
        If imgReplace = False Then 'only rewrite the Image ID List string if it is a new item (not a replacement)
            Dim strID As String = imgID.ToString
            Dim strSrch As String = "System='" & Sys & "' AND Item='" & Itm & "' AND [Parent]='" & Par & "'"
            If frm_Main.dsSys.Tables("dtItems").Select(strSrch).Any Then
                Dim dr As DataRow = frm_Main.dsSys.Tables("dtItems").Select(strSrch).CopyToDataTable.Rows(0)
                If Not String.IsNullOrEmpty(dr.Item("ImageIDs").ToString) Then
                    strID = dr.Item("ImageIDs").ToString & ", " & imgID.ToString
                End If
            End If

            Dim uCmd As New SqlCommand
            uCmd.CommandText = "UPDATE tbl_ToDoItems SET ImageIDs='" & strID & "' " & _
                "WHERE System='" & Sys & "' AND Item='" & Itm & "' AND [Parent]='" & Par & "'"
            Call WriteUpdateSQL(uCmd)
            uCmd.Dispose()

        End If

        Call frm_Main.Load_Images()

        tsBtn_AddImage.Enabled = True
        tsBtn_SaveNewImage.Enabled = False
        tsBtn_Snippit.Enabled = False
        tsBtn_PasteImage.Enabled = False
        imgReplace = False

    End Sub

    Private Sub tsBtn_PasteImage_Click(sender As Object, e As EventArgs) Handles tsBtn_PasteImage.Click
        If Clipboard.ContainsImage = True Then
            Me.pic_PartImage.Image = Clipboard.GetImage
            tsBtn_AddImage.Enabled = False
            'frm_Main.tsbtn_Save.BackColor = Color.OrangeRed
            tsBtn_SaveNewImage.Enabled = True
        Else
            MessageBox.Show("No picture")
        End If
    End Sub

    Private Sub tsBtn_Snippit_Click(sender As Object, e As EventArgs) Handles tsBtn_Snippit.Click
        Dim bmp = SnippingTool.Snip()

        If bmp IsNot Nothing Then
            Me.pic_PartImage.Image = bmp 'Clipboard.GetImage
            tsBtn_AddImage.Enabled = False
            'Me.tsBtn_Save.BackColor = Color.OrangeRed
            tsBtn_SaveNewImage.Enabled = True
        End If
    End Sub

    Private Sub tsBtn_AddImage_Click(sender As Object, e As EventArgs) Handles tsBtn_AddImage.Click
        pic_PartImage.Image = Nothing
        tsBtn_Snippit.Enabled = True
        tsBtn_PasteImage.Enabled = True
    End Sub

    Private Sub btn_ImagePrev_Click(sender As Object, e As EventArgs) Handles btn_ImagePrev.Click

        If dtImagesctrl.Rows.Count > 0 AndAlso CInt(label_ImageNum.Text) > 1 Then
            label_ImageNum.Text = label_ImageNum.Text - 1
            Call Get_Pic() 'label_ImageNum.Text)
        End If

    End Sub

    Private Sub btn_ImageNext_Click(sender As Object, e As EventArgs) Handles btn_ImageNext.Click

        If dtImagesctrl.Rows.Count > 0 AndAlso CInt(label_ImageNum.Text) < dtImagesctrl.Rows.Count Then
            label_ImageNum.Text = label_ImageNum.Text + 1
            Call Get_Pic() 'label_ImageNum.Text)
        End If


        'Call Set_FileListView(lvw_Images, frm_Main.dtCurrImages)
        'lvw_Images.LargeImageList = imgList_Images
        'lvw_Images.BringToFront()
        'lvw_Images.Visible = True

    End Sub


    Sub Set_FileListView(lv As ListView, dtImages As DataTable) ', Paths() As String)

        imgList_Images.Images.Clear()

        'lv.Clear()
        'lv.View = View.Details 'Set View of ListView

        '' Add ListView Columns With Specified Width
        'lv.Columns.Add("id", 15, HorizontalAlignment.Left) 'File Name
        ''lv.Columns.Add("Found", 60, HorizontalAlignment.Left)
        ''lv.Columns.Add("Date Modified", 150, HorizontalAlignment.Left)
        ''lv.Columns.Add("Path", 150, HorizontalAlignment.Left)

        'Dim exeIcon As Icon
        'Dim exePath As String = ""

        For Each dImg As DataRow In dtImages.Rows

            'If Not System.IO.Path.GetExtension(fPath) = String.Empty Then
            '    'Use the function to get the path to the executable for the file
            '    exePath = GetAssociatedProgram(System.IO.Path.GetExtension(fPath))

            '    'Use ExtractAssociatedIcon to get an icon from the path
            '    exeIcon = Drawing.Icon.ExtractAssociatedIcon(exePath)

            '    'Add the icon if we haven't got it already, with the executable path as the key
            '    If imgList_Images.Images.ContainsKey(exePath) Then
            '    Else
            '        imgList_Images.Images.Add(exePath, exeIcon)
            '    End If
            'End If

            If Not IsDBNull(dImg.Item("id")) Then
                Dim xImg As DataRow = frm_Main.dtImages.Select("ImageID=" & dImg.Item("id")).CopyToDataTable.Rows(0)
                'frm_Main.AddImages(fPath, frm_Main.imgList_fileIcons) 'Add Icons
                'Add Files & File Properties To ListView
                'Add the file to the ListView, with the executable path as the key to the ImageList's image
                'If Not String.IsNullOrEmpty(exePath) Then lv.Items.Add(System.IO.Path.GetFileName(fPath), exeIcon) Else lv.Items.Add(System.IO.Path.GetFileName(fPath))

                Dim mybytearray As Byte() 'this should contain your data
                mybytearray = xImg.Item("Image")
                Dim myimage As Image
                Dim ms As System.IO.MemoryStream = New System.IO.MemoryStream(mybytearray)
                myimage = Image.FromStream(ms)

                'Dim strm As New MemoryStream(CType(xImg.Item("Image")("Icon"), Byte()))
                'Dim strm As New IO.MemoryStream(xImg.Item("Image")("Icon"), Byte())
                imgList_Images.Images.Add(xImg.Item("ImageID"), myimage)
                'lv.Items.Add(xImg.Item("ImageID"), xImg.Item("ImageID"))
                'lv.Items(lv.Items.Count - 1).SubItems.Add(If(System.IO.File.Exists(fPath), "True", "False"))
                'lv.Items(lv.Items.Count - 1).SubItems.Add(System.IO.File.GetLastWriteTime(fPath).ToString)
                'lv.Items(lv.Items.Count - 1).SubItems.Add(fPath)
            End If
        Next

    End Sub

    Public Shared Function BytesToIcon(bytes As Byte()) As Icon
        Using ms As New MemoryStream(bytes)
            Return New Icon(ms)
        End Using
    End Function

    Private Sub ctrl_Images_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        'Check if key pressed is Ctrl and then make sure the mouse is hovering over the picturebox
        '  If so, give the picturebox the focus
        '  Otherwise, it will give it the focus every time you cross over the picturebox which makes it difficult to select text for formatting and click the richtext format buttons without losing focuse on the rTxt_Image control

        'If e.Control Then
        '    If FindControlAtCursor(sender.name).Name = "pic_PartImage" Then
        '        Dim scrlPnt As New Drawing.Point(panel_Image.AutoScrollPosition.X, panel_Image.AutoScrollPosition.Y)
        '        panel_Image.SuspendLayout()

        '        pic_PartImage.Select()

        '        panel_Image.AutoScrollPosition = New Drawing.Point(-scrlPnt.X, -scrlPnt.Y)
        '        panel_Image.ResumeLayout()
        '    End If
        'End If

    End Sub

    Function FindControlAtPoint(container As Control, pos As Point) As Control
        Dim child As Control
        For Each c As Control In container.Controls
            If c.Visible AndAlso c.Bounds.Contains(pos) Then
                child = FindControlAtPoint(c, New Point(pos.X - c.Left, pos.Y - c.Top))
                If child Is Nothing Then
                    Return c
                Else
                    Return child
                End If
            End If
        Next
        Return Nothing
    End Function

    Function FindControlAtCursor(frm As Form) As Control
        Dim pos As Point = Cursor.Position
        If frm.Bounds.Contains(pos) Then
            Return FindControlAtPoint(frm, frm.PointToClient(pos))
        End If
        Return Nothing
    End Function

    'Sub Set_FileListViewNoSrch(lv As ListView, Paths() As String, ByVal imglst As ImageList)

    '    lv.Clear()
    '    lv.View = View.LargeIcon 'Set View of ListView

    '    ' Add ListView Columns With Specified Width
    '    lv.Columns.Add("File Name", 200, HorizontalAlignment.Left) 'File Name
    '    'lv.Columns.Add("Found", 60, HorizontalAlignment.Left)
    '    lv.Columns.Add("Date Modified", 150, HorizontalAlignment.Left)
    '    lv.Columns.Add("Path", 150, HorizontalAlignment.Left)

    '    ''Dim exeIcon As Icon
    '    ''Dim exePath As String = ""

    '    For Each fPath In Paths

    '        If Not System.IO.Path.GetExtension(fPath) = String.Empty Then

    '        Dim iRow As DataRow = dtImages.Select(srch).CopyToDataTable.Rows(0)
    '        If Not IsDBNull(iRow.Item("Image")) Then

    '            Dim mybytearray As Byte() 'this should contain your data
    '            mybytearray = iRow.Item("Image")
    '            Dim myimage As Image
    '            Dim ms As System.IO.MemoryStream = New System.IO.MemoryStream(mybytearray)
    '            myimage = Image.FromStream(ms)

    '            Dim imgWid As Long = myimage.Size.Width
    '            Dim imgHgt As Long = myimage.Size.Height

    '            pic_PartImage.Width = panel_Image.Width - 20
    '            pic_PartImage.Height = panel_Image.Height - 20
    '            txt_ImageZoom.Text = "100%" 'zm '* 100 & "%"
    '            'End If

    '            Me.pic_PartImage.Image = Image.FromStream(ms)
    '            imglst.Images.Add(Image.FromStream(ms))

    '            If Not String.IsNullOrEmpty(fPath) Then
    '                If Not IsDBNull(fPath) Then
    '                    'frm_Main.AddImages(fPath, frm_Main.imgList_fileIcons) 'Add Icons
    '                    'Add Files & File Properties To ListView
    '                    'Add the file to the ListView, with the executable path as the key to the ImageList's image
    '                    'If Not String.IsNullOrEmpty(exePath) Then lv.Items.Add(System.IO.Path.GetFileName(fPath), exeIcon) Else lv.Items.Add(System.IO.Path.GetFileName(fPath))

    '                    lv.Items.Add(System.IO.Path.GetFileName(fPath), exePath)
    '                    'lv.Items(lv.Items.Count - 1).SubItems.Add(If(System.IO.File.Exists(fPath), "True", "False"))
    '                    lv.Items(lv.Items.Count - 1).SubItems.Add(System.IO.File.GetLastWriteTime(fPath).ToString)
    '                    lv.Items(lv.Items.Count - 1).SubItems.Add(fPath)
    '                End If
    '                End If
    '            End If
    '        End If
    '    Next

    'End Sub


    Private Sub ctrl_Images_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Sub Get_Images(xSys As String, xPar As String, xItm As String)
        'Get ImageID List from datatable 
        Sys = xSys
        Par = xPar
        Itm = xItm

        pic_PartImage.Image = Nothing
        panel_ImageSpinner.Enabled = False
        rtxt_ImageNotes.Text = ""
        rtxt_ImageNotes.Enabled = False
        tsBtn_ReplaceImage.Enabled = False
        imgReplace = False

        dtImagesctrl = New DataTable
        dtImagesctrl.Columns.Add("Indx", GetType(Integer))
        dtImagesctrl.Columns.Add("Id", GetType(Integer))

        'Dim NewString As String() = tvw_Items.SelectedNode.FullPath.Split(New String() {"\"}, StringSplitOptions.None)

        Dim strSrch As String = "System='" & xSys & "' AND Item='" & xItm & "' AND [Parent]='" & xPar & "'"
        If frm_Main.dsSys.Tables("dtItems").Select(strSrch).Any Then
            Dim dr As DataRow = frm_Main.dsSys.Tables("dtItems").Select(strSrch).CopyToDataTable.Rows(0)
            If Not String.IsNullOrEmpty(dr.Item("ImageIDs").ToString) Then
                ' Split string based on commas.
                Dim IDs As String() = dr.Item("ImageIDs").ToString.Split(New Char() {","c})

                ' Use For Each loop over words and display them.
                Dim ID As String
                Dim i As Integer = 1
                For Each ID In IDs
                    dtImagesctrl.Rows.Add(i, ID)
                    i += 1
                Next

                rtxt_ImageNotes.Enabled = True
                label_ImageNum.Text = "1"
                txt_ImageZoom.Text = "1"
                Get_Pic() 'ctrImg_Tasks.label_ImageNum.Text)
                tsBtn_ReplaceImage.Enabled = True 'turn 'Replace Image' feature only when an image is added.

            End If

            If dtImagesctrl.Rows.Count > 1 Then
                panel_ImageSpinner.Enabled = True
                label_ImageTotQty.Text = "of  " & dtImagesctrl.Rows.Count
            Else
                panel_ImageSpinner.Enabled = False
                label_ImageTotQty.Text = "of  1"
            End If

        End If

    End Sub

    Sub Save_Image(id As Integer)
        Try
            'Convert Image
            'Dim NewString As String() = tvw_Items.SelectedNode.FullPath.Split(New String() {"\"}, StringSplitOptions.None)

            'Dim img = ConvertImage(Me.pic_PartImage.Image)

            'Then Write it to SQL Server
            Dim iCmd As New SqlCommand
            iCmd.CommandText = "SET TRANSACTION ISOLATION LEVEL SERIALIZABLE BEGIN TRANSACTION " & _
                            "IF EXISTS (SELECT 1 FROM tbl_ToDoImages WHERE System=@Sys AND ImageID=@Idx) " & _
                            "BEGIN   UPDATE tbl_ToDoImages " & _
                            "SET Image = @Img " & _
                            "WHERE System=@Sys AND ImageID=@Idx       END " & _
                            "ELSE BEGIN INSERT INTO tbl_ToDoImages (System, ImageID, Image) " & _
                            "   VALUES (@Sys, @Idx, @Img)      END " & _
                            "COMMIT TRANSACTION "
            ' Replace 8000, below, with the correct size of the field
            iCmd.Parameters.AddWithValue("@Sys", Sys)
            iCmd.Parameters.AddWithValue("@Idx", id)
            iCmd.Parameters.Add("@Img", SqlDbType.VarBinary, 1048576).Value = ConvertImage(Me.pic_PartImage.Image)
            Call WriteUpdateSQL(iCmd)
            iCmd.Dispose()

            'Dim con As New SqlConnection

            'con = Open_Connection()
            'con.Open()

            'Using cmd As New SqlCommand("UPDATE  tbl_ToDoImages SET Image = @binaryValue " & _
            '                            "WHERE ImageID = '" & id & "'", con)
            '    ' Replace 8000, below, with the correct size of the field
            '    cmd.Parameters.Add("@binaryValue", SqlDbType.VarBinary, 1048576).Value = ConvertImage(Me.pic_PartImage.Image)
            '    cmd.ExecuteNonQuery()
            'End Using

            'MsgBox("Pic Saved.")

        Catch ex As Exception
            MsgBox("An Error Occurred.  The pic was NOT saved.")
        End Try
    End Sub

    Public Function ConvertImage(ByVal myImage As Image) As Byte()

        Dim ms As MemoryStream = New MemoryStream()
        myImage.Save(ms, Imaging.ImageFormat.Jpeg)
        Dim bytImage(ms.Length) As Byte
        bytImage = ms.ToArray()
        ms.Close()
        Return bytImage

    End Function

    Sub Get_Pic() 'idx As Integer)

        pic_PartImage.Image = Nothing
        Me.rtxt_ImageNotes.Text = ""

        If frm_Main.Loading = True Then Exit Sub
        If String.IsNullOrEmpty(label_ImageNum.Text) Then Exit Sub

        Try
            'idt = dtCurrImages
            Dim srch As String = "Indx=" & label_ImageNum.Text 'idx
            If dtImagesctrl.Select(srch).Any Then
                Dim dr As DataRow = dtImagesctrl.Select(srch).CopyToDataTable.Rows(0)
                label_Image.Text = dr.Item("Id").ToString
                srch = "ImageID=" & label_Image.Text 'idx & ""

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
                        txt_ImageZoom.Text = "100%" 'zm '* 100 & "%"
                        'End If

                        Me.pic_PartImage.Image = Image.FromStream(ms) 'displayImage '


                        If Not IsDBNull(iRow.Item("ImageNotes")) Then rtxt_ImageNotes.Rtf = _
                            ConvertTextToRTF(iRow.Item("ImageNotes"))

                    End If
                End If
            End If

        Catch ex As Exception
            pic_PartImage.Image = Nothing
            MsgBox(ex.Message)
        End Try

    End Sub

   
    Private Sub tsBtn_ReplaceImage_Click(sender As Object, e As EventArgs) Handles tsBtn_ReplaceImage.Click
        'pic_PartImage.Image = Nothing
        imgReplace = True
        tsBtn_Snippit.Enabled = True
        tsBtn_PasteImage.Enabled = True
    End Sub

    Private Sub CopyImageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyImageToolStripMenuItem.Click


        Clipboard.SetImage(pic_PartImage.Image)

        ' '' '' '' ''Dim st As New System.Collections.Specialized.StringCollection()
        ' '' '' '' ''st.Add(sourcebmp) '"c:\image2.bmp")
        ' '' '' '' ''System.Windows.Forms.Clipboard.SetFileDropList(st)

        ' ''Private Sub btnCopyPart_Click(sender As System.Object, e As System.EventArgs) Handles btnCopyPart.Click

        ' ''Grab the content of the first PictureBox and save it as a bitmap
        ''Dim sourcebmp As New Bitmap(pic_PartImage.Image)

        ' ''  Create an empty bitmap the size of the second PictureBox
        ''Dim destinationbmp As New Bitmap(picDestination.Width, picDestination.Height)
        ' '' Create a Graphics object for the destination
        ''Dim gr As Graphics = Graphics.FromImage(destinationbmp)

        ' '' Set the size of the area you want to copy
        ''Dim selectionrectangle As New Rectangle(140, 20, 270, 400)
        ' '' Set the size of the destination rectangle to match the size of the second PictureBox
        ''Dim destinationrectangle As New Rectangle(0, 0, picDestination.Width, picDestination.Height)

        ' '' Draw selected area on to the destination bitmap.
        ''gr.DrawImage(sourcebmp, destinationrectangle, selectionrectangle, GraphicsUnit.Pixel)

        ' '' Set the drawn destination bitmap as the image of the second PictureBox
        ''picDestination.Image = destinationbmp

        ' ''End Sub
    End Sub
End Class
