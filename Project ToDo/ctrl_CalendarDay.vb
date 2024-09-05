Imports System.Globalization
Imports System.Reflection


Public Class ctrl_CalendarDay
    Public ctrDay As Date
    Public DateRef As Date = Nothing
    Public Selected As Boolean = False


    Private Sub ctrl_CalendarDay_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Sub ctrl_CalendarDay_Click(sender As Object, e As EventArgs) Handles Me.Click

        Call ChangeSelectedDay()
        Call frm_Main.Load_WeeklyPlanCal()

    End Sub

    Sub DrawCircle(ctrl As Control, clr As Color)
        Dim g As Graphics = ctrl.CreateGraphics
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

        'get shortest length/width
        Dim size As Integer = If(ctrl.Width < ctrl.Height, ctrl.Width, ctrl.Height) - 4

        Dim myBrush = New System.Drawing.SolidBrush(clr)
        g.FillEllipse(myBrush, New Rectangle(2, 2, size, size))
        myBrush.Dispose()
        g.Dispose()

    End Sub

    Private Sub ctrl_CalendarDay_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        Me.SuspendLayout()

        'Variables
        '----------
        Dim CalFont As Font = New Drawing.Font("Verdana", 9, FontStyle.Regular)
        Dim format As New StringFormat()
        format.Alignment = StringAlignment.Center
        format.LineAlignment = StringAlignment.Center
        Dim clrCircle As Color = Color.White
        '----------


        'Set Colors and Fonts
        Dim br As SolidBrush = New SolidBrush(Color.Black)
        If Selected = True Then clrCircle = Color.White
        If ctrDay = Today Then
            br = New SolidBrush(Color.White)
            CalFont = New Drawing.Font("Verdana", 10, FontStyle.Bold)
            clrCircle = Color.Red
        End If

        Dim ctrCal As ctrl_CalendarSmall = Me.Parent.Parent
        If ctrDay = ctrCal.DateRef Then Me.BackColor = Color.Blue Else Me.BackColor = Color.Transparent

        If Not ctrDay.Month = DateRef.Month Then br = New SolidBrush(Color.LightGray)

        'Draw Graphics
        DrawCircle(Me, clrCircle)
        Me.CreateGraphics.DrawString(ctrDay.Day.ToString.PadLeft(2, "0"), CalFont, br,
                                       New Rectangle(0, 0, Me.Width - 1, Me.Height - 1), format)

        Me.ResumeLayout()
    End Sub

    Public Sub ChangeSelectedDay()

        Dim tmpSel As Boolean = If(Selected = True, False, True) 'Toggle Selected

        'Me.SuspendLayout()

        'Deal with if this day has been selected (clicked)
        'Clear previous selections first
        'If Selected = True Then
        '    DateRef = Me.ctrDay
        'End If
        'Go ahead and clear the color of all items before selecting the new item.
        '[THIS IS NOT EFFICIENT, BUT NOT CLEARING THE PREVIOUS SELECTION 1ST IS CAUSING A QUICK OVERLAP IN 2 ITEMS BEING SELECTED.
        ' IT IS NOT HURTING ANYTHING, BUT DOES NOT LOOK AESTHECIALLY PLEASING.
        ' THIS FIRST LOOP SEQUENCE WILL CHANGE ALL THE COLORS BACK TO UNSELECTED.  THEN NEXT LOOP WILL THEN SET THE COLOR AND 
        ' PERFORM THE ACTIONS BASED ON THE SELECTED DATE]
        For index As Integer = Me.Parent.Controls.Count - 1 To 0 Step -1
            If TypeOf Me.Parent.Controls.Item(index) Is ctrl_CalendarDay Then
                Dim ctrDay As ctrl_CalendarDay = CType(Me.Parent.Controls.Item(index), ctrl_CalendarDay)
                ctrDay.BackColor = Color.White
                ctrDay.Selected = False
            End If
        Next

        For index As Integer = Me.Parent.Controls.Count - 1 To 0 Step -1
            If TypeOf Me.Parent.Controls.Item(index) Is ctrl_CalendarDay Then
                Dim ctrDay As ctrl_CalendarDay = CType(Me.Parent.Controls.Item(index), ctrl_CalendarDay)

                If ctrDay.ctrDay = Me.ctrDay And tmpSel = True Then
                    ctrDay.Selected = True
                    Me.BackColor = Color.Blue
                    'Selected = tmpSel

                    Dim ctrCal As ctrl_CalendarSmall = Me.Parent.Parent
                    ctrCal.DateRef = Me.ctrDay
                    ctrCal.lbl_Today.Text = Me.ctrDay.ToString("ddd, MMM dd")

                    Call GetOutlook(Me.ctrDay)
                    'Else


                End If
                'Me.Parent.Controls.Item(index).BackColor = Color.White
            End If
        Next

        'Selected = tmpSel
        'If Selected = True Then
        '    Dim ctrCal As ctrl_CalendarSmall = Me.Parent.Parent
        '    ctrCal.DateRef = Me.ctrDay
        '    ctrCal.lbl_Today.Text = Me.ctrDay.ToString("ddd, MMM dd")

        '    'Me.Refresh()
        '    Call frm_Main.Load_WeeklyPlanCal()

        '    'Get Outlook Calendar Meetings
        '    Try
        '        Dim olApp As Outlook.Application
        '        olApp = CreateObject("Outlook.Application")
        '        Dim mpnNamespace As Outlook.NameSpace = olApp.GetNamespace("MAPI")
        '        Dim clfFolder As Outlook.Folder =
        '            mpnNamespace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar)

        '        'clfFolder.Items.IncludeRecurrences = False
        '        Dim strAppt As String = ""
        '        'Appointments
        '        For Each appItem As Outlook.AppointmentItem In clfFolder.Items
        '            If appItem.Start.Date = Me.ctrDay Then
        '                Dim ds As DateTime = appItem.Start.TimeOfDay.ToString
        '                Dim de As DateTime = appItem.End.TimeOfDay.ToString
        '                strAppt = strAppt & appItem.Subject & "   " & ds.ToString("h:mm tt") & " - " &
        '                       de.ToString("h:mm tt") & vbCrLf
        '                'MsgBox(appItem.Subject & "   " & ds.ToString("h:mm tt") & " - " &
        '                '       de.ToString("h:mm tt"))
        '            End If
        '        Next
        '        'Tasks
        '        clfFolder = mpnNamespace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderTasks)
        '        For Each appTask As Outlook.TaskItem In clfFolder.Items
        '            If appTask.Start.Date = Me.ctrDay Then
        '                Dim ds As DateTime = appTask.Start.TimeOfDay.ToString
        '                Dim de As DateTime = appTask.End.TimeOfDay.ToString
        '                strAppt = strAppt & appTask.Subject & "   " & ds.ToString("h:mm tt") & " - " &
        '                       de.ToString("h:mm tt") & vbCrLf
        '                'MsgBox(appItem.Subject & "   " & ds.ToString("h:mm tt") & " - " &
        '                '       de.ToString("h:mm tt"))
        '            End If
        '        Next
        '        'ToDo
        '        'clfFolder = mpnNamespace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderToDo)
        '        'For Each appTask As Outlook.to In clfFolder.Items
        '        '    If appTask.Start.Date = Me.ctrDay Then
        '        '        Dim ds As DateTime = appTask.Start.TimeOfDay.ToString
        '        '        Dim de As DateTime = appTask.End.TimeOfDay.ToString
        '        '        strAppt = appTask.Subject & "   " & ds.ToString("h:mm tt") & " - " &
        '        '               de.ToString("h:mm tt") & vbCrLf
        '        '        'MsgBox(appItem.Subject & "   " & ds.ToString("h:mm tt") & " - " &
        '        '        '       de.ToString("h:mm tt"))
        '        '    End If
        '        'Next

        '        If Not String.IsNullOrEmpty(strAppt) Then frm_Main.txt_Meetings.Text = strAppt Else frm_Main.txt_Meetings.Text = ""

        '        olApp = Nothing

        '    Catch ex As Exception
        '        MsgBox(ex.ToString)
        '    End Try

        'End If



        'Me.ResumeLayout()
    End Sub
End Class
