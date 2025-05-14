using System;
using System.Media;
using System.Collections.Generic;
using System.Linq;

public class CybersecurityBot
{
    private string userName = "";
    private string userInterest = "";
    private string lastTopic = "";

    // Keywords and multiple responses for variety
    private readonly Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>()
    {
        { "password", new List<string> {
            "Use strong, unique passwords and consider a password manager.",
            "Avoid using birthdays or common words in passwords.",
            "Change your passwords regularly and never reuse them." }},

        { "scam", new List<string> {
            "Scams can be tricky—don’t trust unsolicited requests for sensitive info.",
            "Be cautious when someone asks for personal details unexpectedly.",
            "Always verify emails or messages claiming to be from your bank." }},

        { "privacy", new List<string> {
            "Be mindful of what personal data you share online.",
            "Review app permissions and adjust privacy settings.",
            "Use incognito mode when browsing sensitive topics." }},

        { "phishing", new List<string> {
            "Don't click links from unknown sources.",
            "Check email addresses for typos or strange formatting.",
            "Never give out login info through email or text.",
            "Phishing messages often use urgency to trick you—be alert!" }}
    };

    private readonly string[] negativeWords = { "confused", "frustrated", "angry", "lost", "worried", "upset" };
    private readonly Random random = new Random();

    public void PlayGreeting()
    {
        try
        {
            SoundPlayer player = new SoundPlayer("C:\\Users\\RC_Student_lab\\Desktop\\Part_2\\Audio\\greeting.wav");
            player.PlaySync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error playing audio: " + ex.Message);
        }
    }

    public void DisplayAsciiLogo()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(@"(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)");
        Console.WriteLine(@"(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)");
        Console.WriteLine(@"(\o/)_________        ___.                 _________                          .__  __          (\o/)");
        Console.WriteLine(@"(/|\)\_   ___ \___.__.\_ |__   ___________/   _____/ ____   ____  __ _________|__|/  |_ ___.__.(/|\)");
        Console.WriteLine(@"(\o/)/    \  \<   |  | | __ \_/ __ \_  __ \_____  \_/ __ \_/ ___\|  |  \_  __ \  \   __<   |  |(\o/)");
        Console.WriteLine(@"(/|\)\     \___\___  | | \_\ \  ___/|  | \/        \  ___/\  \___|  |  /|  | \/  ||  |  \___  |(/|\)");
        Console.WriteLine(@"(\o/) \______  / ____| |___  /\___  >__| /_______  /\___  >\___  >____/ |__|  |__||__|  / ____|(\o/)");
        Console.WriteLine(@"(/|\)        \/\/          \/     \/             \/     \/     \/                       \/     (/|\)");
        Console.WriteLine(@"(\o/)__________        __                                                                      (\o/)");
        Console.WriteLine(@"(/|\)\______   \ _____/  |_                                                                    (/|\)");
        Console.WriteLine(@"(\o/) |    |  _//  _ \   __\                                                                   (\o/)");
        Console.WriteLine(@"(/|\) |    |   (  <_> )  |                                                                     (/|\)");
        Console.WriteLine(@"(\o/) |______  /\____/|__|                                                                     (\o/)");
        Console.WriteLine(@"(/|\)        \/                                                                                (/|\)");
        Console.WriteLine(@"(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)(\o/)");
        Console.WriteLine(@"(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)(/|\)");
        Console.ResetColor();
    }

    public void DisplayWelcomeMessage()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║               Welcome to the Cybersecurity Bot!              ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
        Console.ResetColor();
    }

    public void GreetUser()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Hello! What's your name? ");
        Console.ResetColor();

        while (true)
        {
            userName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(userName))  // Validate user name is not empty or whitespace
            {
                break;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please enter a valid name.");
                Console.ResetColor();
                Console.Write("What's your name? ");
            }
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Welcome, {userName}! I'm here to help you stay safe online.");
        Console.ResetColor();
    }

    public void StartChatbot()
    {
        Console.Clear();
        DisplayWelcomeMessage();
        DisplayAsciiLogo();
        PlayGreeting();
        GreetUser();
        BasicResponseSystem();
    }

    private void BasicResponseSystem()
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("\nYou can ask me anything related to cybersecurity, or type 'exit' to quit.");
        Console.ResetColor();

        while (true)
        {
            Console.Write($"{userName}: ");
            string input = Console.ReadLine().ToLower();

            if (input == "exit")
            {
                DisplayMessage("Goodbye! Stay safe online!", ConsoleColor.Red);
                break;
            }

            if (HandleSentiment(input)) continue;
            if (HandleInterest(input)) continue;
            if (HandleRecall(input)) continue;
            if (HandleFollowUp(input)) continue;
            if (HandleKeywordResponse(input)) continue;

            DisplayMessage("I didn't quite understand that. Could you rephrase or ask something else?", ConsoleColor.Yellow);
        }
    }

    // Modular methods for better readability and maintainability

    private bool HandleKeywordResponse(string input)
    {
        foreach (var entry in keywordResponses)
        {
            if (input.Contains(entry.Key))
            {
                string response = entry.Value[random.Next(entry.Value.Count)];
                DisplayMessage(response, ConsoleColor.DarkCyan);
                lastTopic = entry.Key;
                return true;
            }
        }
        return false;
    }

    private bool HandleSentiment(string input)
    {
        if (negativeWords.Any(word => input.Contains(word)))
        {
            DisplayMessage("It's okay to feel that way. Cybersecurity can be overwhelming, but I'm here to help.", ConsoleColor.Blue);
            return true;
        }
        return false;
    }

    private bool HandleInterest(string input)
    {
        if (input.Contains("interested in"))
        {
            int index = input.IndexOf("interested in") + 13;
            userInterest = input.Substring(index).Trim();
            Console.WriteLine($"Great! I'll remember that you're interested in {userInterest}. It's a crucial part of staying safe online.");
            lastTopic = userInterest;
            return true;
        }
        return false;
    }

    private bool HandleRecall(string input)
    {
        if (!string.IsNullOrEmpty(userInterest) && input.Contains("recommendation"))
        {
            Console.WriteLine($"Since you're into {userInterest}, you might want to review the latest best practices and secure your devices.");
            return true;
        }
        if (!string.IsNullOrEmpty(lastTopic) && input.Contains("remember"))
        {
            Console.WriteLine($"Earlier, we talked about {lastTopic}. Would you like to revisit this topic?");
            return true;
        }

        return false;
    }

    private bool HandleFollowUp(string input)
    {
        if (input.Contains("more") || input.Contains("details"))
        {
            switch (lastTopic)
            {
                case "privacy":
                    Console.WriteLine("Privacy includes managing social media visibility, disabling location tracking, and securing browser data.");
                    break;
                case "password":
                    Console.WriteLine("Consider using a password manager, and never reuse passwords across services.");
                    break;
                case "phishing":
                    Console.WriteLine("Phishing scams often use urgency. Always verify email authenticity.");
                    break;
                case "scam":
                    Console.WriteLine("Check official websites and contact customer support if in doubt about a scam.");
                    break;
                default:
                    Console.WriteLine("Can you clarify which topic you'd like to know more about?");
                    break;
            }
            return true;
        }
        return false;
    }

    private void DisplayMessage(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    // Entry point
    public static void Main(string[] args)
    {
        CybersecurityBot bot = new CybersecurityBot();
        bot.StartChatbot();
    }
}
