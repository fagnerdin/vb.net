    ''' <summary>
    ''' FUNÇÃO DE LEITURA DE ARQUIVO CSV
    ''' </summary>
    ''' <param name="_FILE">ENDEREÇO DO ARQUIVO</param>
    ''' <returns>INSERÇÃO OK OU NAO</returns>
    Public Function read_csv_uraNet(ByVal _FILE As String)

        On Error GoTo errOut

        Dim fr As StreamReader = Nothing
        Dim FileString As String = Nothing
        Dim LineItemsArr() As String = Nothing
        Dim Qry As String

        Dim NM_MARCA_BI As Double
        Dim SK_DATA As Double
        Dim NM_MUNICIPIO_BI As Double
        Dim NM_SCRIPTPOINT_BI As Double
        Dim NR_SCRIPTPOINT As Double
        Dim NM_MOTIVO_URA As Double
        Dim NM_MOTIVO_URA_REMARCADO As Double
        Dim NM_SUBMOTIVO_URA_BI As Double
        Dim NM_EXPURGO_URA_BI As Double
        Dim DH_LIGACAO As Double
        Dim NR_CONTRATO As Double
        Dim CD_IMOVEL As Double
        Dim CD_NODE As Double
        Dim CD_OUTAGE As Double
        Dim QT_RAT_MOT_FINAL As Double
        Dim FC_DIRECIONADO_RETIDO As Double
        Dim NM_SEGMENTACAO_BI As Double

        Dim NM_STATUS_CONTRATO_BI As Double
        Dim NM_FAIXA_TEMPO_BI As Double
        Dim FC_LIGACOES_DIA_URA As Double

        Dim sqlTxtAdd As String = ""

        Dim NM_VENCIMENTO_FATURA_BI As Double
        'Dim LASTSOAFUNCTION As Double
        'Dim RETSOAVT As Double

        Dim s As New ModelCRUD

        Dim texto As New Dictionary(Of Double, String)
        Dim fg As String : Dim spltDHLigacao(2) As String

        '' LE ARQUIVO EM ISO 8859-1
        fr = New System.IO.StreamReader(_FILE, Encoding.GetEncoding("iso-8859-1"))

        '' COLETA CIDADES QUE DEVEM SER CARREGADAS
        Dim cidToup As DataTable = s.MySQLQuery("select group_concat(nm_cidade separator ', ') as grpCidades from  ora_cr.cr_cidades", "ora_cr")
        Dim cids As String = UCase(cidToup.Rows(0)("grpCidades"))

        '' VARRE ARQUIVO
        Dim X As Integer = 0 : Dim Add As Integer = 0 : Dim rwCnt As Integer = 0
        If X = 1 Then Console.WriteLine("... COLETANDO DADOS DO ARQUIVO ...")
        While fr.Peek <> -1
            FileString = fr.ReadLine.Trim

            Qry = FileString.Replace("""", "")
            Qry = Replace(Qry, "\", "").Replace("'", "\'")
            LineItemsArr = Split(UCase(Qry), ";") '<- CSV SEPARATOR

            '' PEGA INDICE DO NOME DOS CABEÇALHOS
            If X < 1 Then
                NM_MARCA_BI = IIf(Array.IndexOf(LineItemsArr, "NM_MARCA_BI") = -1, Array.IndexOf(LineItemsArr, "NM_MARCA"), Array.IndexOf(LineItemsArr, "NM_MARCA_BI"))
                SK_DATA = Array.IndexOf(LineItemsArr, "SK_DATA")
                NM_MUNICIPIO_BI = IIf(Array.IndexOf(LineItemsArr, "NM_MUNICIPIO_BI") = -1, Array.IndexOf(LineItemsArr, "NM_MUNICIPIO"), Array.IndexOf(LineItemsArr, "NM_MUNICIPIO_BI"))
                NM_SCRIPTPOINT_BI = IIf(Array.IndexOf(LineItemsArr, "NM_SCRIPTPOINT_BI") = -1, Array.IndexOf(LineItemsArr, "NM_SCRIPTPOINT"), Array.IndexOf(LineItemsArr, "NM_SCRIPTPOINT_BI"))
                NR_SCRIPTPOINT = Array.IndexOf(LineItemsArr, "NR_SCRIPTPOINT")
                NM_SEGMENTACAO_BI = IIf(Array.IndexOf(LineItemsArr, "NM_SEGMENTACAO_BI") = -1, Array.IndexOf(LineItemsArr, "NM_SEGMENTACAO"), Array.IndexOf(LineItemsArr, "NM_SEGMENTACAO_BI"))
                NM_MOTIVO_URA = Array.IndexOf(LineItemsArr, "NM_MOTIVO_URA")

                NM_STATUS_CONTRATO_BI = IIf(Array.IndexOf(LineItemsArr, "NM_STATUS_CONTRATO_BI") = -1, Array.IndexOf(LineItemsArr, "NM_STATUS_CONTRATO"), Array.IndexOf(LineItemsArr, "NM_STATUS_CONTRATO_BI"))
                NM_FAIXA_TEMPO_BI = IIf(Array.IndexOf(LineItemsArr, "NM_FAIXA_TEMPO_BI") = -1, Array.IndexOf(LineItemsArr, "NM_FAIXA_TEMPO"), Array.IndexOf(LineItemsArr, "NM_FAIXA_TEMPO_BI"))
                FC_LIGACOES_DIA_URA = Array.IndexOf(LineItemsArr, "FC_LIGACOES_DIA_URA")

                NM_MOTIVO_URA_REMARCADO = Array.IndexOf(LineItemsArr, "NM_MOTIVO_URA_REMARCADO")
                NM_SUBMOTIVO_URA_BI = IIf(Array.IndexOf(LineItemsArr, "NM_SUBMOTIVO_URA_BI") = -1, Array.IndexOf(LineItemsArr, "NM_SUBMOTIVO_URA"), Array.IndexOf(LineItemsArr, "NM_SUBMOTIVO_URA_BI"))
                NM_EXPURGO_URA_BI = IIf(Array.IndexOf(LineItemsArr, "NM_EXPURGO_URA_BI") = -1, Array.IndexOf(LineItemsArr, "NM_EXPURGO_URA"), Array.IndexOf(LineItemsArr, "NM_EXPURGO_URA_BI"))
                DH_LIGACAO = Array.IndexOf(LineItemsArr, "DH_LIGACAO") '    dt / hr / hr abre / dia semana
                NR_CONTRATO = Array.IndexOf(LineItemsArr, "NR_CONTRATO")
                CD_IMOVEL = Array.IndexOf(LineItemsArr, "CD_IMOVEL")
                CD_NODE = Array.IndexOf(LineItemsArr, "CD_NODE")
                CD_OUTAGE = Array.IndexOf(LineItemsArr, "CD_OUTAGE")
                QT_RAT_MOT_FINAL = Array.IndexOf(LineItemsArr, "QT_RAT_MOT_FINAL")
                FC_DIRECIONADO_RETIDO = Array.IndexOf(LineItemsArr, "FC_DIRECIONADO_RETIDO")

                NM_VENCIMENTO_FATURA_BI = IIf(Array.IndexOf(LineItemsArr, "NM_VENCIMENTO_FATURA_BI") = -1, Array.IndexOf(LineItemsArr, "NM_VENCIMENTO_FATURA"), Array.IndexOf(LineItemsArr, "NM_VENCIMENTO_FATURA_BI"))

                X += 1

            Else
                '' SE FOR A PRIMEIRA LINHA...
                If Add = 0 Then
                    If LineItemsArr(NM_MARCA_BI).Contains("FIM") = True Then GoTo _next
                    'If cids.Contains(LineItemsArr(NM_MUNICIPIO_BI)) Then '// SEPARA CIDADE

                    spltDHLigacao = Split(LineItemsArr(DH_LIGACAO), " ")

                    sqlTxtAdd = "(DEFAULT,'" & LineItemsArr(SK_DATA) & "',"
                    sqlTxtAdd &= "'" & LineItemsArr(NM_MUNICIPIO_BI) & "',"
                    sqlTxtAdd &= "'" & LineItemsArr(NM_SCRIPTPOINT_BI) & "',"
                    sqlTxtAdd &= "'" & LineItemsArr(NR_SCRIPTPOINT) & "',"
                    sqlTxtAdd &= "'" & LineItemsArr(NM_SEGMENTACAO_BI) & "',"
                    sqlTxtAdd &= "'" & LineItemsArr(NM_STATUS_CONTRATO_BI) & "',"
                    sqlTxtAdd &= "'" & LineItemsArr(NM_FAIXA_TEMPO_BI) & "',"
                    sqlTxtAdd &= "'" & LineItemsArr(NM_MOTIVO_URA) & "',"
                    sqlTxtAdd &= "'" & LineItemsArr(NM_MOTIVO_URA_REMARCADO) & "',"
                    sqlTxtAdd &= "'" & LineItemsArr(NM_SUBMOTIVO_URA_BI) & "',"
                    sqlTxtAdd &= "'" & LineItemsArr(NM_EXPURGO_URA_BI) & "',"
                    sqlTxtAdd &= "'" & LineItemsArr(DH_LIGACAO) & "',"
                    sqlTxtAdd &= "'" & IIf(LineItemsArr(NR_CONTRATO) = "", 0, LineItemsArr(NR_CONTRATO)) & "',"
                    sqlTxtAdd &= "'" & LineItemsArr(CD_IMOVEL) & "',"
                    sqlTxtAdd &= "'" & LineItemsArr(CD_NODE) & "',"
                    sqlTxtAdd &= "'" & LineItemsArr(CD_OUTAGE) & "',"
                    sqlTxtAdd &= "'" & LineItemsArr(FC_DIRECIONADO_RETIDO) & "',"
                    sqlTxtAdd &= "'" & LineItemsArr(FC_LIGACOES_DIA_URA) & "',"
                    sqlTxtAdd &= "'" & IIf(LineItemsArr(QT_RAT_MOT_FINAL) = "", 0, LineItemsArr(QT_RAT_MOT_FINAL)) & "',"
                    sqlTxtAdd &= "'" & IIf(LineItemsArr(NM_VENCIMENTO_FATURA_BI) = "", 0, LineItemsArr(NM_VENCIMENTO_FATURA_BI)) & "',"


                    '# SE O CAMPO TIVER APENAS DATA
                    If spltDHLigacao.Length > 1 Then sqlTxtAdd &= "'" & Format(CDate(spltDHLigacao(1)), "HH:mm:ss") & "'," Else sqlTxtAdd &= "'" & "00:00:00" & "',"

                    sqlTxtAdd &= "'" & Format(CDate(spltDHLigacao(0)), "yyyy-MM-dd") & "',"

                    '# SE O CAMPO TIVER APENAS DATA
                    If spltDHLigacao.Length > 1 Then sqlTxtAdd &= "'" & pegaHoraFechada(spltDHLigacao(1)) & "'," Else sqlTxtAdd &= "'" & "00:00:00" & "',"

                    sqlTxtAdd &= "'" & Format(CDate(spltDHLigacao(0)), "ddd") & "',"
                    sqlTxtAdd &= "'" & Format(CDate(spltDHLigacao(0)), "MM/yy") & "')"

                    texto.Add(X, sqlTxtAdd)
                    Add = 1
                    'End If

                Else
                    '' ... PROXIMAS LINHAS ...
                    If LineItemsArr(NM_MARCA_BI).Contains("FIM") = True Then GoTo _next
                    'If cids.Contains(LineItemsArr(NM_MUNICIPIO_BI)) Then '// SEPARA CIDADE

                    spltDHLigacao = Split(LineItemsArr(DH_LIGACAO), " ")


                    texto(X) &= ",(DEFAULT,'" & LineItemsArr(SK_DATA) & "',"
                    texto(X) &= "'" & LineItemsArr(NM_MUNICIPIO_BI) & "',"
                    texto(X) &= "'" & LineItemsArr(NM_SCRIPTPOINT_BI) & "',"
                    texto(X) &= "'" & LineItemsArr(NR_SCRIPTPOINT) & "',"
                    texto(X) &= "'" & LineItemsArr(NM_SEGMENTACAO_BI) & "',"
                    texto(X) &= "'" & LineItemsArr(NM_STATUS_CONTRATO_BI) & "',"
                    texto(X) &= "'" & LineItemsArr(NM_FAIXA_TEMPO_BI) & "',"
                    texto(X) &= "'" & LineItemsArr(NM_MOTIVO_URA) & "',"
                    texto(X) &= "'" & LineItemsArr(NM_MOTIVO_URA_REMARCADO) & "',"
                    texto(X) &= "'" & LineItemsArr(NM_SUBMOTIVO_URA_BI) & "',"
                    texto(X) &= "'" & LineItemsArr(NM_EXPURGO_URA_BI) & "',"
                    texto(X) &= "'" & LineItemsArr(DH_LIGACAO) & "',"
                    texto(X) &= "'" & IIf(LineItemsArr(NR_CONTRATO) = "", 0, LineItemsArr(NR_CONTRATO)) & "',"
                    texto(X) &= "'" & LineItemsArr(CD_IMOVEL) & "',"
                    texto(X) &= "'" & LineItemsArr(CD_NODE) & "',"
                    texto(X) &= "'" & LineItemsArr(CD_OUTAGE) & "',"
                    texto(X) &= "'" & LineItemsArr(FC_DIRECIONADO_RETIDO) & "',"
                    texto(X) &= "'" & LineItemsArr(FC_LIGACOES_DIA_URA) & "',"
                    texto(X) &= "'" & IIf(LineItemsArr(QT_RAT_MOT_FINAL) = "", 0, LineItemsArr(QT_RAT_MOT_FINAL)) & "',"
                    texto(X) &= "'" & IIf(LineItemsArr(NM_VENCIMENTO_FATURA_BI) = "", 0, LineItemsArr(NM_VENCIMENTO_FATURA_BI)) & "',"

                    '# SE TIVER APENAS DATA
                    If spltDHLigacao.Length > 1 Then texto(X) &= "'" & Format(CDate(spltDHLigacao(1)), "HH:mm:ss") & "'," Else texto(X) &= "'" & "00:00:00" & "',"

                    texto(X) &= "'" & Format(CDate(spltDHLigacao(0)), "yyyy-MM-dd") & "',"

                    '# SE O CAMPO TIVER APENAS DATA
                    If spltDHLigacao.Length > 1 Then texto(X) &= "'" & pegaHoraFechada(spltDHLigacao(1)) & "'," Else texto(X) &= "'" & "00:00:00" & "',"

                    texto(X) &= "'" & Format(CDate(spltDHLigacao(0)), "ddd") & "',"
                    texto(X) &= "'" & Format(CDate(spltDHLigacao(0)), "MM/yy") & "')"

                    'End If
                End If

                rwCnt += 1
                If rwCnt Mod 1000 = 0 Then X += 1 : Add = 0


            End If
_next:
        End While
        fr.Close()
        fr = Nothing
        FileString = Nothing
        LineItemsArr = Nothing

        Console.WriteLine("... SUBINDO DADOS PARA O SISTEMA ...")
        For Each pair In texto
            Qry = "INSERT INTO ora_cr.cr_net VALUES " & pair.Value
            'Qry = "INSERT IGNORE INTO ora_cr.cr_claro_ VALUES " & File.ReadAllText(pair.Value, Encoding.GetEncoding("iso-8859-1"))
            s.MyAdd(Qry.Replace("')(DEFAULT", "'),(DEFAULT"), "ora_cr")
            Qry = Nothing
        Next
        texto = Nothing

        Return rwCnt
        Exit Function

errOut:
        Dim strErr As String = Err.Description
        Dim R As String = Err.GetException.ToString
        Return 0
    End Function
