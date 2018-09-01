Public Class Connection
    Inherits Selectable

    Public Shared AllConnections As New Dictionary(Of Guid, List(Of Connection))

    <Browsable(False)> _
    Public Property [From] As Guid
    <Browsable(False)> _
    Public Property [To] As Guid

    Public ReadOnly Property State As Boolean
        Get
            Return DataPoint.Item(Me.From).State
        End Get
    End Property

    Public Shared Sub Create([From] As Guid, [To] As Guid)
        If DataPoint.Item([From]).IsOutput = DataPoint.Item([To]).IsOutput Then Exit Sub
        If Not DataPoint.Item([From]).IsOutput Then
            Dim TMP = [From]
            [From] = [To]
            [To] = TMP
        End If
        Dim NewCon As New Connection([From], [To])
        If Not AllConnections.ContainsKey([To]) Then
            AllConnections.Add([To], New List(Of Connection))
        End If
        Dim IsIn As Boolean = False
        For Each Con As Connection In AllConnections([To])
            If Con.From = NewCon.From Then
                IsIn = True
                Exit For
            End If
        Next
        If Not IsIn Then _
            AllConnections([To]).Add(NewCon)
    End Sub

    Public Sub New()
    End Sub

    Private Sub New([From] As Guid, [To] As Guid)
        If Not DataPoint.Item([From]).IsOutput Then Throw New Exception("Verbindungen Müssen immer bei einem Output Beginnen!")
        Me.From = [From]
        Me.[To] = [To]
    End Sub

    Public Sub Update()
        Dim State As Boolean = False
        For Each Con As Connection In AllConnections(Me.To)
            State = State Or DataPoint.Item(Con.From).State
        Next
        DataPoint.Item(Me.To).State = State
    End Sub

    Public Sub Remove()
        Dim Kill As New List(Of Guid)
        For Each Key As Guid In AllConnections.Keys
            If (Key = Me.From) Or (Key = Me.To) Then
                If AllConnections(Key).Contains(Me) Then
                    AllConnections(Key).Remove(Me)
                    DataPoint.Item(Me.To).State = False
                    If AllConnections(Key).Count <= 0 Then
                        Kill.Add(Key)
                    End If
                    Exit For
                End If
            End If
        Next
        For Each Key As Guid In Kill
            AllConnections.Remove(Key)
        Next
    End Sub

End Class
