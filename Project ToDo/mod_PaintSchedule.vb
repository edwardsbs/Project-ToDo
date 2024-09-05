Module mod_PaintSchedule
    'Public lHgt(4) As Integer 'used to keep up with the top of each Section
    'Public DateMin As Date = #11/7/1975#
    'Public DateMax As Date = #7/24/2003#
    'Public TotalDays As Integer = Math.Abs(DateMax.Subtract(DateMin).TotalDays)
    'Public dtSchedule As New DataTable
    'Public dtHeader As New DataTable

    'Sub Paint_Schedule()

    '    frm_Main.panel_Schedule.Controls.Clear()
    '    frm_Main.panel_Schedule.Refresh()
    '    frm_Main.pnl_Months.Controls.Clear()
    '    frm_Main.pnl_Months.Refresh()
    '    'frm_Main.pnl_Overall.Controls.Clear()
    '    'frm_Main.pnl_Overall.Refresh()

    '    If IsNothing(frm_Main.dgv_Mgmt.SelectedRows) Then Exit Sub
    '    If frm_Main.dgv_Mgmt.SelectedRows.Count = 0 Then Exit Sub

    '    If frm_Main.Loading = True Then Exit Sub

    '    'Initialize the Public Row Heights for the Calendar Panels
    '    lHgt(1) = frm_Main.panel_Schedule.Height * 0.24 '.36 'Set Header Height
    '    lHgt(2) = frm_Main.panel_Schedule.Height * 0.5 '.61 'Set General Schedule Height
    '    lHgt(3) = 24 'Set Bar Height
    '    'lHgt(3) = Me.panel_GOplan.Height 'frm_Main.panel_Schedule.Height * 0.16 'Set GO Plan Height
    '    'lHgt(4) = Me.panel_Recovery.Height 'frm_Main.panel_Schedule.Height - (lHgt(1) + lHgt(2) + lHgt(3)) 'Set Recovery Plan as to the remainder of the height

    '    frm_Main.panel_Schedule.SuspendLayout()

    '    Dim dgRow As DataGridViewRow = frm_Main.dgv_Mgmt.SelectedRows(0)
    '    'Get Header Row 1st
    '    Dim FilterTxt As String = "System='" & dgRow.Cells("System").Value & "' AND [Path]='" & dgRow.Cells("Path").Value & "' " & _
    '        " AND Item='" & dgRow.Cells("Item").Value & "'"
    '    dtHeader = New DataView(frm_Main.dsSys.Tables("dtItems"), FilterTxt, "Phase, iIndex, Item ASC", DataViewRowState.CurrentRows).ToTable(True, "Item", "Notes", "Scope", "Phase", "Status", "EstTime", "EstTimeUM", "StartDate", "DueDate", "ActualStart", "ActualEnd", "Requester", "RequestDue", "Urgency", "Complexity", "Pri", "Parent")

    '    If Not String.IsNullOrEmpty(dtHeader.Rows(0).Item("Scope").ToString) Then frm_Main.rTxt_ScopeMgmt.Rtf =
    '                ConvertTextToRTF(dtHeader.Rows(0).Item("Scope"))

    '    'Get Child Items
    '    FilterTxt = "System='" & dgRow.Cells("System").Value & "' AND [Parent]='" & dgRow.Cells("Path").Value & "' " & _
    '        " AND Type='Phase'"
    '    dtSchedule = New DataView(frm_Main.dsSys.Tables("dtItems"), FilterTxt, "iIndex, Item ASC", DataViewRowState.CurrentRows).ToTable(True, "Item", "Notes", "Scope", "Phase", "Status", "EstTime", "EstTimeUM", "StartDate", "DueDate", "ActualStart", "ActualEnd", "Requester", "RequestDue", "Urgency", "Complexity", "Pri", "Parent")
    '    'MsgBox(dtSchedule.Rows.Count)

    '    'Fill Header Info:
    '    frm_Main.lbl_Task.Text = dtHeader.Rows(0).Item("Item").ToString
    '    frm_Main.lbl_Priority.Text = dtHeader.Rows(0).Item("Pri").ToString

    '    Try

    '        Dim myPen As Pen
    '        myPen = New Pen(Color.LightSteelBlue, 1) 'instantiate a new pen object using the color structure

    '        ''Draw Horizontal Dividing Lines
    '        ''-------------------------------------------------------------
    '        'frm_Main.panel_Schedule.CreateGraphics.DrawLine(myPen, 0, lHgt(1), frm_Main.panel_Schedule.Width - 4, lHgt(1))

    '        'Dim pos As Integer
    '        'Dim dotPen As New Pen(Color.LightGray, 1)
    '        'dotPen.DashCap = Drawing2D.DashCap.Triangle
    '        'dotPen.DashPattern = New Single() {4.0F, 2.0F, 1.0F, 3.0F} 'Defines Dash Pattern
    '        'pos = (lHgt(2) / 3) + lHgt(1) 'use 1/3 of the General Schedule for Event Ranges and the rest for Ord/Del Ranges
    '        'frm_Main.panel_Schedule.CreateGraphics.DrawLine(dotPen, 0, pos, _
    '        '                   frm_Main.panel_Schedule.Width - 4, pos)
    '        'dotPen.Dispose()

    '        ''-------------------------------------------------------------

    '        Dim CalFont As Font = New Drawing.Font("Verdana", 10, FontStyle.Regular)

    '        'Get MIN date from earliest date possible
    '        DateMin = Date.Today

    '        'Now Check to see if there is a StartDate Date 
    '        If IsNothing(frm_Main.dgv_Mgmt.SelectedRows) Then Exit Sub
    '        If frm_Main.dgv_Mgmt.SelectedRows.Count = 0 Then Exit Sub

    '        If Not String.IsNullOrEmpty(dgRow.Cells("StartDate").Value.ToString) Then
    '            DateMin = If(IsDBNull(dtHeader.Rows(0).Item("ActualStart")), CDate(dtHeader.Rows(0).Item("StartDate")), If(CDate(dtHeader.Rows(0).Item("ActualStart")) > CDate(dtHeader.Rows(0).Item("StartDate")), CDate(dtHeader.Rows(0).Item("StartDate")), CDate(dtHeader.Rows(0).Item("ActualStart"))))
    '        End If

    '        If DateMin = Date.Today Then
    '            DateMin = Today.AddDays(-8) 'Initialize DateMin to 2 years ago
    '        Else
    '            DateMin = DateMin.AddDays(-8) 'back up 60 days for spacing
    '        End If

    '        'DO THE REST AS LONG AS A DueDate Date Exists
    '        If Not String.IsNullOrEmpty(dgRow.Cells("DueDate").Value.ToString) Then
    '            'Draw Vertical HEADER lines (Year & Months)
    '            '-------------------------------------------------------------

    '            DateMax = CDate(If(IsDBNull(dtHeader.Rows(0).Item("ActualEnd")), CDate(dtHeader.Rows(0).Item("DueDate")), If(CDate(dtHeader.Rows(0).Item("ActualEnd")) > CDate(dtHeader.Rows(0).Item("DueDate")), CDate(dtHeader.Rows(0).Item("ActualEnd")), CDate(dtHeader.Rows(0).Item("DueDate"))))).AddDays(8) 'CDate(frm_Main.txt_End.Text) '

    '            Dim ctrNum As Integer = 0
    '            TotalDays = Math.Abs(DateMax.Subtract(DateMin).TotalDays)

    '            'Draw Month Lines and Labels 1st
    '            '-------------------------------------------------------------
    '            frm_Main.panel_Schedule.SuspendLayout()

    '            Call Draw_Cal_Lines(frm_Main.panel_Schedule)
    '            'Call Draw_Cal_Lines(frm_Main.pnl_Overall)
    '            Call Draw_Cal_Lines(frm_Main.pnl_Months)

    '            frm_Main.panel_Schedule.ResumeLayout()
    '            '-------------------------------------------------------------


    '            'Draw Phases
    '            Call Draw_Build_Ranges()

    '        End If 'Max date present


    '    Catch ex As Exception

    '    End Try

    '    frm_Main.panel_Schedule.ResumeLayout()

    'End Sub

    'Sub Draw_Build_Ranges()

    '    Try

    '        Dim TotalDays As Integer = Math.Abs(DateMax.Subtract(CDate(DateMin)).TotalDays)

    '        'Dim g As System.Drawing.Graphics
    '        Dim r As Rectangle = New Rectangle(30, 30, 60, 30) 'rectangle size
    '        Dim d As Integer = 6 '((lHgt(1) * 0.3333) - 2) '15 'degree of corner roundness
    '        Dim b As Brush = New SolidBrush(ColorTranslator.FromHtml("#67E46F")) 'Brush Color
    '        Dim locXa As Double 'Left location
    '        Dim widX As Double 'Width
    '        Dim thk As Double = lHgt(3) '0.36 * 0.333333 '(lHgt(1) * 0.3333) 'height

    '        Dim RangeColor(8) As Color 'String
    '        RangeColor(0) = Color.Orange '"#FF9840" 'Blue
    '        RangeColor(1) = Color.Green '"#67E46F" 'Green
    '        RangeColor(2) = Color.Blue '"#5DC8CD" 'Orange
    '        RangeColor(3) = Color.MediumOrchid '"#FF7673" 'Pink
    '        'RangeColor(4) = "#218555" 'Forest Green
    '        'RangeColor(5) = "#D30068"
    '        'RangeColor(6) = "#086FA1"

    '        frm_Main.pnl_Schedule.Controls.Clear()
    '        frm_Main.pnl_Schedule.Refresh()

    '        '------LOOP THRU EVENTS IN StartUp Table AND DRAW RANGES
    '        Dim Sdate As DateTime
    '        Dim Edate As DateTime
    '        Dim top As Integer = 8
    '        Dim offset As Integer = 4 'offset to center drawn ranges with control
    '        Dim myPen As Pen = New Pen(Color.Black, 1)

    '        Dim CalFont As Font = New Drawing.Font("Verdana", 10, FontStyle.Bold)
    '        'Dim format As New StringFormat()
    '        Dim br As New SolidBrush(Color.Black)
    '        Dim frmt As New StringFormat()
    '        frmt.Alignment = StringAlignment.Near
    '        frmt.LineAlignment = StringAlignment.Near

    '        '[DRAW FULL RANGE 1ST]
    '        '---------------------------------------------------
    '        Try
    '            Sdate = If(IsDBNull(dtHeader.Rows(0).Item("ActualStart")), CDate(dtHeader.Rows(0).Item("StartDate")), If(CDate(dtHeader.Rows(0).Item("ActualStart")) > CDate(dtHeader.Rows(0).Item("StartDate")), CDate(dtHeader.Rows(0).Item("StartDate")), CDate(dtHeader.Rows(0).Item("ActualStart"))))
    '            Edate = If(IsDBNull(dtHeader.Rows(0).Item("ActualEnd")), CDate(dtHeader.Rows(0).Item("DueDate")), If(CDate(dtHeader.Rows(0).Item("ActualEnd")) > CDate(dtHeader.Rows(0).Item("DueDate")), CDate(dtHeader.Rows(0).Item("ActualEnd")), CDate(dtHeader.Rows(0).Item("DueDate"))))
    '            locXa = ((Math.Abs(DateMin.Subtract(Sdate).TotalDays) + 1) / TotalDays) * frm_Main.panel_Schedule.Width 'Left
    '            widX = (Math.Abs(Edate.Subtract(Sdate).TotalDays) / TotalDays) * frm_Main.panel_Schedule.Width 'Width
    '            'r = New Rectangle(locXa, 5, widX, thk) 'New Rectangle(locXa, lHgt(1) + 5, widX, thk)
    '            r = New Rectangle(locXa, (top + offset + 2), widX, thk)
    '            b = New SolidBrush(Color.Black)
    '            Call FillRoundedRectangle(r, 3, b, frm_Main.panel_Schedule)
    '            Call Add_Label(dtHeader.Rows(0).Item("Item").ToString, r, frm_Main.panel_Schedule)

    '            'Add the labels to the controls side as a header
    '            r = New Rectangle(3, (top + offset + 2), widX, thk) '(X, Y, Lenght, Width)
    '            Dim format As New StringFormat()
    '            format.Alignment = StringAlignment.Near
    '            format.LineAlignment = StringAlignment.Center
    '            frm_Main.pnl_Schedule.CreateGraphics.DrawString(dtHeader.Rows(0).Item("Item").ToString, New Drawing.Font("Verdana", 12, FontStyle.Bold), New SolidBrush(Color.Black), r, format)

    '            'frm_Main.panel_Schedule.CreateGraphics.DrawLine(myPen, 0, top, frm_Main.panel_Schedule.Width, top)
    '            'frm_Main.pnl_Schedule.CreateGraphics.DrawLine(myPen, 0, top + 2, frm_Main.panel_Schedule.Width, top + 2)

    '            top = 48
    '        Catch ex As Exception

    '        End Try
    '        '---------------------------------------------------


    '        '[NOW DRAW THE PHASES]
    '        For Each itm As DataRow In dtSchedule.Rows

    '            'Draw A Separator Line
    '            myPen = New Pen(RangeColor(dtSchedule.Rows.IndexOf(itm)), 1)
    '            frm_Main.panel_Schedule.CreateGraphics.DrawLine(myPen, 0, top - 2, frm_Main.panel_Schedule.Width, top - 2)
    '            frm_Main.pnl_Schedule.CreateGraphics.DrawLine(myPen, 0, top - 2, frm_Main.panel_Schedule.Width, top - 2)

    '            '-----------------------------------------------------
    '            '--------[START ctr_ProjTimeLine]---------------------
    '            Dim ctrTL As New ctr_ProjTimeLine
    '            ctrTL.Name = itm.Item("Item").ToString.Replace(" Phase", "")
    '            ctrTL.Top = top
    '            ctrTL.lbl_Item.Text = itm.Item("Item").ToString.Replace(" Phase", "")
    '            ctrTL.lbl_PlanStart.Text = CDate(itm.Item("StartDate")).ToShortDateString
    '            ctrTL.lbl_PlanEnd.Text = CDate(itm.Item("DueDate")).ToShortDateString
    '            ctrTL.lbl_ActualStart.Text = If(IsDBNull(itm.Item("ActualStart")), "", CDate(itm.Item("ActualStart")).ToShortDateString)
    '            ctrTL.lbl_ActualEnd.Text = If(IsDBNull(itm.Item("ActualEnd")), "", CDate(itm.Item("ActualEnd")).ToShortDateString)
    '            ctrTL.pic_Status.Image = ctrTL.imgLst_Status.Images(GetImageID(itm.Item("Status").ToString))
    '            'ctrTL.lbl_Item.ForeColor = RangeColor(dtSchedule.Rows.IndexOf(itm))

    '            'ctrTL.dtp_Start.Value = CDate(itm.Item("StartDate")).ToShortDateString
    '            'ctrTL.dtp_End.Value = CDate(itm.Item("DueDate")).ToShortDateString

    '            'Status Combobox
    '            '-------------------------------------------------------
    '            'Setup image combobox items
    '            'Dim items(ctrTL.imgLst_Status.Images.Count - 1) As String
    '            'For i As Int32 = 0 To ctrTL.imgLst_Status.Images.Count - 1
    '            '    items(i) = "Item " & i.ToString
    '            'Next
    '            ''Add image combobox and format the control
    '            'ctrTL.cbo_Status.Items.AddRange(items)
    '            'ctrTL.cbo_Status.DropDownStyle = ComboBoxStyle.DropDownList
    '            'ctrTL.cbo_Status.DrawMode = DrawMode.OwnerDrawVariable
    '            'ctrTL.cbo_Status.ItemHeight = ctrTL.imgLst_Status.ImageSize.Height
    '            'ctrTL.cbo_Status.Width = ctrTL.imgLst_Status.ImageSize.Width + 30
    '            'ctrTL.cbo_Status.MaxDropDownItems = ctrTL.imgLst_Status.Images.Count

    '            'Dim imgID As Integer = GetImageID(itm.Item("Status").ToString)
    '            'If Not IsNothing(imgID) Then ctrTL.cbo_Status.SelectedIndex = imgID
    '            '-------------------------------------------------------


    '            frm_Main.pnl_Schedule.Controls.Add(ctrTL)
    '            'top = ctrTL.Top + ctrTL.Height
    '            '--------[END ctr_ProjTimeLine]-----------------------
    '            '-----------------------------------------------------

    '            'Draw the Phase Range
    '            If Not IsDBNull(itm.Item("StartDate")) And
    '                Not IsDBNull(itm.Item("DueDate")) Then

    '                'Add Plan Rectangle
    '                '--------------------------------------------
    '                Dim actPen As Pen
    '                actPen = New Pen(RangeColor(dtSchedule.Rows.IndexOf(itm)), 2)
    '                actPen.DashPattern = New Single() {2.0F, 2.0F, 2.0F, 2.0F} '{4.0F, 2.0F, 1.0F, 3.0F} 'Defines Dash Pattern
    '                actPen.Alignment = Drawing2D.PenAlignment.Outset
    '                Sdate = CDate(itm.Item("StartDate"))
    '                Edate = CDate(itm.Item("DueDate"))
    '                locXa = (((Math.Abs(DateMin.Subtract(Sdate).TotalDays) + 1) / TotalDays) * (frm_Main.panel_Schedule.Width)) 'Left
    '                widX = (((Math.Abs(Edate.Subtract(Sdate).TotalDays)) / TotalDays) * (frm_Main.panel_Schedule.Width)) 'Width
    '                r = New Rectangle(locXa, (top + offset), widX, thk + 6)
    '                frm_Main.panel_Schedule.CreateGraphics.DrawRectangle(actPen, r)
    '                '--------------------------------------------


    '                'Add Actual Rounded Rectangle Block
    '                '--------------------------------------------
    '                If Not IsDBNull(itm.Item("ActualStart")) Then
    '                    Sdate = CDate(itm.Item("ActualStart"))
    '                    locXa = (((Math.Abs(DateMin.Subtract(Sdate).TotalDays) + 1) / TotalDays) * (frm_Main.panel_Schedule.Width)) 'Left
    '                    If Not IsDBNull(itm.Item("ActualEnd")) Then
    '                        Edate = CDate(itm.Item("ActualEnd"))
    '                        widX = (((Math.Abs(Edate.Subtract(Sdate).TotalDays)) / TotalDays) * (frm_Main.panel_Schedule.Width)) 'Width
    '                        r = New Rectangle(locXa, (top + offset + 2), widX, thk) 'New Rectangle(locXa, (lHgt(1) + thk + offset), widX, thk)
    '                        b = New SolidBrush(RangeColor(dtSchedule.Rows.IndexOf(itm))) 'ColorTranslator.FromHtml(RangeColor(dtSchedule.Rows.IndexOf(itm)))) 'i))) 'Brush Color
    '                        Call FillRoundedRectangle(r, d, b, frm_Main.panel_Schedule)

    '                        'Add Event Label
    '                        Call Add_Label(itm.Item("Item").ToString.Replace(" Phase", ""), r, frm_Main.panel_Schedule)

    '                    Else

    '                        r = New Rectangle(locXa, (top + offset + 8), 11, 11)
    '                        frm_Main.panel_Schedule.CreateGraphics.DrawImage(My.Resources.start_point, r)

    '                        'Draw a line to today to show progress
    '                        If Sdate < Date.Today Then
    '                            If Date.Today.Subtract(Sdate).TotalDays > 0 Then
    '                                myPen = New Pen(Color.Black, 3)
    '                                Dim t As Integer = Date.Today.Subtract(Sdate).TotalDays
    '                                Dim len As Integer = (t / TotalDays) * frm_Main.panel_Schedule.Width

    '                                frm_Main.panel_Schedule.CreateGraphics.DrawLine(myPen, r.X + 11, (top + offset + 14), r.X + len, (top + offset + 14))

    '                            End If
    '                        End If
    '                    End If
    '                End If
    '                '--------------------------------------------


    '                'offset = offset + thk + 5

    '                ''Add Packout Date
    '                'Dim r2 As Rectangle = New Rectangle(locXa - 20, lHgt(1) + 5, widX, (lHgt(1) * 0.3333))
    '                'Using outlinePath As New Drawing2D.GraphicsPath
    '                '    outlinePath.AddString("P", CalFont.FontFamily, CalFont.Style, CalFont.Size, New Point(r2.X + 3, r2.Y + 2), StringFormat.GenericTypographic)
    '                '    'g.FillPath(Brushes.LightGray, outlinePath)
    '                '    'g.TranslateTransform(+1, +1)
    '                '    g.FillPath(Brushes.White, outlinePath)
    '                '    g.DrawPath(New Pen(Color.White, 2), outlinePath)
    '                'End Using
    '                'frm_Main.panel_Schedule.CreateGraphics.DrawString("P", CalFont, b, r2, format) 'Simulating Packout Date
    '                ''End If


    '                '' '' ''Draw Order/Delivery Ranges
    '                '' '' ''-------------------------------------------------------------
    '                '' '' ''[SET THE CODE UP BELOW TO GET THE ORDER & DELIVERY DATES FROM THE tbl_Event_TimingSchedule TABLE]
    '                ' '' ''Dim myPen As Pen = New Pen(Color.SteelBlue, 2)
    '                ' '' ''Dim bufr As Integer = (lHgt(2) / 3) + lHgt(1) + 3
    '                ' '' ''Dim hgt As Integer = ((lHgt(2) * (2 / 3)) / 3) - 3 'use 1/3 of the General Schedule for Event Ranges and the rest for Ord/Del Ranges (((panel_Calendar.Height / 4) - 15) / 4) / 2
    '                ' '' ''Dim ProcLoc(9) As String
    '                ' '' ''ProcLoc(1) = "Initial Phase"
    '                ' '' ''ProcLoc(2) = "Development Phase"
    '                ' '' ''ProcLoc(3) = "Implementation Phase"
    '                ' '' ''ProcLoc(4) = "Reflection Phase"
    '                ' '' ''ProcLoc(5) = "AE PO Date"
    '                ' '' ''ProcLoc(6) = "AF PO Date"
    '                ' '' ''ProcLoc(7) = "WE Delivery"
    '                ' '' ''ProcLoc(8) = "AE Delivery"
    '                ' '' ''ProcLoc(9) = "AF Delivery"

    '                '' '' ''Create a Range between Order and Delivery for WE, AE, and AF
    '                ' '' ''For j As Integer = 1 To 4

    '                ' '' ''    myPen = New Pen(ColorTranslator.FromHtml("#67E46F"), 2) 'RangeColor(i)), 2)
    '                ' '' ''    br = New SolidBrush(ColorTranslator.FromHtml("#67E46F")) 'RangeColor(i)))

    '                ' '' ''    'Delivery Ranges
    '                ' '' ''    If Not IsDBNull(itm.Item(ProcLoc(j + 3))) And _
    '                ' '' ''        Not IsDBNull(itm.Item(ProcLoc(j + 6))) Then
    '                ' '' ''        CalFont = New Drawing.Font("Arial", 8, FontStyle.Regular)
    '                ' '' ''        Sdate = CDate(itm.Item(ProcLoc(j + 3)))
    '                ' '' ''        Edate = CDate(itm.Item(ProcLoc(j + 6)))
    '                ' '' ''        locXa = ((Math.Abs(DateMin.Subtract(Sdate).TotalDays) / TotalDays) * (frm_Main.panel_Schedule.Width)) 'Left
    '                ' '' ''        widX = (((Math.Abs(DateMin.Subtract(Edate).TotalDays) + 1) / TotalDays) * (frm_Main.panel_Schedule.Width)) 'Width
    '                ' '' ''        frm_Main.panel_Schedule.CreateGraphics.DrawLine(myPen, CInt(locXa), bufr, CInt(locXa), bufr + hgt)
    '                ' '' ''        g.FillRectangle(b, New Rectangle(CInt(locXa) + 2, bufr, (CInt(widX) - CInt(locXa)) - 4, hgt))
    '                ' '' ''        'frm_Main.panel_Calendar.CreateGraphics.DrawString(">", CalFont, br, New Rectangle(locXa + 1, bufr - (hgt / 2), 10, 15), frmt)
    '                ' '' ''        frm_Main.panel_Schedule.CreateGraphics.DrawLine(myPen, CInt(widX), bufr, CInt(widX), bufr + hgt)

    '                ' '' ''        'DSPS POs
    '                ' '' ''        If Not IsDBNull(itm.Item(ProcLoc(j))) Then
    '                ' '' ''            Sdate = CDate(itm.Item(ProcLoc(j)))
    '                ' '' ''            locXa = ((Math.Abs(DateMin.Subtract(Sdate).TotalDays) / TotalDays) * (frm_Main.panel_Schedule.Width)) 'Left
    '                ' '' ''            CalFont = New Drawing.Font("Verdana", 10, FontStyle.Bold)
    '                ' '' ''            If itm.Item(ProcLoc(j)) = itm.Item(ProcLoc(j + 3)) Then
    '                ' '' ''                br = New SolidBrush(Color.White)
    '                ' '' ''            End If
    '                ' '' ''            frm_Main.panel_Schedule.CreateGraphics.DrawString("*", CalFont, br, New Rectangle(CInt(locXa), bufr, 10, 15), frmt)
    '                ' '' ''        End If
    '                ' '' ''    End If

    '                ' '' ''    bufr = bufr + hgt + 2
    '                ' '' ''Next j
    '                '-------------------------------------------------------------

    '            End If 'Startup table BuildStart & BuildEnd dates are not Null

    '            'Draw Separater Line
    '            'myPen = New Pen(Color.LightGray, 1)
    '            myPen = New Pen(RangeColor(dtSchedule.Rows.IndexOf(itm)), 1)
    '            frm_Main.panel_Schedule.CreateGraphics.DrawLine(myPen, 0, top + ctrTL.Height + 2, frm_Main.panel_Schedule.Width, top + ctrTL.Height + 2)
    '            frm_Main.pnl_Schedule.CreateGraphics.DrawLine(myPen, 0, top + ctrTL.Height + 2, frm_Main.panel_Schedule.Width, top + ctrTL.Height + 2)

    '            'Move Top for next ctrl
    '            top = ctrTL.Top + ctrTL.Height + 8

    '        Next

    '    Catch ex As Exception

    '    End Try

    'End Sub

    'Public Sub Add_Label(xTxt As String, rect As Rectangle, pnl As Panel)

    '    Dim g As System.Drawing.Graphics = pnl.CreateGraphics()
    '    Dim xFont As Drawing.Font
    '    xFont = New Drawing.Font("Verdana", Get_Font_Size(g, xTxt, New Rectangle(rect.X - 3, rect.Y + 0.5, rect.Width + 3, rect.Height - 8)),
    '                               FontStyle.Bold)
    '    Dim format As New StringFormat()
    '    format.Alignment = StringAlignment.Center
    '    format.LineAlignment = StringAlignment.Center
    '    Dim TxtBr As Brush = New SolidBrush(Color.White)
    '    If rect.Width < 30 And Len(xTxt) > 3 Then
    '        'reset the brush and rectangle size if width is too small
    '        TxtBr = New SolidBrush(Color.Black)
    '        rect = New Rectangle(rect.X - 30, rect.Y, rect.Width + 60, rect.Height) '(lHgt(2) * 0.3333))
    '        xFont = New Drawing.Font("Verdana", 8, FontStyle.Bold)
    '    End If
    '    'frm_Main.pnl_Months.CreateGraphics.DrawString(xTxt, xFont,
    '    '                                            TxtBr, New Rectangle(rect.X - 3, rect.Y + 1, rect.Width + 3, rect.Height), format)
    '    pnl.CreateGraphics.DrawString(xTxt, xFont,
    '                                                TxtBr, New Rectangle(rect.X - 3, rect.Y + 1, rect.Width + 3, rect.Height), format)

    'End Sub

    'Public Sub Draw_Cal_Lines(pnl As Panel)

    '    'Now Loop Thru Remaining Months to Draw Dividing Lines and Write Month Name
    '    Dim locx As Double
    '    Dim t As Double
    '    Dim myPen As New Pen(Color.SteelBlue, 2)

    '    Dim CalFont As Font = New Drawing.Font("Verdana", 11, FontStyle.Bold)
    '    Dim format As New StringFormat()
    '    format.Alignment = StringAlignment.Center
    '    format.LineAlignment = StringAlignment.Center
    '    pnl.SuspendLayout()
    '    Dim pos As Integer = 0 'lHgt(1) '(frm_Main.lHgt(2) / 3) '+ frm_Main.lHgt(1) 'use 1/3 of the General Schedule for Event Ranges and the rest for Ord/Del Ranges
    '    Dim startY As Integer = 0

    '    'Draw 1st Month label first (if enough space - 15th or greater).  Otherwise it gets skipped because it is looping to look for the 1st day in the month
    '    '----------------------------------------------------
    '    '----------------------------------------------------
    '    Dim dayCnt As Integer = System.DateTime.DaysInMonth(DateMin.Year, DateMin.Month) - DateMin.Day
    '    Dim drwStrWid As Integer = (dayCnt / TotalDays) * pnl.Width 'Draw String Width - calculate how much space the month can take up
    '    Dim r1 As New Rectangle(1, pnl.Top + 3, drwStrWid, pnl.Height - 4)
    '    If pnl.Name = "pnl_Months" AndAlso DateMin.Day > 1 Then
    '        If drwStrWid >= 45 Then 'Make sure the width is at least 45
    '            'Plenty of room, write the month label normally
    '            CalFont = New Drawing.Font("Verdana", 11, FontStyle.Bold)
    '            format.Alignment = StringAlignment.Center
    '            pnl.CreateGraphics.DrawString(MonthName(DateMin.Month, True) & " " & DateMin.Year, CalFont,
    '                                                        New SolidBrush(Color.Blue), r1, format)
    '        Else
    '            If drwStrWid > 30 Then 'Only write if there is enough room.  otherwise it will overlap the next month.
    '                'Not much room, write a smaller month label
    '                r1 = New Rectangle(1, 6, drwStrWid, frm_Main.pnl_Months.Height - 4)
    '                CalFont = New Drawing.Font("Verdana", 10, FontStyle.Regular)
    '                format.Alignment = StringAlignment.Near
    '                frm_Main.pnl_Months.CreateGraphics.DrawString(MonthName(DateMin.Month, True) & " " & DateMin.Year, CalFont, New SolidBrush(Color.Blue), r1, format)
    '            End If
    '        End If
    '    End If
    '    '----------------------------------------------------
    '    '----------------------------------------------------

    '    'Draw Vertical Lines
    '    For Each Day As DateTime In Enumerable.Range(0, (DateMax - DateMin).Days) _
    '                        .Select(Function(i) DateMin.AddDays(i))

    '        '[WEEK]
    '        '----------------------------------------------------
    '        Dim dotPen As New Pen(Color.LightGray, 1.5)
    '        dotPen.DashCap = Drawing2D.DashCap.Triangle
    '        dotPen.DashPattern = New Single() {4.0F, 2.0F, 1.0F, 3.0F} 'Defines Dash Pattern


    '        If Day.DayOfWeek = 1 Then
    '            myPen = New Pen(Color.LightGray, 1)
    '            t = Day.Subtract(DateMin).TotalDays + 1 't + System.DateTime.DaysInMonth(Day.Year, Day.Month)
    '            locx = (t / TotalDays) * pnl.Width
    '            Dim chkString As String() = {"panel_Schedule", "pnl_Overall"} 'do not add day lines for pnl_Months
    '            If chkString.Contains(pnl.Name) Then
    '                pnl.CreateGraphics.DrawLine(dotPen, CInt(locx), 0, CInt(locx), pnl.Height)

    '                'put day indicator in small label at the top
    '                CalFont = New Drawing.Font("Verdana", 7, FontStyle.Regular)
    '                r1 = New Rectangle(CInt(locx), 0.5, 20, 11)
    '                format.Alignment = StringAlignment.Near
    '                pnl.CreateGraphics.DrawString(Day.Day, CalFont,
    '                                                        New SolidBrush(Color.Black), r1, format)
    '            End If
    '        End If
    '            dotPen.Dispose()
    '        'End If
    '        '----------------------------------------------------

    '        '[MONTH]
    '        '----------------------------------------------------
    '        If Day.Day = 1 Then
    '            If Day.Month = 1 Then
    '                myPen = New Pen(Color.SteelBlue, 3)
    '            Else
    '                myPen = New Pen(Color.LightSteelBlue, 2)
    '            End If
    '            't = Day.Subtract(DateMin).TotalDays + 1 't + System.DateTime.DaysInMonth(Day.Year, Day.Month)
    '            'locx = (t / TotalDays) * pnl.Width
    '            locx = (Day.Subtract(DateMin).TotalDays / TotalDays) * pnl.Width

    '            pnl.CreateGraphics.DrawLine(myPen, CInt(locx), 0, CInt(locx), pnl.Height)

    '            'If pnl.Name = "panel_Schedule" OrElse pnl.Name = "pnl_Months" Then
    '            If pnl.Name = "pnl_Months" Then
    '                drwStrWid = 45 'Draw String Width - calculate how much space the month can take up
    '                dayCnt = If(Day.Month = DateMax.Month, DateMax.Day, System.DateTime.DaysInMonth(Day.Year, Day.Month))
    '                drwStrWid = (dayCnt / TotalDays) * pnl.Width
    '                If drwStrWid >= 45 Then 'Make sure the width is at least 45
    '                    'Plenty of room, write the month label normally
    '                    CalFont = New Drawing.Font("Verdana", 11, FontStyle.Bold)
    '                    format.Alignment = StringAlignment.Center
    '                    r1 = New Rectangle(locx, pnl.Top + 4, drwStrWid, pnl.Height - 4)
    '                    pnl.CreateGraphics.DrawString(MonthName(Day.Month, True) & " " & Day.Year, CalFont,
    '                                                        New SolidBrush(Color.Blue), r1, format)
    '                Else
    '                    If drwStrWid >= 30 Then 'Only write if there is enough room.  otherwise it will overlap the next month.
    '                        'Not much room, write a smaller month label
    '                        r1 = New Rectangle(3, 6, drwStrWid, frm_Main.pnl_Months.Height - 4)
    '                        CalFont = New Drawing.Font("Verdana", 10, FontStyle.Regular)
    '                        format.Alignment = StringAlignment.Near
    '                        frm_Main.pnl_Months.CreateGraphics.DrawString(MonthName(DateMin.Month, True) & " " & DateMin.Year, CalFont, New SolidBrush(Color.Blue), r1, format)
    '                    End If
    '                End If
    '            End If
    '        End If
    '        '----------------------------------------------------

    '        'Add a red line for today
    '        If DateDiff(DateInterval.Day, Day, Date.Today) = 0 Then
    '            myPen = New Pen(Color.Red, 3)
    '            t = Day.Subtract(DateMin).TotalDays + 1  't + System.DateTime.DaysInMonth(Day.Year, Day.Month)
    '            'Dim t As Integer = Date.Today.Subtract(Sdate).TotalDays
    '            locx = (t / TotalDays) * pnl.Width
    '            pnl.CreateGraphics.DrawLine(myPen, CInt(locx), 0, CInt(locx), pnl.Height)
    '        End If

    '    Next Day 'mnth


    '    pnl.ResumeLayout()

    'End Sub

    'Public Function GetImageID(status As String) As Integer

    '    Select Case status
    '        Case "Complete"
    '            GetImageID = 1
    '        Case "On Track"
    '            GetImageID = 2
    '        Case "At Risk"
    '            GetImageID = 0
    '        Case "Not Started"
    '            GetImageID = 3
    '        Case "Off Track"
    '            GetImageID = 4
    '        Case "On Hold"
    '            GetImageID = 5
    '        Case Else
    '            GetImageID = -1
    '    End Select

    'End Function

    'Public Sub FillRoundedRectangle(ByVal r As Rectangle, ByVal d As Integer, ByVal b As Brush, pnl As Panel)

    '    'Public Sub FillRoundedRectangle(ByVal g As Drawing.Graphics, ByVal r As Rectangle, ByVal d As Integer, ByVal b As Brush)
    '    Dim g As Drawing.Graphics = pnl.CreateGraphics()
    '    Dim mode As Drawing2D.SmoothingMode = g.SmoothingMode
    '    g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias 'System.Drawing.Drawing2D.SmoothingMode.HighSpeed

    '    'Rounded Corners - only if big enough
    '    If r.Width > 10 Then
    '        g.FillPie(b, r.X, r.Y, d, d, 178, 360)
    '        g.FillPie(b, r.X + r.Width - d, r.Y, d, d, 268, 360)
    '        g.FillPie(b, r.X, r.Y + r.Height - d, d, d, 88, 360)
    '        g.FillPie(b, r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 360)
    '    Else
    '        g.FillRectangle(b, r) 'Top
    '    End If

    '    'Filled Rectangle Middle
    '    '           (brush),       x,            y,        Width,              Height
    '    g.FillRectangle(b, (CInt(r.X + d / 2)) - 1, (r.Y), (r.Width - d) + 1, CInt(d / 2) + 2) 'Top
    '    g.FillRectangle(b, r.X, CInt(r.Y + d / 2), r.Width, CInt(r.Height - d) + 2) 'Middle
    '    g.FillRectangle(b, (CInt(r.X + d / 2)) - 1, CInt(r.Y + r.Height - d / 2), CInt(r.Width - d) + 1, CInt(d / 2)) 'Bottom
    '    g.SmoothingMode = mode

    'End Sub

    'Public Function Get_Font_Size(g As Graphics, txt As String, r As Rectangle)
    '    Dim font_size As Single
    '    Dim new_font As Font
    '    'Dim X As Single
    '    'Dim Y As Single
    '    Dim StringSize As New SizeF

    '    For font_size = 9.75 To 30 Step 0.75
    '        ' Get the next font.
    '        new_font = New Font("Verdana", font_size, FontStyle.Bold)

    '        ' See if we have room.
    '        'MsgBox("H: " & new_font.Height & " / " & r.Height & "  ||  W: " & _
    '        '       TextRenderer.MeasureText(ds.Tables("dtStartUp").Rows(i).Item("Name"), new_font).ToString & _
    '        '       " / " & r.Width)
    '        StringSize = TextRenderer.MeasureText(txt, new_font)
    '        If font_size > r.Height Or StringSize.Width > r.Width Then
    '            'If new_font.Height < r.Height Then
    '            '' Start a new column.
    '            'Y = 0
    '            'X += g.MeasureString(txt, new_font).Width

    '            'step back 2 sizes to give the text a buffer to the borders of the rectangle
    '            font_size = font_size - 3
    '            Exit For
    '            '' Draw the text.
    '            'g.DrawString(txt, _
    '            '    new_font, Brushes.Black, X, Y)
    '            'Y += new_font.Height
    '        End If

    '    Next font_size

    '    Return font_size

    'End Function
End Module
