
public class Manager : Employee
{
    public int TeamSize { get; set; }

    public override void DisplayDetails()
    {
        Console.WriteLine($"[Manager] ID={Id}, Name={Name}, Age={Age}, Dept={Department}, TeamSize={TeamSize}");
    }
}
