Public Class Portal
    Inherits ComponentBase

    Public Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.portal
        End Get
    End Property

    Public I As New DataPoint(-(COMP_WIDTH / 2), 0, Me)
    Public O As New DataPoint((COMP_WIDTH / 2), 0, Me, True)

    Private Class PortalGroup
        Public Shared Index As Integer = 1
        Public Name As String
        Public Portals As New List(Of Portal)
        Public Color As Color
    End Class
    Private Shared AllPortals As New Dictionary(Of Color, PortalGroup)
    Private _PortalColor As Color
    Public Property PortalColor As Color
        Get
            Return _PortalColor
        End Get
        Set(value As Color)
            Dim OldPortalGroup As PortalGroup = Nothing
            If AllPortals.ContainsKey(_PortalColor) Then OldPortalGroup = AllPortals(_PortalColor)
            If value <> _PortalColor Then
                If OldPortalGroup IsNot Nothing Then
                    OldPortalGroup.Portals.Remove(Me)
                    If OldPortalGroup.Portals.Count <= 0 Then
                        AllPortals.Remove(OldPortalGroup.Color)
                    End If
                End If
                _PortalColor = value
                If Not AllPortals.ContainsKey(_PortalColor) Then
                    Dim newGroup As New PortalGroup
                    newGroup.Color = _PortalColor
                    newGroup.Name = "Group" & PortalGroup.Index
                    PortalGroup.Index += 1
                    AllPortals.Add(_PortalColor, newGroup)
                End If
                AllPortals(_PortalColor).Portals.Add(Me)
            End If
        End Set
    End Property

    Public Property GroupName() As String
        Get
            If AllPortals.ContainsKey(PortalColor) Then
                Return AllPortals(PortalColor).Name
            End If
            Return "<Unknown>"
        End Get
        Set(ByVal value As String)
            If AllPortals.ContainsKey(PortalColor) Then
                AllPortals(PortalColor).Name = value
            End If
        End Set
    End Property

    Public Sub New()
        MyBase.New()
        Me.PortalColor = Drawing.Color.DeepSkyBlue
    End Sub

    Protected Overrides Function GetInputs() As Guid()
        Return {I.Key}
    End Function

    Protected Overrides Function GetOutputs() As Guid()
        Return {O.Key}
    End Function

    Public Overrides Sub Update()
        Dim P As Boolean = False
        Dim Group As PortalGroup = Nothing
        If AllPortals.ContainsKey(PortalColor) Then _
            Group = AllPortals(PortalColor)
        If Group IsNot Nothing Then
            For Each Por As Portal In Group.Portals
                If Por.I.State Then
                    P = True
                    Exit For
                End If
            Next
        End If
        O.State = P
    End Sub

    Protected Overrides Sub DrawInner(G As Graphics, B As Rectangle)
        Dim InnerBounds As New Rectangle(B.X + 20, B.Y + 10, 20, 40)
        G.FillEllipse(Brushes.White, InnerBounds.X - 2, InnerBounds.Y - 2, InnerBounds.Width + 4, InnerBounds.Height + 4)
        G.FillEllipse(New SolidBrush(Drawing.Color.FromArgb(128, PortalColor)), InnerBounds)
        G.DrawEllipse(New Pen(PortalColor, 3), InnerBounds)
        Dim Size As SizeF = G.MeasureString(GroupName, COMP_FONT)
        G.DrawString(GroupName, COMP_FONT, Brushes.Black, B.X + ((B.Width - Size.Width) / 2), B.Y + 40)
    End Sub

End Class
