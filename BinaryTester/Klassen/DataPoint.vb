Public Class DataPoint

    Public Shared ReadOnly AllDataPoints As New Dictionary(Of Guid, DataPoint)

    Public Shared ReadOnly Property Item(Key As Guid) As DataPoint
        Get
            If AllDataPoints.ContainsKey(Key) Then
                Return AllDataPoints(Key)
            End If
            Return Nothing
        End Get
    End Property

    Public Property State As Boolean = False

    Public Property IsOutput As Boolean

    Public Property Offset As Point

    <Newtonsoft.Json.JsonIgnore> _
    Public Property Parent As ComponentBase

    Private _Key As Guid? = Nothing
    Public Property Key As Guid
        Get
            Return _Key
        End Get
        Set(value As Guid)
            If value <> _Key Then
                If AllDataPoints.ContainsKey(_Key) Then
                    AllDataPoints.Remove(_Key)
                End If
                _Key = value
                AddToDataPointList()
            End If
        End Set
    End Property

    Public Sub New()
    End Sub

    Public Sub New(Offset As Point, Parent As ComponentBase, Optional IsOutput As Boolean = False)
        Init(Offset, Parent, IsOutput)
    End Sub

    Public Sub New(OffX As Integer, OffY As Integer, Parent As ComponentBase, Optional IsOutput As Boolean = False)
        Init(New Point(OffX, OffY), Parent, IsOutput)
    End Sub

    Private Sub Init(Offset As Point, Parent As ComponentBase, IsOutput As Boolean)
        Me._Key = Guid.NewGuid
        Me._Offset = Offset
        Me._Parent = Parent
        Me._IsOutput = IsOutput
        AddToDataPointList()
    End Sub

    Private Sub AddToDataPointList()
        AllDataPoints.Add(Me.Key, Me)
    End Sub

    Public Function GetBounds() As Rectangle
        Dim Bounds As New Rectangle(Me.Parent.Position.X + Me.Offset.X - 5 + frmMain.ScrollOffset.X, Me.Parent.Position.Y + Me.Offset.Y - 5 + frmMain.ScrollOffset.Y, 10, 10)
        Return Bounds
    End Function

    Public Function GetCenter() As Point
        Dim Center As New Point(Me.Parent.Position.X + Me.Offset.X + frmMain.ScrollOffset.X, Me.Parent.Position.Y + Me.Offset.Y + frmMain.ScrollOffset.Y)
        Return Center
    End Function

    Public Sub Remove()
        Dim Kill As New List(Of Connection)
        For Each Key As Guid In Connection.AllConnections.Keys
            For Each Con As Connection In Connection.AllConnections(Key)
                If (Con.To = Me.Key) Or (Con.From = Me.Key) Then
                    Kill.Add(Con)
                End If
            Next
        Next
        For Each Con As Connection In Kill
            Con.Remove()
        Next
        AllDataPoints.Remove(Me.Key)
    End Sub

    Public Sub Draw(G As Graphics, Optional IsConnecting As Boolean = False)
        Dim dpb As Rectangle = Me.GetBounds
        If IsConnecting Then
            If IsOutput Then
                G.DrawEllipse(frmMain.ConnectingPen, dpb)
            Else
                G.DrawRectangle(frmMain.ConnectingPen, dpb)
            End If
        Else
            If IsOutput Then
                If Me.State Then
                    G.FillEllipse(Brushes.Red, dpb)
                Else
                    G.FillEllipse(Brushes.White, dpb)
                End If
                G.DrawEllipse(Pens.Black, dpb)
            Else
                If Me.State Then
                    G.FillRectangle(Brushes.Red, dpb)
                Else
                    G.FillRectangle(Brushes.White, dpb)
                End If
                G.DrawRectangle(Pens.Black, dpb)
            End If
        End If
    End Sub

End Class
