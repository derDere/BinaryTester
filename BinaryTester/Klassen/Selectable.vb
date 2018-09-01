Public MustInherit Class Selectable

    Public Shared Event SelectionChanged()

    Public Shared Property SelectedObject As Selectable = Nothing

    <Newtonsoft.Json.JsonIgnore> _
    <Browsable(False)> _
    Public Property IsSelected As Boolean
        Get
            Return SelectedObject Is Me
        End Get
        Set(value As Boolean)
            Dim OldSelection As Selectable = SelectedObject
            If value = True Then
                SelectedObject = Me
            Else
                SelectedObject = Nothing
            End If
            If SelectedObject IsNot OldSelection Then
                RaiseEvent SelectionChanged()
            End If
        End Set
    End Property

End Class
