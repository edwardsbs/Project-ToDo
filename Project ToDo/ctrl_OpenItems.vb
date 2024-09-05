Imports System.Data.SqlClient
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Runtime.InteropServices

Public Class ctrl_OpenItems
    Public dt As DataTable
    Public DateMin As Date
    Public DateMax As Date
    Public dtItemsSrch As DataTable
    Public TotalDays As Integer
    Public tdyStorage As Date

    Private Sub ctrl_OpenItems_Load(sender As Object, e As EventArgs) Handles MyBase.Load


    End Sub

    'Public Shared Function ResizeImage(ByVal InputImage As Image) As Image
    '    Return New Bitmap(InputImage, New Size(24, 24))
    'End Function

    Sub Paint_OpenItems(Optional tdy As Date = Nothing)

        If IsNothing(tdy) Then tdy = Date.Today
        tdyStorage = tdy

        'If IsNothing(frm_Main.Loading) Then Exit Sub
        'If frm_Main.Loading = True Then Exit Sub

        pnl_Schedule.Controls.Clear()
        pnl_Schedule.Refresh()
        Dim index As Integer
        For index = pnl_Items.Controls.Count - 1 To 0 Step -1
            pnl_Items.Controls.Item(index).Dispose()
        Next
        pnl_Months.Controls.Clear()
        pnl_Months.Refresh()


        pnl_Schedule.SuspendLayout()
        'pnl_Schedule.SuspendDrawing
        '------------------------------------------------------------------------------------------

        Dim strSearch As String = "Type IN ('Activity', 'Task', 'Bug Fix', 'Allocated/Undefined') AND Not Phase = 'Complete' AND Not Status = 'Complete' AND StartDate IS NOT NULL AND DueDate IS NOT NULL "
        If IsNothing(dt) Then Exit Sub

        If dt.Select(strSearch).Any Then
            dtItemsSrch = New DataTable
            Dim dtTmp As DataTable = New DataView(frm_Main.dsSys.Tables("dtItems"), strSearch, "StartDate, ActualStart, DueDate, iIndex, System", DataViewRowState.CurrentRows).ToTable(True, "System", "Parent", "Item", "Path", "iIndex", "Notes", "Phase", "Type", "Status", "EstTime", "EstTimeUM", "StartDate", "Requester", "RequestDue", "DueDate", "ActualStart", "ActualEnd", "ActualTime", "ActualUM", "LastUpdate", "Complexity", "Urgency", "PercentComplete", "EstUseDate")

            'Now Re-order by Phase
            Dim ord = {"Bug Fix", "Task", "Activity", "Allocated/Undefined"} 'Setting Type Order
            Dim qry = From id In ord
                      Join row In dtTmp.AsEnumerable On id Equals row(7)
                      Select row
            dtTmp = New DataTable 'clear before adding the data back in order
            dtTmp = qry.CopyToDataTable

            'Now Re-order by Phase
            Dim ids = {"Emergency", "In Progress", "Published/Verifying", "Ice Box", "Testing", "Complete", "User Testing"} 'Setting Phase Order
            Dim query = From id In ids
                        Join row In dtTmp.AsEnumerable On id Equals row(6)
                        Select row

            dtItemsSrch = query.CopyToDataTable

            'Get DateMin and DateMax
            DateMin = FindMinMaxDateInTable(dtItemsSrch, "Min", Date.Today).AddDays(-8)
            DateMax = FindMinMaxDateInTable(dtItemsSrch, "Max", DateMin.AddDays(14)).AddDays(8)
            TotalDays = Math.Abs(DateMax.Subtract(DateMin).TotalDays)


            'Setup Variables
            '---------------------------------------------------------------------
            Dim itmColor As Color = Color.Black
            Dim r As Rectangle = New Rectangle(30, 30, 60, 30) 'rectangle size
            Dim d As Integer = 6 '((lHgt(1) * 0.3333) - 2) '15 'degree of corner roundness
            Dim b As Brush = New SolidBrush(ColorTranslator.FromHtml("#67E46F")) 'Brush Color
            Dim locX As Double = 0 'Left location
            Dim wid As Double = 0 'Width
            Dim offset As Integer = 12
            Dim myPen As Pen = New Pen(Color.Black, 1)
            Dim thk As Double = 0
            Dim frmt As New StringFormat()
            frmt.Alignment = StringAlignment.Near
            frmt.LineAlignment = StringAlignment.Near
            '---------------------------------------------------------------------

            Dim ctrTL As New ctrl_ItemTimeLine

            'Resize the Panels first
            ' (do this up front so the Resize and SizeChanged events don't fire with every entry)
            pnl_Details.Height = Me.Height - Panel1.Height
            pnl_Schedule.Width = pnl_Details.Width - pnl_Items.Width - 20
            pnl_Months.Width = pnl_Schedule.Width
            pnl_Items.Height = (dtItemsSrch.Rows.Count * (ctrTL.Height + 5)) + 5
            pnl_Schedule.Height = (dtItemsSrch.Rows.Count * (ctrTL.Height + 5)) + 5

            'Load Months 2nd (now that the panels are resized)
            Draw_Cal_Lines(pnl_Months)
            Draw_Cal_Lines(pnl_Schedule, tdy)

            'If ctrTL.Top + ctrTL.Height > pnl_Items.Height Then
            '    pnl_Items.Height = pnl_Items.Height + ctrl_ItemTimeLine.Height
            '    pnl_Schedule.Height = pnl_Items.Height
            'End If

            'Load Items
            Dim Top As Integer = 18
            For Each Itm As DataRow In dtItemsSrch.Rows
                'Create New Control(s)
                ctrTL = New ctrl_ItemTimeLine
                offset = ctrTL.Height / 4
                ctrTL.Name = Itm.Item("Item").ToString.Replace(" ", "_")
                ctrTL.Top = Top
                ctrTL.lbl_Item.Text = Itm.Item("Item").ToString
                ctrTL.lbl_System.Text = SystemNameFromPath(Itm.Item("Path").ToString)
                ctrTL.Path = Itm.Item("Path").ToString
                If Not String.IsNullOrEmpty(Itm.Item("Notes").ToString) Then ctrTL.pic_Notes.Visible = True
                'ctrTL.lbl_Path.Text = Itm.Item("Path").ToString

                pnl_Items.Controls.Add(ctrTL)
                '--------------------------------------

                'Draw the Phase image
                itmColor = SetColor(Itm)
                Dim imgPhase As Image = Nothing 'Drawing.Bitmap(24, 24)
                If Itm.Item("Type").ToString = "Allocated/Undefined" Then
                    imgPhase = My.Resources.un_scheduled_light
                    GoTo skip_image
                End If
                If Itm.Item("Phase").ToString = "Published/Verifying" Then
                    imgPhase = My.Resources.published
                    itmColor = Color.Green
                    GoTo skip_image
                End If
                If Itm.Item("Phase").ToString = "User Testing" Then
                    imgPhase = My.Resources.status_UserTesting
                    itmColor = Color.ForestGreen
                    GoTo skip_image
                End If
                If CDate(Itm.Item("StartDate")) < Date.Today AndAlso Not Itm.Item("Phase").ToString = "Published/Verifying" AndAlso Not Itm.Item("Phase").ToString = "Testing" Then
                    If ((IsDBNull(Itm.Item("ActualStart")) Or String.IsNullOrEmpty(Itm.Item("ActualStart").ToString))) OrElse Calc_DueDate(Itm.Item("StartDate"), Itm.Item("EstTime"), Itm.Item("EstTimeUM")) <= Date.Today Then

                        imgPhase = My.Resources.past_due_stamp
                        GoTo skip_image
                    End If
                End If

                Select Case Itm.Item("Phase").ToString
                    Case "In Progress"
                        imgPhase = My.Resources.in_progress
                    Case "Ice Box"
                        imgPhase = My.Resources.icebox
                    Case "Emergency"
                        imgPhase = My.Resources.emergency
                    Case "Testing"
                        imgPhase = My.Resources.testing_3
                        itmColor = Color.BlueViolet
                    Case "Published/Verifying"
                        imgPhase = My.Resources.published
                End Select
