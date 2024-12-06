PartOne(args[0]);
PartTwo(args[0]);

return;

static void PartTwo(string path)
{
    var fileContents = File.ReadAllLines(Path.GetFullPath(path)).ToList();

    var lineNumberOfSeparator = FindSeparator(fileContents);

    var rulesRaw = fileContents[..lineNumberOfSeparator];
    var rules = ParseRawRules(rulesRaw);

    var updates = fileContents[(lineNumberOfSeparator + 1)..];

    var middleNumberTotal = 0;
    foreach (var update in updates)
    {
        var values = update
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();

        var changed = false;
        while (!CheckRulesAndFix(values, rules))
        {
            changed = true;
        }

        if (!changed) 
            continue;
        
        var middleIndex = values.Count / 2;
        middleNumberTotal += values[middleIndex];
    }

    Console.WriteLine("The middle number total is {0}", middleNumberTotal);
}

static void PartOne(string path)
{
    var fileContents = File.ReadAllLines(Path.GetFullPath(path)).ToList();

    var lineNumberOfSeparator = FindSeparator(fileContents);

    var rulesRaw = fileContents[..lineNumberOfSeparator];
    var rules = ParseRawRules(rulesRaw);

    var updates = fileContents[(lineNumberOfSeparator + 1)..];

    var middleNumberTotal = 0;
    foreach (var update in updates)
    {
        var values = update
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();

        if (!CheckRules(values, rules))
            continue;

        var middleIndex = values.Count / 2;

        middleNumberTotal += values[middleIndex];
    }

    Console.WriteLine("The middle number total is {0}", middleNumberTotal);
}

static int FindSeparator(IEnumerable<string> lines) => lines.Count(line => line.Contains('|'));

static Dictionary<int, List<int>> ParseRawRules(IEnumerable<string> rawRules)
{
    var rules = new Dictionary<int, List<int>>();

    foreach (var line in rawRules)
    {
        var split = line.Split('|');

        var lValue = int.Parse(split[0]);
        var rValue = int.Parse(split[1]);

        if (!rules.ContainsKey(lValue))
            rules[lValue] = [];

        rules[lValue].Add(rValue);
    }

    return rules;
}

static bool CheckRules(List<int> values, Dictionary<int, List<int>> rules)
{
    foreach (var (lValue, rValues) in rules)
    {
        foreach (var rValue in rValues)
        {
            if (!values.Contains(lValue))
                continue;

            if (!values.Contains(rValue))
                continue;

            var leftIndex = values.FindIndex(value => value == lValue);
            var rightIndex = values.FindIndex(value => value == rValue);

            if (leftIndex >= rightIndex)
                return false;
        }
    }

    return true;
}

static bool CheckRulesAndFix(List<int> values, Dictionary<int, List<int>> rules)
{
    foreach (var (lValue, rValues) in rules)
    {
        foreach (var rValue in rValues)
        {
            if (!values.Contains(lValue))
                continue;

            if (!values.Contains(rValue))
                continue;

            var leftIndex = values.FindIndex(value => value == lValue);
            var rightIndex = values.FindIndex(value => value == rValue);

            if (leftIndex < rightIndex) 
                continue;
            
            (values[leftIndex], values[rightIndex]) = (values[rightIndex], values[leftIndex]);
            return false;
        }
    }

    return true;
}