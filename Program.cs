using SplashKitSDK;
using System;

public class Program
{
    public static void Main()
    {
        Window gameWindow = new Window("MineSweeper", 400, 400);
        GameBoard gameBoard = new GameBoard();
        bool startNewGame = false;
        while (!gameWindow.CloseRequested)
        {
            gameBoard.Draw();
            SplashKit.ProcessEvents();
            if (!gameBoard.GameOver)
            {
                if (SplashKit.MouseClicked(MouseButton.LeftButton))
                {
                    gameBoard.RevealCellAtPoint(SplashKit.MousePosition());
                }

                if (SplashKit.MouseClicked(MouseButton.RightButton))
                {
                    gameBoard.FlagCellAtPoint(SplashKit.MousePosition());
                }
            }
            else
            {
                if (gameBoard.PlayerWon)
                {
                    SplashKit.DrawText("You won! Click to play again.", Color.Yellow, "Arial.ttf", 30, 10, 150);
                }
                else
                {
                    SplashKit.DrawText("You lost! Click to play again.", Color.Red, "Arial.ttf", 30, 10, 150);
                }
                if (SplashKit.MouseClicked(MouseButton.LeftButton))
                {
                    startNewGame = true;
                }
            }
            gameWindow.Refresh(60);

            if (startNewGame)
            {
                gameBoard = new GameBoard();
                startNewGame = false;
            }
        }
    }
}

public class Cell
{
    public bool HasMine { get; set; }
    public bool IsRevealed { get; set; }
    public int SurroundingMines { get; set; }
    public bool IsFlagged { get; set; }
}

public class GameBoard
{
    private const int Rows = 10;
    private const int Columns = 10;
    private const int CellSize = 40;
    private Cell[,] _cells = new Cell[Rows, Columns];
    private Random _random = new Random();
    private int _mines;
    private int _revealedCells;
    private bool _gameOver;
    private bool _playerWon;
    private Bitmap _flagBitmap;
    public bool GameOver
    {
        get { return _gameOver; }
        set { _gameOver = value; }
    }
    public bool PlayerWon
    {
        get { return _playerWon; }
        set { _playerWon = value; }
    }

    public GameBoard()
    {
        InitializeBoard();
        _mines = 10;
        _revealedCells = 0;
        _gameOver = false;
        _playerWon = false;
        _flagBitmap = new Bitmap("flag", "flag.png");
    }

