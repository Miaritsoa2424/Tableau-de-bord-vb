Imports MySql.Data.MySqlClient

Public Class EvenementDao
    ' Ajouter un évènement
    Public Shared Sub Ajouter(evenement As Evenement)
        Dim conn As MySqlConnection = Nothing
        Try
            conn = Database.GetConnection()
            Dim sql As String = "INSERT INTO evenement (id_voiture, acceleration, v_init, date) VALUES (@id_voiture, @acceleration, @v_init, @date)"
            Using cmd As New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@id_voiture", evenement.IdVoiture)
                cmd.Parameters.AddWithValue("@acceleration", evenement.Acceleration)
                cmd.Parameters.AddWithValue("@v_init", evenement.VInit)
                cmd.Parameters.AddWithValue("@date", evenement.DateEvenement)
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As MySqlException
            Throw New Exception("Erreur lors de l'ajout de l'évènement : " & ex.Message)
        Catch ex As Exception
            Throw New Exception("Erreur inattendue pendant l'ajout : " & ex.Message)
        Finally
            Database.CloseConnection(conn)
        End Try
    End Sub

    ' Récupérer un évènement par ID
    Public Shared Function GetById(id As Integer) As Evenement
        Dim conn As MySqlConnection = Nothing
        Try
            conn = Database.GetConnection()
            Dim sql As String = "SELECT * FROM evenement WHERE id_event = @id"
            Using cmd As New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@id", id)
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Return New Evenement(
                            reader.GetInt32("id_event"),
                            reader.GetInt32("id_voiture"),
                            reader.GetDouble("acceleration"),
                            reader.GetDouble("v_init"),
                            reader.GetDateTime("date")
                        )
                    Else
                        Throw New Exception("Évènement avec ID " & id & " introuvable.")
                    End If
                End Using
            End Using
        Catch ex As MySqlException
            Throw New Exception("Erreur lors de la récupération de l'évènement : " & ex.Message)
        Catch ex As Exception
            Throw New Exception("Erreur inattendue pendant la récupération : " & ex.Message)
        Finally
            Database.CloseConnection(conn)
        End Try
    End Function
End Class
