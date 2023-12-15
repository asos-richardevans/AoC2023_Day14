var input = File.ReadAllLines("./input.txt");
var panel = new Dictionary<(int x, int y), char>();
var rowCount = 0;
foreach (var line in input)
{
    for (var i = 0; i < line.Length; i++)
    {
        panel.Add((i, rowCount), line[i]);
    }
    rowCount++;
}
var panelWidth = panel.Max(a => a.Key.x) + 1;
var panelLength = panel.Max(a => a.Key.y) + 1;
var totalCycles = 250;
var cycles = 0;
var uniqueTotalLoad = new List<int>();
var duplicateTotalLoad = new List<(int, int)>();
var lastPatternCycle = 0;
var patternLength = 0;
//DisplayPanel();

while (cycles < totalCycles)
{
    panel.Where(x => x.Value == 'O').
        OrderBy(x => x.Key.y).
        ThenBy(x => x.Key.x).
        ToList().
        ForEach(rock => MoveRockNorth(rock.Key.x, rock.Key.y));
    //DisplayPanel();
    panel.Where(x => x.Value == 'O').
        OrderBy(x => x.Key.x).
        ThenBy(x => x.Key.y).
        ToList().
        ForEach(rock => MoveRockWest(rock.Key.x, rock.Key.y));
    //DisplayPanel();
    panel.Where(x => x.Value == 'O').
        OrderByDescending(x => x.Key.y).
        ThenByDescending(x => x.Key.x).
        ToList().
        ForEach(rock => MoveRockSouth(rock.Key.x, rock.Key.y));
    //DisplayPanel();
    panel.Where(x => x.Value == 'O').
        OrderByDescending(x => x.Key.x).
        ThenByDescending(x => x.Key.y).
        ToList().
        ForEach(rock => MoveRockEast(rock.Key.x, rock.Key.y));
    //DisplayPanel();

    var total = 0;

    panel.
        Where(x => x.Value == 'O').
        OrderBy(x => x.Key.y).
        ThenBy(x => x.Key.x).
        ToList().
        ForEach(
            rock => total += panelLength - rock.Key.y);

    if (!uniqueTotalLoad.Contains(total))
    {
        uniqueTotalLoad.Add(total);
        Console.WriteLine($"{cycles + 1} - {total}");
        duplicateTotalLoad.Clear();
        lastPatternCycle = 0;
    }
    else
    {
        duplicateTotalLoad.Add((cycles + 1, total));
        Console.WriteLine($"{cycles + 1} - {total} - DUPLICATE");
        if (CheckForPattern())
        {
            lastPatternCycle = cycles + 1;
            patternLength = duplicateTotalLoad.Count / 2;
            break;
        }
    }

    cycles++;
}

var pattern = duplicateTotalLoad.Where(x => x.Item1 >= lastPatternCycle + 1 - patternLength).Take(patternLength).ToList();

var count = 0;

for (long i = lastPatternCycle+1; i < 1000000001; i++)
{
    if (i > 999999950)
    {
        Console.WriteLine($"{i} - {pattern[count]}");
    }

    count++;
    if (count >= patternLength) { count = 0; }
}

Console.WriteLine("The 1 billionth calculation is shared in the repeating pattern of calculations! Here the pattern is projected forward.");

bool CheckForPattern()
{
    int i = duplicateTotalLoad.Count / 2;

    for (int j = 2; j <= i; j++)
    {
        var firstValues = duplicateTotalLoad.Take(j).Select(x => x.Item2);
        var secondValues = duplicateTotalLoad.Skip(j).Take(j).Select(x => x.Item2);
        if (firstValues.SequenceEqual(secondValues))
        {
            return true;
        }
    }

    return false;
}

void DisplayPanel()
{
    Console.WriteLine();
    Console.WriteLine("NEW PANEL");
    Console.WriteLine("----------------------");
    foreach (var panelItem in panel.
                 OrderBy(x => x.Key.y).
                 ThenBy(x => x.Key.x))
    {
        if (panelItem.Key.x == 0)
        {
            Console.WriteLine();
        }
        Console.Write(panel[(panelItem.Key.x, panelItem.Key.y)]);
    }
    Console.WriteLine();
}

void MoveRockNorth(int x, int y)
{
    while (y > 0 && CanMoveTo(x, y - 1))
    {
        SwapRockPositions(x, y, x, y - 1);
        y--;
    }
}

void MoveRockWest(int x, int y)
{
    while (x > 0 && CanMoveTo(x - 1, y))
    {
        SwapRockPositions(x - 1, y, x, y);
        x--;
    }
}

void MoveRockEast(int x, int y)
{
    while (x < panelWidth && CanMoveTo(x + 1, y))
    {
        SwapRockPositions(x + 1, y, x, y);
        x++;
    }
}

void MoveRockSouth(int x, int y)
{
    while (y < panelLength && CanMoveTo(x, y + 1))
    {
        SwapRockPositions(x, y, x, y + 1);
        y++;
    }
}

void SwapRockPositions(int x1, int y1, int x2, int y2)
{
    var temp = panel[(x1, y1)];
    panel[(x1, y1)] = panel[(x2, y2)];
    panel[(x2, y2)] = temp;
}

bool CanMoveTo(int x, int y)
{
    return panel.FirstOrDefault(z => z.Key.x == x && z.Key.y == y).Value == '.';
}