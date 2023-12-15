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

panel.
    Where(x => x.Value == 'O').
    OrderBy(x => x.Key.y).
    ThenBy(x => x.Key.x).
    ToList().
    ForEach(
        rock => MoveRock(rock.Key.x, rock.Key.y));

var total = 0;
var panelLength = panel.Max(x => x.Key.y) + 1;
panel.
    Where(x => x.Value == 'O').
    OrderBy(x => x.Key.y).
    ThenBy(x => x.Key.x).
    ToList().
    ForEach(
        rock => total += panelLength - rock.Key.y);

Console.WriteLine(total);

void MoveRock(int x, int y)
{
    while (y > 0 && CanMoveTo(x, y - 1))
    {
        SwapRockPositions(x, y, x, y - 1);
        y--;
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