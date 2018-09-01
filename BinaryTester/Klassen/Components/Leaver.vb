﻿Public Class Leaver
    Inherits ComponentBase

    Public Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.lever
        End Get
    End Property

    Public Property State As Boolean = False

    Public A As New DataPoint((COMP_WIDTH / 2), 0, Me, True)

    Protected Overrides Function GetInputs() As Guid()
        Return {}
    End Function

    Protected Overrides Function GetOutputs() As Guid()
        Return {A.Key}
    End Function

    Public Overrides Sub Update()
        A.State = State
    End Sub

    Protected Overrides Sub DrawInner(G As Graphics, B As Rectangle)
        Dim InnerBounds As New Rectangle(B.X + 20, B.Y + 10, B.Width - 40, B.Height - 20)
        If A.State Then
            G.FillRectangle(Brushes.Red, InnerBounds)
        Else
            G.FillRectangle(Brushes.White, InnerBounds)
        End If
        G.DrawRectangle(Pens.Black, InnerBounds)
    End Sub

    Friend Overrides Sub OnClick()
        Me.State = Not Me.State
    End Sub

End Class