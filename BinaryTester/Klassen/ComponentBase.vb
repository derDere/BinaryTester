﻿Public MustInherit Class ComponentBase
    Inherits Selectable

    <Newtonsoft.Json.JsonIgnore> _
    <Browsable(False)> _
    Public Overridable ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.slash
        End Get
    End Property

    Public Const MAX_DELAY As Integer = Int32.MaxValue
    Public Const COMP_WIDTH As Integer = 60
    Public Const COMP_HEIGHT As Integer = 60
    Public Shared ReadOnly COMP_FONT As New Font("Arial", 10)

    Public Sub New()
    End Sub

    Friend MustOverride Function GetInputs() As Guid()

    Friend MustOverride Function GetOutputs() As Guid()

    Public MustOverride ReadOnly Property Symbol As Byte

    Public MustOverride Sub Update()

    <Browsable(False)> _
    Public Property Position As New Point(COMP_WIDTH, COMP_HEIGHT)

    Public Function GetBounds() As Rectangle
        Dim Bounds As New Rectangle(
                                    Me.Position.X - CInt(COMP_WIDTH / 2) + frmMain.ScrollOffset.X,
                                    Me.Position.Y - CInt(COMP_HEIGHT / 2) + frmMain.ScrollOffset.Y,
                                    COMP_WIDTH,
                                    COMP_HEIGHT
                                )
        Return Bounds
    End Function

    Public Function GetAllDataPoints() As List(Of Guid)
        Dim L As New List(Of Guid)
        L.AddRange(Me.GetInputs)
        L.AddRange(Me.GetOutputs)
        Return L
    End Function

    Public Sub Compile(Stream As IO.Stream, DataPoints As Dictionary(Of Guid, UInt16))
        Stream.Write({Me.Symbol}, 0, 1)
        For Each DP As Guid In Me.GetInputs
            Stream.Write({Compiler.NEXT_PARAM}, 0, 1)
            Stream.Write(BitConverter.GetBytes(DataPoints(DP)), 0, 2)
        Next
        Stream.Write({Compiler.NEXT_PART}, 0, 1)
        For Each DP As Guid In Me.GetOutputs
            Stream.Write({Compiler.NEXT_PARAM}, 0, 1)
            Stream.Write(BitConverter.GetBytes(DataPoints(DP)), 0, 2)
        Next
        OnCompile(Stream, DataPoints)
        Stream.Write({Compiler.NEXT_COMMAND}, 0, 1)
    End Sub

    Protected MustOverride Sub OnCompile(Stream As IO.Stream, DataPoints As Dictionary(Of Guid, UInt16))

    Public Sub Draw(G As Graphics)
        Dim Bounds As Rectangle = GetBounds()
        If Me.IsSelected Then
            G.FillRectangle(Brushes.LightBlue, Bounds)
        Else
            G.FillRectangle(Brushes.White, Bounds)
        End If
        G.DrawRectangle(Pens.Black, Bounds)
        DrawInner(G, Bounds)
        For Each DPK As Guid In GetInputs()
            Dim DP As DataPoint = DataPoint.Item(DPK)
            DP.Draw(G)
        Next
        For Each DPK As Guid In GetOutputs()
            Dim DP As DataPoint = DataPoint.Item(DPK)
            DP.Draw(G)
        Next
    End Sub

    Protected Overridable Sub DrawInner(G As Graphics, B As Rectangle)
    End Sub

    Friend Overridable Sub OnClick()
    End Sub

    Public Sub RemoveFrom(AllComponents As List(Of ComponentBase))
        If Selectable.SelectedObject Is Me Then
            Selectable.SelectedObject = Nothing
        End If
        For Each DPK As Guid In Me.GetAllDataPoints
            DataPoint.Item(DPK).Remove()
        Next
        If AllComponents.Contains(Me) Then _
            AllComponents.Remove(Me)
    End Sub

End Class
