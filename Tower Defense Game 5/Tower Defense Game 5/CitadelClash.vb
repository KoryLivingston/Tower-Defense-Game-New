
Public Class CitadelClash

    '972 Lines of Code in Total




    'List to hold every tower object to allow towers to be sold and to be removed from the list without affecting the logic of the code
    'Arrays of Tower and pictureboxes for each tower

    Private currentTowers As New List(Of Tower)
    Private Towers() As Tower
    Private PicTower() As PictureBox

    'Arrays of Panel, Label and pictureboxes for the UI for each tower

    Private TowerUI() As Panel
    Private LblBuffPrice() As Label
    Private UpgradeSlots1() As PictureBox
    Private UpgradeSlots2() As PictureBox
    Private UpgradeSlots3() As PictureBox


    'Towercount is used to redclare the arrays to hold the new position of the next tower to be created
    'Towerplacing is used to determine whether the player is placing a tower and to run certain code while doing so

    Private TowerCount As Integer
    Private TowerPlacing As Boolean

    'ClickedTower holds the tower object that has been clicked, this will then be used in the code involving the tower UI
    'Index holds the position of this tower in the list, so if it is sold it can be removed from the list

    Private clickedTower As Tower
    Private Index As Integer


    'Player stats

    Private Lives As Integer = 10
    Private Coins As Integer = 60
    Private Wave As Integer = 1

    Private LeaderBoard As New LeaderBoardInfo
    Private EnemyTools As New EnemyInfo


    Private Sub StartButton_Click(sender As Object, e As EventArgs) Handles StartButton.Click


        'Once the startbutton is clicked it:
        'Spawns the enemies for wave 1
        'Initializes the labels to be used in the leaderboard
        'Loads the game UI
        'Starts the games timers

        EnemyTools.WaveSpawn()
        LeaderBoard.InitializeLabels()
        LoadUI()
        startGameTimers()


    End Sub

    Private Sub GameLogic_Tick(sender As Object, e As EventArgs) Handles GameLogic.Tick

        'Updates Player Stats Labels

        LblLives.Text = "LIVES " & Lives
        LblCoins.Text = "COINS " & Coins
        LblWave.Text = "WAVE " & Wave

        'Checks if a wave has ended by comparing the enemieskilled to the totalenemies for that wave
        'Waveended is used to prevent the condition from running again until the next wave has been called from pressing the next wave button
        'Starts the wavecompletionUI timer to simulate the label showing then dissapearing 2 seconds later and to show the nextwavebutton once this has been done


        EnemyTools.CheckWaveEnded()

        'Once the player Lives reaches 0 or they complete the final wave it ends the game

        If Lives = 0 Or Wave = 51 Then
            Endgame()
        End If

        'UI brought to the front when in use to prevent the tower indicator from overlapping it

        LblWaveCompleted.BringToFront()
        NextWaveButton.BringToFront()
        CancelPlacing.BringToFront()

    End Sub

    Private Sub EnemyLogic_Tick(sender As Object, e As EventArgs) Handles EnemyLogic.Tick

        EnemyTools.EnemyConditions()
        EnemyTools.CheckToSpawnMore()

    End Sub

    Private Sub TowerLogic_Tick(sender As Object, e As EventArgs) Handles TowerLogic.Tick

        'Updates Tower Cap Label

        LblTowerCapacity.Text = "Tower Capacity" & " " & TowerCount & "/25"


        'For every tower find it a target and update this towers buff price label so that it holds the price of the next available upgrade

        For counter = 0 To TowerCount - 1

            currentTowers(counter).FindTarget()

            currentTowers(counter).setLblBuffPrice(currentTowers(counter).getcurrentUpgrade)

        Next

        'Timer is used to allow the adjusting of the tower indicators psotion to be as responsive as possible
        'Adjust the tower indicators position so that its center is at the cursors position

        If TowerPlacing = True Then

            Dim p As New Point(18, 16)
            TowerIndicator.Location = New Point(PointToClient(Cursor.Position) - p)

        End If

    End Sub


    Private Sub WaveCompletionUI_Tick(sender As Object, e As EventArgs) Handles WaveCompletionUI.Tick

        LblWaveCompleted.Hide()
        WaveCompletionUI.Stop()
        NextWaveButton.Show()
        NextWaveButton.BringToFront()

    End Sub




    'If the nextwavebutton is pressed it hides it and increases the wave by one
    'Checks if the wave is now any of the waves that brings in a new type of enemy so that additionalenemies can be set to 0 for the new enemy type
    'additionalenemies is increased by 2 to allow a new total of enemies to be brought into the next wave
    'Wavespawn sub routine is called to spawn the next wave


    Private Sub NextWaveButton_Click(sender As Object, e As EventArgs) Handles NextWaveButton.Click

        NextWaveButton.Hide()

        EnemyTools.NextWave()
        EnemyTools.WaveSpawn()


    End Sub

    'If this timer starts it will then spawn in another 10 enemies, If at any point enemynum is no longer less than totalenemiesinwave then stop the timer

    Private Sub EnemySpawnTimer_Tick(sender As Object, e As EventArgs) Handles NextSpawnDelay.Tick

        EnemyTools.SpawnNextGroup()

    End Sub

    'Runs if the player has clicked the UI to buy a tower
    'If they have enough coins and are currently not placing a tower as well as the total towers placed has yet to reach 25 and the game hasn't ended
    'Then set towerplacing to true and show the cancel button

    Private Sub TowerBuy1_Click(sender As Object, e As EventArgs) Handles TowerBuy1.Click

        If Coins >= 30 And TowerPlacing = False AndAlso TowerCount < 25 AndAlso Lives > 0 Then

            TowerPlacing = True
            CancelPlacing.Show()

            TowerIndicator.Show()
            TowerIndicator.BringToFront()
            TowerIndicator.BackColor = Color.FromArgb(200, TowerIndicator.BackColor)

        End If

    End Sub

    Private Sub TowerIndicator_Click(sender As Object, e As EventArgs) Handles TowerIndicator.Click

        Dim p As New Point(18, 16)

        'Occurs if the player clicks the towerindicator and is in tower placing mode
        'Checks if the tower indicator is not in collision with any picturebox to ensure that the spawn location is valid
        'If the location is valid then it will hide the UI used during tower placing and create a tower to be placed at this new set position

        For Each PictureBox As PictureBox In Controls.OfType(Of PictureBox)

            If PictureBox IsNot TowerIndicator AndAlso TowerIndicator.Bounds.IntersectsWith(PictureBox.Bounds) Then
                Exit Sub
            End If

        Next

        TowerIndicator.Hide()
        CancelPlacing.Hide()

        'Redeclares PicTower to towercount to allow a new tower picturebox to be created

        ReDim Preserve PicTower(TowerCount)


        PicTower(TowerCount) = New PictureBox With {
    .Size = New Size(37, 33),
    .BackColor = Color.CadetBlue,
    .Name = "PicTower" & TowerCount.ToString,
    .Location = PointToClient(Cursor.Position) - p
                    }

        'Adds it to the forms controls so that it can be used

        Controls.Add(PicTower(TowerCount))
        PicTower(TowerCount).BringToFront()

        'Adds this picturebox click event as a handler of the tower_click sub routine

        AddHandler PicTower(TowerCount).Click, AddressOf Tower_Click

        'Redeclares TowerUI to towercount to allow a new tower UI to be created

        ReDim Preserve TowerUI(TowerCount)

        TowerUI(TowerCount) = New Panel With {
    .Size = New Size(108, 164),
    .BackColor = Color.FromArgb(130, TurretPanel.BackColor),
    .Location = New Point(-1000, 100)
                }

        Controls.Add(TowerUI(TowerCount))

        'Redeclares LBlBuffPrice to allow a new tower buff price label to be created

        ReDim Preserve LblBuffPrice(TowerCount)

        LblBuffPrice(TowerCount) = New Label With {
    .Font = New Font("Agency FB", 10, FontStyle.Bold),
    .ForeColor = Color.White,
    .TextAlign = 2,
    .Size = New Size(82, 17),
    .BackColor = Color.FromArgb(130, TurretPanel.BackColor),
    .Location = New Point(14, 50)
            }

        'Added to the towerUI Controls as it will be used in this panel

        TowerUI(TowerCount).Controls.Add(LblBuffPrice(TowerCount))

        'Redcalres every upgradeslot array before use

        ReDim Preserve UpgradeSlots1(TowerCount)
        ReDim Preserve UpgradeSlots2(TowerCount)
        ReDim Preserve UpgradeSlots3(TowerCount)

        'Creates every upgrade slot for this tower

        CreateUpgradeSlots(UpgradeSlots1, 14, 5)
        CreateUpgradeSlots(UpgradeSlots2, 45, 5)
        CreateUpgradeSlots(UpgradeSlots3, 75, 5)

        'New Tower is created

        ReDim Preserve Towers(TowerCount)
        Towers(TowerCount) = New Tower(PicTower(TowerCount), TowerUI(TowerCount), LblBuffPrice(TowerCount), UpgradeSlots1(TowerCount), UpgradeSlots2(TowerCount), UpgradeSlots3(TowerCount), 1, 100)
        currentTowers.Add(Towers(TowerCount))

        Coins -= 30
        TowerCount += 1
        TowerPlacing = False

    End Sub

    Public Sub CreateUpgradeSlots(Slot() As PictureBox, LocationX As Integer, LocationY As Integer)

        Slot(TowerCount) = New PictureBox With {
    .Size = New Size(21, 11),
    .BackColor = Color.White,
    .Location = New Point(LocationX, LocationY)
            }

        TowerUI(TowerCount).Controls.Add(Slot(TowerCount))

    End Sub

    'If the cancel Placing button is pressed then stop the player from placing a tower and hide the relevant UI

    Private Sub CancelPlacing_Click(sender As Object, e As EventArgs) Handles CancelPlacing.Click

        TowerPlacing = False
        TowerIndicator.Hide()
        CancelPlacing.Hide()

    End Sub

    'Timer that runs every second and works as if the tower is dealing damage to the enemy every second

    Private Sub TowerShooting_Tick(sender As Object, e As EventArgs) Handles TowerShooting.Tick

        For counter = 0 To TowerCount - 1
            currentTowers(counter).shootTarget()
        Next

    End Sub


    'Runs if a tower is clicked

    Public Sub Tower_Click(sender As Object, e As EventArgs)


        'If clickedTower has already been declared then this must be the second time that the same tower was clicked
        'First time brings up the UI, and this second time will remove it

        If clickedTower IsNot Nothing Then

            clickedTower.getTowerUI.Hide()
            clickedTower = Nothing

            Exit Sub

        End If

        'Find the tower object that has been clicked and set it to clickedtower

        For counter = 0 To TowerCount - 1

            If sender Is currentTowers(counter).getTowerGraphic Then
                clickedTower = currentTowers(counter)
                Index = counter
                Exit For
            End If

        Next


        'Once clickedtower has been assigned a value then show the towers UI

        If clickedTower IsNot Nothing Then

            With clickedTower
                .getTowerUI.Show()
                .getTowerUI.bringtofront()
                .getTowerGraphic.bringtofront()

                .getTowerUI.Location = New Point(clickedTower.getTowerGraphic.location.x - 36, clickedTower.getTowerGraphic.location.y - 80)
                .getTowerUI.controls.add(SellButton)
                .getTowerUI.controls.add(DamageBuffButton)
                SellButton.Location = New Point(14, 130)
                DamageBuffButton.Location = New Point(14, 22)

            End With

        End If


    End Sub


    Private Sub SellButton_Click(sender As Object, e As EventArgs) Handles SellButton.Click

        'If the sell button is clicked then increase the players coins by 20 and delete the tower and hide the towers UI

        Coins += 20


        Controls.Remove(currentTowers(Index).getTowerGraphic)
        currentTowers.RemoveAt((Index))
        TowerCount -= 1

        clickedTower.getTowerUI.Hide()
        clickedTower = Nothing


    End Sub

    Private Sub DamageBuffbutton_Click(sender As Object, e As EventArgs) Handles DamageBuffButton.Click

        'If the damageBuffbutton is clicked and the player has enough coins and the tower is currently not fully upgraded
        'Then upgrade the tower to the next availble upgrading, increasing its damage by 1 with every succesive one, changing the upgradeslot of the associated upgrade to green to indicate it has been bought
        'deduct the buffprice from the coins and set the new buffprice for the next upgrade as well increasing currentUpgrade by 1 so that the next upgrade can be bought


        With clickedTower
            If Coins >= .getBuffprice And .getcurrentUpgrade <= 2 Then


                .setDamage(2 + .getcurrentUpgrade)

                .getUpgradeSlots(.getcurrentUpgrade).BackColor = Color.Green
                Coins -= .getBuffprice
                .setBuffPrice(250 + (.getcurrentUpgrade * 250))
                .setcurrentUpgrade(1)

            End If
        End With

    End Sub


    'Runs if the player dies or completes the last wave
    'Stops the game from running by stopping the timers and displays the game over screen while starting the timer for the leaderboard to be shown 2.5 seconds later

    Public Sub Endgame()

        GameLogic.Stop()
        EnemyLogic.Stop()
        TowerLogic.Stop()
        TowerShooting.Stop()

        LblGameOver.Show()
        LblGameOver.BringToFront()

        LeaderBoardDelay.Start()

    End Sub
    Private Sub LeaderBoardDelay_Tick(sender As Object, e As EventArgs) Handles LeaderBoardDelay.Tick

        'Hides Game over screen
        'Moves the leaderboard panel to cover the screen and stops the timer to prevent it from being shown once the next game has started

        LblGameOver.Hide()
        LeaderBoardPanel.Location = New Point(0, 0)
        LeaderBoardPanel.Show()
        LeaderBoardPanel.BringToFront()
        LeaderBoardDelay.Stop()


    End Sub

    'Runs if there is a key that is pressed down while using the newName text box

    Private Sub newName_KeyDown(sender As Object, e As KeyEventArgs) Handles newName.KeyDown

        'If the key was enter and the name entered is valid then it hides the previous leaderbaord UI
        'Declares the new name and waves reached to be held in the 6th position of the arrays for playernames and waves reached

        If e.KeyCode = Keys.Enter And newName.Text.Length = 3 Then

            newName.Hide()
            LblEnterUsername.Hide()


            With LeaderBoard

                .setUserName(newName.Text)
                .set5thPlayer(.getUserName, Wave)
                .InsertionSortLeaderBoard()

            End With

            'Displays leaderbaord UI

            UpdateTableButton.Show()
            LblPlayerNamesHeading.Show()
            LblWavesReachedHeading.Show()

        End If

    End Sub




    Private Sub RetryButton_Click(sender As Object, e As EventArgs) Handles RetryButton.Click


        'Hides Leaderboard UI and resets the text box and shows the tex box for next use

        newName.Show()
        LblEnterUsername.Show()
        LblPlayerNamesHeading.Hide()
        LblWavesReachedHeading.Hide()
        UpdateTableButton.Hide()
        LeaderBoardPanel.Hide()
        newName.Text = Nothing

        For counter = 0 To 4
            LeaderBoard.hideLabels(counter)
        Next

        LeaderBoard.resetStats()
        'Resets player stats

        Lives = 0
        Coins = 60
        Wave = 15


        'Resets the game for enemies

        EnemyTools.setadditionalEnemies(Nothing)
        EnemyTools.InitializeEnemies()

        'Resets the game for towers

        For counter = 0 To TowerCount - 1
            Controls.Remove(currentTowers(counter).getTowerGraphic)
        Next

        Towers = Nothing
        PicTower = Nothing
        TowerUI = Nothing
        LblBuffPrice = Nothing
        UpgradeSlots1 = Nothing
        UpgradeSlots2 = Nothing
        UpgradeSlots3 = Nothing
        currentTowers.Clear()
        TowerCount = 0
        TowerPlacing = False

        startGameTimers()


        'Spawns wave 1 again

        EnemyTools.WaveSpawn()


    End Sub

    Private Sub QuitButton_Click(sender As Object, e As EventArgs) Handles QuitButton.Click

        Close()

    End Sub


    Public Sub startGameTimers()

        GameLogic.Start()
        EnemyLogic.Start()
        TowerLogic.Start()
        TowerShooting.Start()

    End Sub

    Public Sub LoadUI()

        StartButton.Hide()
        QuitButton.Hide()

        LblLives.Show()
        LblCoins.Show()
        LblWave.Show()

        TurretPanel.Show()
        LblTower1Cost.Show()
        TowerBuy1.Show()
        DamageBuffButton.Show()
        LblTowerCapacity.Show()
        TurretPanel.BackColor = Color.FromArgb(130, TurretPanel.BackColor)
        LblTower1Cost.BackColor = Color.FromArgb(130, TurretPanel.BackColor)
        LblTowerCapacity.BackColor = Color.FromArgb(130, TurretPanel.BackColor)

    End Sub

    Public Sub setLives(LivesLost As Integer)

        Lives -= LivesLost

    End Sub

    Public Sub setCoins(CoinsEarned As Integer)

        Coins += CoinsEarned

    End Sub

    Public Sub setWave()

        Wave += 1

    End Sub

    Public Function getWave()

        Return Wave

    End Function

    Public Function getTowers()

        Return Towers

    End Function

    Public Function getEnemyTools()

        Return EnemyTools

    End Function

    Public Function getEnemyToolsEnemies()

        Return EnemyTools.getEnemiesFully

    End Function
    Public Function getLeaderBoard()

        Return LeaderBoard

    End Function

End Class




