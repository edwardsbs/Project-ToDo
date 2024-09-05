Imports System.Data.SqlClient

Public Class frm_NewItem
    Public Loading As Boolean = True
    Public TopNode As String = ""
    Public Shared Property frmMain() As frm_Main

    Private Sub frm_NewItem_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Loading = True

        Center_Form_to_Main(Me)

        txt_Item.Text = ""
        TopNode = ""

        Dim dt As New DataTable
        dt = frm_Main.dsSys.Tables("dtSystems").Copy
        combo_System.DataSource = dt
        combo_System.ValueMember = "System"
        combo_System.DisplayMember = "System"

        Dim NewString As String() = frm_Main.tvw_Items.SelectedNode.FullPath.Split(New String() {" || "}, StringSplitOptions.None)

        combo_Parent.DataSource = Nothing

        If IsNothing(NewString) Then Exit Sub

        TopNode = NewString(1)

        Select Case NewString.Length
            Case 1 'The word 'Systems' is selected, not a node
                'combo_System.Text = "Systems"
                'TopNode = "Systems"
                combo_Parent.Text = TopNode
            Case Is > 1
                If Not String.IsNullOrEmpty(NewString(1).ToString) Then

                    'combo_System.SelectedValue = NewString(1)
                    TopNode = NewString(1)

                    If frm_Main.dsSys.Tables("dtItems").Select("System='" & NewString(1) & "'").Any Then
                        Dim dvP = New DataView(frm_Main.dsSys.Tables("dtItems").Select("System='" & NewString(1) & "' OR " &
                                                                                       "[Parent]='" & NewString(1) & "'").CopyToDataTable)
                        Dim dvPsub As DataTable = dvP.ToTable(True, "Parent")
                        combo_Parent.DataSource = dvPsub
                        combo_Parent.ValueMember = "Parent"
                        combo_Parent.DisplayMember = "Parent"
                        combo_Parent.SelectedValue = NewString(frm_Main.tvw_Items.SelectedNode.Level)
                    End If
                End If
        End Select

        combo_System.SelectedIndex = combo_System.FindString(TopNode)

        Loading = False

        'Call combo_System_SelectedIndexChanged(sender, e)

        txt_Path.Text = frm_Main.tvw_Items.SelectedNode.FullPath

    End Sub

    Private Sub btn_AddNew_Click(sender As Object, e As EventArgs) Handles btn_AddNew.Click

        If String.IsNullOrEmpty(txt_Item.Text) Then
            Dim answ As MsgBoxResult = MsgBox("Is this a NEW System?", MsgBoxStyle.YesNoCancel, "New System")
            If Not answ = MsgBoxResult.Yes Then
                MsgBox("Add an Item Name before saving.")
                Exit Sub
            End If
        End If

        'Write to SQL Server
        'Dim NewString As String() = frm_Main.tvw_Items.SelectedNode.FullPath.Split(New String() {"\"}, StringSplitOptions.None)
        'Add New SYSTEM first (if System Name not already in the tables)
        Dim uCmd As New SqlCommand 'UPDATE Query
        uCmd.CommandText = "IF NOT EXISTS (SELECT 1 FROM tbl_ToDoSystems WHERE System=@Sys AND Architect=@Usr) " &
                            "BEGIN   INSERT INTO tbl_ToDoSystems (System, Architect) " &
                            "   VALUES (@Sys, @Usr)      END "

        uCmd.Parameters.AddWithValue("@Sys", TopNode) 'combo_System.Text)
        uCmd.Parameters.AddWithValue("@Usr", frm_Main.ss_User.Text)
        Call WriteUpdateSQL(uCmd)
        uCmd.Parameters.Clear()

        'Insert/Update Items
        uCmd.CommandText = "IF EXISTS (SELECT 1 FROM tbl_ToDoItems WHERE System=@Sys AND Item=@Itm AND [Parent]=@Par) " &
                            "BEGIN   UPDATE tbl_ToDoItems " &
                            "SET LastUpdate=@UpDt " &
                            "WHERE System=@Sys AND Item=@Itm AND [Parent]=@Par       END " &
                            "ELSE BEGIN INSERT INTO tbl_ToDoItems (System, Item, [Parent], Path, Phase, LastUpdate) " &
                            "   VALUES (@Sys, @Itm, @Par, @Pth, @Phs, @UpDt)      END "

        uCmd.Parameters.AddWithValue("@Sys", TopNode) 'combo_System.Text)
        uCmd.Parameters.AddWithValue("@Itm", If(String.IsNullOrEmpty(txt_Item.Text), txt_Path.Text, txt_Item.Text))
        uCmd.Parameters.AddWithValue("@Par", txt_Path.Text) 'combo_Parent.Text)
        uCmd.Parameters.AddWithValue("@Pth", txt_Path.Text & " || " & txt_Item.Text)
        uCmd.Parameters.AddWithValue("@Phs", "") 'This is required for new items to show up
        uCmd.Parameters.AddWithValue("@UpDt", Now())
        Call WriteUpdateSQL(uCmd)
        uCmd.Parameters.Clear()

        'Add New Project Phase Structure
        If chk_AddPhases.Checked = True Then
            'Insert/Update Items

            Dim pth As String = txt_Path.Text & " || " & txt_Item.Text
            uCmd.CommandText = "INSERT INTO tbl_ToDoItems (System, Item, [Parent], Path, Phase, Type, iIndex, LastUpdate) " &
            "VALUES (@Sys, 'Initial Phase', @Par, '" & pth & " || " & "Initial Phase', @Phs, 'Phase', 0, @UpDt), " &
            "(@Sys, 'Planning & Design', '" & pth & " || " & "Initial Phase', '" & pth & " || " & "Initial Phase || Planning & Design', @Phs, 'Task', 0, @UpDt), " &
            "" &
            "(@Sys, 'Development Phase', @Par, '" & pth & " || " & "Development Phase', @Phs, 'Phase', 1, @UpDt), " &
            "(@Sys, 'Programming', '" & pth & " || " & "Development Phase', '" & pth & " || " & "Development Phase || Programming', @Phs, 'Task', 0, @UpDt), " &
            "(@Sys, 'Testing / Debugging', '" & pth & " || " & "Development Phase', '" & pth & " || " & "Development Phase || Testing / Debugging', @Phs, 'Task', 1, @UpDt), " &
            "" &
            "(@Sys, 'Implementation Phase', @Par, '" & pth & " || " & "Implementation Phase', @Phs, 'Phase', 2, @UpDt), " &
            "(@Sys, 'Beta/Debugging',  '" & pth & " || " & "Implementation Phase', '" & pth & " || " & "Implementation Phase || Beta/Debugging', @Phs, 'Task', 2, @UpDt), " &
            "" &
            "(@Sys, 'Reflection Phase', @Par, '" & pth & " || " & "Reflection Phase', @Phs, 'Phase', 3, @UpDt) "

            uCmd.Parameters.AddWithValue("@Sys", TopNode) 'combo_System.Text)
            uCmd.Parameters.AddWithValue("@Itm", txt_Item.Text)
            uCmd.Parameters.AddWithValue("@Par", pth) 'combo_Parent.Text)
            uCmd.Parameters.AddWithValue("@Pth", txt_Path.Text & " || " & txt_Item.Text)
            uCmd.Parameters.AddWithValue("@Phs", "") 'This is required for new items to show up
            uCmd.Parameters.AddWithValue("@UpDt", Now())
            Call WriteUpdateSQL(uCmd)
            uCmd.Parameters.Clear()
        End If

        uCmd.Dispose()

        'Reload Items
        Call frm_Main.Load_Systems()
        'Call frm_Main.Load_Images()
        'Call frm_Main.Load_Phases()
        Me.Close()

    End Sub


    'Private Sub combo_System_SelectedIndexChanged(sender As Object, e As EventArgs) Handles combo_System.SelectedIndexChanged

    '    If Loading = True Then Exit Sub

    '    combo_Parent.DataSource = Nothing

    '    Dim NewString As String() = frm_Main.tvw_Items.SelectedNode.FullPath.Split(New String() {"|"}, StringSplitOptions.None)

    '    Dim strSrch As String = "System='" & NewString(1) & If(Not IsError(frm_Main.tvw_Items.SelectedNode.Parent.FullPath), "' OR [Parent]='" & frm_Main.tvw_Items.SelectedNode.Parent.FullPath & "'", "")

    '    If Not String.IsNullOrEmpty(NewString(1).ToString) Then
    '        If frm_Main.dsSys.Tables("dtItems").Select("System='" & NewString(1) & "'").Any Then
    '            Dim dvP = New DataView(frm_Main.dsSys.Tables("dtItems").Select(strSrch).CopyToDataTable)
    '            Dim dvPsub As DataTable = dvP.ToTable(True, "Parent")
    '            dvPsub.Rows.Add({frm_Main.tvw_Items.SelectedNode.Text})
    '            combo_Parent.DataSource = dvPsub
    '            combo_Parent.ValueMember = "Parent"
    '            combo_Parent.DisplayMember = "Parent"
    '            'MsgBox(frm_Main.tvw_Items.SelectedNode.Level & "  -  " & NewString(frm_Main.tvw_Items.SelectedNode.Level))
    '            combo_Parent.SelectedValue = NewString(frm_Main.tvw_Items.SelectedNode.Level)
    '        End If
    '    End If

    'End Sub

    'Private Sub combo_System_TextChanged(sender As Object, e As EventArgs) Handles combo_System.TextChanged

    '    If frm_Main.tvw_Items.SelectedNode.FullPath = "Systems" Then
    '        txt_Path.Text = TopNode
    '    Else
    '        txt_Path.Text = TopNode & " || " & combo_System.Text
    '    End If


    'End Sub
End Class