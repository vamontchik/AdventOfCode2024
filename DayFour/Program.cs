PartOne(args[0]);
PartTwo(args[0]);

return;

void PartTwo(string path)
{
    var lines = File
        .ReadAllLines(Path.GetFullPath(path))
        .ToList();

    var row = 0;
    var column = 0;
    var rowWidth = lines[0].Length;
    var columnWidth = lines.Count;
    var boundary = rowWidth * columnWidth;
    var xmasCount = 0;
    while ((row * rowWidth + column) < boundary)
    {
        xmasCount += SearchForChristmasShape(lines, rowWidth, columnWidth, row, column);

        if (column == (rowWidth - 1))
        {
            ++row;
            column = 0;
        }
        else
        {
            ++column;
        }
    }

    Console.WriteLine("Found {0} XMAS squares", xmasCount);
}

static int SearchForChristmasShape(IReadOnlyList<string> lines, int rowWidth, int columnWidth, int r, int c)
{
    if ((r + 2) >= columnWidth || (c + 2) >= rowWidth)
        return 0;

    var normal =
        lines[r][c] == 'M' && lines[r][c + 2] == 'S' &&
        lines[r + 1][c + 1] == 'A' &&
        lines[r + 2][c] == 'M' && lines[r + 2][c + 2] == 'S'
            ? 1
            : 0;

    var right =
        lines[r][c] == 'M' && lines[r][c + 2] == 'M' &&
        lines[r + 1][c + 1] == 'A' &&
        lines[r + 2][c] == 'S' && lines[r + 2][c + 2] == 'S'
            ? 1
            : 0;

    var rightleft =
        lines[r][c] == 'S' && lines[r][c + 2] == 'M' &&
        lines[r + 1][c + 1] == 'A' &&
        lines[r + 2][c] == 'S' && lines[r + 2][c + 2] == 'M'
            ? 1
            : 0;

    var left =
        lines[r][c] == 'S' && lines[r][c + 2] == 'S' &&
        lines[r + 1][c + 1] == 'A' &&
        lines[r + 2][c] == 'M' && lines[r + 2][c + 2] == 'M'
            ? 1
            : 0;

    return normal + right + rightleft + left;
}

void PartOne(string path)
{
    var lines = File
        .ReadAllLines(Path.GetFullPath(path))
        .ToList();

    var row = 0;
    var column = 0;
    var rowWidth = lines[0].Length;
    var columnWidth = lines.Count;
    var boundary = rowWidth * columnWidth;
    var xmasCount = 0;
    while ((row * rowWidth + column) < boundary)
    {
        var nextChar = lines[row][column];

        if (nextChar == 'X')
            xmasCount += SearchForChristmas(lines, rowWidth, columnWidth, row, column);

        if (column == (rowWidth - 1))
        {
            ++row;
            column = 0;
        }
        else
        {
            ++column;
        }
    }

    Console.WriteLine("Found {0} XMAS strings", xmasCount);
}

static int SearchForChristmas(IReadOnlyList<string> lines, int rowWidth, int columnWidth, int r, int c)
{
    var total = 0;

    //up left
    if ((r - 3) >= 0 && (c - 3) >= 0)
        total += lines[r - 1][c - 1] == 'M' && lines[r - 2][c - 2] == 'A' && lines[r - 3][c - 3] == 'S' ? 1 : 0;

    //up
    if ((r - 3) >= 0)
        total += lines[r - 1][c] == 'M' && lines[r - 2][c] == 'A' && lines[r - 3][c] == 'S' ? 1 : 0;

    //up right
    if ((r - 3) >= 0 && (c + 3) < rowWidth)
        total += lines[r - 1][c + 1] == 'M' && lines[r - 2][c + 2] == 'A' && lines[r - 3][c + 3] == 'S' ? 1 : 0;

    //right
    if ((c + 3) < rowWidth)
        total += lines[r][c + 1] == 'M' && lines[r][c + 2] == 'A' && lines[r][c + 3] == 'S' ? 1 : 0;

    //right down
    if ((r + 3) < columnWidth && (c + 3) < rowWidth)
        total += lines[r + 1][c + 1] == 'M' && lines[r + 2][c + 2] == 'A' && lines[r + 3][c + 3] == 'S' ? 1 : 0;

    //down
    if ((r + 3) < columnWidth)
        total += lines[r + 1][c] == 'M' && lines[r + 2][c] == 'A' && lines[r + 3][c] == 'S' ? 1 : 0;

    //down left
    if ((r + 3) < columnWidth && (c - 3) >= 0)
        total += lines[r + 1][c - 1] == 'M' && lines[r + 2][c - 2] == 'A' && lines[r + 3][c - 3] == 'S' ? 1 : 0;

    //left
    if ((c - 3) >= 0)
        total += lines[r][c - 1] == 'M' && lines[r][c - 2] == 'A' && lines[r][c - 3] == 'S' ? 1 : 0;

    return total;
}