Imports System.Data.SqlClient
Imports System.IO
Imports System.Runtime.CompilerServices
Imports Project_ToDo.frm_Main
Imports Microsoft.Office.Interop

Module mod_Functions

    <Extension()>
    Public Function GetNodeByFullPath(ByVal treeView As TreeView, ByVal fullPath As String) As TreeNode
        Dim tn() As TreeNode = treeView.Nodes.Find(fullPath, True)
        If tn.Count > 0 Then Return tn(0) Else Return Nothing
    End Function

    Function IsBetween(Value As Integer, LowerValue As Integer, UpperValue As Integer) As Boolean
        If Value >= LowerValue And Value <= UpperValue Then
            Return True
        Else
            Return False
        End If

    End Function


    Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hWnd As Integer,
                                                                    ByVal wMsg As Integer,
                                                                    ByVal wParam As Integer,
                                                                    ByVal lParam As Integer) As Integer

    Private Const WM_SETREDRAW As Integer = 11

    'Extension methods for Control
    <Extension()>
    Public Sub ResumeDrawing(ByVal Target As Control, ByVal Redraw As Boolean)
        SendMessage(Target.Handle, WM_SETREDRAW, 1, 0)
        If Redraw Then
            Target.Refresh()
        End If
    End Sub

    <Extension()>
    Public Sub SuspendDrawing(ByVal Target As Control)
        SendMessage(Target.Handle, WM_SETREDRAW, 0, 0)
    End Sub
    <Extension()>
    Public Sub ResumeDrawing(ByVal Target As Control)
        ResumeDrawing(Target, True)
    End Sub


    Function FindMinMaxDateInRow(dRow As DataRow, op As String, Optional StartDt As Date = Nothing) As Date
        'op needs to be either "Min" or "Max"
        Dim minDt As Date = If(Not IsNothing(StartDt), StartDt, Nothing)
        For Each col As DataColumn In dRow.Table.Columns
            If IsDate(dRow.Item(col.ColumnName)) Then
                If minDt = Nothing Then
                    If op = "Max" AndAlso col.ColumnName.ToString = "StartDate" Then
                        If dRow.Table.Columns.Contains("StartDate") AndAlso dRow.Table.Columns.Contains("EstTime") Then
                            Dim calDate As Date = Calc_DueDate(dRow.Item("StartDate"), dRow.Item("EstTime"), dRow.Item("EstTimeUM"))
                            If Not IsNothing(calDate) Then minDt = calDate Else minDt = CDate(dRow.Item(col.ColumnName))
                        End If
                    Else
                        minDt = CDate(dRow.Item(col.ColumnName))
                    End If
                Else
                    Select Case op
                        Case "Min"
                            If CDate(dRow.Item(col.ColumnName)) < minDt Then
                                minDt = CDate(dRow.Item(col.ColumnName))
                            End If
                        Case "Max"
                            Select Case col.ColumnName.ToString
                                Case "StartDate"
                                    If dRow.Table.Columns.Contains("StartDate") AndAlso dRow.Table.Columns.Contains("EstTime") Then
                                        Dim calDate As Date = Calc_DueDate(dRow.Item("StartDate"), dRow.Item("EstTime"), dRow.Item("EstTimeUM"))
                                        If Not IsNothing(calDate) Then
                                            If calDate > minDt Then minDt = calDate
                                        End If
                                    End If
                                Case Else
                                    If CDate(dRow.Item(col.ColumnName)) > minDt Then
                                        minDt = CDate(dRow.Item(col.ColumnName))
                                    End If
                            End Select
                    End Select
                End If
            End If
        Next
        Return minDt
    End Function

    Function FindMinMaxDateInTable(dTbl As DataTable, op As String, Optional StartDt As Date = Nothing) As Date
        'op needs to be either "Min" or "Max"
        Dim mDt As Date = If(Not IsNothing(StartDt), StartDt, Nothing)
        For Each dRow As DataRow In dTbl.Rows
            For Each col As DataColumn In dRow.Table.Columns
                If IsDate(dRow.Item(col.ColumnName)) Then
                    If mDt = Nothing Then
                        mDt = CDate(dRow.Item(col.ColumnName))
                    Else
                        Select Case op
                            Case "Min"
                                If CDate(dRow.Item(col.ColumnName)) < mDt Then
                                    mDt = CDate(dRow.Item(col.ColumnName))
                                End If
                            Case "Max"
                                If CDate(dRow.Item(col.ColumnName)) > mDt Then
                                    mDt = CDate(dRow.Item(col.ColumnName))
                                End If
                        End Select
                    End If
                End If
            Next
        Next
        Return mDt
    End Function

    Function SystemNameFromPath(path As String)
        'Find the System or System Feature for a child item
        Dim SysName As String = ""

        Dim strPaths As String() = path.ToString.Split(New String() {" || "}, StringSplitOptions.None)
        Dim levels As Integer = strPaths.Count - 1
        Dim drPath() As DataRow = Nothing
        Do While levels > 0
            Dim chkPath As String = ""
            For i As Integer = 0 To levels
                chkPath = (If(chkPath = "", chkPath, chkPath & " || ")) & strPaths(i)
            Next

            drPath = dsSys.Tables("dtItems").Select("Path='" & chkPath & "'")
            If drPath.Length > 0 Then
                If drPath(0)("Type").ToString = "System" Or drPath(0)("Type").ToString = "System Feature" Then
                    SysName = drPath(0)("Item")
                    Exit Do
                End If
            End If
            levels -= 1
        Loop

        Return SysName
    End Function

    Function SystemFromPath(path As String)
        'Find the System or System Feature for a child item
        Dim SysName As String = ""

        Dim strPaths As String() = path.ToString.Split(New String() {" || "}, StringSplitOptions.None)
        Dim levels As Integer = strPaths.Count - 1
        Dim drPath() As DataRow = Nothing
        Do While levels > 0
            Dim chkPath As String = ""
            For i As Integer = 0 To levels
                chkPath = (If(chkPath = "", chkPath, chkPath & " || ")) & strPaths(i)
            Next

            drPath = dsSys.Tables("dtItems").Select("Path='" & chkPath & "'")
            If drPath.Length > 0 Then
                If drPath(0)("Type").ToString = "System" Then
                    SysName = drPath(0)("Item")
                    Exit Do
                End If
            End If
            levels -= 1
        Loop

        Return SysName
    End Function

    Function PhaseNameFromPath(path As String) As String()
        'Find the System or System Feature for a child item
        Dim PhsName As String() = {"", ""}

        Dim strPaths As String() = path.ToString.Split(New String() {" || "}, StringSplitOptions.None)
        Dim levels As Integer = strPaths.Count - 1
        Dim drPath() As DataRow = Nothing
        Do While levels > 0
            Dim chkPath As String = ""
            For i As Integer = 0 To levels
                chkPath = (If(chkPath = "", chkPath, chkPath & " || ")) & strPaths(i)
            Next

            drPath = dsSys.Tables("dtItems").Select("Path='" & chkPath & "'")
            If drPath.Length > 0 Then
                If drPath(0)("Type").ToString = "Phase" Or drPath(0)("Type").ToString = "System" Or drPath(0)("Type").ToString = "System Feature" Then
                    PhsName(0) = drPath(0)("Item")
                    PhsName(1) = drPath(0)("Path")
                    Exit Do
                End If
            End If
            levels -= 1
        Loop

        Return PhsName
    End Function

    Function StatusColor(status As String)

        Dim clr As Color = Color.Black
        Select Case status
            Case "Ice Box", "On Hold"
                clr = Color.Blue
            Case "Emergency", "Off Track"
                clr = Color.Red
            Case "In Progress", "Testing", "On Track"
                clr = Color.Green
            Case "Complete"
                clr = Color.Black
            Case "At Risk"
                clr = Color.Orange
            Case "Not Started"
                clr = Color.Gray
            Case Else
                clr = Color.Black
        End Select
        Return clr
    End Function

    Public Sub MenuSelection(id As Integer)

        For Each mCtrl As Control In frm_Main.panel_Menu.Controls
            Dim mnu As ctrl_MenuItem = CType(frm_Main.panel_Menu.Controls(mCtrl.Name), ctrl_MenuItem)
            If mnu.Name = id Then 'Selected
                mnu.pan_Selected.BackColor = Color.OrangeRed
                mnu.panel_Tab.BackColor = Color.Gray
                mnu.BackColor = frm_Main.panel_Menu.BackColor 'Color.Gray
                mnu.label_TabText.Font = New Font(mnu.label_TabText.Font.Name, mnu.label_TabText.Font.Size, FontStyle.Bold)
                'Call Panel_3D(mnu.panel_Tab)
            Else 'Not Selected
                mnu.pan_Selected.BackColor = System.Drawing.Color.FromArgb(41, 41, 49)
                'mnu.pan_Selected.BackColor = ColorTranslator.FromHtml("#161734") 'frm_Main.panel_Menu.BackColor 'mnu.BackColor
                mnu.panel_Tab.BackColor = frm_Main.panel_Menu.BackColor
                mnu.BackColor = frm_Main.panel_Menu.BackColor
                mnu.label_TabText.Font = New Font(mnu.label_TabText.Font.Name, mnu.label_TabText.Font.Size, FontStyle.Regular)
                'Call Panel_3D(mnu.panel_Tab)
            End If

        Next

        frm_Main.tab_Main.SelectedIndex = id

    End Sub

    Public Sub AddToolTips()
        'Dim ttt As New ToolTip
        'Call ToolTextSetup(ttt)
        'ttt.ToolTipTitle = "Select Group"
        'ttt.SetToolTip(frm_Main.mnu_Main_Group.DropDown, "Hold Ctrl key down to select multiples")

        Dim ttMain As New ToolTip 'Report Refresh
        Call ToolTextSetup(ttMain, frm_Main.pic_MenuStructure, "Project Details, Planning Materials, and Notes", "Project Data")

        Dim ttPlan As New ToolTip 'Report Data Refresh
        Call ToolTextSetup(ttPlan, frm_Main.pic_Weekly, "Weekly/Daily Planning", "Planning")

        Dim ttPri As New ToolTip 'Select All Report Data for copying
        Call ToolTextSetup(ttPri, frm_Main.pic_MenuList, "Items listed in order by Priority", "Priority List")

        Dim ttMgmt As New ToolTip 'Report Download
        Call ToolTextSetup(ttMgmt, frm_Main.pic_Mgmt, "Management View / Project Status", "Management View")

    End Sub

    Sub ToolTextSetup(t As ToolTip, ctrl As Control, txt As String, Optional title As String = "")

        t.IsBalloon = True
        t.ToolTipIcon = ToolTipIcon.Info
        t.BackColor = Color.LightGoldenrodYellow
        t.UseAnimation = True
        t.UseFading = True
        t.AutoPopDelay = 1000000
        t.ToolTipTitle = title

        t.SetToolTip(ctrl, txt)

    End Sub

    Public Sub WriteUpdateSQL(uCmd As SqlCommand)
        'Using SqlCommands in variables so Parameters can be passed

        'Dim builder As SqlConnectionStringBuilder = _
        '    New SqlConnectionStringBuilder("Data Source=" & frm_Main.xMode & ";User ID=pmtdatabase;Password=h0nd4hm4!!")
        Dim builder As SqlConnectionStringBuilder = New SqlConnectionStringBuilder("Data Source=hma-sqld-dev_purchasing_new_model.hma.am.honda.com;Initial Catalog=Dev Purchasing New Model;User ID=pmtdatabase;Password=h0nd4hm4!!")
        builder.ConnectTimeout = 120
        Dim cnSQL As SqlClient.SqlConnection = New SqlConnection(builder.ToString())
        cnSQL.Open()

        'Dim StatusString As String = frm_Main.ts_Status.Text
        'Dim StatusColor As Color = frm_Main.ts_Status.BackColor

        Try
            'Test Found Records, therefore the UPDATE query (uString) is used
            'frm_Main.ts_Status.Text = "Updating Records"
            'frm_Main.ts_Status.BackColor = Color.LightGreen
            'frm_Main.StatusStrip2.Refresh()

            uCmd.Connection = cnSQL
            uCmd.ExecuteNonQuery()
            uCmd.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        cnSQL.Close()
        cnSQL.Dispose()

        'frm_Main.ts_Status.Text = StatusString
        'frm_Main.ts_Status.BackColor = StatusColor
        'frm_Main.StatusStrip2.Refresh()

    End Sub

    Public Sub Load_Data(SQL As String, dt As System.Data.DataTable, Optional chk As Boolean = False)

        'Dim cStatus As String = frm_Main.ts_Status.Text
        'frm_Main.ts_Status.Text = "Retrieving Data from SQL Server..."
        'frm_Main.ts_Status.BackColor = Color.LightGreen
        'frm_Main.StatusStrip1.Refresh()

        Dim builder As SqlConnectionStringBuilder
        Dim CNsql As New SqlClient.SqlConnection '("Data Source=hmasql23;Initial Catalog=" & frm_Main.xMode & ";User ID=pmtdatabase;Password=h0nd4hm4!!")
        'builder = New SqlConnectionStringBuilder("Data Source=" & frm_Main.xMode & ";User ID=pmtdatabase;Password=h0nd4hm4!!")
        builder = New SqlConnectionStringBuilder("Data Source=hma-sqld-dev_purchasing_new_model.hma.am.honda.com;Initial Catalog=Dev Purchasing New Model;User ID=pmtdatabase;Password=h0nd4hm4!!")
        builder.ConnectTimeout = 120
        CNsql = New SqlConnection(builder.ToString())

        Dim daRET As New SqlClient.SqlDataAdapter
        Dim con_try As Integer = 0
        Do
            Try
                daRET = New SqlClient.SqlDataAdapter(SQL, CNsql)
                Exit Do
            Catch ex As Exception
                con_try += 1
                Select Case con_try
                    Case Is < 10
                        'frm_Main.ts_Status.Text = "Trying to connect to SQL Server:  " & con_try
                        'frm_Main.StatusStrip1.Refresh()
                        'GoTo repeat
                    Case Else
                        MsgBox("Having trouble connecting to SQL Server.  Try again in a few minutes.")
                        Exit Sub
                End Select
            End Try
        Loop

        Try

            daRET.Fill(dt)

        Catch ex As Exception

            MsgBox("Having trouble connecting. This is usually a temporary issue.  Try again in about 30 seconds. " &
                               vbCrLf & "If the problem perists, check that you have a good network connection before contacting.")
        End Try


        CNsql.Close()
        CNsql.Dispose()
        daRET.Dispose()


        'frm_Main.ts_Status.Text = cStatus
        'frm_Main.ts_Status.BackColor = SystemColors.Control
        'frm_Main.StatusStrip1.Refresh()

    End Sub

    Public Function Open_Connection()

        'If frm_Main.ts_State.Text = "TESTING" Then 'Connect to Dev db if testing
        Open_Connection = New SqlConnection("Data Source=hma-sqld-dev_purchasing_new_model.hma.am.honda.com;Initial Catalog=Dev Purchasing New Model;User ID=pmtdatabase;Password=h0nd4hm4!!")
        'Else
        'Open_Connection = New SqlConnection("Data Source=hma-sqlp-purchasing_new_model.hma.am.honda.com;Initial Catalog=Purchasing New Model;User ID=pmtdatabase;Password=h0nd4hm4!!")
        'End If


    End Function

    Public Sub RetrieveIdentity(ByVal connectionString As String)
        Using connection As SqlConnection = New SqlConnection(connectionString)
            Dim adapter As SqlDataAdapter = New SqlDataAdapter("SELECT CategoryID, CategoryName FROM dbo.Categories", connection)
            adapter.InsertCommand = New SqlCommand("dbo.InsertCategory", connection)
            adapter.InsertCommand.CommandType = CommandType.StoredProcedure
            adapter.InsertCommand.Parameters.Add(New SqlParameter("@CategoryName", SqlDbType.NVarChar, 15, "CategoryName"))
            Dim parameter As SqlParameter = adapter.InsertCommand.Parameters.Add("@Identity", SqlDbType.Int, 0, "CategoryID")
            parameter.Direction = ParameterDirection.Output
            Dim categories As DataTable = New DataTable()
            adapter.Fill(categories)
            Dim newRow As DataRow = categories.NewRow()
            newRow("CategoryName") = "New Category"
            categories.Rows.Add(newRow)
            adapter.Update(categories)
            Console.WriteLine("List All Rows:")

            For Each row As DataRow In categories.Rows

                If True Then
                    Console.WriteLine("{0}: {1}", row(0), row(1))
                End If
            Next
        End Using
    End Sub

    Function GetUserName() As String
        'MsgBox(System.Environment.UserName & vbCrLf & My.User.Name)
        'If TypeOf My.User.CurrentPrincipal Is 
        '  Security.Principal.WindowsPrincipal Then
        '    ' The application is using Windows authentication.
        '    ' The name format is DOMAIN\USERNAME.
        '    Dim parts() As String = Split(My.User.Name, "\")
        '    Dim username As String = parts(1)
        '    Return username
        'Else
        '    ' The application is using custom authentication.
        '    Return My.User.Name
        '    'MsgBox(My.User.Name)
        'End If
        Return System.Environment.UserName
    End Function

    Function ConvertTextToRTF(ByVal RTFtext As String) As String
        Dim data_object As New DataObject
        data_object.SetData(DataFormats.Rtf, RTFtext)
        Return data_object.GetData(DataFormats.Rtf)
    End Function

    Public Function ConvertRtfToText(ByVal input As String) As String
        'On Error GoTo ErrTrap
        If input.ToString <> String.Empty Then
            If input.IndexOf("{\rtf1\ansi\deff0") <> 0 Then
                input = "{\rtf1\ansi\deff0 " & input & "\par}"
            End If
            Dim returnValue As String = String.Empty
            Using converter As New System.Windows.Forms.RichTextBox()
                converter.Rtf = input
                returnValue = converter.Text
            End Using
            Return returnValue
        Else
            Return String.Empty
        End If
        Exit Function