skip_image:
                If Not IsNothing(imgPhase) Then
                    'Dim gr_dest As Graphics = Graphics.FromImage(imgPhase)
                    ctrTL.CreateGraphics.DrawImage(imgPhase, 1, 2, 24, 24)
                End If
                '--------------------------------------


                'Draw the Timeline(s)
                thk = ctrTL.Height / 2

                'Add Plan Rectangle
                '--------------------------------------------
                'Dim pnlImg As New PictureBox
                'pnlImg.Height = ctrTL.Height
                'pnlImg.Width = pnl_Details.Width - ctrTL.Width - 20
                'pnlImg.Top = ctrTL.Top
                'Dim BMP As Bitmap = New Bitmap(pnlImg.Width, pnlImg.Height)
                'Dim G As Graphics = Graphics.FromImage(BMP)

                Dim actPen As Pen
                actPen = New Pen(itmColor, 2)
                actPen.DashPattern = New Single() {1.5F, 1.5F, 1.5F, 1.5F} '{4.0F, 2.0F, 1.0F, 3.0F} 'Defines Dash Pattern
                actPen.Alignment = Drawing2D.PenAlignment.Outset
                Dim Sdate As Date = CDate(Itm.Item("StartDate"))
                Dim Edate As Date = Calc_DueDate(Itm.Item("StartDate"), Itm.Item("EstTime"), Itm.Item("EstTimeUM")) 'CDate(Itm.Item("DueDate"))
                Dim totdays As Double = If(Math.Abs(Edate.Subtract(Sdate).TotalDays) = 0, 1, Math.Abs(Edate.Subtract(Sdate).TotalDays))
                locX = (((Math.Abs(DateMin.Subtract(Sdate).TotalDays) - 0.15) / TotalDays) * (pnl_Schedule.Width)) 'Left
                wid = (totdays / TotalDays) * pnl_Schedule.Width 'Width
                r = New Rectangle(locX, (Top + offset), wid, thk)
                pnl_Schedule.CreateGraphics.DrawRectangle(actPen, r)
                'G.DrawRectangle(actPen, New Rectangle(locX, offset, wid, thk))

                '--------------------------------------------


                'Add Actual Rectangle

                'Check for Daily inputs 1st
                If Not IsNothing(frm_Main.dtTaskJournal) Then
                    strSearch = "Task='" & Itm.Item("Item") & "' AND Path='" & Itm.Item("Path") & "' AND Architect='" & frm_Main.ss_User.Text & "'"
                    If frm_Main.dtTaskJournal.Select(strSearch).Any Then
                        Dim dtJrnl As DataTable = New DataView(frm_Main.dtTaskJournal, strSearch, "WorkDtPlan, WorkDtActual", DataViewRowState.CurrentRows).ToTable(True, "WorkDescription", "WorkDtPlan", "WorkHrsPlan", "WorkDtActual", "WorkHrsActual", "Notes")
                        For Each j As DataRow In dtJrnl.Rows
                            Sdate = If(IsDBNull(j.Item("WorkDtActual")), CDate(j.Item("WorkDtPlan")), CDate(j.Item("WorkDtActual")))
                            Edate = If(IsDBNull(j.Item("WorkDtActual")), CDate(j.Item("WorkDtPlan")), CDate(j.Item("WorkDtActual")))
                            totdays = If(Math.Abs(Edate.Subtract(Sdate).TotalDays) = 0, 0.5, Math.Abs(Edate.Subtract(Sdate).TotalDays))
                            locX = ((Math.Abs(DateMin.Subtract(Sdate).TotalDays)) / TotalDays) * pnl_Schedule.Width 'Left
                            'wid = ((Math.Abs(Edate.Subtract(Sdate).TotalDays)) / TotalDays) * pnl_Schedule.Width 'Width
                            wid = (0.75 / TotalDays) * pnl_Schedule.Width 'Width
                            r = New Rectangle(locX + 2, Top + offset + 2, wid, thk - 4)
                            pnl_Schedule.CreateGraphics.FillRectangle(New SolidBrush(If(IsDBNull(j.Item("WorkDtActual")), Color.Gray, Color.LimeGreen)), r)
                        Next
                    Else 'Journal entries have NOT been loaded.  Do the full timeline
                        If Not IsDBNull(Itm.Item("ActualStart")) Then
                            Sdate = CDate(Itm.Item("ActualStart"))
                            Edate = If(IsDBNull(Itm.Item("ActualEnd")), Date.Today, CDate(Itm.Item("ActualEnd")))
                            totdays = If(Math.Abs(Edate.Subtract(Sdate).TotalDays) = 0, 0.5, Math.Abs(Edate.Subtract(Sdate).TotalDays))
                            locX = ((Math.Abs(DateMin.Subtract(Sdate).TotalDays) + 0.5) / TotalDays) * pnl_Schedule.Width 'Left
                            'wid = ((Math.Abs(Edate.Subtract(Sdate).TotalDays)) / TotalDays) * pnl_Schedule.Width 'Width
                            wid = (totdays / TotalDays) * pnl_Schedule.Width  'Width
                            r = New Rectangle(locX + 2, Top + offset + 2, wid, thk - 4)
                            pnl_Schedule.CreateGraphics.FillRectangle(New SolidBrush(itmColor), r)
                            'G.FillRectangle(New SolidBrush(itmColor), New Rectangle(locX + 2, offset + 2, wid, thk - 4))
                        End If
                    End If
                End If
                '--------------------------------------

                'Add the % Complete label
                myPen = New Pen(itmColor, 1)
                Dim len As Integer = ((Date.Today.Subtract(Sdate).TotalDays) / TotalDays) * pnl_Schedule.Width
                r = New Rectangle(r.X + 10, (Top + offset - 1), 45, (Top + offset - 1)) '(X, Y, Length, Width)
                pnl_Schedule.CreateGraphics.DrawString(Itm.Item("PercentComplete").ToString, New Drawing.Font("Tahoma", 10, FontStyle.Regular), myPen.Brush, r, frmt)

                'DrawStringOutline(pnl_Schedule.CreateGraphics, Itm.Item("PercentComplete").ToString, New Drawing.Font("Tahoma", 10, FontStyle.Regular), itmColor, False, r.X, r.Y)

                'DrawLabelOutlined(pnl_Schedule.CreateGraphics, Itm.Item("PercentComplete").ToString, New Drawing.Font("Tahoma", 10, FontStyle.Regular), itmColor, False, r)

                'pnl_Schedule.CreateGraphics.SmoothingMode = SmoothingMode.HighQuality
                'Using gp As New GraphicsPath, f As New Font("Segoe UI", 10, FontStyle.Bold), p As New Pen(Brushes.White, 3)
                '    gp.AddString(Itm.Item("PercentComplete").ToString, f.FontFamily, f.Style, f.Size + 2, New Point(r.X, r.Y), StringFormat.GenericTypographic)
                '    pnl_Schedule.CreateGraphics.DrawPath(p, gp)
                '    pnl_Schedule.CreateGraphics.FillPath(myPen.Brush, gp)
                'End Using

                'G.DrawString(Itm.Item("PercentComplete").ToString, New Drawing.Font("Tahoma", 10, FontStyle.Regular), myPen.Brush, New Rectangle(r.X + len + 5, offset - 2, 40, offset - 1), frmt)
                '--------------------------------------

                'Add First Use Date (Event Date) to Initial Phase Row (assuming most space available)
                '-------------------------------------------------------------
                'If dtSchedule.Rows.IndexOf(Itm) = 0 Then 'Initial Phase Row
                If Not IsDBNull(Itm.Item("EstUseDate")) Then
                    If IsDate(Itm.Item("EstUseDate")) Then
                        locX = (((Math.Abs(DateMin.Subtract(CDate(Itm.Item("EstUseDate"))).TotalDays) + 1) / TotalDays) * (pnl_Schedule.Width)) 'Left
                        r = New Rectangle(locX, (Top + 6), 11, 11)
                        'Add the Indicator Image
                        pnl_Schedule.CreateGraphics.DrawImage(My.Resources.start_point, r)
                        'G.DrawImage(My.Resources.start_point, New Rectangle(locX, 6, 11, 11))
                        'Add the Indicator label
                        Dim lblX As Integer = r.X + 12
                        Dim lblWid As Integer = 100
                        'determine if the label has to go on the Left or Right [based on amount of room]
                        If (pnl_Schedule.Width - (r.X + 12) - 8) < 110 Then
                            lblX = r.X - 103
                            frmt.Alignment = StringAlignment.Far
                        End If
                        r = New Rectangle(lblX, (Top + 2), lblWid, (Top + 3)) '(X, Y, Lenght, Width)
                        pnl_Schedule.CreateGraphics.DrawString(Itm.Item("RequestDue").ToString, New Drawing.Font("Tahoma", 9, FontStyle.Regular), myPen.Brush, r, frmt)
                        'G.DrawString(Itm.Item("RequestDue").ToString, New Drawing.Font("Tahoma", 9, FontStyle.Regular), myPen.Brush, New Rectangle(lblX, 2, lblWid, 2), frmt)
                    End If
                End If

                'pnl_Schedule.Controls.Add(pnlImg)
                'pnlImg.Image = BMP


                'End If
                '-------------------------------------------------------------

                'Finalize
                Top = Top + ctrTL.Height + 4
                pnl_Items.CreateGraphics.DrawLine(myPen, 0, Top - 2, pnl_Items.Width, Top - 2)
                pnl_Schedule.CreateGraphics.DrawLine(myPen, 0, Top - 2, pnl_Schedule.Width, Top - 2)

            Next

        End If

        'Draw_Cal_Lines(pnl_Schedule, tdy)

        pnl_Schedule.ResumeLayout()
        'pnl_Schedule.ResumeDrawing

    End Sub

    Public Sub Draw_Cal_Lines(pnl As Panel, Optional tdy As Date = Nothing)

        If IsNothing(tdy) Then tdy = Date.Today

        'Now Loop Thru Remaining Months to Draw Dividing Lines and Write Month Name
        Dim locx As Double
        Dim t As Double
        Dim myPen As New Pen(Color.SteelBlue, 2)

        Dim CalFont As Font = New Drawing.Font("Verdana", 11, FontStyle.Bold)
        Dim format As New StringFormat()
        format.Alignment = StringAlignment.Center
        format.LineAlignment = StringAlignment.Center
        pnl.SuspendLayout()
        'pnl.SuspendDrawing

        'Setup Drawing Graphics for drawing lines

        Dim pnlImg As New PictureBox
        '----------------------------------------------------


        '----------------------------------------------------

        'Draw 1st Month label first (if enough space - 15th or greater).  Otherwise it gets skipped because it is looping to look for the 1st day in the month
        '----------------------------------------------------
        '----------------------------------------------------
        Dim dayCnt As Integer = Date.DaysInMonth(DateMin.Year, DateMin.Month) - DateMin.Day
        Dim drwStrWid As Integer = (dayCnt / TotalDays) * pnl.Width 'Draw String Width - calculate how much space the month can take up
        Dim r1 As New Rectangle(1, pnl.Top + 3, drwStrWid, pnl.Height - 4)
        If pnl.Name = "pnl_Months" AndAlso DateMin.Day > 1 Then
            If drwStrWid >= 45 Then 'Make sure the width is at least 45
                'Plenty of room, write the month label normally
                CalFont = New Drawing.Font("Verdana", 11, FontStyle.Bold)
                format.Alignment = StringAlignment.Center
                pnl.CreateGraphics.DrawString(MonthName(DateMin.Month, True) & " " & DateMin.Year, CalFont,
                                                            New SolidBrush(Color.Blue), r1, format)
            Else
                If drwStrWid > 30 Then 'Only write if there is enough room.  otherwise it will overlap the next month.
                    'Not much room, write a smaller month label
                    r1 = New Rectangle(1, 6, drwStrWid, pnl.Height - 4)
                    CalFont = New Drawing.Font("Verdana", 10, FontStyle.Regular)
                    format.Alignment = StringAlignment.Near
                    pnl.CreateGraphics.DrawString(MonthName(DateMin.Month, True) & " " & DateMin.Year, CalFont, New SolidBrush(Color.Blue), r1, format)
                End If
            End If
        End If
        '----------------------------------------------------
        '----------------------------------------------------

        'Add OffDays DateRange(s) [do this before adding labels so the labels and lines can be drawn on top]
        '-------------------------------------------------------------
        Dim itmColor(4) As Color

        If Not pnl.Name.ToString = "pnl_Months" Then
            Dim DtCheck As String = "(DateStart >= '" & DateMin.ToString("MM/dd/yyyy") & "' AND DateEnd <='" & DateMax.ToString("MM/dd/yyyy") & "')  "
                        If frm_Main.dtOffDays.Select(DtCheck).Any Then
                'Draw Vertical Line for Start & Stop
                Dim myBrush As SolidBrush = New SolidBrush(Color.Silver)
                For Each offday As DataRow In frm_Main.dtOffDays.Select(DtCheck).CopyToDataTable.Rows
                    If offday.Item("Item").ToString = "PTO" Then myBrush = New SolidBrush(Color.PowderBlue) Else myBrush = New SolidBrush(Color.Gray)

                    't = CDate(offday.Item("DateStart")).Subtract(DateMin).TotalDays   't + System.DateTime.DaysInMonth(Day.Year, Day.Month)
                    locx = ((Math.Abs(DateMin.Subtract(CDate(offday.Item("DateStart"))).TotalDays)) / TotalDays) * pnl_Schedule.Width ' ((t + 1) / TotalDays) * pnl.Width

                    'Dim locXa As Double = ((Math.Abs(DateMin.Subtract(offday.Item("DateStart")).TotalDays)) / TotalDays) * pnl_Schedule.Width 'Left
                    Dim widX As Double = (1 / TotalDays) * pnl_Schedule.Width '(Math.Abs(offday.Item("DateEnd").Subtract(offday.Item("DateStart")).TotalDays + 1) / TotalDays) * pnl_Schedule.Width 'Width
                    Dim r As New Rectangle(locx, 0, widX - 1, pnl.Height) 'added -1 to widX as long as the light daily divider line is present
                    pnl.CreateGraphics.FillRectangle(myBrush, r)
                    'Call FillRoundedRectangle(r, 2, New SolidBrush(Color.Gray), panel_Schedule)

                    'pnl.CreateGraphics.DrawRectangle(myPen, r)
                Next
            End If
        End If
        '-------------------------------------------------------------

        'Draw Vertical Lines
        For Each Day As DateTime In Enumerable.Range(0, (DateMax - DateMin).Days) _
                            .Select(Function(i) DateMin.AddDays(i))

            '[WEEK]
            '----------------------------------------------------
            Dim dotPen As New Pen(Color.LightGray, 1.5)
            dotPen.DashCap = Drawing2D.DashCap.Triangle
            dotPen.DashPattern = New Single() {4.0F, 2.0F, 1.0F, 3.0F} 'Defines Dash Pattern

            Dim chkString As String() = {"pnl_Schedule", "panel_Schedule", "pnl_Overall"} 'do not add day lines for pnl_Months

            If chkString.Contains(pnl.Name) Then
                Select Case Day.DayOfWeek
            'If Day.DayOfWeek = 1 Then 'Monday
                    Case DayOfWeek.Monday 'Monday
                        myPen = New Pen(Color.Black, 1)
                        t = Day.Subtract(DateMin).TotalDays  't + System.DateTime.DaysInMonth(Day.Year, Day.Month)
                        locx = (t / TotalDays) * pnl.Width
                        pnl.CreateGraphics.DrawLine(myPen, CInt(locx), 0, CInt(locx), pnl.Height)

                        'put day indicator in small label at the top
                        CalFont = New Drawing.Font("Verdana", 7, FontStyle.Regular)
                        r1 = New Rectangle(CInt(locx), 0.5, 20, 11)
                        format.Alignment = StringAlignment.Near
                        pnl.CreateGraphics.DrawString(Day.Day, CalFont, New SolidBrush(Color.Black), r1, format)
                        'Dim BMP As Bitmap = New Bitmap(r1.Width, r1.Height)
                        'Dim G As Graphics = Graphics.FromImage(BMP)
                        'G.DrawString(Day.Day, CalFont, New SolidBrush(Color.Black), New Rectangle(0, 0, 20, 11), format)
                        'pnlImg.Top = r1.Y
                        'pnlImg.Left = r1.X
                        'pnlImg.Width = r1.Width
                        'pnlImg.Height = r1.Height
                        'pnlImg.Image = BMP
                        'pnl.Controls.Add(pnlImg)

                    Case DayOfWeek.Saturday ', DayOfWeek.Sunday 'Saturday & Sunday
                        Dim locXa As Double = ((Math.Abs(DateMin.Subtract(Day).TotalDays)) / TotalDays) * pnl_Schedule.Width 'Left
                        Dim widX As Double = (2 / TotalDays) * pnl_Schedule.Width 'Width
                        Dim r As New Rectangle(locXa, 0, widX, pnl.Height)
                        pnl.CreateGraphics.FillRectangle(New SolidBrush(Color.Gainsboro), r)
                        'Dim BMP As Bitmap = New Bitmap(r.Width, r.Height)
                        'Dim G As Graphics = Graphics.FromImage(BMP)
                        'G.FillRectangle(New SolidBrush(Color.Gainsboro), New Rectangle(0, 0, r.Width, r.Height))
                        'pnlImg.Top = r.Y
                        'pnlImg.Left = r.X
                        'pnlImg.Width = r.Width
                        'pnlImg.Height = r.Height
                        'pnlImg.Image = BMP
                        'pnl.Controls.Add(pnlImg)

                    Case Else 'Testing only, comment out
                        'Dim locXa As Double = ((Math.Abs(DateMin.Subtract(Day).TotalDays)) / TotalDays) * pnl_Schedule.Width 'Left
                        'Dim widX As Double = (1 / TotalDays) * pnl_Schedule.Width 'Width
                        'Dim r As New Rectangle(locXa, 0, widX, pnl.Height)
                        'pnl.CreateGraphics.FillRectangle(New SolidBrush(Color.Gainsboro), r)
                        myPen = New Pen(Color.LightCyan, 1)
                        locx = (Day.Subtract(DateMin).TotalDays / TotalDays) * pnl.Width
                        pnl.CreateGraphics.DrawLine(myPen, CInt(locx), 0, CInt(locx), pnl.Height)

                End Select
            End If
            dotPen.Dispose()
            'End If
            '----------------------------------------------------

            '[MONTH]
            '----------------------------------------------------
            If Day.Day = 1 Then
                If Day.Month = 1 Then
                    myPen = New Pen(Color.Black, 4)
                Else
                    myPen = New Pen(Color.SteelBlue, 3)
                End If
                't = Day.Subtract(DateMin).TotalDays + 1 't + System.DateTime.DaysInMonth(Day.Year, Day.Month)
                'locx = (t / TotalDays) * pnl.Width
                locx = (Day.Subtract(DateMin).TotalDays / TotalDays) * pnl.Width

                pnl.CreateGraphics.DrawLine(myPen, CInt(locx), 0, CInt(locx), pnl.Height)

                'Dim BMP As Bitmap = New Bitmap(3, pnl.Height)
                'Dim G As Graphics = Graphics.FromImage(BMP)
                'G.DrawLine(myPen, 0, 0, myPen.Width, pnl.Height)
                'pnlImg.Top = 0
                'pnlImg.Left = CInt(locx)
                'pnlImg.Width = myPen.Width
                'pnlImg.Height = pnl.Height
                'pnlImg.Image = BMP
                'pnl.Controls.Add(pnlImg)

                'If pnl.Name = "panel_Schedule" OrElse pnl.Name = "pnl_Months" Then
                If pnl.Name = "pnl_Months" Then
                    drwStrWid = 45 'Draw String Width - calculate how much space the month can take up
                    dayCnt = If(Day.Month = DateMax.Month, DateMax.Day, Date.DaysInMonth(Day.Year, Day.Month))
                    drwStrWid = (dayCnt / TotalDays) * pnl.Width
                    If drwStrWid >= 45 Then 'Make sure the width is at least 45
                        'Plenty of room, write the month label normally
                        CalFont = New Drawing.Font("Verdana", 11, FontStyle.Bold)
                        format.Alignment = StringAlignment.Center
                        r1 = New Rectangle(locx, pnl.Top + 4, drwStrWid, pnl.Height - 4)
                        pnl.CreateGraphics.DrawString(MonthName(Day.Month, True) & " " & Day.Year, CalFont,
                                                            New SolidBrush(Color.Blue), r1, format)
                    Else
                        If drwStrWid >= 30 Then 'Only write if there is enough room.  otherwise it will overlap the next month.
                            'Not much room, write a smaller month label
                            r1 = New Rectangle(locx, pnl.Top + 4, drwStrWid, pnl.Height - 4)
                            CalFont = New Drawing.Font("Verdana", 9, FontStyle.Regular)
                            format.Alignment = StringAlignment.Near
                            pnl_Months.CreateGraphics.DrawString(MonthName(Day.Month, True) & " " & Day.Year, CalFont, New SolidBrush(Color.Blue), r1, format)
                        End If
                    End If
                End If
            End If
            '----------------------------------------------------

            'Add a red line for today
            If DateDiff(DateInterval.Day, CDate(Day.ToShortDateString), CDate(tdy.ToShortDateString)) = 0 Then
                myPen = New Pen(Color.Red, 3)
                t = Day.Subtract(DateMin).TotalDays   't + System.DateTime.DaysInMonth(Day.Year, Day.Month)
                'Dim t As Integer = Date.Today.Subtract(Sdate).TotalDays
                locx = (t / TotalDays) * pnl.Width
                pnl.CreateGraphics.DrawLine(myPen, CSng(locx), 0, CSng(locx), pnl.Height)
                'Dim BMP As Bitmap = New Bitmap(r1.Width, r1.Height)
                'Dim G As Graphics = Graphics.FromImage(BMP)
                'G.DrawLine(myPen, 0, 0, 3, pnl.Height)
                'pnlImg.Top = 0
                'pnlImg.Left = CInt(locx)
                'pnlImg.Width = 3
                'pnlImg.Height = pnl.Height
                'pnlImg.Image = BMP
                'pnl.Controls.Add(pnlImg)
            End If

        Next Day 'mnth



        pnl.ResumeLayout()
        'pnl.ResumeDrawing

    End Sub

    Function SetColor(dRow As DataRow) As Color

        If IsNothing(dRow) Then Return Color.Black

        'Check if 'Allocated/Undefined'.  These items will always be gray
        If dRow.Table.Columns.Contains("Type") Then
            Select Case dRow.Item("Type").ToString
                Case "Allocated/Undefined"
                    Return Color.Gray
            End Select
        End If

        'Check if Late 1st
        If dRow.Table.Columns.Contains("EstTime") Then
            If IsDate(dRow.Item("StartDate")) Then
                If CDate(Calc_DueDate(dRow.Item("StartDate"), dRow.Item("EstTime"), dRow.Item("EstTimeUM"))) <= Date.Today Then
                    Return Color.Red
                End If
            End If
        End If

        'Check if Not started after Start Date
        If dRow.Table.Columns.Contains("StartDate") Then
            If IsDate(dRow.Item("StartDate")) Then
                If CDate(dRow.Item("StartDate")) <= Today AndAlso Not IsDate(dRow.Item("ActualStart")) Then
                    Return Color.Red
                End If
            End If
        End If

        'Check Phase
        If dRow.Table.Columns.Contains("Phase") Then
            Select Case dRow.Item("Phase").ToString
                Case "In Progress", "Testing"
                    Return Color.Green
                Case "Emergency"
                    Return Color.Red
                Case "Ice Box"
                    Return Color.Blue
                Case Else
                    Return Color.Black
            End Select
        End If

    End Function

    Private Sub ctrl_OpenItems_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged

    End Sub


    Private Sub pnl_Details_Scroll(sender As Object, e As ScrollEventArgs) Handles pnl_Details.Scroll
        Me.SuspendLayout()
        'Me.SuspendDrawing
        Paint_OpenItems()
        'Me.ResumeDrawing
        Me.ResumeLayout()
    End Sub

    Private Sub ctrl_OpenItems_Layout(sender As Object, e As LayoutEventArgs) Handles Me.Layout
        'pnl_Details.Height = Me.Height - Panel1.Height
        'pnl_Schedule.Width = pnl_Details.Width - pnl_Items.Width - 20
        'pnl_Months.Width = pnl_Schedule.Width
        'Paint_OpenItems()
    End Sub

    Private Sub pic_Refresh_Click(sender As Object, e As EventArgs) Handles pic_Refresh.Click
        Paint_OpenItems(tdyStorage)
    End Sub



    'Private Sub pnl_Schedule_Paint(sender As Object, e As PaintEventArgs) Handles pnl_Schedule.Paint

    'End Sub
    'Private Sub pnl_Schedule_Resize(sender As Object, e As EventArgs) Handles pnl_Schedule.Resize
    '    'Call Paint_OpenItems()
    'End Sub
    'Private Sub pnl_Schedule_SizeChanged(sender As Object, e As EventArgs) Handles pnl_Schedule.SizeChanged
    '    'Call Paint_OpenItems()
    'End Sub

    'Private Sub pnl_Schedule_Layout(sender As Object, e As LayoutEventArgs) Handles pnl_Schedule.Layout
    '    'If frm_Main.Loading = True Then Exit Sub
    '    'Call Paint_OpenItems(frm_Main.cctrl_Cal.DateRef)
    'End Sub


End Class
