Public Class Compiler

    Public Const NEXT_COMMAND As Byte = 29 'GS
    Public Const NEXT_PART As Byte = 30 'RS
    Public Const NEXT_PARAM As Byte = 31 'US
    Public Const APP_END As Byte = 28 'FS
    Public Const CONNECTION As Byte = 92 '\
    Public Const DATA_POINT As Byte = 46 '.

    Friend Shared ReadOnly Type2ByteDict As New Dictionary(Of Type, Byte)
    Friend Shared ReadOnly Byte2TypeDict As New Dictionary(Of Byte, Type)

    Private Project As SaveFile

    Public Sub New(Project As SaveFile)
        Me.Project = Project
    End Sub

    Public Sub compile(Stream As IO.Stream)
        Dim DataPoints As New Dictionary(Of Guid, UInt16)

        Dim Id As UInt16 = 0
        For Each Comp As ComponentBase In Project.AllComponents
            For Each DP As Guid In Comp.GetAllDataPoints()
                If Not DataPoints.ContainsKey(DP) Then
                    DataPoints.Add(DP, Id)
                    Id += 1
                End If
            Next
        Next
        Stream.Write({DATA_POINT}, 0, 1)
        Stream.Write(BitConverter.GetBytes(Id), 0, 2)
        Stream.Write({NEXT_COMMAND}, 0, 1)

        For Each Comp As ComponentBase In Project.AllComponents
            Stream.Write({Comp.Symbol}, 0, 1)
            For Each DP As Guid In Comp.GetInputs
                Stream.Write({NEXT_PARAM}, 0, 1)
                Stream.Write(BitConverter.GetBytes(DataPoints(DP)), 0, 2)
            Next
            Stream.Write({NEXT_PART}, 0, 1)
            For Each DP As Guid In Comp.GetOutputs
                Stream.Write({NEXT_PARAM}, 0, 1)
                Stream.Write(BitConverter.GetBytes(DataPoints(DP)), 0, 2)
            Next
            Stream.Write({NEXT_COMMAND}, 0, 1)
        Next

        For Each Con As Connection In Project.AllConnections
            Stream.Write({CONNECTION}, 0, 1)
            Stream.Write(BitConverter.GetBytes(DataPoints(Con.From)), 0, 2)
            Stream.Write({NEXT_PARAM}, 0, 1)
            Stream.Write(BitConverter.GetBytes(DataPoints(Con.To)), 0, 2)
            Stream.Write({NEXT_COMMAND}, 0, 1)
        Next

        Stream.Write({APP_END}, 0, 1)
    End Sub

End Class
