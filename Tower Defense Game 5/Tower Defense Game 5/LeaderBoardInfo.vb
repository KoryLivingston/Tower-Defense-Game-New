﻿Public Class LeaderBoardInfo

    Private PlayerNames() As String = {"QSD", "BMV", "AJS", "123", "HJT", Nothing}
    Private WavesReached() As Integer = {"13", "10", "8", "12", "5", Nothing}
    Private NameLabels(4) As Label
    Private WaveReachedLabels(4) As Label
    Private UserName As String
    Private TotalEnemiesKilled As Integer
    Private TotalCoinsEarned As Integer


    Public Sub InitializeLabels()

        With CitadelClash
            NameLabels(0) = .LblName1
            NameLabels(1) = .LblName2
            NameLabels(2) = .LblName3
            NameLabels(3) = .LblName4
            NameLabels(4) = .LblName5

            WaveReachedLabels(0) = .LblWaveReached1
            WaveReachedLabels(1) = .LblWaveReached2
            WaveReachedLabels(2) = .LblWaveReached3
            WaveReachedLabels(3) = .LblWaveReached4
            WaveReachedLabels(4) = .LblWaveReached5
        End With

        For counter = 0 To 4

            NameLabels(counter).Text = PlayerNames(counter)
            WaveReachedLabels(counter).Text = WavesReached(counter)

        Next

    End Sub

    Public Sub InsertionSortLeaderBoard()

        For counter = 1 To 5

            Dim tempName = PlayerNames(counter)
            Dim tempWave = WavesReached(counter)
            Dim index = counter

            While index > 0 AndAlso tempWave > WavesReached(index - 1)

                WavesReached(index) = WavesReached(index - 1)
                PlayerNames(index) = PlayerNames(index - 1)

                index = index - 1
            End While

            WavesReached(index) = tempWave
            PlayerNames(index) = tempName

        Next

        'Sets each label to their associated playerName and waves reached for the first 5 positions and shows the leaderboard

        For counter = 0 To 4

            NameLabels(counter).Text = PlayerNames(counter)
            WaveReachedLabels(counter).Text = WavesReached(counter)

            NameLabels(counter).Show()
            WaveReachedLabels(counter).Show()

        Next
    End Sub

    Public Sub set5thPlayer(NewName As String, NewWavesReached As Integer)

        PlayerNames(5) = NewName
        WavesReached(5) = NewWavesReached

    End Sub

    Public Sub hideLabels(Index As Integer)

        NameLabels(Index).Hide()
        WaveReachedLabels(Index).Hide()

    End Sub

    Public Sub setUserName(NewName As String)

        UserName = NewName

    End Sub

    Public Function getUserName()

        Return UserName

    End Function

    Public Sub setTotalEnemiesKilled()

        TotalEnemiesKilled += 1

    End Sub


    Public Sub setTotalCoinsEarned(AmountEarned As Integer)

        TotalCoinsEarned += AmountEarned

    End Sub

    Public Sub resetStats()

        TotalEnemiesKilled = 0
        TotalCoinsEarned = 0

    End Sub

    Public Function getTotalEnemiesKilled()

        Return TotalEnemiesKilled

    End Function

    Public Function getTotalCoinsEarned()

        Return TotalCoinsEarned

    End Function


End Class
