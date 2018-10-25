Public Class TFlipFlop
    Inherits ComponentBase

    Public Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.tflipflop
        End Get
    End Property

    Private LastA As Boolean = False

    Public A As New DataPoint(-(COMP_WIDTH / 2), 0, Me)
    Public B As New DataPoint((COMP_WIDTH / 2), -10, Me, True)
    Public C As New DataPoint((COMP_WIDTH / 2), 10, Me, True)

    Friend Overrides Function GetInputs() As Guid()
        Return {A.Key}
    End Function

    Friend Overrides Function GetOutputs() As Guid()
        Return {B.Key, C.Key}
    End Function

    Public Overrides ReadOnly Property Symbol As Byte
        Get
            Return CByte(Asc("T"))
        End Get
    End Property

    Public Property Output As Boolean = False

    Protected Overrides Sub OnCompile(Stream As IO.Stream, DataPoints As Dictionary(Of Guid, UShort))
    End Sub

    Public Overrides Sub Update()
        Dim PosA As Boolean = False

        If (LastA <> A.State) And A.State Then
            PosA = True
        End If

        LastA = A.State

        If PosA Then
            Output = Not Output
        End If

        B.State = Output
        C.State = Not B.State
    End Sub

    Protected Overrides Sub DrawInner(G As Graphics, B As Rectangle)
        G.DrawString("        O" & vbNewLine & "TFF" & vbNewLine & "        N", COMP_FONT, Brushes.Black, B.Location.Add(10, 10))
    End Sub

End Class
