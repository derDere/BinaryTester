Public Class ButtonComp
    Inherits ComponentBase

    Public Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.button
        End Get
    End Property

    Private _PressLength As Integer = 1
    Public Property PressLength() As Integer
        Get
            Return _PressLength
        End Get
        Set(ByVal value As Integer)
            If value < 1 Then value = 1
            If value > MAX_DELAY Then value = MAX_DELAY
            _PressLength = value
        End Set
    End Property

    Public Property Pressed As Boolean = False

    Private LastPressed As Boolean = False
    Private TickCount As Integer = 0

    Public A As New DataPoint((COMP_WIDTH / 2), 0, Me, True)

    Friend Overrides Function GetInputs() As Guid()
        Return {}
    End Function

    Friend Overrides Function GetOutputs() As Guid()
        Return {A.Key}
    End Function

    Public Overrides ReadOnly Property Symbol As Byte
        Get
            Return CByte(Asc("B"))
        End Get
    End Property

    Public Overrides Sub Update()
        If (LastPressed <> Pressed) And Pressed Then
            TickCount = _PressLength
        End If

        A.State = (TickCount > 0)
        Pressed = (TickCount > 0)

        LastPressed = Pressed

        If TickCount > 0 Then _
            TickCount -= 1
    End Sub

    Protected Overrides Sub DrawInner(G As Graphics, B As Rectangle)
        Dim InnerBounds As New Rectangle(B.X + 20, B.Y + 25, B.Width - 40, B.Height - 50)
        If A.State Then
            G.FillRectangle(Brushes.Red, InnerBounds)
        Else
            G.FillRectangle(Brushes.White, InnerBounds)
        End If
        G.DrawRectangle(Pens.Black, InnerBounds)
    End Sub

    Friend Overrides Sub OnClick()
        Me.Pressed = True
    End Sub

End Class
