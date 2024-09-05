Imports System.Data.SqlClient
Imports System.IO
Imports System.Runtime.InteropServices

Public Class frm_Main

    Private mDragg As TreeNode
    Private mDropp As TreeNode

    Public Loading As Boolean
    Public Refreshing As Boolean
    Public Reordering As Boolean
    Friend Shared WithEvents dsSys As DataSet
    Friend Shared WithEvents dtItems As DataTable
    Public dtImages As DataTable
    Public dtCurrImages As DataTable
    Public dtPhases As DataTable
    Public strSysList As String
    Friend Shared WithEvents dtWkly As DataTable
    Friend Shared WithEvents dtDaily As DataTable
    Friend Shared WithEvents dtTaskJournal As DataTable
    Friend Shared WithEvents dtOffDays As DataTable
    Friend Shared WithEvents dtFactors As DataTable

    Private _ExpandedNodeList As New List(Of String)

    Private _originalSize As Size = Nothing
    Private _scale As Single = 1
    Private _scaleDelta As Single = 0.0005
    'Private m_PanStartPoint As New Point
    Dim CtrlIsDown As Boolean

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)> _
    Public Shared Function GetScrollPos(hWnd As IntPtr, nBar As Integer) As Integer
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)> _
    Public Shared Function SetScrollPos(hWnd As IntPtr, nBar As Integer, nPos As Integer, bRedraw As Boolean) As Integer
    End Function

    Private Const SB_HORZ As Integer = &H0
    Private Const SB_VERT As Integer = &H1

    Private Sub frm_Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Loading = True

        panel_Menu.Width = 33

        'GET USER INFO-------------------------
        ss_User.Text = GetUserName()
        Dim tvFont As Font = New Font(tvw_Items.Font.FontFamily, tvw_Items.Font.Size + 1, FontStyle.Bold)
        tvw_Items.Font = tvFont

        Dim dt As New DataTable
        Dim Sql As String = "SELECT [Assoc Name] FROM Associate_List WHERE [Planner Number]='" & ss_User.Text & "'"
        Call Load_Data(Sql, dt)
        ss_UserName.Text = dt.Rows(0).Item("Assoc Name").ToString
        '---------------------------------------

        check_HideComplete.Checked = True
        chk_HideWeekends.Checked = True

        SplitContainer1.SplitterWidth = 10

        'Call Load_Menu()
        'Dim tvw As New TreeViewOwnerDraw

        Call Refresh_Data()

        'init this from here or a method depending on your needs
        'If pic_PartImage.Image IsNot Nothing Then
        '    pic_PartImage.Size = Panel1.Size
        '    _originalSize = Panel1.Size
        'End If

        'tvw_Items.Nodes(0).EnsureVisible()

        AddToolTips()

        'pic_PartImage.Width = panel_Image.Width - 20
        'pic_PartImage.Height = panel_Image.Height - 20

        Loading = False

    End Sub

    Public Function MouseIsOverControl(ByVal c As Control) As Boolean
        Return c.ClientRectangle.Contains(c.PointToClient(Control.MousePosition))
    End Function

    Private Sub frm_Main_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        'Check if key pressed is Ctrl and then make sure the mouse is hovering over the picturebox
        '  If so, give the picturebox the focus
        '  Otherwise, it will give it the focus every time you cross over the picturebox which makes it difficult to select text for formatting and click the richtext format buttons without losing focuse on the rTxt_Image control

        'ss_Control.Text = FindControlAtCursor(Me).Name.ToString & "  ||  " & FindControlAtCursor(Me).Parent.Name.ToString
        'Me.StatusStrip1.Refresh()
        If e.Control Then
            If IsNothing(FindControlAtCursor(Me)) Then GoTo check_HotKeys
            If FindControlAtCursor(Me).Name = "pic_PartImage" Then
                'Dim scrlPnt As New Drawing.Point(panel_Image.AutoScrollPosition.X, panel_Image.AutoScrollPosition.Y)
                'panel_Image.SuspendLayout()
                'pic_PartImage.Select()
                'panel_Image.AutoScrollPosition = New Drawing.Point(-scrlPnt.X, -scrlPnt.Y)
                'panel_Image.ResumeLayout()

                Dim pnl As Panel = DirectCast(FindControlAtCursor(Me).Parent, Panel)
                Dim scrlPnt As New Drawing.Point(pnl.AutoScrollPosition.X, pnl.AutoScrollPosition.Y)
                pnl.SuspendLayout()
                FindControlAtCursor(Me).Select()
                pnl.AutoScrollPosition = New Drawing.Point(-scrlPnt.X, -scrlPnt.Y)
                pnl.ResumeLayout()

            End If
        End If

