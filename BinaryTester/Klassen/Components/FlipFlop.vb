Public Class FlipFlop
    Inherits ComponentBase

    Public Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.rsflipflop
        End Get
    End Property

    Private LastA As Boolean = False
    Private LastB As Boolean = False

    Public A As New DataPoint(-(COMP_WIDTH / 2), -10, Me)
    Public B As New DataPoint(-(COMP_WIDTH / 2), 10, Me)
    Public C As New DataPoint((COMP_WIDTH / 2), -10, Me, True)
    Public D As New DataPoint((COMP_WIDTH / 2), 10, Me, True)

    Friend Overrides Function GetInputs() As Guid()
        Return {A.Key, B.Key}
    End Function

    Friend Overrides Function GetOutputs() As Guid()
        Return {C.Key, D.Key}
    End Function

    Public Overrides ReadOnly Property Symbol As Byte
        Get
            Return CByte(Asc("F"))
        End Get
    End Property

    Public Property Output As Boolean = False

    Public Overrides Sub Update()
        Dim PosA As Boolean = False
        Dim PosB As Boolean = False

        If (LastA <> A.State) And A.State Then
            PosA = True
        End If

        If (LastB <> B.State) And B.State Then
            PosB = True
        End If

        LastA = A.State
        LastB = B.State

        If PosA Then
            Output = True
        ElseIf PosB Then
            Output = False
        End If

        C.State = Output
        D.State = Not C.State
    End Sub

    Protected Overrides Sub DrawInner(G As Graphics, B As Rectangle)
        G.DrawString("S    O" & vbNewLine & "  FF  " & vbNewLine & "R    N", COMP_FONT, Brushes.Black, B.Location.Add(10, 10))
    End Sub

End Class
