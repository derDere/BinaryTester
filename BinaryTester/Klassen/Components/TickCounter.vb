Public Class TickCounter
    Inherits ComponentBase

    Public Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.counter
        End Get
    End Property

    Private Counting As Boolean = False
    Private LastA As Boolean = False
    Public A As New DataPoint(-(COMP_WIDTH / 2), 0, Me)

    Private _Count As Integer = 0
    Public ReadOnly Property Count As Integer
        Get
            Return _Count
        End Get
    End Property

    Protected Overrides Function GetInputs() As Guid()
        Return {A.Key}
    End Function

    Protected Overrides Function GetOutputs() As Guid()
        Return {}
    End Function

    Public Overrides ReadOnly Property Symbol As Byte
        Get
            Return CByte(Asc("C"))
        End Get
    End Property

    Public Overrides Sub Update()
        Dim Pos As Boolean = False
        Dim Neg As Boolean = False

        If LastA <> A.State Then
            If A.State Then Pos = True
            If Not A.State Then Neg = True
        End If
        LastA = A.State

        If Pos Then
            _Count = 0
            Counting = True
        End If

        If Neg Then
            Counting = False
        End If

        If Counting Then _
            _Count += 1
    End Sub

    Protected Overrides Sub DrawInner(G As Graphics, B As Rectangle)
        G.DrawString("N++" & vbNewLine & "P: " & IIf(Counting, "1", "0") & vbNewLine & "N: " & Count, COMP_FONT, Brushes.Black, B.Location.Clone.Add(5, 5))
    End Sub

End Class
