Public Class classBench

    Private hrIni As String
    Private tmpTotal As TimeSpan

    Private usrLogin As String
    Private mchName As String

    Private Classe As String
    Private Metodo As String


    ''' <summary>
    ''' CONSTRUTOR
    ''' PEGA LOGIN, MAQUINA E HORA INICIAL
    ''' </summary>
    ''' <param name="vClasse">CLASSE: My.Application.Info.AssemblyName</param>
    ''' <param name="vMetodo">METODO: System.Reflection.MethodBase.GetCurrentMethod.Name</param>
    ''' <remarks>ENJOY</remarks>
    Sub New(ByVal vClasse As String, ByVal vMetodo As String)
        Dim str As String = vbNullString

        Me.hrIni = Now

        If System.Security.Principal.WindowsIdentity.GetCurrent.IsAuthenticated Then
            str = System.Security.Principal.WindowsIdentity.GetCurrent.Name.ToString

            '' ATRIBUI ATRIBUTOS
            Dim vlr As String() = str.Split("\") 
            Me.usrLogin = vlr(1).ToUpper '' Seta usuario logado
            Me.mchName = LCase(Environment.MachineName) & ".netservicos.corp" '' Nome da maquina
            Me.Classe = vClasse '' Nome da classe
            Me.Metodo = vMetodo '' metodo

        End If

    End Sub


    ''' <summary>
    ''' SALVA BENCH
    ''' </summary>
    ''' <remarks>ENJOY</remarks>
    Sub execute()

        
        '' MARCA O TEMPO DE EXECUÇÃO
        tmpTotal = Now - CDate(Me.hrIni)
        Dim tempo As String = Format(CDate(tmpTotal.ToString), "HH:mm:ss")

        Dim crud As New ModelCRUD
        Dim QryString As New Dictionary(Of String, String)

        '' APLICAÇÃO
        QryString("INSERT_INTO") = "ora_log.log_acoes"
        QryString("VALUES") = "('" & Format(Now.Date, "yyyy-MM-dd") & "','" & Me.Classe & "','" & Me.Metodo & "',1,'" & tempo & "') " & _
                    "ON DUPLICATE KEY UPDATE MED_ROWS = MED_ROWS + 1, ELAP_TIME = SEC_TO_TIME((TIME_TO_SEC(ELAP_TIME) + TIME_TO_SEC('" & tempo & "'))/2) "
        crud.Inclui(QryString, "ora_log")

        '' USUARIO
        QryString = New Dictionary(Of String, String)
        QryString("INSERT_INTO") = "ora_log.log_acoes_app(DT_ACAO,HR_ACAO, NOM_APP, ATIVIDADE, MAQUINA, USR_LOGIN, CONT)"
        QryString("VALUES") = "('" & Format(Now.Date, "yyyy-MM-dd") & "','" & Now.Hour & "','" & Me.Classe & "','" & Me.Metodo & "','" & Me.mchName & "','" & Me.usrLogin & "',1) " & _
                    "ON DUPLICATE KEY UPDATE CONT = CONT + 1"
        crud.Inclui(QryString, "ora_log")

        crud = Nothing

    End Sub

    ''' <summary>
    ''' SALVA BENCH
    ''' </summary>
    ''' <param name="num">NUM OF ROWS RESULT</param>
    ''' <remarks></remarks>
    Sub execute(ByVal num As String)


        '' MARCA O TEMPO DE EXECUÇÃO
        tmpTotal = Now - CDate(Me.hrIni)
        Dim tempo As String = Format(CDate(tmpTotal.ToString), "HH:mm:ss")

        Dim crud As New ModelCRUD
        Dim QryString As New Dictionary(Of String, String)

        '' APLICAÇÃO
        QryString("INSERT_INTO") = "ora_log.log_acoes"
        QryString("VALUES") = "('" & Format(Now.Date, "yyyy-MM-dd") & "','" & Me.Classe & "','" & Me.Metodo & "'," & num & ",'" & tempo & "') " & _
                    "ON DUPLICATE KEY UPDATE MED_ROWS = (MED_ROWS + " & num & ") / 2, ELAP_TIME = SEC_TO_TIME((TIME_TO_SEC(ELAP_TIME) + TIME_TO_SEC('" & tempo & "'))/2) "
        crud.Inclui(QryString, "ora_log")

        '' USUARIO
        QryString = New Dictionary(Of String, String)
        QryString("INSERT_INTO") = "ora_log.log_acoes_app(DT_ACAO,HR_ACAO, NOM_APP, ATIVIDADE, MAQUINA, USR_LOGIN, CONT)"
        QryString("VALUES") = "('" & Format(Now.Date, "yyyy-MM-dd") & "','" & Now.Hour & "','" & Me.Classe & "','" & Me.Metodo & "','" & Me.mchName & "','" & Me.usrLogin & "',1) " & _
                    "ON DUPLICATE KEY UPDATE CONT = CONT + 1"
        crud.Inclui(QryString, "ora_log")

        crud = Nothing

    End Sub
End Class
