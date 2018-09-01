Public Class Ticker
    Inherits ComponentBase

    Public Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.alarm_clock_blue
        End Get
    End Property

    Private _Delay As Integer = 1
    Public Property Delay As Integer
        Get
            Return _Delay
        End Get
        Set(value As Integer)
            If value < 0 Then value = 0
            If value > MAX_DELAY Then value = MAX_DELAY
            _Delay = value
        End Set
    End Property

    Private _PulseLength As Integer = 1
    Public Property PulseLength As Integer
        Get
            Return _PulseLength
        End Get
        Set(value As Integer)
            If value < 0 Then value = 0
            If value > MAX_DELAY Then value = MAX_DELAY
            _PulseLength = value
        End Set
    End Property

    Private TickStep As Integer = 1
    Private TickCounter As Integer = 0

    Public A As New DataPoint((COMP_WIDTH / 2), 0, Me, True)

    Protected Overrides Function GetInputs() As Guid()
        Return {}
    End Function

    Protected Overrides Function GetOutputs() As Guid()
        Return {A.Key}
    End Function

    Public Overrides Sub Update()
        A.State = (TickStep = -1)
        TickCounter += TickStep
        If TickCounter >= Delay And TickStep = 1 Then
            TickCounter = PulseLength
            TickStep = -1
        End If
        If TickCounter <= 0 And TickStep = -1 Then
            TickCounter = 0
            TickStep = 1
        End If
    End Sub

    Protected Overrides Sub DrawInner(G As Graphics, B As Rectangle)
        G.DrawString("C" & vbNewLine & "t: " & TickCounter & "/" & Delay & "," & PulseLength, COMP_FONT, Brushes.Black, B.Location.Add(5, 10))
    End Sub

End Class
