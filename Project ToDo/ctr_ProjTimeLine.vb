Imports System.Data.SqlClient

Public Class ctr_ProjTimeLine

    Private Sub cbo_Status_DrawItem(sender As Object, e As DrawItemEventArgs) Handles cbo_Status.DrawItem
        If e.Index <> -1 Then
            e.Graphics.DrawImage(imgLst_Status.Images(e.Index) _
             , e.Bounds.Left, e.Bounds.Top)
        End If
    End Sub

    Private Sub cbo_Status_MeasureItem(sender As Object, e As MeasureItemEventArgs) Handles cbo_Status.MeasureItem
        e.ItemHeight = imgLst_Status.ImageSize.Height
        e.ItemWidth = imgLst_Status.ImageSize.Width
    End Sub

    'Private Sub ctr_ProjTimeLine_BackColorChanged(sender As Object, e As EventArgs) Handles Me.BackColorChanged
    '    pic_Status.BackColor = Me.BackColor
    'End Sub

    Private Sub ctr_ProjTimeLine_Click(sender As Object, e As EventArgs) Handles Me.Click
        Me.BackColor = Color.AliceBlue
    End Sub

    'Private Sub cbo_Status_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Status.SelectedIndexChanged
    '    If frm_Main.Loading = True Then Exit Sub

    '    'MsgBox(Me.combo_Judge.SelectedValue)
    '    If cbo_Status.SelectedIndex > -1 Then
    '        Dim uCmd As SqlCommand
    '        uCmd.CommandText = "UPDATE tbl_PRS_Data " & _
    '                    "SET Judgement = '" & imgFontTag(cbo_Status.SelectedIndex) & "' " & _
    '                    "WHERE Prog = '" & Me.tsCombo_Prog.ComboBox.SelectedValue & "' AND " & _
    '                    "      prsID = '" & Me.label_prsID.Text & "' "

    '        Call WriteUpdateSQL(uCmd)

    '    End If
    'End Sub

    Private Sub ctr_ProjTimeLine_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub lbl_Item_Click(sender As Object, e As EventArgs) Handles lbl_Item.Click
        For Each ctrl As Control In Me.Parent.Controls
            ctrl.BackColor = SystemColors.Control
        Next
        Me.BackColor = Color.LightBlue
    End Sub
End Class
