Public Class ctrl_ItemTimeLine
    Public Path As String = Nothing

    Private Sub ctrl_ItemTimeLine_DoubleClick(sender As Object, e As EventArgs) Handles Me.DoubleClick, lbl_Item.DoubleClick, lbl_System.DoubleClick

        If Not IsNothing(Path) Then _
        frm_Main.tvw_Items.SelectedNode = frm_Main.GetNodeByFullPath(frm_Main.tvw_Items.Nodes, Path)

        frm_Main.tab_Main.SelectedTab = frm_Main.page_Main

    End Sub

    Private Sub ctrl_ItemTimeLine_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub pic_Notes_Click(sender As Object, e As EventArgs) Handles pic_Notes.Click
        Dim drItm() As DataRow = Nothing
        drItm = frm_Main.dsSys.Tables("dtItems").Select("Path='" & Path & "'")
        If drItm.Length > 0 Then
            frm_Notes.lbl_Task.Text = lbl_Item.Text
            frm_Notes.lbl_System.Text = SystemNameFromPath(Path)
            If Not String.IsNullOrEmpty(drItm(0)("Notes").ToString) Then
                frm_Notes.rTxt_Notes.Rtf = drItm(0)("Notes").ToString
            Else
                frm_Notes.rTxt_Notes.Text = ""
            End If

            'Add Image Here

            frm_Notes.Show()
        End If

    End Sub
End Class
