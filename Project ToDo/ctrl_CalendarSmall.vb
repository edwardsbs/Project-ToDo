Public Class ctrl_CalendarSmall
    Public DateRef As Date = Nothing

    Private Sub ctrl_CalendarSmall_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If DateRef = Nothing Then DateRef = Date.Today

        Me.lbl_Today.Text = Today.ToString("ddd, MMM dd")
        Me.lbl_Current.Text = DateRef.ToString("MMM yyyy")

        'LoadCalendar()

    End Sub

    Sub LoadCalendar()

        Me.SuspendLayout()

        'Clear Calendar Controls
        For index As Integer = pnl_CalDays.Controls.Count - 1 To 0 Step -1
            If Not TypeOf pnl_CalDays.Controls.Item(index) Is Label Then
                pnl_CalDays.Controls.Item(index).Dispose()
            End If
        Next
        'pnl_CalDays.Refresh()

        'Create Date Range of 35 days (5 wks of 7) starting with Sunday prior to the 1st day of the month
        Dim DateStart As Date = Nothing
        Dim DateEnd As Date = Nothing
        GetDateRange(DateRef, DateStart, DateEnd)

        'Create the Day Controls
        Dim Top As Integer = 15
        Dim Lft As Integer = 0
        Dim dCnt As Integer = 1
        Dim ReachedEnd As Boolean = False

        For Each xDay As Date In Enumerable.Range(0, (DateEnd - DateStart).Days).Select(Function(i) DateStart.AddDays(i))
            ' Enumerable.Range(0, (DateMax - DateMin).Days).Select(Function(i) DateMin.AddDays(i))
            Dim cDay As New ctrl_CalendarDay
            cDay.ctrDay = xDay
            cDay.DateRef = DateRef
            cDay.Top = Top
            cDay.Left = Lft
            If xDay = LastDayOfMonth(DateRef) Then ReachedEnd = True

            'Add the Control
            pnl_CalDays.Controls.Add(cDay)

            'Setup for next Day Control
            '------------------------------------------------
            'Reset if day count has reached 7
            dCnt += 1
            Lft = Lft + cDay.Width
            If dCnt = 8 Then 'Check if End of Week, Start New Week if Necessary
                If ReachedEnd = True Then Exit For
                dCnt = 1
                Lft = 0
                Top = Top + cDay.Height
            End If
            '------------------------------------------------
        Next xDay

        Me.ResumeLayout()

    End Sub

    Sub GetDateRange(DateRef As Date, ByRef DateStart As Date, ByRef DateEnd As Date)

        'Find out what day of the week is the 1st and back up to the previous sunday
        DateStart = FirstDayOfMonth(DateRef)
        If Not DateStart.DayOfWeek = DayOfWeek.Sunday Then
            DateStart = DateStart.AddDays(0 - DateStart.DayOfWeek)
        End If

        'Add 34 days from the start date to get the end date
        DateEnd = DateStart.AddDays(45) 'ReachedEnd will stop it when it hits the correct # of weeks

    End Sub

    Sub DrawCircle(ctrl As Control)

        'get shortest length/width
        Dim size As Integer = If(ctrl.Width < ctrl.Height, ctrl.Width, ctrl.Height) - 1

        Dim myBrush = New System.Drawing.SolidBrush(Color.Fuchsia)
        ctrl.CreateGraphics.FillEllipse(myBrush, New Rectangle(1, 1, size, size))
        myBrush.Dispose()

    End Sub

    'Get the first day of the month
    Public Function FirstDayOfMonth(ByVal sourceDate As DateTime) As DateTime
        Return New DateTime(sourceDate.Year, sourceDate.Month, 1)
    End Function

    'Get the last day of the month
    Public Function LastDayOfMonth(ByVal sourceDate As DateTime) As DateTime
        Dim lastDay As DateTime = New DateTime(sourceDate.Year, sourceDate.Month, 1)
        Return lastDay.AddMonths(1).AddDays(-1)
    End Function

    Private Sub btn_MthPrev_Click(sender As Object, e As EventArgs) Handles btn_MthPrev.Click
        DateRef = DateRef.AddMonths(-1)
        'Me.lbl_Today.Text = DateRef.ToString("ddd, MMM dd")
        Me.lbl_Current.Text = DateRef.ToString("MMM yyyy")
        LoadCalendar()
    End Sub

    Private Sub btn_MthNext_Click(sender As Object, e As EventArgs) Handles btn_MthNext.Click
        'Add a month
        DateRef = DateRef.AddMonths(1)
        'Me.lbl_Today.Text = DateRef.ToString("ddd, MMM dd")
        Me.lbl_Current.Text = DateRef.ToString("MMM yyyy")
        'MsgBox(lbl_Current.Width & "  " & Me.Width)
        LoadCalendar()
    End Sub

    Private Sub lbl_Today_Click(sender As Object, e As EventArgs) Handles lbl_Today.Click
        'set back to today
        DateRef = Today
        Me.lbl_Today.Text = Today.ToString("ddd, MMM dd")
        'Me.lbl_Today.Text = DateRef.ToString("ddd, MMM dd")
        Me.lbl_Current.Text = DateRef.ToString("MMM yyyy")
        LoadCalendar()
    End Sub

End Class
