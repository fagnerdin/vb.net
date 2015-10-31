Imports System.IO
''' <summary>
''' Controle de envio de email pelo exchange
''' </summary>
''' <remarks>enjoy</remarks>
Public Class mailController
    Public Sub sent(ByVal strDest As String, ByVal strAssnt As String, ByVal strBody As String)

        Dim olapp As Object
        Dim oitem As Object
        Dim VARDEST As String = "fagnerdin@no-mail.com;"
        olapp = CreateObject("Outlook.Application")
        oitem = olapp.CreateItem(0)

        With oitem
            .Subject = strAssnt & " - " & Environment.MachineName
            .To = VARDEST
            .HTMLBody = strBody
        End With

        oitem.Send()
        oitem.dispose()

    End Sub
End Class
