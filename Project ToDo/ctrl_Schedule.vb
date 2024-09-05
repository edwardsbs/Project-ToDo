Public Class ctrl_Schedule
    'Public lHgt(4) As Integer 'used to keep up with the top of each Section
    Public DateMin As Date = #11/7/1975#
    Public DateMax As Date = #7/24/2003#
    Public TotalDays As Integer = Math.Abs(DateMax.Subtract(DateMin).TotalDays)
    Public System As String = Nothing
    Public Path As String = Nothing
    Public Item As String = Nothing
    Public Type(3) As String
    Public dtSchedule As New DataTable
    Public dtHeader As New DataTable

    Private Sub ctrl_Schedule_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Sub Paint_Schedule()

        If IsNothing(System) Then Exit Sub
        If IsNothing(Path) Then Exit Sub
        If IsNothing(Item) Then Exit Sub

        panel_Schedule.Controls.Clear()
        panel_Schedule.Refresh()
        Dim index As Integer
        For index = pnl_Schedule.Controls.Count - 1 To 0 Step -1
            pnl_Schedule.Controls.Item(index).Dispose()
        Next
        'pnl_Schedule.Controls.Clear()
        'pnl_Schedule.Refresh()
        pnl_Months.Controls.Clear()
        pnl_Months.Refresh()

        'If Loading = True Then Exit Sub

        panel_Schedule.SuspendLayout()

        'Get Header Row 1st
        Dim FilterTxt As String = ""
        Select Case Type(0)
            Case "System", "System Feature", "Enhancement"
                FilterTxt = "System='" & System & "' AND [Path]='" & Path & "' AND Item='" & Item & "'"
            Case "Phase", "Activity Group"
                FilterTxt = "System='" & System & "' AND [Path]='" & Path & "' AND Item='" & Item & "'"
            Case "Activity", "Task"
                FilterTxt = "System='" & System & "' AND [Path]='" & Path & "'"
        End Select

        dtHeader = New DataView(frm_Main.dsSys.Tables("dtItems"), FilterTxt, "Phase, iIndex, Item ASC", DataViewRowState.CurrentRows).ToTable(True, "Item", "Notes", "Scope", "Phase", "Status", "EstTime", "EstTimeUM", "StartDate", "DueDate", "ActualStart", "ActualEnd", "Requester", "RequestDue", "EstUseDate", "Urgency", "Complexity", "Pri", "Parent", "PercentComplete")

        'Get Child Items
        Select Case Type(0)
            Case "System", "System Feature", "Enhancement"
                FilterTxt = "System='" & System & "' AND [Parent]='" & Path & "' AND Type='Phase'"
            Case "Phase"
                FilterTxt = "System='" & System & "' AND [Parent]='" & Type(2) & "' AND Type IN ('Activity', 'Task')"
            Case "Activity", "Task"
                FilterTxt = "System='" & System & "' AND [Path]='" & Type(2) & "'"
        End Select

        dtSchedule = New DataView(frm_Main.dsSys.Tables("dtItems"), FilterTxt, "iIndex, Item ASC", DataViewRowState.CurrentRows).ToTable(True, "Item", "Notes", "Scope", "Phase", "Status", "EstTime", "EstTimeUM", "StartDate", "DueDate", "ActualStart", "ActualEnd", "Requester", "RequestDue", "Urgency", "Complexity", "Pri", "Parent", "PercentComplete")

        'Fill Header Info:
        '---------------------------------------------------------------
        'Labels
        lbl_Task.Text = If(dtHeader.Rows.Count = 0, "No System", dtHeader.Rows(0).Item("Item").ToString)
        lbl_Priority.Text = If(dtHeader.Rows.Count = 0, "-", dtHeader.Rows(0).Item("Pri").ToString)

        Try

            'Get MIN date from earliest date possible

            'FindMinMaxDateInRow function needs to be either "Min" or "Max"
            DateMin = FindMinMaxDateInRow(dtHeader.Rows(0), "Min")
            'Check if dtSchedule has an Earlier Date
            If dtSchedule.Rows.Count > 0 Then DateMin = FindMinMaxDateInRow(dtSchedule.Rows(0), "Min", DateMin)

            If IsNothing(DateMin) Then DateMin = Date.Today.AddDays(-8) Else DateMin = DateMin.AddDays(-8) 'back up 8 days for spacing

            'FindMinMaxDateInRow function needs to be either "Min" or "Max"
            DateMax = FindMinMaxDateInRow(dtHeader.Rows(0), "Max")
            'Check if dtSchedule has an Earlier Date
            If dtSchedule.Rows.Count > 0 Then DateMax = FindMinMaxDateInRow(dtSchedule.Rows(0), "Max", DateMax)
            If IsNothing(DateMax) Then DateMax = Date.Today.AddDays(+8) Else DateMax = DateMax.AddDays(+8) 'add 8 days for spacing

            'Calculate TotalDays
            TotalDays = Math.Abs(DateMax.Subtract(DateMin).TotalDays)

            'Draw Month Lines and Labels 1st
            '-------------------------------------------------------------
            panel_Schedule.SuspendLayout()

            Call Draw_Cal_Lines(panel_Schedule)
            Call Draw_Cal_Lines(pnl_Months)

            panel_Schedule.ResumeLayout()
            '-------------------------------------------------------------

            'Draw Phases
            Call Draw_Build_Ranges()

        Catch ex As Exception

        End Try

        panel_Schedule.ResumeLayout()

    End Sub

    Sub Draw_Build_Ranges()

        Try

            Dim TotalDays As Integer = Math.Abs(DateMax.Subtract(CDate(DateMin)).TotalDays)

            'Dim g As System.Drawing.Graphics
            Dim r As Rectangle = New Rectangle(30, 30, 60, 30) 'rectangle size
            Dim d As Integer = 6 '((lHgt(1) * 0.3333) - 2) '15 'degree of corner roundness
            Dim b As Brush = New SolidBrush(ColorTranslator.FromHtml("#67E46F")) 'Brush Color
            Dim locXa As Double 'Left location
            Dim widX As Double 'Width
            Dim thk As Double = 24 '0.36 * 0.333333 '(lHgt(1) * 0.3333) 'height

            Dim RangeColor(8) As Color 'String
            RangeColor(0) = Color.Orange '"#FF9840" 'Blue
            RangeColor(1) = Color.Green '"#67E46F" 'Green
            RangeColor(2) = Color.Blue '"#5DC8CD" 'Orange
            RangeColor(3) = Color.MediumOrchid '"#FF7673" 'Pink
            'RangeColor(4) = "#218555" 'Forest Green
            'RangeColor(5) = "#D30068"
            RangeColor(6) = Color.Black
            RangeColor(7) = Color.Silver

            pnl_Schedule.Controls.Clear()
            pnl_Schedule.Refresh()

            '------LOOP THRU EVENTS IN StartUp Table AND DRAW RANGES
            Dim Sdate As DateTime
            Dim Edate As DateTime
            Dim top As Integer = 8
            Dim offset As Integer = 6 'offset to center drawn ranges with control
            Dim myPen As Pen = New Pen(Color.Black, 1)

            Dim CalFont As Font = New Drawing.Font("Verdana", 10, FontStyle.Bold)
            'Dim format As New StringFormat()
            Dim br As New SolidBrush(Color.Black)
            Dim frmt As New StringFormat()
            frmt.Alignment = StringAlignment.Near
            frmt.LineAlignment = StringAlignment.Near

            '[DRAW FULL RANGE 1ST]
            '---------------------------------------------------
            Try
                Dim dtRow As DataRow = New DataView(dtHeader, "", "", DataViewRowState.CurrentRows).ToTable(True, "ActualStart", "StartDate", "EstTime", "EstTimeUM", "DueDate", "ActualEnd").Rows(0)
                Sdate = FindMinMaxDateInRow(dtRow, "Min")
                Edate = FindMinMaxDateInRow(dtRow, "Max")
                'Sdate = If(IsDBNull(dtHeader.Rows(0).Item("ActualStart")), CDate(dtHeader.Rows(0).Item("StartDate")), If(CDate(dtHeader.Rows(0).Item("ActualStart")) > CDate(dtHeader.Rows(0).Item("StartDate")), CDate(dtHeader.Rows(0).Item("StartDate")), CDate(dtHeader.Rows(0).Item("ActualStart"))))
                'Edate = If(IsDBNull(dtHeader.Rows(0).Item("ActualEnd")), CDate(dtHeader.Rows(0).Item("DueDate")), If(CDate(dtHeader.Rows(0).Item("ActualEnd")) > CDate(dtHeader.Rows(0).Item("DueDate")), CDate(dtHeader.Rows(0).Item("ActualEnd")), CDate(dtHeader.Rows(0).Item("DueDate"))))
                locXa = ((Math.Abs(DateMin.Subtract(Sdate).TotalDays) + 1) / TotalDays) * panel_Schedule.Width 'Left
                widX = ((Math.Abs(Edate.Subtract(Sdate).TotalDays) + 1) / TotalDays) * panel_Schedule.Width 'Width
                'r = New Rectangle(locXa, 5, widX, thk) 'New Rectangle(locXa, lHgt(1) + 5, widX, thk)
                r = New Rectangle(locXa, (top + offset + 2), widX, thk)
                b = New SolidBrush(Color.Black)
                Call FillRoundedRectangle(r, 3, b, panel_Schedule)
                Call Add_Label(dtHeader.Rows(0).Item("Item").ToString, r, panel_Schedule)

                'Add the labels to the controls side as a header
                r = New Rectangle(3, (top + offset + 2), widX, thk) '(X, Y, Lenght, Width)
                Dim format As New StringFormat()
                format.Alignment = StringAlignment.Near
                format.LineAlignment = StringAlignment.Center
                'pnl_Schedule.CreateGraphics.DrawString(dtHeader.Rows(0).Item("Item").ToString, New Drawing.Font("Verdana", 12, FontStyle.Bold), New SolidBrush(Color.Black), r, format)

                'Add Phase/Status labels 
                r = New Rectangle(pnl_Schedule.Width - 75, top - 2, widX, thk) '(X, Y, Lenght, Width)
                pnl_Schedule.CreateGraphics.DrawString(dtHeader.Rows(0).Item("Phase").ToString, New Drawing.Font("Tahoma", 8, FontStyle.Regular), New SolidBrush(StatusColor(dtHeader.Rows(0).Item("Phase").ToString)), r, format)

                r = New Rectangle(pnl_Schedule.Width - 75, (top + 12), widX, thk) '(X, Y, Lenght, Width)
                pnl_Schedule.CreateGraphics.DrawString(dtHeader.Rows(0).Item("Status").ToString, New Drawing.Font("Tahoma", 8, FontStyle.Regular), New SolidBrush(StatusColor(dtHeader.Rows(0).Item("Status").ToString)), r, format)

                'Add Urgency & Complexity (Scale: 1-5)
                r = New Rectangle(20, (top + 2), widX, thk) '(X, Y, Lenght, Width)
                pnl_Schedule.CreateGraphics.DrawString("Urgency", New Drawing.Font("Verdana", 8, FontStyle.Regular), New SolidBrush(Color.Black), r, format)
                Dim imgUrg As Image = If(IsDBNull(dtHeader.Rows(0).Item("Urgency")), Nothing, If(IsBetween(dtHeader.Rows(0).Item("Urgency"), 0, 2), My.Resources.gauge_low, If(dtHeader.Rows(0).Item("Urgency") = 3, My.Resources.gauge_medium, My.Resources.gauge_high)))
                If Not IsNothing(imgUrg) Then pnl_Schedule.CreateGraphics.DrawImage(imgUrg, New Rectangle(r.X + 53, r.Y - 7, 40, 40))

                r = New Rectangle(160, (top + 2), widX, thk) '(X, Y, Lenght, Width)
                pnl_Schedule.CreateGraphics.DrawString("Complexity", New Drawing.Font("Verdana", 8, FontStyle.Regular), New SolidBrush(Color.Black), r, format)
                Dim imgComp As Image = If(IsDBNull(dtHeader.Rows(0).Item("Complexity")), Nothing, If(IsBetween(dtHeader.Rows(0).Item("Complexity"), 1, 2), My.Resources.gauge_low, If(dtHeader.Rows(0).Item("Complexity") = 3, My.Resources.gauge_medium, My.Resources.gauge_high)))
                If Not IsNothing(imgComp) Then pnl_Schedule.CreateGraphics.DrawImage(imgComp, New Rectangle(r.X + 70, r.Y - 7, 40, 40))

                '---------------------------------------------------------------
                'Draw A Separator Line
                '-----------------------------------------------------
                top = 48

                myPen = New Pen(RangeColor(6), 1) 'New Pen(RangeColor(dtSchedule.Rows.IndexOf(itm)), 2)
                panel_Schedule.CreateGraphics.DrawLine(myPen, 0, top - 1, panel_Schedule.Width, top - 1)
                pnl_Schedule.CreateGraphics.DrawLine(myPen, 0, top - 1, panel_Schedule.Width, top - 1)

            Catch ex As Exception

            End Try
            '---------------------------------------------------


            '[NOW DRAW THE PHASES]
            For Each itm As DataRow In dtSchedule.Rows

                'Draw A Separator Line
                '-----------------------------------------------------
                myPen = New Pen(RangeColor(6), 1) 'New Pen(RangeColor(dtSchedule.Rows.IndexOf(itm)), 2)
                'If Type(0) = "Phase" AndAlso Not Type(1) = itm.Item("Item").ToString Then myPen = New Pen(RangeColor(7), 1)
                If Type(0) = "Phase" AndAlso Type(1) = itm.Item("Item").ToString Then
                    top = top + 2 'add an extra buffer for drawing thicker dividing line
                    myPen = New Pen(RangeColor(dtSchedule.Rows.IndexOf(itm)), 2) 'thicker dividing line & with Phase color
                End If
                panel_Schedule.CreateGraphics.DrawLine(myPen, 0, top - 1, panel_Schedule.Width, top - 1)
                pnl_Schedule.CreateGraphics.DrawLine(myPen, 0, top - 1, panel_Schedule.Width, top - 1)
                '-----------------------------------------------------

                '--------[START ctr_ProjTimeLine]---------------------
                Dim ctrTL As New ctr_ProjTimeLine
                ctrTL.Name = itm.Item("Item").ToString.Replace(" Phase", "")
                ctrTL.Top = top
                ctrTL.lbl_Item.Text = itm.Item("Item").ToString.Replace(" Phase", "")
                ctrTL.lbl_PlanStart.Text = CDate(itm.Item("StartDate")).ToShortDateString
                ctrTL.lbl_PlanEnd.Text = Calc_DueDate(itm.Item("StartDate"), itm.Item("EstTime"), itm.Item("EstTimeUM")) 'CDate(itm.Item("DueDate")).ToShortDateString
                ctrTL.lbl_ActualStart.Text = If(IsDBNull(itm.Item("ActualStart")), "", CDate(itm.Item("ActualStart")).ToShortDateString)
                ctrTL.lbl_ActualEnd.Text = If(IsDBNull(itm.Item("ActualEnd")), "", CDate(itm.Item("ActualEnd")).ToShortDateString)
                ctrTL.pic_Status.Image = ctrTL.imgLst_Status.Images(GetImageID(itm.Item("Status").ToString))
                If Type(0) = "Phase" Then 'Color the labels if Phase is selected
                    If Type(1) = itm.Item("Item").ToString Then ctrTL.lbl_Item.ForeColor = RangeColor(dtSchedule.Rows.IndexOf(itm)) Else _
                        ctrTL.lbl_Item.ForeColor = RangeColor(6)
                End If
                '-------------------------------------------------------

                pnl_Schedule.Controls.Add(ctrTL)
                'top = ctrTL.Top + ctrTL.Height
                '--------[END ctr_ProjTimeLine]-----------------------
                '-----------------------------------------------------

                'Draw the Phase Range
                If Not IsDBNull(itm.Item("StartDate")) And
                    Not IsDBNull(itm.Item("EstTime")) Then

                    'Add Plan Rectangle
                    '--------------------------------------------
                    Dim actPen As Pen
                    actPen = New Pen(RangeColor(dtSchedule.Rows.IndexOf(itm)), 2)
                    'If Type(0) = "Phase" AndAlso Not Type(1) = itm.Item("Item").ToString Then actPen = New Pen(RangeColor(6), 2)
                    actPen.DashPattern = New Single() {2.0F, 2.0F, 2.0F, 2.0F} '{4.0F, 2.0F, 1.0F, 3.0F} 'Defines Dash Pattern
                    actPen.Alignment = Drawing2D.PenAlignment.Outset
                    Sdate = CDate(itm.Item("StartDate"))
                    Edate = Calc_DueDate(itm.Item("StartDate"), itm.Item("EstTime"), itm.Item("EstTimeUM")) 'CDate(itm.Item("DueDate"))
                    locXa = (((Math.Abs(DateMin.Subtract(Sdate).TotalDays) + 1) / TotalDays) * (panel_Schedule.Width)) 'Left
                    widX = (((Math.Abs(Edate.Subtract(Sdate).TotalDays) + 1) / TotalDays) * (panel_Schedule.Width)) 'Width
                    r = New Rectangle(locXa, (top + offset), widX, thk + 6)
                    panel_Schedule.CreateGraphics.DrawRectangle(actPen, r)
                    '--------------------------------------------


                    'Add Actual Rounded Rectangle Block
                    '--------------------------------------------
                    If Not IsDBNull(itm.Item("ActualStart")) Then
                        Sdate = CDate(itm.Item("ActualStart"))
                        locXa = (((Math.Abs(DateMin.Subtract(Sdate).TotalDays) + 1) / TotalDays) * (panel_Schedule.Width)) 'Left
                        If Not IsDBNull(itm.Item("ActualEnd")) Then
                            Edate = CDate(itm.Item("ActualEnd"))
                            widX = (((Math.Abs(Edate.Subtract(Sdate).TotalDays) + 1) / TotalDays) * (panel_Schedule.Width)) 'Width
                            r = New Rectangle(locXa, (top + offset + 2), widX, thk) 'New Rectangle(locXa, (lHgt(1) + thk + offset), widX, thk)
                            b = New SolidBrush(RangeColor(dtSchedule.Rows.IndexOf(itm))) 'ColorTranslator.FromHtml(RangeColor(dtSchedule.Rows.IndexOf(itm)))) 'i))) 'Brush Color
                            If Type(0) = "Phase" AndAlso Not Type(1) = itm.Item("Item").ToString Then b = New SolidBrush(RangeColor(7))
                            Call FillRoundedRectangle(r, d, b, panel_Schedule)

                            'Add Event Label
                            Call Add_Label(itm.Item("Item").ToString.Replace(" Phase", ""), r, panel_Schedule)

                        Else
                            'If Not Complete, add a Start Flag and a Line indicating progress
                            r = New Rectangle(locXa, (top + offset + 3), 20, 20)
                            panel_Schedule.CreateGraphics.DrawImage(My.Resources.start_flag, r)

                            'Draw a line to today to show progress
                            If Sdate < Date.Today Then
                                If Date.Today.Subtract(Sdate).TotalDays > 0 Then
                                    myPen = New Pen(RangeColor(dtSchedule.Rows.IndexOf(itm)), 3)
                                    'If Type(0) = "Phase" AndAlso Not Type(1) = itm.Item("Item").ToString Then myPen = New Pen(RangeColor(6), 3)
                                    Dim t As Integer = Date.Today.Subtract(Sdate).TotalDays
                                    Dim len As Integer = (t / TotalDays) * panel_Schedule.Width

                                    panel_Schedule.CreateGraphics.DrawLine(myPen, r.X + 11, (top + offset + 14), r.X + len, (top + offset + 14))

                                    'Add the % Complete label
                                    r = New Rectangle(r.X + len + 5, (top + offset + 5), 40, (top + offset + 5)) '(X, Y, Lenght, Width)
                                    panel_Schedule.CreateGraphics.DrawString(itm.Item("PercentComplete").ToString, New Drawing.Font("Tahoma", 10, FontStyle.Regular), myPen.Brush, r, frmt)

                                End If
                            End If
                        End If
                    End If
                    '-------------------------------------------------------------

                End If 'Startup table BuildStart & BuildEnd dates are not Null


                'Add First Use Date (Event Date) to Initial Phase Row (assuming most space available)
                '-------------------------------------------------------------
                If dtSchedule.Rows.IndexOf(itm) = 0 Then 'Initial Phase Row
                    If Not IsDBNull(dtHeader.Rows(0).Item("EstUseDate")) Then
                        If IsDate(dtHeader.Rows(0).Item("EstUseDate")) Then
                            locXa = (((Math.Abs(DateMin.Subtract(CDate(dtHeader.Rows(0).Item("EstUseDate"))).TotalDays) + 1) / TotalDays) * (panel_Schedule.Width)) 'Left
                            r = New Rectangle(locXa, (top + offset + 8), 11, 11)
                            'Add the Indicator Image
                            panel_Schedule.CreateGraphics.DrawImage(My.Resources.start_point, r)
                            'Add the Indicator label
                            Dim lblX As Integer = r.X + 12
                            Dim lblWid As Integer = 100
                            'determine if the label has to go on the Left or Right [based on amount of room]
                            If (panel_Schedule.Width - (r.X + 12) - 8) < 110 Then
                                lblX = r.X - 103
                                frmt.Alignment = StringAlignment.Far
                            End If
                            r = New Rectangle(lblX, (top + offset + 5), lblWid, (top + offset + 5)) '(X, Y, Lenght, Width)
                            panel_Schedule.CreateGraphics.DrawString(dtHeader.Rows(0).Item("RequestDue").ToString, New Drawing.Font("Tahoma", 9, FontStyle.Regular), myPen.Brush, r, frmt)
                        End If
                    End If
                End If
                '-------------------------------------------------------------



                'Draw Separater Line
                myPen = New Pen(RangeColor(6), 1) 'New Pen(RangeColor(dtSchedule.Rows.IndexOf(itm)), 2)
                If Type(0) = "Phase" AndAlso Type(1) = itm.Item("Item").ToString Then myPen = New Pen(RangeColor(dtSchedule.Rows.IndexOf(itm)), 2)
                panel_Schedule.CreateGraphics.DrawLine(myPen, 0, top + ctrTL.Height + 1, panel_Schedule.Width, top + ctrTL.Height + 1)
                pnl_Schedule.CreateGraphics.DrawLine(myPen, 0, top + ctrTL.Height + 1, panel_Schedule.Width, top + ctrTL.Height + 1)

                'Move Top for next ctrl
                top = ctrTL.Top + ctrTL.Height + 2
                If Type(0) = "Phase" AndAlso Type(1) = itm.Item("Item").ToString Then top = top + 1 'add an extra buffer for drawing thicker dividing line

            Next

            lbl_Task.Width = (pnl_Schedule.Width - lbl_Task.Left) - 4
            'Me.Height = top + 6

        Catch ex As Exception

        End Try

    End Sub

    Public Sub Add_Label(xTxt As String, rect As Rectangle, pnl As Panel)

        Dim g As System.Drawing.Graphics = pnl.CreateGraphics()
        Dim xFont As Drawing.Font
        xFont = New Drawing.Font("Verdana", Get_Font_Size(g, xTxt, New Rectangle(rect.X - 3, rect.Y + 0.5, rect.Width + 3, rect.Height - 8)),
                                   FontStyle.Bold)
        Dim format As New StringFormat()
        format.Alignment = StringAlignment.Center
        format.LineAlignment = StringAlignment.Center
        Dim TxtBr As Brush = New SolidBrush(Color.White)
        If rect.Width < 30 And Len(xTxt) > 3 Then
            'reset the brush and rectangle size if width is too small
            TxtBr = New SolidBrush(Color.Black)
            rect = New Rectangle(rect.X - 30, rect.Y, rect.Width + 60, rect.Height) '(lHgt(2) * 0.3333))
            xFont = New Drawing.Font("Verdana", 8, FontStyle.Bold)
        End If

        pnl.CreateGraphics.DrawString(xTxt, xFont, TxtBr, New Rectangle(rect.X - 3, rect.Y + 1, rect.Width + 3, rect.Height), format)

    End Sub

    Public Sub Draw_Cal_Lines(pnl As Panel)

        'Now Loop Thru Remaining Months to Draw Dividing Lines and Write Month Name
        Dim locx As Double
        Dim t As Double
        Dim myPen As New Pen(Color.SteelBlue, 2)

        Dim CalFont As Font = New Drawing.Font("Verdana", 11, FontStyle.Bold)
        Dim format As New StringFormat()
        format.Alignment = StringAlignment.Center
        format.LineAlignment = StringAlignment.Center
        pnl.SuspendLayout()

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
                    r1 = New Rectangle(1, 6, drwStrWid, pnl_Months.Height - 4)
                    CalFont = New Drawing.Font("Verdana", 10, FontStyle.Regular)
                    format.Alignment = StringAlignment.Near
                    pnl_Months.CreateGraphics.DrawString(MonthName(DateMin.Month, True) & " " & DateMin.Year, CalFont, New SolidBrush(Color.Blue), r1, format)
                End If
            End If
        End If
        '----------------------------------------------------
        '----------------------------------------------------

        'Add OffDays DateRange(s)  (Do this first so lines and labels can be drawn on top)
        '-------------------------------------------------------------
        Dim itmColor(4) As Color

        If Not pnl.Name.ToString = "pnl_Months" Then
            Dim DtCheck As String = "(DateStart >= '" & DateMin.ToString("MM/dd/yyyy") & "' AND DateEnd <='" & DateMax.ToString("MM/dd/yyyy") & "')  "
            If frm_Main.dtOffDays.Select(DtCheck).Any Then
                'Draw Vertical Line for Start & Stop
                Dim myBrush As SolidBrush = New SolidBrush(Color.Silver)
                For Each offday As DataRow In frm_Main.dtOffDays.Select(DtCheck).CopyToDataTable.Rows
                    If offday.Item("Item").ToString = "PTO" Then myBrush = New SolidBrush(Color.PowderBlue) Else myBrush = New SolidBrush(Color.Silver)

                    Dim locXa As Double = ((Math.Abs(DateMin.Subtract(offday.Item("DateStart")).TotalDays) + 1) / TotalDays) * panel_Schedule.Width 'Left
                    Dim widX As Double = (Math.Abs(offday.Item("DateEnd").Subtract(offday.Item("DateStart")).TotalDays + 1) / TotalDays) * panel_Schedule.Width 'Width
                    Dim r As New Rectangle(locXa, pnl.Top, widX, pnl.Height)
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

            Dim chkString As String() = {"panel_Schedule", "pnl_Overall"} 'do not add day lines for pnl_Months
            If chkString.Contains(pnl.Name) Then
                Select Case Day.DayOfWeek
            'If Day.DayOfWeek = 1 Then 'Monday
                    Case DayOfWeek.Monday 'Monday
                        t = Day.Subtract(DateMin).TotalDays + 1 't + System.DateTime.DaysInMonth(Day.Year, Day.Month)
                        locx = (t / TotalDays) * pnl.Width
                        pnl.CreateGraphics.DrawLine(dotPen, CInt(locx), 0, CInt(locx), pnl.Height)

                        'put day indicator in small label at the top
                        CalFont = New Drawing.Font("Verdana", 7, FontStyle.Regular)
                        r1 = New Rectangle(CInt(locx), 0.5, 20, 11)
                        format.Alignment = StringAlignment.Near
                        pnl.CreateGraphics.DrawString(Day.Day, CalFont,
                                                                New SolidBrush(Color.Black), r1, format)

                    Case DayOfWeek.Saturday ', DayOfWeek.Sunday 'Saturday & Sunday
                        Dim locXa As Double = ((Math.Abs(DateMin.Subtract(Day).TotalDays) + 1) / TotalDays) * panel_Schedule.Width 'Left
                        Dim widX As Double = (2 / TotalDays) * panel_Schedule.Width 'Width
                        Dim r As New Rectangle(locXa, pnl.Top - 2, widX, pnl.Height)
                        pnl.CreateGraphics.FillRectangle(New SolidBrush(Color.Gainsboro), r)

                End Select
            End If
            dotPen.Dispose()
            'End If
            '----------------------------------------------------

            '[MONTH]
            '----------------------------------------------------
            If Day.Day = 1 Then
                If Day.Month = 1 Then
                    myPen = New Pen(Color.SteelBlue, 3)
                Else
                    myPen = New Pen(Color.LightSteelBlue, 2)
                End If
                't = Day.Subtract(DateMin).TotalDays + 1 't + System.DateTime.DaysInMonth(Day.Year, Day.Month)
                'locx = (t / TotalDays) * pnl.Width
                locx = (Day.Subtract(DateMin).TotalDays / TotalDays) * pnl.Width

                pnl.CreateGraphics.DrawLine(myPen, CInt(locx), 0, CInt(locx), pnl.Height)

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
            If DateDiff(DateInterval.Day, Day, Date.Today) = 0 Then
                myPen = New Pen(Color.Red, 3)
                t = Day.Subtract(DateMin).TotalDays + 1  't + System.DateTime.DaysInMonth(Day.Year, Day.Month)
                'Dim t As Integer = Date.Today.Subtract(Sdate).TotalDays
                locx = (t / TotalDays) * pnl.Width
                pnl.CreateGraphics.DrawLine(myPen, CInt(locx), 0, CInt(locx), pnl.Height)
            End If

        Next Day 'mnth


        pnl.ResumeLayout()

    End Sub

    Public Function GetImageID(status As String) As Integer

        Select Case status
            Case "Complete"
                GetImageID = 1
            Case "On Track"
                GetImageID = 2
            Case "At Risk"
                GetImageID = 0
            Case "Not Started"
                GetImageID = 3
            Case "Off Track"
                GetImageID = 4
            Case "On Hold"
                GetImageID = 5
            Case Else
                GetImageID = -1
        End Select

    End Function

    Public Sub FillRoundedRectangle(ByVal r As Rectangle, ByVal d As Integer, ByVal b As Brush, pnl As Panel)

        'Public Sub FillRoundedRectangle(ByVal g As Drawing.Graphics, ByVal r As Rectangle, ByVal d As Integer, ByVal b As Brush)
        Dim g As Drawing.Graphics = pnl.CreateGraphics()
        Dim mode As Drawing2D.SmoothingMode = g.SmoothingMode
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias 'System.Drawing.Drawing2D.SmoothingMode.HighSpeed

        'Rounded Corners - only if big enough
        If r.Width > 10 Then
            g.FillPie(b, r.X, r.Y, d, d, 178, 360)
            g.FillPie(b, r.X + r.Width - d, r.Y, d, d, 268, 360)
            g.FillPie(b, r.X, r.Y + r.Height - d, d, d, 88, 360)
            g.FillPie(b, r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 360)
        Else
            g.FillRectangle(b, r) 'Top
        End If

        'Filled Rectangle Middle
        '           (brush),       x,            y,        Width,              Height
        g.FillRectangle(b, (CInt(r.X + d / 2)) - 1, (r.Y), (r.Width - d) + 1, CInt(d / 2) + 2) 'Top
        g.FillRectangle(b, r.X, CInt(r.Y + d / 2), r.Width, CInt(r.Height - d) + 2) 'Middle
        g.FillRectangle(b, (CInt(r.X + d / 2)) - 1, CInt(r.Y + r.Height - d / 2), CInt(r.Width - d) + 1, CInt(d / 2)) 'Bottom
        g.SmoothingMode = mode

    End Sub

    Public Function Get_Font_Size(g As Graphics, txt As String, r As Rectangle)
        Dim font_size As Single
        Dim new_font As Font
        Dim StringSize As New SizeF

        For font_size = 9.75 To 30 Step 0.75
            ' Get the next font.
            new_font = New Font("Verdana", font_size, FontStyle.Bold)

            ' See if we have room.
            StringSize = TextRenderer.MeasureText(txt, new_font)
            If font_size > r.Height Or StringSize.Width > r.Width Then
                'step back 2 sizes to give the text a buffer to the borders of the rectangle
                font_size = font_size - 3
                Exit For

            End If

        Next font_size

        Return font_size

    End Function


End Class