    private void InitializeBoard()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                _cells[i, j] = new Cell();
            }
        }

        PlaceMines(10);
        CalculateSurroundingMines();
    }

    private void PlaceMines(int mineCount)
    {
        int placedMines = 0;

        while (placedMines < mineCount)
        {
            int row = _random.Next(Rows);
            int col = _random.Next(Columns);

            if (!_cells[row, col].HasMine)
            {
                _cells[row, col].HasMine = true;
                placedMines++;
            }
        }
    }

    private void CalculateSurroundingMines()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (!_cells[i, j].HasMine)
                {
                    int count = 0;

                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            int newRow = i + x;
                            int newCol = j + y;

                            if (newRow >= 0 && newRow < Rows && newCol >= 0 && newCol < Columns && _cells[newRow, newCol].HasMine)
                            {
                                count++;
                            }
                        }
                    }

                    _cells[i, j].SurroundingMines = count;
                }
            }
        }
    }

    public void Draw()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                int x = j * CellSize;
                int y = i * CellSize;

                if (_cells[i, j].IsRevealed)
                {
                    SplashKit.FillRectangle(Color.Gray, x, y, CellSize, CellSize);

                    if (_cells[i, j].HasMine)
                    {
                        SplashKit.FillCircle(Color.Black, x + CellSize / 2, y + CellSize / 2, CellSize / 4);
                    }
                    else if (_cells[i, j].SurroundingMines == 1)
                        SplashKit.DrawText(_cells[i, j].SurroundingMines.ToString(), Color.Blue, "Arial.ttf", 18, x + CellSize / 2 - 5, y + CellSize / 2 - 10);
                    else if (_cells[i, j].SurroundingMines == 2)
                        SplashKit.DrawText(_cells[i, j].SurroundingMines.ToString(), Color.Green, "Arial.ttf", 18, x + CellSize / 2 - 5, y + CellSize / 2 - 10);
                    else if (_cells[i, j].SurroundingMines == 3)
                        SplashKit.DrawText(_cells[i, j].SurroundingMines.ToString(), Color.Red, "Arial.ttf", 18, x + CellSize / 2 - 5, y + CellSize / 2 - 10);
                    else if (_cells[i, j].SurroundingMines == 4)
                        SplashKit.DrawText(_cells[i, j].SurroundingMines.ToString(), Color.HotPink, "Arial.ttf", 18, x + CellSize / 2 - 5, y + CellSize / 2 - 10);
                    else if (_cells[i, j].SurroundingMines == 5)
                        SplashKit.DrawText(_cells[i, j].SurroundingMines.ToString(), Color.Brown, "Arial.ttf", 18, x + CellSize / 2 - 5, y + CellSize / 2 - 10);
                    else if (_cells[i, j].SurroundingMines == 6)
                        SplashKit.DrawText(_cells[i, j].SurroundingMines.ToString(), Color.Purple, "Arial.ttf", 18, x + CellSize / 2 - 5, y + CellSize / 2 - 10);
                    else if (_cells[i, j].SurroundingMines == 7)
                        SplashKit.DrawText(_cells[i, j].SurroundingMines.ToString(), Color.YellowGreen, "Arial.ttf", 18, x + CellSize / 2 - 5, y + CellSize / 2 - 10);
                    else if (_cells[i, j].SurroundingMines == 8)
                        SplashKit.DrawText(_cells[i, j].SurroundingMines.ToString(), Color.White, "Arial.ttf", 18, x + CellSize / 2 - 5, y + CellSize / 2 - 10);
                }
                else
                {
                    SplashKit.FillRectangle(Color.LightGray, x, y, CellSize, CellSize);
                }

                SplashKit.DrawRectangle(Color.Black, x, y, CellSize, CellSize);
            }
        }

        for (int m = 0; m < Rows; m++)
        {
            for (int n = 0; n < Columns; n++)
            {
                if (_cells[m, n].IsFlagged)
                {
                    int x = (n - 1) * CellSize;
                    int y = (m - 1) * CellSize;
                    SplashKit.DrawBitmap(_flagBitmap, x - 2, y - 3, SplashKit.OptionScaleBmp(CellSize / (float)_flagBitmap.Width, CellSize / (float)_flagBitmap.Height));
                }
            }
        }
    }
    public void RevealCellAtPoint(Point2D point)
    {
        int row = (int)point.Y / CellSize;
        int col = (int)point.X / CellSize;

        if (row >= 0 && row < Rows && col >= 0 && col < Columns)
        {
            RevealCell(row, col);
        }
    }

    public void FlagCellAtPoint(Point2D point)
    {
        int row = (int)point.Y / CellSize;
        int col = (int)point.X / CellSize;

        if (row >= 0 && row < Rows && col >= 0 && col < Columns)
        {
            if (!_cells[row, col].IsRevealed)
            {
                _cells[row, col].IsFlagged = !_cells[row, col].IsFlagged;
            }
        }
    }

    private void RevealCell(int row, int col)
    {
        if (_gameOver || _cells[row, col].IsRevealed || _cells[row, col].IsFlagged) return;

        _cells[row, col].IsRevealed = true;
        _revealedCells++;

        if (_cells[row, col].HasMine)
        {
            _gameOver = true;
            _playerWon = false;
            return;
        }

        if (Rows * Columns - _revealedCells == _mines)
        {
            _gameOver = true;
            _playerWon = true;
            return;
        }

        if (_cells[row, col].SurroundingMines == 0 && !_cells[row, col].HasMine)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int newRow = row + x;
                    int newCol = col + y;

                    if (newRow >= 0 && newRow < Rows && newCol >= 0 && newCol < Columns)
                    {
                        RevealCell(newRow, newCol);
                    }
                }
            }
        }
    }
}
