Imports System.Drawing.Drawing2D

Public Class DashboardForm
    Inherits Form
    Private lastTickTime As DateTime
    Public vitesse As Double = 0.0F
    Public distanceParcourue As Single = 0.0F ' km
    Public voitures As List(Of Voiture)
    Public indexVoitureActuelle As Integer = 1
    Public Voiture As Voiture ' Instance de la classe Voiture


    ' Couleurs personnalisées
    Private ReadOnly couleurFond As Color = Color.FromArgb(30, 30, 40)
    Private ReadOnly couleurCadran As Color = Color.FromArgb(60, 60, 70)
    Private ReadOnly couleurAiguille As Color = Color.FromArgb(220, 50, 50)
    Private ReadOnly couleurTexte As Color = Color.WhiteSmoke
    Private clavier As ClavierControl
    Private heureActuelle As String = DateTime.Now.ToString("HH:mm:ss")


    Public timer As Timer
    Public replay As Boolean = False
    Public newValue As Double = 0 ' La variable à partager
    Public acceleration As Double = 0
    Public upPressed As Boolean = False
    Public downPressed As Boolean = False

    Public Sub New()
        Me.voitures = VoitureDao.GetAll()
        Me.Voiture = voitures(indexVoitureActuelle)
        Me.Text = "Tableau de Bord Voiture"
        Me.ClientSize = New Size(800, 500)
        Me.BackColor = couleurFond
        Me.DoubleBuffered = True ' Pour éviter le scintillement
        Me.KeyPreview = True
        Me.focus()
        clavier = New ClavierControl(Me, Me)

        ' Utiliser la variable globale, sans Dim
        timer = New Timer()
        timer.Interval = 50
        lastTickTime = DateTime.Now
        AddHandler timer.Tick, AddressOf Timer_Tick
        timer.Start()


    End Sub

    Private Sub Timer_Tick(sender As Object, e As EventArgs)
        ' Simulation de changement de vitesse et de carburant
        Dim currentTime = DateTime.Now
        heureActuelle = DateTime.Now.ToString("HH:mm:ss")
        Dim deltaTime = CSng((currentTime - lastTickTime).TotalSeconds) ' Temps écoulé en secondes
        lastTickTime = currentTime ' Mémorisez pour le prochain tick

        If(Not replay) Then
            acceleration = newValue * Voiture.Acceleration
            If newValue < 0 Then 
                acceleration = newValue * Voiture.Deceleration
            End If

            If voiture.Carburant <= 0 And newValue > 0 Then
                voiture.Carburant = 0.0F
                newValue = 0.0F
            End If

            If vitesse < 0.0F Then 
                vitesse = 0.0F
                newValue = 0.0F
            End If

            If vitesse > Voiture.V_max Then
                vitesse = Voiture.V_max
                newValue = 0.0F
            End If

            distanceParcourue += (1/2)* acceleration * (10.0 / 36.0) * deltaTime * deltaTime + vitesse * (10.0 / 36.0) * deltaTime
            vitesse = Voiture.Accelerer(newValue,deltaTime,vitesse)  ' Utiliser la nouvelle valeur pour ajuster la vitesse
        Else
            Static indexEvent As Integer = 0
            Static dureeActuel As Double = 0.0
            Static dureeEvent As Double = 0.0
            Static evenements As List(Of Evenement) = EvenementDao.GetAllByIdVoiture(Voiture.Id)

            ' Recommencer si on arrive à la fin
            If indexEvent >= evenements.Count - 1 Then
                timer.Stop()
            End If

            ' Récupération des événements courant et suivant
            Dim evt As Evenement = evenements(indexEvent)
            If(indexEvent + 1 >= evenements.Count) Then
                ' Si on est à la fin, on ne peut pas accéder à l'événement suivant
                timer.Stop()
                MessageBox.Show("Fin de l'animation")
                Return
            End If
            Dim evtSuiv As Evenement = evenements(indexEvent + 1)

            If dureeActuel = 0.0 Then
                vitesse = evt.VInit
                dureeEvent = (evtSuiv.DateEvenement - evt.DateEvenement).TotalSeconds
            End If

            ' Même calcul que dans le mode normal
            vitesse = voiture.AccelererByAccel(evt.Acceleration,deltaTime,vitesse)
            distanceParcourue += (1/5) * evt.Acceleration * (10.0 / 36.0) * deltaTime * deltaTime + vitesse * (10.0 / 36.0) * deltaTime
            Console.WriteLine(distanceParcourue)

            dureeActuel += deltaTime

            If dureeActuel >= dureeEvent Then
                indexEvent += 1
                dureeActuel = 0.0
            End If
        End If


        Me.Invalidate() ' Redessiner le formulaire
    End Sub


    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias

        ' Dessiner le fond
        Using brush As New SolidBrush(couleurFond)
            g.FillRectangle(brush, ClientRectangle)
        End Using

        ' Afficher l'horloge
        DessinerHorloge(g, New Point(600, 250))


        ' Dessiner le compteur de vitesse analogique
        DessinerCompteurVitesse(g, New Rectangle(100, 100, 300, 300), vitesse, 0, Voiture.V_max)

        ' Dessiner la jauge de carburant
        DessinerJaugeCarburant(g, New Rectangle(450, 100, 200, 100), Voiture.Carburant_max ,Voiture.Carburant) 

        ' Afficher la vitesse numérique
        DessinerVitesseNumerique(g, vitesse)

        ' Afficher la distance parcourue
        DessinerDistanceParcourue(g, distanceParcourue)

        ' Afficher la marque et modèle
        DessinerMarqueModele(g, Voiture.Marque)
    End Sub

    Private Sub BtnStop_Click(sender As Object, e As EventArgs)
        timer.Stop()
        Me.Focus() ' Reprendre le focus pour continuer à capter le clavier
    End Sub

    Private Sub BtnStart_Click(sender As Object, e As EventArgs)
        timer.Start()
        Me.Focus() ' Reprendre le focus aussi
    End Sub

    Private Sub DessinerHorloge(g As Graphics, position As Point)
        Dim heureActuelle As String = DateTime.Now.ToString("HH:mm:ss")
        Dim fontHorloge As New Font("Segoe UI", 16, FontStyle.Bold)
        Dim brushHorloge As New SolidBrush(Color.White)
        g.DrawString(heureActuelle, fontHorloge, brushHorloge, position)
    End Sub

    
    Private Sub DessinerCompteurVitesse(g As Graphics, rect As Rectangle, valeur As Integer, min As Integer, max As Integer)
        ' Dessiner le cadran
        Using pen As New Pen(couleurCadran, 10)
            g.DrawArc(pen, rect, 135, 270)
        End Using

        ' Dessiner les marques
        For i As Integer = min To max Step 20
            ' Ajustement de l'angle pour que 0 commence au bon endroit
            Dim angle As Single = 225 + ((i - min) / CSng(max - min)) * 270
            Dim pt1 As PointF = PointSurCercle(rect, angle, 0.9F)
            Dim pt2 As PointF = PointSurCercle(rect, angle, If(i Mod 40 = 0, 0.8F, 0.85F))

            Using pen As New Pen(couleurTexte, If(i Mod 40 = 0, 3, 2))
                g.DrawLine(pen, pt1, pt2)
            End Using

            ' Ajouter les nombres pour les grandes marques
            If i Mod 40 = 0 OrElse i = min Then ' Inclure le minimum (0)
                Dim ptText As PointF = PointSurCercle(rect, angle, 0.7F)
                Dim textSize As SizeF = g.MeasureString(i.ToString(), Me.Font)
                ptText.X -= textSize.Width / 2
                ptText.Y -= textSize.Height / 2

                Using brush As New SolidBrush(couleurTexte)
                    g.DrawString(i.ToString(), Me.Font, brush, ptText)
                End Using
            End If
        Next

        ' Dessiner l'aiguille - ajuster le calcul pour inclure le min
        Dim angleAiguille As Single = 225 + ((valeur - min) / CSng(max - min)) * 270
        Dim ptCentre As PointF = New PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2)
        Dim ptAiguille As PointF = PointSurCercle(rect, angleAiguille, 0.8F)

        Using pen As New Pen(couleurAiguille, 3)
            g.DrawLine(pen, ptCentre, ptAiguille)
        End Using

        ' Dessiner le centre du compteur
        Using brush As New SolidBrush(couleurAiguille)
            g.FillEllipse(brush, ptCentre.X - 5, ptCentre.Y - 5, 10, 10)
        End Using

        ' Ajouter le label
        Using brush As New SolidBrush(couleurTexte)
            Dim fontLabel As New Font(Me.Font.FontFamily, 12, FontStyle.Bold)
            Dim text As String = "km/h"
            Dim textSize As SizeF = g.MeasureString(text, fontLabel)
            Dim ptText As New PointF(ptCentre.X - textSize.Width / 2, ptCentre.Y + rect.Height * 0.2F)

            g.DrawString(text, fontLabel, brush, ptText)
        End Using
    End Sub

    Private Sub DessinerJaugeCarburant(g As Graphics, rect As Rectangle, carburantMax As Single, niveauActuel As Single)
    ' Dessiner le contour
    Using pen As New Pen(couleurTexte, 2)
        g.DrawRectangle(pen, rect)
    End Using

    ' Calculer le pourcentage actuel
    Dim pourcentage As Single = 0
    If carburantMax > 0 Then
        pourcentage = (niveauActuel / carburantMax) * 100
    End If

    ' Dessiner le niveau de carburant
    Dim niveauRect As New Rectangle(rect.X + 2, rect.Y + 2, CInt((rect.Width - 4) * (pourcentage / 100)), rect.Height - 4)
    Dim couleurNiveau As Color = If(pourcentage > 20, Color.FromArgb(50, 180, 50), Color.FromArgb(220, 50, 50))

    Using brush As New SolidBrush(couleurNiveau)
        g.FillRectangle(brush, niveauRect)
    End Using

    ' Ajouter le texte (affichage en pourcentage)
    Using brush As New SolidBrush(couleurTexte)
        Dim fontLabel As New Font(Me.Font.FontFamily, 10, FontStyle.Bold)
        Dim text As String = $"CARBURANT: {pourcentage:F0}%"
        Dim textSize As SizeF = g.MeasureString(text, fontLabel)
        Dim ptText As New PointF(rect.X + rect.Width / 2 - textSize.Width / 2, rect.Y + rect.Height + 5)

        g.DrawString(text, fontLabel, brush, ptText)
    End Using
