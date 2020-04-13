' # Bare bones version of snake. L.Minett (2020)

Imports System.Console
Imports System.Text

Module Module1
    ' barebones version of snake
    Structure Coord
        Dim x As Short
        Dim y As Short
    End Structure

    Dim SnakeTrail As New Queue(Of Coord)
    Dim GameWindow As New Coord With {.x = 80, .y = 40}
    Dim Bearing As New Coord With {.x = 1, .y = 0}
    Dim Tail As Short = 5
    Sub Main()
        Dim Apple As New Coord With {.x = 5, .y = 5}
        Dim Player As New Coord With {.x = 10, .y = 10}
        Dim MovesWatch As New Stopwatch
        Dim interval As Short = 200 ' speed
        Dim Finished As Boolean = False

        SetWindowSize(GameWindow.x, GameWindow.y)
        OutputEncoding = Encoding.UTF8 ' enables full set of Unicode characters
        CursorVisible = False

        SetCursorPosition(Apple.x, Apple.y)
        ForegroundColor = ConsoleColor.Red
        Write("O")

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
                Player.x = GameWindow.x - 1
            ElseIf Player.x >= GameWindow.x Then
                Player.x = 0
            End If

            If Player.y <= 0 Then
                Player.y = GameWindow.y - 1
            ElseIf Player.y >= GameWindow.y Then
                Player.y = 0
            End If

            ForegroundColor = ConsoleColor.Green
            SetCursorPosition(Player.x, Player.y)
            Write(ChrW(9608))

            While SnakeTrail.Count > Tail 'if trail queue is longer than tail size, remove from queue
                SetCursorPosition(SnakeTrail.Peek.x, SnakeTrail.Peek.y) ' Position of earliest drawn snake trail
                SnakeTrail.Dequeue() ' remove position from trail
                Write(" ") ' cover snake tail
            End While

            If SnakeTrail.Contains(Player) Then
                Tail = 5
            Else
                SnakeTrail.Enqueue(Player)
            End If

            If Apple.x = Player.x And Apple.y = Player.y Then
                Tail += 1
                Apple.x = New Random().Next(0, GameWindow.x)
                Apple.y = New Random().Next(0, GameWindow.y)
                SetCursorPosition(Apple.x, Apple.y)
                ForegroundColor = ConsoleColor.Red
                Write("O")
            End If

            MovesWatch.Restart()
        Loop Until Finished
    End Sub

End Module
