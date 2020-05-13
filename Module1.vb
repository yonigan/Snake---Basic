' # Bare bones version of snake. L.Minett (2020)
Imports System.IO
Imports System.Console
Imports System.Text

Module Module1
    ' barebones version of snake
    Structure Coord
        Dim x As Short
        Dim y As Short
    End Structure

    Const AppleShape As Integer = &H25CF
    Const SnakeShape As Integer = 9632
    Const SnakeHead As Integer = 9633
    Dim SnakeTrail As New Queue(Of Coord)
    Dim GameWindow As New Coord With {.x = 80, .y = 40}
    Dim Bearing As New Coord With {.x = 1, .y = 0}
    Dim Tail As Short = 0
    Sub PlayBackgroundSoundFile()
        My.Computer.Audio.Play("C:\Users\yonig\OneDrive\Documents\Audacity\snake.wav",
        AudioPlayMode.BackgroundLoop)
    End Sub
    Sub Main()
        Dim highscore As Integer
        Dim score As Integer
        Dim Apple As New Coord With {.x = 5, .y = 5}
        Dim Player As New Coord With {.x = 10, .y = 10}
        Dim MovesWatch As New Stopwatch
        Dim interval As Short = 200 ' speed
        Dim Finished As Boolean = False

        SetWindowSize(GameWindow.x, GameWindow.y)
        OutputEncoding = Encoding.UTF8 ' enables full set of Unicode characters
        CursorVisible = False
        ForegroundColor = ConsoleColor.Yellow
        SetCursorPosition(0, 0)
        Write($"Score:{score.ToString.PadLeft(4, " ")}")
        SetCursorPosition(Apple.x, Apple.y)
        ForegroundColor = ConsoleColor.Red
        Write(ChrW(AppleShape))
        PlayBackgroundSoundFile()
        Dim fileReader As StreamReader
        fileReader = My.Computer.FileSystem.OpenTextFileReader("C:\Users\yonig\OneDrive\Documents\GitHub\Snake---Basic\highscore.txt")
        highscore = fileReader.ReadLine()
        fileReader.Close()
        ForegroundColor = ConsoleColor.Magenta
        SetCursorPosition(15, 0)
        Write($"Highscore:{highscore.ToString.PadLeft(4, " ")}")
        MovesWatch.Start()
        Do
            Do
                If KeyAvailable Then
                    Select Case ReadKey(True).Key
                        Case ConsoleKey.UpArrow
                            Bearing.y = -1
                            Bearing.x = 0
                        Case ConsoleKey.DownArrow
                            Bearing.y = 1
                            Bearing.x = 0
                        Case ConsoleKey.LeftArrow
                            Bearing.y = 0
                            Bearing.x = -1
                        Case ConsoleKey.RightArrow
                            Bearing.y = 0
                            Bearing.x = +1
                        Case ConsoleKey.Q
                            Finished = True
                    End Select
                    Exit Do
                End If
            Loop Until MovesWatch.ElapsedMilliseconds >= interval

            Player.x += Bearing.x
            Player.y += Bearing.y

            If Player.x <= 0 Then
                'Player.x = GameWindow.x - 1
                Finished = True
            ElseIf Player.x >= GameWindow.x Then
                'Player.x = 0
                Finished = True
            End If

            If Player.y <= 0 Then
                'Player.y = GameWindow.y - 1
                Finished = True
            ElseIf Player.y >= GameWindow.y Then
                'Player.y = 0
                Finished = True
            End If


            ForegroundColor = ConsoleColor.Green
            SetCursorPosition(Player.x, Player.y)
            Write(ChrW(SnakeShape))



            While SnakeTrail.Count > Tail 'if trail queue is longer than tail size, remove from queue
                SetCursorPosition(SnakeTrail.Peek.x, SnakeTrail.Peek.y) ' Position of earliest drawn snake trail
                SnakeTrail.Dequeue() ' remove position from trail
                Write(" ") ' cover snake tail

            End While

            If SnakeTrail.Contains(Player) Then
                Finished = True
            Else
                SnakeTrail.Enqueue(Player)

            End If

            If Apple.x = Player.x And Apple.y = Player.y Then
                Tail += 1
                score += 1
                ForegroundColor = ConsoleColor.Yellow
                SetCursorPosition(0, 0)
                Write($"Score:{score.ToString.PadLeft(4, " ")}")
                Apple.x = New Random().Next(5, GameWindow.x - 5)
                Apple.y = New Random().Next(5, GameWindow.y - 5)
                SetCursorPosition(Apple.x, Apple.y)
                ForegroundColor = ConsoleColor.Red
                Write(ChrW(AppleShape))
                interval -= 5
            End If

            MovesWatch.Restart()

        Loop Until Finished

        Dim path As String = "C:\Users\yonig\OneDrive\Documents\GitHub\Snake---Basic\highscore.txt"
        If highscore > score Then
        Else
            Dim fs As FileStream = File.Create(path)
            fs.Close()
            Dim filewrite As StreamWriter
            filewrite = My.Computer.FileSystem.OpenTextFileWriter("C:\Users\yonig\OneDrive\Documents\GitHub\Snake---Basic\highscore.txt", True)
            filewrite.WriteLine(score)
            filewrite.Close()
            highscore = score
        End If
        MsgBox("Game Over

" & "score = " & score & "

" & "highscore = " & highscore)
    End Sub

End Module
