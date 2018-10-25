Public Class Negate
    Inherits ComponentBase

    Public Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.logical_not
        End Get
    End Property

    Public A As New DataPoint(-(COMP_WIDTH / 2), 0, Me)
    Public B As New DataPoint((COMP_WIDTH / 2), 0, Me, True)

    Friend Overrides Function GetInputs() As Guid()
        Return {A.Key}
    End Function

    Friend Overrides Function GetOutputs() As Guid()
        Return {B.Key}
    End Function

    Public Overrides ReadOnly Property Symbol As Byte
        Get
            Return CByte(Asc("!"))
        End Get
    End Property

    Protected Overrides Sub OnCompile(Stream As IO.Stream, DataPoints As Dictionary(Of Guid, UShort))
    End Sub

    Public Overrides Sub Update()
        B.State = Not A.State
    End Sub

    Protected Overrides Sub DrawInner(G As Graphics, B As Rectangle)
        G.DrawString("!", COMP_FONT, Brushes.Black, B.Location.Add(10, 10))
    End Sub

End Class
