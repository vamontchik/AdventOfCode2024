var firstIds = new List<int>();
var secondIds = new List<int>();

File.ReadAllLines(Path.GetFullPath(args[0]))
    .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
    .ToList()
    .ForEach(splitLine =>
    {
        var first = int.Parse(splitLine[0]);
        var second = int.Parse(splitLine[1]);
        firstIds.Add(first);
        secondIds.Add(second);
    });

firstIds.Sort();
secondIds.Sort();

var totalDistance = 0;
var frequencyBucket = new int[100000];
foreach (var (firstId, secondId) in firstIds.Zip(secondIds))
{
    var diff = Math.Abs(firstId - secondId);
    totalDistance += diff;
    
    frequencyBucket[secondId]++;
}

Console.WriteLine("The total distance is {0}", totalDistance);

var similarityScore = 0;
foreach (var (firstId, _) in firstIds.Zip(secondIds))
{
    similarityScore += firstId * frequencyBucket[firstId];
}

Console.WriteLine("The similarity score is {0}", similarityScore);