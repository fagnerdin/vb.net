Public Class UsrCFG

    ''' <summary>
    ''' FUNÇÃO QUE LE O USUARIO LOGADO NO WINDOWS
    ''' </summary>
    ''' <returns>USUARIO</returns>
    ''' <remarks>ENJOY</remarks>
    Public Function NomeDoUsuario() As String
        Dim str As String = vbNullString

        If System.Security.Principal.WindowsIdentity.GetCurrent.IsAuthenticated Then
            str = System.Security.Principal.WindowsIdentity.GetCurrent.Name.ToString

            Dim vlr As String() = str.Split("\")
            str = vlr(1).ToUpper

        End If

        Return str

    End Function
End Class
