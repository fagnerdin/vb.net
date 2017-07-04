Public Class frmPrincipal
  ' QUANDO MINIMIZAR A JANELA
  Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
          NotifyIcon1.Visible = True
          Me.Hide()
          NotifyIcon1.BalloonTipText = "Vou ficar aqui no relógio, tá...?"
          NotifyIcon1.ShowBalloonTip(500)
      End If
  End Sub

  ' QUANDO DER DOIS CLIQUES NO ICONE DA BANDEJA
  Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick
      Me.Show()
      Me.WindowState = FormWindowState.Normal
      NotifyIcon1.Visible = False
  End Sub
End Class
