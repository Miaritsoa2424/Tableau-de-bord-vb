Imports MySql.Data.MySqlClient

Public Class VoitureDao
    ' Ajouter une voiture
    Public Sub Ajouter(voiture As Voiture)
        Dim conn As MySqlConnection = Nothing
        Try
            conn = Database.GetConnection()
            Dim sql As String = "INSERT INTO voiture (marque, acceleration, deceleration, consommation, carburant,carburant_max, v_max) VALUES (@marque, @acceleration, @deceleration, @consommation, @carburant,@carburant_max, @v_max)"
            Using cmd As New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@marque", voiture.Marque)
                cmd.Parameters.AddWithValue("@acceleration", voiture.Acceleration)
                cmd.Parameters.AddWithValue("@deceleration", voiture.Deceleration)
                cmd.Parameters.AddWithValue("@consommation", voiture.Consommation)
                cmd.Parameters.AddWithValue("@carburant", voiture.Carburant)
                cmd.Parameters.AddWithValue("@carburant_max", voiture.Carburant)
                cmd.Parameters.AddWithValue("@v_max", voiture.V_max)
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As MySqlException
            Throw New Exception("Erreur lors de l'ajout de la voiture : " & ex.Message)
        Catch ex As Exception
            Throw New Exception("Erreur inattendue pendant l'ajout : " & ex.Message)
        Finally
            Database.CloseConnection(conn)
        End Try
    End Sub

    ' Récupérer toutes les voitures
    Public Shared Function GetAll() As List(Of Voiture)
        Dim liste As New List(Of Voiture)
        Dim conn As MySqlConnection = Nothing
        Try
            conn = Database.GetConnection()
            Dim sql As String = "SELECT * FROM voiture"
            Using cmd As New MySqlCommand(sql, conn)
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim v As New Voiture(
                            reader.GetInt32("id"),
                            reader.GetString("marque"),
                            reader.GetDouble("acceleration"),
                            reader.GetDouble("deceleration"),
                            reader.GetDouble("consommation"),
                            reader.GetDouble("carburant"),
                            reader.GetDouble("carburant_max"),
                            reader.GetDouble("v_max")
                        )
                        liste.Add(v)
                    End While
                End Using
            End Using
        Catch ex As MySqlException
            Throw New Exception("Erreur lors de la récupération des voitures : " & ex.Message)
        Catch ex As Exception
            Throw New Exception("Erreur inattendue pendant la récupération : " & ex.Message)
        Finally
            Database.CloseConnection(conn)
        End Try
        Return liste
    End Function

    ' Récupérer une voiture par ID
    Public Shared Function GetById(id As Integer) As Voiture
        Dim conn As MySqlConnection = Nothing
        Try
            conn = Database.GetConnection()
            Dim sql As String = "SELECT * FROM voiture WHERE id = @id"
            Using cmd As New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@id", id)
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Return New Voiture(
                            reader.GetInt32("id"),
                            reader.GetString("marque"),
                            reader.GetDouble("acceleration"),
                            reader.GetDouble("deceleration"),
                            reader.GetDouble("consommation"),
                            reader.GetDouble("carburant"),
                            reader.GetDouble("carburant_max"),
                            reader.GetDouble("v_max")
                        )
                    Else
                        Return Nothing
                    End If
                End Using
            End Using
        Catch ex As MySqlException
            Throw New Exception("Erreur lors de la récupération de la voiture : " & ex.Message)
        Catch ex As Exception
            Throw New Exception("Erreur inattendue pendant la récupération : " & ex.Message)
        Finally
            Database.CloseConnection(conn)
        End Try
    End Function

    ' Modifier une voiture
    Public Sub Modifier(voiture As Voiture)
        Dim conn As MySqlConnection = Nothing
        Try
            conn = Database.GetConnection()
            Dim sql As String = "UPDATE voiture SET marque = @marque, acceleration = @acceleration, deceleration = @deceleration, consommation = @consommation, carburant = @carburant,carburant_max = @carburant_max, v_max = @v_max WHERE id = @id"
            Using cmd As New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@id", voiture.Id)
                cmd.Parameters.AddWithValue("@marque", voiture.Marque)
                cmd.Parameters.AddWithValue("@acceleration", voiture.Acceleration)
                cmd.Parameters.AddWithValue("@deceleration", voiture.Deceleration)
                cmd.Parameters.AddWithValue("@consommation", voiture.Consommation)
                cmd.Parameters.AddWithValue("@carburant", voiture.Carburant)
                cmd.Parameters.AddWithValue("@carburant_max", voiture.Carburant_max)
                cmd.Parameters.AddWithValue("@v_max", voiture.V_max)
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As MySqlException
            Throw New Exception("Erreur lors de la modification de la voiture : " & ex.Message)
        Catch ex As Exception
            Throw New Exception("Erreur inattendue pendant la modification : " & ex.Message)
        Finally
            Database.CloseConnection(conn)
        End Try
    End Sub

    ' Supprimer une voiture
    Public Sub Supprimer(id As Integer)
        Dim conn As MySqlConnection = Nothing
        Try
            conn = Database.GetConnection()
            Dim sql As String = "DELETE FROM voiture WHERE id = @id"
            Using cmd As New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@id", id)
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As MySqlException
            Throw New Exception("Erreur lors de la suppression de la voiture : " & ex.Message)
        Catch ex As Exception
            Throw New Exception("Erreur inattendue pendant la suppression : " & ex.Message)
        Finally
            Database.CloseConnection(conn)
        End Try
    End Sub
End Class
