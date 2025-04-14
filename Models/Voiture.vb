Public Class Voiture
    Private _id As Integer
    Private _marque As String
    Private _acceleration As Double
    Private _deceleration As Double
    Private _consommation As Double
    Private _carburant As Double
    Private _carburant_max As Double
    Private _v_max As Double


    ' Constructeur
    ' id est l'identifiant de la voiture
    ' marque est la marque de la voiture
    Public Sub New(id As Integer, marque As String, acceleration As Double, deceleration As Double, consommation As Double,carburant As Double,carburant_max As Double, v_max As Double)
        Me.Id = id
        Me.Marque = marque
        Me.Acceleration = acceleration
        Me.Deceleration = deceleration
        Me.Consommation = consommation
        Me.Carburant = carburant
        Me.Carburant_max = carburant_max
        Me.V_max = v_max
    End Sub

    ' Getter et Setter pour Id
    ' Id est un entier
    ' Id est l'identifiant de la voiture
    Public Property Id As Integer
        Get
            Return _id
        End Get
        Set(value As Integer)
            _id = value
        End Set
    End Property

    ' Getter et Setter pour Marque
    ' Marque est une chaîne de caractères
    Public Property Marque As String
        Get
            Return _marque
        End Get
        Set(value As String)
            _marque = value
        End Set
    End Property

    ' Getter et Setter pour Acceleration
    ' Accélération est en m/heure/seconde
    Public Property Acceleration As Double
        Get
            Return _acceleration
        End Get
        Set(value As Double)
            If value >= 0 Then
                _acceleration = value
            Else
                Throw New ArgumentException("L'accélération doit être positive.")
            End If
        End Set
    End Property

    ' Getter et Setter pour Deceleration
    ' Décélération est en m/heure/seconde
    Public Property Deceleration As Double
        Get
            Return _deceleration
        End Get
        Set(value As Double)
            If value >= 0 Then
                _deceleration = value
            Else
                Throw New ArgumentException("La décélération doit être positive.")
            End If
        End Set
    End Property

    ' Getter et Setter pour Consommation
    ' Consommation est en litres/seconde
    ' La consommation doit être positive
    Public Property Consommation As Double
        Get
            Return _consommation
        End Get
        Set(value As Double)
            If value >= 0 Then
                _consommation = value
            Else
                Throw New ArgumentException("La consommation doit être positive.")
            End If
        End Set
    End Property

    ' Getter et Setter pour Carburant
    ' Carburant est en litres
    Public Property Carburant As Double
        Get
            Return _carburant
        End Get
        Set(value As Double)
            If value >= 0 Then
                _carburant = value
            Else
                _carburant = 0
            End If
        End Set
    End Property

    Public Property Carburant_max As Double
        Get
            Return _carburant_max
        End Get
        Set(value As Double)
            If value >= 0 Then
                _carburant_max = value
            Else
                Throw New ArgumentException("Le carburant maximum doit être positif.")
            End If
        End Set
    End Property

    ' Getter et Setter pour V_max
    ' Vitesse maximale est en km/h
    Public Property V_max As Double
        Get
            Return _v_max
        End Get
        Set(value As Double)
            If value >= 0 Then
                _v_max = value
            Else
                Throw New ArgumentException("La vitesse maximale doit être positive.")
            End If
        End Set
    End Property

    Public Function Accelerer(pourcentage As Double, duree As Double, vitesseInit As Double) As Double
        Dim acceleration As Double = 0
        If pourcentage < 0 Then
            acceleration = Me.Deceleration * pourcentage
        ElseIf pourcentage > 0 Then
            If Me.Carburant <= 0 Then
                Return vitesseInit
            End If
            acceleration = Me.Acceleration * pourcentage
            Dim conso As Double = Me.Consommation * pourcentage
            Me.Carburant -= (conso * duree)
        End If

        Dim vitesse As Double = acceleration * duree + vitesseInit
        If vitesse < 0 Then
            Return -0.1
        End If

        Return vitesse
    End Function

    Public Function ConsommationMoyenne(distanceParcourue As Double) As Double
        If distanceParcourue <= 0 Then
            Return 0
        End If
        Dim carburantConsomme AS Double = Me.Carburant_max - Me.Carburant
        Dim conso As Double = (100*carburantConsomme)/distanceParcourue
        Return conso
    End Function

End Class
