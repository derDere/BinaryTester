Public Class frmMain

    Public Property ScrollOffset As New Point(0, 0)

    Private Shared ComponentTypes As Type() = {
        GetType(Leaver),
        GetType(Negate),
        GetType(AndComp),
        GetType(Lamp),
        GetType(XorComp),
        GetType(Repeater),
        GetType(Ticker),
        GetType(FlipFlop),
        GetType(TFlipFlop),
        GetType(ButtonComp),
        GetType(Anchor),
        GetType(Portal),
        GetType(Extender),
        GetType(TickCounter)
    }
    Friend Shared ReadOnly ConnectingPen As New Pen(Color.FromArgb(128, Color.Blue), 4)
    Friend Shared ReadOnly DraggingPen As New Pen(Color.FromArgb(128, Color.Orange), 4)
    Friend Shared ReadOnly SelectedPen As New Pen(Brushes.DeepSkyBlue, 10)
    Friend Shared ReadOnly ConPenOff As New Pen(Brushes.Gray, 5)
    Friend Shared ReadOnly ConPenOn As New Pen(Brushes.Red, 5)
    Private B As Bitmap

    Public Property AllComponents As New List(Of ComponentBase)
    Private ToolButtons As New Dictionary(Of Type, PictureBox)

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        For Each T As Type In ComponentTypes
            Dim B As New PictureBox
            ToolTip1.SetToolTip(B, T.Name.Replace("Comp", ""))
            B.Tag = T
            B.Width = 24
            B.Height = 24
            B.BackColor = Me.BackColor
            B.BorderStyle = BorderStyle.FixedSingle
            B.SizeMode = PictureBoxSizeMode.CenterImage
            B.Image = Activator.CreateInstance(T).Icon
            AddHandler B.MouseDown, AddressOf ButtonMouseDown
            FlowLayoutPanel1.Controls.Add(B)
            ToolButtons.Add(T, B)
        Next

        AddHandler Selectable.SelectionChanged, AddressOf Selectable_SelectionChanged

        B = New Bitmap(CanvasPic.Width, CanvasPic.Height)

        UpdateTicker.Start()
    End Sub

    Private NewObject As ComponentBase = Nothing
    Private Sub ButtonMouseDown(sender As Object, e As EventArgs)
        If TypeOf sender Is PictureBox Then
            Dim btn As PictureBox = sender
            If TypeOf btn.Tag Is Type Then
                Dim T As Type = btn.Tag
                Try
                    Dim Obj As ComponentBase = Activator.CreateInstance(T)
                    If NewObject IsNot Nothing Then _
                        ToolButtons(NewObject.GetType).BackColor = Me.BackColor
                    NewObject = Obj
                    btn.BackColor = Color.DeepSkyBlue
                Catch ex As Exception
                End Try
            End If
        End If
    End Sub

    Private Sub Selectable_SelectionChanged()
        PropertyGrid1.SelectedObject = Selectable.SelectedObject
    End Sub

    Private Sub UpdateTicker_Tick(sender As Object, e As EventArgs) Handles UpdateTicker.Tick
        UpdateTicker.Stop()

        'Update
        Update()

        'Draw
        Using G As Graphics = Graphics.FromImage(B)
            Draw(G)
        End Using
        CanvasPic.Image = B

        UpdateTicker.Start()
    End Sub

    Private Overloads Sub Update()
        'Update Connections
        For Each Key As Guid In Connection.AllConnections.Keys
            Connection.AllConnections(Key)(0).Update()
        Next

        'Update Components
        For Each Com As ComponentBase In AllComponents
            Com.Update()
        Next

        'Check Canvas Size
        If B.Width <> CanvasPic.Width OrElse B.Height <> CanvasPic.Height Then
            B.Dispose()
            B = New Bitmap(CanvasPic.Width, CanvasPic.Height)
        End If
    End Sub

    Private Sub Draw(G As Graphics)
        G.Clear(Color.White)
        G.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        'Draw Connections
        For Each Key In Connection.AllConnections.Keys
            For Each Con As Connection In Connection.AllConnections(Key)
                Dim P1 As Point = DataPoint.Item(Con.From).GetCenter
                Dim P2 As Point = DataPoint.Item(Con.To).GetCenter
                If Con.IsSelected Then
                    G.DrawLine(SelectedPen, P1, P2)
                End If
                If DataPoint.Item(Con.From).State Then
                    G.DrawLine(ConPenOn, P1, P2)
                Else
                    G.DrawLine(ConPenOff, P1, P2)
                End If
            Next
        Next

        Dim TopLine As Boolean = False
        Dim LefLine As Boolean = False
        Dim RigLine As Boolean = False
        Dim BotLine As Boolean = False

        'Draw Components
        For Each Com As ComponentBase In AllComponents
            Com.Draw(G)
            Dim cB As Rectangle = Com.GetBounds
            If cB.X < 0 Then LefLine = True
            If cB.Y < 0 Then TopLine = True
            If (cB.X + cB.Width) > B.Width Then RigLine = True
            If (cB.Y + cB.Height) > B.Height Then BotLine = True
        Next

        If DraggingDatapoint IsNot Nothing Then
            Dim P1 As Point = DraggingDatapoint.GetCenter
            Dim p2 As Point = CurrentMouseDragPos
            G.DrawLine(ConnectingPen, P1, p2)
        End If

        If DragObj IsNot Nothing Then
            Dim Bou As Rectangle = DragObj.GetBounds
            G.DrawRectangle(DraggingPen, Bou)
        End If

        For Each LastDataPoint As DataPoint In LastChoosenDataPoints
            LastDataPoint.Draw(G, True)
        Next

        G.SmoothingMode = Drawing2D.SmoothingMode.None
        If TopLine Then
            G.FillRectangle(Brushes.Blue, 0, 0, B.Width, 3)
        End If
        If LefLine Then
            G.FillRectangle(Brushes.Blue, 0, 0, 3, B.Height)
        End If
        If RigLine Then
            G.FillRectangle(Brushes.Blue, B.Width - 3, 0, 3, B.Height)
        End If
        If BotLine Then
            G.FillRectangle(Brushes.Blue, 0, B.Height - 3, B.Width, 3)
        End If
    End Sub

    Private LastChoosenDataPoints As New List(Of DataPoint)
    Private DraggingDatapoint As DataPoint = Nothing

    Private CanvasDragStart As New Point(0, 0)
    Private CanvasDragging As Boolean = False

    Private Sub CanvasPic_MouseDown(sender As Object, e As MouseEventArgs) Handles CanvasPic.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            CanvasDragStart = New Point(e.X - ScrollOffset.X, e.Y - ScrollOffset.Y)
            CanvasDragging = True
            Exit Sub
        End If
        If NewObject IsNot Nothing Then
            Dim NewPos As Point = New Point(e.X - ScrollOffset.X, e.Y - ScrollOffset.Y)
            If My.Computer.Keyboard.AltKeyDown Then
                NewPos.Add(ComponentBase.COMP_WIDTH / 2, ComponentBase.COMP_HEIGHT / 2)
                Dim X As Integer = NewPos.X - (NewPos.X Mod (ComponentBase.COMP_WIDTH + 20))
                Dim Y As Integer = NewPos.Y - (NewPos.Y Mod (ComponentBase.COMP_HEIGHT + 20))
                NewPos = New Point(X, Y)
            End If
            NewObject.Position = NewPos
            AllComponents.Add(NewObject)
            If My.Computer.Keyboard.ShiftKeyDown Then
                NewObject = Activator.CreateInstance(NewObject.GetType)
            Else
                ToolButtons(NewObject.GetType).BackColor = Me.BackColor
                NewObject = Nothing
            End If
            Exit Sub
        End If
        Dim MP As New Point(e.X, e.Y)
        Dim NewSelectedObj As Selectable = Nothing
        Dim ClickedObject As Object = Nothing
        Dim MPB As New Rectangle(MP.X - 5, MP.Y - 5, 10, 10)
        For Each Key In Connection.AllConnections.Keys
            For Each Con As Connection In Connection.AllConnections(Key)
                If LineIntersectsRect(DataPoint.Item(Con.From).GetCenter, DataPoint.Item(Con.To).GetCenter, MPB) Then
                    NewSelectedObj = Con
                    ClickedObject = Con
                End If
            Next
        Next
        For Each Com As ComponentBase In AllComponents
            If MP.IsIn(Com.GetBounds) Then
                NewSelectedObj = Com
                ClickedObject = Com
            End If
            For Each DPK As Guid In Com.GetAllDataPoints
                Dim DP As DataPoint = DataPoint.Item(DPK)
                If MP.IsIn(DP.GetBounds) Then
                    ClickedObject = DP
                    DraggingDatapoint = DP
                    CurrentMouseDragPos = MP
                End If
            Next
        Next
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If My.Computer.Keyboard.CtrlKeyDown Then
                If TypeOf ClickedObject Is DataPoint Then
                    Dim DP As DataPoint = ClickedObject
                    If LastChoosenDataPoints.Count <= 0 Then
                        LastChoosenDataPoints.Add(DP)
                    ElseIf LastChoosenDataPoints(0).IsOutput = DP.IsOutput Then
                        If LastChoosenDataPoints.Contains(DP) Then
                            LastChoosenDataPoints.Remove(DP)
                        Else
                            LastChoosenDataPoints.Add(DP)
                        End If
                    Else
                        For Each LastDP As DataPoint In LastChoosenDataPoints
                            Connection.Create(LastDP.Key, DP.Key)
                        Next
                        If Not My.Computer.Keyboard.ShiftKeyDown Then
                            LastChoosenDataPoints.Clear()
                        End If
                    End If
                End If
            Else
                If NewSelectedObj IsNot Nothing Then
                    NewSelectedObj.IsSelected = True
                    Dim Comp As ComponentBase = TryCast(ClickedObject, ComponentBase)
                    If Comp IsNot Nothing Then
                        If Not My.Computer.Keyboard.AltKeyDown Then
                            Comp.OnClick()
                        End If
                    End If
                End If
            End If
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            DragStart = MP
            If TypeOf ClickedObject Is ComponentBase Then
                Dim Com As ComponentBase = ClickedObject
                DragOrigin = Com.Position.Clone
                DragObj = Com
                Com.IsSelected = True
            End If
        End If
    End Sub

    Private CurrentMouseDragPos As New Point(0, 0)
    Private DragStart As New Point(0, 0)
    Private DragOrigin As New Point(0, 0)
    Private DragObj As ComponentBase = Nothing
    Private Sub CanvasPic_MouseMove(sender As Object, e As MouseEventArgs) Handles CanvasPic.MouseMove
        If CanvasDragging Then
            Dim Offset As New Point(e.X - CanvasDragStart.X, e.Y - CanvasDragStart.Y)
            ScrollOffset = Offset
            Exit Sub
        End If
        If DragObj IsNot Nothing Then
            Dim OffSet As New Point(e.X - DragStart.X, e.Y - DragStart.Y)
            Dim NewPos As Point = DragOrigin.Clone.Add(OffSet.X, OffSet.Y)
            If My.Computer.Keyboard.AltKeyDown Then
                NewPos.Add(ComponentBase.COMP_WIDTH / 2, ComponentBase.COMP_HEIGHT / 2)
                Dim X As Integer = NewPos.X - (NewPos.X Mod (ComponentBase.COMP_WIDTH + 20))
                Dim Y As Integer = NewPos.Y - (NewPos.Y Mod (ComponentBase.COMP_HEIGHT + 20))
                NewPos = New Point(X, Y)
            End If
            DragObj.Position = NewPos
        ElseIf DraggingDatapoint IsNot Nothing Then
            CurrentMouseDragPos = New Point(e.X, e.Y)
        End If
    End Sub

    Private Sub CanvasPic_MouseUp(sender As Object, e As MouseEventArgs) Handles CanvasPic.MouseUp
        If DraggingDatapoint IsNot Nothing Then
            Dim MP As New Point(e.X, e.Y)
            Do
                For Each Com As ComponentBase In AllComponents
                    For Each DPK As Guid In Com.GetAllDataPoints
                        Dim DP As DataPoint = DataPoint.Item(DPK)
                        If DP.GetBounds.Contains(MP) Then
                            Connection.Create(DraggingDatapoint.Key, DP.Key)
                            Exit Do
                        End If
                    Next
                Next
            Loop While False
        End If
        CanvasDragging = False
        DragObj = Nothing
        DraggingDatapoint = Nothing
    End Sub

    Private Sub RemoveBtn_Click(sender As Object, e As EventArgs) Handles RemoveBtn.Click
        If Selectable.SelectedObject IsNot Nothing Then
            Dim Comp As ComponentBase = TryCast(Selectable.SelectedObject, ComponentBase)
            If Comp IsNot Nothing Then
                Comp.RemoveFrom(AllComponents)
            Else
                Dim Con As Connection = TryCast(Selectable.SelectedObject, Connection)
                If Con IsNot Nothing Then
                    Con.Remove()
                End If
            End If
            If AllComponents.Count <= 0 Then
                ScrollOffset = New Point(0, 0)
            End If
        End If
    End Sub

    Private CurrentFI As IO.FileInfo = Nothing

    Private Sub SpeichernToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SpeichernToolStripMenuItem.Click
        If CurrentFI Is Nothing Then
            SpeichernunterToolStripMenuItem_Click(sender, e)
        Else
            Save()
        End If
    End Sub

    Private Sub SpeichernunterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SpeichernunterToolStripMenuItem.Click
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            CurrentFI = New IO.FileInfo(SaveFileDialog1.FileName)
            Save()
        End If
    End Sub

    Private Sub Save()
        If CurrentFI Is Nothing Then
            Stop
            Exit Sub
        End If
        Dim JJ As String = SaveToJJ()
        Try
            IO.File.WriteAllText(CurrentFI.FullName, JJ)

            SavedFileLab.Visible = True
            SavedMsgTimer.Start()
        Catch ex As Exception
            If Debugger.IsAttached() Then Stop
            MsgBox("Fehler beim Speichern!")
        End Try
    End Sub

    Private Function SaveToJJ() As String
        Dim Save As New SaveFile
        Save.AllComponents.AddRange(AllComponents)
        For Each Key As Guid In Connection.AllConnections.Keys
            Save.AllConnections.AddRange(Connection.AllConnections(Key))
        Next
        Save.ScrollOffset = ScrollOffset.Clone

        Dim JJsettings As New Newtonsoft.Json.JsonSerializerSettings
        JJsettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All

        Dim JJ As String = Newtonsoft.Json.JsonConvert.SerializeObject(Save, Newtonsoft.Json.Formatting.Indented, JJsettings)

        Return JJ
    End Function

    Private Sub BeendenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BeendenToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If MsgBox("Wirklich schließen ohne Speichern?!", MsgBoxStyle.YesNo, "Achtung!") = MsgBoxResult.No Then
            e.Cancel = True
        End If
    End Sub

    Private Sub NeuToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NeuToolStripMenuItem.Click
        ClearAll()
    End Sub

    Public Sub ClearAll()
        CurrentFI = Nothing
        Selectable.SelectedObject = Nothing
        Connection.AllConnections.Clear()
        AllComponents.Clear()
        DataPoint.AllDataPoints.Clear()
        ScrollOffset = New Point(0, 0)
    End Sub

    Private Sub ÖffnenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ÖffnenToolStripMenuItem.Click
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            ClearAll()
            Dim FI As New IO.FileInfo(OpenFileDialog1.FileName)
            If IO.File.Exists(FI.FullName) Then
                Try
                    Dim JJ As String = IO.File.ReadAllText(FI.FullName)

                    Dim JJsettings As New Newtonsoft.Json.JsonSerializerSettings
                    JJsettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All

                    ClearAll()
                    CurrentFI = FI

                    Dim SavedFile As SaveFile = Newtonsoft.Json.JsonConvert.DeserializeObject(JJ, JJsettings)

                    For Each Con As Connection In SavedFile.AllConnections
                        If Not Connection.AllConnections.ContainsKey(Con.To) Then
                            Connection.AllConnections.Add(Con.To, New List(Of Connection))
                        End If
                        Connection.AllConnections(Con.To).Add(Con)
                    Next
                    AllComponents.AddRange(SavedFile.AllComponents)
                    ScrollOffset = SavedFile.ScrollOffset
                Catch ex As Exception
                    If Debugger.IsAttached Then Stop
                    MsgBox("Fehler beim Öffnen der Datei!", MsgBoxStyle.OkOnly, "Fehler!")
                    ClearAll()
                End Try
            End If
        End If
    End Sub

    Private Sub SavedMsgTimer_Tick(sender As Object, e As EventArgs) Handles SavedMsgTimer.Tick
        SavedMsgTimer.Stop()
        SavedFileLab.Visible = False
    End Sub

End Class