ErrTrap:
        'MsgBox(Err.Number & "  -  " & Err.Description)
        'Select Case Err.Number
        '    Case 5
        '        input = 
        'End Select
        Resume Next
    End Function

    Sub AddBevel(g As Graphics, obj As Object, Optional thk As Integer = 2)
        'Drop Shadow to an object
        Dim rect As Rectangle = obj.clientrectangle
        Dim p1 As Point = New Point(rect.Left, rect.Top + 2)  'top left
        Dim p2 As Point = New Point(rect.Right, rect.Top + 2)  'Top Right 
        Dim p3 As Point = New Point(rect.Left, rect.Bottom - 2)  'Bottom Left
        Dim p4 As Point = New Point(rect.Right, rect.Bottom - 2)  'Bottom Right

        Dim pen1 As Pen = New Pen(System.Drawing.Color.Gainsboro, thk)
        Dim pen2 As Pen = New Pen(System.Drawing.Color.Black, thk)
        'pen2.Alignment = PenAlignment.Center

        g.DrawLine(pen1, p1, p2) 'top
        g.DrawLine(pen1, p1, p3) 'left side
        g.DrawLine(pen2, p2, p4) 'right side
        g.DrawLine(pen2, p3, p4) 'bottom
    End Sub

    Sub Refresh_PriorityList()
        frm_Main.dgv_PriorityList.DataSource = Nothing

        Dim FilterTxt As String = "(Type IN ('System Feature', 'System', 'Enhancement') AND ShowInList='True' AND (Status NOT IN ('Complete', 'On Hold') OR (Status='Complete' AND MgmtReview='False'))) OR " &
            "(Type IN ('Task', 'Activity Group', 'Activity', 'Milestone', 'Bug Fix') AND ShowInList='True' AND Not Status ='Complete')"
        Dim dtAcc As DataTable = New DataView(frm_Main.dsSys.Tables("dtItems"), FilterTxt, "Pri ASC, Complexity DESC, Urgency DESC", DataViewRowState.CurrentRows).ToTable(True, "Pri", "System", "Item", "Parent", "Type", "Complexity", "Urgency", "EstTime", "EstTimeUM", "DueDate", "Phase", "PercentComplete", "MgmtReview")
        frm_Main.dgv_PriorityList.DataSource = dtAcc
    End Sub

    Public Function DateAddWeekDaysOnly(ByVal dDate As DateTime, ByVal iAddDays As Int32) As DateTime
        If iAddDays <> 0 Then
            Dim iIncrement As Int32 = If(iAddDays > 0, 1, -1)
            Dim iCounter As Int32

            Do
                dDate = dDate.AddDays(iIncrement)
                If dDate.DayOfWeek <> DayOfWeek.Saturday AndAlso dDate.DayOfWeek <> DayOfWeek.Sunday AndAlso
                    (Not frm_Main.dtOffDays.Select("'" & dDate.ToString("MM/dd/yyyy") & "' >= DateStart AND '" & dDate.ToString("MM/dd/yyyy") & "' <= DateEnd").Any) Then iCounter += iIncrement
            Loop Until iCounter = iAddDays
        End If

        Return dDate
    End Function

    Private Function AddBusinessDays(ByVal dtStartDate As DateTime, ByVal intVal As Integer) As DateTime

        Dim dtTemp As DateTime = dtStartDate
        dtTemp = dtStartDate.AddDays(intVal)
        Select Case dtTemp.DayOfWeek
            Case 0, 6
                dtTemp = dtTemp.AddDays(2)
        End Select
        AddBusinessDays = dtTemp

    End Function

    Function Calc_DueDate(Optional SDate As Date = Nothing, Optional Span As Double = Nothing, Optional UM As String = "") As Date
        Dim DueDt As Date = Nothing
        If IsNothing(SDate) OrElse IsNothing(Span) Then
            Return ""
        End If

        Select Case UM
            Case "day", "days", "day(s)", ""
                DueDt = DateAddWeekDaysOnly(CDate(SDate), CLng(Span)).ToShortDateString
            Case "hr", "hrs", "hours", "hour"
                DueDt = CDate(SDate).ToShortDateString
        End Select

        Return DueDt
    End Function

    Sub Center_Form_to_Main(frm As Form, Optional pnt As Boolean = False)

        ' Get all screens, then determine what screen the form is currently on
        Dim allScreens As Screen() = Screen.AllScreens
        Dim currentScreen As Screen = Screen.FromControl(frm_Main)
        Dim currentIndex As Integer = Array.IndexOf(allScreens, currentScreen)
        Dim newBounds As Rectangle = currentScreen.WorkingArea
        ' Now, we adjust to global coordinates by adding the new screen's location
        Dim finalPoint As Point = New Point
        If pnt = False Then
            ' This is the center of the new screen's working area
            Dim centerPoint As New Point((newBounds.Width - frm.Width) \ 2, (newBounds.Height - frm.Height) \ 2)
            finalPoint = New Point(centerPoint.X + newBounds.X, centerPoint.Y + newBounds.Y)
        Else
            ' This puts it at the mouse cursor point
            finalPoint = New Point(Cursor.Position.X, Cursor.Position.Y)
        End If

        ' Move the form
        frm.Location = finalPoint

    End Sub

    Public Function getQueryFromCommand(cmd As SqlCommand) As String

        Dim CommandTxt As String = cmd.CommandText

        For Each parms As SqlParameter In cmd.Parameters
            Dim val As String = Nothing
            If parms.DbType.Equals(DbType.String) Or parms.DbType.Equals(DbType.DateTime) Then
                val = "'" & Convert.ToString(parms.Value).Replace("\", "\\").Replace("'", "\'") & "'"
            End If
            If parms.DbType.Equals(DbType.Int16) Or parms.DbType.Equals(DbType.Int32) Or parms.DbType.Equals(DbType.Int64) Or parms.DbType.Equals(DbType.Decimal) Or parms.DbType.Equals(DbType.Double) Then
                val = Convert.ToString(parms.Value)
            End If
            If parms.DbType.Equals(DbType.Boolean) Then
                val = "'" & Convert.ToString(parms.Value) & "'"
            End If
            Dim paramname As String = parms.ParameterName
            CommandTxt = CommandTxt.Replace(paramname, val)
        Next

        Return CommandTxt

    End Function

    Sub Highlight_Tab()

        For Each ctrl In frm_Main.panel_Menu.Controls
            If TypeOf ctrl Is PictureBox Then
                Dim pCtrl As PictureBox = DirectCast(ctrl, PictureBox)
                pCtrl.BackColor = frm_Main.panel_Menu.BackColor
            End If
        Next
        'frm_Main.pic_MenuList.BackColor = Color.DarkGray
        '    frm_Main.pic_MenuStructure.BackColor = Color.DarkGray
        'frm_Main.pic_Weekly.BackColor = Color.DarkGray
        'frm_Main.pic_Mgmt.BackColor = Color.DarkGray

        Select Case frm_Main.tab_Main.SelectedTab.Text
            Case "List"
                frm_Main.pic_MenuList.BackColor = Color.DarkTurquoise
            Case "Main"
                frm_Main.pic_MenuStructure.BackColor = Color.DarkTurquoise
            Case "Mgmt View"
                frm_Main.pic_Mgmt.BackColor = Color.DarkTurquoise
            Case "Weekly Plan"
                frm_Main.pic_Weekly.BackColor = Color.DarkTurquoise
            Case "Off Days"
                frm_Main.pic_MenuOffDays.BackColor = Color.DarkTurquoise
            Case "Calendar"
                frm_Main.pic_MenuCal.BackColor = Color.DarkTurquoise
        End Select

    End Sub


    Public Sub DrawStringOutline(g As Graphics, ByVal theString As String, theFont As Font, theFontcolor As Color, theShadow As Boolean, x1 As Single, y1 As Single)

        With g
            .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
            .SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            Using outlinePath As New Drawing2D.GraphicsPath
                Using useFont As Font = New Font(theFont.Name, theFont.Size, FontStyle.Regular)
                    outlinePath.AddString(theString, useFont.FontFamily, FontStyle.Regular, theFont.Size, New Point(x1 + 2, y1), StringFormat.GenericTypographic)
                    If theShadow Then
                        .FillPath(Brushes.LightGray, outlinePath)
                        .TranslateTransform(-3, -3)
                    End If
                    'Dim br As Brush = New Pen(theFontcolor, 1).Brush
                    .FillPath(Brushes.White, outlinePath)
                    .DrawPath(New Pen(theFontcolor), outlinePath)
                End Using
            End Using
        End With
    End Sub

    Public Sub DrawLabelOutlined(g As Graphics, theString As String, theFont As Font, theFontcolor As Color, theShadow As Boolean, r As Rectangle)
        'Dim theString As String = theLabel.Text

        With g
            .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
            .SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

            Using f As Font = theFont
                Using pth As New Drawing2D.GraphicsPath
                    Using sf As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                        pth.AddString(theString, f.FontFamily, f.Style, ((.DpiY * f.Size) / 72), r, sf)
                    End Using
                    If theShadow Then
                        .FillPath(Brushes.LightGray, pth)
                        .TranslateTransform(-3, -3)
                    End If
                    .FillPath(Brushes.White, pth)
                    .DrawPath(New Pen(theFontcolor), pth)

                End Using
            End Using
        End With
    End Sub

    Sub GetOutlook(dd As Date)
        'Get Outlook Calendar Meetings
        Try
            Dim olApp As Outlook.Application
            olApp = CreateObject("Outlook.Application")
            Dim mpnNamespace As Outlook.NameSpace = olApp.GetNamespace("MAPI")
            Dim clfFolder As Outlook.Folder =
                mpnNamespace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar)

            'clfFolder.Items.IncludeRecurrences = False
            Dim strAppt As String = ""
            'Appointments
            For Each appItem As Outlook.AppointmentItem In clfFolder.Items
                If appItem.Start.Date = dd Then
                    Dim ds As DateTime = appItem.Start.TimeOfDay.ToString
                    Dim de As DateTime = appItem.End.TimeOfDay.ToString
                    strAppt = strAppt & appItem.Subject & "   " & ds.ToString("h:mm tt") & " - " &
                           de.ToString("h:mm tt") & vbCrLf
                    'MsgBox(appItem.Subject & "   " & ds.ToString("h:mm tt") & " - " &
                    '       de.ToString("h:mm tt"))
                End If
            Next
            'Tasks
            clfFolder = mpnNamespace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderTasks)
            For Each appTask As Outlook.TaskItem In clfFolder.Items
                If appTask.Start.Date = dd Then
                    Dim ds As DateTime = appTask.Start.TimeOfDay.ToString
                    Dim de As DateTime = appTask.End.TimeOfDay.ToString
                    strAppt = strAppt & appTask.Subject & "   " & ds.ToString("h:mm tt") & " - " &
                           de.ToString("h:mm tt") & vbCrLf
                    'MsgBox(appItem.Subject & "   " & ds.ToString("h:mm tt") & " - " &
                    '       de.ToString("h:mm tt"))
                End If
            Next

            If Not String.IsNullOrEmpty(strAppt) Then frm_Main.txt_Meetings.Text = strAppt Else frm_Main.txt_Meetings.Text = ""

            olApp = Nothing

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

End Module

Public Class ClassToBeSorted
End Class
