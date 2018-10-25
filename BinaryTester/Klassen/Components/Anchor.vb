Public Class Anchor
    Inherits ComponentBase

    Public Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.anchor_point
        End Get
    End Property

    Public I_T As New DataPoint(-10, -(COMP_HEIGHT / 2), Me)
    Public I_L As New DataPoint(-(COMP_WIDTH / 2), 10, Me)
    Public I_R As New DataPoint((COMP_WIDTH / 2), -10, Me)
    Public I_B As New DataPoint(10, (COMP_HEIGHT / 2), Me)
    Public O_T As New DataPoint(10, -(COMP_HEIGHT / 2), Me, True)
    Public O_L As New DataPoint(-(COMP_WIDTH / 2), -10, Me, True)
    Public O_R As New DataPoint((COMP_WIDTH / 2), 10, Me, True)
    Public O_B As New DataPoint(-10, (COMP_HEIGHT / 2), Me, True)

    Friend Overrides Function GetInputs() As Guid()
        Return {
            I_T.Key,
            I_L.Key,
            I_B.Key,
            I_R.Key
        }
    End Function

    Friend Overrides Function GetOutputs() As Guid()
        Return {
            O_T.Key,
            O_L.Key,
            O_B.Key,
            O_R.Key
        }
    End Function

    Public Overrides ReadOnly Property Symbol As Byte
        Get
            Return CByte(Asc("#"))
        End Get
    End Property

    Protected Overrides Sub OnCompile(Stream As IO.Stream, DataPoints As Dictionary(Of Guid, UShort))
    End Sub

    Public Overrides Sub Update()
        Dim P As Boolean = False
        For Each I As Guid In GetInputs()
            If DataPoint.Item(I).State Then
                P = True
                Exit For
            End If
        Next
        For Each O As Guid In GetOutputs()
            DataPoint.Item(O).State = P
        Next
    End Sub

    Protected Overrides Sub DrawInner(G As Graphics, B As Rectangle)
        G.DrawLine(Pens.Black, I_T.GetCenter, O_B.GetCenter)
        G.DrawLine(Pens.Black, O_T.GetCenter, I_B.GetCenter)
        G.DrawLine(Pens.Black, I_L.GetCenter, O_R.GetCenter)
        G.DrawLine(Pens.Black, O_L.GetCenter, I_R.GetCenter)
        G.FillEllipse(Brushes.Black, B.X + 18, B.Y + 18, 4, 4)
        G.FillEllipse(Brushes.Black, B.X + 38, B.Y + 18, 4, 4)
        G.FillEllipse(Brushes.Black, B.X + 18, B.Y + 38, 4, 4)
        G.FillEllipse(Brushes.Black, B.X + 38, B.Y + 38, 4, 4)
    End Sub

End Class
