''' <summary>
''' ESCREVE ARQUIVO DE LOG (Importe System.IO)
''' </summary>
''' <param name="DATAHORA">dateTime do acontecimento</param>
''' <param name="MSG">mensagem de erro</param>
''' <param name="FUNCAO">funcao do erro</param>
''' <returns>Nothing</returns>
''' <remarks>ENJOY</remarks>
Function WhriteLog(ByVal DATAHORA As String, ByVal MSG As String, ByVal FUNCAO As String)

    Dim ADDRESS As String = "c:/log/" 'pasta em que o arquivo deve ser salvo
    Dim NOMELOG As String = "_" & Environment.MachineName & "_" & Format(Now, "yyyyMMdd") & ".txt" 'maquina e data do relatorio -> arquivo

    Dim log As StreamWriter
    Dim texto As String = ""
    Dim fluxoTexto As IO.StreamReader
    Dim linhaTexto As String

    '// Leitura do texto no arquivo, caso ele exista
    If IO.File.Exists(ADDRESS & NOMELOG) Then
        fluxoTexto = New IO.StreamReader(ADDRESS & NOMELOG)
        linhaTexto = fluxoTexto.ReadLine

        '// Pega texto antigo, para montar o historico
        While linhaTexto <> Nothing
            texto = texto & linhaTexto & vbNewLine
            linhaTexto = fluxoTexto.ReadLine
        End While
        fluxoTexto.Close()
    End If
    texto = NomeDoUsuario & ": "  vbNewLine & texto '// Usuario logado na maquina e Texto do relatorio
    log = New StreamWriter(ADDRESS & NOMELOG)
    log.Write(texto) '// escreve texto no arquivo
    log.Close()

    Return Nothing

End Function

''' <summary>
''' FUNÇÃO QUE LE O USUARIO LOGADO NO WINDOWS
''' </summary>
''' <returns>USUARIO</returns>
''' <remarks>ENJOY</remarks>
Private Function NomeDoUsuario() As String
    Dim str As String = vbNullString

    If System.Security.Principal.WindowsIdentity.GetCurrent.IsAuthenticated Then
        str = System.Security.Principal.WindowsIdentity.GetCurrent.Name.ToString

        Dim vlr As String() = str.Split("\")
        str = vlr(1).ToUpper

    End If
    Return str '//  RETORNA USUARIO
End Function