End Sub

    Private Sub DessinerVitesseNumerique(g As Graphics, vitesse As Double)
        Using brush As New SolidBrush(couleurTexte)
            Dim font As New Font("Digital-7", 48, FontStyle.Bold)
            Dim text As String = vitesse.ToString("F1")+ " km/h"
            Dim textSize As SizeF = g.MeasureString(text, font)
            ' Dim ptText As New PointF(Me.ClientSize.Width / 2 - textSize.Width / 2, 360)
            ' Dim ptText As New PointF(Me.ClientSize.Width - textSize.Width - 20, 360) ' 20px depuis la droite
            Dim ptText As New PointF(50, 400) ' 50 pixels depuis la gauche

            g.DrawString(text, font, brush, ptText)
        End Using
    End Sub

    Private Sub DessinerDistanceParcourue(g As Graphics, distance As Single)
        Using brush As New SolidBrush(couleurTexte)
            Dim font As New Font(Me.Font.FontFamily, 12, FontStyle.Bold)

            ' Fusionner la distance et la consommation dans une seule chaîne
            Dim texteComplet As String = $"DISTANCE: {distance/1000:F1} km" & vbCrLf &
                                        $"Consommation: {Voiture.ConsommationMoyenne(distance/1000):F2} L/100"

            Dim textSize As SizeF = g.MeasureString(texteComplet, font)
            Dim ptText As New PointF(Me.ClientSize.Width - textSize.Width - 20, Me.ClientSize.Height - textSize.Height - 20)

            g.DrawString(texteComplet, font, brush, ptText)
        End Using
    End Sub


    Private Sub DessinerMarqueModele(g As Graphics, texte As String)
        Using brush As New SolidBrush(couleurTexte)
            Dim font As New Font("Arial", 16, FontStyle.Bold)
            Dim textSize As SizeF = g.MeasureString(texte, font)
            Dim ptText As New PointF(20, 20)

            g.DrawString(texte, font, brush, ptText)
        End Using
    End Sub

    Private Function PointSurCercle(rect As Rectangle, angle As Single, rayonRelatif As Single) As PointF
        Dim centreX As Single = rect.X + rect.Width / 2
        Dim centreY As Single = rect.Y + rect.Height / 2
        Dim rayon As Single = rect.Width / 2 * rayonRelatif

        ' Convertir l'angle en radians (avec ajustement pour le 0° en haut)
        Dim angleRad As Single = (angle - 90) * CSng(Math.PI) / 180.0F

        Return New PointF(
            centreX + rayon * CSng(Math.Cos(angleRad)),
            centreY + rayon * CSng(Math.Sin(angleRad)))
    End Function
End Class