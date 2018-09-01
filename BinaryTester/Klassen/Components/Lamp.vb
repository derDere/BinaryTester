Public Class Lamp
    Inherits ComponentBase

    Public Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.light_bulb
        End Get
    End Property

    Public A As New DataPoint(-(COMP_WIDTH / 2), 0, Me)

    Public ReadOnly Property Power As Boolean
        Get
            Return A.State
        End Get
    End Property

    Protected Overrides Function GetInputs() As Guid()
        Return {A.Key}
    End Function

    Protected Overrides Function GetOutputs() As Guid()
        Return {}
    End Function

    Public Overrides Sub Update()
    End Sub

    Protected Overrides Sub DrawInner(G As Graphics, B As Rectangle)
        Dim InnerBounds As New Rectangle(B.X + 10, B.Y + 10, B.Width - 20, B.Height - 20)
        If A.State Then
            G.FillEllipse(Brushes.Red, InnerBounds)
        Else
            G.FillEllipse(Brushes.White, InnerBounds)
        End If
        G.DrawEllipse(Pens.Black, InnerBounds)

        Dim P11 As New Point(B.X + 16, B.Y + 16)
        Dim P12 As New Point(B.X + B.Width - 16, B.Y + B.Height - 16)
        Dim P21 As New Point(B.X + B.Width - 16, B.Y + 16)
        Dim P22 As New Point(B.X + 16, B.Y + B.Height - 16)

        G.DrawLine(Pens.Black, P11, P12)
        G.DrawLine(Pens.Black, P21, P22)
    End Sub

End Class
