Public Class Extender
    Inherits ComponentBase

    Public Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.clock_select_remain
        End Get
    End Property

    'Private Class WorkItem
    '    Public Delay As Integer
    '    Public Ticks As Integer
    'End Class
    'Private WorkList As New List(Of WorkItem)
    Private DelayCount As Integer = 0
    Private PulseCount As Integer = 0
    Private LastA As Boolean = False

    Public A As New DataPoint(-(COMP_WIDTH / 2), 0, Me)
    Public B As New DataPoint((COMP_WIDTH / 2), 0, Me, True)

    Private _Delay As Integer = 0
    Public Property Delay() As Integer
        Get
            Return _Delay
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then value = 0
            If value > MAX_DELAY Then value = MAX_DELAY
            _Delay = value
        End Set
    End Property

    Private _PulseLength As Integer = 4
    Public Property PulseLength() As Integer
        Get
            Return _PulseLength
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then value = 0
            If value > MAX_DELAY Then value = MAX_DELAY
            _PulseLength = value
        End Set
    End Property

    Friend Overrides Function GetInputs() As Guid()
        Return {A.Key}
    End Function

    Friend Overrides Function GetOutputs() As Guid()
        Return {B.Key}
    End Function

    Public Overrides ReadOnly Property Symbol As Byte
        Get
            Return CByte(Asc("-"))
        End Get
    End Property

    Private WentTrou As Boolean = True
    Public Overrides Sub Update()
        Dim Pos As Boolean = False
        If (A.State <> LastA) Then
            LastA = A.State
            If A.State Then _
                Pos = True
        End If
        If A.State And WentTrou Then
            PulseCount = PulseLength
        End If
        If Pos And (DelayCount <= 0) And WentTrou Then
            DelayCount = Delay
            WentTrou = False
        End If

        B.State = (DelayCount <= 0) And (PulseCount > 0)

        If (PulseCount > 0) And (DelayCount <= 0) Then _
            PulseCount -= 1
        If DelayCount > 0 Then _
            DelayCount -= 1

        If (DelayCount = 0) And (PulseCount = 0) Then _
            WentTrou = True
    End Sub

    Protected Overrides Sub DrawInner(G As Graphics, B As Rectangle)
        G.DrawString("-->" & vbNewLine & "s: " & Delay & "/" & PulseLength & vbNewLine & "t: " & DelayCount & "/" & PulseCount, COMP_FONT, Brushes.Black, B.Location.Add(10, 10))
    End Sub

End Class
