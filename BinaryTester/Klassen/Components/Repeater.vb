Public Class Repeater
    Inherits ComponentBase

    Public Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.clock__arrow
        End Get
    End Property

    Private Class WorkItem
        Public State As Boolean
        Public Ticks As Integer
    End Class
    Private WorkList As New List(Of WorkItem)
    Private LastA As Boolean = False

    Public A As New DataPoint(-(COMP_WIDTH / 2), 0, Me)
    Public B As New DataPoint((COMP_WIDTH / 2), 0, Me, True)

    Private _Delay As Integer = 1
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

    Protected Overrides Function GetInputs() As Guid()
        Return {A.Key}
    End Function

    Protected Overrides Function GetOutputs() As Guid()
        Return {B.Key}
    End Function

    Public Overrides Sub Update()
        If A.State <> LastA Then
            WorkList.Add(New WorkItem With {.State = A.State, .Ticks = Delay})
            LastA = A.State
        End If
        For Each WI As WorkItem In WorkList
            WI.Ticks -= 1
        Next
        Dim KillList As New List(Of WorkItem)
        For Each WI As WorkItem In WorkList
            If WI.Ticks < 0 Then
                B.State = WI.State
                KillList.Add(WI)
            End If
        Next
        For Each WI As WorkItem In KillList
            WorkList.Remove(WI)
        Next
    End Sub

    Protected Overrides Sub DrawInner(G As Graphics, B As Rectangle)
        Dim Current As Integer = 0
        If WorkList.Count > 0 Then
            Current = WorkList(0).Ticks
        End If
        G.DrawString("-|>" & vbNewLine & "t: " & Current & "/" & Delay, COMP_FONT, Brushes.Black, B.Location.Add(10, 10))
    End Sub

End Class
