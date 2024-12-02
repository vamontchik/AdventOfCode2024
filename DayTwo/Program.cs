var lines = File
    .ReadAllLines(Path.GetFullPath(args[0]))
    .Select(x => x.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
    .Select(x => x.Select(int.Parse).ToList())
    .ToList();

var totalSafe = lines.Sum(line => CheckSafelyIncreasing(line) || CheckSafelyDecreasing(line) ? 1 : 0);
Console.WriteLine("There are {0} reports that are safe", totalSafe);

var totalSafeWithRemoval = lines.Sum(line =>
    CheckSafelyIncreasingWithRemoval(line) || CheckSafelyDecreasingWithRemoval(line) ? 1 : 0);
Console.WriteLine("There are {0} reports that are safe with removal", totalSafeWithRemoval);

return;

static bool CheckSafelyIncreasingWithRemoval(IReadOnlyList<int> line)
{
    var safelyIncreasing = CheckSafelyIncreasing(line);

    if (safelyIncreasing)
        return true;

    var safeInnerCount = 0;

    for (var i = 0; i < line.Count; ++i)
    {
        var duplicate = new List<int>(line);

        duplicate.RemoveAt(i);

        var innerTest = CheckSafelyIncreasing(duplicate);

        if (!innerTest)
            continue;

        safeInnerCount += 1;
    }

    return safeInnerCount > 0;
}

static bool CheckSafelyDecreasingWithRemoval(IReadOnlyList<int> line)
{
    var safelyDecreasing = CheckSafelyDecreasing(line);

    if (safelyDecreasing)
        return true;

    var safeInnerCount = 0;

    for (var i = 0; i < line.Count; ++i)
    {
        var duplicate = new List<int>(line);

        duplicate.RemoveAt(i);

        var innerTest = CheckSafelyDecreasing(duplicate);

        if (!innerTest)
            continue;

        safeInnerCount += 1;
    }

    return safeInnerCount > 0;
}

static bool CheckSafelyIncreasing(IReadOnlyList<int> report)
{
    // -1 b/c windows of size 2 but need to look at last pair... it cancels out :(
    for (var i = 0; i < report.Count - 1; i++)
    {
        var first = report[i];
        var second = report[i + 1];

        var diff = first - second;
        var absDiff = Math.Abs(first - second);

        if (diff < 0) // not increasing
            return false;

        if (absDiff is < 1 or > 3) // not safe
            return false;
    }

    return true;
}

static bool CheckSafelyDecreasing(IReadOnlyList<int> report)
{
    for (var i = 0; i < report.Count - 1; i++)
    {
        var first = report[i];
        var second = report[i + 1];

        var diff = first - second;
        var absDiff = Math.Abs(first - second);

        if (diff > 0) // not decreasing
            return false;

        if (absDiff is < 1 or > 3) // not safe
            return false;
    }

    return true;
}