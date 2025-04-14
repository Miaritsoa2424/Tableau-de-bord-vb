Imports MySql.Data.MySqlClient

Public Class Database
    ' Variables privées pour la connexion
    Private Shared serveur As String = "localhost"
    Private Shared utilisateur As String = "root"
    Private Shared motDePasse As String = ""
    Private Shared baseDeDonnees As String = "voiture_db"

    ' Générer la connection string
    Private Shared connectionString As String = $"server={serveur};userid={utilisateur};password={motDePasse};database={baseDeDonnees}"

    ' Méthode pour obtenir une connexion ouverte
    Public Shared Function GetConnection() As MySqlConnection
        Dim conn As New MySqlConnection(connectionString)
        Try
            conn.Open()
            Return conn
        Catch ex As MySqlException
            ' En cas d'erreur MySQL, relancer l'exception
            Throw New Exception("Erreur lors de l'ouverture de la connexion à la base de données : " & ex.Message)
        Catch ex As Exception
            ' Autres erreurs
            Throw New Exception("Erreur inconnue : " & ex.Message)
        End Try
    End Function

    ' Méthode pour fermer proprement une connexion
    Public Shared Sub CloseConnection(ByRef conn As MySqlConnection)
        If conn IsNot Nothing AndAlso conn.State <> ConnectionState.Closed Then
            Try
                conn.Close()
                conn.Dispose()
            Catch ex As MySqlException
                Throw New Exception("Erreur lors de la fermeture de la connexion : " & ex.Message)
            Catch ex As Exception
                Throw New Exception("Erreur inconnue pendant la fermeture : " & ex.Message)
            End Try
        End If
    End Sub
End Class
