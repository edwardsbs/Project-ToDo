Public Class ctrl_MenuItem

    Private Sub ctrl_MenuItem_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub ctrl_MenuItem_MouseClick(sender As Object, e As MouseEventArgs) Handles Me.MouseClick, pan_Selected.MouseClick, panel_Tab.MouseClick, label_TabText.MouseClick, pic_Icon.MouseClick, lab_Index.MouseClick
        'MsgBox(Me.Name)
        MenuSelection(CInt(Me.Name))
    End Sub

    Private Sub panel_Tab_Paint(sender As Object, e As PaintEventArgs) Handles panel_Tab.Paint
        Dim g As Graphics = panel_Tab.CreateGraphics()

        Dim panelRect As Rectangle = panel_Tab.ClientRectangle

        Dim p1 As Point = New Point(panelRect.Left, panelRect.Top)  'top left
        Dim p2 As Point = New Point(panelRect.Right + 1, panelRect.Top)  'Top Right (was .Rigtht - 1, changed to +1 for overlap)
        Dim p3 As Point = New Point(panelRect.Left, panelRect.Bottom - 1)  'Bottom Left
        Dim p4 As Point = New Point(panelRect.Right + 1, panelRect.Bottom - 1)  'Bottom Right (was .Rigtht - 1, changed to +1 for overlap)

        Dim pen1 As Pen = New Pen(System.Drawing.Color.Gray)
        Dim pen2 As Pen = New Pen(System.Drawing.Color.Black)

        g.DrawLine(pen1, p1, p2)
        g.DrawLine(pen1, p1, p3)
        g.DrawLine(pen2, p2, p4)
        g.DrawLine(pen2, p3, p4)
        panel_Tab.Width = frm_Main.panel_Menu.Width - 1
    End Sub

    Private Sub pan_Selected_Paint(sender As Object, e As PaintEventArgs) Handles pan_Selected.Paint
        Dim g As Graphics = pan_Selected.CreateGraphics()

        Dim panelRect As Rectangle = pan_Selected.ClientRectangle

        Dim p1 As Point = New Point(panelRect.Left, panelRect.Top)  'top left
        Dim p2 As Point = New Point(panelRect.Right - 1, panelRect.Top)  'Top Right 
        Dim p3 As Point = New Point(panelRect.Left, panelRect.Bottom - 1)  'Bottom Left
        Dim p4 As Point = New Point(panelRect.Right - 1, panelRect.Bottom - 1)  'Bottom Right

        Dim pen2 As Pen = New Pen(System.Drawing.Color.Gray)
        Dim pen1 As Pen = New Pen(System.Drawing.Color.Black)

        g.DrawLine(pen1, p1, p2)
        g.DrawLine(pen1, p1, p3)
        g.DrawLine(pen2, p2, p4)
        g.DrawLine(pen2, p3, p4)
    End Sub

End Class
