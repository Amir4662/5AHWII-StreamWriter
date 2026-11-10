var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Website aus wwwroot bereitstellen
app.UseDefaultFiles();
app.UseStaticFiles();

// Sätze für den Typewriter
string[] sentences =
{
    "Heute ist ein guter Tag, um etwas Neues zu lernen.",
    "Die Katze sitzt am Fenster und beobachtet die Vögel.",
    "Manchmal sagen kleine Dinge mehr als tausend Worte.",
    "Der schnelle Zug fährt durch die grüne Landschaft.",
    "Jeder Anfang ist eine neue Möglichkeit.",
    "Ich schreibe diesen Satz mit meiner Tastatur.",
    "Am Morgen scheint die Sonne durch mein Fenster.",
    "Das Leben besteht aus vielen kleinen Momenten.",
    "Ein gutes Buch öffnet Türen zu anderen Welten.",
    "Der Regen klopft leise gegen das Fenster."
};

// API liefert einen zufälligen Satz ohne zusätzliche Anführungszeichen
app.MapGet("/api/sentence", () =>
{
    string sentence = sentences[Random.Shared.Next(sentences.Length)];

    return Results.Text(sentence, "text/plain");
});

app.Run();