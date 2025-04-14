Public Class Evenement
    Private _id As Integer
    Private _idVoiture As Integer
    Private _acceleration As Double
    Private _vInit As Double
    Private _date As DateTime

    ' Constructeur
    Public Sub New(id As Integer, idVoiture As Integer, acceleration As Double, vInit As Double, dateEvenement As DateTime)
        Me.Id = id
        Me.IdVoiture = idVoiture
        Me.Acceleration = acceleration
        Me.VInit = vInit
        Me.DateEvenement = dateEvenement
    End Sub

    ' Getter et Setter pour Id
    Public Property Id As Integer
        Get
            Return _id
        End Get
        Set(value As Integer)
            _id = value
        End Set
    End Property

    ' Getter et Setter pour IdVoiture
    Public Property IdVoiture As Integer
        Get
            Return _idVoiture
        End Get
        Set(value As Integer)
            _idVoiture = value
        End Set
    End Property

    ' Getter et Setter pour Acceleration
    Public Property Acceleration As Double
        Get
            Return _acceleration
        End Get
        Set(value As Double)
            _acceleration = value
        End Set
    End Property

    ' Getter et Setter pour VInit
    Public Property VInit As Double
        Get
            Return _vInit
        End Get
        Set(value As Double)
            _vInit = value
        End Set
    End Property

    ' Getter et Setter pour DateEvenement
    Public Property DateEvenement As DateTime
        Get
            Return _date
        End Get
        Set(value As DateTime)
            _date = value
        End Set
    End Property
End Class