check_HotKeys:
        If (e.Control AndAlso (e.KeyCode = Keys.S)) Then
            ' When Ctrl + S is pressed, the event for Save is executed
            tsbtn_Save.PerformClick()
        End If
        If (e.Control AndAlso (e.KeyCode = Keys.B)) Then
            ' When Ctrl + B is pressed, the event for Bold Text is executed
            tsbtn_Bold.PerformClick()
        End If
        If (e.Control AndAlso (e.KeyCode = Keys.I)) Then
            ' When Ctrl + I is pressed, the event for Italics Text is executed
            tsbtn_Italic.PerformClick()
        End If
        If (e.Control AndAlso (e.KeyCode = Keys.U)) Then
            ' When Ctrl + U is pressed, the event for Underline Text is executed
            tsbtn_Underline.PerformClick()
        End If
        If (e.Alt AndAlso (e.KeyCode = Keys.B)) Then
            ' When Alt + B is pressed, Bullet Points
            tsbtn_Bullets.PerformClick()
        End If
        If (e.Alt AndAlso (e.KeyCode = Keys.I)) Then
            ' When Alt + I is pressed, Indent
            tsbtn_Indent.PerformClick()
        End If
    End Sub




    Sub Load_Menu()
        panel_Menu.SuspendLayout()
        panel_Menu.Controls.Clear()

        Dim dt As New DataTable
        Dim Sql As String = "SELECT * FROM tbl_ToDo_Menu ORDER BY MenuOrd "
        'FIELDS:  MenuItem, MenuText, MenuOrd, MenuVisible
        Call Load_Data(Sql, dt)

        If dt.Rows.Count > 0 Then
            Dim fSel As Font = New Font("Tahoma", 10, FontStyle.Bold)
            Dim fNorm As Font = New Font("Tahoma", 10, FontStyle.Regular)
            Dim Top As Single = 9 'Where 1st Tab top starts
            Dim Lft As Single = 3 'Left Indent of Tab
            Dim Hgt As Single = 42 'Tab Height
            Dim Gap As Single = 0 'Gap Between Tabs
            'Dim id As Integer = 0
            panel_Menu.VerticalScroll.Value = 0
            panel_Menu.AutoScroll = False

            For Each dRow As DataRow In dt.Rows
                If dRow.Item("MenuVisible") = "True" Then
                    Dim iMenu As New ctrl_MenuItem
                    iMenu.Name = dRow.Item("MenuOrd")
                    iMenu.Height = Hgt
                    iMenu.lab_Index.Text = dRow.Item("MenuOrd")
                    iMenu.label_TabText.Text = dRow.Item("MenuText").ToString
                    If iList_Menu.Images.Count - 1 >= CInt(dRow.Item("MenuOrd")) Then iMenu.pic_Icon.Image = iList_Menu.Images(dRow.Item("MenuOrd"))

                    'Add Control to the Panel
                    panel_Menu.Controls.Add(iMenu)
                    iMenu.Top = Top
                    iMenu.Left = Lft
                    iMenu.Width = panel_Menu.Width - 3
                    Top = Top + iMenu.Height + Gap
                    'Call Panel_3D(iMenu.panel_Tab)

                    'id += 1
                End If

            Next

            Call MenuSelection(0)

        End If

    End Sub

    Sub Refresh_Data()

        Call Load_Systems()
        Call Load_Images()
        Call Load_Phases()
        Call Load_Factors()
        Call Load_OffDays()



    End Sub

    Sub Load_Images()
        dtImages = New DataTable
        Dim Sql As String = "SELECT * FROM tbl_ToDoImages " & _
            If(String.IsNullOrEmpty(strSysList), "WHERE System IN (" & strSysList & ")", "")
        Call Load_Data(Sql, dtImages)
    End Sub

    Sub Load_Phases()

        dtPhases = New DataTable
        Dim Sql As String = "SELECT * FROM tbl_ToDoPhases"
        Call Load_Data(Sql, dtPhases)

        combo_Phase.DataSource = dtPhases
        combo_Phase.DisplayMember = "Phase"
        combo_Phase.ValueMember = "Phase"

    End Sub

    Sub Load_Factors()
        dtFactors = New DataTable
        Dim Sql As String = "SELECT * FROM tbl_ToDoFactors WHERE UserID IN ('All', '" & ss_User.Text & "') "
        Call Load_Data(Sql, dtFactors)

        'dgv_.DataSource = Nothing
        ''ID, Item, DateStart, DateEnd, Comments, Image, UserID
        'Dim dtF As DataTable = New DataView(dtFactors, "", "Item ASC, Property ASC", DataViewRowState.CurrentRows).ToTable(True, "Item", "Property", "UserID")
        'dgv_.DataSource = dtF
    End Sub

    Sub Load_OffDays()
        dtOffDays = New DataTable
        Dim Sql As String = "SELECT * FROM tbl_ToDo_OffDays WHERE UserID IN ('All', '" & ss_User.Text & "') " &
            "ORDER BY DateEnd DESC, DateStart DESC "
        Call Load_Data(Sql, dtOffDays)

        dgv_OffDays.DataSource = Nothing
        'ID, Item, DateStart, DateEnd, Comments, Image, UserID
        'Dim odFilter As String = If(chk_OffDaysViewPast.Checked = True, "", "DateStart >= #" & Date.Today() & "# OR DateEnd >= #" & Date.Today() & "#")
        Dim odFilter As String = If(chk_OffDaysViewPast.Checked = True, "", "DateStart >= #1/1/" & Date.Today.Year & "# AND DateEnd <= #12/31/" & Date.Today.Year & "#")
        Dim dtOD As DataTable = New DataView(dtOffDays, odFilter, "DateStart ASC, DateEnd ASC, Item", DataViewRowState.CurrentRows).ToTable(True, "Item", "Hours", "DateStart", "DateEnd", "ID")
        dgv_OffDays.DataSource = dtOD

    End Sub

    Sub Load_Items()

        If dsSys.Tables.Contains("dtItems") Then
            dsSys.Tables.Remove("dtItems")
            dsSys.Tables.Add("dtItems")
        End If


        strSysList = "'" &
                String.Join("','",
                            (From row In dsSys.Tables("dtSystems").Rows Select CType(row.Item("System"), String))) & "'"

        Dim Sql As String = "SELECT I.*, S.iIndex as sIndex " &
            "FROM tbl_ToDoItems AS I LEFT JOIN tbl_ToDoSystems AS S " &
            "ON I.System=S.System  " &
            "WHERE S.Architect = '" & ss_User.Text & "' " &
            "ORDER BY S.iIndex, System, I.Parent, I.iIndex, I.Item " 'S.iIndex, System, P.iIndex, Parent, I.iIndex, I.Item 
        ' System IN (" & strSysList & ") " & _

        '"SELECT I.*, S.iIndex as sIndex, I.iIndex, P.iIndex as pIndex, P.Phase as pPhase, P.KeepAsRef as pKeep " & _
        '    "FROM (tbl_ToDoItems AS I LEFT JOIN tbl_ToDoSystems AS S " & _
        '    "ON I.System=S.System ) LEFT JOIN tbl_ToDoItems P " & _
        '    "ON I.System = P.System AND I.Parent = (P.Parent + ' || ' + P.Item)  " & _
        '    "WHERE S.Architect = '" & ss_User.Text & "' " & _
        '    "ORDER BY S.iIndex, System, P.iIndex, I.iIndex, Parent, I.Item " 'S.iIndex, System, P.iIndex, Parent, I.iIndex, I.Item 
        '' System IN (" & strSysList & ") " & _

        Call Load_Data(Sql, dsSys.Tables("dtItems"))
        dsSys.Tables("dtItems").Columns("OverallPriority").ColumnName = "Pri"
        'dsSys.Tables("dtItems").Columns("OverallPriority").Namespace = "Pri"

        'Load Task/Item Journal
        dtTaskJournal = New DataTable
        Sql = "SELECT * FROM tbl_ToDoTaskJournal WHERE Architect = '" & ss_User.Text & "' "
        Call Load_Data(Sql, dtTaskJournal)

    End Sub


    Sub Load_Systems()

        Loading = True

        Try

            Dim VisIndex As Boolean = False 'determine 1st node that is visible (where scrolled to)
            Dim scrollpnt As New Point 'store the scroll point

            'Load Systems
            dsSys = New DataSet
            dsSys.Tables.Add("dtSystems")
            dsSys.Tables.Add("dtItems")
            Call Refresh_Systems()

            If dsSys.Tables("dtSystems").Rows.Count = 0 Then Exit Sub

            Call Load_Items()

            'dsSys.Relations.Add("relSystems", dsSys.Tables("dtSystems").Columns("System"), dsSys.Tables("dtItems").Columns("System"))

            'Save Scroll Position if it is not the initial Load
            If tvw_Items.Nodes.Count > 0 Then
                VisIndex = True
                scrollpnt = New Point(GetScrollPos(CInt(tvw_Items.Handle), SB_HORZ), GetScrollPos(CInt(tvw_Items.Handle), SB_VERT))
            End If

            'Fill in the treeview
            tvw_Items.Nodes.Clear()

            'Dim tvTier As New TreeNode
            'tvTier = New TreeNode("Systems")
            'tvTier.Name = "Systems"
            'tvw_Items.Nodes.Add(tvTier)

            'Dim ParentTable As System.Data.DataTable
            'ParentTable = dsSys.Tables("dtSystems")
            Dim hideCheck As String = ""
            Dim dtSys As New DataTable
            If check_HideComplete.Checked = True Then
                hideCheck = " (NOT Phase = 'Complete' OR KeepAsRef = 'True')  " 'AND (NOT pPhase = 'Complete' OR pKeep = 'True')
            End If

            Dim currentFont As System.Drawing.Font = tvw_Items.Font

            'Dim strSrch As String = ""
            If dsSys.Tables("dtItems").Select("[Parent]='Base' AND Type='Category'").Any Then 'If dsSys.Tables("dtItems").Select(hideCheck).Any Then
                'Call PopulateTreeView(tvw_Items, dsSys.Tables("dtItems").Select(hideCheck, "sIndex ASC, System, pIndex, Parent, iIndex ASC, Item").CopyToDataTable, " || ") '"sIndex ASC, System, pIndex, Parent, iIndex ASC, Item"

                'Call PopulateTreeView(tvw_Items, _
                '                      New DataView(dsSys.Tables("dtItems"), hideCheck, "sIndex ASC, System, pIndex, Parent, iIndex ASC, Item", DataViewRowState.CurrentRows).ToTable(True, "System", "sIndex") _
                '                      , " || ") '"sIndex ASC, System, pIndex, Parent, iIndex ASC, Item"
                Dim dtCategories As DataTable = New DataView(dsSys.Tables("dtItems"), "[Parent]='Base' AND Type='Category'", "sIndex ASC, System", DataViewRowState.CurrentRows).ToTable(True, "Path", "sIndex") 'hideCheck
                'For Each dr As DataRow In dtCategories.Rows
                '    dr.Item("Path") = Trim(Microsoft.VisualBasic.Left(dr.Item("Path"), InStr(dr.Item("Path"), " || ")))
                'Next
                'For Each nPath As DataRow In New DataView(dtCategories, "Path IS NOT NULL AND NOT Path = ''", "sIndex ASC", DataViewRowState.CurrentRows).ToTable(True, "Path", "sIndex").Rows
                '    MsgBox(nPath.Item("sIndex") & "  " & nPath.Item("Path"))
                'Next
                'For Each tPath As DataRow In New DataView(dtCpy, "Path IS NOT NULL", "sIndex ASC", DataViewRowState.CurrentRows).ToTable(True, "Path").Rows
                '    MsgBox(tPath.Item("Path").ToString)
                'Next

                'For Each nPath As DataRow In New DataView(dsSys.Tables("dtItems"), hideCheck, "sIndex ASC, System", DataViewRowState.CurrentRows).ToTable(True, "System", "sIndex").Rows
                For Each nPath As DataRow In New DataView(dtCategories, "Path IS NOT NULL AND NOT Path = ''", "sIndex ASC", DataViewRowState.CurrentRows).ToTable(True, "Path").Rows
                    'Dim sysNode As TreeNode
                    'Dim drSys() As DataRow = dsSys.Tables("dtItems").Select("System='" & nPath.Item("System").ToString & "'")
                    'If drSys.Length > 0 Then
                    If nPath.Item("Path").ToString = "PQ System" Then
                        Dim strTest As String = ""
                    End If
                    Dim sysNode As New TreeNode
                    sysNode = New TreeNode(nPath.Item("Path").ToString) 'drSys(0)("System").ToString())
                    sysNode.Name = nPath.Item("Path").ToString 'nPath.Item("System").ToString
                    'sysNode.NodeFont = New Font(tvw_Items.Font.FontFamily, tvw_Items.Font.Size + 1, FontStyle.Bold)
                    'sysNode.Bounds.Inflate(sysNode.Bounds.Width + 25, sysNode.Bounds.Height)

                    tvw_Items.Nodes.Add(sysNode)
                    'If drSys(0)("System").ToString = "TGT / BOMM Compare" Then
                    '    Dim strTest As String = ""
                    'End If
                    'sysNode = sysNode.Nodes.Add(drSys(0)("Path").ToString())
                    'sysNode.Name = nPath.Item("System").ToString
                    Call AddNodeColor(sysNode, sysNode.FullPath)
                    Call AddChildNodes(sysNode)
                    'End If

                    'Dim sTag As New mTag
                    'sTag.Add("Sys", nPath.Item("System").ToString)
                    'sTag.Add("Name", nPath.Item("System").ToString)
                    'sTag.Add("Par", "Systems")
                    'sTag.Add("Path", "")
                    'sTag.Add("Exp", "")
                    'sysNode.Tag = sTag
                    'sysNode.Tag = nPath.Item("Path").ToString
                Next
            End If

            ' ''For Each parentrow As DataRow In dsSys.Tables("dtSystems").Rows
            ' ''    If Not parentrow.Item("Phase").ToString = "Complete" Then
            ' ''        Dim parentnode As TreeNode
            ' ''        parentnode = tvTier.Nodes.Add(parentrow.Item(0))
            ' ''        parentnode.Name = parentrow.Item(0)
            ' ''        parentnode.Tag = If(IsDBNull(parentrow.Item("Expanded")), "", If(parentrow.Item("Expanded") = True, "Expanded", "")) 'parentrow.Item("System")

            ' ''        ''''''Change font for Systems'''
            ' ''        parentnode.NodeFont = New Font(currentFont.FontFamily, currentFont.Size + 1, FontStyle.Bold)

            ' ''        ''''''Add Color''''''
            ' ''        'Call AddNodeColor(parentnode, parentrow.Item("Phase").ToString)

            ' ''        ''''populate child'''''
            ' ''        '''''''''''''''''''''''
            ' ''        'hideCheck = 
            ' ''        Dim strSrch As String = "System='" & parentnode.Text & "' AND [Parent]='" & parentnode.Text & "'" & hideCheck

            ' ''        If dsSys.Tables("dtItems").Select(strSrch).Any Then
            ' ''            Dim childnode As New TreeNode()
            ' ''            dsSys.Tables("dtItems").DefaultView.Sort = "iIndex ASC"
            ' ''            For Each childrow As DataRow In dsSys.Tables("dtItems").Select(strSrch, "iIndex ASC").CopyToDataTable.Rows 'parentrow.GetChildRows("relSystems")
            ' ''                'If childrow.Item("System").ToString = childrow.Item("[Parent]").ToString Then
            ' ''                childnode = parentnode.Nodes.Add(childrow(2)) '& " " & childrow(2))
            ' ''                childnode.Name = childrow(2).ToString
            ' ''                childnode.Tag = If(IsDBNull(childrow.Item("Expanded")), "", If(childrow.Item("Expanded") = True, "Expanded", "")) 'childrow("Item")
            ' ''                strSrch = "System='" & parentnode.Text & "' AND [Parent]='" & childrow(2).ToString & "'" & hideCheck
            ' ''                If dsSys.Tables("dtItems").Select(strSrch).Any Then
            ' ''                    Call AddChildNodes(dsSys.Tables("dtItems").Select(strSrch, "iIndex ASC").CopyToDataTable, childnode)
            ' ''                End If
            ' ''                Call AddNodeColor(childnode, childrow.Item("Phase").ToString, childrow.Item("Type").ToString)

            ' ''                If Not IsDBNull(childrow.Item("Expanded")) AndAlso childrow.Item("Expanded") = True Then
            ' ''                    childnode.Expand()
            ' ''                End If
            ' ''                'End If
            ' ''            Next childrow
            ' ''        End If
            ' ''    End If
            ' ''Next parentrow

            tvw_Items.Nodes(0).Expand()

            For Each tNode As TreeNode In GetAllNodes(tvw_Items)
                'If IsNothing(tNode.Tag) Then MsgBox(tNode.Name & "  has no Tag")
                If IsNothing(tNode.Tag) Then Continue For
                Dim exTag As New mTag
                exTag = CType((tNode.Tag), mTag)
                'MsgBox(exTag.Get("Name"))
                'Dim strTag As String = exTag.[Get]("Exp").ToString 'CType((exTag.[Get]("Exp")), String)
                'MsgBox(tNode.Name & vbCrLf & exTag.[Get]("Path").ToString & exTag.[Get]("Exp").ToString)
                If exTag.[Get]("Exp").ToString = "Expanded" Then 'If tNode.Tag = "Expanded" Then
                    tNode.Expand()
                End If
            Next

            'Set Scroll Position
            If VisIndex = True Then
                SetScrollPos(tvw_Items.Handle, SB_HORZ, scrollpnt.X, True)
                SetScrollPos(tvw_Items.Handle, SB_VERT, scrollpnt.Y, True)
            End If

            Call tvw_Items_AfterSelect(Nothing, Nothing)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Loading = False

    End Sub

    'Sub AddChildNodes(dt As DataTable, parentNd As TreeNode)
    '    'Dim childnode As TreeNode
    '    Dim childnode As New TreeNode()
    '    For Each childrow As DataRow In dt.Rows
    '        childnode = parentNd.Nodes.Add(childrow(2)) '& " " & childrow(2))
    '        childnode.Tag = childrow("Item")

    '        Call AddNodeColor(childnode, childrow.Item("Phase").ToString, childrow.Item("Type").ToString)
    '    Next childrow
    'End Sub

    Sub AddChildNodes(parentNd As TreeNode)
        'Dim childnode As TreeNode
        Dim hideCheck As String = ""
        If check_HideComplete.Checked = True Then
            hideCheck = " AND (NOT Phase = 'Complete' OR KeepAsRef = 'True') " 'AND (NOT pPhase = 'Complete' OR pKeep = 'True') "
        End If
        If dsSys.Tables("dtItems").Select("[Parent]='" & parentNd.FullPath & "'" & hideCheck).Any Then
            Dim childnode As New TreeNode()
            For Each childrow As DataRow In dsSys.Tables("dtItems").Select("[Parent]='" & parentNd.FullPath & "'" & hideCheck, "iIndex ASC").CopyToDataTable.Rows
                childnode.NodeFont = New Font(tvw_Items.Font.FontFamily, tvw_Items.Font.Size - 1, FontStyle.Regular)
                'Dim exTag As New mTag
                'exTag = CType((childnode.Tag), mTag)
                'exTag.Update("Path", childrow("Path").ToString)
                'childnode.Tag = exTag 'childrow("Path").ToString
                childnode = parentNd.Nodes.Add(childrow("Item")) '& " " & childrow(2))
                If parentNd.FullPath = "PQ System || PQ System" Then
                    Dim strTest As String = ""
                End If
                Call AddNodeColor(childnode, childrow.Item("Path").ToString)
                If childnode.Text = "Development Phase" Then
                    Dim str As String = ""
                End If
                If dsSys.Tables("dtItems").Select("[Parent]='" & childnode.FullPath & "'").Any Then Call AddChildNodes(childnode)
            Next childrow
        End If
    End Sub

    Sub AddNodeColor(xNode As TreeNode, Optional ByRef xPath As String = "", Optional ByRef xType As String = "")

        'If DragDropEffects =

        'If Loading = True Then Exit Sub
        If IsNothing(xNode) Then Exit Sub
        'If IsNothing(xNode.Parent) Then Exit Sub

        Try

            Dim dr As DataRow = Nothing
            Dim drSys As DataRow = Nothing
            'If System
            If xNode.FullPath = "PQ System || PQ System" Then
                Dim strTest As String = ""
            End If

            xNode.Bounds.Inflate(xNode.Bounds.Width + 25, xNode.Bounds.Height)

            If xNode.Text = xNode.FullPath Then 'Meaning top level Or 'System' item
                If dsSys.Tables("dtSystems").Select("System='" & xNode.Name & "'").Any Then
                    'If dsSys.Tables("dtItems").Select("System='" & xNode.Text & "'").Any Then
                    drSys = dsSys.Tables("dtSystems").Select("System='" & xNode.Name & "'").CopyToDataTable.Rows(0)
                    'dr = dsSys.Tables("dtItems").Select("System='" & xNode.Text & "'").CopyToDataTable.Rows(0)
                    'xNode.NodeFont = New Font(tvw_Items.Font.FontFamily, tvw_Items.Font.Size + 1, FontStyle.Bold)
                    'xNode.Bounds.Width = xNode.Bounds.Width + 10
                End If
            Else
                'If Items
                'If dsSys.Tables("dtItems").Select("Path='" & xPath & "'").Any Then
                '    dr = dsSys.Tables("dtItems").Select("Path='" & xPath & "'").CopyToDataTable.Rows(0)
                'End If
            End If

            If dsSys.Tables("dtItems").Select("Path='" & xNode.FullPath & "'").Any Then
                dr = dsSys.Tables("dtItems").Select("Path='" & xNode.FullPath & "'").CopyToDataTable.Rows(0)
            End If

            ''''''Set Tags''''''
            If String.IsNullOrEmpty(dr.Item("System").ToString) Then Exit Sub

            Dim exTag As New mTag()
            exTag.Add("Sys", dr.Item("System").ToString) 'If(xNode.Parent.Text = "Systems", "Systems",
            exTag.Add("Name", xNode.Text)
            exTag.Add("Path", dr.Item("Path").ToString)
            exTag.Add("Par", dr.Item("Parent").ToString)
            exTag.Add("Type", dr.Item("Type").ToString)
            'exTag.Add("Exp", "") 'This one is added below


            ''''''Add Color'''''
            Dim Phase As String = If(xNode.Text = xNode.FullPath, drSys.Item("Phase").ToString, dr.Item("Phase").ToString)
            Select Case Phase 'dr.Item("Phase").ToString 'xPhase
                Case "Ice Box"
                    xNode.ForeColor = Color.SteelBlue
                Case "Emergency"
                    xNode.ForeColor = Color.Red
                    If Not IsNothing(xNode.Parent) Then xNode.Parent.ForeColor = Color.Red
                Case "In Progress"
                    xNode.ForeColor = Color.Green
                    If Not IsNothing(xNode.Parent) AndAlso Not xNode.Parent.ForeColor = Color.Red Then xNode.Parent.ForeColor = Color.Green
                Case "Testing"
                    xNode.ForeColor = Color.YellowGreen
                Case "Complete"
                    xNode.ForeColor = Color.DimGray
                    'Dim currentFont As System.Drawing.Font = tvw_Items.Font
                    xNode.NodeFont = New Font(tvw_Items.Font.FontFamily, tvw_Items.Font.Size - 1, FontStyle.Strikeout)
                Case String.Empty
                    xNode.ForeColor = Color.Black
                Case Else
                    xNode.ForeColor = Color.Black
            End Select


            'Check Type (for Items only)
            If xNode.Text = xNode.FullPath Then
                Select Case dr.Item("Type").ToString 'xType
                    Case "Phase"
                        Select Case dr.Item("Status").ToString
                            Case "Complete"
                                xNode.ForeColor = Color.Blue
                                xNode.NodeFont = New Font(tvw_Items.Font.FontFamily, tvw_Items.Font.Size - 1, FontStyle.Italic)
                            Case Else
                                xNode.ForeColor = Color.Blue
                                xNode.NodeFont = New Font(tvw_Items.Font.FontFamily, tvw_Items.Font.Size - 1, FontStyle.Bold Or FontStyle.Italic)
                        End Select
                    Case "Milestone"
                        xNode.ForeColor = Color.Black
                        xNode.NodeFont = New Font(tvw_Items.Font.FontFamily, tvw_Items.Font.Size - 1, FontStyle.Italic)
                End Select
            End If

            'Override color if missing Dates or Type
            Dim strRef As String() = {"Category", "Reference", "Activity Group"}
            Try
                If dr.Table.Columns.Contains("Type") Then
                    If String.IsNullOrEmpty(dr.Item("Type").ToString) Then
                        xNode.ForeColor = Color.Orange
                    Else
                        If Not strRef.Contains(dr.Item("Type").ToString) Then
                            If dr.Table.Columns.Contains("StartDate") Then
                                If String.IsNullOrEmpty(dr.Item("StartDate").ToString) Then
                                    xNode.ForeColor = Color.Orange
                                End If
                            End If
                            If dr.Table.Columns.Contains("EstTime") Then
                                If String.IsNullOrEmpty(dr.Item("EstTime").ToString) Then
                                    xNode.ForeColor = Color.Orange
                                End If
                            End If
                            If dr.Table.Columns.Contains("Phase") Then
                                If String.IsNullOrEmpty(dr.Item("Phase").ToString) Then
                                    xNode.ForeColor = Color.Orange
                                End If
                            End If
                            If dr.Table.Columns.Contains("Status") Then
                                If String.IsNullOrEmpty(dr.Item("Status").ToString) Then
                                    xNode.ForeColor = Color.Orange
                                End If
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

            'Expand
            If Not IsDBNull(dr.Item("Expanded")) AndAlso dr.Item("Expanded") = True Then
                'exTag.Update("Exp", "Expanded") '
                exTag.Add("Exp", "Expanded") 'Initialize to not expanded or ""...will correct below if not 'Expanded'
                xNode.Expand()
                'xNode.Tag = exTag
            Else
                exTag.Add("Exp", "") '
            End If

            xNode.Tag = exTag

        Catch ex As Exception
            MsgBox(xNode.FullPath & "        " & ex.Message)
        End Try

    End Sub

    Sub PopulateTreeView(treeView As TreeView, dt As DataTable, pathSeparator As Char) 'paths As IEnumerable(Of String), pathSeparator As Char)
        Try
            System.Diagnostics.Debug.WriteLine("--Adding Nodes Start--")
            Dim lastNode As TreeNode = Nothing
            Dim subPathAgg As String
            For Each nPath1 As DataRow In dt.Rows 'For Each path As String In paths

                For Each nPath As DataRow In dsSys.Tables("dtItems").Select("System='" & nPath1.Item("System").ToString & "'").CopyToDataTable.Rows
                    '[Move past the node if the parent Phase is 'Complete' and 'KeepAsRef' is False]
                    '--------------------------------------------------------------------------------
                    If nPath.Item("System").ToString = "PQ System" AndAlso nPath.Item("Item").ToString = "Comp Request Early Assessment" Then
                        Dim str As String = ""
                    End If

                    If nPath.Item("Phase").ToString = "Complete" Then
                        If Not IsDBNull(nPath.Item("KeepAsRef")) Then
                            If nPath.Item("KeepAsRef") = False Then Continue For
                        End If
                    End If
                    Dim pSearch As String = "[Parent]='" & _
                        nPath.Item("System").ToString & "' AND Item='" & nPath.Item("Parent").ToString & "'"
                    If dsSys.Tables("dtItems").Select(pSearch).Any Then
                        Dim nParent As DataRow = dsSys.Tables("dtItems").Select(pSearch).CopyToDataTable.Rows(0)
                        If nParent.Item("Phase").ToString = "Complete" Then
                            If Not IsDBNull(nParent.Item("KeepAsRef")) Then
                                If nParent.Item("KeepAsRef") = False Then Continue For
                            Else
                                Continue For
                            End If
                        End If
                    End If
                    '--------------------------------------------------------------------------------

                    subPathAgg = String.Empty
                    For Each subPath As String In nPath.Item("Path").ToString.Split(pathSeparator)
                        subPathAgg += subPath & pathSeparator
                        Dim nodes As TreeNode() = treeView.Nodes.Find(subPathAgg, True) 'Check to make sure node does not already exist
                        If nodes.Length = 0 Then 'did not find
                            If lastNode Is Nothing Then
                                lastNode = treeView.Nodes.Add(subPathAgg, subPath)
                                'New DataView(dsSys.Tables("dtItems"), FilterTxtGlobal, "Assessed, [Dwg Add Date] DESC, [DWG DC Number], Supplier ASC, [GO Issue Date]", DataViewRowState.CurrentRows).ToTable(True, "DWG DC Number", "Dwg Add Date", "GO DC Level", "GO Issue Date", "Supplier", "Supplier Name", "Assoc Name", "Assessed", "Cmp")
                            Else
                                lastNode = lastNode.Nodes.Add(subPathAgg, subPath)
                            End If
                            Call AddNodeColor(lastNode, nPath.Item("Path").ToString)
                        Else
                            lastNode = nodes(0)
                        End If

                        'Format Node
                        'If nPath.Item("Item").ToString = "Comp Request Early Assessment" Then
                        '    Dim str As String = ""
                        'End If
                        'Call AddNodeColor(lastNode, nPath.Item("Path").ToString)
                        'System.Diagnostics.Debug.WriteLine(subPathAgg.ToString & "  -  " & lastNode.Text)
                    Next
                    lastNode = Nothing
                Next
            Next
            System.Diagnostics.Debug.WriteLine("--Adding Nodes End--")
        Catch ex As Exception
            MsgBox(ex.StackTrace)
        End Try

    End Sub

    Private Sub PrintRecursive(ByVal n As TreeNode)
        'System.Diagnostics.Debug.WriteLine(n.Text)
        'MessageBox.Show(n.Text)
        Dim aNode As TreeNode
        For Each aNode In n.Nodes
            System.Diagnostics.Debug.WriteLine(aNode.FullPath)
            PrintRecursive(aNode)
        Next
    End Sub

    ' Call the procedure using the top nodes of the treeview.  
    Private Sub CallRecursive(ByVal aTreeView As TreeView)
        Dim n As TreeNode
        For Each n In aTreeView.Nodes
            PrintRecursive(n)
        Next
    End Sub

    Sub Refresh_Systems()

        'Get Systems
        If dsSys.Tables.Contains("dtSystems") Then dsSys.Tables("dtSystems").Clear() Else dsSys.Tables.Add("dtSystems")

        Dim Sql As String = "SELECT * FROM tbl_ToDoSystems " & _
            "WHERE Architect = '" & ss_User.Text & "' " & _
            "ORDER BY Architect, iIndex, System "

        Call Load_Data(Sql, dsSys.Tables("dtSystems"))

        'Get Off Days
        dtOffDays = New DataTable
        Sql = "SELECT * FROM tbl_ToDo_OffDays WHERE UserID IN ('All', '" & ss_User.Text & "') OR UserID IS NULL "
        Call Load_Data(Sql, dtOffDays)

    End Sub

    

    'Private Sub tvw_Items_AfterCollapse(sender As Object, e As TreeViewEventArgs) Handles tvw_Items.AfterCollapse
    '    If _ExpandedNodeList.Contains(e.Node.Tag.ToString) Then
    '        _ExpandedNodeList.Remove(e.Node.Tag.ToString)
    '    End If
    'End Sub

    'Private Sub tvw_Items_AfterExpand(sender As Object, e As TreeViewEventArgs) Handles tvw_Items.AfterExpand
    '    If Not _ExpandedNodeList.Contains(e.Node.Tag.ToString) Then
    '        _ExpandedNodeList.Add(e.Node.Tag.ToString)
    '    End If
    'End Sub

    Private Sub tvw_Items_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tvw_Items.AfterSelect

        If Reordering = True Then Exit Sub

        If Loading = True Then Exit Sub
        If IsNothing(tvw_Items) Then Exit Sub
        If tvw_Items.Nodes.Count = 0 Then Exit Sub
        If IsNothing(tvw_Items.SelectedNode) Then Exit Sub
        If IsDBNull(tvw_Items.SelectedNode.FullPath) Then Exit Sub
        'If tvw_Items.SelectedNode.Level = 0 Then Exit Sub

        tvw_Items.SelectedNode.BackColor = SystemColors.MenuHighlight
        tvw_Items.SelectedNode.ForeColor = Color.White
        'page_Structure_Paint(sender, Nothing)

        Dim exTag As mTag = CType(tvw_Items.SelectedNode.Tag, mTag)

        'Dim NodePath As String = tvw_Items.SelectedNode.FullPath
        'Dim NewString As String() = tvw_Items.SelectedNode.FullPath.ToString.Split(New String() {" || "}, StringSplitOptions.None)
        ss_System.Text = exTag.Get("Sys")
        ss_Parent.Text = exTag.Get("Par")
        ss_Item.Text = tvw_Items.SelectedNode.Text

        'Tag Info
        lbl_TagName.Text = ""
        lbl_TagPath.Text = ""
        lbl_TagExp.Text = ""
        lbl_TagName.Text = exTag.Get("Name")
        lbl_TagPath.Text = exTag.Get("Path")
        lbl_TagExp.Text = exTag.Get("Exp")

        'MsgBox("'" & tvw_Items.SelectedNode.FullPath.ToString & "'")

        Call SelectionChange_Handler()

        'If tvw_Items.SelectedNode.Nodes.Count > 0 Then tvw_Items.SelectedNode.Expand()

        tvw_Items.Refresh()
        'Call tvw_Items_DrawNode(sender, Nothing)
        'Call page_Structure_Paint(Nothing, Nothing)

    End Sub

    Sub SelectionChange_Handler()

        If Reordering = True Then Exit Sub
        If Loading = True Then Exit Sub
        If tab_Details.SelectedTab.Name = "page_OpenItems" Then GoTo Open_Items
        If IsNothing(tvw_Items) Then Exit Sub
        If tvw_Items.Nodes.Count = 0 Then Exit Sub
        If IsNothing(tvw_Items.SelectedNode) Then Exit Sub
        If IsDBNull(tvw_Items.SelectedNode.FullPath) Then Exit Sub
        'If tvw_Items.SelectedNode.Level = 0 Then Exit Sub

        Select Case tab_Details.SelectedTab.Name
            Case "page_Details"
                Call Get_Details()
            Case "page_MgmtView"
                Call Handler_CreateSchedule(page_MgmtView)
            Case "page_4Square"
                Call Get_FourSquare()
            Case "page_OpenItems"
                ctr_OpenItms.Paint_OpenItems(Date.Today)
                'Call Get_OpenItems
        End Select

        Exit Sub
Open_Items:
        ctr_OpenItms.dt = dsSys.Tables("dtItems")
        ctr_OpenItms.Paint_OpenItems(Date.Today)

    End Sub

    Sub Get_Details()

        Refreshing = True
        GoTo Clear_Labels

Refresh_Item_Data:

        'MsgBox(tvw_Items.SelectedNode.Level)


        dtCurrImages = New DataTable 'initialize the Current Selection's image datatable

        'If tvw_Items.SelectedNode.Nodes.Count = 0 Then 'Does not have Children means it is the last node

        'Put together value string and pass to SQL Function
        Try

            'Load Notes
            Dim exTag As mTag = CType(tvw_Items.SelectedNode.Tag, mTag)

            Dim strSrch As String = "System='" & exTag.Get("Sys") & "'"
            If dsSys.Tables("dtSystems").Select(strSrch).Any Then
                If Not String.IsNullOrEmpty(dsSys.Tables("dtSystems").Select(strSrch).CopyToDataTable.Rows(0).Item("Description").ToString) Then
                    txt_Item.Text = dsSys.Tables("dtSystems").Select(strSrch).CopyToDataTable.Rows(0).Item("Description").ToString
                End If
            End If

            strSrch = "System='" & exTag.Get("Sys") & "' AND Item='" & ss_Item.Text & "' AND [Parent]='" & exTag.Get("Par") & "'"
            If dsSys.Tables("dtItems").Select(strSrch).Any Then
                Dim drItem As DataRow
                drItem = dsSys.Tables("dtItems").Select(strSrch).CopyToDataTable.Rows(0)
                txt_Item.Text = drItem.Item("Item").ToString
                If Not String.IsNullOrEmpty(drItem.Item("Notes").ToString) Then rTxt_Notes.Rtf =
                    ConvertTextToRTF(drItem.Item("Notes"))

                If Not String.IsNullOrEmpty(drItem.Item("Scope").ToString) Then rTxt_Scope.Rtf =
                    ConvertTextToRTF(drItem.Item("Scope"))

                combo_Phase.SelectedValue = If(String.IsNullOrEmpty(drItem.Item("Phase").ToString), -1, drItem.Item("Phase"))
                cbo_Status.Text = If(String.IsNullOrEmpty(drItem.Item("Status").ToString), "", drItem.Item("Status"))
                label_Index.Text = If(String.IsNullOrEmpty(drItem.Item("iIndex").ToString), "", drItem.Item("iIndex"))
                txt_Requester.Text = If(String.IsNullOrEmpty(drItem.Item("Requester").ToString), "", drItem.Item("Requester"))
                txt_RequestedDue.Text = If(String.IsNullOrEmpty(drItem.Item("RequestDue").ToString), "", drItem.Item("RequestDue"))
                txt_EstUseDate.Text = If(String.IsNullOrEmpty(drItem.Item("EstUseDate").ToString), "", drItem.Item("EstUseDate"))
                txt_EstTime.Text = If(String.IsNullOrEmpty(drItem.Item("EstTime").ToString), "", drItem.Item("EstTime"))
                cbo_UM.Text = If(String.IsNullOrEmpty(drItem.Item("EstTimeUM").ToString), "day(s)", drItem.Item("EstTimeUM"))
                txt_StartDate.Text = If(String.IsNullOrEmpty(drItem.Item("StartDate").ToString), "", drItem.Item("StartDate"))
                txt_DueDate.Text = If(IsDate(drItem.Item("StartDate")), Calc_DueDate(drItem.Item("StartDate"), drItem.Item("EstTime"), drItem.Item("EstTimeUM")), "") 'If(String.IsNullOrEmpty(drItem.Item("DueDate").ToString), "", drItem.Item("DueDate"))

                txt_ActualStart.Text = If(String.IsNullOrEmpty(drItem.Item("ActualStart").ToString), "", drItem.Item("ActualStart"))
                txt_ActualEnd.Text = If(String.IsNullOrEmpty(drItem.Item("ActualEnd").ToString), "", drItem.Item("ActualEnd"))
                txt_ActualTime.Text = If(String.IsNullOrEmpty(drItem.Item("ActualTime").ToString), "", drItem.Item("ActualTime"))
                cbo_ActualUM.Text = If(String.IsNullOrEmpty(drItem.Item("ActualUM").ToString), "day(s)", drItem.Item("ActualUM"))

                txt_PublishDate.Text = If(String.IsNullOrEmpty(drItem.Item("PublishDate").ToString), "", drItem.Item("PublishDate"))
                txt_Complexity.Text = If(String.IsNullOrEmpty(drItem.Item("Complexity").ToString), "", drItem.Item("Complexity"))
                txt_Urgency.Text = If(String.IsNullOrEmpty(drItem.Item("Urgency").ToString), "", drItem.Item("Urgency"))
                txt_PercentComplete.Text = If(String.IsNullOrEmpty(drItem.Item("PercentComplete").ToString), "", drItem.Item("PercentComplete"))
                cbo_Type.Text = If(String.IsNullOrEmpty(drItem.Item("Type").ToString), "", drItem.Item("Type"))
                check_Show.Checked = If(String.IsNullOrEmpty(drItem.Item("ShowInList").ToString), False, drItem.Item("ShowInList"))
                chk_Reference.Checked = If(String.IsNullOrEmpty(drItem.Item("KeepAsRef").ToString), False, drItem.Item("KeepAsRef"))
                chk_MgmtReview.Checked = If(String.IsNullOrEmpty(drItem.Item("MgmtReview").ToString), False, drItem.Item("MgmtReview"))
                rTxt_MgmtRevNotes.Rtf = If(Not String.IsNullOrEmpty(drItem.Item("MgmtRevNotes").ToString), ConvertTextToRTF(drItem.Item("MgmtRevNotes")), "")
                txt_Priority.Text = If(String.IsNullOrEmpty(drItem.Item("Pri").ToString), "", drItem.Item("Pri"))
                lbl_LastSaved.Text = If(IsDate(drItem.Item("LastUpdate")), CDate(drItem.Item("LastUpdate")), "")

                'Load Images & Image Notes
                ctrImg_Tasks.Get_Images(ss_System.Text, ss_Parent.Text, ss_Item.Text)

                'Load Task Journal entries
                Dim strJrnl As String = "Task='" & tvw_Items.SelectedNode.Text & "' AND Path='" & exTag.Get("Path") & "' AND Architect='" & ss_User.Text & "' "
                dgv_TaskJournal.DataSource = New DataView(dtTaskJournal, strJrnl, "WorkDtPlan ASC", DataViewRowState.CurrentRows).ToTable(True, "WorkDescription", "WorkDtPlan", "WorkHrsPlan", "WorkDtActual", "WorkHrsActual", "Notes", "id")
                dgv_TaskJournal.Columns("id").DefaultCellStyle.ForeColor = Color.Gray

                'if the Header tab is selected, Create the Project View
                If tab_DetailsMain.SelectedTab.Text = "Schedule" Then

                    Call Handler_CreateSchedule(pnl_Sch)

                End If

            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        'Else 'expand the view
        'tvw_Items.SelectedNode.Expand()

        'End If

        Refreshing = False

        Exit Sub

Clear_Labels:

        'ss_Item.Text = ""
        txt_Item.Text = ""
        rTxt_Notes.Text = ""
        rTxt_Scope.Text = ""
        ctrImg_Tasks.rtxt_ImageNotes.Text = ""
        combo_Phase.SelectedIndex = -1
        cbo_Type.Text = ""
        cbo_Status.Text = ""
        label_Index.Text = ""
        lbl_LastSaved.Text = ""
        txt_Requester.Text = ""
        txt_RequestedDue.Text = ""
        txt_EstTime.Text = ""
        txt_DueDate.Text = ""
        txt_ActualEnd.Text = ""
        txt_ActualStart.Text = ""
        txt_ActualTime.Text = ""
        cbo_ActualUM.Text = ""
        txt_PublishDate.Text = ""
        txt_EstTime.Text = ""
        txt_EstUseDate.Text = ""
        rTxt_MgmtRevNotes.Text = ""
        cbo_UM.Text = ""
        ctrImg_Tasks.pic_PartImage.Image = Nothing
        ctrImg_Tasks.label_ImageNum.Text = ""
        ctrImg_Tasks.label_Image.Text = ""
        ctrImg_Tasks.label_ImageTotQty.Text = ""
        check_Show.Checked = False
        chk_MgmtReview.Checked = False
        txt_Complexity.Text = ""
        txt_Urgency.Text = ""
        txt_PercentComplete.Text = ""
        chk_Reference.Checked = False
        txt_Priority.Text = ""
        dgv_TaskJournal.DataSource = Nothing

        GoTo Refresh_Item_Data

    End Sub

    Sub Get_MgmtReview()



    End Sub

    Sub Get_OpenItems()

        'If strSrch = Nothing Then Exit Sub

        'If dsSys.Tables("dtItems").Select(strSrch).Any Then

        '    Dim drItem As DataRow = dsSys.Tables("dtItems").Select(strSrch).CopyToDataTable.Rows(0)

        '    '"Type IN ('System Feature', 'System', 'Enhancement') AND ShowInList='True' AND (Status NOT IN ('Complete', 'On Hold') OR (Status='Complete' AND MgmtReview='False'))"
        '    Dim chkStr As String() = {"System Feature", "System", "Enhancement"}

        '    If (chkStr.Contains(drItem.Item("Type").ToString) AndAlso drItem.Item("ShowInList").ToString =
        '        "True") OrElse drItem.Item("Type").ToString = "Phase" Then

        '        If tab_Main.SelectedTab.Text = "Mgmt View" Then
        '            rTxt_ScopeMgmt.Rtf = If(Not IsDBNull(drItem.Item("Scope")), ConvertTextToRTF(drItem.Item("Scope")), "")
        '            rTxt_MgtRvwNotes.Rtf = If(Not IsDBNull(drItem.Item("MgmtRevNotes")), ConvertTextToRTF(drItem.Item("MgmtRevNotes")), "")
        '        End If

        '        Dim ctrSch As New ctrl_OpenItems
        '        ctrSch.System = drItem.Item("System").ToString
        '        ctrSch.Path = If(drItem.Item("Type").ToString = "Phase", tvw_Items.SelectedNode.Parent.FullPath, drItem.Item("Path").ToString)
        '        ctrSch.Item = If(drItem.Item("Type").ToString = "Phase", tvw_Items.SelectedNode.Parent.Text, drItem.Item("Item").ToString)
        '        ctrSch.Type(0) = drItem.Item("Type").ToString 'Keeps Type
        '        ctrSch.Type(1) = If(drItem.Item("Type").ToString = "Phase", tvw_Items.SelectedNode.Text, "") 'Keeps Item Name for the Type (so i know it is 'Initial', 'Devel.' later in processing [for coloring]
        '        ctrSch.Width = fCtrl.Width
        '        fCtrl.Controls.Add(ctrSch)
        '        ctrSch.Paint_Schedule()
        '    End If
        'End If

    End Sub

    Sub Handler_CreateSchedule(fCtrl As Control)

        If Loading = True Then Exit Sub
        If IsNothing(fCtrl) Then Exit Sub
        If IsNothing(tvw_Items.SelectedNode) Then Exit Sub

        fCtrl.Controls.Clear()

        Dim exTag As mTag = CType(tvw_Items.SelectedNode.Tag, mTag)

        Dim strSrch As String = Nothing
        Select Case tab_Main.SelectedTab.Text
            Case "Mgmt View"
                Dim dgRow As DataGridViewRow = dgv_Mgmt.SelectedRows(0)
                strSrch = "System='" & dgRow.Cells("System").Value & "' AND Item='" & dgRow.Cells("Item").Value & "' AND [Path]='" & dgRow.Cells("Path").Value & "'"
            Case "Main"
                If IsNothing(tvw_Items.SelectedNode) Then Exit Sub
                strSrch = "System='" & ss_System.Text & "' AND Item='" & ss_Item.Text & "' AND [Parent]='" & exTag.Get("Par") & "'"
        End Select

        If strSrch = Nothing Then Exit Sub

        If dsSys.Tables("dtItems").Select(strSrch).Any Then

            Dim drItem As DataRow = dsSys.Tables("dtItems").Select(strSrch).CopyToDataTable.Rows(0)

            '"Type IN ('System Feature', 'System', 'Enhancement') AND ShowInList='True' AND (Status NOT IN ('Complete', 'On Hold') OR (Status='Complete' AND MgmtReview='False'))"
            Dim chkStr As String() = {"System Feature", "System", "Enhancement"}
            Dim chkStr2 As String() = {"Phase", "Activity", "Task"}

            If (chkStr.Contains(drItem.Item("Type").ToString) AndAlso drItem.Item("ShowInList").ToString =
                "True") OrElse chkStr2.Contains(drItem.Item("Type").ToString) Then

                If tab_Main.SelectedTab.Text = "Mgmt View" Then
                    rTxt_ScopeMgmt.Rtf = If(Not IsDBNull(drItem.Item("Scope")), ConvertTextToRTF(drItem.Item("Scope")), "")
                    rTxt_MgtRvwNotes.Rtf = If(Not IsDBNull(drItem.Item("MgmtRevNotes")), ConvertTextToRTF(drItem.Item("MgmtRevNotes")), "")
                End If

                'Set Variables
                Dim Pth As String = ""
                Dim Itm As String = ""
                Dim Typ As String() = {drItem.Item("Type").ToString, "", ""}

                Select Case drItem.Item("Type").ToString
                    Case "System", "System Feature", "Enhancement"
                        Pth = drItem.Item("Path").ToString
                        Itm = drItem.Item("Item").ToString
                    Case "Phase", "Activity Group"
                        Pth = drItem.Item("Path").ToString 'tvw_Items.SelectedNode.Parent.FullPath
                        Itm = drItem.Item("Item").ToString 'tvw_Items.SelectedNode.Parent.Text
                        Typ(1) = tvw_Items.SelectedNode.Text 'Selected Node Text
                        Typ(2) = drItem.Item("Path").ToString  'My Path
                    Case "Activity", "Task"
                        Dim tPth As String() = PhaseNameFromPath(drItem.Item("Path").ToString)
                        Pth = If(String.IsNullOrEmpty(tPth(1)), drItem.Item("Path").ToString, tPth(1)) '0=Name, 1=Path
                        Itm = If(String.IsNullOrEmpty(tPth(0)), drItem.Item("Item").ToString, tPth(0))
                        Typ(1) = tvw_Items.SelectedNode.Text
                        Typ(2) = drItem.Item("Path").ToString
                End Select


                Dim ctrSch As New ctrl_Schedule
                ctrSch.System = drItem.Item("System").ToString
                ctrSch.Path = Pth
                ctrSch.Item = Itm
                ctrSch.Type(0) = Typ(0) 'Keeps Type
                ctrSch.Type(1) = Typ(1) 'Keeps Item Name for the Type (so i know it is 'Initial', 'Devel.' later in processing [for coloring]
                ctrSch.Type(2) = Typ(2) 'Keeps Item Name for the Type (so i know it is 'Initial', 'Devel.' later in processing [for coloring]
                ctrSch.Width = fCtrl.Width
                fCtrl.Controls.Add(ctrSch)
                ctrSch.Paint_Schedule()
            End If
        End If

    End Sub


    Sub Get_FourSquare()
        If Loading = True Then Exit Sub

        'If selected a different system, update 4-Square data
        If label_System.Text = ss_System.Text Then Exit Sub

        'FIELDS:  System, Architect, Description, ApplyBy, iIndex, Expanded, Objective, Benefits, Accomplishments, Notes, IssuesRisks, Changes, Department, Users, PIC, Developer, Status, Milestone_Initial, Milestone_Plan, Milestone_Programming, Milestone_Beta, Milestone_RollOut
        label_System.Text = ss_System.Text
        GoTo Clear_Labels

Update_Labels:

        Dim strSrch As String = "System='" & ss_System.Text & "'"
        If dsSys.Tables("dtSystems").Select(strSrch).Any Then
            Dim drItem As DataRow
            drItem = dsSys.Tables("dtSystems").Select(strSrch).CopyToDataTable.Rows(0)

            'Get Panel Sizes and Locations
            If Not String.IsNullOrEmpty(drItem.Item("FourSquareLayout").ToString) Then
                Dim pnlNames As String() = {"panel_Objective", "panel_Notes", "panel_Issues", "panel_Milestones"}
                Dim ctrPanel As Panel
                Dim xPanels As String() = drItem.Item("FourSquareLayout").ToString.Split(";") 'Split(New Char() {";"c})
                For Each nPanel In xPanels
                    ctrPanel = DirectCast(page_4Square.Controls(pnlNames(CInt(Array.IndexOf(xPanels, nPanel)))), Panel)
                    Dim xAttr As String() = nPanel.ToString.Split(",")
                    ctrPanel.Top = CInt(xAttr(0))
                    ctrPanel.Left = CInt(xAttr(1))
                    ctrPanel.Height = CInt(xAttr(2))
                    ctrPanel.Width = CInt(xAttr(3))
                    ctrPanel.Refresh()
                    'MsgBox(Array.IndexOf(xPanels, nPanel) & nPanel.ToString)
                Next
            Else
                'set to Default Sizes:
                '0 - 46, 3, 190, 398 
                '1 - 242, 3, 164, 398 
                '2 - 46, 407, 190, 333 
                '3 - 242, 407, 164, 333
            End If



            'Populate Data
            Dim FilterTxt As String = strSrch & " AND NOT Phase='Complete' AND DueDate IS NOT NULL"

            If Not String.IsNullOrEmpty(drItem.Item("Objective").ToString) Then
                rText_Objective.Rtf = ConvertTextToRTF(drItem.Item("Objective"))
            End If
            If Not String.IsNullOrEmpty(drItem.Item("Benefits").ToString) Then
                rText_Benefits.Rtf = ConvertTextToRTF(drItem.Item("Benefits"))
            End If
            If Not String.IsNullOrEmpty(drItem.Item("Accomplishments").ToString) Then
                rText_Accomplish.Rtf = ConvertTextToRTF(drItem.Item("Accomplishments"))
            End If
            If Not String.IsNullOrEmpty(drItem.Item("Notes").ToString) Then 'Commentary
                rText_Notes.Rtf = ConvertTextToRTF(drItem.Item("Notes"))
            End If
            If Not String.IsNullOrEmpty(drItem.Item("IssuesRisks").ToString) Then
                rText_Issues.Rtf = ConvertTextToRTF(drItem.Item("IssuesRisks"))
            End If
            If Not String.IsNullOrEmpty(drItem.Item("Changes").ToString) Then
                rText_Changes.Rtf = ConvertTextToRTF(drItem.Item("Changes"))
            End If

            'Accomplishments
            FilterTxt = strSrch & " AND Phase='Complete'"
            Dim dtAcc As DataTable = New DataView(dsSys.Tables("dtItems"), FilterTxt, "PublishDate DESC", DataViewRowState.CurrentRows).ToTable(True, "PublishDate", "Item", "Parent")
            dgv_Accomplishments.DataSource = dtAcc

            'Timing
            FilterTxt = strSrch & " AND NOT Phase='Complete' AND DueDate IS NOT NULL"
            Dim dtTime As DataTable = New DataView(dsSys.Tables("dtItems"), FilterTxt, "DueDate", DataViewRowState.CurrentRows).ToTable(True, "DueDate", "Item", "Parent")
            dgv_Timing.DataSource = dtTime

        End If

        Exit Sub

Clear_Labels:

        rText_Objective.Rtf = ""
        rText_Benefits.Rtf = ""
        rText_Accomplish.Rtf = ""
        rText_Notes.Rtf = ""
        rText_Issues.Rtf = ""
        rText_Changes.Rtf = ""
        dgv_Accomplishments.DataSource = Nothing
        dgv_Timing.DataSource = Nothing

        GoTo Update_Labels

    End Sub

    Private Sub tsbtn_Save_Click(sender As Object, e As EventArgs) Handles tsbtn_Save.Click

        'Dim saved As Boolean = False
        Select Case tab_Main.SelectedTab.Text
            Case "Main"
                Select Case tab_Details.SelectedTab.Text
                    Case "Details"
                        Call Save_Details()
                        Call Load_Items()
                        Call Load_Images()
                        If tab_DetailsMain.SelectedTab.Text = "Schedule" Then
                            Call Handler_CreateSchedule(pnl_Sch)
                        End If
                        'saved = True
                    Case "4-Square"
                        Call Save_4Square()
                        Call Refresh_Systems()
                        'saved = True
                End Select
            Case "Weekly Plan"
                Call Save_Weekly()

            Case "Calendar"
                Call Save_CalendarPlan()

        End Select


        'If saved = True Then Call Load_Items()

    End Sub

    Sub Save_Details()

        ss_Saving.Visible = True
        StatusStrip1.Refresh()
        'MsgBox(tvw_Items.SelectedNode.Parent.FullPath)
        Dim NewString As String()

        Dim iCmd As New SqlCommand

        Dim exTag As mTag = CType(tvw_Items.SelectedNode.Tag, mTag)

        'Update Item Name if Necessary
        '---Only works if Structure is selected--
        Select Case tab_List.SelectedTab.Text
            Case "Structure"
                If IsNothing(tvw_Items.SelectedNode) Then Exit Sub

                'Save Expanded for System if selected node is Level 1
                iCmd.CommandText = "UPDATE tbl_ToDoSystems " & _
                        "SET Expanded=@Exp WHERE System=@Sys "
                iCmd.Parameters.AddWithValue("@Sys", tvw_Items.SelectedNode.Text)
                iCmd.Parameters.AddWithValue("@Exp", If(tvw_Items.SelectedNode.IsExpanded, True, False))
                Call WriteUpdateSQL(iCmd)
                iCmd.Parameters.Clear()



                Select Case tvw_Items.SelectedNode.Level
                    Case 1 'System Name
                        If Not String.IsNullOrEmpty(txt_Item.Text) Then
                            'Update the Phase
                            iCmd.CommandText = "UPDATE tbl_ToDoSystems " & _
                                        "SET Phase=@Phs WHERE System=@SysOr "
                            iCmd.Parameters.AddWithValue("@SysOr", tvw_Items.SelectedNode.Text)
                            iCmd.Parameters.AddWithValue("@Phs", combo_Phase.Text)
                            Call WriteUpdateSQL(iCmd)

                            'Update the System Name if it does not match
                            If txt_Item.Text <> tvw_Items.SelectedNode.Text Then
                                iCmd.CommandText = "UPDATE tbl_ToDoSystems " & _
                                        "SET System=@SysNw WHERE System=@SysOr "
                                iCmd.Parameters.AddWithValue("@SysNw", txt_Item.Text)
                                iCmd.Parameters.AddWithValue("@SysOr", tvw_Items.SelectedNode.Text)
                                Call WriteUpdateSQL(iCmd)

                                iCmd.CommandText = "UPDATE tbl_ToDoItems " & _
                                        "SET System=@SysNw " & _
                                        "WHERE System=@SysOr "
                                Call WriteUpdateSQL(iCmd)

                                iCmd.CommandText = "UPDATE tbl_ToDoItems " & _
                                        "SET [Parent]=@SysNw " & _
                                        "WHERE [Parent]=@SysOr "
                                Call WriteUpdateSQL(iCmd)

                                iCmd.Dispose()
                                tvw_Items.SelectedNode.Text = txt_Item.Text
                                Call Refresh_Systems()
                            End If
                        End If
                    Case Else 'Items
                        If txt_Item.Text <> tvw_Items.SelectedNode.Text Then
                            If Not String.IsNullOrEmpty(txt_Item.Text) Then 'verify new name is not empty
                                NewString = tvw_Items.SelectedNode.FullPath.Split(New String() {" || "}, StringSplitOptions.None)
                                iCmd.CommandText = "UPDATE tbl_ToDoItems " & _
                                        "SET Item=@ItmNw " & _
                                        "WHERE System=@Sys AND Item=@ItmOr AND [Parent]=@Par "
                                iCmd.Parameters.AddWithValue("@Sys", exTag.Get("Sys"))
                                iCmd.Parameters.AddWithValue("@ItmNw", txt_Item.Text)
                                iCmd.Parameters.AddWithValue("@ItmOr", tvw_Items.SelectedNode.Text)
                                iCmd.Parameters.AddWithValue("@Par", exTag.Get("Par"))
                                Call WriteUpdateSQL(iCmd)
                                iCmd.Parameters.Clear()

                                'If Parent, update Child Items
                                If tvw_Items.SelectedNode.Nodes.Count > 0 Then
                                    iCmd.CommandText = "UPDATE tbl_ToDoItems " & _
                                        "SET [Parent]=@ParNw " & _
                                        "WHERE System=@Sys AND [Parent]=@ParOr "
                                    iCmd.Parameters.AddWithValue("@Sys", exTag.Get("Sys"))
                                    iCmd.Parameters.AddWithValue("@ParNw", exTag.Get("Par") & " || " & txt_Item.Text)
                                    'iCmd.Parameters.AddWithValue("@ItmOr", tvw_Items.SelectedNode.Text)
                                    iCmd.Parameters.AddWithValue("@ParOr", exTag.Get("Par"))
                                    Call WriteUpdateSQL(iCmd)
                                End If
                                tvw_Items.SelectedNode.Text = txt_Item.Text
                                iCmd.Dispose()
                                'Call Load_Items()
                            End If
                        End If
                End Select
        End Select

        'Select Case tvw_Items.SelectedNode.Level
        '    Case 1 'Systems Header
        '    Case 2 '
        '    Case Is > 2

        'End Select

        ''Check to make sure Due Date and Publish Date texts are a dates
        Dim dDate As Date = Nothing 'for Due Date
        If Not String.IsNullOrEmpty(txt_DueDate.Text) Then If IsDate(txt_DueDate.Text) Then dDate = CDate(txt_DueDate.Text).ToShortDateString

        Dim pDate As Date = Nothing 'for Publish Date
        If Not String.IsNullOrEmpty(txt_PublishDate.Text) Then If IsDate(txt_PublishDate.Text) Then pDate = CDate(txt_PublishDate.Text).ToShortDateString

        Dim sDate As Date = Nothing 'for Publish Date
        If Not String.IsNullOrEmpty(txt_StartDate.Text) Then If IsDate(txt_StartDate.Text) Then sDate = CDate(txt_StartDate.Text).ToShortDateString

        ''verify something is selected before saving
        Dim Xpanded As Boolean = True
        Select Case tab_List.SelectedTab.Text
            Case "Structure"
                If IsNothing(tvw_Items.SelectedNode) Then Exit Sub
                Xpanded = If(tvw_Items.SelectedNode.IsExpanded, True, False)
            Case "List by Date"
                'verify something is selected before saving
                If dgv_List.SelectedCells.Count = 0 Then Exit Sub
            Case "Complete"
                If dgv_Complete.SelectedCells.Count = 0 Then Exit Sub
        End Select

        Dim uCmd As New SqlCommand 'UPDATE Query
        uCmd.CommandText = "SET TRANSACTION ISOLATION LEVEL SERIALIZABLE BEGIN TRANSACTION " &
                            "IF EXISTS (SELECT 1 FROM tbl_ToDoItems WHERE System=@Sys AND Item=@Itm AND [Parent]=@Par) " &
                            "BEGIN   UPDATE tbl_ToDoItems " &
                            "SET Path=@Pth, iIndex=@Ind, Phase=@Phs, Status=@Stat, EstTime=@Time, EstTimeUM=@UM, StartDate=@Strt, DueDate=@Due, ActualStart=@AStrt, ActualEnd=@AEnd, PublishDate=@Pub, Requester=@Req, Notes=@Note, Scope=@Scp, Expanded=@Exp, LastUpdate=@UpDt, RequestDue=@RDue, EstUseDate=@eDuDt, ShowInList=@Shw, MgmtReview=@Mgmt, MgmtRevNotes=@mNote, Complexity=@Cplx, Urgency=@Urg, Type=@Typ, PercentComplete=@PCmp, KeepAsRef=@Ref, OverallPriority=@Pri " &
                            "WHERE System=@Sys AND Item=@Itm AND [Parent]=@Par       END " &
                            "ELSE BEGIN INSERT INTO tbl_ToDoItems (System, Item, [Parent], Path, iIndex, EstTime, EstTimeUM, StartDate, DueDate, ActualStart, ActualEnd, PublishDate, Phase, Status, Requester, Notes, Scope,  LastUpdate, RequestDue, EstUseDate, ShowInList, MgmtReview, MgmtRevNotes, Complexity, Urgency, Type, PercentComplete, KeepAsRef, OverallPriority) " &
                            "   VALUES (@Sys, @Itm, @Par, @Pth, @Ind, @Time, @UM, @Strt, @Due, @AStrt, @AEnd, @Pub, @Phs, @Stat, @Req, @Note, @Scp, @UpDt, @RDue, @eDuDt, @Shw, @Mgmt, @mNote, @Cplx, @Urg, @Typ, @PCmp, @Ref, @Pri)      END " &
                            "COMMIT TRANSACTION "

        uCmd.Parameters.AddWithValue("@Sys", ss_System.Text)
        uCmd.Parameters.AddWithValue("@Itm", ss_Item.Text)
        uCmd.Parameters.AddWithValue("@Par", exTag.Get("Par"))
        uCmd.Parameters.AddWithValue("@Pth", tvw_Items.SelectedNode.FullPath)
        uCmd.Parameters.AddWithValue("@Ind", If(label_Index.Text = String.Empty, DBNull.Value, CInt(label_Index.Text)))
        uCmd.Parameters.AddWithValue("@Phs", combo_Phase.Text)
        uCmd.Parameters.AddWithValue("@Stat", cbo_Status.Text)
        uCmd.Parameters.AddWithValue("@Time", If(String.IsNullOrEmpty(txt_EstTime.Text), DBNull.Value, txt_EstTime.Text))
        uCmd.Parameters.AddWithValue("@UM", If(String.IsNullOrEmpty(cbo_UM.Text), DBNull.Value, cbo_UM.Text))
        uCmd.Parameters.AddWithValue("@Strt", If(sDate = Nothing, DBNull.Value, sDate))
        uCmd.Parameters.AddWithValue("@Due", If(dDate = Nothing, DBNull.Value, dDate))
        uCmd.Parameters.AddWithValue("@AStrt", If(Not String.IsNullOrEmpty(txt_ActualStart.Text), If(IsDate(txt_ActualStart.Text), CDate(txt_ActualStart.Text).ToShortDateString, DBNull.Value), DBNull.Value))
        uCmd.Parameters.AddWithValue("@AEnd", If(Not String.IsNullOrEmpty(txt_ActualEnd.Text), If(IsDate(txt_ActualEnd.Text), CDate(txt_ActualEnd.Text).ToShortDateString, DBNull.Value), DBNull.Value))
        uCmd.Parameters.AddWithValue("@Pub", If(pDate = Nothing, DBNull.Value, pDate))
        uCmd.Parameters.AddWithValue("@Req", If(String.IsNullOrEmpty(txt_Requester.Text), DBNull.Value, txt_Requester.Text))
        uCmd.Parameters.AddWithValue("@Note", If(String.IsNullOrEmpty(rTxt_Notes.Text), DBNull.Value, rTxt_Notes.Rtf))
        uCmd.Parameters.AddWithValue("@Scp", rTxt_Scope.Rtf)
        uCmd.Parameters.AddWithValue("@Exp", Xpanded)
        uCmd.Parameters.AddWithValue("@Shw", check_Show.Checked)
        uCmd.Parameters.AddWithValue("@Mgmt", chk_MgmtReview.Checked)
        uCmd.Parameters.AddWithValue("@mNote", If(String.IsNullOrEmpty(rTxt_MgmtRevNotes.Text), DBNull.Value, rTxt_MgmtRevNotes.Rtf))
        uCmd.Parameters.AddWithValue("@RDue", If(String.IsNullOrEmpty(txt_RequestedDue.Text), DBNull.Value, txt_RequestedDue.Text))
        uCmd.Parameters.AddWithValue("@eDuDt", If(IsDate(txt_EstUseDate.Text), CDate(txt_EstUseDate.Text).ToShortDateString, DBNull.Value))
        uCmd.Parameters.AddWithValue("@Cplx", If(String.IsNullOrEmpty(txt_Complexity.Text), DBNull.Value, txt_Complexity.Text))
        uCmd.Parameters.AddWithValue("@Urg", If(String.IsNullOrEmpty(txt_Urgency.Text), DBNull.Value, txt_Urgency.Text))
        uCmd.Parameters.AddWithValue("@Typ", If(String.IsNullOrEmpty(cbo_Type.Text), DBNull.Value, cbo_Type.Text))
        uCmd.Parameters.AddWithValue("@Pri", If(String.IsNullOrEmpty(txt_Priority.Text), DBNull.Value, CInt(txt_Priority.Text)))
        uCmd.Parameters.AddWithValue("@PCmp", If(String.IsNullOrEmpty(txt_PercentComplete.Text), DBNull.Value, _
                                                 txt_PercentComplete.Text & If(InStr(txt_PercentComplete.Text, "%") > 0, "", "%")))
        uCmd.Parameters.AddWithValue("@Ref", chk_Reference.Checked)
        uCmd.Parameters.AddWithValue("@UpDt", Now())
        Call WriteUpdateSQL(uCmd)
        uCmd.Parameters.Clear()

        'Update the Image Notes
        If Not String.IsNullOrEmpty(ctrImg_Tasks.label_Image.Text) Then
            uCmd.CommandText = "UPDATE tbl_ToDoImages " & _
                            "SET ImageNotes = @Nt " & _
                            "WHERE System=@Sys AND ImageID=@Idx "
            ' Replace 8000, below, with the correct size of the field
            uCmd.Parameters.AddWithValue("@Sys", ss_System.Text)
            uCmd.Parameters.AddWithValue("@Idx", CInt(ctrImg_Tasks.label_Image.Text))
            uCmd.Parameters.AddWithValue("@Nt", ctrImg_Tasks.rtxt_ImageNotes.Rtf)
            Call WriteUpdateSQL(uCmd)
        End If

        uCmd.Dispose()
        Call Load_Items()

        ss_Saving.Visible = False
        StatusStrip1.Refresh()

    End Sub

    Sub Save_4Square()

        'Save 4-Square System Data

        'Save the data and size and location of the 4 panels that make up the 4-Square presentation
        Dim FSL As String = ""
        'Structure will be 'Top, Left, Height, Width' ';'  for each panel (separated by semicolon)
        FSL = panel_Objective.Top & ", " & panel_Objective.Left & ", " & panel_Objective.Height & ", " & panel_Objective.Width & "; "
        FSL = FSL & panel_Notes.Top & ", " & panel_Notes.Left & ", " & panel_Notes.Height & ", " & panel_Notes.Width & "; "
        FSL = FSL & panel_Issues.Top & ", " & panel_Issues.Left & ", " & panel_Issues.Height & ", " & panel_Issues.Width & "; "
        FSL = FSL & panel_Milestones.Top & ", " & panel_Milestones.Left & ", " & panel_Milestones.Height & ", " & panel_Milestones.Width


        Dim iCmd As New SqlCommand
        'FIELDS:  System, Architect, Description, ApplyBy, iIndex, Expanded, FourSquareLayout, Objective, Benefits, Accomplishments, Notes, IssuesRisks, Changes, Department, Users, PIC, Developer, Status, Milestone_Initial, Milestone_Plan, Milestone_Programming, Milestone_Beta, Milestone_RollOut
        iCmd.CommandText = "UPDATE tbl_ToDoSystems " & _
                "SET FourSquareLayout=@FSL, Objective=@Obj, Benefits=@Ben, Accomplishments=@Acc, Notes=@Nt, IssuesRisks=@Rsk, " & _
                "    Changes=@Chg, Department=@Dept, Users=@Usr, PIC=@PIC, Developer=@Dev, Status=@Sts, Milestone_Initial=@mIni, " & _
                "    Milestone_Plan=@mPln, Milestone_Programming=@mPrg, Milestone_Beta=@mBta, Milestone_RollOut=@mRO " & _
                "WHERE System=@Sys AND Architect = '" & ss_User.Text & "' "
        iCmd.Parameters.AddWithValue("@Sys", ss_System.Text)
        iCmd.Parameters.AddWithValue("@FSL", FSL) 'Four Square Layout
        iCmd.Parameters.AddWithValue("@Obj", rText_Objective.Rtf)
        iCmd.Parameters.AddWithValue("@Ben", rText_Benefits.Rtf)
        iCmd.Parameters.AddWithValue("@Acc", rText_Accomplish.Rtf)
        iCmd.Parameters.AddWithValue("@Nt", rText_Notes.Rtf)
        iCmd.Parameters.AddWithValue("@Rsk", rText_Issues.Rtf)
        iCmd.Parameters.AddWithValue("@Chg", rText_Changes.Rtf)
        iCmd.Parameters.AddWithValue("@Dept", "")
        iCmd.Parameters.AddWithValue("@Usr", "")
        iCmd.Parameters.AddWithValue("@PIC", "")
        iCmd.Parameters.AddWithValue("@Dev", "")
        iCmd.Parameters.AddWithValue("@Sts", "")
        iCmd.Parameters.AddWithValue("@mIni", "")
        iCmd.Parameters.AddWithValue("@mPln", "")
        iCmd.Parameters.AddWithValue("@mPrg", "")
        iCmd.Parameters.AddWithValue("@mBta", "")
        iCmd.Parameters.AddWithValue("@mRO", "")
        Call WriteUpdateSQL(iCmd)

    End Sub

    Sub Save_CalendarPlan()

        ss_Saving.Visible = True
        StatusStrip1.Refresh()

        Dim uCmd As New SqlCommand 'UPDATE Query
        'userID, wkRange, wkDate, wkDailyNote, wkTaskIDs
        'wkRange example: '8/28/2017 - 9/3/2017'
        'Create wkRange
        Dim Firstday As DateTime = CDate(cctrl_Cal.DateRef).AddDays(-CInt(CDate(cctrl_Cal.DateRef).DayOfWeek) + 1)
        Dim wkRange As String = Firstday & " - " & Firstday.AddDays(6)

        uCmd.CommandText = "IF EXISTS (SELECT 1 FROM tbl_ToDoDaily WHERE userID=@Usr AND wkRange=@Wk AND wkDate=@Dt) " &
            "BEGIN   UPDATE tbl_ToDoDaily " &
            "   SET wkDailyNote=@Nt    " &
            "   WHERE userID=@Usr AND wkRange=@Wk AND wkDate=@Dt       END " &
            "ELSE BEGIN INSERT INTO tbl_ToDoDaily (userID, wkRange, wkDate, wkDailyNote) " &
            "  VALUES (@Usr, @Wk, @Dt, @Nt)    END"

        uCmd.Parameters.AddWithValue("@Dt", CDate(cctrl_Cal.DateRef))
        uCmd.Parameters.AddWithValue("@Usr", ss_User.Text)
        uCmd.Parameters.AddWithValue("@Wk", wkRange)
        uCmd.Parameters.AddWithValue("@Nt", rtxt_WklyPlan.Rtf)
        Call WriteUpdateSQL(uCmd)
        uCmd.Parameters.Clear()


        uCmd.Dispose()

        Call Get_WeeklyPlanData()

        ss_Saving.Visible = False
        StatusStrip1.Refresh()

    End Sub

    Sub Save_Weekly()

        ss_Saving.Visible = True
        StatusStrip1.Refresh()

        Dim NewString As String() = tvw_Weekly.SelectedNode.FullPath.ToString.Split(New String() {"\"}, StringSplitOptions.None)
        Dim uCmd As New SqlCommand 'UPDATE Query

        'MsgBox(tvw_Weekly.SelectedNode.Level)
        
        'Exit Sub
        'ss_System.Text = NewString(1)
        'ss_Parent.Text = tvw_Items.SelectedNode.Parent.Text
        'ss_Item.Text = tvw_Items.SelectedNode.Text

        Select Case tvw_Weekly.SelectedNode.Level
            Case 1 'Weekly
                uCmd.CommandText = "SET TRANSACTION ISOLATION LEVEL SERIALIZABLE BEGIN TRANSACTION " & _
                            "IF EXISTS (SELECT 1 FROM tbl_ToDoWeekly WHERE userID=@Usr AND wkRange=@Wk) " & _
                            "BEGIN   UPDATE tbl_ToDoWeekly " & _
                            "SET wkNotes=@Nt " & _
                            "WHERE userID=@Usr AND wkRange=@Wk       END " & _
                            "COMMIT TRANSACTION "
            Case 2 'Daily
                uCmd.CommandText = "SET TRANSACTION ISOLATION LEVEL SERIALIZABLE BEGIN TRANSACTION " & _
                            "IF EXISTS (SELECT 1 FROM tbl_ToDoDaily WHERE userID=@Usr AND wkRange=@Wk AND wkDate=@Dt) " & _
                            "BEGIN   UPDATE tbl_ToDoDaily " & _
                            "SET wkDailyNote=@Nt    " & _
                            "WHERE userID=@Usr AND wkRange=@Wk AND wkDate=@Dt       END " & _
                            "COMMIT TRANSACTION "
                uCmd.Parameters.AddWithValue("@Dt", CDate(tvw_Weekly.SelectedNode.Text))
            Case Else
                MsgBox("Did not save anything.  Select a Date or Week before adding Notes.")
                Exit Sub
        End Select

        uCmd.Parameters.AddWithValue("@Usr", ss_User.Text)
        uCmd.Parameters.AddWithValue("@Wk", NewString(1))
        uCmd.Parameters.AddWithValue("@Nt", If(String.IsNullOrEmpty(rtxt_WklyNotes.Text), DBNull.Value, rtxt_WklyNotes.Rtf))
        Call WriteUpdateSQL(uCmd)
        uCmd.Parameters.Clear()

        'Update the Image Notes
        If Not String.IsNullOrEmpty(ctrImg_Weekly.label_Image.Text) Then
            uCmd.CommandText = "UPDATE tbl_ToDoImages " & _
                            "SET ImageNotes = @Nt " & _
                            "WHERE System=@Sys AND ImageID=@Idx "
            ' Replace 8000, below, with the correct size of the field
            uCmd.Parameters.AddWithValue("@Sys", ctrImg_Weekly.Sys)
            uCmd.Parameters.AddWithValue("@Idx", CInt(ctrImg_Weekly.label_Image.Text))
            uCmd.Parameters.AddWithValue("@Nt", ctrImg_Weekly.rtxt_ImageNotes.Rtf)
            Call WriteUpdateSQL(uCmd)
        End If

        uCmd.Dispose()

        Call Get_WeeklyPlanData()

        ss_Saving.Visible = False
        StatusStrip1.Refresh()

    End Sub


    Private Sub tsbtn_FontColor_Click(sender As Object, e As EventArgs) Handles tsbtn_FontColor.Click
        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub

        If Me.ColorDialog1.ShowDialog <> Windows.Forms.DialogResult.Cancel Then

            rtBox.SelectionColor = Me.ColorDialog1.Color

        End If
    End Sub

    Private Sub tsbtn_FontSizeIncrease_Click(sender As Object, e As EventArgs) Handles tsbtn_FontSizeIncrease.Click
        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub
        If rtBox.SelectionFont IsNot Nothing Then
            Dim currentFont As System.Drawing.Font = rtBox.SelectionFont
            rtBox.SelectionFont = New Font(currentFont.FontFamily, currentFont.Size + 1, currentFont.Style)
        End If
    End Sub

    Private Sub tsbtn_FontSizeDecrease_Click(sender As Object, e As EventArgs) Handles tsbtn_FontSizeDecrease.Click
        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub
        If rtBox.SelectionFont IsNot Nothing Then
            Dim currentFont As System.Drawing.Font = rtBox.SelectionFont
            If currentFont.Size > 3 Then rtBox.SelectionFont = New Font(currentFont.FontFamily, currentFont.Size - 1, currentFont.Style)
        End If
    End Sub

    Private Sub tsbtn_Bold_Click(sender As Object, e As EventArgs) Handles tsbtn_Bold.Click

        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub

        If rtBox.SelectionFont IsNot Nothing Then
            Dim currentFont As System.Drawing.Font = rtBox.SelectionFont

            If rtBox.SelectionFont.Bold = False Then
                rtBox.SelectionFont = New Font(currentFont.FontFamily, currentFont.Size, currentFont.Style Or FontStyle.Bold)
            Else
                rtBox.SelectionFont = New Font(currentFont.FontFamily, currentFont.Size, currentFont.Style And Not FontStyle.Bold)
            End If

        End If
    End Sub

    Private Sub tsbtn_Italic_Click(sender As Object, e As EventArgs) Handles tsbtn_Italic.Click
        
        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub

        If rtBox.SelectionFont IsNot Nothing Then
            Dim currentFont As System.Drawing.Font = rtBox.SelectionFont

            If rtBox.SelectionFont.Italic = False Then
                rtBox.SelectionFont = New Font(currentFont.FontFamily, currentFont.Size, currentFont.Style Or FontStyle.Italic)
            Else
                rtBox.SelectionFont = New Font(currentFont.FontFamily, currentFont.Size, currentFont.Style And Not FontStyle.Italic)
            End If

        End If
    End Sub

    Private Sub tsbtn_Underline_Click(sender As Object, e As EventArgs) Handles tsbtn_Underline.Click

        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub

        If rtBox.SelectionFont IsNot Nothing Then
            Dim currentFont As System.Drawing.Font = rtBox.SelectionFont

            If rtBox.SelectionFont.Underline = False Then
                rtBox.SelectionFont = New Font(currentFont.FontFamily, currentFont.Size, currentFont.Style Or FontStyle.Underline)
            Else
                rtBox.SelectionFont = New Font(currentFont.FontFamily, currentFont.Size, currentFont.Style And Not FontStyle.Underline)
            End If

        End If
    End Sub

    Private Sub tsbtn_Fill_Click(sender As Object, e As EventArgs) Handles tsbtn_Fill.Click
        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub

        If Me.ColorDialog1.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            rtBox.SelectionBackColor = Me.ColorDialog1.Color
        End If
    End Sub

    Private Sub tsbtn_FontDlg_Click(sender As Object, e As EventArgs) Handles tsbtn_FontDlg.Click
        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub

        FontDialog1.ShowColor = True
        FontDialog1.ShowEffects = True

        If FontDialog1.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            rtBox.SelectionColor = FontDialog1.Color
            rtBox.SelectionFont = FontDialog1.Font
        End If
    End Sub

    Private Sub tsbtn_AddNew_Click(sender As Object, e As EventArgs) Handles tsbtn_AddNew.Click

        frm_NewItem.ShowDialog()

    End Sub

    Private Sub tsbtn_Bullets_Click(sender As Object, e As EventArgs) Handles tsbtn_Bullets.Click
        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub

        If rtBox.SelectionBullet = False Then
            rtBox.SelectionBullet = True
            rtBox.BulletIndent = 12
        Else
            rtBox.SelectionBullet = False
            rtBox.BulletIndent = 0
        End If

    End Sub

    Private Sub tsbtn_Indent_Click(sender As Object, e As EventArgs) Handles tsbtn_Indent.Click
        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub
        rtBox.SelectionIndent = rtBox.SelectionIndent + 8

    End Sub

    Private Sub tsbtn_Outdent_Click(sender As Object, e As EventArgs) Handles tsbtn_Outdent.Click
        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub
        rtBox.SelectionIndent = rtBox.SelectionIndent - 8
    End Sub

    Private Sub tsbtn_Numbered_Click(sender As Object, e As EventArgs) Handles tsbtn_Numbered.Click


    End Sub

    Function DetermineRichTextBox(xCtrl As Control) As RichTextBox

        If TypeOf xCtrl Is ContainerControl Then
            xCtrl = DirectCast(xCtrl, ContainerControl).ActiveControl
        End If
        'If xCtrl.Name = "SplitContainer1" Then
        '    xCtrl = SplitContainer1.ActiveControl
        'End If
        DetermineRichTextBox = Nothing
        Select Case xCtrl.GetType.ToString
            Case "Project_ToDo.ctrl_Images"
                Dim ctrl As ctrl_Images = DirectCast(Me.ActiveControl, ctrl_Images)
                'MsgBox("Do Control Stuff  " & ctrl.ActiveControl.Name & "  " & ctrl.ActiveControl.GetType.ToString)
                If Not ctrl.ActiveControl.GetType.ToString = "System.Windows.Forms.RichTextBox" Then Exit Function
                DetermineRichTextBox = CType(ctrl.ActiveControl.Parent.Controls(ctrl.ActiveControl.Name), RichTextBox)
            Case Else
                'MsgBox(Me.ActiveControl.Name & "  " & Me.ActiveControl.GetType.ToString)
                If Not xCtrl.GetType.ToString = "System.Windows.Forms.RichTextBox" Then Exit Function
                DetermineRichTextBox = CType(xCtrl, RichTextBox)
        End Select

        Return DetermineRichTextBox

    End Function

    Private Sub btn_SaveItemName_Click(sender As Object, e As EventArgs)

        Dim NewString As String() = tvw_Items.SelectedNode.FullPath.Split(New String() {" || "}, StringSplitOptions.None)

        Dim uCmd As New SqlCommand 'UPDATE Query
        uCmd.CommandText = "UPDATE tbl_ToDoItems " & _
                            "SET Item='" & txt_Item.Text & "' " & _
                            "WHERE System=@Sys AND Item=@Itm AND [Parent]=@Par "

        uCmd.Parameters.AddWithValue("@Sys", NewString(1))
        uCmd.Parameters.AddWithValue("@Itm", NewString(tvw_Items.SelectedNode.Level))
        uCmd.Parameters.AddWithValue("@Par", NewString(tvw_Items.SelectedNode.Level - 1))
        Call WriteUpdateSQL(uCmd)
        uCmd.Parameters.Clear()

        Call Load_Systems()

    End Sub

    Private Sub btn_PriorityUp_Click(sender As Object, e As EventArgs) Handles btn_PriorityUp.Click
        Reordering = True

        Dim exTag As mTag = CType(tvw_Items.SelectedNode.Tag, mTag)

        'Dim NewString As String() = tvw_Items.SelectedNode.FullPath.Split(New String() {" || "}, StringSplitOptions.None)
        Dim SystemName As String = exTag.Get("Sys")
        'MsgBox("Lev " & tvw_Items.SelectedNode.Level & ",  Ind " & tvw_Items.SelectedNode.Index)

        Dim parent As TreeNode = tvw_Items.SelectedNode.Parent
        Dim currNode As TreeNode = tvw_Items.SelectedNode
        If parent IsNot Nothing Then

            Dim index As Integer = parent.Nodes.IndexOf(currNode)
            If index > 0 Then
                parent.Nodes.RemoveAt(index)
                parent.Nodes.Insert(index - 1, currNode)

                ' bw : & " AND Path IS NOT NULL"add this line to restore the originally selected node as selected
                tvw_Items.SelectedNode.TreeView.SelectedNode = currNode
                label_Index.Text = index - 1
            End If

        Else 'Level 0 item
            Dim index As Integer = tvw_Items.SelectedNode.Index
            If index > 0 Then
                tvw_Items.Nodes.RemoveAt(index)
                tvw_Items.Nodes.Insert(index - 1, currNode)

                ' bw : & " AND Path IS NOT NULL"add this line to restore the originally selected node as selected
                tvw_Items.SelectedNode.TreeView.SelectedNode = currNode
                label_Index.Text = index - 1
            End If
        End If

        'Write Indexes back to SQL Server
        Dim uCmd As New SqlCommand
        Dim Sql As String = ""
        If tvw_Items.SelectedNode.Level = 0 Or exTag.Get("Type") = "System" Then
            'Case "System"  'Base level (just above Category Level)
            Sql = "UPDATE tbl_ToDoSystems " &
            "SET iIndex=@indx WHERE System=@Sys AND Architect='" & ss_User.Text & "' "
            For Each cNode As TreeNode In tvw_Items.Nodes
                If cNode.Level = tvw_Items.SelectedNode.Level Then
                    uCmd.CommandText = Sql
                    uCmd.Parameters.AddWithValue("@indx", cNode.Index)
                    uCmd.Parameters.AddWithValue("@Sys", cNode.Text)
                    uCmd.Parameters.AddWithValue("@Par", exTag.Get("Par"))
                    uCmd.Parameters.AddWithValue("@Itm", cNode.Text)
                    Call WriteUpdateSQL(uCmd)
                    uCmd.Parameters.Clear()
                End If
            Next
        Else 'Items
            Sql = "UPDATE tbl_ToDoItems " &
                "SET iIndex=@indx WHERE System=@Sys AND [Parent]=@Par AND Item=@Itm "
            For Each cNode As TreeNode In tvw_Items.SelectedNode.Parent.Nodes
                uCmd.CommandText = Sql
                uCmd.Parameters.AddWithValue("@indx", cNode.Index)
                uCmd.Parameters.AddWithValue("@Sys", If(cNode.Parent.Level = 0, cNode.Text, cNode.Parent.Text))
                uCmd.Parameters.AddWithValue("@Par", tvw_Items.SelectedNode.Parent.FullPath)
                uCmd.Parameters.AddWithValue("@Itm", cNode.Text)
                Call WriteUpdateSQL(uCmd)
                uCmd.Parameters.Clear()
            Next
        End If


        Call Load_Items()

        Reordering = False
    End Sub

    Private Sub btn_PriorityDown_Click(sender As Object, e As EventArgs) Handles btn_PriorityDown.Click
        Reordering = True

        Dim exTag As mTag = CType(tvw_Items.SelectedNode.Tag, mTag)

        'Dim NewString As String() = tvw_Items.SelectedNode.FullPath.Split(New String() {" || "}, StringSplitOptions.None)
        Dim SystemName As String = exTag.Get("Sys")

        Dim parent As TreeNode = tvw_Items.SelectedNode.Parent
        Dim currNode As TreeNode = tvw_Items.SelectedNode
        If parent IsNot Nothing Then

            Dim index As Integer = parent.Nodes.IndexOf(currNode)
            If index < parent.Nodes.Count - 1 Then
                parent.Nodes.RemoveAt(index)
                parent.Nodes.Insert(index + 1, currNode)

                ' bw : add this line to restore the originally selected node as selected
                tvw_Items.SelectedNode.TreeView.SelectedNode = currNode
                label_Index.Text = index + 1
            End If
        Else 'Level 0 item
            Dim index As Integer = tvw_Items.SelectedNode.Index
            If index < tvw_Items.Nodes.Count - 1 Then
                tvw_Items.Nodes.RemoveAt(index)
                tvw_Items.Nodes.Insert(index + 1, currNode)

                ' bw : & " AND Path IS NOT NULL"add this line to restore the originally selected node as selected
                tvw_Items.SelectedNode.TreeView.SelectedNode = currNode
                label_Index.Text = index + 1
            End If
        End If

        'Write Indexes back to SQL Server
        Dim uCmd As New SqlCommand
        Dim Sql As String = ""
        If tvw_Items.SelectedNode.Level = 0 Or exTag.Get("Type") = "System" Then
            'Case 0  'System
            Sql = "UPDATE tbl_ToDoSystems " &
                "SET iIndex=@indx WHERE System=@Sys AND Architect='" & ss_User.Text & "' "
            For Each cNode As TreeNode In tvw_Items.Nodes
                If cNode.Level = tvw_Items.SelectedNode.Level Then
                    uCmd.CommandText = Sql
                    uCmd.Parameters.AddWithValue("@indx", cNode.Index)
                    uCmd.Parameters.AddWithValue("@Sys", cNode.Text)
                    uCmd.Parameters.AddWithValue("@Par", exTag.Get("Par"))
                    uCmd.Parameters.AddWithValue("@Itm", cNode.Text)
                    Call WriteUpdateSQL(uCmd)
                    uCmd.Parameters.Clear()
                End If
            Next
            'Case Else 'Items
        Else
            Sql = "UPDATE tbl_ToDoItems " &
                "SET iIndex=@indx WHERE System=@Sys AND [Parent]=@Par AND Item=@Itm "
            For Each cNode As TreeNode In tvw_Items.SelectedNode.Parent.Nodes
                uCmd.CommandText = Sql
                uCmd.Parameters.AddWithValue("@indx", cNode.Index)
                uCmd.Parameters.AddWithValue("@Sys", If(cNode.Parent.Level = 0, cNode.Text, cNode.Parent.Text))
                uCmd.Parameters.AddWithValue("@Par", tvw_Items.SelectedNode.Parent.FullPath)
                uCmd.Parameters.AddWithValue("@Itm", cNode.Text)
                Call WriteUpdateSQL(uCmd)
                uCmd.Parameters.Clear()
            Next
            'End Select
        End If

        'Dim uCmd As New SqlCommand
        'Dim pNode As TreeNode
        'If tvw_Items.SelectedNode.Parent.Level = 1 Then pNode = tvw_Items.SelectedNode.Parent Else pNode = currNode
        'For Each cNode As TreeNode In tvw_Items.SelectedNode.Parent.Nodes
        '    uCmd.CommandText = "UPDATE tbl_ToDoItems " & _
        '        "SET iIndex = " & cNode.Index & " " & _
        '        "WHERE [Parent]='" & pNode.Text & "' AND Item='" & cNode.Text & "'"
        '    Call WriteUpdateSQL(uCmd)
        '    'Dim dRow As DataRow = dsSys.Tables("dtItems").Select().CopyToDataTable.Rows(0)
        '    'dRow.Item("iIndex") = cNode.Index
        'Next
        Call Load_Items()

        Reordering = False
    End Sub

    Private Sub tsbtn_Refresh_Click(sender As Object, e As EventArgs) Handles tsbtn_Refresh.Click

        Call Refresh_Data()

    End Sub

    Private Sub combo_Phase_SelectedIndexChanged(sender As Object, e As EventArgs) Handles combo_Phase.SelectedIndexChanged
    End Sub

    Private Sub combo_Phase_SelectedValueChanged(sender As Object, e As EventArgs) Handles combo_Phase.SelectedValueChanged
        If Loading = True Then Exit Sub
        If Refreshing = True Then Exit Sub

        If combo_Phase.Text = "Complete" Then txt_PercentComplete.Text = "100%"
        If combo_Phase.Text = "Complete" Then cbo_Status.Text = "Complete"
        If combo_Phase.Text = "Complete" AndAlso String.IsNullOrWhiteSpace(txt_PublishDate.Text) Then txt_PublishDate.Text = Today().ToShortDateString
        If combo_Phase.Text = "Complete" AndAlso String.IsNullOrWhiteSpace(txt_ActualEnd.Text) Then txt_ActualEnd.Text = Today().ToShortDateString


        If String.IsNullOrEmpty(txt_PublishDate.Text) AndAlso combo_Phase.Text = "Complete" Then

            'Dim uCmd As New SqlCommand 'UPDATE Query
            'uCmd.CommandText = "UPDATE tbl_ToDoItems " &
            '                    "SET Phase=@Phs, Status=@Phs, PercentComplete=@Prc, PublishDate=@UpDt, ActualEnd=@EDt, OverallPriority=@Pri, LastUpdate=@UpDt " &
            '                    "WHERE System=@Sys AND Item=@Itm AND [Parent]=@Par "
            'uCmd.Parameters.AddWithValue("@Sys", ss_System.Text)
            'uCmd.Parameters.AddWithValue("@Itm", ss_Item.Text)
            'uCmd.Parameters.AddWithValue("@Par", ss_Parent.Text)
            'uCmd.Parameters.AddWithValue("@Phs", "Complete")
            'uCmd.Parameters.AddWithValue("@Prc", "100%")
            'uCmd.Parameters.AddWithValue("@EDt", If(IsDate(txt_ActualEnd.Text), CDate(txt_ActualEnd.Text).ToShortDateString, DBNull.Value))
            'uCmd.Parameters.AddWithValue("@Pri", DBNull.Value)
            'uCmd.Parameters.AddWithValue("@UpDt", Now())
            'Call WriteUpdateSQL(uCmd)
            'uCmd.Parameters.Clear()

            'Call Load_Items()
            'Refresh_PriorityList()

        End If

        If Not IsNothing(tvw_Items.SelectedNode) Then
            Call AddNodeColor(tvw_Items.SelectedNode, tvw_Items.SelectedNode.FullPath) 'combo_Phase.Text)
        End If

    End Sub


    Public Shared Function GetAllNodes(objTree As TreeView) As List(Of TreeNode)
        Dim nodes As List(Of TreeNode) = New List(Of TreeNode)
        For Each parentNode As TreeNode In objTree.Nodes
            nodes.Add(parentNode)
            GetAllChildren(parentNode, nodes)
        Next

        Return nodes
    End Function

    Public Shared Sub GetAllChildren(parentNode As TreeNode, ByRef nodes As List(Of TreeNode))
        For Each childNode As TreeNode In parentNode.Nodes
            nodes.Add(childNode)
            GetAllChildren(childNode, nodes)
        Next
    End Sub




    Private Sub ShowVisibleNodes()
        Dim Node As TreeNode

        For Each ExpandedChildNodeName As String In _ExpandedNodeList
            For Each RootNode As TreeNode In tvw_Items.Nodes
                Node = NodeFromTagName(ExpandedChildNodeName, RootNode)
                If Node IsNot Nothing Then
                    Node.Expand()
                End If
            Next
        Next

    End Sub

    Private Function NodeFromTagName(ByVal NodeTagName As String, NodeToSearch As TreeNode) As TreeNode
        Dim NextNode As TreeNode

        'check input node
        If NodeToSearch.Tag IsNot Nothing Then
            If NodeToSearch.Tag.Equals(NodeTagName) Then
                Return NodeToSearch
            End If
        End If

        'check child nodes and recurse on intermediary nodes
        For Each Node As TreeNode In NodeToSearch.Nodes
            If Node.Tag IsNot Nothing Then
                If Node.Tag.Equals(NodeTagName) Then
                    Return Node
                End If
                NextNode = NodeFromTagName(NodeTagName, Node)
                If NextNode IsNot Nothing Then
                    Return NextNode
                End If
            End If
        Next

        Return Nothing

    End Function

    Private Sub tvw_Items_BeforeSelect(sender As Object, e As TreeViewCancelEventArgs) Handles tvw_Items.BeforeSelect
        If IsNothing(tvw_Items.SelectedNode) Then Exit Sub
        If Not IsNothing(tvw_Items.SelectedNode) Then tvw_Items.SelectedNode.BackColor = Color.Transparent
        tvw_Items.SelectedNode.BackColor = tvw_Items.BackColor
        Call AddNodeColor(tvw_Items.SelectedNode, tvw_Items.SelectedNode.FullPath)
        'tvw_Items.SelectedNode.ForeColor = Color.Black
    End Sub


    Private Sub tvw_Items_DragDrop(sender As Object, e As DragEventArgs) Handles tvw_Items.DragDrop

        ' Retrieve the client coordinates of the drop location.
        Dim targetPoint As Point = tvw_Items.PointToClient(New Point(e.X, e.Y))

        ' Retrieve the node at the drop location.
        Dim pt As Point
        'Dim DestinationNode As TreeNode
        pt = CType(sender, TreeView).PointToClient(New Point(e.X, e.Y))
        'DestinationNode = CType(sender, TreeView).GetNodeAt(pt)
        Dim targetNode As TreeNode = CType(sender, TreeView).GetNodeAt(pt) 'tvw_Items.GetNodeAt(targetPoint)

        ' Retrieve the node that was dragged.

        Dim draggedNode As TreeNode = CType(e.Data.GetData(GetType(TreeNode)), TreeNode)
        Dim tagDrag As mTag = CType(draggedNode.Tag, mTag)
        Dim tagTgt As mTag = CType(targetNode.Tag, mTag)
        Dim NewString As String() = draggedNode.FullPath.Split(New String() {" || "}, StringSplitOptions.None)
        Dim OrigSys As String = tagDrag.Get("Sys") 'NewString(1)
        Dim OrigPar As String = tagDrag.Get("Par") 'draggedNode.Parent.Text

        'Dim OrigPth As String = 

        ' Confirm that the node at the drop location is not 
        ' the dragged node or a descendant of the dragged node.
        If Not draggedNode.Equals(targetNode) AndAlso Not ContainsNode(draggedNode, targetNode) Then

            ' If it is a move operation, remove the node from its current 
            ' location and add it to the node at the drop location.
            If e.Effect = DragDropEffects.Move Then
                draggedNode.Remove()
                targetNode.Nodes.Add(draggedNode)

                ' If it is a copy operation, clone the dragged node 
                ' and add it to the node at the drop location.
            ElseIf e.Effect = DragDropEffects.Copy Then
                targetNode.Nodes.Add(CType(draggedNode.Clone(), TreeNode))
                label_Index.Text = targetNode.Nodes.Count
            End If

            'Update Sql Server to put Item in new System/Parent
            'MsgBox("Tgt: " & targetNode.Text & ", Prt: " & targetNode.Parent.Text & ", Node: " & draggedNode.Text)
            MsgBox("From:" & vbCrLf & "   System: " & tagDrag.Get("Sys") & vbCrLf &
                   "   Parent: " & tagDrag.Get("Par") & vbCrLf &
                   "   Path: " & tagDrag.Get("Path") & vbCrLf &
                   "To:" & vbCrLf & "   System: " & If(draggedNode.Text = tagDrag.Get("Sys"), draggedNode.Text, SystemNameFromPath(tagTgt.Get("Path"))) & vbCrLf &
                   "   Parent: " & tagTgt.Get("Path") & vbCrLf &
                   "   Path: " & tagTgt.Get("Path") & " || " & draggedNode.Text)
            'Exit Sub

            Dim uCmd As New SqlCommand 'UPDATE Query
            uCmd.CommandText = "UPDATE tbl_ToDoItems " &
                                "SET System=@Sys, [Parent]=@Par, Path=@Pth, iIndex=@Ind, Expanded=@Exp " &
                                "WHERE System=@origSys AND Item=@Itm AND [Parent]=@origPar AND Path=@origPth"

            uCmd.Parameters.AddWithValue("@origSys", tagDrag.Get("Sys"))
            uCmd.Parameters.AddWithValue("@origPar", tagDrag.Get("Par"))
            uCmd.Parameters.AddWithValue("@origPth", tagDrag.Get("Path"))
            uCmd.Parameters.AddWithValue("@Itm", draggedNode.Text)
            'if the Original System name = New Item Name (meaning your moving an entire system) then keep the original System Name
            uCmd.Parameters.AddWithValue("@Sys", If(draggedNode.Text = tagDrag.Get("Sys"), draggedNode.Text, SystemNameFromPath(tagTgt.Get("Path"))))
            'uCmd.Parameters.AddWithValue("@Sys", SystemNameFromPath(tagTgt.Get("Path") & " || " & draggedNode.Text))
            uCmd.Parameters.AddWithValue("@Par", If(targetNode.Text = targetNode.FullPath, targetNode.Text, tagTgt.Get("Path"))) 'if Parent is 'System' then change Parent to be the same as the system name
            uCmd.Parameters.AddWithValue("@Pth", tagTgt.Get("Path") & " || " & draggedNode.Text)
            uCmd.Parameters.AddWithValue("@Ind", targetNode.Nodes.Count)
            uCmd.Parameters.AddWithValue("@Exp", If(tvw_Items.SelectedNode.IsExpanded, True, False))
            'MsgBox(getQueryFromCommand(uCmd))
            Call WriteUpdateSQL(uCmd)
            uCmd.Parameters.Clear()

            'Update the Child Items where this node is the Parent
            Dim strReplacePth As String = "REPLACE(Path, '" & tagDrag.Get("Path") & "', '" &
                tagTgt.Get("Path") & " || " & draggedNode.Text & "') "
            Dim strReplacePar As String = "REPLACE([Parent], '" & tagDrag.Get("Path") & "', '" &
                tagTgt.Get("Path") & " || " & draggedNode.Text & "') "
            uCmd.CommandText = "UPDATE tbl_ToDoItems " &
                                "SET System=@Sys, Path = " & strReplacePth & ", " &
                                "    [Parent]= " & strReplacePar &
                                "WHERE System=@origSys AND Path LIKE @origPth" 'AND [Parent] LIKE @origPar
            uCmd.Parameters.AddWithValue("@origSys", tagDrag.Get("Sys"))
            uCmd.Parameters.AddWithValue("@origPar", tagDrag.Get("Par") & "%")
            uCmd.Parameters.AddWithValue("@origPth", tagDrag.Get("Path") & "%")
            'if the Original System name = New Item Name (meaning your moving an entire system) then keep the original System Name
            uCmd.Parameters.AddWithValue("@Sys", If(draggedNode.Text = tagDrag.Get("Sys"), draggedNode.Text, SystemNameFromPath(tagTgt.Get("Path"))))
            uCmd.Parameters.AddWithValue("@Par", tagTgt.Get("Path"))
            uCmd.Parameters.AddWithValue("@Pth", tagTgt.Get("Path") & " || " & draggedNode.Text)
            Dim strCmd As String = getQueryFromCommand(uCmd)
            MsgBox(getQueryFromCommand(uCmd))
            Call WriteUpdateSQL(uCmd)

            Call Load_Items()

            ' Expand the node at the location 
            ' to show the dropped node.
            targetNode.Expand()
        End If
    End Sub

    Private Sub tvw_Items_DragEnter(sender As Object, e As DragEventArgs) Handles tvw_Items.DragEnter
        e.Effect = DragDropEffects.Move
        'If ss_Control.Text = "--" Then ss_Control.Text = 1
        'ss_Control.Text = CInt(ss_Control.Text) + 1
        'e.Effect = DragDropEffects.Copy
    End Sub

    Dim lastDragDestination As New TreeNode
    Dim lastDragDestinationTime As DateTime

    Private Sub tvw_Items_DragOver(sender As Object, e As DragEventArgs) Handles tvw_Items.DragOver

        'If ss_Control.Text = "--" Then ss_Control.Text = 1
        'ss_Control.Text = CInt(ss_Control.Text) + 1

        'Dim dragDropObject As Icon = Nothing
        'Dim dragDropNode As New TreeNode

        'e.Effect = DragDropEffects.None

        'If e.Data.GetDataPresent().type Is TreeNode Then

        'End If
        ''Dim pt As New Point(e.X, e.Y)
        ''Dim pnt As Point = DirectCast(sender, TreeView).PointToClient(pt)
        ''mDropp = DirectCast(sender, TreeView).GetNodeAt(pnt)
        ''If mDropp IsNot Nothing Then tvw_Items.SelectedNode = mDropp

        ''Try 1 & 2
        '' Retrieve the client coordinates of the mouse position.
        'Dim targetPoint As Point = tvw_Items.PointToClient(New Point(e.X, e.Y))

        '' Select the node at the mouse position.
        'tvw_Items.SelectedNode = tvw_Items.GetNodeAt(targetPoint)

    End Sub

    ' Create a Font object for the node tags.
    Private tagFont As New Font("Helvetica", 8, FontStyle.Bold)

    Private Sub tvw_Items_DrawNode(sender As Object, e As DrawTreeNodeEventArgs) Handles tvw_Items.DrawNode

        If (e.State And TreeNodeStates.Selected) <> 0 Then

            ' Draw the background of the selected node. The NodeBounds
            ' method makes the highlight rectangle large enough to
            ' include the text of a node tag, if one is present.
            e.Graphics.FillRectangle(Brushes.Green, NodeBounds(e.Node))

            ' Retrieve the node font. If the node font has not been set,
            ' use the TreeView font.
            Dim nodeFont As Font = e.Node.NodeFont
            If nodeFont Is Nothing Then
                nodeFont = CType(sender, TreeView).Font
            End If

            ' Draw the node text.
            e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.White,
                e.Bounds.Left - 2, e.Bounds.Top)

            ' Use the default background and node text.
        Else
            e.DrawDefault = True
        End If

        ' If a node tag is present, draw its string representation 
        ' to the right of the label text.
        If (e.Node.Tag IsNot Nothing) Then
            e.Graphics.DrawString(e.Node.Tag.ToString(), tagFont,
                Brushes.Yellow, e.Bounds.Right + 20, e.Bounds.Top)
        End If

        ' If the node has focus, draw the focus rectangle large, making
        ' it large enough to include the text of the node tag, if present.
        If (e.State And TreeNodeStates.Focused) <> 0 Then
            Dim focusPen As New Pen(Color.Black)
            Try
                focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
                Dim focusBounds As Rectangle = NodeBounds(e.Node)
                focusBounds.Size = New Size(focusBounds.Width - 1, _
                    focusBounds.Height - 3)
                e.Graphics.DrawRectangle(focusPen, focusBounds)
            Finally
                focusPen.Dispose()
            End Try
        End If

    End Sub

    Private Function NodeBounds(ByVal node As TreeNode) As Rectangle

        ' Set the return value to the normal node bounds.
        Dim bounds As Rectangle = node.Bounds
        If (node.Tag IsNot Nothing) Then

            ' Retrieve a Graphics object from the TreeView handle
            ' and use it to calculate the display width of the tag.
            Dim g As Graphics = tvw_Items.CreateGraphics()
            Dim tagWidth As Integer = CInt(g.MeasureString(
                node.Tag.ToString(), tagFont).Width) + 80

            ' Adjust the node bounds using the calculated value.
            bounds.Offset(tagWidth \ 2, 0)
            bounds = Rectangle.Inflate(bounds, tagWidth \ 2, 0)
            g.Dispose()
        End If
        Return bounds
    End Function 'NodeBounds


    Private Sub tvw_Items_ItemDrag(sender As Object, e As ItemDragEventArgs) Handles tvw_Items.ItemDrag

        'mDragg = DirectCast(e.Item, TreeNode)
        'If CBool(mDragg.Tag) Then
        '    tvw_Items.AllowDrop = True
        '    tvw_Items.DoDragDrop("", DragDropEffects.All)
        'End If

        'Try 1 & 2
        DoDragDrop(e.Item, DragDropEffects.Move)

    End Sub


    Private Function ContainsNode(ByVal node1 As TreeNode, ByVal node2 As TreeNode) As Boolean
        ' Determine whether one node is a parent 
        ' or ancestor of a second node.

        ' Check the parent node of the second node.
        If node2.Parent Is Nothing Then
            Return False
        End If
        If node2.Parent.Equals(node1) Then
            Return True
        End If

        ' If the parent node is not null or equal to the first node, 
        ' call the ContainsNode method recursively using the parent of 
        ' the second node.
        Return ContainsNode(node1, node2.Parent)
    End Function 'ContainsNode

    Private Sub tsbtn_StrikeThru_Click(sender As Object, e As EventArgs) Handles tsbtn_StrikeThru.Click

        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub

        If rtBox.SelectionFont IsNot Nothing Then
            Dim currentFont As System.Drawing.Font = rtBox.SelectionFont

            If rtBox.SelectionFont.Strikeout = False Then
                rtBox.SelectionFont = New Font(currentFont.FontFamily, currentFont.Size, currentFont.Style Or FontStyle.Strikeout)
            Else
                rtBox.SelectionFont = New Font(currentFont.FontFamily, currentFont.Size, currentFont.Style And Not FontStyle.Strikeout)
            End If

        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Dim selectionIndex = rTxt_Notes.SelectionStart

        Dim xSymbol As String = TextBox1.Text
        Dim value As Integer = Integer.Parse(xSymbol, System.Globalization.NumberStyles.HexNumber)
        Dim symbol As String = Char.ConvertFromUtf32(value).ToString()
        TextBox2.Text = symbol
        rTxt_Notes.SelectedText = symbol
        'rTxt_Notes.Rtf = rTxt_Notes.Rtf.Insert(selectionIndex, symbol)
        'rTxt_Notes.SelectionStart = selectionIndex + symbol.Length
    End Sub

    Private Sub tsSBtn_InsertSpecialChar_ButtonClick(sender As Object, e As EventArgs) Handles tsSBtn_InsertSpecialChar.ButtonClick
        Dim xSymbol As String = "2714" 'TextBox1.Text
        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub
        Dim value As Integer = Integer.Parse(xSymbol, System.Globalization.NumberStyles.HexNumber)
        Dim symbol As String = Char.ConvertFromUtf32(value).ToString()
        rtBox.SelectedText = symbol
    End Sub

    Private Sub tsBtn_Check_Click(sender As Object, e As EventArgs) Handles tsBtn_Check.Click
        Dim xSymbol As String = "2714" '2714
        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub
        rtBox.SelectedText = Char.ConvertFromUtf32(Integer.Parse(xSymbol, System.Globalization.NumberStyles.HexNumber)).ToString()
    End Sub

    Private Sub tsBtn_ArrowUp_Click(sender As Object, e As EventArgs) Handles tsBtn_ArrowUp.Click
        '2191
        Dim xSymbol As String = "2191"
        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub
        rtBox.SelectedText = Char.ConvertFromUtf32(Integer.Parse(xSymbol, System.Globalization.NumberStyles.HexNumber)).ToString()
    End Sub

    Private Sub tsBtn_ArrowDown_Click(sender As Object, e As EventArgs) Handles tsBtn_ArrowDown.Click
        '2193
        Dim xSymbol As String = "2193"
        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub
        rtBox.SelectedText = Char.ConvertFromUtf32(Integer.Parse(xSymbol, System.Globalization.NumberStyles.HexNumber)).ToString()
    End Sub

    Private Sub tsBtn_NoGoodX_Click(sender As Object, e As EventArgs) Handles tsBtn_NoGoodX.Click
        Dim xSymbol As String = "2718"
        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub
        rtBox.SelectedText = Char.ConvertFromUtf32(Integer.Parse(xSymbol, System.Globalization.NumberStyles.HexNumber)).ToString()
    End Sub
    Private Sub tsBtn_Asterick_Click(sender As Object, e As EventArgs) Handles tsBtn_Asterick.Click
        Dim xSymbol As String = "2726"
        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub
        rtBox.SelectedText = Char.ConvertFromUtf32(Integer.Parse(xSymbol, System.Globalization.NumberStyles.HexNumber)).ToString()
    End Sub

    Private Sub check_HideComplete_CheckedChanged(sender As Object, e As EventArgs) Handles check_HideComplete.CheckedChanged

    End Sub

    Private Sub check_HideComplete_CheckStateChanged(sender As Object, e As EventArgs) Handles check_HideComplete.CheckStateChanged
        If Loading = True Then Exit Sub
        Call Load_Systems()
    End Sub

    Private Sub tsbtn_Delete_Click(sender As Object, e As EventArgs) Handles tsbtn_Delete.Click
        'Causes the user to have to delete 2 x's to be sure they want to delete the item
        tsbtn_Delete.Visible = False
        tsbtn_Delete2.Visible = True
    End Sub

    Private Sub tsbtn_Delete2_Click(sender As Object, e As EventArgs) Handles tsbtn_Delete2.Click

        If tvw_Items.Nodes.Count < 1 Then Exit Sub
        If tvw_Items.SelectedNode.Index < 0 Then Exit Sub

        'Delete Item
        Dim answ As MsgBoxResult = MsgBoxResult.No
        answ = MsgBox("Do You Really Want To Delete Item '" & tvw_Items.SelectedNode.Text & "'", MsgBoxStyle.YesNoCancel, "DELETE ITEM")
        If answ = MsgBoxResult.Yes Then
            Dim NewString As String() = tvw_Items.SelectedNode.FullPath.Split(New String() {" || "}, StringSplitOptions.None)

            Dim dCmd As New SqlCommand
            dCmd.CommandText = "DELETE tbl_ToDoItems " & _
                "WHERE Item=@Itm AND Parent=@Par AND System=@Sys "
            dCmd.Parameters.AddWithValue("@Itm", tvw_Items.SelectedNode.Text)
            dCmd.Parameters.AddWithValue("@Par", tvw_Items.SelectedNode.Parent.FullPath)
            dCmd.Parameters.AddWithValue("@Sys", NewString(1))
            Call WriteUpdateSQL(dCmd)
            dCmd.Dispose()
        End If

        'Set Delete System back up for next delete
        tsbtn_Delete2.Visible = False
        tsbtn_Delete.Visible = True

        Call Refresh_Data()

    End Sub

    'Private Sub tsBtn_Snippit_Click(sender As Object, e As EventArgs)
    '    Dim bmp = SnippingTool.Snip()

    '    If bmp IsNot Nothing Then
    '        Me.pic_PartImage.Image = bmp 'Clipboard.GetImage
    '        tsBtn_AddImage.Enabled = False
    '        'Me.tsBtn_Save.BackColor = Color.OrangeRed
    '        tsBtn_SaveNewImage.Enabled = True
    '    End If
    'End Sub

    'Private Sub tsBtn_PasteImage_Click(sender As Object, e As EventArgs)

    '    If Clipboard.ContainsImage = True Then
    '        Me.pic_PartImage.Image = Clipboard.GetImage
    '        tsBtn_AddImage.Enabled = False
    '        'frm_Main.tsbtn_Save.BackColor = Color.OrangeRed
    '        tsBtn_SaveNewImage.Enabled = True
    '    Else
    '        MessageBox.Show("No picture")
    '    End If
    'End Sub


    'Private Sub tsBtn_AddImage_Click(sender As Object, e As EventArgs)

    '    pic_PartImage.Image = Nothing
    '    tsBtn_Snippit.Enabled = True
    '    tsBtn_PasteImage.Enabled = True

    'End Sub







    'Private Sub btn_ImageNext_Click(sender As Object, e As EventArgs) Handles btn_ImageNext.Click

    '    If dtCurrImages.Rows.Count > 0 AndAlso CInt(label_ImageNum.Text) < dtCurrImages.Rows.Count Then
    '        label_ImageNum.Text = label_ImageNum.Text + 1
    '        Call Get_Pic(label_ImageNum.Text, )
    '    End If

    'End Sub

    Private Sub tsBtn_SaveNewImage_Click(sender As Object, e As EventArgs)

        ''Get Next Image ID Number
        'Dim imgID As Integer = 1

        'Dim dtMax As New DataTable
        'Dim Sql As String = "SELECT Max(ImageID) FROM tbl_ToDoImages " '& _
        ''"WHERE Architect ='" & ss_User.Text & "' "
        'Call Load_Data(Sql, dtMax)

        'If dtMax.Rows.Count > 0 Then
        '    imgID = CInt(dtMax.Rows(0).Item(0)) + 1
        'End If

        'label_Image.Text = imgID

        ''Save Image
        'Call Save_Image(imgID)

        ''Write Image ID to ToDo's table
        ''Dim NewString As String() = tvw_Items.SelectedNode.FullPath.Split(New String() {" || "}, StringSplitOptions.None)
        'Dim strID As String = imgID.ToString
        'Dim strSrch As String = "System='" & ss_System.Text & "' AND Item='" & ss_Item.Text & "' AND [Parent]='" & ss_Parent.Text & "'"
        'If dsSys.Tables("dtItems").Select(strSrch).Any Then
        '    Dim dr As DataRow = dsSys.Tables("dtItems").Select(strSrch).CopyToDataTable.Rows(0)
        '    If Not String.IsNullOrEmpty(dr.Item("ImageIDs").ToString) Then
        '        strID = dr.Item("ImageIDs").ToString & ", " & imgID.ToString
        '    End If
        'End If

        'Dim uCmd As New SqlCommand
        'uCmd.CommandText = "UPDATE tbl_ToDoItems SET ImageIDs='" & strID & "' " & _
        '    "WHERE System='" & ss_System.Text & "' AND Item='" & ss_Item.Text & "' AND [Parent]='" & ss_Parent.Text & "'"
        'Call WriteUpdateSQL(uCmd)
        'uCmd.Dispose()

        'Call Load_Images()

        'tsBtn_AddImage.Enabled = True
        'tsBtn_SaveNewImage.Enabled = False
        'tsBtn_Snippit.Enabled = False
        'tsBtn_PasteImage.Enabled = False

    End Sub

    'Private Sub btn_ImagePrev_Click(sender As Object, e As EventArgs) Handles btn_ImagePrev.Click

    '    If dtCurrImages.Rows.Count > 0 AndAlso CInt(label_ImageNum.Text) > 1 Then
    '        label_ImageNum.Text = label_ImageNum.Text - 1
    '        Call Get_Pic(label_ImageNum.Text)
    '    End If

    'End Sub

    Private Sub pic_PartImage_Click(sender As Object, e As EventArgs)
        ''When image clicked, Set the focus to the picturebox
        'Dim scrlPnt As New Drawing.Point(panel_Image.AutoScrollPosition.X, panel_Image.AutoScrollPosition.Y)
        'panel_Image.SuspendLayout()
        'pic_PartImage.Select()
        'panel_Image.AutoScrollPosition = New Drawing.Point(-scrlPnt.X, -scrlPnt.Y)
        'panel_Image.ResumeLayout()
    End Sub

    Private Sub pic_PartImage_DoubleClick(sender As Object, e As EventArgs)

        'If Not String.IsNullOrEmpty(label_Image.Text) Then
        '    frm_Image.label_Image.Text = Me.label_Image.Text
        '    frm_Image.ts_System.Text = Me.ss_System.Text
        '    frm_Image.ts_Item.Text = Me.ss_Item.Text
        '    frm_Image.Show()
        'End If

    End Sub

    Private Sub pic_PartImage_MouseDown(sender As Object, e As MouseEventArgs)
        ''Capture the initial point 
        'm_PanStartPoint = New Point(e.X, e.Y)
    End Sub

    Private Sub pic_PartImage_MouseEnter(sender As Object, e As EventArgs)
        'Dim scrlPnt As New Drawing.Point(panel_Image.AutoScrollPosition.X, panel_Image.AutoScrollPosition.Y)
        'panel_Image.SuspendLayout()

        ''pic_PartImage.Select()
        ''Handled w/ Control Key Press now

        'panel_Image.AutoScrollPosition = New Drawing.Point(-scrlPnt.X, -scrlPnt.Y)
        'panel_Image.ResumeLayout()

    End Sub

    Private Sub pic_PartImage_MouseMove(sender As Object, e As MouseEventArgs)
        ''Verify Left Button is pressed while the mouse is moving
        'If e.Button = Windows.Forms.MouseButtons.Left Then

        '    'Here we get the change in coordinates.
        '    Dim DeltaX As Integer = (m_PanStartPoint.X - e.X)
        '    Dim DeltaY As Integer = (m_PanStartPoint.Y - e.Y)

        '    'Then we set the new autoscroll position.
        '    'ALWAYS pass positive integers to the panels autoScrollPosition method
        '    panel_Image.AutoScrollPosition = _
        '    New Drawing.Point((DeltaX - panel_Image.AutoScrollPosition.X), _
        '                    (DeltaY - panel_Image.AutoScrollPosition.Y))
        'End If
    End Sub

    Private Sub pic_PartImage_MouseWheel(sender As Object, e As MouseEventArgs)

        'Dim scrlPnt As New Drawing.Point(panel_Image.AutoScrollPosition.X, panel_Image.AutoScrollPosition.Y)

        ''check to see if mousepointer is over the picturebox
        ''check if control is being held down
        'If My.Computer.Keyboard.CtrlKeyDown Or My.Computer.Keyboard.ShiftKeyDown Then

        '    '1) Give the picturebox focus first

        '    panel_Image.SuspendLayout()
        '    pic_PartImage.Select()
        '    'panel_Image.AutoScrollPosition = New Drawing.Point(-scrlPnt.X, -scrlPnt.Y)
        '    panel_Image.ResumeLayout()

        '    '2) Scroll
        '    If e.Delta < 0 Then 'scroll down
        '        'scrlPnt = New Drawing.Point(panel_Image.AutoScrollPosition.X, panel_Image.AutoScrollPosition.Y)
        '        scrlPnt = New Drawing.Point(panel_Image.AutoScrollPosition.X, panel_Image.AutoScrollPosition.Y + 120)
        '        pic_PartImage.Width = pic_PartImage.Width - 20
        '        pic_PartImage.Height = pic_PartImage.Height - 20
        '        txt_ImageZoom.Text = Math.Round(pic_PartImage.Width / (panel_Image.Width - 20), 2) * 100 & "%"
        '        'panel_Image.AutoScrollPosition = New Drawing.Point(-scrlPnt.X, -scrlPnt.Y)
        '        'panel_Image.Refresh()
        '    ElseIf e.Delta > 0 Then 'scroll up
        '        'scrlPnt = New Drawing.Point(panel_Image.AutoScrollPosition.X, panel_Image.AutoScrollPosition.Y)
        '        scrlPnt = New Drawing.Point(panel_Image.AutoScrollPosition.X, panel_Image.AutoScrollPosition.Y - 120)
        '        pic_PartImage.Width = pic_PartImage.Width + 20
        '        pic_PartImage.Height = pic_PartImage.Height + 20
        '        txt_ImageZoom.Text = Math.Round(pic_PartImage.Width / (panel_Image.Width - 20), 2) * 100 & "%"
        '        'panel_Image.AutoScrollPosition = New Drawing.Point(-scrlPnt.X, -scrlPnt.Y)
        '        'panel_Image.Refresh()
        '    End If

        'End If
        ''End If

        'panel_Image.AutoScrollPosition = New Drawing.Point(-scrlPnt.X, -scrlPnt.Y)

    End Sub

    Private Sub btn_Calendar_Click(sender As Object, e As EventArgs) Handles btn_Calendar.Click
        frm_Calendar.Show()
    End Sub

    Private Sub tab_Main_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tab_Main.SelectedIndexChanged
        If Loading = True Then Exit Sub

        Call Highlight_Tab()

        Select Case tab_Main.SelectedTab.Text
            Case "List"
                Call Refresh_PriorityList()
            Case "Main"
                If tab_DetailsMain.SelectedTab.Text = "Schedule" Then
                    Call Handler_CreateSchedule(pnl_Sch)
                End If
            Case "Mgmt View"
                Call Load_MgmtView()
            Case "Weekly Plan"
                Call Load_WeeklyPlan()
            Case "Setup"
                Load_OffDays()
            Case "Calendar"
                cctrl_Cal.LoadCalendar()
                Load_WeeklyPlanCal()
                GetOutlook(cctrl_Cal.DateRef)
                'ctrl_OpenItmWklyPlan.Paint_OpenItems(cctrl_Cal.DateRef)

        End Select

    End Sub

    Private Sub tab_List_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tab_List.SelectedIndexChanged
        If Loading = True Then Exit Sub

        Select Case tab_List.SelectedTab.Text
            Case "Structure"
                Call tvw_Items_AfterSelect(Nothing, Nothing)
            Case "List by Date"
                dgv_List.DataSource = Nothing
                Dim strSearch As String = "DueDate IS NOT NULL AND NOT Phase ='Complete' "
                If dsSys.Tables("dtItems").Select(strSearch).Any Then
                    'Dim dt As New DataTable
                    'dt = dsSys.Tables("dtItems").Select(strSearch, "DueDate ASC, iIndex ASC").CopyToDataTable
                    Dim displayView = New DataView(dsSys.Tables("dtItems").Select(strSearch, "DueDate ASC, iIndex ASC").CopyToDataTable)
                    Dim subset As DataTable = displayView.ToTable(False, "Item", "DueDate", "Phase", "System", "Parent")
                    dgv_List.DataSource = subset 'dsSys.Tables("dtItems").Select(strSearch, "DueDate ASC, iIndex ASC").CopyToDataTable
                    dgv_List.Columns("Item").Width = 200
                    dgv_List.Columns("DueDate").Width = 68
                End If
            Case "Complete"
                dgv_Complete.DataSource = Nothing
                Dim strSearch As String = "Phase ='Complete' "
                If dsSys.Tables("dtItems").Select(strSearch).Any Then
                    Dim displayView = New DataView(dsSys.Tables("dtItems").Select(strSearch, "DueDate DESC, iIndex ASC").CopyToDataTable)
                    Dim subset As DataTable = displayView.ToTable(False, "Item", "DueDate", "System", "Parent")
                    dgv_Complete.DataSource = subset 'dsSys.Tables("dtItems").Select(strSearch, "DueDate ASC, iIndex ASC").CopyToDataTable
                    dgv_Complete.Columns("Item").Width = 200
                    dgv_Complete.Columns("DueDate").Width = 68
                End If
        End Select

    End Sub

    Private Sub dgv_List_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_List.CellContentClick
    End Sub
    Private Sub dgv_List_SelectionChanged(sender As Object, e As EventArgs) Handles dgv_List.SelectionChanged
        If Loading = True Then Exit Sub

        If IsNothing(dgv_List.DataSource) Then Exit Sub
        If dgv_List.Rows.Count = 0 Then Exit Sub
        If IsNothing(dgv_List.SelectedRows) Then Exit Sub
        If dgv_List.SelectedCells.Count = 0 Then Exit Sub
        If IsDBNull(dgv_List.Rows(dgv_List.SelectedCells(0).RowIndex).Cells("Item").Value) Then Exit Sub

        Dim dRow As DataGridViewRow = dgv_List.Rows(dgv_List.SelectedCells(0).RowIndex)

        ss_System.Text = dRow.Cells("System").Value
        ss_Parent.Text = dRow.Cells("Parent").Value
        ss_Item.Text = dRow.Cells("Item").Value

        Call Get_Details()

    End Sub


    Private Sub dgv_Complete_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Complete.CellContentClick
    End Sub

    Private Sub dgv_Complete_SelectionChanged(sender As Object, e As EventArgs) Handles dgv_Complete.SelectionChanged
        If Loading = True Then Exit Sub

        If IsNothing(dgv_Complete.DataSource) Then Exit Sub
        If dgv_Complete.Rows.Count = 0 Then Exit Sub
        If IsNothing(dgv_Complete.SelectedRows) Then Exit Sub
        If dgv_Complete.SelectedCells.Count = 0 Then Exit Sub
        If IsDBNull(dgv_Complete.Rows(dgv_Complete.SelectedCells(0).RowIndex).Cells("Item").Value) Then Exit Sub

        Dim dRow As DataGridViewRow = dgv_Complete.Rows(dgv_Complete.SelectedCells(0).RowIndex)

        ss_System.Text = dRow.Cells("System").Value
        ss_Parent.Text = dRow.Cells("Parent").Value
        ss_Item.Text = dRow.Cells("Item").Value

        Call Get_Details()
    End Sub

    Private Sub panel_Objective_Leave(sender As Object, e As EventArgs) Handles panel_Objective.Leave, panel_Issues.Leave, panel_Notes.Leave, panel_Milestones.Leave
        Call txt_MouseLeave(sender, e)
    End Sub

    Private Sub panel_Objective_MouseDown(sender As Object, e As MouseEventArgs) Handles panel_Objective.MouseDown, panel_Issues.MouseDown, panel_Notes.MouseDown, panel_Milestones.MouseDown
        Call txt_MouseDown(sender, e)
    End Sub

    'Private Sub panel_Issues_MouseEnter(sender As Object, e As EventArgs) Handles panel_Issues.MouseEnter, panel_Issues.MouseEnter, panel_Notes.MouseEnter, panel_Milestones.MouseEnter
    '    Dim panl As Panel
    '    panl = CType(sender, Panel)
    '    Dim pnt As Point = New Point(panl.PointToScreen(System.Windows.Forms.Control.MousePosition).X, _
    '                                 panl.PointToClient(System.Windows.Forms.Control.MousePosition).Y)
    '    Call txt_MouseOver(sender, pnt)
    'End Sub

    'Private Sub panel_Issues_MouseHover(sender As Object, e As EventArgs) Handles panel_Issues.MouseHover, panel_Issues.MouseHover, panel_Notes.MouseHover, panel_Milestones.MouseHover

    '    Call txt_MouseMove(sender, e)

    'End Sub

    Private Sub panel_Objective_MouseMove(sender As Object, e As EventArgs) Handles panel_Objective.MouseMove, panel_Issues.MouseMove, panel_Notes.MouseMove, panel_Milestones.MouseMove
        Call txt_MouseMove(sender, e)
    End Sub

    Private Sub panel_Objective_MouseUp(sender As Object, e As MouseEventArgs) Handles panel_Objective.MouseUp, panel_Issues.MouseUp, panel_Notes.MouseUp, panel_Milestones.MouseUp
        Call txt_MouseUp(sender, e)
        Dim panl As Panel
        panl = CType(sender, Panel)
        panl.Refresh()
    End Sub

    Private Sub panel_Objective_Paint(sender As Object, e As PaintEventArgs) Handles panel_Objective.Paint, panel_Issues.Paint, panel_Notes.Paint, panel_Milestones.Paint
        'MsgBox(sender.name)
        Dim panl As Panel
        panl = CType(sender, Panel)
        Dim g As Graphics = panl.CreateGraphics()
        Dim panelRect As Rectangle = panl.ClientRectangle

        Dim p1 As Point = New Point(panelRect.Left, panelRect.Top)  'top left
        Dim p2 As Point = New Point(panelRect.Right, panelRect.Top)  'Top Right (was .Rigtht - 1, changed to +1 for overlap)
        Dim p3 As Point = New Point(panelRect.Left, panelRect.Bottom)  'Bottom Left
        Dim p4 As Point = New Point(panelRect.Right, panelRect.Bottom)  'Bottom Right (was .Rigtht - 1, changed to +1 for overlap)

        Dim pen1 As Pen = New Pen(System.Drawing.Color.DodgerBlue, 3)
        pen1.Alignment = Drawing2D.PenAlignment.Inset

        g.DrawLine(pen1, p1, p2) 'Top
        g.DrawLine(pen1, p1, p3) 'Left Side
        g.DrawLine(pen1, p2, p4) 'Right Side
        g.DrawLine(pen1, p3, p4) 'Bottom

        'Resize markers
        Dim r2 As Point = New Point(panelRect.Right, panelRect.Height - 20)  'Top Right (was .Rigtht - 1, changed to +1 for overlap)
        Dim r3 As Point = New Point(panelRect.Right - 20, panelRect.Bottom)  'Bottom Left
        Dim r4 As Point = New Point(panelRect.Right, panelRect.Bottom)  'Bottom Right (was .Rigtht - 1, changed to +1 for overlap)

        Dim pen2 As Pen = New Pen(System.Drawing.Color.DimGray, 5)
        pen2.Alignment = Drawing2D.PenAlignment.Inset

        g.DrawLine(pen2, r2, r4) 'Right Side
        g.DrawLine(pen2, r3, r4) 'Bottom


    End Sub

#Region "Panel Dragging"
    'Private txt As TextBox
    Private pnl As Panel
    Private pnlptX, pnlptY As Integer
    Private Sub txt_MouseLeave(sender As Object, e As EventArgs)
        Me.Cursor = Cursors.Arrow
    End Sub
    Dim MoveMode As Boolean
    Private Sub txt_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        'If DragMode = True Then
        MoveMode = True
        If e.Button = MouseButtons.Left Then
            'txt = CType(sender, TextBox)
            pnl = CType(sender, Panel)
            pnlptX = e.X : pnlptY = e.Y
            If e.X >= pnl.Width - 10 Then
                pnl.Cursor = Cursors.Cross
            Else
                pnl.Cursor = Cursors.Arrow
            End If

            If e.Y >= pnl.Height - 10 Then
                pnl.Cursor = Cursors.Cross
            Else
                pnl.Cursor = Cursors.Arrow
            End If
        End If
        'End If
    End Sub

    Private Sub txt_MouseOver(ByVal sender As Object, ByVal e As Point)
        pnl = CType(sender, Panel)
        pnlptX = e.X : pnlptY = e.Y
        If e.X >= pnl.Width - 10 Then
            pnl.Cursor = Cursors.Cross
        Else
            pnl.Cursor = Cursors.Arrow
        End If

        If e.Y >= pnl.Height - 10 Then
            pnl.Cursor = Cursors.Hand
        Else
            pnl.Cursor = Cursors.Arrow
        End If

    End Sub

    Private Sub txt_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        If MoveMode = True Then
            If pnl.Cursor = Cursors.Cross Then
                pnl.Width = e.X
                pnl.Height = e.Y
            Else
                pnl.Location = New Point(pnl.Location.X + e.X - pnlptX, pnl.Location.Y + e.Y - pnlptY)
                Me.Refresh()
            End If
        Else

            pnl = CType(sender, Panel)
            pnlptX = e.X : pnlptY = e.Y
            If e.X >= pnl.Width - 20 AndAlso e.Y >= pnl.Height - 20 Then
                pnl.Cursor = Cursors.Cross
            ElseIf e.Y <= pnl.Top + 20 Then
                pnl.Cursor = Cursors.Hand
            Else
                pnl.Cursor = Cursors.Arrow
            End If

        End If
    End Sub

    Private Sub txt_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        'txt = CType(sender, TextBox)
        pnl = CType(sender, Panel)
        If pnl.Cursor = Cursors.Cross Then
            pnl.Cursor = Cursors.Arrow
        End If
        MoveMode = False
    End Sub
#End Region

    Private Sub txt_StartDate_TextChanged(sender As Object, e As EventArgs) Handles txt_StartDate.TextChanged
        If Refreshing = True Then Exit Sub
        If String.IsNullOrEmpty(txt_StartDate.Text) Then Exit Sub
        'If String.IsNullOrEmpty(txt_EstTime.Text) Then Exit Sub
        If String.IsNullOrEmpty(cbo_UM.Text) Then Exit Sub
        If Not IsDate(txt_StartDate.Text) Then Exit Sub
        If CDate(txt_StartDate.Text).ToShortDateString < Today.AddDays(-365) Then Exit Sub

        Refreshing = True

        'Set Estimated Time if it is blank but Start And Due Date are both loaded with dates
        If String.IsNullOrEmpty(txt_EstTime.Text) AndAlso IsDate(txt_DueDate.Text) Then
            If DateDiff(DateInterval.Day, CDate(txt_StartDate.Text), CDate(txt_DueDate.Text)) >= 1 Then
                txt_EstTime.Text = DateDiff(DateInterval.Day, CDate(txt_StartDate.Text), CDate(txt_DueDate.Text))
                cbo_UM.Text = "day(s)"
            End If
            Exit Sub
        End If

        'Calculate Due Date if Start Date and Estimated Time loaded
        Select Case cbo_UM.Text
            Case "hrs"
                txt_DueDate.Text = CDate(txt_StartDate.Text)
            Case "day(s)", "days"
                Try
                    txt_DueDate.Text = DateAddWeekDaysOnly(CDate(txt_StartDate.Text), CLng(txt_EstTime.Text)).ToShortDateString
                    'txt_DueDate.Text = CDate(CDate(txt_StartDate.Text)).AddDays(CLng(txt_EstTime.Text)).ToShortDateString
                Catch ex As Exception
                End Try
        End Select

        If tab_Details.SelectedTab.Text = "Details" AndAlso tab_DetailsMain.SelectedTab.Text = "Schedule" Then
            Call Handler_CreateSchedule(pnl_Sch)
        End If

        Refreshing = False
    End Sub


    Private Sub txt_DueDate_TextChanged(sender As Object, e As EventArgs) Handles txt_DueDate.TextChanged
        If Refreshing = True Then Exit Sub
        If String.IsNullOrEmpty(txt_DueDate.Text) Then Exit Sub
        If String.IsNullOrEmpty(cbo_UM.Text) Then Exit Sub
        If Not IsDate(txt_DueDate.Text) Then Exit Sub
        If CDate(txt_DueDate.Text).ToShortDateString < Today.AddDays(-365) Then Exit Sub

        Refreshing = True

        'Set Estimated Time if it is blank but Start And Due Date are both loaded with dates
        If String.IsNullOrEmpty(txt_EstTime.Text) AndAlso IsDate(txt_StartDate.Text) Then
            If DateDiff(DateInterval.Day, CDate(txt_StartDate.Text), CDate(txt_DueDate.Text)) >= 1 Then
                txt_EstTime.Text = DateDiff(DateInterval.Day, CDate(txt_StartDate.Text), CDate(txt_DueDate.Text))
                cbo_UM.Text = "day(s)"
            End If
            Exit Sub
        End If

        'Calculate Start Date if Due Date and Estimated Time loaded
        Select Case cbo_UM.Text
            Case "hrs"
                txt_StartDate.Text = CDate(txt_DueDate.Text)
            Case "day(s)", "days"
                Try
                    txt_StartDate.Text = DateAddWeekDaysOnly(CDate(txt_DueDate.Text), -CLng(txt_EstTime.Text)).ToShortDateString
                    'txt_StartDate.Text = CDate(CDate(txt_DueDate.Text)).AddDays(-CLng(txt_EstTime.Text)).ToShortDateString
                Catch ex As Exception
                End Try
        End Select

        'Now check to see if due date is > Parent Due Date and ask if the parent needs to be extended
        Dim strSrch As String = "System='" & ss_System.Text & "' AND Path='" & tvw_Items.SelectedNode.Parent.FullPath & "'"
        If dsSys.Tables("dtItems").Select(strSrch).Any Then
            If Not IsDBNull(dsSys.Tables("dtItems").Select(strSrch).CopyToDataTable.Rows(0).Item("DueDate")) Then
                If CDate(dsSys.Tables("dtItems").Select(strSrch).CopyToDataTable.Rows(0).Item("DueDate")) < CDate(txt_DueDate.Text) Then
                    Dim answ As MsgBoxResult = MsgBox("Update the Parent Item?", MsgBoxStyle.YesNo, "UPDATE PARENT")
                    If answ = MsgBoxResult.Yes Then
                        MsgBox(dsSys.Tables("dtItems").Select(strSrch).CopyToDataTable.Rows(0).Item("DueDate") & " < " & CDate(txt_DueDate.Text))
                    End If
                End If
            End If
            'Dim FilterTxt As String = "System='" & dgRow.Cells("System").Value & "' AND [Path]='" & dgRow.Cells("Parent").Value & "' "
            'dtHeader = New DataView(dsSys.Tables("dtItems"), strSrch, "iIndex, Item ASC", DataViewRowState.CurrentRows).ToTable(True, "Item", "Notes", "Phase", "Status", "EstTime", "EstTimeUM", "StartDate", "DueDate", "Requester", "RequestDue", "Urgency", "Complexity", "Pri", "Parent")
        End If

        Refreshing = False

    End Sub

    Private Sub txt_EstTime_TextChanged(sender As Object, e As EventArgs) Handles txt_EstTime.TextChanged, cbo_UM.SelectedIndexChanged
        If Refreshing = True Then Exit Sub
        If String.IsNullOrEmpty(txt_EstTime.Text) Then Exit Sub
        If String.IsNullOrEmpty(cbo_UM.Text) Then Exit Sub
        If String.IsNullOrEmpty(txt_DueDate.Text) AndAlso String.IsNullOrEmpty(txt_StartDate.Text) Then Exit Sub

        Refreshing = True
        If Not String.IsNullOrEmpty(txt_StartDate.Text) Then 'AndAlso String.IsNullOrEmpty(txt_DueDate.Text) Then
            'StartDate is loaded but Due Date is empty
            If Not IsDate(txt_StartDate.Text) Then Exit Sub
            Select Case cbo_UM.Text
                Case "hrs"
                    txt_DueDate.Text = CDate(txt_StartDate.Text)
                Case "day(s)", "days"
                    Try
                        txt_DueDate.Text = DateAddWeekDaysOnly(CDate(txt_StartDate.Text), CLng(txt_EstTime.Text)).ToShortDateString
                        'txt_DueDate.Text = CDate(CDate(txt_StartDate.Text)).AddDays(CLng(txt_EstTime.Text)).ToShortDateString
                    Catch ex As Exception
                    End Try
            End Select
        ElseIf Not String.IsNullOrEmpty(txt_DueDate.Text) AndAlso String.IsNullOrEmpty(txt_StartDate.Text) Then
            'DueDate is loaded but Start Date is empty
            If Not IsDate(txt_DueDate.Text) Then Exit Sub
            Select Case cbo_UM.Text
                Case "hrs"
                    txt_StartDate.Text = CDate(txt_DueDate.Text)
                Case "day(s)", "days"
                    Try
                        txt_StartDate.Text = DateAddWeekDaysOnly(CDate(txt_DueDate.Text), -CLng(txt_EstTime.Text)).ToShortDateString
                        'txt_StartDate.Text = CDate(CDate(txt_DueDate.Text)).AddDays(-CLng(txt_EstTime.Text)).ToShortDateString
                    Catch ex As Exception
                    End Try
            End Select
        End If

        Refreshing = False

    End Sub



    'Private Sub page_Structure_Paint(sender As Object, e As PaintEventArgs) Handles page_Structure.Paint

    '    If IsNothing(tvw_Items.SelectedNode) Then Exit Sub

    '    'tvw_Items.SelectedNode.BackColor = Color.Orange
    '    'tvw_Items.Refresh()

    '    Dim g As Graphics = tvw_Items.CreateGraphics()
    '    Dim panelRect As Rectangle = tvw_Items.ClientRectangle

    '    Dim p1 As Point = New Point(tvw_Items.SelectedNode.Bounds.Left - 2, tvw_Items.SelectedNode.Bounds.Top + 2)  'top left
    '    Dim p2 As Point = New Point(tvw_Items.SelectedNode.Bounds.Right + 15, tvw_Items.SelectedNode.Bounds.Top + 2)  'Top Right 
    '    Dim p3 As Point = New Point(tvw_Items.SelectedNode.Bounds.Left - 2, tvw_Items.SelectedNode.Bounds.Bottom - 2)  'Bottom Left
    '    Dim p4 As Point = New Point(tvw_Items.SelectedNode.Bounds.Right + 15, tvw_Items.SelectedNode.Bounds.Bottom - 2)  'Bottom Right 

    '    Dim pen1 As Pen = New Pen(System.Drawing.Color.Orange, 2)
    '    pen1.Alignment = Drawing2D.PenAlignment.Inset

    '    g.DrawLine(pen1, p1, p2) 'Top
    '    g.DrawLine(pen1, p1, p3) 'Left Side
    '    g.DrawLine(pen1, p2, p4) 'Right Side
    '    g.DrawLine(pen1, p3, p4) 'Bottom

    'End Sub

    'Private Sub btn_ImageZoomIn_Click(sender As Object, e As EventArgs)

    '    pic_PartImage.Width = pic_PartImage.Width + 50
    '    pic_PartImage.Height = pic_PartImage.Height + 50 '* zm

    '    txt_ImageZoom.Text = Math.Round(pic_PartImage.Width / (panel_Image.Width - 20), 2) * 100 & "%"

    '    panel_Image.Refresh()

    'End Sub

    'Private Sub btn_ImageZoomOut_Click(sender As Object, e As EventArgs)

    '    pic_PartImage.Width = pic_PartImage.Width - 50
    '    pic_PartImage.Height = pic_PartImage.Height - 50 '* zm

    '    txt_ImageZoom.Text = Math.Round(pic_PartImage.Width / (panel_Image.Width - 20), 2) * 100 & "%"

    '    panel_Image.Refresh()

    'End Sub

    'Private Sub txt_ImageZoom_TextChanged(sender As Object, e As EventArgs)

    'End Sub

    Private Sub btn_Exit_Click(sender As Object, e As EventArgs) Handles btn_Exit.Click
        Me.Close()
    End Sub


    'Private Sub btn_ImageZoomFit_Click(sender As Object, e As EventArgs)
    '    pic_PartImage.Width = panel_Image.Width - 20
    '    pic_PartImage.Height = panel_Image.Height - 20
    '    txt_ImageZoom.Text = "100%"
    'End Sub

    'Private Sub tvw_Items_LostFocus(sender As Object, e As EventArgs) Handles tvw_Items.LostFocus
    '    Call page_Structure_Paint(sender, e)
    'End Sub

    Private Sub tvw_Items_MouseEnter(sender As Object, e As EventArgs) Handles tvw_Items.MouseEnter
        'tvw_Items.Select()
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

    'Private Sub frm_Main_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
    '    'tab_Details.

    'End Sub

    Private Sub pic_MenuList_Click(sender As Object, e As EventArgs) Handles pic_MenuList.Click
        tab_Main.SelectedTab = page_List

    End Sub

    Private Sub pic_MenuStructure_Click(sender As Object, e As EventArgs) Handles pic_MenuStructure.Click
        tab_Main.SelectedTab = page_Main

    End Sub
    Private Sub pic_MenuSetup_Click(sender As Object, e As EventArgs) Handles pic_MenuOffDays.Click
        tab_Main.SelectedTab = page_OffDays
    End Sub

    Private Sub pic_MenuCal_Click(sender As Object, e As EventArgs) Handles pic_MenuCal.Click
        tab_Main.SelectedTab = page_Calendar
    End Sub

    Private Sub dgv_PriorityList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_PriorityList.CellContentClick

    End Sub
    Private Sub dgv_PriorityList_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgv_PriorityList.CellBeginEdit

        If IsDBNull(dgv_PriorityList.Rows(e.RowIndex).Cells("Pri").Value) Then
            lbl_ListPriority.Text = String.Empty
        Else
            lbl_ListPriority.Text = CInt(dgv_PriorityList.Rows(e.RowIndex).Cells("Pri").Value)
        End If

    End Sub

    Private Sub dgv_PriorityList_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_PriorityList.CellEndEdit
        'If Priority field is updated


        If e.ColumnIndex = 0 Then 'if the Priority field was updated

            Dim xPri As Integer = CInt(dgv_PriorityList.Rows(e.RowIndex).Cells("Pri").Value)

            Dim uCmd As New SqlCommand 'UPDATE Query
            Select Case lbl_ListPriority.Text
                Case String.Empty 'Not previously prioritized
                    'Insert Current Row into priority location & Update the rest from that priority down
                    For Each row As DataGridViewRow In dgv_PriorityList.Rows
                        If Not IsDBNull(row.Cells("Pri").Value) AndAlso row.Cells("Pri").Value >= xPri Then
                            'Dim uCmd As New SqlCommand 'UPDATE Query
                            uCmd.CommandText = "UPDATE tbl_ToDoItems " & _
                                                "SET OverallPriority=@Pri " & _
                                                "WHERE System=@Sys AND Item=@Itm AND [Parent]=@Par "
                            uCmd.Parameters.AddWithValue("@Sys", row.Cells("System").Value)
                            uCmd.Parameters.AddWithValue("@Itm", row.Cells("Item").Value)
                            uCmd.Parameters.AddWithValue("@Par", row.Cells("Parent").Value)
                            'if it hits the current (modified) row, don't add 1....only if it was already set.
                            uCmd.Parameters.AddWithValue("@Pri", If(row.Index = dgv_PriorityList.CurrentCell.RowIndex, xPri, row.Cells("Pri").Value + 1))
                            uCmd.Parameters.AddWithValue("@UpDt", Now())
                            Call WriteUpdateSQL(uCmd)
                            uCmd.Parameters.Clear()
                        End If
                    Next
                Case Else
                    If lbl_ListPriority.Text > xPri Then 'moving up in priority
                        'For i As Integer = xPri To CInt(lbl_ListPriority.Text)
                        'Move items down that are above the new priority
                        uCmd.CommandText = "UPDATE tbl_ToDoItems " & _
                                               "SET OverallPriority=OverallPriority + 1 " & _
                                               "WHERE OverallPriority < " & CInt(lbl_ListPriority.Text) & _
                                               "      AND OverallPriority >= " & xPri
                        Call WriteUpdateSQL(uCmd)
                        uCmd.Parameters.Clear()

                        'Now update selected item
                        uCmd.CommandText = "UPDATE tbl_ToDoItems " & _
                                                "SET OverallPriority=@Pri " & _
                                                "WHERE System=@Sys AND Item=@Itm AND [Parent]=@Par "
                        uCmd.Parameters.AddWithValue("@Sys", dgv_PriorityList.Rows(e.RowIndex).Cells("System").Value)
                        uCmd.Parameters.AddWithValue("@Itm", dgv_PriorityList.Rows(e.RowIndex).Cells("Item").Value)
                        uCmd.Parameters.AddWithValue("@Par", dgv_PriorityList.Rows(e.RowIndex).Cells("Parent").Value)
                        'if it hits the current (modified) row, don't add 1....only if it was already set.
                        uCmd.Parameters.AddWithValue("@Pri", xPri)
                        uCmd.Parameters.AddWithValue("@UpDt", Now())
                        Call WriteUpdateSQL(uCmd)
                        uCmd.Parameters.Clear()

                        'Next
                    Else 'moving down in priority
                        'Move items up that are below the new priority
                        uCmd.CommandText = "UPDATE tbl_ToDoItems " & _
                                               "SET OverallPriority=OverallPriority - 1 " & _
                                               "WHERE OverallPriority > " & CInt(lbl_ListPriority.Text) & _
                                               "      AND OverallPriority <= " & xPri
                        Call WriteUpdateSQL(uCmd)
                        uCmd.Parameters.Clear()

                        'Now update selected item
                        uCmd.CommandText = "UPDATE tbl_ToDoItems " & _
                                                "SET OverallPriority=@Pri " & _
                                                "WHERE System=@Sys AND Item=@Itm AND [Parent]=@Par "
                        uCmd.Parameters.AddWithValue("@Sys", dgv_PriorityList.Rows(e.RowIndex).Cells("System").Value)
                        uCmd.Parameters.AddWithValue("@Itm", dgv_PriorityList.Rows(e.RowIndex).Cells("Item").Value)
                        uCmd.Parameters.AddWithValue("@Par", dgv_PriorityList.Rows(e.RowIndex).Cells("Parent").Value)
                        'if it hits the current (modified) row, don't add 1....only if it was already set.
                        uCmd.Parameters.AddWithValue("@Pri", xPri)
                        uCmd.Parameters.AddWithValue("@UpDt", Now())
                        Call WriteUpdateSQL(uCmd)
                        uCmd.Parameters.Clear()
                    End If
            End Select



            'uCmd.CommandText = "UPDATE tbl_ToDoItems " & _
            '                    "SET OverallPriority=@Pri " & _
            '                    "WHERE System=@Sys AND Item=@Itm AND [Parent]=@Par "
            'uCmd.Parameters.AddWithValue("@Sys", dgv_PriorityList.Rows(e.RowIndex).Cells("System").Value)
            'uCmd.Parameters.AddWithValue("@Itm", dgv_PriorityList.Rows(e.RowIndex).Cells("Item").Value)
            'uCmd.Parameters.AddWithValue("@Par", dgv_PriorityList.Rows(e.RowIndex).Cells("Parent").Value)
            'uCmd.Parameters.AddWithValue("@Pri", dgv_PriorityList.Rows(e.RowIndex).Cells("Pri").Value)
            'uCmd.Parameters.AddWithValue("@UpDt", Now())
            'Call WriteUpdateSQL(uCmd)
            'uCmd.Parameters.Clear()

            
        End If

        Call Load_Systems()
        Refresh_PriorityList()

    End Sub

    Private Sub HideFromListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HideFromListToolStripMenuItem.Click
        If IsNothing(dgv_PriorityList.DataSource) Then Exit Sub
        If Not dgv_PriorityList.Rows.Count > 0 Then Exit Sub

        Dim xPri As Integer = CInt(dgv_PriorityList.Rows(dgv_PriorityList.CurrentCell.RowIndex).Cells("Pri").Value)
        Dim uCmd As New SqlCommand 'UPDATE Query
        uCmd.CommandText = "UPDATE tbl_ToDoItems " & _
                            "SET ShowInList=@Shw " & _
                            "WHERE System=@Sys AND Item=@Itm AND [Parent]=@Par "
        uCmd.Parameters.AddWithValue("@Sys", dgv_PriorityList.Rows(dgv_PriorityList.CurrentCell.RowIndex).Cells("System").Value)
        uCmd.Parameters.AddWithValue("@Itm", dgv_PriorityList.Rows(dgv_PriorityList.CurrentCell.RowIndex).Cells("Item").Value)
        uCmd.Parameters.AddWithValue("@Par", dgv_PriorityList.Rows(dgv_PriorityList.CurrentCell.RowIndex).Cells("Parent").Value)
        uCmd.Parameters.AddWithValue("@Shw", False)
        uCmd.Parameters.AddWithValue("@UpDt", Now())
        Call WriteUpdateSQL(uCmd)
        uCmd.Parameters.Clear()

        Call Load_Systems()
        Refresh_PriorityList()

    End Sub

    Private Sub check_Show_CheckedChanged(sender As Object, e As EventArgs) Handles check_Show.CheckedChanged
        If Refreshing = True Then Exit Sub
        If Reordering = True Then Exit Sub
        If Loading = True Then Exit Sub
        If IsNothing(tvw_Items) Then Exit Sub
        If tvw_Items.Nodes.Count = 0 Then Exit Sub
        If IsNothing(tvw_Items.SelectedNode) Then Exit Sub
        If IsDBNull(tvw_Items.SelectedNode.FullPath) Then Exit Sub
        If tvw_Items.SelectedNode.Level = 0 Then Exit Sub

        'When Check state changes, Priority needs to be Nulled out.  This is to keep duplicate priorities from ending up in the system.  If an item is Re-Checked to 'Show', the priority will have to be re-assessed.
        Dim uCmd As New SqlCommand 'UPDATE Query
        uCmd.CommandText = "UPDATE tbl_ToDoItems " & _
                            "SET ShowInList=@Shw, OverallPriority=@Pri " & _
                            "WHERE System=@Sys AND Item=@Itm AND [Parent]=@Par "
        uCmd.Parameters.AddWithValue("@Sys", ss_System.Text)
        uCmd.Parameters.AddWithValue("@Itm", ss_Item.Text)
        uCmd.Parameters.AddWithValue("@Par", ss_Parent.Text)
        uCmd.Parameters.AddWithValue("@Shw", check_Show.CheckState)
        uCmd.Parameters.AddWithValue("@Pri", DBNull.Value)
        uCmd.Parameters.AddWithValue("@UpDt", Now())
        Call WriteUpdateSQL(uCmd)
        uCmd.Parameters.Clear()

        Call Load_Items()
        Refresh_PriorityList()

    End Sub

    Private Sub dgv_PriorityList_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_PriorityList.CellDoubleClick
        dgv_PriorityList.EndEdit()
        Dim dRow As DataGridViewRow = dgv_PriorityList.Rows(e.RowIndex)
        tvw_Items.SelectedNode = GetNodeByFullPath(tvw_Items.Nodes, dRow.Cells("Parent").Value & " || " & dRow.Cells("Item").Value)
        tab_Main.SelectedTab = page_Main

    End Sub


    Private Sub pic_Weekly_Click(sender As Object, e As EventArgs) Handles pic_Weekly.Click
        tab_Main.SelectedTab = page_Week

    End Sub

    Private Sub pic_Mgmt_Click(sender As Object, e As EventArgs) Handles pic_Mgmt.Click
        tab_Main.SelectedTab = page_MgmtVw

    End Sub


    Private Sub dgv_WklyProj_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_WklyProj.CellContentClick

    End Sub

    Private Sub dgv_WklyProj_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_WklyProj.CellContentDoubleClick

        Dim dRow As DataGridViewRow = dgv_WklyProj.Rows(e.RowIndex)
        tvw_Items.SelectedNode = GetNodeByFullPath(tvw_Items.Nodes, dRow.Cells("Parent").Value & " || " & dRow.Cells("Item").Value)
        tab_Main.SelectedTab = page_Main

    End Sub

   

    Private Sub dgv_WklyProj_SelectionChanged(sender As Object, e As EventArgs) Handles dgv_WklyProj.SelectionChanged
        If Loading = True Then Exit Sub

        GoTo Clear_Labels

Load_Data:
        If IsNothing(dgv_WklyProj.DataSource) Then Exit Sub
        If dgv_WklyProj.Rows.Count = 0 Then Exit Sub
        If IsNothing(dgv_WklyProj.SelectedRows) Then Exit Sub
        If dgv_WklyProj.SelectedCells.Count = 0 Then Exit Sub
        If IsDBNull(dgv_WklyProj.Rows(dgv_WklyProj.SelectedCells(0).RowIndex).Cells("Item").Value) Then Exit Sub

        Dim dRow As DataGridViewRow = dgv_WklyProj.Rows(dgv_WklyProj.SelectedCells(0).RowIndex)

        'ss_System.Text = dRow.Cells("System").Value
        'ss_Parent.Text = dRow.Cells("Parent").Value
        'ss_Item.Text = dRow.Cells("Item").Value

        'Call Get_Details()
        lbl_SelectedName.Text = dRow.Cells("Item").Value
        lbl_TaskOrGroup.Text = "Group"

        Dim strSrch As String = "System='" & dRow.Cells("System").Value & "' AND Item='" & dRow.Cells("Item").Value & "' AND [Parent]='" & dRow.Cells("Parent").Value & "'"

        If dsSys.Tables("dtItems").Select(strSrch).Any Then
            Dim drItem As DataRow
            drItem = dsSys.Tables("dtItems").Select(strSrch).CopyToDataTable.Rows(0)
            If Not String.IsNullOrEmpty(drItem.Item("Notes").ToString) Then rtxt_WklyItem.Rtf = _
                ConvertTextToRTF(drItem.Item("Notes"))

            'Load Images & Image Notes
            Call ctrImg_Weekly.Get_Images(dRow.Cells("System").Value, dRow.Cells("Parent").Value, dRow.Cells("Item").Value)

        End If

        Exit Sub
Clear_Labels:
        'dgv_WklyTasks.ClearSelection()
        rtxt_WklyItem.Text = ""
        ctrImg_Weekly.rtxt_ImageNotes.Text = ""
        ctrImg_Weekly.pic_PartImage.Image = Nothing
        ctrImg_Weekly.label_ImageNum.Text = ""
        ctrImg_Weekly.label_Image.Text = ""
        ctrImg_Weekly.label_ImageTotQty.Text = ""
        lbl_SelectedName.Text = ""
        lbl_TaskOrGroup.Text = ""
        'Call ctrImg_Weekly.Get_Images("", "", "")

        GoTo Load_Data
    End Sub

    Private Sub dgv_Weekly_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_WklyTasks.CellContentClick

    End Sub

    Private Sub dgv_Weekly_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_WklyTasks.CellDoubleClick

        Dim dRow As DataGridViewRow = dgv_WklyTasks.Rows(e.RowIndex)
        'tvw_Items.SelectedNode = GetNodeByText(tvw_Items.Nodes, dRow.Cells("Item").Value)
        tvw_Items.SelectedNode = GetNodeByFullPath(tvw_Items.Nodes, dRow.Cells("Parent").Value & " || " & dRow.Cells("Item").Value)
        tab_Main.SelectedTab = page_Main

    End Sub

    Private Sub dgv_Weekly_DataSourceChanged(sender As Object, e As EventArgs) Handles dgv_WklyTasks.DataSourceChanged
        'Color Code line items
        'Start Today - Green 
        'Due Today - Blue
        'Past Due - Red
        If dgv_WklyTasks.Rows.Count > 0 Then
            Select Case tvw_Weekly.SelectedNode.Level
                Case 1 'Weekly

                Case 2 'Daily
                    For Each dRow As DataGridViewRow In dgv_WklyTasks.Rows
                        If DateTime.Compare(CDate(dRow.Cells("StartDate").Value).ToShortDateString, CDate(tvw_Weekly.SelectedNode.Text).ToShortDateString) = 0 Then
                            dRow.DefaultCellStyle.ForeColor = Color.Green
                        End If
                        If DateTime.Compare(CDate(dRow.Cells("DueDate").Value).ToShortDateString, CDate(tvw_Weekly.SelectedNode.Text).ToShortDateString) = 0 Then
                            dRow.DefaultCellStyle.ForeColor = Color.Blue
                            dRow.DefaultCellStyle.Font = New Font(dgv_WklyTasks.Font.Name, dgv_WklyTasks.Font.Size + 1, FontStyle.Bold)
                        End If
                        If DateTime.Compare(CDate(tvw_Weekly.SelectedNode.Text).ToShortDateString, CDate(dRow.Cells("DueDate").Value).ToShortDateString) > 0 Then
                            System.Diagnostics.Debug.WriteLine(CDate(tvw_Weekly.SelectedNode.Text).ToShortDateString & " > " & CDate(dRow.Cells("DueDate").Value).ToShortDateString)
                            dRow.DefaultCellStyle.ForeColor = Color.Red
                        End If
                    Next
            End Select
        End If
    End Sub

    Private Sub dgv_Weekly_SelectionChanged(sender As Object, e As EventArgs) Handles dgv_WklyTasks.SelectionChanged
        If Loading = True Then Exit Sub

        GoTo Clear_Labels

Load_Data:
        If IsNothing(dgv_WklyTasks.DataSource) Then Exit Sub
        If dgv_WklyTasks.Rows.Count = 0 Then Exit Sub
        If IsNothing(dgv_WklyTasks.SelectedRows) Then Exit Sub
        If dgv_WklyTasks.SelectedCells.Count = 0 Then Exit Sub
        If IsDBNull(dgv_WklyTasks.Rows(dgv_WklyTasks.SelectedCells(0).RowIndex).Cells("Item").Value) Then Exit Sub

        Dim dRow As DataGridViewRow = dgv_WklyTasks.Rows(dgv_WklyTasks.SelectedCells(0).RowIndex)

        'ss_System.Text = dRow.Cells("System").Value
        'ss_Parent.Text = dRow.Cells("Parent").Value
        'ss_Item.Text = dRow.Cells("Item").Value

        'Call Get_Details()
        lbl_SelectedName.Text = dRow.Cells("Item").Value
        lbl_TaskOrGroup.Text = "Task"

        Dim strSrch As String = "System='" & dRow.Cells("System").Value & "' AND Item='" & dRow.Cells("Item").Value & "' AND [Parent]='" & dRow.Cells("Parent").Value & "'"

        If dsSys.Tables("dtItems").Select(strSrch).Any Then
            Dim drItem As DataRow
            drItem = dsSys.Tables("dtItems").Select(strSrch).CopyToDataTable.Rows(0)
            If Not String.IsNullOrEmpty(drItem.Item("Notes").ToString) Then rtxt_WklyItem.Rtf = _
                ConvertTextToRTF(drItem.Item("Notes"))

            'Load Images & Image Notes
            Call ctrImg_Weekly.Get_Images(dRow.Cells("System").Value, dRow.Cells("Parent").Value, dRow.Cells("Item").Value)

        End If

        Exit Sub
Clear_Labels:
        'dgv_WklyProj.ClearSelection()
        rtxt_WklyItem.Text = ""
        ctrImg_Weekly.rtxt_ImageNotes.Text = ""
        ctrImg_Weekly.pic_PartImage.Image = Nothing
        ctrImg_Weekly.label_ImageNum.Text = ""
        ctrImg_Weekly.label_Image.Text = ""
        ctrImg_Weekly.label_ImageTotQty.Text = ""
        lbl_SelectedName.Text = ""
        lbl_TaskOrGroup.Text = ""
        'Call ctrImg_Weekly.Get_Images("", "", "")

        GoTo Load_Data
    End Sub

    Private Sub tvw_Weekly_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tvw_Weekly.AfterSelect
        If Loading = True Then Exit Sub

        GoTo Clear_Labels

Load_Data:
        If IsNothing(tvw_Weekly) Then Exit Sub
        If tvw_Weekly.Nodes.Count = 0 Then Exit Sub
        If IsNothing(tvw_Weekly.SelectedNode) Then Exit Sub
        If IsDBNull(tvw_Weekly.SelectedNode.FullPath) Then Exit Sub
        If tvw_Weekly.SelectedNode.Level = 0 Then Exit Sub

        Dim NewString As String() = tvw_Weekly.SelectedNode.FullPath.ToString.Split(New String() {"\"}, StringSplitOptions.None)
        Dim strSearchNt As String = "" 'Weekly Notes
        Dim strSearchItm As String = "System = 'x' " 'ToDo Task Items
        Dim strSearchProj As String = "System = 'x' " 'ToDo Task Items
        Dim dtRange As String = ""
        Select Case tvw_Weekly.SelectedNode.Level
            Case 1 'Weekly
                strSearchNt = "wkRange='" & NewString(1) & "' "
                If dtWkly.Select(strSearchNt).Any Then
                    If Not String.IsNullOrEmpty(dtWkly.Select(strSearchNt).CopyToDataTable.Rows(0).Item("wkNotes").ToString) Then
                        rtxt_WklyNotes.Rtf = ConvertTextToRTF(dtWkly.Select(strSearchNt).CopyToDataTable.Rows(0).Item("wkNotes"))
                    End If
                End If

                dtRange = "AND ((StartDate >= #" & _
                    CDate(dtWkly.Select("wkRange='" & NewString(1) & "'").CopyToDataTable.Rows(0).Item("wkStart")) & "# AND " & _
                    "StartDate <= #" & CDate(dtWkly.Select("wkRange='" & NewString(1) & "'").CopyToDataTable.Rows(0).Item("wkEnd")) & "#) OR (DueDate >= #" & _
                    CDate(dtWkly.Select("wkRange='" & NewString(1) & "'").CopyToDataTable.Rows(0).Item("wkStart")) & "# AND " & _
                    "DueDate <= #" & CDate(dtWkly.Select("wkRange='" & NewString(1) & "'").CopyToDataTable.Rows(0).Item("wkEnd")) & "#))"

                strSearchItm = "NOT Phase ='Complete' AND Type IN ('Task', 'Bug Fix', 'Enhancement') " & dtRange
                strSearchProj = "NOT Phase ='Complete' AND Type IN ('Phase', 'Activity', 'Milestone', 'Activity Group') " & dtRange

            Case 2 'Daily
                strSearchNt = "wkRange='" & NewString(1) & "' AND wkDate=#" & CDate(tvw_Weekly.SelectedNode.Text) & "# "

                If dtDaily.Select(strSearchNt).Any Then
                    If Not String.IsNullOrEmpty(dtDaily.Select(strSearchNt).CopyToDataTable.Rows(0).Item("wkDailyNote").ToString) Then
                        rtxt_WklyNotes.Rtf = ConvertTextToRTF(dtDaily.Select(strSearchNt).CopyToDataTable.Rows(0).Item("wkDailyNote"))
                    End If
                End If

                dtRange = "AND ((StartDate<=#" & CDate(tvw_Weekly.SelectedNode.Text) & "# AND DueDate>=#" & CDate(tvw_Weekly.SelectedNode.Text) & "#) OR (StartDate=#" & CDate(tvw_Weekly.SelectedNode.Text) & "# OR DueDate<=#" & CDate(tvw_Weekly.SelectedNode.Text) & "#) OR (NOT Phase ='Ice Box' AND (StartDate<#" & CDate(tvw_Weekly.SelectedNode.Text) & "# AND DueDate>=#" & CDate(tvw_Weekly.SelectedNode.Text) & "#)))"

                strSearchItm = "NOT Phase ='Complete' AND Type IN ('Task', 'Bug Fix', 'Enhancement') " & dtRange
                strSearchProj = "NOT Phase ='Complete' AND Type IN ('Phase', 'Activity', 'Milestone', 'Activity Group') " & dtRange

                ctrl_OpenItmsPlan.dt = dsSys.Tables("dtItems")
                ctrl_OpenItmsPlan.Paint_OpenItems(CDate(tvw_Weekly.SelectedNode.Text))

            Case Else
                Exit Sub
        End Select

        'Tasks List
        If dsSys.Tables("dtItems").Select(strSearchItm).Any Then
            Dim displayView = New DataView(dsSys.Tables("dtItems").Select(strSearchItm, "DueDate ASC, StartDate, sIndex, iIndex ASC").CopyToDataTable)
            Dim subset As DataTable = displayView.ToTable(True, "Item", "DueDate", "Type", "Phase", "System", "Parent", "PercentComplete", "StartDate")
            subset.Columns("PercentComplete").ColumnName = "% Comp"
            dgv_WklyTasks.DataSource = subset 'dsSys.Tables("dtItems").Select(strSearch, "DueDate ASC, iIndex ASC").CopyToDataTable
            dgv_WklyTasks.Columns("Item").Width = 200
            dgv_WklyTasks.Columns("DueDate").Width = 68
            dgv_WklyTasks.Columns("StartDate").Width = 68
        End If

        'Projects List
        If dsSys.Tables("dtItems").Select(strSearchProj).Any Then
            Dim displayView = New DataView(dsSys.Tables("dtItems").Select(strSearchProj, "DueDate ASC, StartDate, sIndex, iIndex ASC").CopyToDataTable)
            Dim subset As DataTable = displayView.ToTable(True, "Item", "DueDate", "Type", "Phase", "System", "Parent", "PercentComplete", "StartDate")
            subset.Columns("PercentComplete").ColumnName = "% Comp"
            dgv_WklyProj.DataSource = subset 'dsSys.Tables("dtItems").Select(strSearch, "DueDate ASC, iIndex ASC").CopyToDataTable
            dgv_WklyProj.Columns("Item").Width = 200
            dgv_WklyProj.Columns("DueDate").Width = 68
            dgv_WklyProj.Columns("StartDate").Width = 68
        End If

        If tvw_Weekly.SelectedNode.Nodes.Count > 0 Then tvw_Weekly.SelectedNode.Expand()

        tvw_Weekly.Refresh()

        Exit Sub

Clear_Labels:
        dgv_WklyTasks.DataSource = Nothing
        rtxt_WklyNotes.Text = ""
        lbl_DailyDate.Text = ""
        GoTo Load_Data

    End Sub

    Sub Load_WeeklyPlanCal()

        rtxt_WklyPlan.Text = ""

        Dim strSearchNt As String = "wkDate=#" & CDate(cctrl_Cal.DateRef) & "# "

        If IsNothing(dtWkly) OrElse IsNothing(dtDaily) Then
            Get_WeeklyPlanData()
        End If

        If dtDaily.Select(strSearchNt).Any Then
            If Not String.IsNullOrEmpty(dtDaily.Select(strSearchNt).CopyToDataTable.Rows(0).Item("wkDailyNote").ToString) Then
                rtxt_WklyPlan.Rtf = ConvertTextToRTF(dtDaily.Select(strSearchNt).CopyToDataTable.Rows(0).Item("wkDailyNote"))
            End If
        End If


        ctrl_OpenItmWklyPlan.dt = dsSys.Tables("dtItems")
        ctrl_OpenItmWklyPlan.Paint_OpenItems(cctrl_Cal.DateRef)

    End Sub

    Sub Load_WeeklyPlan()

        If IsNothing(dtWkly) OrElse IsNothing(dtDaily) Then
            Get_WeeklyPlanData()
        End If

        'If dtWkly.Rows.Count > 0 AndAlso dtDaily.Rows.Count > 0 Then Exit Sub

        Loading = True

        Try

            'Dim VisIndex As Boolean = False 'determine 1st node that is visible (where scrolled to)
            'Dim scrollpnt As New Point 'store the scroll point

            'Load Weekly and Daily Data
            Call Get_WeeklyPlanData()

            If dtWkly.Rows.Count = 0 Then Exit Sub

            'Fill in the treeview
            tvw_Weekly.Nodes.Clear()

            Dim tvTier As TreeNode
            tvTier = New TreeNode("Weekly Plan")
            tvTier.Name = "Weekly Plan"
            tvw_Weekly.Nodes.Add(tvTier)

            Dim currentFont As System.Drawing.Font = tvw_Weekly.Font

            For Each parentrow As DataRow In dtWkly.Rows
                'If Not parentrow.Item("Phase").ToString = "Complete" Then
                Dim parentnode As TreeNode
                parentnode = tvTier.Nodes.Add(parentrow.Item(1))
                parentnode.Name = parentrow.Item(1)
                'parentnode.Tag = If(IsDBNull(parentrow.Item("Expanded")), "", If(parentrow.Item("Expanded") = True, "Expanded", "")) 'parentrow.Item("System")

                ''''''Change font for Systems'''
                parentnode.NodeFont = New Font(currentFont.FontFamily, currentFont.Size + 1, FontStyle.Bold)
                parentnode.Bounds.Inflate(parentnode.Bounds.Width + 50, parentnode.Bounds.Height)


                ''''populate child'''''
                '''''''''''''''''''''''
                Dim strSrch As String = "wkRange='" & parentnode.Text & "' "
                If dtDaily.Select(strSrch).Any Then
                    Dim childnode As New TreeNode()
                    'dsSys.Tables("dtItems").DefaultView.Sort = "iIndex ASC"
                    For Each childrow As DataRow In dtDaily.Select(strSrch, "wkDate ASC").CopyToDataTable.Rows
                        If chk_HideWeekends.Checked = True AndAlso (CDate(childrow(2)).DayOfWeek = DayOfWeek.Saturday _
                                                                    Or CDate(childrow(2)).DayOfWeek = DayOfWeek.Sunday) Then
                            Continue For 'Skip item if HideWeekends is checked and the Day is Sat or Sun
                        End If
                        childnode = parentnode.Nodes.Add(childrow(2)) '& " " & childrow(2))
                        childnode.Name = childrow(2).ToString
                        'childnode.Tag = If(IsDBNull(childrow.Item("Expanded")), "", If(childrow.Item("Expanded") = True, "Expanded", "")) 'childrow("Item")
                    Next childrow
                End If

            Next parentrow

            tvw_Weekly.Nodes(0).Expand()
            'tvw_Weekly.Nodes(0).Nodes(0).Expand()

            'For Each tNode As TreeNode In GetAllNodes(tvw_Weekly)
            '    If tNode.Tag = "Expanded" Then
            '        tNode.Expand()
            '    End If
            'Next

            'Set Scroll Position
            'If VisIndex = True Then
            '    SetScrollPos(tvw_Items.Handle, SB_HORZ, scrollpnt.X, True)
            '    SetScrollPos(tvw_Items.Handle, SB_VERT, scrollpnt.Y, True)
            'End If

            'Call tvw_Items_AfterSelect(Nothing, Nothing)

            'Go To today's date
            'Dim nodes As TreeNode() = tvw_Weekly.Nodes(0).Nodes.Find(Date.Today.ToShortDateString.ToString, True)
            'Dim node As TreeNode

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Loading = False

        Call goto_Today()

        ctrl_OpenItmsPlan.dt = dsSys.Tables("dtItems")
        ctrl_OpenItmsPlan.Paint_OpenItems(Date.Today)

    End Sub


    Sub Get_WeeklyPlanData()

        'Loading = True

        dtWkly = New DataTable
        Dim Sql As String = "SELECT * FROM tbl_ToDoWeekly  WHERE userID = '" & ss_User.Text & "' ORDER BY wkEnd DESC "
        Call Load_Data(Sql, dtWkly)

        dtDaily = New DataTable
        Sql = "SELECT * FROM tbl_ToDoDaily  WHERE userID = '" & ss_User.Text & "' ORDER BY wkDate ASC "
        Call Load_Data(Sql, dtDaily)

        'Loading = False

    End Sub

    Sub goto_Today()
        For Each nodes As TreeNode In tvw_Weekly.Nodes(0).Nodes
            For Each node As TreeNode In nodes.Nodes
                If node.Text = Date.Today.ToShortDateString.ToString Then
                    tvw_Weekly.SelectedNode = node
                    tvw_Weekly_AfterSelect(Me, Nothing)
                    Exit Sub
                End If
            Next
        Next
    End Sub

    Private Sub btn_Today_Click(sender As Object, e As EventArgs) Handles btn_Today.Click
        goto_Today()
    End Sub

    Private Sub btn_AddWeek_Click(sender As Object, e As EventArgs) Handles btn_AddWeek.Click
        frm_Calendar.Show()

    End Sub

    Private Sub chk_HideWeekends_CheckedChanged(sender As Object, e As EventArgs) Handles chk_HideWeekends.CheckedChanged
        If Loading = True Then Exit Sub
        Call Load_WeeklyPlan()
    End Sub

    Sub Get_DateRange()
        Dim dtRange As Date = frm_Calendar.MonthCalendar1.SelectionStart

        Dim ans As MsgBoxResult
        ans = MsgBox("Are you Sure you want to add Date Range " & dtRange & "", MsgBoxStyle.YesNoCancel, "Add Date Range")
        If ans = MsgBoxResult.Yes Then
            '[DO ADD WEEK RANGE TO WEEKLY PLAN]
        End If
    End Sub

    Private Function GetNodeByText(nodes As TreeNodeCollection, searchtext As String) As TreeNode
        Dim n_found_node As TreeNode = Nothing
        Dim b_node_found As Boolean = False

        For Each node As TreeNode In nodes

            If node.Text = searchtext Then
                b_node_found = True
                n_found_node = node

                Return n_found_node
            End If

            If Not b_node_found Then
                n_found_node = GetNodeByText(node.Nodes, searchtext)

                If n_found_node IsNot Nothing Then
                    Return n_found_node
                End If
            End If
        Next
        Return Nothing
    End Function

    Function GetNodeByFullPath(nodes As TreeNodeCollection, searchtext As String) As TreeNode
        Dim n_found_node As TreeNode = Nothing
        Dim b_node_found As Boolean = False

        For Each node As TreeNode In nodes

            If node.FullPath = searchtext Then
                b_node_found = True
                n_found_node = node

                Return n_found_node
            End If

            If Not b_node_found Then
                n_found_node = GetNodeByFullPath(node.Nodes, searchtext)

                If n_found_node IsNot Nothing Then
                    Return n_found_node
                End If
            End If
        Next
        Return Nothing
    End Function

    Private Sub AddNewTaskToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddNewTaskToolStripMenuItem.Click
        frm_NewItem.ShowDialog()
    End Sub


    Private Sub tvw_Items_NodeMouseClick(sender As Object, e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvw_Items.NodeMouseClick
        'Move selection to right-clicked Node for context menu
        If e.Button = Windows.Forms.MouseButtons.Right Then
            tvw_Items.SelectedNode = e.Node
        End If
    End Sub

    Private Sub tab_Details_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tab_Details.SelectedIndexChanged
        Call SelectionChange_Handler()
    End Sub


    Sub Load_MgmtView()

        Dim mySL As New SortedList()
        mySL.Add(0, "Emergency")
        mySL.Add(1, "Complete")
        mySL.Add(2, "Testing")
        mySL.Add(3, "In Progress")
        mySL.Add(4, "Ice Box")

        Dim FilterTxt As String = "Type IN ('System Feature', 'System', 'Enhancement') AND ShowInList='True' AND (Status NOT IN ('Complete') OR (Status='Complete' AND MgmtReview='False'))"

        Dim dt As DataTable = New DataView(dsSys.Tables("dtItems"), FilterTxt, "Phase, Pri, DueDate, Urgency DESC, iIndex, StartDate, Item ASC", DataViewRowState.CurrentRows).ToTable(True, "Item", "System", "Path", "DueDate", "StartDate", "Phase", "Status", "Pri", "Urgency", "iIndex")

        'Now Re-order by Phase
        Dim ids = {"Emergency", "Complete", "Testing", "In Progress", "Ice Box"} 'Setting Phase Order
        Dim query = From id In ids
                    Join row In dt.AsEnumerable On id Equals row(5)
                    Select row

        dgv_Mgmt.DataSource = query.CopyToDataTable


        'dgv_Mgmt.DataSource = New DataView(dsSys.Tables("dtItems"), FilterTxt, "Phase, Pri, DueDate, Urgency DESC, iIndex, StartDate, Item ASC", DataViewRowState.CurrentRows).ToTable(True, "Item", "System", "Path", "DueDate", "StartDate", "Phase", "Status")

        Call dgv_Mgmt_RowEnter(Nothing, Nothing)

    End Sub

    Private Sub dgv_Mgmt_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Mgmt.CellContentClick

    End Sub

    Private Sub dgv_Mgmt_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Mgmt.CellDoubleClick
        Dim dRow As DataGridViewRow = dgv_Mgmt.Rows(e.RowIndex)
        tvw_Items.SelectedNode = GetNodeByText(tvw_Items.Nodes, dRow.Cells("Item").Value)
        tab_Main.SelectedTab = page_Main
    End Sub

    Private Sub dgv_Mgmt_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgv_Mgmt.CellMouseDoubleClick

    End Sub
    Private Sub dgv_Mgmt_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Mgmt.RowEnter
        If IsNothing(dgv_Mgmt.SelectedRows) Then Exit Sub
        If dgv_Mgmt.SelectedRows.Count = 0 Then Exit Sub

        'Call Paint_Schedule()

        If tab_Main.SelectedTab.Text = "Mgmt View" Then
            Call Handler_CreateSchedule(pnl_MgmtSchedule)
        End If

    End Sub

    Private Sub dgv_Mgmt_SelectionChanged(sender As Object, e As EventArgs) Handles dgv_Mgmt.SelectionChanged

    End Sub

    Private Sub dgv_Mgmt_DataSourceChanged(sender As Object, e As EventArgs) Handles dgv_Mgmt.DataSourceChanged
        If IsNothing(dgv_Mgmt.DataSource) Then Exit Sub
        If dgv_Mgmt.Rows.Count = 0 Then Exit Sub

        For Each r As DataGridViewRow In dgv_Mgmt.Rows
            If Not IsNothing(r.Cells("Phase").Value) Then
                Select Case r.Cells("Phase").Value.ToString
                    Case "Complete"
                        r.DefaultCellStyle.ForeColor = Color.Gray
                    Case "Ice Box"
                        r.DefaultCellStyle.ForeColor = Color.Blue
                    Case "In Progress"
                        r.DefaultCellStyle.ForeColor = Color.Green
                    Case "Emergency"
                        r.DefaultCellStyle.ForeColor = Color.Red
                End Select
            End If
        Next

    End Sub


    Private Sub frm_Main_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged

        Select Case tab_Main.SelectedTab.Text
            Case "List"

            Case "Main"
                If tab_Details.SelectedTab.Text = "Open Items" Then ctr_OpenItms.Paint_OpenItems(Date.Today)
            Case "Mgmt View"
                'Call Paint_Schedule()
                Call Handler_CreateSchedule(pnl_MgmtSchedule)
            Case "Weekly Plan"
                ctrl_OpenItmsPlan.Paint_OpenItems(Date.Today)
            Case "Calendar"
                'cctrl_Cal.LoadCalendar()
                'Load_WeeklyPlanCal()
                ctrl_OpenItmWklyPlan.Paint_OpenItems(cctrl_Cal.DateRef)
        End Select

    End Sub

    Private Sub page_Schedule_Click(sender As Object, e As EventArgs) Handles page_Schedule.Click

    End Sub

    Private Sub AddPhaseStructureToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddPhaseStructureToolStripMenuItem.Click

        Dim uCmd As New SqlCommand
        Dim pth As String = tvw_Items.SelectedNode.FullPath
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

        uCmd.Parameters.AddWithValue("@Sys", ss_System.Text)
        uCmd.Parameters.AddWithValue("@Itm", txt_Item.Text)
        uCmd.Parameters.AddWithValue("@Par", pth) 'combo_Parent.Text)
        uCmd.Parameters.AddWithValue("@Phs", "") 'This is required for new items to show up
        uCmd.Parameters.AddWithValue("@UpDt", Now())
        Call WriteUpdateSQL(uCmd)
        uCmd.Parameters.Clear()

        Call Load_Systems()

    End Sub

    Private Sub page_Schedule_Paint(sender As Object, e As PaintEventArgs) Handles page_Schedule.Paint

    End Sub

    Private Sub page_Schedule_Resize(sender As Object, e As EventArgs) Handles page_Schedule.Resize

    End Sub

    Private Sub pnl_Sch_Paint(sender As Object, e As PaintEventArgs) Handles pnl_Sch.Paint

    End Sub

    Private Sub page_Schedule_SizeChanged(sender As Object, e As EventArgs) Handles page_Schedule.SizeChanged

    End Sub

    Private Sub pnl_Sch_SizeChanged(sender As Object, e As EventArgs) Handles pnl_Sch.SizeChanged

        '!!!!! Doing this now when MgmtReviewNotes Panel location changes.  Using this event was redrawing the Schedule prior to the MgmtReviewNotes panel moving out of the way which left a white box over Schedule where the panel used to be !!!!

        'If tab_Details.SelectedTab.Text = "Details" AndAlso tab_DetailsMain.SelectedTab.Text = "Schedule" Then
        '    Call Handler_CreateSchedule(pnl_Sch)
        'End If
    End Sub

    Private Sub chk_MgmtReview_CheckedChanged(sender As Object, e As EventArgs) Handles chk_MgmtReview.CheckedChanged
        txt_Priority.Text = ""
    End Sub

    Private Sub ToolStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles ToolStrip1.ItemClicked

    End Sub

    Private Sub tab_DetailsMain_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tab_DetailsMain.SelectedIndexChanged
        If tab_Details.SelectedTab.Text = "Details" AndAlso tab_DetailsMain.SelectedTab.Text = "Schedule" Then
            Call Handler_CreateSchedule(pnl_Sch)
        End If
    End Sub

    Private Sub Panel4_Paint(sender As Object, e As PaintEventArgs) Handles pnl_MgmtRevNotes.Paint

    End Sub

    Private Sub ToolStrip1_Paint(sender As Object, e As PaintEventArgs) Handles ToolStrip1.Paint
        Call AddBevel(e.Graphics, ToolStrip1)
    End Sub

    Private Sub pnl_MgmtReview_Paint(sender As Object, e As PaintEventArgs) Handles pnl_MgmtReview.Paint

    End Sub

    Private Sub pnl_MgmtRevNotes_LocationChanged(sender As Object, e As EventArgs) Handles pnl_MgmtRevNotes.LocationChanged
        If tab_Details.SelectedTab.Text = "Details" AndAlso tab_DetailsMain.SelectedTab.Text = "Schedule" Then
            Call Handler_CreateSchedule(pnl_Sch)
        End If
    End Sub

    Private Sub pnl_MgmtReview_SizeChanged(sender As Object, e As EventArgs) Handles pnl_MgmtReview.SizeChanged
        'Set Mgmt Review Notes to the bottom.  Can't do it with normal Anchor settings as it messes up the AutoScroll setting
        pnl_MgmtNotes.Height = pnl_MgmtReview.Height - pnl_MgmtNotes.Top - 2
    End Sub

    Private Sub dgv_OffDays_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_OffDays.CellContentClick

    End Sub
    Private Sub dgv_OffDays_SelectionChanged(sender As Object, e As EventArgs) Handles dgv_OffDays.SelectionChanged

        If IsNothing(dgv_OffDays) Then Exit Sub
        If IsNothing(dgv_OffDays.DataSource) Then Exit Sub
        If dgv_OffDays.Rows.Count = 0 Then Exit Sub

        GoTo clear_labels

fill_labels:

        'get item data
        Dim drOD() As DataRow = Nothing
        drOD = dtOffDays.Select("ID=" & dgv_OffDays.SelectedRows(0).Cells("ID").Value & "")
        If drOD.Length > 0 Then
            'If drOD(0)("Type").ToString = "System" Or drOD(0)("Type").ToString = "System Feature" Then
            'Item, DateStart, DateEnd, Comments, Image, UserID
            lbl_OffDayStart.Text = If(Not String.IsNullOrEmpty(drOD(0)("DateStart")), If(IsDate(drOD(0)("DateStart")), CDate(drOD(0)("DateStart")).ToShortDateString, "select"), "select")
            lbl_OffDayEnd.Text = If(Not String.IsNullOrEmpty(drOD(0)("DateEnd")), If(IsDate(drOD(0)("DateEnd")), CDate(drOD(0)("DateEnd")).ToShortDateString, "select"), "select")
            lbl_OffDayID.Text = dgv_OffDays.SelectedRows(0).Cells("ID").Value
            txt_OffDayItem.Text = drOD(0)("Item").ToString
            txt_OffDayComments.Text = drOD(0)("Comments").ToString
            cbo_OffDayType.Text = If(drOD(0)("UserID") = "All", "Plant", If(drOD(0)("UserID") = "All", "Personal", ""))

            'PTO
            If Not IsDBNull(drOD(0)("Item")) Then
                If drOD(0)("Item") = "PTO" Then
                    chk_OffDayPTO.Checked = True
                    'pnl_OffDaysPTO.Visible = True
                    txt_PTOHrs.Text = drOD(0)("Hours").ToString
                    'Get PTO Data
                    If IsDate(drOD(0)("DateStart")) Then
                        Dim drPTO() As DataRow = Nothing
                        drPTO = dtFactors.Select("Item='PTO Hours' AND Factor='" & CDate(drOD(0)("DateStart")).Year & "'")
                        If drPTO.Length > 0 Then
                            lbl_PTOTotal.Text = drPTO(0)("Property").ToString
                            If Not String.IsNullOrEmpty(drPTO(0)("Property")) Then
                                'Dim sumObject As Object
                                'sumObject =
                                lbl_PTOScheduled.Text = dtOffDays.Compute("SUM(Hours)", "DateStart >= #" & New DateTime(CDate(drOD(0)("DateStart")).Year, 1, 1) & "# AND DateEnd <= #" & New DateTime(CDate(drOD(0)("DateStart")).Year, 12, 31) & "#").ToString
                                lbl_PTOTaken.Text = dtOffDays.Compute("SUM(Hours)", "(DateStart >= #" & New DateTime(CDate(drOD(0)("DateStart")).Year, 1, 1) & "# AND DateStart <= #" & Date.Today & "#) AND (DateStart >= #" & New DateTime(CDate(drOD(0)("DateStart")).Year, 1, 1) & "# And DateEnd <= #" & New DateTime(CDate(drOD(0)("DateStart")).Year, 12, 31) & "#)").ToString
                                lbl_PTORemaining.Text = CDbl(lbl_PTOTotal.Text) - CDbl(lbl_PTOScheduled.Text)
                            End If
                        End If
                    End If
                End If
            End If
        End If

        Exit Sub

clear_labels:
        lbl_OffDayNew.Visible = False
        lbl_OffDayID.Text = ""
        lbl_OffDayStart.Text = "Select"
        lbl_OffDayEnd.Text = "Select"
        txt_OffDayItem.Text = ""
        txt_OffDayComments.Text = ""
        chk_OffDayPTO.Checked = False
        txt_PTOHrs.Text = ""
        'pnl_OffDaysPTO.Visible = False

        If dgv_OffDays.SelectedRows.Count > 0 Then
            If dgv_OffDays.SelectedRows(0).IsNewRow Then
                lbl_OffDayNew.Visible = True
            Else
                GoTo fill_labels
            End If
        End If
    End Sub

    Private Sub ctr_OpenItms_SizeChanged(sender As Object, e As EventArgs) Handles ctr_OpenItms.SizeChanged
        Call ctr_OpenItms.Paint_OpenItems()
    End Sub

    Private Sub btn_NewOffDay_Click(sender As Object, e As EventArgs) Handles btn_NewOffDay.Click
        dgv_OffDays.Rows(dgv_OffDays.Rows.Count - 1).Selected = True
    End Sub

    Private Sub btn_SaveOffDay_Click(sender As Object, e As EventArgs) Handles btn_SaveOffDay.Click

        Dim dgvR As DataGridViewRow = dgv_OffDays.SelectedRows(0)

        'IF PTO ONLY - Create or Save an entry for each day (This is so PTO Calculations will work)

        Select Case txt_OffDayItem.Text
            Case "PTO"
                For Each xDay As Date In Enumerable.Range(0, (CDate(lbl_OffDayEnd.Text) - CDate(lbl_OffDayStart.Text)).Days + 1).Select(Function(i) CDate(lbl_OffDayStart.Text).AddDays(i))

                    'Skip if weekend
                    If xDay.DayOfWeek = DayOfWeek.Saturday OrElse xDay.DayOfWeek = DayOfWeek.Sunday Then Continue For

                    Dim cmd As New SqlCommand
                    cmd.CommandText = "If (Select COUNT(*) FROM tbl_ToDo_OffDays WHERE ID=@ID) > 0  " &
                        "BEGIN UPDATE tbl_ToDo_OffDays  " &
                        "   Set Item=@Itm, DateStart=@sDt, DateEnd=@eDt, Hours=@Hrs, Comments=@Com, UserID=@Usr  " &
                        "   WHERE ID=@ID     End  " &
                        "Else BEGIN INSERT INTO tbl_ToDo_OffDays (Item, DateStart, DateEnd, Hours, Comments, UserID)  " &
                        "   VALUES (@Itm, @sDt, @eDt, @Hrs, @Com, @Usr)       End "
                    cmd.Parameters.AddWithValue("@ID", If(String.IsNullOrEmpty(lbl_OffDayID.Text), DBNull.Value, CInt(lbl_OffDayID.Text)))
                    cmd.Parameters.AddWithValue("@Itm", If(String.IsNullOrEmpty(txt_OffDayItem.Text), DBNull.Value, txt_OffDayItem.Text))
                    cmd.Parameters.AddWithValue("@sDt", xDay)
                    cmd.Parameters.AddWithValue("@eDt", xDay)
                    cmd.Parameters.AddWithValue("@Hrs", If(String.IsNullOrEmpty(txt_PTOHrs.Text), 8, CInt(txt_PTOHrs.Text)))
                    cmd.Parameters.AddWithValue("@Com", If(String.IsNullOrEmpty(txt_OffDayComments.Text), DBNull.Value, txt_OffDayComments.Text))
                    cmd.Parameters.AddWithValue("@Usr", If(txt_OffDayItem.Text = "PTO", ss_User.Text, "All"))

                    WriteUpdateSQL(cmd)

                Next

            Case Else 'just write one entry
                Dim cmd As New SqlCommand
                cmd.CommandText = "If (Select COUNT(*) FROM tbl_ToDo_OffDays WHERE ID=@ID) > 0  " &
                    "BEGIN UPDATE tbl_ToDo_OffDays  " &
                    "   Set Item=@Itm, DateStart=@sDt, DateEnd=@eDt, Comments=@Com, UserID=@Usr  " &
                    "   WHERE ID=@ID     End  " &
                    "Else BEGIN INSERT INTO tbl_ToDo_OffDays (Item, DateStart, DateEnd, Comments, UserID)  " &
                    "   VALUES (@Itm, @sDt, @eDt, @Com, @Usr)       End "
                cmd.Parameters.AddWithValue("@ID", If(String.IsNullOrEmpty(lbl_OffDayID.Text), DBNull.Value, CInt(lbl_OffDayID.Text)))
                cmd.Parameters.AddWithValue("@Itm", If(String.IsNullOrEmpty(txt_OffDayItem.Text), DBNull.Value, txt_OffDayItem.Text))
                cmd.Parameters.AddWithValue("@sDt", If(String.IsNullOrEmpty(lbl_OffDayStart.Text), DBNull.Value, CDate(lbl_OffDayStart.Text)))
                cmd.Parameters.AddWithValue("@eDt", If(String.IsNullOrEmpty(lbl_OffDayEnd.Text), DBNull.Value, CDate(lbl_OffDayEnd.Text)))
                cmd.Parameters.AddWithValue("@Com", If(String.IsNullOrEmpty(txt_OffDayComments.Text), DBNull.Value, txt_OffDayComments.Text))
                cmd.Parameters.AddWithValue("@Usr", If(txt_OffDayItem.Text = "PTO", ss_User.Text, "All"))

                WriteUpdateSQL(cmd)
        End Select


        Load_OffDays()


    End Sub

    Private Sub btn_OffDayDelete_Click(sender As Object, e As EventArgs) Handles btn_OffDayDelete.Click

        If String.IsNullOrEmpty(lbl_OffDayID.Text) Then Exit Sub

        Dim cmd As New SqlCommand
        cmd.CommandText = "DELETE tbl_ToDo_OffDays   WHERE ID=@ID  "
        cmd.Parameters.AddWithValue("@ID", CInt(lbl_OffDayID.Text))
        WriteUpdateSQL(cmd)

        Load_OffDays()

    End Sub

    Private Sub lbl_OffDayStart_Click(sender As Object, e As EventArgs) Handles lbl_OffDayStart.Click
        frm_Calendar.dtCtrl = CType(lbl_OffDayStart, Label)
        frm_Calendar.Show()
    End Sub

    Private Sub lbl_OffDayEnd_Click(sender As Object, e As EventArgs) Handles lbl_OffDayEnd.Click
        frm_Calendar.dtCtrl = CType(lbl_OffDayEnd, Label)
        frm_Calendar.Show()
    End Sub

    Private Sub chk_OffDaysViewPast_CheckedChanged(sender As Object, e As EventArgs) Handles chk_OffDaysViewPast.CheckedChanged
        If Loading = True Then Exit Sub
        Load_OffDays()
    End Sub

    Private Sub ctrl_OpenItmsPlan_Load(sender As Object, e As EventArgs) Handles ctrl_OpenItmsPlan.Load

    End Sub

    Private Sub PinNotStartedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PinNotStartedToolStripMenuItem.Click
        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub


        'Dim img As Bitmap = New Bitmap(My.Resources.pin.Width, My.Resources.pin.Height)
        'Dim gpx As Graphics = Graphics.FromImage(img)
        'gpx.Clear(rtBox.BackColor)
        'gpx.DrawImage(My.Resources.pin, Point.Empty)

        Dim img As Bitmap = New Bitmap(15, 15) '(My.Resources.pin.Width, My.Resources.pin.Height)
        'img = My.Resources.pin
        Dim gpx As Graphics = Graphics.FromImage(img)
        gpx.Clear(rtBox.BackColor)
        gpx.DrawImage(My.Resources.pin, 0, 0, 15, 15)
        Try
            Clipboard.SetImage(img)
            rtBox.Paste() 'Image)
        Catch ex As Exception

        End Try


    End Sub




    'Private Sub chk_OffDayPTO_CheckedChanged(sender As Object, e As EventArgs) Handles chk_OffDayPTO.CheckedChanged
    '    'If chk_OffDayPTO.Checked = True Then
    '    '    pnl_OffDaysPTO.Visible = True
    '    'Else
    '    '    pnl_OffDaysPTO.Visible = False
    '    'End If
    'End Sub

    Private Sub dgv_OffDays_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_OffDays.RowEnter

    End Sub

    Private Sub BugBugFixToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BugBugFixToolStripMenuItem.Click
        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub

        Dim img As Bitmap = New Bitmap(15, 15) '(My.Resources.pin.Width, My.Resources.pin.Height)
        'img = My.Resources.pin
        Dim gpx As Graphics = Graphics.FromImage(img)
        gpx.Clear(rtBox.BackColor)
        gpx.DrawImage(My.Resources.bug_insect, 0, 0, 15, 15)
        Clipboard.SetImage(img)
        rtBox.Paste() 'Image)
    End Sub

    Private Sub IdeaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles IdeaToolStripMenuItem.Click
        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub

        Dim img As Bitmap = New Bitmap(15, 15) '(My.Resources.pin.Width, My.Resources.pin.Height)
        'img = My.Resources.pin
        Dim gpx As Graphics = Graphics.FromImage(img)
        gpx.Clear(rtBox.BackColor)
        gpx.DrawImage(My.Resources.bulb_idea, 0, 0, 15, 15)
        Clipboard.SetImage(img)
        rtBox.Paste() 'Image)
    End Sub

    Private Sub btn_CopyRTF_Click(sender As Object, e As EventArgs) Handles btn_CopyRTF.Click
        'Clipboard.SetText(rtxt_WklyPlan.Text)
        Clipboard.SetText(rtxt_WklyPlan.Rtf, TextDataFormat.Rtf)
        'RichTextBox.SelectAll & then richtextbox.copy  Is working correctly
    End Sub

    Private Sub MadeProgressToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MadeProgressToolStripMenuItem.Click
        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub

        Dim img As Bitmap = New Bitmap(17, 15) '(My.Resources.pin.Width, My.Resources.pin.Height)
        'img = My.Resources.pin
        Dim gpx As Graphics = Graphics.FromImage(img)
        gpx.Clear(rtBox.BackColor)
        gpx.DrawImage(My.Resources.progress_up, 0, 0, 15, 15)
        Try
            Clipboard.SetImage(img)
            rtBox.Paste() 'Image)
        Catch ex As Exception
        End Try

    End Sub
    Private Sub page_Calendar_SizeChanged(sender As Object, e As EventArgs) Handles page_Calendar.SizeChanged
        'ctrl_OpenItmWklyPlan.Paint_OpenItems(cctrl_Cal.DateRef)
    End Sub

    Private Sub dgv_TaskJournal_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_TaskJournal.CellContentClick

    End Sub

    Private Sub dgv_TaskJournal_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_TaskJournal.CellEndEdit

        Dim exTag As mTag = CType(tvw_Items.SelectedNode.Tag, mTag)

        Dim dgvRow As DataGridViewRow = dgv_TaskJournal.Rows(e.RowIndex)

        'For New entries - if no Current Journal Date has been set (i.e. it is a new entry) but the new entry date has been set in the datagridview, Then set the CurrJournalDate variable to the new entry date so the insert/update query will work
        If IsNothing(CurrJournalDate) And IsDate(dgvRow.Cells("WorkDtPlan").Value.ToString) Then CurrJournalDate = CDate(dgvRow.Cells("WorkDtPlan").Value).ToShortDateString

        'needs a Description and Plan date to be saved
        If Not String.IsNullOrEmpty(dgvRow.Cells("WorkDescription").Value.ToString) AndAlso
                IsDate(dgvRow.Cells("WorkDtPlan").Value.ToString) Then '

            Dim cmd As New SqlCommand
            cmd.CommandText = "IF EXISTS(SELECT 1 FROM tbl_ToDoTaskJournal WHERE Task=@Tsk AND Architect=@Usr AND Path=@Pth AND " &
                " WorkDescription=@Desc AND WorkDtPlan=@upDtPln) " &
                "BEGIN UPDATE tbl_ToDoTaskJournal " &
                "  SET WorkDescription=@upDsc, WorkDtPlan=@upDtPln, WorkHrsPlan=@HrPln, WorkDtActual=@DtAct, WorkHrsActual=@HrAct, Notes=@Nt" &
                "  WHERE id=@id    END " &
                "ELSE BEGIN INSERT INTO tbl_ToDoTaskJournal (Task, Architect, Path, WorkDescription, WorkDtPlan, WorkHrsPlan, WorkDtActual, WorkHrsActual, Notes) " &
                "  VALUES(@Tsk, @Usr, @Pth, @upDsc, @upDtPln, @HrPln, @DtAct, @HrAct, @Nt) " &
                "  SET @Identity = SCOPE_IDENTITY()     END "
            cmd.Parameters.AddWithValue("@id", dgvRow.Cells("id").Value) 'id
            cmd.Parameters.AddWithValue("@Tsk", tvw_Items.SelectedNode.Text) 'Task
            cmd.Parameters.AddWithValue("@Usr", ss_User.Text) 'Architect
            cmd.Parameters.AddWithValue("@Pth", exTag.Get("Path")) 'Path
            cmd.Parameters.AddWithValue("@Desc", CurrJournalDesc) 'WorkDescription
            cmd.Parameters.AddWithValue("@upDsc", dgvRow.Cells("WorkDescription").Value.ToString) 'WorkDescription (updated)
            cmd.Parameters.AddWithValue("@DtPln", If(IsNothing(CurrJournalDate), DBNull.Value, CurrJournalDate)) 'WorkDtPlan
            cmd.Parameters.AddWithValue("@upDtPln", CDate(dgvRow.Cells("WorkDtPlan").Value).ToShortDateString) 'WorkDtPlan
            cmd.Parameters.AddWithValue("@HrPln", dgvRow.Cells("WorkHrsPlan").Value.ToString) 'WorkHrsPlan
            cmd.Parameters.AddWithValue("@DtAct", If(IsDate(dgvRow.Cells("WorkDtActual").Value.ToString), CDate(dgvRow.Cells("WorkDtActual").Value).ToShortDateString, DBNull.Value)) 'WorkDtActual
            cmd.Parameters.AddWithValue("@HrAct", dgvRow.Cells("WorkHrsActual").Value.ToString) 'WorkHrsActual
            cmd.Parameters.AddWithValue("@Nt", dgvRow.Cells("Notes").Value.ToString) 'Notes
            'cmd.Parameters.Add("@Identity", SqlDbType.Int, 0, "id")
            Dim parameter As SqlParameter = cmd.Parameters.Add("@Identity", SqlDbType.Int, 0, "id")
            parameter.Direction = ParameterDirection.Output
            WriteUpdateSQL(cmd)

            If String.IsNullOrEmpty(dgvRow.Cells("id").Value.ToString) Then dgvRow.Cells("id").Value = cmd.Parameters("@Identity").Value

            Call Load_Items()

        End If

    End Sub

    Private Sub dgv_TaskJournal_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles dgv_TaskJournal.UserDeletingRow
        If IsNothing(dgv_TaskJournal.DataSource) Then Exit Sub
        If dgv_TaskJournal.Rows.Count = 0 Then Exit Sub

        Dim exTag As mTag = CType(tvw_Items.SelectedNode.Tag, mTag)
        Dim dgvRow As DataGridViewRow = dgv_TaskJournal.Rows(e.Row.Index)

        Dim answ As MsgBoxResult = MsgBox("Are you sure you want to delete the " & dgvRow.Cells("WorkDescription").Value.ToString &
                                          " entry for " & CDate(dgvRow.Cells("WorkDtPlan").Value).ToShortDateString & "?", MsgBoxStyle.YesNoCancel, "DELETE Task Journal Entry")

        If answ = MsgBoxResult.Yes Then
            Dim cmd As New SqlCommand
            cmd.CommandText = "DELETE tbl_ToDoTaskJournal WHERE id=@id"
            'For Each row As DataGridViewRow In dgv_Reports.SelectedRows

            cmd.Parameters.AddWithValue("@id", dgvRow.Cells("id").Value) '
            'cmd.Parameters.AddWithValue("@Tsk", tvw_Items.SelectedNode.Text) 'Task
            'cmd.Parameters.AddWithValue("@Usr", ss_User.Text) 'Architect
            'cmd.Parameters.AddWithValue("@Pth", exTag.Get("Path")) 'Path
            'cmd.Parameters.AddWithValue("@Desc", dgvRow.Cells("WorkDescription").Value.ToString) 'WorkDescription
            'cmd.Parameters.AddWithValue("@DtPln", CDate(dgvRow.Cells("WorkDtPlan").Value).ToShortDateString) 'WorkDtPlan
            WriteUpdateSQL(cmd)
            cmd.Parameters.Clear()

            'Call DeleteCCP(dgvRow, dgvRow.Cells("RefNo").Value.ToString, dgvRow.Cells("PartNo").Value.ToString)
            'Next
            cmd.Dispose()
        End If
    End Sub

    Public CurrJournalDesc As String
    Public CurrJournalDate As Nullable(Of Date) = Nothing

    Private Sub MeetingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MeetingToolStripMenuItem.Click

        Dim rtBox As RichTextBox = DetermineRichTextBox(Me.ActiveControl)
        If IsNothing(rtBox) Then Exit Sub

        Dim img As Bitmap = New Bitmap(22, 20) '(My.Resources.pin.Width, My.Resources.pin.Height)
        'img = My.Resources.pin
        Dim gpx As Graphics = Graphics.FromImage(img)
        gpx.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        gpx.Clear(rtBox.BackColor)
        gpx.DrawImage(My.Resources.CalendarEvent, 0, 3, 18, 18)
        Clipboard.SetImage(img)
        rtBox.Paste() 'Image)

    End Sub

    Private Sub CopyRTFToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyRTFToolStripMenuItem.Click
        Clipboard.SetText(rTxt_Notes.Rtf, TextDataFormat.Rtf)
    End Sub

    Private Sub dgv_TaskJournal_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgv_TaskJournal.CellBeginEdit
        If dgv_TaskJournal.Rows.Count = 0 Then Exit Sub
        'Save the Current Journal Description in case it needs to be updated
        CurrJournalDesc = dgv_TaskJournal.Rows(e.RowIndex).Cells("WorkDescription").Value.ToString

        'Save the Current Journal Date in case it is updated
        If dgv_TaskJournal.Columns(e.ColumnIndex).Name = "WorkDtPlan" Then
            If IsDate(dgv_TaskJournal.Rows(e.RowIndex).Cells("WorkDtPlan").Value) Then
                CurrJournalDate = CDate(dgv_TaskJournal.Rows(e.RowIndex).Cells("WorkDtPlan").Value).ToShortDateString
            Else
                CurrJournalDate = Nothing
            End If

        End If
    End Sub


End Class
