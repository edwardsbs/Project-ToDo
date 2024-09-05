Module Snippit
    Partial Public Class SnippingTool
        Inherits Form
        Public Shared Function Snip() As Image

            'Dim rc = Screen.PrimaryScreen.Bounds
            'Using bmp As New Bitmap(rc.Width, rc.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb)
            Using bmp As New Bitmap( _
                    Screen.AllScreens.Sum(Function(s As Screen) s.Bounds.Width),
                    Screen.AllScreens.Max(Function(s As Screen) s.Bounds.Height), System.Drawing.Imaging.PixelFormat.Format32bppPArgb)
                Using gr As Graphics = Graphics.FromImage(bmp)
                    gr.CopyFromScreen(0, 0, 0, 0, bmp.Size)
                End Using
                Using snipper = New SnippingTool(bmp)
                    If snipper.ShowDialog() = DialogResult.OK Then
                        Return snipper.Image
                    End If
                End Using
                Return Nothing
            End Using
        End Function

        Public Sub New(screenShot As Image)
            'InitializeComponent()
            Me.BackgroundImage = screenShot
            Me.ShowInTaskbar = False
            Me.FormBorderStyle = FormBorderStyle.None
            Me.WindowState = FormWindowState.Maximized
            Me.DoubleBuffered = True
        End Sub
        Public Property Image() As Image
            Get
                Return m_Image
            End Get
            Set(value As Image)
                m_Image = value
            End Set
        End Property
        Private m_Image As Image

        Private rcSelect As New Rectangle()
        Private pntStart As Point

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            ' Start the snip on mouse down
            If e.Button <> MouseButtons.Left Then
                Return
            End If
            pntStart = e.Location
            rcSelect = New Rectangle(e.Location, New Size(0, 0))
            Me.Invalidate()
        End Sub
        Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
            ' Modify the selection on mouse move
            If e.Button <> MouseButtons.Left Then
                Return
            End If
            Dim x1 As Integer = Math.Min(e.X, pntStart.X)
            Dim y1 As Integer = Math.Min(e.Y, pntStart.Y)
            Dim x2 As Integer = Math.Max(e.X, pntStart.X)
            Dim y2 As Integer = Math.Max(e.Y, pntStart.Y)
            rcSelect = New Rectangle(x1, y1, x2 - x1, y2 - y1)
            Me.Invalidate()
        End Sub
        Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
            ' Complete the snip on mouse-up
            If rcSelect.Width <= 0 OrElse rcSelect.Height <= 0 Then
                Return
            End If
            Image = New Bitmap(rcSelect.Width, rcSelect.Height)
            Using gr As Graphics = Graphics.FromImage(Image)
                gr.DrawImage(Me.BackgroundImage, New Rectangle(0, 0, Image.Width, Image.Height), rcSelect, GraphicsUnit.Pixel)
            End Using
            DialogResult = DialogResult.OK
        End Sub
        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            ' Draw the current selection
            Using br As Brush = New SolidBrush(Color.FromArgb(120, Color.White))
                Dim x1 As Integer = rcSelect.X
                Dim x2 As Integer = rcSelect.X + rcSelect.Width
                Dim y1 As Integer = rcSelect.Y
                Dim y2 As Integer = rcSelect.Y + rcSelect.Height
                e.Graphics.FillRectangle(br, New Rectangle(0, 0, x1, Me.Height))
                e.Graphics.FillRectangle(br, New Rectangle(x2, 0, Me.Width - x2, Me.Height))
                e.Graphics.FillRectangle(br, New Rectangle(x1, 0, x2 - x1, y1))
                e.Graphics.FillRectangle(br, New Rectangle(x1, y2, x2 - x1, Me.Height - y2))
            End Using
            Using pen As New Pen(Color.Red, 3)
                e.Graphics.DrawRectangle(pen, rcSelect)
            End Using
        End Sub
        Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
            ' Allow canceling the snip with the Escape key
            If keyData = Keys.Escape Then
                Me.DialogResult = DialogResult.Cancel
            End If
            Return MyBase.ProcessCmdKey(msg, keyData)
        End Function
    End Class
End Module
