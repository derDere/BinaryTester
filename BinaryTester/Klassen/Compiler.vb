Public Class Compiler

    Public Const NEXT_COMMAND As Byte = 29 'GS
    Public Const NEXT_PART As Byte = 30 'RS
    Public Const NEXT_PARAM As Byte = 31 'US
    Public Const APP_END As Byte = 28 'FS

    Friend Shared ReadOnly Type2ByteDict As New Dictionary(Of Type, Byte)
    Friend Shared ReadOnly Byte2TypeDict As New Dictionary(Of Byte, Type)



End Class
