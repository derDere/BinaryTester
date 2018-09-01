Imports System.Runtime.CompilerServices

Module Utilities

    <Extension> _
    Public Function Clone(ByRef P As Point) As Point
        P = New Point(P.X, P.Y)
        Return P
    End Function

    <Extension> _
    Public Function Add(ByRef P As Point, X As Integer, Y As Integer) As Point
        P = New Point(P.X + X, P.Y + Y)
        Return P
    End Function

    <Extension> _
    Public Function IsIn(P As Point, R As Rectangle) As Boolean
        If P.X >= R.X _
        And P.Y >= R.Y _
        And P.X < (R.X + R.Width) _
        And P.Y < (R.Y + R.Height) Then
            Return True
        End If
        Return False
    End Function

    <Extension> _
    Public Function Contains(R As Rectangle, P As Point) As Boolean
        Return IsIn(P, R)
    End Function

    Public Function LineIntersectsRect(ByVal p1 As Point, ByVal p2 As Point, ByVal r As Rectangle) As Boolean
        Return LineIntersectsLine(p1, p2, New Point(r.X, r.Y), New Point(r.X + r.Width, r.Y)) OrElse LineIntersectsLine(p1, p2, New Point(r.X + r.Width, r.Y), New Point(r.X + r.Width, r.Y + r.Height)) OrElse LineIntersectsLine(p1, p2, New Point(r.X + r.Width, r.Y + r.Height), New Point(r.X, r.Y + r.Height)) OrElse LineIntersectsLine(p1, p2, New Point(r.X, r.Y + r.Height), New Point(r.X, r.Y)) OrElse (r.Contains(p1) AndAlso r.Contains(p2))
    End Function

    Private Function LineIntersectsLine(ByVal l1p1 As Point, ByVal l1p2 As Point, ByVal l2p1 As Point, ByVal l2p2 As Point) As Boolean
        Dim q As Single = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y)
        Dim d As Single = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X)

        If d = 0 Then
            Return False
        End If

        Dim r As Single = q / d
        q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y)
        Dim s As Single = q / d

        If r < 0 OrElse r > 1 OrElse s < 0 OrElse s > 1 Then
            Return False
        End If

        Return True
    End Function

End Module
