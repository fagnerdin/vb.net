Public Sub getXml()
  Dim str As String
  Dim DS As New DataSet
  Dim xml As String = "http://endereco.xml.com/" '
  DS.ReadXml(xml)
    
  For Each TRows In DS.Tables(0).Rows
        str = TRows("cabecalho").ToString()
  Next
  
End Sub
