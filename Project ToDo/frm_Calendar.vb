Imports System.Data.SqlClient

Public Class frm_Calendar
    Public GettingNewDateRange As Boolean = False
    Public SelectedDate As Date
    Public dtCtrl As Control

    Private Sub frm_Calendar_Deactivate(sender As Object, e As EventArgs) Handles Me.Deactivate
        If GettingNewDateRange = False Then
            Me.Close()
            frm_Main.Activate()
        End If
    End Sub

    Private Sub frm_Calendar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Center_Form_to_Main(Me)
        GettingNewDateRange = False
    End Sub

    Private Sub frm_Calendar_LostFocus(sender As Object, e As EventArgs) Handles Me.LostFocus

    End Sub

    Private Sub MonthCalendar1_DateChanged(sender As Object, e As DateRangeEventArgs) Handles MonthCalendar1.DateChanged
        Select Case frm_Main.tab_Main.SelectedTab.Text
            Case "Weekly Plan"
                GettingNewDateRange = True
                Call Get_DateRange()
            Case "Setup" 'OffDays
                SelectedDate = MonthCalendar1.SelectionStart.ToShortDateString
        End Select

    End Sub

    Sub Get_DateRange()
        Dim Firstday As DateTime = MonthCalendar1.SelectionStart.AddDays(-CInt(MonthCalendar1.SelectionStart.DayOfWeek) + 1)
        Dim Endaday As DateTime = Firstday.AddDays(6)

        Dim ans As MsgBoxResult = MsgBox("Are you Sure you want to add Date Range " & Firstday.ToShortDateString & " - " & Endaday.ToShortDateString & "", MsgBoxStyle.YesNoCancel, "Add Date Range")

        If ans = MsgBoxResult.Yes Then

            Dim uCmd As New SqlCommand
            'Add Week Range to Weekly Table tbl_ToDoWeekly
            uCmd.CommandText = "SET TRANSACTION ISOLATION LEVEL SERIALIZABLE BEGIN TRANSACTION " & _
                            "IF NOT EXISTS (SELECT 1 FROM tbl_ToDoWeekly WHERE userID=@Usr AND wkRange=@WkRg) " & _
                            "BEGIN INSERT INTO tbl_ToDoWeekly (userID, wkRange, wkStart, wkEnd, wkNotes) " & _
                            "   VALUES (@Usr, @WkRg, @WkSt, @WkEnd, @Nt)      END " & _
                            "COMMIT TRANSACTION "

            uCmd.Parameters.AddWithValue("@Usr", frm_Main.ss_User.Text)
            uCmd.Parameters.AddWithValue("@WkRg", Firstday.ToShortDateString & " - " & Endaday.ToShortDateString)
            uCmd.Parameters.AddWithValue("@WkSt", Firstday)
            uCmd.Parameters.AddWithValue("@WkEnd", Endaday)
            uCmd.Parameters.AddWithValue("@Nt", DBNull.Value)
            Call WriteUpdateSQL(uCmd)
            uCmd.Parameters.Clear()

            'Add Daily Items
            uCmd.CommandText = "SET TRANSACTION ISOLATION LEVEL SERIALIZABLE BEGIN TRANSACTION " & _
                            "IF NOT EXISTS (SELECT 1 FROM tbl_ToDoDaily WHERE userID=@Usr AND wkRange=@WkRg AND wkDate=@WkDt) " & _
                            "BEGIN INSERT INTO tbl_ToDoDaily (userID, wkRange, wkDate, wkDailyNote) " & _
                            "   VALUES (@Usr, @WkRg, @WkDt, @Nt)      END " & _
                            "COMMIT TRANSACTION "

            Dim cDt As Date = Firstday 'Current Date adding
            Do Until cDt = Endaday.AddDays(1)
                uCmd.Parameters.AddWithValue("@Usr", frm_Main.ss_User.Text)
                uCmd.Parameters.AddWithValue("@WkRg", Firstday.ToShortDateString & " - " & Endaday.ToShortDateString)
                uCmd.Parameters.AddWithValue("@WkDt", cDt)
                uCmd.Parameters.AddWithValue("@Nt", DBNull.Value)
                Call WriteUpdateSQL(uCmd)
                uCmd.Parameters.Clear()
                cDt = cDt.AddDays(1)
            Loop
            frm_Main.dtDaily.Clear() 'this is so the datatables will refresh
            Call frm_Main.Load_WeeklyPlan()
        End If

        GettingNewDateRange = False
        Me.Close()

    End Sub

    Private Sub MonthCalendar1_DateSelected(sender As Object, e As DateRangeEventArgs) Handles MonthCalendar1.DateSelected
        Select Case frm_Main.tab_Main.SelectedTab.Text
            Case "Off Days" 'OffDays
                dtCtrl.Text = MonthCalendar1.SelectionStart.ToShortDateString
                dtCtrl = Nothing
                Me.Close()
        End Select
    End Sub
End Class