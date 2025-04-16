Public Class ClavierControl
    Private WithEvents form As Form
    Private dashboard As DashboardForm ' Référence à l'instance de Dashboard
    Private shouldSave As Boolean = False ' Indicateur si la valeur doit être enregistrée

    ' Constructor qui accepte une instance de Dashboard
    Public Sub New(form As Form, dashboard As DashboardForm)
        Me.form = form
        Me.dashboard = dashboard
        form.KeyPreview = True ' Assurer que le formulaire capte les événements de touches
    End Sub

    ' Gérer l'événement KeyDown pour détecter les touches enfoncées
    Private Sub form_KeyDown(sender As Object, e As KeyEventArgs) Handles form.KeyDown
        ' Vérification des flèches directionnelles
        
        If e.KeyCode = Keys.Up Then
            dashboard.upPressed = True
            dashboard.downPressed = False
        ElseIf e.KeyCode = Keys.Down Then
            dashboard.downPressed = True
            dashboard.upPressed = False
        ElseIf e.KeyCode = Keys.C Then
            ' Passer à la voiture suivante
            dashboard.indexVoitureActuelle = (dashboard.indexVoitureActuelle + 1) Mod dashboard.voitures.Count
            ' Mettre à jour la voiture dans le dashboard
            Dim voiture As Voiture = dashboard.voitures(dashboard.indexVoitureActuelle)
            dashboard.Voiture = voiture
            dashboard.acceleration = 0
            dashboard.vitesse = 0
            dashboard.distanceParcourue = 0
            dashboard.upPressed = False
        ' Vérification de la touche S pour stoper l'animation
        ElseIf e.KeyCode = Keys.S Then
            shouldSave = True
            dashboard.timer.Stop()
        ElseIf e.KeyCode = Keys.R Then
            ' Reprendre l'animation
            dashboard.timer.Start()
            dashboard.acceleration = 0
            dashboard.vitesse = 0
            dashboard.distanceParcourue = 0
            dashboard.replay = True
        ' Vérification de la touche 0
        ElseIf e.KeyCode = Keys.D0 Then
            dashboard.newValue = 0
            shouldSave = True
        ' Vérification des touches H après une flèche directionnelle
        ElseIf dashboard.upPressed AndAlso (e.KeyCode = Keys.H OrElse e.KeyCode = Keys.H) Then
            dashboard.newValue = 1
            dashboard.upPressed = False
            shouldSave = True
        ElseIf dashboard.downPressed AndAlso (e.KeyCode = Keys.H OrElse e.KeyCode = Keys.H) Then
            dashboard.newValue = -1
            dashboard.downPressed = False
            shouldSave = True
        ' Vérification des chiffres (1 à 9) après une flèche directionnelle
        ElseIf dashboard.upPressed AndAlso e.KeyCode >= Keys.D1 AndAlso e.KeyCode <= Keys.D9 Then
            dashboard.newValue = (e.KeyCode - Keys.D0) * 0.1
            dashboard.upPressed = False
            shouldSave = True
        ElseIf dashboard.downPressed AndAlso e.KeyCode >= Keys.D1 AndAlso e.KeyCode <= Keys.D9 Then
            dashboard.newValue = (e.KeyCode - Keys.D0) * -0.1
            dashboard.downPressed = False
            shouldSave = True
        End If

        ' Si une nouvelle valeur a été enregistrée, afficher la valeur
        If shouldSave Then
            Dim acceleration As Double = 0
            shouldSave = False
            If dashboard.newValue = 0 Then
                acceleration = 0
            ElseIf dashboard.newValue > 0 Then
                acceleration = dashboard.Voiture.Acceleration * dashboard.newValue
            ElseIf dashboard.newValue < 0 Then
                acceleration = dashboard.Voiture.Deceleration * dashboard.newValue
            End If
            Dim evenement As Evenement = new Evenement(1,dashboard.Voiture.Id,acceleration,dashboard.vitesse,DateTime.Now)
            EvenementDao.Ajouter(evenement)
        End If
    End Sub
End Class
